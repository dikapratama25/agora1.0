Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions

Imports System.Drawing

Public Class IPPEntryPop
    Inherits AgoraLegacy.AppBaseClass
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim dsDoc As New DataSet
    Dim aryDoc As New ArrayList
    Dim dsIPPDoc As New DataSet
    Dim objDB As New EAD.DBCom
    Dim objGlobal As New AppGlobals
    Dim objDoc As New IPPMain
    Dim dsIPPDocDetails As New DataSet
    Dim strMsg As String
    Dim tempStr As String
    Dim strIsGst As String
    Dim exceedCutOffDt As String = ""
    Dim compType As String = ""
    'Modified for IPP GST Stage 2A - CH - 2 Feb 2015
    Dim strDefIPPCompID As String = System.Configuration.ConfigurationManager.AppSettings("DefIPPCompID")

    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents cmdCancel As System.Web.UI.WebControls.Button
    Protected WithEvents vldsum As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lbltitle As System.Web.UI.WebControls.Label
    Protected WithEvents Image As New System.Web.UI.WebControls.Image
    Protected WithEvents hidButton As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButton0 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidButton1 As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lineNo As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hidexceedCutOffDt As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hidIsGST As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hidResidentType As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hidMode As System.Web.UI.WebControls.HiddenField
    'Zulham 27/01/2016 - IPP Stage 4 Phase 2
    Protected WithEvents hidButtonTest As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidHeaderGstAmount As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hidButtonCancel As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hidCoyType As System.Web.UI.WebControls.HiddenField
    'mimi 10/04/18 - withholding Tax
    'Protected WithEvents rdbWHTVendor As System.Web.UI.WebControls.RadioButton
    'Protected WithEvents rdbNoWHT As System.Web.UI.WebControls.RadioButton
    'Protected WithEvents txtNoWHTReason As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtWHT As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWithholding As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWithholdingOpt As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnTaxHolding As System.Web.UI.WebControls.Button
    'Zulham 30/4/3018 - PAMB
    Protected WithEvents hidGLCodeTest As System.Web.UI.WebControls.HiddenField



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


    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Session("SelectedComp_Edit") = Nothing 'Feb 12, 2014
        Session("GLCode") = Nothing
        Session("IM2") = ""
        strNewCSS = "true"
        MyBase.Page_Load(sender, e)
        Dim objDoc As New IPPMain
        Dim blnIPPOfficer As Boolean
        Dim blnIPPOfficerS As Boolean
        Dim objUsers As New Users
        If Request.QueryString("action") = "edit" Then
            lbltitle.Text = "Modify E2P Document Line"
        End If
        blnIPPOfficer = objUsers.checkUserFixedRole("'IPP Officer(F)'")
        blnIPPOfficerS = objUsers.checkUserFixedRole("'IPP Officer'")
        ViewState("IPPOfficer") = blnIPPOfficer
        ViewState("IPPOfficerS") = blnIPPOfficerS
        ViewState("role") = getUserRole(blnIPPOfficer, blnIPPOfficerS)

        'Jules 2018.07.09 - Allow "\" and "#"
        'Zulham 27/01/2016 - IPP GST Stage 4 Phase 2
        Dim headerGstAmount = objDB.GetVal("SELECT IFNULL(im_invoice_gst,0.0) 'im_invoice_gst' FROM invoice_mstr WHERE im_invoice_no = '" & Common.Parse(Server.UrlDecode(Replace(Replace(Request.QueryString("docno"), "\", "\\"), "#", "\#"))) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
        hidHeaderGstAmount.Value = headerGstAmount

        'Check for GST
        Dim gst As New GST
        compType = objDB.GetVal("SELECT IFNULL(ic_coy_type,'') 'im_doc_date' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
        'Zulham 17/02/2016
        hidCoyType.Value = compType.Trim

        'Jules 2018.07.09 - Allow "\"
        Dim documentDate = objDB.GetVal("SELECT IFNULL(im_created_on,'') 'im_doc_date' FROM invoice_mstr WHERE im_invoice_no = '" & Common.Parse(Replace(Replace(Request.QueryString("docno"), "\", "\\"), "#", "\#")) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
        Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
        Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
        'Zulham 13082015
        'Additional condition for company's effective date
        Dim effectiveDate = objDB.GetVal("SELECT IFNULL(ic_gst_eff_date, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
        'Zulham 19082015
        Dim residentType As String = ""
        'Zulham 13/10/2015 - Added condition that is coytype = 'V'
        If Request.QueryString("vencomp") IsNot Nothing Then residentType = objDB.GetVal("SELECT IFNULL(ic_resident_Type, '') FROM IPP_COMPANY WHERE ic_coy_type = 'V' and ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")

        If Trim(effectiveDate) <> "" Then
            If CDate(createdDate) >= CDate(_cutoffDate) And CDate(createdDate) >= CDate(effectiveDate) And Trim(residentType.ToUpper) <> "N" Then
                exceedCutOffDt = "Yes"
                hidexceedCutOffDt.Value = "Yes"
                If Request.QueryString("vencomp") IsNot Nothing And Not documentDate = "" Then
                    Dim GSTRegNo = objDB.GetVal("SELECT IFNULL(IC_TAX_REG_NO, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                    'Dim residentType = objDB.GetVal("SELECT IFNULL(ic_resident_Type, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                    If (GSTRegNo <> "" And CDate(documentDate) >= CDate(_cutoffDate) And CDate(documentDate) >= CDate(effectiveDate)) Or (CDate(documentDate) >= CDate(_cutoffDate) And compType = "E") Then
                        strIsGst = "Yes"
                        hidIsGST.Value = "Yes"
                    Else
                        strIsGst = "No"
                        hidIsGST.Value = "No"
                    End If
                    hidResidentType.Value = residentType
                Else
                    strIsGst = "Yes"
                    hidIsGST.Value = "Yes"
                End If
            ElseIf CDate(createdDate) >= CDate(_cutoffDate) And Trim(residentType.ToUpper) = "N" Then
                exceedCutOffDt = "Yes"
                hidexceedCutOffDt.Value = "Yes"
                If Request.QueryString("vencomp") IsNot Nothing And Not documentDate = "" Then
                    Dim GSTRegNo = objDB.GetVal("SELECT IFNULL(IC_TAX_REG_NO, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                    'Dim residentType = objDB.GetVal("SELECT IFNULL(ic_resident_Type, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                    If GSTRegNo <> "" And CDate(documentDate) >= CDate(_cutoffDate) Then
                        strIsGst = "Yes"
                        hidIsGST.Value = "Yes"
                    Else
                        strIsGst = "No"
                        hidIsGST.Value = "No"
                    End If
                    hidResidentType.Value = residentType
                Else
                    strIsGst = "Yes"
                    hidIsGST.Value = "Yes"
                End If
            Else
                strIsGst = "No"
                hidIsGST.Value = "No"
            End If
        Else
            strIsGst = "No"
            hidIsGST.Value = "No"
        End If

        'Zulham 19082015
        If CDate(createdDate) >= CDate(_cutoffDate) Then
            exceedCutOffDt = "Yes"
            hidexceedCutOffDt.Value = "Yes"
            hidResidentType.Value = residentType
        End If

        'end
        If Not Page.IsPostBack Then
            ConstructTable()
            PopulateTypeAhead()
        End If
        'ConstructTable()
        If vldsum.InnerHtml <> "" Then
            vldsum.InnerHtml = ""
        End If
    End Sub
    Sub PopulateTypeAhead()
        Dim typeahead As String
        Dim i, count As Integer
        Dim content, content2, validate As String
        Dim strCompID As String
        Dim vtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY")
        Dim gltypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=GLCode")
        Dim agtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AssetGroup")
        Dim asgtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AssetSubGroup")
        Dim catypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=CostAlloc")
        Dim rulecattypeahead As String = ""
        rulecattypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=RuleCategory")

        'Jules 2018.04.17 - PAMB Scrum 1
        Dim actypeahead1 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L1")
        Dim actypeahead2 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L2")
        Dim actypeahead3 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L3")
        Dim actypeahead4 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L4")
        Dim actypeahead5 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L5")
        'Dim actypeahead6 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L6")
        'Dim actypeahead7 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L7")
        Dim actypeahead8 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L8")
        Dim actypeahead9 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L9")
        'End modification.

        Dim venidx As String
        Dim objipp As New IPPMain
        Dim cctypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=CostCentre_wo_Branch&role=" & ViewState("role") & "")
        Dim brtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Branch&role=" & ViewState("role") & "")

        'Zulham 24/11/2015 - Changed the status from 'I' to 'A'
        'venidx = objipp.getIPPCompanyIndex(Common.Parse(Request.QueryString("vencomp")), ViewState("IPPOfficer"), "I")
        venidx = objipp.getIPPCompanyIndex(Common.Parse(Request.QueryString("vencomp")), ViewState("IPPOfficer"), "A")

        If Request.QueryString("action") = "edit" Then
            count = 1
        Else
            count = 10
        End If

        For i = 0 To count - 1
            strCompID = Request.Form("ddlPayFor" & i) '"Own Co."

            content &= "$(""#txtGLCode" & i & """).autocomplete(""" & gltypeahead & """, {" & vbCrLf &
            "width: 180," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "}).result(function(event,data,item) {" & vbCrLf &
            "if (data)" & vbCrLf &
            "$(""#hidGLCodeVal" & i & """).val(data[1]);" & vbCrLf &
            "$(""#txtGLCode" & i & """).val(data[1]);" & vbCrLf &
            "document.getElementById(""hidGLCodeTest"").value = data[1];" & vbCrLf &
            "var clickevent = $(""#hidGLCode" & i & """).val();" & vbCrLf &
            "var changeclick = updateparam(clickevent,""GLCodeSearch.aspx?"",""GLCodeSearch.aspx?id="" + encodeURIComponent($(""#txtGLCode" & i & """).val()) + ""&"");" & vbCrLf &
            "var newclick = Function(changeclick);" & vbCrLf &
            "document.getElementById(""btnGLCode" & i & """).onclick = newclick;" & vbCrLf &
            "});" & vbCrLf &
            "//$(""#txtGLCode" & i & """).blur(function() {" & vbCrLf &
            "//$.ajax({" & vbCrLf &
            "//type:""POST"",url: ""../../Common/Initial/TypeAhead.aspx?from=RuleCategory&GLCode="" + encodeURIComponent($(""#txtGLCode" & i & """).val()) + ""&""," &
            "//cache: false,success: function(data){var a = data.split('|');var dataCount = a.length;if(dataCount<3 && dataCount>1){document.getElementById(""txtRuleCategory" & i & """).readOnly=true;$(""#txtRuleCategory" & i & """).val(a[0]);$(""#hidRuleCategoryVal" & i & """).val(a[1]);}" & vbCrLf &
            "//else if(dataCount>2){" & vbCrLf &
            "//$(""#txtRuleCategory" & i & """).val("""");document.getElementById(""txtRuleCategory" & i & """).readOnly=false;" & vbCrLf &
            "//$(""#txtRuleCategory" & i & """).autocomplete(""" & rulecattypeahead & "&i=" & i & "&GLCode="" + encodeURIComponent($(""#txtGLCode" & i & """).val()) + """ & """, {" & vbCrLf &
            "//width: 180," & vbCrLf &
            "//scroll: true," & vbCrLf &
            "//selectFirst: false" & vbCrLf &
            "//}).result(function(event,data,item) {" & vbCrLf &
            "//if (data)" & vbCrLf &
            "//$(""#hidRuleCategoryVal" & i & """).val(data[1]);" & vbCrLf &
            "//});" & vbCrLf &
            "//}else{document.getElementById(""txtRuleCategory" & i & """).readOnly=true;$(""#txtRuleCategory" & i & """).val("""");$(""#hidRuleCategoryVal" & i & """).val("""");};},datatype:""text""" & vbCrLf &
            "//});" & vbCrLf &
            "//var clickevent = $(""#hidRuleCategory2" & i & """).val();" & vbCrLf &
            "//var changeclick = updateparam(clickevent,""SubDescriptionSearch.aspx?GLCode="",""SubDescriptionSearch.aspx?GLCode="" + encodeURIComponent($(""#txtGLCode" & i & """).val()) + ""&SubDescCode="" + encodeURIComponent($(""#txtRuleCategory" & i & """).val()) + ""&i="" + " & i & " +""&"");" & vbCrLf &
            "//var newclick = Function(changeclick);" & vbCrLf &
            "//document.getElementById(""btnSubDescCode" & i & """).onclick = newclick;" & vbCrLf &
            "//});" & vbCrLf &
            "//var clickevent = $(""#hidRuleCategory2" & i & """).val();" & vbCrLf &
            "//var changeclick = updateparam(clickevent,""SubDescriptionSearch.aspx?GLCode="",""SubDescriptionSearch.aspx?GLCode="" + encodeURIComponent($(""#txtGLCode" & i & """).val()) + ""&SubDescCode="" + encodeURIComponent($(""#txtRuleCategory" & i & """).val()) + ""&i="" + " & i & " +""&"");" & vbCrLf &
            "//var newclick = Function(changeclick);" & vbCrLf &
            "//document.getElementById(""btnSubDescCode" & i & """).onclick = newclick;" & vbCrLf &
            "//$(""#txtRuleCategory" & i & """).autocomplete(""" & rulecattypeahead & "&i=" & i & "&GLCode="" + encodeURIComponent($(""#txtGLCode" & i & """).val()) + ""&"", {" & vbCrLf &
            "//width: 180," & vbCrLf &
            "//scroll: true," & vbCrLf &
            "//selectFirst: false" & vbCrLf &
            "//}).result(function(event,data,item) {" & vbCrLf &
            "//if (data)" & vbCrLf &
            "//var clickevent = $(""#hidRuleCategory2" & i & """).val();" & vbCrLf &
            "//var changeclick = updateparam(clickevent,""SubDescriptionSearch.aspx?GLCode="",""SubDescriptionSearch.aspx?GLCode="" + encodeURIComponent($(""#txtGLCode" & i & """).val()) + ""&SubDescCode="" + encodeURIComponent($(""#txtRuleCategory" & i & """).val()) + ""&i="" + " & i & " +""&"");" & vbCrLf &
            "//var newclick = Function(changeclick);" & vbCrLf &
            "//document.getElementById(""btnSubDescCode" & i & """).onclick = newclick;" & vbCrLf &
            "//$(""#hidRuleCategoryVal" & i & """).val(data[1]);" & vbCrLf &
            "//});" & vbCrLf &
             "//$(""#txtBranch" & i & """).autocomplete(""" & brtypeahead & "&compid=" & strCompID & """, {" & vbCrLf &
            "//width: 180," & vbCrLf &
            "//scroll: true," & vbCrLf &
            "//selectFirst: false" & vbCrLf &
            "//}).result(function(event,data,item) {" & vbCrLf &
            "//if (data)" & vbCrLf &
            "//$(""#hidBranchVal" & i & """).val(data[1]);" & vbCrLf &
            "//$(""#txtBranch" & i & """).val(data[1]);" & vbCrLf &
            "//});" & vbCrLf &
            "//$(""#txtBranch" & i & """).blur(function() {" & vbCrLf &
            "//$.ajax({" & vbCrLf &
            "//type:""POST"",url: ""../../Common/Initial/TypeAhead.aspx?from=IPPCostCentre&branchCode="" + ""&i="" + encodeURIComponent((""" & i & """)) + ""&""," &
            "//cache: false,success: function(data){var a = data.split('|');var dataCount = a.length;if(dataCount<3 && dataCount>1){document.getElementById(""txtCC" & i & """).readOnly=true;$(""#txtCC" & i & """).val(a[0]);$(""#hidCCVal" & i & """).val(a[1]);}" & vbCrLf &
            "//else if(dataCount>2){" & vbCrLf &
            "//$(""#txtCC" & i & """).val("""");$(""#hidCCVal" & i & """).val("""");document.getElementById(""txtCC" & i & """).readOnly=false;" & vbCrLf &
            "//$(""#txtCC" & i & """).autocomplete(""" & cctypeahead & "&i=" & i & "&branchCode="" + """ & """, {" & vbCrLf &
            "//width: 180," & vbCrLf &
            "//scroll: true," & vbCrLf &
            "//selectFirst: false" & vbCrLf &
            "//}).result(function(event,data,item) {" & vbCrLf &
            "//if (data)" & vbCrLf &
            "//$(""#hidCCVal" & i & """).val(data[1]);" & vbCrLf &
            "//});" & vbCrLf &
            "//}else{document.getElementById(""txtCC" & i & """).readOnly=true;$(""#txtCC" & i & """).val("""");$(""#hidCCVal" & i & """).val("""");};},datatype:""text""" & vbCrLf &
            "//});" & vbCrLf &
            "//})" & vbCrLf &
            "$(""#txtCC" & i & """).autocomplete(""" & cctypeahead & "&i=" & i & "&branchCode="" + """ & """, {" & vbCrLf &
            "width: 180," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "}).result(function(event,data,item) {" & vbCrLf &
            "if (data)" & vbCrLf &
            "$(""#hidCCVal" & i & """).val(data[1]);" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtGLCode" & i & """).blur(function() {" & vbCrLf &
            "var hidglcodeval = document.getElementById(""hidGLCodeVal" & i & """).value;" & vbCrLf &
            "if(hidglcodeval == """")" & vbCrLf &
            "{" & vbCrLf &
            "$(""#txtGLCode" & i & """).val("""");" & vbCrLf &
            "}" & vbCrLf &
            "});" & vbCrLf &
            "//$(""#txtRuleCategory" & i & """).blur(function() {" & vbCrLf &
            "//var clickevent = $(""#hidRuleCategory2" & i & """).val();" & vbCrLf &
            "//var changeclick = updateparam(clickevent,""SubDescriptionSearch.aspx?GLCode="",""SubDescriptionSearch.aspx?GLCode="" + encodeURIComponent($(""#txtGLCode" & i & """).val()) + ""&SubDescCode="" + encodeURIComponent($(""#txtRuleCategory" & i & """).val()) + ""&i="" + " & i & " +""&"");" & vbCrLf &
            "//var newclick = Function(changeclick);" & vbCrLf &
            "//document.getElementById(""btnSubDescCode" & i & """).onclick = newclick;" & vbCrLf &
            "//var hidRuleCategoryVal = document.getElementById(""hidRuleCategoryVal" & i & """).value;" & vbCrLf &
            "//if(hidRuleCategoryVal == """")" & vbCrLf &
            "//{" & vbCrLf &
            "//$(""#txtRuleCategory" & i & """).val("""");" & vbCrLf &
            "//}" & vbCrLf &
            "//});" & vbCrLf &
            "$(""#txtAssetGroup" & i & """).blur(function() {" & vbCrLf &
            "var hidagval = document.getElementById(""hidAssetGroupVal" & i & """).value;" & vbCrLf &
            "if(hidagval == """")" & vbCrLf &
            "{" & vbCrLf &
            "$(""#txtAssetGroup" & i & """).val("""");" & vbCrLf &
            "}" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAssetSubGroup" & i & """).blur(function() {" & vbCrLf &
            "var hidasgval = document.getElementById(""hidAssetSubGroupVal" & i & """).value;" & vbCrLf &
            "if(hidasgval == """")" & vbCrLf &
            "{" & vbCrLf &
            "$(""#txtAssetSubGroup" & i & """).val("""");" & vbCrLf &
            "}" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtCC" & i & """).blur(function() {" & vbCrLf &
            "var hidccval = document.getElementById(""hidCCVal" & i & """).value;" & vbCrLf &
            "if(hidccval == """")" & vbCrLf &
            "{" & vbCrLf &
            "$(""#txtCC" & i & """).val("""");" & vbCrLf &
            "}" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtGLCode" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtGLCode" & i & """).val() == """") {" & vbCrLf &
            "$(""#hidGLCodeVal" & i & """).val(""""); }" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAssetGroup" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtAssetGroup" & i & """).val() == """") {" & vbCrLf &
            "$(""#hidAssetGroupVal" & i & """).val(""""); }" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAssetSubGroup" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtAssetSubGroup" & i & """).val() == """") {" & vbCrLf &
            "$(""#hidAssetSubGroupVal" & i & """).val(""""); }" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtBranch" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtBranch" & i & """).val() == """") {" & vbCrLf &
            "$(""#hidBranchVal" & i & """).val(""""); }" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtCC" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtCC" & i & """).val() == """") {" & vbCrLf &
            "$(""#hidCCVal" & i & """).val(""""); }" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode1_" & i & """).autocomplete(""" & actypeahead1 & """, {" & vbCrLf &
            "width :   180," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "}).result(function(event,data,item) {" & vbCrLf &
            "if (data)" & vbCrLf &
            "$(""#hidAnalysisCodeVal1_" & i & """).val(data[1]);" & vbCrLf &
            "$(""#txtAnalysisCode1_" & i & """).val(data[1]);" & vbCrLf &
            "var clickevent = $(""#hidAnalysisCode1_" & i & """).val();" & vbCrLf &
            "var changeclick = updateparam(clickevent,""AnalysisCodeSearch.aspx?"",""AnalysisCodeSearch.aspx?id="" + encodeURIComponent($(""#txtAnalysisCode1_" & i & """).val()) + ""&"");" & vbCrLf &
            "var newclick = Function(changeclick);" & vbCrLf &
            "document.getElementById(""btnAnalysisCode1_" & i & """).onclick = newclick;" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode1_" & i & """).blur(function() {" & vbCrLf &
            "var hidanalysiscodeval = document.getElementById(""hidAnalysisCodeVal1_" & i & """).value;" & vbCrLf &
            "if(hidanalysiscodeval == """")" & vbCrLf &
            "{" & vbCrLf &
            "$(""#txtAnalysisCode1_" & i & """).val("""");" & vbCrLf &
            "}" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode1_" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtAnalysisCode1_" & i & """).val() == """") {" & vbCrLf &
            "$(""#hidAnalysisCodeVal1_" & i & """).val(""""); }" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode2_" & i & """).autocomplete(""" & actypeahead2 & """, {" & vbCrLf &
            "width :   180," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "}).result(function(event,data,item) {" & vbCrLf &
            "if (data)" & vbCrLf &
            "$(""#hidAnalysisCodeVal2_" & i & """).val(data[1]);" & vbCrLf &
            "$(""#txtAnalysisCode2_" & i & """).val(data[1]);" & vbCrLf &
            "var clickevent = $(""#hidAnalysisCode2_" & i & """).val();" & vbCrLf &
            "var changeclick = updateparam(clickevent,""AnalysisCodeSearch.aspx?"",""AnalysisCodeSearch.aspx?id="" + encodeURIComponent($(""#txtAnalysisCode2_" & i & """).val()) + ""&"");" & vbCrLf &
            "var newclick = Function(changeclick);" & vbCrLf &
            "document.getElementById(""btnAnalysisCode2_" & i & """).onclick = newclick;" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode2_" & i & """).blur(function() {" & vbCrLf &
            "var hidanalysiscodeval = document.getElementById(""hidAnalysisCodeVal2_" & i & """).value;" & vbCrLf &
            "if(hidanalysiscodeval == """")" & vbCrLf &
            "{" & vbCrLf &
            "$(""#txtAnalysisCode2_" & i & """).val("""");" & vbCrLf &
            "}" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode2_" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtAnalysisCode2_" & i & """).val() == """") {" & vbCrLf &
            "$(""#hidAnalysisCodeVal2_" & i & """).val(""""); }" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode3_" & i & """).autocomplete(""" & actypeahead3 & """, {" & vbCrLf &
            "width :   180," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "}).result(function(event,data,item) {" & vbCrLf &
            "if (data)" & vbCrLf &
            "$(""#hidAnalysisCodeVal3_" & i & """).val(data[1]);" & vbCrLf &
            "$(""#txtAnalysisCode3_" & i & """).val(data[1]);" & vbCrLf &
            "var clickevent = $(""#hidAnalysisCode3_" & i & """).val();" & vbCrLf &
            "var changeclick = updateparam(clickevent,""AnalysisCodeSearch.aspx?"",""AnalysisCodeSearch.aspx?id="" + encodeURIComponent($(""#txtAnalysisCode3_" & i & """).val()) + ""&"");" & vbCrLf &
            "var newclick = Function(changeclick);" & vbCrLf &
            "document.getElementById(""btnAnalysisCode3_" & i & """).onclick = newclick;" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode3_" & i & """).blur(function() {" & vbCrLf &
            "var hidanalysiscodeval = document.getElementById(""hidAnalysisCodeVal3_" & i & """).value;" & vbCrLf &
            "if(hidanalysiscodeval == """")" & vbCrLf &
            "{" & vbCrLf &
            "$(""#txtAnalysisCode3_" & i & """).val("""");" & vbCrLf &
            "}" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode3_" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtAnalysisCode3_" & i & """).val() == """") {" & vbCrLf &
            "$(""#hidAnalysisCodeVal3_" & i & """).val(""""); }" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode4_" & i & """).autocomplete(""" & actypeahead4 & """, {" & vbCrLf &
            "width :   180," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "}).result(function(event,data,item) {" & vbCrLf &
            "if (data)" & vbCrLf &
            "$(""#hidAnalysisCodeVal4_" & i & """).val(data[1]);" & vbCrLf &
            "$(""#txtAnalysisCode4_" & i & """).val(data[1]);" & vbCrLf &
            "var clickevent = $(""#hidAnalysisCode4_" & i & """).val();" & vbCrLf &
            "var changeclick = updateparam(clickevent,""AnalysisCodeSearch.aspx?"",""AnalysisCodeSearch.aspx?id="" + encodeURIComponent($(""#txtAnalysisCode4_" & i & """).val()) + ""&"");" & vbCrLf &
            "var newclick = Function(changeclick);" & vbCrLf &
            "document.getElementById(""btnAnalysisCode4_" & i & """).onclick = newclick;" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode4_" & i & """).blur(function() {" & vbCrLf &
            "var hidanalysiscodeval = document.getElementById(""hidAnalysisCodeVal4_" & i & """).value;" & vbCrLf &
            "if(hidanalysiscodeval == """")" & vbCrLf &
            "{" & vbCrLf &
            "$(""#txtAnalysisCode4_" & i & """).val("""");" & vbCrLf &
            "}" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode4_" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtAnalysisCode4_" & i & """).val() == """") {" & vbCrLf &
            "$(""#hidAnalysisCodeVal4_" & i & """).val(""""); }" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode5_" & i & """).autocomplete(""" & actypeahead5 & """, {" & vbCrLf &
            "width :   180," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "}).result(function(event,data,item) {" & vbCrLf &
            "if (data)" & vbCrLf &
            "$(""#hidAnalysisCodeVal5_" & i & """).val(data[1]);" & vbCrLf &
            "$(""#txtAnalysisCode5_" & i & """).val(data[1]);" & vbCrLf &
            "var clickevent = $(""#hidAnalysisCode5_" & i & """).val();" & vbCrLf &
            "var changeclick = updateparam(clickevent,""AnalysisCodeSearch.aspx?"",""AnalysisCodeSearch.aspx?id="" + encodeURIComponent($(""#txtAnalysisCode5_" & i & """).val()) + ""&"");" & vbCrLf &
            "var newclick = Function(changeclick);" & vbCrLf &
            "document.getElementById(""btnAnalysisCode5_" & i & """).onclick = newclick;" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode5_" & i & """).blur(function() {" & vbCrLf &
            "var hidanalysiscodeval = document.getElementById(""hidAnalysisCodeVal5_" & i & """).value;" & vbCrLf &
            "if(hidanalysiscodeval == """")" & vbCrLf &
            "{" & vbCrLf &
            "$(""#txtAnalysisCode5_" & i & """).val("""");" & vbCrLf &
            "}" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode5_" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtAnalysisCode5_" & i & """).val() == """") {" & vbCrLf &
            "$(""#hidAnalysisCodeVal5_" & i & """).val(""""); }" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode8_" & i & """).autocomplete(""" & actypeahead8 & """, {" & vbCrLf &
            "width :   180," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "}).result(function(event,data,item) {" & vbCrLf &
            "if (data)" & vbCrLf &
            "$(""#hidAnalysisCodeVal8_" & i & """).val(data[1]);" & vbCrLf &
            "$(""#txtAnalysisCode8_" & i & """).val(data[1]);" & vbCrLf &
            "var clickevent = $(""#hidAnalysisCode8_" & i & """).val();" & vbCrLf &
            "var changeclick = updateparam(clickevent,""AnalysisCodeSearch.aspx?"",""AnalysisCodeSearch.aspx?id="" + encodeURIComponent($(""#txtAnalysisCode8_" & i & """).val()) + ""&"");" & vbCrLf &
            "var newclick = Function(changeclick);" & vbCrLf &
            "document.getElementById(""btnAnalysisCode8_" & i & """).onclick = newclick;" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode8_" & i & """).blur(function() {" & vbCrLf &
            "var hidanalysiscodeval = document.getElementById(""hidAnalysisCodeVal8_" & i & """).value;" & vbCrLf &
            "if(hidanalysiscodeval == """")" & vbCrLf &
            "{" & vbCrLf &
            "$(""#txtAnalysisCode8_" & i & """).val("""");" & vbCrLf &
            "}" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode8_" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtAnalysisCode8_" & i & """).val() == """") {" & vbCrLf &
            "$(""#hidAnalysisCodeVal8_" & i & """).val(""""); }" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode9_" & i & """).autocomplete(""" & actypeahead9 & """, {" & vbCrLf &
            "width :   180," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "}).result(function(event,data,item) {" & vbCrLf &
            "if (data)" & vbCrLf &
            "$(""#hidAnalysisCodeVal9_" & i & """).val(data[1]);" & vbCrLf &
            "$(""#txtAnalysisCode9_" & i & """).val(data[1]);" & vbCrLf &
            "var clickevent = $(""#hidAnalysisCode9_" & i & """).val();" & vbCrLf &
            "var changeclick = updateparam(clickevent,""AnalysisCodeSearch.aspx?"",""AnalysisCodeSearch.aspx?id="" + encodeURIComponent($(""#txtAnalysisCode9_" & i & """).val()) + ""&"");" & vbCrLf &
            "var newclick = Function(changeclick);" & vbCrLf &
            "document.getElementById(""btnAnalysisCode9_" & i & """).onclick = newclick;" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode9_" & i & """).blur(function() {" & vbCrLf &
            "var hidanalysiscodeval = document.getElementById(""hidAnalysisCodeVal9_" & i & """).value;" & vbCrLf &
            "if(hidanalysiscodeval == """")" & vbCrLf &
            "{" & vbCrLf &
            "$(""#txtAnalysisCode9_" & i & """).val("""");" & vbCrLf &
            "}" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtAnalysisCode9_" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtAnalysisCode9_" & i & """).val() == """") {" & vbCrLf &
            "$(""#hidAnalysisCodeVal9_" & i & """).val(""""); }" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtWithholding" & i & """).blur(function() {" & vbCrLf &
            "var hidholdingtaxval = document.getElementById(""hidHoldingTaxVal" & i & """).value;" & vbCrLf &
            "if(hidholdingtaxval == """")" & vbCrLf &
            "{" & vbCrLf &
            "$(""#txtWithholding" & i & """).val("""");" & vbCrLf &
            "}" & vbCrLf &
            "});" & vbCrLf &
            "$(""#txtWithholding" & i & """).keyup(function() {" & vbCrLf &
            "if ($(""#txtWithholding" & i & """).val() == """") {" & vbCrLf &
            "$(""#hidHoldingTaxVal" & i & """).val(""""); }" & vbCrLf &
            "});"

            ' 
            ' for edit purpose
            If Request.QueryString("action") = "edit" Then
                content2 = "var clickevent = $(""#hidGLCode" & i & """).val();" & vbCrLf &
                            "var a = document.getElementById(""hidGLCodeVal" & i & """).value; var changeclick = updateparam(clickevent,""GLCodeSearch.aspx?"",""GLCodeSearch.aspx?GLCode="" + encodeURIComponent(a) + ""&"");" & vbCrLf &
                            "var newclick = Function(changeclick);" & vbCrLf &
                            "document.getElementById(""btnGLCode" & i & """).onclick = newclick;" & vbCrLf &
                            "//var clickevent = $(""#hidCostAlloc" & i & """).val();" & vbCrLf &
                            "//var changeclick = updateparam(clickevent,""CostAllocCodeSearch.aspx?"",""CostAllocCodeSearch.aspx?CostAllocCode="" + encodeURIComponent($(""#txtCostAlloc" & i & """).val()) + ""&"");" & vbCrLf &
                            "//var newclick = Function(changeclick);" & vbCrLf &
                            "//document.getElementById(""btnCostAlloc" & i & """).onclick = newclick;" & vbCrLf &
                            "//var CostAlloc = document.getElementById(""txtCostAlloc" & i & """).value" & vbCrLf &
                            "//updatebtnCostAlloc(CostAlloc,'hidCostAlloc2" & i & "','btnCostAlloc2" & i & "','CostAllocDetail.aspx?','CostAllocDetail.aspx?VenIdx=" & venidx & "&InvLine=" & i + 1 & "&InvIdx=" & Session("DocNo") & "&CostAllocCode');" & vbCrLf &
                            "//if ($(""#txtCostAlloc" & i & """).val() != """") {" & vbCrLf &
                            "//$(""#txtBranch" & i & """).attr('disabled','disabled');" & vbCrLf &
                            "//$(""#txtCC" & i & """).attr('disabled','disabled');" & vbCrLf &
                            "//};" & vbCrLf &
                            "$(""#txtGLCode" & i & """).blur(function() {" & vbCrLf &
                            "var hidglcodeval = document.getElementById(""hidGLCodeVal" & i & """).value;" & vbCrLf &
                            "if(hidglcodeval == """")" & vbCrLf &
                            "{" & vbCrLf &
                            "$(""#txtGLCode" & i & """).val("""");" & vbCrLf &
                            "}" & vbCrLf &
                            "});" & vbCrLf &
                            "//$(""#txtCostAlloc" & i & """).blur(function() {" & vbCrLf &
                            "//var hidcaval = document.getElementById(""hidCostAllocVal" & i & """).value;" & vbCrLf &
                            "//if(hidcaval == """")" & vbCrLf &
                            "//{" & vbCrLf &
                            "//$(""#txtCostAlloc" & i & """).val("""");" & vbCrLf &
                            "//}" & vbCrLf &
                            "//});" & vbCrLf &
                             "//$(""#txtBranch" & i & """).blur(function() {" & vbCrLf &
                            "//var hidbrval = document.getElementById(""hidBranchVal" & i & """).value;" & vbCrLf &
                            "//if(hidbrval == """")" & vbCrLf &
                            "//{" & vbCrLf &
                            "//$(""#txtBranch" & i & """).val("""");" & vbCrLf &
                            "//}" & vbCrLf &
                            "//});" & vbCrLf &
                            "$(""#txtCC" & i & """).blur(function() {" & vbCrLf &
                            "var hidccval = document.getElementById(""hidCCVal" & i & """).value;" & vbCrLf &
                            "if(hidccval == """")" & vbCrLf &
                            "{" & vbCrLf &
                            "$(""#txtCC" & i & """).val("""");" & vbCrLf &
                            "}" & vbCrLf &
                            "});" & vbCrLf &
                            "$(""#txtGLCode" & i & """).keyup(function(event) {" & vbCrLf &
                             "if(event.keyCode != ""13"")" & vbCrLf &
                            "{ $(""#hidGLCodeVal" & i & """).val("""");}" & vbCrLf &
                            "if ($(""#txtGLCode" & i & """).val() == """") {" & vbCrLf &
                            "$(""#hidGLCodeVal" & i & """).val(""""); }" & vbCrLf &
                            "});" & vbCrLf &
                            "//$(""#txtCostAlloc" & i & """).keyup(function() {" & vbCrLf &
                            "//if ($(""#txtCostAlloc" & i & """).val() == """") {" & vbCrLf &
                            "//$(""#hidCostAllocVal" & i & """).val(""""); }" & vbCrLf &
                            "//});" & vbCrLf &
                           "$(""#txtBranch" & i & """).keyup(function() {" & vbCrLf &
                        "if ($(""#txtBranch" & i & """).val() == """") {" & vbCrLf &
                        "$(""#hidBranchVal" & i & """).val(""""); }" & vbCrLf &
                        "});" & vbCrLf &
                           "$(""#txtCC" & i & """).keyup(function() {" & vbCrLf &
                        "if ($(""#txtCC" & i & """).val() == """") {" & vbCrLf &
                        "$(""#hidCCVal" & i & """).val(""""); }" & vbCrLf &
                        "});" & vbCrLf

            End If
            validate = validate & "var hidglcodeval = document.getElementById(""hidGLCodeVal" & i & """).value;" & vbCrLf &
           "if(hidglcodeval == """")" & vbCrLf &
           "{" & vbCrLf &
           "$(""#txtGLCode" & i & """).val("""");" & vbCrLf &
           "}" & vbCrLf &
           "//var hidcaval = document.getElementById(""hidCostAllocVal" & i & """).value;" & vbCrLf &
           "//if(hidcaval == """")" & vbCrLf &
           "//{" & vbCrLf &
           "//$(""#txtCostAlloc" & i & """).val("""");" & vbCrLf &
            "//}" & vbCrLf &
           "//var hidbrval = document.getElementById(""hidBranchVal" & i & """).value;" & vbCrLf &
           "//if(hidbrval == """")" & vbCrLf &
           "//{" & vbCrLf &
           "//$(""#txtBranch" & i & """).val("""");" & vbCrLf &
           "//}" & vbCrLf &
           "var hidccval = document.getElementById(""hidCCVal" & i & """).value;" & vbCrLf &
           "if(hidccval == """")" & vbCrLf &
           "{" & vbCrLf &
           "$(""#txtCC" & i & """).val("""");" & vbCrLf &
           "}"


        Next
        content = content & "$(""#Form1"").submit(function() {" & vbCrLf &
        validate & vbCrLf &
        "});" & vbCrLf

        '  "}).result(function(event, item) {" & vbCrLf & _
        '"$(""#txtTemp"").focus();" & vbCrLf & _
        If Request.QueryString("action") = "edit" Then
            typeahead = "<script language=""javascript"">" & vbCrLf &
                      "<!--" & vbCrLf &
                        "$(document).ready(function(){" & vbCrLf &
                        content & vbCrLf &
                        content2 & vbCrLf &
                        "});" & vbCrLf &
                        "-->" & vbCrLf &
                        "</script>"
        Else
            typeahead = "<script language=""javascript"">" & vbCrLf &
          "<!--" & vbCrLf &
            "$(document).ready(function(){" & vbCrLf &
            content & vbCrLf &
            "});" & vbCrLf &
            "-->" & vbCrLf &
            "</script>"
        End If
        Session("typeahead") = typeahead
        Session("arydoc") = aryDoc

    End Sub
    Private Function ConstructTable()
        Dim GST As New GST
        Dim strrow, GSTAmount, strGSTPerc, strGSTID As String
        Dim i, c, count, Sno, record As Integer
        Dim table As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False
        Dim dsDoc_temp As New ArrayList
        Dim objCAD As New IPP
        Dim objipp As New IPPMain
        Dim objGST As New GST
        Dim objGlobal As New AgoraLegacy.AppGlobals

        'Jules 2018.07.10
        Dim strscript As New System.Text.StringBuilder
        strscript.Append("<script language=""javascript"">")
        Dim strLiteralScript As String = ""

        'Zulham 01/04/2016 - IM5/IM6 Enhancement
        'added variable for im5 output  
        Dim compOutputTaxValue_IM2, compOutputTaxValue_TX4, compInputTaxValue_TX4, compInputTaxValue_Block, compOutputTaxValue_IM5 As String
        Dim dsPayFor, dsUOM, dsInputTax, dsOutputTax As DataSet
        dsPayFor = objDoc.PopPayFor
        dsUOM = objDoc.PopUOM
        buildarydoc()
        'CH - Issue 8317 (Production Issue) - 1st April 2015
        compOutputTaxValue_IM2 = objDB.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT' AND IP_COY_ID = '" & Session("CompanyId") & "'")
        compOutputTaxValue_TX4 = objDB.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'GIFT_LUCKY_OUTPUT' AND IP_COY_ID = '" & Session("CompanyId") & "'")
        compInputTaxValue_TX4 = objDB.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'GIFT_LUCKY_INPUT' AND IP_COY_ID = '" & Session("CompanyId") & "'")
        compInputTaxValue_Block = objDB.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'BLOCK' AND IP_COY_ID = '" & Session("CompanyId") & "'")
        'Zulham 01/04/2016 - IM5/IM6 Enhancement
        compOutputTaxValue_IM5 = objDB.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT_IM6' AND IP_COY_ID = '" & Session("CompanyId") & "'")
        dsOutputTax = GST.GetTaxCode_forIPP("", "S")

        'Jules 2018.08.13 - To eliminate dupes due to same tax code but different category.
        'dsInputTax = GST.GetTaxCode_forIPP("", "P")
        dsInputTax = GST.GetTaxCode_forIPP("", "P", "IPP")
        'End modification.

        If Request.QueryString("action") = "edit" Then
            hidMode.Value = "edit"
            'get detail from database
            Dim ds As DataSet
            Dim venidx As String
            Sno = Request.QueryString("Lineno")
            'Zulham 24/11/2015 - Changed the status from 'I' to 'A'
            'venidx = objipp.getIPPCompanyIndex(Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))), ViewState("IPPOfficer"), "I")
            venidx = objipp.getIPPCompanyIndex(Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))), ViewState("IPPOfficer"), "A")

            ds = objipp.GetIPPDocDetails(Request.QueryString("docno"), venidx, Sno, Request.QueryString("olddocno"))
            strrow &= "<tr style=""background-color:#fdfdfd;"">"

            strrow &= "<td align=""right"" style=""display:none;"">"
            strrow &= "<input type=""hidden""  id=""txtSNo" & i & """ name=""txtSNo" & i & """ value=""" & Sno & """>"
            strrow &= "<asp:label style=""width:1%;"" class=""lbl""  id=""lblSNo" & i & """ name=""lblSNo" & i & """ value=""""/>" & Sno & ""
            strrow &= "</td>"

            strrow &= "<td>"
            'strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" onchange =""onClick();"" name=""ddlPayFor" & i & """ value="""">"
            'Zulham 29062015 - HLB-IPP Stage 4 (CR)
            'Disable payDDL if login company = HLISB
            If Common.Parse(Session("CompanyID")).ToUpper = "HLISB" Then
                strrow &= "<select class=""ddl2"" disabled=""disabled"" style=""width:75px;margin-right:0px;"" onchange =""onClick('" & i & "');"" name=""ddlPayFor" & i & """ value="""">"
            Else
                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" onchange =""onClick('" & i & "');"" name=""ddlPayFor" & i & """ value="""">"
            End If
            strrow &= "<option value=""Own Co."" selected=""selected"">" & "Own Co." & "</option>"

            'Zulham 17062015 - Fix for issue where selected payfor company reverts back to the original payfor company
            For c = 0 To dsPayFor.Tables(0).Rows.Count - 1
                If aryDoc(i)(1) Is Nothing Then
                    If ds.Tables(0).Rows(0).Item("ID_PAY_FOR") = dsPayFor.Tables(0).Rows(c).Item(0).ToString Then
                        strrow &= "<option value=""" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & "</option>"
                        Session("SelectedComp_Edit") = ds.Tables(0).Rows(0).Item("ID_PAY_FOR")
                    Else
                        strrow &= "<option value=""" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & """>" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    End If
                ElseIf aryDoc(i)(1).ToString.Trim.Length = 0 Then
                    If ds.Tables(0).Rows(0).Item("ID_PAY_FOR") = dsPayFor.Tables(0).Rows(c).Item(0).ToString Then
                        strrow &= "<option value=""" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & "</option>"
                        Session("SelectedComp_Edit") = ds.Tables(0).Rows(0).Item("ID_PAY_FOR")
                    Else
                        strrow &= "<option value=""" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & """>" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    End If
                Else
                    If aryDoc(i)(1).ToString.Trim = dsPayFor.Tables(0).Rows(c).Item(0).ToString.Trim Then
                        strrow &= "<option value=""" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & "</option>"
                        Session("SelectedComp_Edit") = aryDoc(i)(1).ToString.Trim
                    Else
                        strrow &= "<option value=""" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & """>" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    End If
                End If
            Next

            'Zulham Aug 27, 2014
            'Added column: Reimbursement
            If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                If strIsGst = "Yes" Then
                    If Not ds.Tables(0).Rows(0).Item("ID_GST_REIMB") Is DBNull.Value And aryDoc(i)(20) Is Nothing Then
                        Select Case ds.Tables(0).Rows(0).Item("ID_GST_REIMB")
                            Case "R"
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                                strrow &= "<option value=""R""  selected=""selected"">" & "Reimbursement" & "</option>"
                                strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                            Case "D"
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                strrow &= "<option value=""D""  selected=""selected"">" & "Disbursement" & "</option>"
                                strrow &= "<option value=""R"" >" & "Reimbursement" & "</option>"
                                strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                            Case "N/A"
                                'Zulham 25102018
                                If aryDoc(i)(1) Is Nothing Then
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                                    strrow &= "<option value=""R"" selected=""selected"">" & "Reimbursement" & "</option>"
                                    strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                                End If
                            Case "N"
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                strrow &= "<option value=""0"" selected=""selected"">" & "N/A" & "</option>"
                        End Select
                    ElseIf aryDoc(i)(20) IsNot Nothing Then
                        'Zulham 25102018
                        If aryDoc(i)(20) = "R" Then
                            strrow &= "<td>"
                            strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                            strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                            strrow &= "<option value=""R""  selected=""selected"">" & "Reimbursement" & "</option>"
                            strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                        ElseIf aryDoc(i)(20) = "D" Then
                            strrow &= "<td>"
                            strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                            strrow &= "<option value=""D""  selected=""selected"">" & "Disbursement" & "</option>"
                            strrow &= "<option value=""R"" >" & "Reimbursement" & "</option>"
                            strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                        End If
                    Else
                        strrow &= "<td>"
                        strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                        strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                        strrow &= "<option value=""R""  selected=""selected"">" & "Reimbursement" & "</option>"
                        strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                    End If
                Else 'Jules 2018.07.13
                    'Zulham 26102018 
                    If Not ds.Tables(0).Rows(0).Item("ID_GST_REIMB") Is DBNull.Value Then
                        If aryDoc(i)(20) Is Nothing Then
                            Select Case ds.Tables(0).Rows(0).Item("ID_GST_REIMB")
                                Case "R"
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                                    strrow &= "<option value=""R""  selected=""selected"">" & "Reimbursement" & "</option>"
                                    strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                                Case "D"
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""D""  selected=""selected"">" & "Disbursement" & "</option>"
                                    strrow &= "<option value=""R"" >" & "Reimbursement" & "</option>"
                                    strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                                Case "N/A"
                                    'Zulham 25102018
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                                    strrow &= "<option value=""R""  selected=""selected"">" & "Reimbursement" & "</option>"
                                    strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                                Case "N"
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""0"" selected=""selected"">" & "N/A" & "</option>"
                            End Select
                        Else
                            Select Case aryDoc(i)(20).ToString.Trim
                                Case "R"
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                                    strrow &= "<option value=""R""  selected=""selected"">" & "Reimbursement" & "</option>"
                                    strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                                Case "D"
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""D""  selected=""selected"">" & "Disbursement" & "</option>"
                                    strrow &= "<option value=""R"" >" & "Reimbursement" & "</option>"
                                    strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                                Case "N/A"
                                    'Zulham 25102018
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                                    strrow &= "<option value=""R""  selected=""selected"">" & "Reimbursement" & "</option>"
                                    strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                                Case "N"
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""0"" selected=""selected"">" & "N/A" & "</option>"
                            End Select
                        End If
                    Else
                        strrow &= "<td>"
                        strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value=""""  disabled=""disabled"">"
                        strrow &= "<option value=""0"" selected=""selected"">" & "N/A" & "</option>"
                    End If
                End If
            Else 'Jules 2018.07.13
                strrow &= "<td>"
                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                strrow &= "<option value=""R""  selected=""selected"">" & "Reimbursement" & "</option>"
                strrow &= "<option value=""0"" >" & "N/A" & "</option>"
            End If
            'End
            strrow &= "</td>" 'Jules 2018.07.13 

            If Request.QueryString("doctype") <> "Invoice" And Request.QueryString("doctype") <> "Bill" And Request.QueryString("doctype") <> "Letter" Then
                strrow &= "<td >"
                strrow &= "<input style=""width:105px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtInvNo" & i & """ name=""txtInvNo" & i & """ value=""" & ds.Tables(0).Rows(0).Item("ID_REF_NO") & """>"
                strrow &= "</td>"
            End If

            If aryDoc(i)(3) Is Nothing Then
                strrow &= "<td >"
                strrow &= "<input style=""width:145px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtDesc" & i & """ name=""txtDesc" & i & """ value=""" & ds.Tables(0).Rows(0).Item("ID_PRODUCT_DESC") & """>"
                strrow &= "</td>"
            Else
                strrow &= "<td >"
                strrow &= "<input style=""width:145px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtDesc" & i & """ name=""txtDesc" & i & """ value=""" & aryDoc(i)(3) & """>"
                strrow &= "</td>"
            End If

            'ZULHAM 24092018 - PAMB
            'UAT U00007
            strrow &= "<td style=""display:none;"">"
            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlUOM" & i & """ name=""ddlUOM" & i & """>"
            strrow &= "<option value=""Unit"" selected=""selected"">" & "Unit" & "</option>"

            For c = 0 To dsUOM.Tables(0).Rows.Count - 1
                If aryDoc(i)(4) Is Nothing Then
                    If ds.Tables(0).Rows(0).Item("ID_UOM") = dsUOM.Tables(0).Rows(c).Item(0).ToString Then
                        strrow &= "<option value=""" & dsUOM.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsUOM.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    Else
                        strrow &= "<option value=""" & dsUOM.Tables(0).Rows(c).Item(0).ToString & """>" & dsUOM.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    End If
                Else
                    If aryDoc(i)(4) = dsUOM.Tables(0).Rows(c).Item(0).ToString Then
                        strrow &= "<option value=""" & dsUOM.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsUOM.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    Else
                        strrow &= "<option value=""" & dsUOM.Tables(0).Rows(c).Item(0).ToString & """>" & dsUOM.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    End If
                End If
            Next
            'ZULHAM 24092018 - PAMB
            'UAT - U00007
            If aryDoc(i)(5) Is Nothing Then
                strrow &= "<td style=""display:none;"" align=""right"">"
                strrow &= "<span class=""qty""><input style=""width:100px;margin-right:0px;"" class=""numerictxtbox"" onkeypress=""return isDecimalKey(event);"" id=""txtQty" & i & """ onfocus = ""return focusControl('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" name=""txtQty" & i & """ value=""" & ds.Tables(0).Rows(0).Item("ID_RECEIVED_QTY") & """></span>"
                strrow &= "</td>"
            Else
                strrow &= "<td style=""display:none;"" align=""right"">"
                strrow &= "<span class=""qty""><input style=""width:100px;margin-right:0px;"" class=""numerictxtbox"" onkeypress=""return isDecimalKey(event);"" id=""txtQty" & i & """ onfocus = ""return focusControl('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" name=""txtQty" & i & """ value=""" & aryDoc(i)(5) & """></span>"
                strrow &= "</td>"
            End If

            If aryDoc(i)(6) Is Nothing Then
                strrow &= "<td style=""display:none;"" align=""right"">"
                strrow &= "<span class=""unit""><input style=""width:100px;margin-right:0px;"" class=""numerictxtbox""  onkeypress=""return isDecimalKey(event);"" id=""txtUnitPrice" & i & """ onfocus = ""return focusControl('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" name=""txtUnitPrice" & i & """ value=""" & Format(ds.Tables(0).Rows(0).Item("ID_UNIT_COST"), "#.00") & """></span>"
                strrow &= "</td>"
            Else
                strrow &= "<td style=""display:none;"" align=""right"">"
                strrow &= "<span class=""unit""><input style=""width:100px;margin-right:0px;"" class=""numerictxtbox""  onkeypress=""return isDecimalKey(event);"" id=""txtUnitPrice" & i & """ onfocus = ""return focusControl('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" name=""txtUnitPrice" & i & """ value=""" & aryDoc(i)(6) & """></span>"
                strrow &= "</td>"
            End If

            'Zulham 11102018 - PAMB SST
            If aryDoc(i)(7) Is Nothing Then
                strrow &= "<td align=""right"">"
                strrow &= "<span class=""amount""><input style=""width:100px;margin-right:0px; "" onblur=""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');""  class=""numerictxtbox"" id=""txtAmt" & i & """ name=""txtAmt" & i & """ value=""" & CDbl(ds.Tables(0).Rows(0).Item("ID_AMOUNT")) & """></span>"
                'strrow &= "<asp:label style=""width:100%;margin-right:0px; "" class=""lbl"" type=""text""  id=""lblAmt" & i & """ name=""lblAmt" & i & """ value=""" & aryDoc(i)(7) & """>"
                strrow &= "</td>"
            Else
                strrow &= "<td align=""right"">"
                strrow &= "<span class=""amount""><input style=""width:100px;margin-right:0px; "" onblur=""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');""  class=""numerictxtbox"" id=""txtAmt" & i & """ name=""txtAmt" & i & """ value=""" & CDbl(aryDoc(i)(7)) & """></span>"
                strrow &= "</td>"
            End If

            'Zulham 29092015 - Set aryDoc(i)(1) to Own Comp if it's HLISB
            If Session("CompanyId").ToString.ToUpper = "HLISB" And aryDoc(i)(1) Is Nothing And aryDoc(i)(22) IsNot Nothing Then
                aryDoc(i)(1) = "Own Co."
            End If

            'Zulham Aug 28, 2014
            'Added columns : GST Amount, Input Tax Code, Output Tax Code
            If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                If strIsGst = "Yes" Or compType = "E" Then
                    If Not Request.QueryString("isResident") Is Nothing Then
                        If Request.QueryString("isResident") = True Then
                            If aryDoc(i)(20) IsNot Nothing Then
                                If aryDoc(i)(20) = "R" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Reimbursement
                                    'Zulham 25092018 - PAMB SST
                                    'GST Amount
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"

                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")

                                    'Zulham 26/04/2018 - PAMB
                                    'Zulham 06/05/2018 - PAMB
                                    'IF POB Foreign Inter-Co & Category = Life then TXRE
                                    If Not aryDoc(i)(1) Is Nothing And Not aryDoc(i)(25) Is Nothing Then
                                        If Not aryDoc(i)(1) = "" And Not aryDoc(i)(25) = "" Then
                                            'Zulham 02102018 - PAMB SST
											'Dim strCountry = objDB.GetVal("SELECT ic_country FROM ipp_company WHERE ic_other_b_coy_code = '" & aryDoc(i)(1).ToString.Trim & "' and ic_coy_id ='" & Session("CompanyId") & "'")
                                            'If aryDoc(i)(25) = "Life" And strCountry <> "MY" Then
                                            '    For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            '        If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = "TXRE" Then
                                            '            tempStr = "TXRE"
                                            '            'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '            'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                            '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                            '            strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            'End modification.
                                            '        Else
                                            '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                            '        End If
                                            '    Next
                                            'Else
                                                'Jules 2018.08.13
                                                'If aryDoc(i)(22).ToString = "Select" Then
                                                '    strrow &= "<option value=""Select"">---Select---</option>"
                                                'End If
                                                If aryDoc(i)(22).ToString = "Select" OrElse aryDoc(i)(22).ToString = "TXRE" Then
                                                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                    strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                End If
                                                'End modification.
                                                For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                Next
                                            'End If
                                        End If
                                    End If
                                    'End

                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"

                                ElseIf aryDoc(i)(20) = "D" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Disbursement
                                    If Not aryDoc(i)(1).ToString.ToUpper = "HLISB" Then
                                        'Gst Amount
                                        'Zulham 25102018 - PAMB SST
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ ></span>"
                                        strrow &= "</td>"
                                        'Input TaxS
                                        'strrow &= "<td>"
                                        'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """  disabled=""disabled"" onchange =""onClick('" & i & "');"">"

                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                        'dsInputTax = GST.GetTaxCode_forIPP("", "P")

                                        'Zulham 26/04/2018 - PAMB
                                        'Zulham 06/05/2018 - PAMB
                                        'IF POB Foreign Inter-Co & Category = Life then TXRE
                                        If Not aryDoc(i)(1) Is Nothing And Not aryDoc(i)(25) Is Nothing Then
                                            If Not aryDoc(i)(1) = "" And Not aryDoc(i)(25) = "" Then
                                                'Zulham 02102018 - PAMB SST
                                                'Dim strCountry = objDB.GetVal("SELECT ic_country FROM ipp_company WHERE ic_other_b_coy_code = '" & aryDoc(i)(1).ToString.Trim & "' and ic_coy_id ='" & Session("CompanyId") & "'")
                                                'If aryDoc(i)(25) = "Life" And strCountry <> "MY" Then
                                                '    For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                '        If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = "TXRE" Then
                                                '            tempStr = "TXRE"
                                                '            'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                                '            'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                '            strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                '            'End modification.
                                                '        Else
                                                '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                '        End If
                                                '    Next
                                                'Else
                                                'Jules 2018.08.13
                                                'If aryDoc(i)(22).ToString = "Select" Then
                                                '    strrow &= "<option value=""Select"">---Select---</option>"
                                                'End If
                                                'Zulham 25102018
                                                'If aryDoc(i)(22).ToString = "Select" OrElse aryDoc(i)(22).ToString = "TXRE" Then
                                                '    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                '    strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                'End If
                                                'End modification.
                                                For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    Next
                                                'End If
                                            End If
                                        End If
                                        'End

                                        'Output Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                    Else

                                        'Start
                                        ''GST Amount
                                        If Not aryDoc(i)(22) Is Nothing Then '
                                            If aryDoc(i)(22) = "IM2" Then
                                                GSTAmount = ""
                                                ViewState("IM2") = "Yes"
                                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                                For Each row As DataRow In strPerc
                                                    strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                                Next
                                                If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                    GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                    'Zulham 14042015 IPP GST Stage 1
                                                    GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                                End If
                                                'Zulham 25092018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                                GSTAmount = ""
                                                ViewState("Prize") = "Yes"
                                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                                For Each row As DataRow In strPerc
                                                    strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                                Next
                                                If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                    GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                    'Zulham 14042015 IPP GST Stage 1
                                                    GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                                End If
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            Else
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If

                                        'Predefined tax codes will be default value
                                        Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")
                                        Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")

                                        'Input Tax
                                        If aryDoc(i)(22) Is Nothing Then
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            'If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                            strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                                                    tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        Else
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            If aryDoc(i)(22).ToString = "Select" Then
                                                strrow &= "<option value=""Select"">---Select---</option>"
                                            End If
                                            For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                                    tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        End If

                                        'Output Tax
                                        If aryDoc(i)(23) Is Nothing Then
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                strrow &= "<td>"
                                                'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                If Not aryDoc(i)(22) Is Nothing Then
                                                    If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    Else
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    End If
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                If Not aryDoc(i)(23) Is Nothing Then
                                                    If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        strrow &= "<option value=""Select"">---Select---</option>"
                                                    End If
                                                    If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                                tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                                strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    Else
                                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                    End If
                                                Else
                                                    'If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    '    strrow &= "<option value=""Select"">---Select---</option>"
                                                    'End If
                                                    'strrow &= "<option value=""Select"">---Select---</option>"
                                                    If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        'For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        '    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        'Next
                                                        'strrow &= "<td>"
                                                        'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                        'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                        If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                                tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                                strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                            ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    Else
                                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                    End If
                                                End If
                                            Else
                                                If Not aryDoc(i)(23) Is Nothing Then
                                                    strrow &= "<td>"
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                    If aryDoc(i)(23).ToString = "Select" Then
                                                        strrow &= "<option value=""Select"">---Select---</option>"
                                                    End If
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                Else
                                                    strrow &= "<td>"
                                                    If VenOutputTaxValue.ToString.Contains("N/A") Then
                                                        strrow &= "<select class=""ddl2"" disabled=""disabled"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    Else
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    End If
                                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                    If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                        ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                End If
                                            End If
                                        Else
                                            strrow &= "<td>"
                                            'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                strrow &= "<option value=""Select"">---Select---</option>"
                                            End If
                                            If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "IM2" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = compInputTaxValue_TX4 Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                        tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM2 Then
                                                        strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            ElseIf aryDoc(i)(22).ToString = compInputTaxValue_TX4 Then
                                                For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                                        strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            Else
                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            End If
                                        End If
                                        'End

                                    End If
                                ElseIf aryDoc(i)(1) = "Own Co." Then
                                    'GST Amount
                                    'Zulham 04102018 - PAMB UAT
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'strrow &= "<option value=""Select"">---Select---</option>" 'Jules 2018.08.10 commented.

                                    'Zulham 27/04/2018 - PAMB
                                    'Zulham 06/05/2018 - PAMB
                                    If Not aryDoc(i)(25) Is Nothing OrElse aryDoc(i)(8) IsNot Nothing OrElse aryDoc(i)(37) IsNot Nothing Then
                                        'Zulham 03102018 - PAMB SST
										'Dim strGLType = ""
                                        'strGLType = objDB.GetVal("SELECT IFNULL(cbg_b_gl_type,"""") 'cbg_b_gl_type' FROM company_b_gl_code  WHERE cbg_b_gl_code = '" & aryDoc(i)(8).ToString.Split(":")(0).ToString & "'")

                                        Dim blnReset As Boolean = False 'Jules 2018.08.10
                                        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            'If strGLType = "CAP" Then
                                            '    ViewState("isCAPEX" & i) = True
                                            '    Select Case aryDoc(i)(25).ToString
                                            '        Case "Life"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "TXC1" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXC1" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '        Case "Non-Life"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "TXC2" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXC2" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '        Case "Mixed"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "TXCG" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXCG" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '    End Select
                                            'ElseIf strGLType = "BLC" Then
                                            '    ViewState("isCAPEX" & i) = False
                                            '    Select Case aryDoc(i)(25).ToString
                                            '        Case "Life"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "BK" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "BK" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '        Case Else
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            End If
                                            '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            'End modification.
                                            '    End Select
                                            'Else
                                            '    ViewState("isCAPEX" & i) = False
                                            '    If aryDoc(i)(37).ToString = "Yes" And aryDoc(i)(25).ToString = "Life" Then
                                            '        'Jules 2018.08.10 - To reset the tax code.
                                            '        If record = 0 Then
                                            '            If aryDoc(i)(22).ToString.Trim = "FG" Then
                                            '                strrow &= "<option value=""Select"">---Select---</option>"
                                            '            Else
                                            '                strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '            End If
                                            '        End If
                                            '        'End modification.
                                            '        If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "FG" Then
                                            '            'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '            'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '        Else
                                            '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '        End If
                                            '    Else
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If Not aryDoc(i)(22).ToString.Trim = "" Then
                                                    'Jules 2018.08.10 - To reset the tax code.                                                                
                                                    If record = 0 Then
                                                        If aryDoc(i)(22).ToString.Trim <> "TXC1" AndAlso aryDoc(i)(22).ToString.Trim <> "TXC2" AndAlso aryDoc(i)(22).ToString.Trim <> "TXCG" AndAlso aryDoc(i)(22).ToString.Trim <> "BK" AndAlso
                                                            aryDoc(i)(22).ToString.Trim <> "FG" AndAlso aryDoc(i)(22).ToString.Trim <> "TXRE" AndAlso aryDoc(i)(22).ToString.Trim <> "Select" Then
                                                            strrow &= "<option value=""Select"">---Select---</option>"
                                                        Else
                                                            strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                            blnReset = True
                                                            strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                        End If

                                                    End If
                                                    'End modification.
                                                    If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                                        'Jules 2018.08.10
                                                        If blnReset Then
                                                            strrow &= "<option value=""" & aryDoc(i)(22).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                        'End modification.                                                                
                                                    Else
                                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Else
                                                    'Jules 2018.08.10 - To reset the tax code.
                                                    If record = 0 Then
                                                        strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                        strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                    End If
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    'End modification.                                                            
                                                End If
                                                'Zulham 25102018
                                            Else
                                                If record = 0 Then
                                                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                    strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                End If
                                                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            End If
                                            'End If
                                            'End If
                                        Next
                                    Else
                                        strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Next
                                    End If

                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                        If Not aryDoc(i)(25) Is Nothing AndAlso aryDoc(i)(37) IsNot Nothing Then
                                            If aryDoc(i)(37).ToString = "Yes" And aryDoc(i)(25).ToString = "Life" Then
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "SR-G" Then
                                                    strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & "SR-G" & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                End If
                                            Else
                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            End If
                                        Else
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        End If
                                    Next
                                    'End
                                    'Zulham 27112018
                                    If record = 0 Then strrow &= "<option value=""" & 0 & """>N/A</option>"
                                End If
                            Else
                                If aryDoc(i)(20) = "R" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Reimbursement
                                    'GST Amount
                                    'Zulham 04102018 - PAMB UAT
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    strrow &= "<option value=""Select"">---Select---</option>"
                                    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Next
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                ElseIf aryDoc(i)(20) = "D" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Disbursement
                                    'Gst Amount
                                    'Zulham 04102018 - PAMB UAT
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                                    strrow &= "</td>"
                                    'Input TaxS
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """  disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                ElseIf aryDoc(i)(1) = "Own Co." Then

                                    'GST Amount
                                    If Not aryDoc(i)(22) Is Nothing Then
                                        If aryDoc(i)(22) = "IM2" Then
                                            GSTAmount = ""
                                            ViewState("IM2") = "Yes"
                                            ViewState("Row") = i
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                'Zulham 14042015 IPP GST Stage 1
                                                GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                            End If
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                            GSTAmount = ""
                                            ViewState("Prize") = "Yes"
                                            ViewState("Row") = i
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE ='" & compInputTaxValue_TX4 & "'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                'Zulham 14042015 IPP GST Stage 1
                                                GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                            End If
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        Else
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        'Zulham 04102018 - PAMB SST
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If

                                    'If Not aryDoc(i)(22) Is Nothing Then
                                    '    'Input Tax
                                    '    strrow &= "<td>"
                                    '    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    '    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    '    If aryDoc(i)(22).ToString = "Select" Then
                                    '        strrow &= "<option value=""Select"">---Select---</option>"
                                    '    End If
                                    '    For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                    '        If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                    '            tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                    '            strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                    '        Else
                                    '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                    '        End If
                                    '    Next
                                    'Else
                                    '    'if aryDoc(i)(22) Is Nothing, originally no tax code was selected which means it's n/a 
                                    '    'strrow &= "<option value=""Select"">---Select---</option>"
                                    '    'For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                    '    '    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                    '    'Next
                                    '    'Input TaxS
                                    '    strrow &= "<td>"
                                    '    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """  disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                    '    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                    'End If

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    'strrow &= "<option value=""Select"">---Select---</option>" 'Jules 2018.08.10 commented.

                                    'Zulham 27/04/2018 - PAMB
                                    'Zulham 06/05/2018 - PAMB
                                    If Not aryDoc(i)(25) Is Nothing OrElse aryDoc(i)(8) IsNot Nothing OrElse aryDoc(i)(37) IsNot Nothing Then
                                        'Zulham 09102018 - PAMB SST
                                        'Dim strGLType = ""
                                        'strGLType = objDB.GetVal("SELECT IFNULL(cbg_b_gl_type,"""") 'cbg_b_gl_type' FROM company_b_gl_code  WHERE cbg_b_gl_code = '" & aryDoc(i)(8).ToString.Split(":")(0).ToString & "'")

                                        Dim blnReset As Boolean = False 'Jules 2018.08.10
                                        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            'If strGLType = "CAP" Then
                                            '    ViewState("isCAPEX" & i) = True
                                            '    Select Case aryDoc(i)(25).ToString
                                            '        Case "Life"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "TXC1" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXC1" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '        Case "Non-Life"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "TXC2" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXC2" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '        Case "Mixed"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "TXCG" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXCG" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '    End Select
                                            'ElseIf strGLType = "BLC" Then
                                            '    ViewState("isCAPEX" & i) = False
                                            '    Select Case aryDoc(i)(25).ToString
                                            '        Case "Life"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "BK" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "BK" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '        Case Else
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            End If
                                            '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            'End modification.
                                            '    End Select
                                            'Else
                                            'ViewState("isCAPEX" & i) = False
                                            'If aryDoc(i)(37).ToString = "Yes" And aryDoc(i)(25).ToString = "Life" Then
                                            '    'Jules 2018.08.10 - To reset the tax code.
                                            '    If record = 0 Then
                                            '        If aryDoc(i)(22).ToString.Trim = "FG" Then
                                            '            strrow &= "<option value=""Select"">---Select---</option>"
                                            '        Else
                                            '            strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '        End If
                                            '    End If
                                            '    'End modification.
                                            '    If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "FG" Then
                                            '        'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '        'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '        strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '    Else
                                            '        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '    End If
                                            'Else
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If Not aryDoc(i)(22).ToString.Trim = "" Then
                                                    'Jules 2018.08.10 - To reset the tax code.                                                                
                                                    If record = 0 Then
                                                        If aryDoc(i)(22).ToString.Trim <> "TXC1" AndAlso aryDoc(i)(22).ToString.Trim <> "TXC2" AndAlso aryDoc(i)(22).ToString.Trim <> "TXCG" AndAlso aryDoc(i)(22).ToString.Trim <> "BK" AndAlso
                                                                    aryDoc(i)(22).ToString.Trim <> "FG" AndAlso aryDoc(i)(22).ToString.Trim <> "TXRE" AndAlso aryDoc(i)(22).ToString.Trim <> "Select" Then
                                                            strrow &= "<option value=""Select"">---Select---</option>"
                                                        Else
                                                            strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                            blnReset = True
                                                            strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                        End If

                                                    End If
                                                    'End modification.
                                                    If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                                        'Jules 2018.08.10
                                                        If blnReset Then
                                                            strrow &= "<option value=""" & aryDoc(i)(22).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                        'End modification.                                                                
                                                    Else
                                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Else
                                                    'Jules 2018.08.10 - To reset the tax code.
                                                    If record = 0 Then
                                                        strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                        strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                    End If
                                                    'End modification.
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            End If
                                            'End If
                                            'End If
                                        Next
                                    Else
                                        strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Next
                                    End If


                                    '----
                                    'Output Tax
                                    If aryDoc(i)(23) Is Nothing Then
                                        If Not aryDoc(i)(22) Is Nothing Then
                                            strrow &= "<td>"
                                            'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" _
                                                Or Request.QueryString("isResident") Or aryDoc(i)(22).ToString = compInputTaxValue_TX4 Or aryDoc(i)(22).ToString = "IM2" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            If Not aryDoc(i)(23) Is Nothing Then
                                                If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Or Request.QueryString("isResident")) Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                If Not (Request.QueryString("isResident") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                Else
                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                End If
                                            Else
                                                If Not (Request.QueryString("isResident") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    If Not aryDoc(i)(22).ToString = compInputTaxValue_TX4 Then
                                                        strrow &= "<option value=""Select"">---Select---</option>"
                                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        Next
                                                    Else
                                                        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                                                strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    End If
                                                Else
                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                End If
                                            End If
                                        Else
                                            If Not aryDoc(i)(23) Is Nothing Then
                                                strrow &= "<td>"
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                If aryDoc(i)(23).ToString = "Select" Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                        tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            Else
                                                'if aryDoc(i)(23) Is Nothing, then originally it's N/A
                                                'strrow &= "<td>"
                                                'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                'strrow &= "<option value=""Select"">---Select---</option>"
                                                'For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                '    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                'Next
                                                'Output Tax
                                                strrow &= "<td>"
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            End If
                                        End If
                                    Else
                                        strrow &= "<td>"
                                        'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                        If Not aryDoc(i)(22) Is Nothing Then
                                            'Zulham 30/03/2016 - IM5/im6 Enhancemnet
                                            'Added IM5 to the condition
                                            If aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Or aryDoc(i)(22).ToString = "IM5" Then
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            ElseIf aryDoc(i)(22).ToString = "IM2" Or aryDoc(i)(22).ToString = compInputTaxValue_TX4 Then
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                        Else
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                        End If
                                        'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                        If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                            strrow &= "<option value=""Select"">---Select---</option>"
                                        End If
                                        If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "IM2" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = compInputTaxValue_TX4 Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                            For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                    tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                End If
                                            Next '
                                        ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM2 Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        ElseIf aryDoc(i)(22).ToString = compInputTaxValue_TX4 Then
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        Else
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        End If
                                    End If
                                    '----
                                ElseIf aryDoc(i)(1) = Nothing Then 'Initially

                                    If Not ds.Tables(0).Rows(0).Item("ID_GST_VALUE") Is DBNull.Value Then
                                        'GST Amount
                                        If ds.Tables(0).Rows(0).Item("ID_GST_REIMB") = "D" And Not ds.Tables(0).Rows(0).Item("ID_PAY_FOR").ToString.ToUpper = "HLISB" Then
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input disabled=""disabled""  style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        Else
                                            If ds.Tables(0).Rows(0).Item("ID_GST_VALUE").ToString.Substring(0, 1).ToString = "0" Then
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & CDec(ds.Tables(0).Rows(0).Item("ID_GST_VALUE")) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            Else
                                                'Zulham 05/04/2016 - IM5/IM6 Enhancement
                                                If Not ds.Tables(0).Rows(0).Item("ID_GST_INPUT_TAX_CODE") Is DBNull.Value Then
                                                    If ds.Tables(0).Rows(0).Item("ID_GST_INPUT_TAX_CODE").ToString.Trim = "IM5" Then
                                                        'Zulham 04102018 - PAMB SST
                                                        strrow &= "<td align=""right"">"
                                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & Format(ds.Tables(0).Rows(0).Item("ID_GST_VALUE"), "#.00") & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');"" ></span>"
                                                        strrow &= "</td>"
                                                    Else
                                                        'Zulham 04102018 - PAMB SST
                                                        strrow &= "<td align=""right"">"
                                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & Format(ds.Tables(0).Rows(0).Item("ID_GST_VALUE"), "#.00") & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                        strrow &= "</td>"
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Else
                                        'GST Amount
                                        'Zulham 04102018 - PAMB SST
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If

                                    'Input Tax
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")

                                    If Not ds.Tables(0).Rows(0).Item("ID_GST_INPUT_TAX_CODE") Is DBNull.Value Then
                                        If ds.Tables(0).Rows(0).Item("ID_GST_REIMB") = "D" And Not ds.Tables(0).Rows(0).Item("ID_PAY_FOR").ToString.ToUpper = "HLISB" Then
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            strrow &= "<option value=""" & 0 & """>" & "N/A" & "</option>"
                                        ElseIf ds.Tables(0).Rows(0).Item("ID_GST_INPUT_TAX_CODE").ToString.Trim = "" Then
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """  disabled=""disabled"" >"
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        Else
                                            'Zulham 11102018 - PAMB
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If ds.Tables(0).Rows(0).Item("ID_GST_INPUT_TAX_CODE").ToString.Trim = dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        End If
                                    Else
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    End If


                                    'Output Tax
                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                    If Not ds.Tables(0).Rows(0).Item("ID_GST_outPUT_TAX_CODE") Is DBNull.Value Then
                                        If ds.Tables(0).Rows(0).Item("ID_GST_REIMB") = "D" And Not ds.Tables(0).Rows(0).Item("ID_PAY_FOR").ToString.ToUpper = "HLISB" Then
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        ElseIf ds.Tables(0).Rows(0).Item("ID_GST_outPUT_TAX_CODE") = "" Or ds.Tables(0).Rows(0).Item("ID_GST_outPUT_TAX_CODE") = "N/A" Then
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        Else '
                                            strrow &= "<td>"
                                            If Not ds.Tables(0).Rows(0).Item("ID_GST_INPUT_TAX_CODE") Is Nothing Then
                                                'Zulham 29/03/2016 - IM5/IM6 Enhancement
                                                'Zulham 07052015 IPP GST Stage 1
                                                'TX4 Change Request
                                                If ds.Tables(0).Rows(0).Item("ID_GST_INPUT_TAX_CODE") = "IM2" Or ds.Tables(0).Rows(0).Item("ID_GST_INPUT_TAX_CODE").ToString.Contains(compInputTaxValue_TX4) Or ds.Tables(0).Rows(0).Item("ID_GST_INPUT_TAX_CODE") = "IM5" Then
                                                    strrow &= "<select class=""ddl2"" disabled=""disabled"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If ds.Tables(0).Rows(0).Item("ID_GST_outPUT_TAX_CODE").ToString.Trim = dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        End If
                                    Else
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    End If

                                Else 'PoB selected
                                    'GST Amount
                                    'Zulham 25102018 - PAMB SST
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""0.00"" onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    strrow &= "<option value = ""Select"">---Select---</option>"
                                    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        If ds.Tables(0).Rows(0).Item("ID_GST_INPUT_TAX_CODE").ToString.Trim = dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim Then
                                            strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        End If
                                    Next
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                End If

                            End If
                        Else 'Non-Resident
                            If aryDoc(i)(20) = "R" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Reimbursement
                                If aryDoc(i)(22) = "IM1" Or aryDoc(i)(22) = "IM3" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                    'GST Amount
                                    'strrow &= "<td align=""right"">"
                                    'strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    'strrow &= "</td>"
                                    If ViewState("IM2") IsNot Nothing Then
                                        If ViewState("IM2") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                            If i = ViewState("Row") Then
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                                ViewState("Row") = ""
                                                ViewState("IM2") = ""
                                            Else
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    ElseIf ViewState("Prize") IsNot Nothing Then
                                        If ViewState("Prize") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                            If i = ViewState("Row") Then
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                                ViewState("Row") = ""
                                                ViewState("Prize") = ""
                                            Else
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        'Zulham 04102018 - PAMB SST
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        If dsInputTax.Tables(0).Rows(record).Item(1).ToString = aryDoc(i)(21) Then
                                            strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        End If
                                    Next
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                ElseIf aryDoc(i)(22) = "IM2" Then
                                    'GST Amount - Auto Calculated
                                    'Get IM2 rate
                                    GSTAmount = ""
                                    ViewState("IM2") = "Yes"
                                    ViewState("Row") = i
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                    For Each row As DataRow In strPerc
                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                    Next
                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                        GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                        'Zulham 14042015 IPP GST Stage 1
                                        GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                    End If
                                    'Zulham 04102018 - PAMB SST
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    'strrow &= "<option value=""Select"">---Select---</option>"
                                    strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        If dsInputTax.Tables(0).Rows(record).Item(1).ToString = "IM2" Then
                                            strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        End If
                                    Next

                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    'strrow &= "<option value=""" & compOutputTaxValue & """>" & compOutputTaxValue & "</option>"
                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM2 Then
                                            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        End If
                                        'strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Next
                                ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                    'GST Amount - Auto Calculated
                                    'Get IM2 rate
                                    GSTAmount = ""
                                    ViewState("Prize") = "Yes"
                                    ViewState("Row") = i
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                    For Each row As DataRow In strPerc
                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                    Next
                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                        GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                        'Zulham 14042015 IPP GST Stage 1
                                        GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                    End If
                                    'Zulham 04102018 - PAMB SST
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    strrow &= "<option value=""Select"">---Select---</option>"
                                    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Next

                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    'strrow &= "<option value=""" & compOutputTaxValue & """>" & compOutputTaxValue & "</option>"
                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        End If
                                        'strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Next

                                Else 'reimbursement selected
                                    If ViewState("IM2") IsNot Nothing Then
                                        'Zulham 04102018 - PAMB SST
                                        If ViewState("IM2") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                            If i = ViewState("Row") Then
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            Else
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    ElseIf ViewState("Prize") IsNot Nothing Then
                                        'Zulham 04102018 - PAMB SST
                                        If ViewState("Prize") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                            If i = ViewState("Row") Then
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            Else
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    If aryDoc(i)(22).ToString = "Select" Then
                                        strrow &= "<option value=""Select"">---Select---</option>"
                                    End If
                                    For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                            tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                            strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                        End If
                                    Next
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                    If Not aryDoc(i)(23) Is Nothing Then
                                        If aryDoc(i)(23).ToString = "Select" Then
                                            strrow &= "<option value=""Select"">---Select---</option>"
                                        End If

                                        If ViewState("IM2") IsNot Nothing Then
                                            If ViewState("IM2") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                                If i = ViewState("Row") Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    Next
                                                    ViewState("IM2") = ""
                                                    ViewState("Row") = ""
                                                End If
                                            Else
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                        tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            End If
                                        ElseIf ViewState("Prize") IsNot Nothing Then
                                            If ViewState("Prize") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                                If i = ViewState("Row") Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    Next
                                                    ViewState("Prize") = ""
                                                    ViewState("Row") = ""
                                                End If
                                            Else
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                        tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            End If
                                        Else
                                            For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                    tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        End If
                                    Else
                                        If ViewState("IM2") IsNot Nothing Then
                                            If ViewState("IM2") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                                If i = ViewState("Row") Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    Next
                                                    ViewState("IM2") = ""
                                                    ViewState("Row") = ""
                                                Else
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    Next
                                                End If
                                            Else
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                Next
                                            End If
                                        ElseIf ViewState("Prize") IsNot Nothing Then
                                            If ViewState("Prize") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                                If i = ViewState("Row") Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    Next
                                                    ViewState("Prize") = ""
                                                    ViewState("Row") = ""
                                                Else
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    Next
                                                End If
                                            Else
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                Next
                                            End If
                                        Else
                                            For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                            Next
                                        End If
                                    End If
                                End If
                            ElseIf aryDoc(i)(20) = "D" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Disbursement
                                If Not aryDoc(i)(1).ToString.ToUpper = "HLISB" Then
                                    'Gst Amount
                                    'Zulham 04102018 - PAMB SST
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """  disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                Else

                                    'Start
                                    ''GST Amount
                                    If Not aryDoc(i)(22) Is Nothing Then '
                                        If aryDoc(i)(22) = "IM2" Then
                                            GSTAmount = ""
                                            ViewState("IM2") = "Yes"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                'Zulham 14042015 IPP GST Stage 1
                                                GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                            End If
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                            GSTAmount = ""
                                            ViewState("Prize") = "Yes"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                'Zulham 14042015 IPP GST Stage 1
                                                GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                            End If
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        Else
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        'Zulham 04102018 - PAMB SST
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If

                                    'Predefined tax codes will be default value
                                    Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")
                                    Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")

                                    'Input Tax
                                    If aryDoc(i)(22) Is Nothing Then
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                        'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        'If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                        strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                                                tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                            Else
                                                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            End If
                                        Next
                                    Else
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                        'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        If aryDoc(i)(22).ToString = "Select" Then
                                            strrow &= "<option value=""Select"">---Select---</option>"
                                        End If
                                        For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                                tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                            Else
                                                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                            End If
                                        Next
                                    End If

                                    'Output Tax
                                    If aryDoc(i)(23) Is Nothing Then
                                        If Not aryDoc(i)(22) Is Nothing Then
                                            strrow &= "<td>"
                                            'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            If Not aryDoc(i)(23) Is Nothing Then
                                                If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                Else
                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                End If
                                            Else
                                                'If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                '    strrow &= "<option value=""Select"">---Select---</option>"
                                                'End If
                                                'strrow &= "<option value=""Select"">---Select---</option>"
                                                If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    'For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    '    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    'Next
                                                    'strrow &= "<td>"
                                                    'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                    If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                        ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                Else
                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                End If
                                            End If
                                        Else
                                            If Not aryDoc(i)(23) Is Nothing Then
                                                strrow &= "<td>"
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                If aryDoc(i)(23).ToString = "Select" Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                        tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            Else
                                                strrow &= "<td>"
                                                If VenOutputTaxValue.ToString.Contains("N/A") Then
                                                    strrow &= "<select class=""ddl2"" disabled=""disabled"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                        tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                    ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            End If
                                        End If
                                    Else
                                        strrow &= "<td>"
                                        'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                        If Not aryDoc(i)(22) Is Nothing Then
                                            If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                        Else
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                        End If
                                        'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                        If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                            strrow &= "<option value=""Select"">---Select---</option>"
                                        End If
                                        If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "IM2" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = compInputTaxValue_TX4 Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                            For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                    tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM2 Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        ElseIf aryDoc(i)(22).ToString = compInputTaxValue_TX4 Then
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        Else
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        End If
                                    End If
                                    'End
                                End If
                            ElseIf aryDoc(i)(1) = "Own Co." Then
                                If aryDoc(i)(22) = "IM1" Or aryDoc(i)(22) = "IM3" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                    'GST Amount
                                    If ViewState("IM2") IsNot Nothing Then
                                        If ViewState("IM2") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                            'Zulham 04102018 - PAMB SST
                                            If i = ViewState("Row") Then
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                                ViewState("Row") = ""
                                                ViewState("IM2") = ""
                                            Else
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            'strrow &= "<td align=""right"">"
                                            'strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            'strrow &= "</td>"
                                            'Zulham 11/05/2016
                                            'IM5/IM6 Enhancement
                                            'Zulham 04102018 - PAMB SST
                                            If Trim(aryDoc(i)(22)) = "IM5" Then
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            Else
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly = ""readonly""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ ></span>"
                                                strrow &= "</td>"
                                            End If
                                        End If
                                    Else
                                        '    strrow &= "<td align=""right"">"
                                        '    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        'strrow &= "</td>"
                                        'Zulham 11/05/2016
                                        'IM5/IM6 Enhancement
                                        'Zulham 04102018 - PAMB SST
                                        If Trim(aryDoc(i)(22)) = "IM5" Then
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        Else
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly = ""readonly""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ ></span>"
                                            strrow &= "</td>"
                                        End If
                                    End If

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    If aryDoc(i)(22).ToString = "Select" Then
                                        strrow &= "<option value=""Select"">---Select---</option>"
                                    End If
                                    For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                            tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                            strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                        End If
                                    Next
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                ElseIf aryDoc(i)(22) = "IM2" Then
                                    'GST Amount - Auto Calculated
                                    'Get IM2 rate
                                    GSTAmount = ""
                                    ViewState("IM2") = "Yes"
                                    ViewState("Row") = i
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                    For Each row As DataRow In strPerc
                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                    Next
                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                        GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                        'Zulham 14042015 IPP GST Stage 1
                                        GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                    End If
                                    'GST Amount
                                    'Zulham 04102018 - PAMB SST
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        If dsInputTax.Tables(0).Rows(record).Item(1).ToString = aryDoc(i)(22) Then
                                            strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        End If
                                        'strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Next

                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    'strrow &= "<option value=""" & compOutputTaxValue & """>" & compOutputTaxValue & "</option>"
                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM2 Then
                                            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        End If
                                        'strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Next
                                ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                    'GST Amount - Auto Calculated
                                    'Get IM2 rate
                                    GSTAmount = ""
                                    ViewState("Prize") = "Yes"
                                    ViewState("Row") = i
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                    For Each row As DataRow In strPerc
                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                    Next
                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                        GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                        'Zulham 14042015 IPP GST Stage 1
                                        GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                    End If
                                    'GST Amount
                                    'Zulham 04102018 - PAMB SST
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        If dsInputTax.Tables(0).Rows(record).Item(1).ToString = aryDoc(i)(22) Then
                                            strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        End If
                                    Next

                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        End If
                                        'strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Next
                                Else
                                    'GST Amount
                                    'strrow &= "<td align=""right"">"
                                    'strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    'strrow &= "</td>"
                                    'Zulham 04102018 - PAMB SST
                                    If Trim(aryDoc(i)(22)) = "IM5" Then
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    Else
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly = ""readonly""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ ></span>"
                                        strrow &= "</td>"
                                    End If


                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    If aryDoc(i)(22).ToString = "Select" Then
                                        strrow &= "<option value=""Select"">---Select---</option>"
                                    End If
                                    For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                            tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                            strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                        End If
                                    Next
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                    If Not aryDoc(i)(23) Is Nothing Then
                                        If aryDoc(i)(23).ToString = "Select" Then
                                            strrow &= "<option value=""Select"">---Select---</option>"
                                        End If
                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                            If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                            Else
                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                            End If
                                        Next
                                    Else
                                        strrow &= "<option value=""Select"">---Select---</option>"
                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                        Next
                                    End If
                                End If
                            ElseIf aryDoc(i)(1) = Nothing Then 'Initially
                                If Not ds.Tables(0).Rows(0).Item("ID_GST_VALUE") Is DBNull.Value Then
                                    'GST Amount
                                    'If ds.Tables(0).Rows(0).Item("ID_GST_REIMB") = "D" And Not ds.Tables(0).Rows(0).Item("ID_PAY_FOR") = "HLISB" Then
                                    '    strrow &= "<td align=""right"">"
                                    '    strrow &= "<span class=""GSTamount""><input disabled=""disabled""  style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    '    strrow &= "</td>"
                                    'Else
                                    If CDec(ds.Tables(0).Rows(0).Item("ID_GST_VALUE")) < CDec(1) Then
                                        'Zulham 04102018 - PAMB SST
                                        If CDec(ds.Tables(0).Rows(0).Item("ID_GST_VALUE")) = CDec(0) Then
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "0.00" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        Else
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & ds.Tables(0).Rows(0).Item("ID_GST_VALUE") & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        ''Zulham 11/05/2016
                                        ''IM5/IM6 Enhancement
                                        'If Trim(ds.Tables(0).Rows(0).Item("id_gst_input_tax_code")) = "IM5" Then
                                        '    strrow &= "<td align=""right"">"
                                        '    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & Format(ds.Tables(0).Rows(0).Item("ID_GST_VALUE"), "#.00") & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        '    strrow &= "</td>"
                                        'Else
                                        'Zulham 04102018 - PAMB SST
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly = ""readonly""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & Format(ds.Tables(0).Rows(0).Item("ID_GST_VALUE"), "#.00") & """ ></span>"
                                        strrow &= "</td>"
                                        'End If
                                    End If
                                    'End If
                                Else
                                    'GST Amount
                                    'Zulham 04102018 - PAMB SST
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                End If

                                'Input Tax
                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")

                                If Not ds.Tables(0).Rows(0).Item("ID_GST_INPUT_TAX_CODE") Is DBNull.Value Then
                                    If ds.Tables(0).Rows(0).Item("ID_GST_REIMB") = "D" And Not ds.Tables(0).Rows(0).Item("ID_PAY_FOR") = "HLISB" Then
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                        strrow &= "<option value=""" & 0 & """>" & "N/A" & "</option>"
                                    Else
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                        strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            If ds.Tables(0).Rows(0).Item("ID_GST_INPUT_TAX_CODE").ToString.Trim = dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim Then
                                                strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            Else
                                                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            End If
                                        Next
                                    End If
                                Else
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                End If


                                'Output Tax
                                'Zulham 11012019
                                'If Not ds.Tables(0).Rows(0).Item("ID_GST_outPUT_TAX_CODE") Is DBNull.Value Then
                                '    strrow &= "<td>"
                                '    If ds.Tables(0).Rows(0).Item("ID_GST_INPUT_TAX_CODE").ToString.Trim = "" Then
                                '        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                '        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                '    Else
                                '        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                '        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                '            If ds.Tables(0).Rows(0).Item("ID_GST_outPUT_TAX_CODE").ToString.Trim = dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim Then
                                '                strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                '            Else
                                '                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                '            End If
                                '        Next
                                '    End If
                                'Else
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                'End If

                            ElseIf aryDoc(i)(1) <> "Own Co." And aryDoc(i)(20) = Nothing Then 'Selected Comp + Disbursement would be selected first
                                If Not aryDoc(i)(1).ToString.ToUpper = "HLISB" Then
                                    'Zulham 04102018 - PAMB SST
                                    'GST Amount
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """ name=""txtGSTAmount" & i & """ value=""N/A"" disabled=""disabled""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"" disabled=""disabled"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                Else

                                    'Start
                                    ''GST Amount
                                    If Not aryDoc(i)(22) Is Nothing Then '
                                        If aryDoc(i)(22) = "IM2" Then
                                            GSTAmount = ""
                                            ViewState("IM2") = "Yes"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                'Zulham 14042015 IPP GST Stage 1
                                                GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                            End If
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                            GSTAmount = ""
                                            ViewState("Prize") = "Yes"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                'Zulham 14042015 IPP GST Stage 1
                                                GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                            End If
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        Else
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        'Zulham 04102018 - PAMB SST
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If

                                    'Predefined tax codes will be default value
                                    Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")
                                    Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")

                                    'Input Tax
                                    If aryDoc(i)(22) Is Nothing Then
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                        'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        'If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                        strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                                                tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                            Else
                                                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            End If
                                        Next
                                    Else
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                        'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        If aryDoc(i)(22).ToString = "Select" Then
                                            strrow &= "<option value=""Select"">---Select---</option>"
                                        End If
                                        For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                                tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                            Else
                                                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                            End If
                                        Next
                                    End If

                                    'Output Tax
                                    If aryDoc(i)(23) Is Nothing Then
                                        If Not aryDoc(i)(22) Is Nothing Then
                                            strrow &= "<td>"
                                            'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            If Not aryDoc(i)(23) Is Nothing Then
                                                If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                Else
                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                End If
                                            Else
                                                'If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                '    strrow &= "<option value=""Select"">---Select---</option>"
                                                'End If
                                                'strrow &= "<option value=""Select"">---Select---</option>"
                                                If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    'For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    '    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    'Next
                                                    'strrow &= "<td>"
                                                    'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                    If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                        ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                Else
                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                End If
                                            End If
                                        Else
                                            If Not aryDoc(i)(23) Is Nothing Then
                                                strrow &= "<td>"
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                If aryDoc(i)(23).ToString = "Select" Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                        tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            Else
                                                strrow &= "<td>"
                                                If VenOutputTaxValue.ToString.Contains("N/A") Then
                                                    strrow &= "<select class=""ddl2"" disabled=""disabled"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                        tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                    ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            End If
                                        End If
                                    Else
                                        strrow &= "<td>"
                                        'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                        If Not aryDoc(i)(22) Is Nothing Then
                                            If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                        Else
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                        End If
                                        'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                        If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                            strrow &= "<option value=""Select"">---Select---</option>"
                                        End If
                                        If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "IM2" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = compInputTaxValue_TX4 Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                            For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                    tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM2 Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        ElseIf aryDoc(i)(22).ToString = compInputTaxValue_TX4 Then
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        Else
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        End If
                                    End If
                                    'End

                                End If
                            End If
                        End If
                    End If
                Else
                    ''GST Amount
                    'strrow &= "<td align=""right"">"
                    'strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """ name=""txtGSTAmount" & i & """ value=""N/A"" disabled=""disabled""></span>"
                    'strrow &= "</td>"
                    ''Input Tax
                    'strrow &= "<td>"
                    'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"" disabled=""disabled"">"
                    'strrow &= "<option value=""" & 0 & """>N/A</option>"
                    ''Output Tax
                    'strrow &= "<td>"
                    'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                    'strrow &= "<option value=""" & 0 & """>N/A</option>"

                    '---
                    ''Predefined tax codes will be default value
                    'Dim VenInputTaxValue = "NR" 'objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")
                    'Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")

                    ''GST Amount
                    'If aryDoc(i)(21) Is Nothing Then
                    '    strrow &= "<td align=""right"">"
                    '    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                    '    strrow &= "</td>"
                    'Else
                    '    strrow &= "<td align=""right"">"
                    '    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                    '    strrow &= "</td>"
                    'End If

                    ''Input Tax
                    'If aryDoc(i)(22) Is Nothing Then
                    '    strrow &= "<td>"
                    '    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                    '    dsInputTax = GST.GetTaxCode_forIPP("", "P")
                    '    If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                    '    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                    '        If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                    '            tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                    '            strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                    '        Else
                    '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                    '        End If
                    '    Next
                    'Else
                    '    strrow &= "<td>"
                    '    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                    '    dsInputTax = GST.GetTaxCode_forIPP("", "P")
                    '    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                    '        If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = aryDoc(i)(22).Trim Then
                    '            tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                    '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & tempStr & "</option>"
                    '        Else
                    '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                    '        End If
                    '    Next
                    'End If

                    ''Output Tax
                    'strrow &= "<td>"
                    'strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlOutputTax" & i & """ value="""" disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                    'strrow &= "<option value=""0"" selected=""selected"">" & "N/A" & "</option>"
                    '---

                    '===============
                    'Jules 2018.07.09 - Allow "\" and "#"
                    'Check for document dated before cut off date
                    Dim documentDate = objDB.GetVal("SELECT IFNULL(im_doc_date,'') 'im_doc_date' FROM invoice_mstr WHERE im_invoice_no = '" & Common.Parse(Replace(Replace(Request.QueryString("docno"), "\", "\\"), "#", "\#")) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                    Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")

                    If CDate(documentDate) < CDate(_cutoffDate) Then

                        'Gst Amount
                        'strrow &= "<td align=""right"">"
                        'strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                        'strrow &= "</td>"
                        'Zulham 02062015 IPP GST Stage 1
                        'Changed the GST Amount from N/A to 0.00
                        'Zulham 04102018 - PAMB SST
                        strrow &= "<td align=""right"">"
                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "0.00" & """></span>"
                        strrow &= "</td>"
                        'Input TaxS
                        strrow &= "<td>"
                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """  disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                        'Output Tax
                        strrow &= "<td>"
                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                        strrow &= "<option value=""" & 0 & """>N/A</option>"

                    Else
                        'Predefined tax codes will be default value
                        'Zulham 07052015 IPP GST Stage 1
                        'TX4 Change Request 
                        'Dim VenInputTaxValue = "NR" 'objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")
                        'Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")
                        Dim VenInputTaxValue = ds.Tables(0).Rows(0).Item("ID_GST_input_tax_code")
                        Dim VenOutputTaxValue = ds.Tables(0).Rows(0).Item("ID_GST_output_tax_code")

                        ''GST Amount
                        'If aryDoc(i)(21) Is Nothing Then
                        '    strrow &= "<td align=""right"">"
                        '    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                        '    strrow &= "</td>"
                        'Else
                        '    strrow &= "<td align=""right"">"
                        '    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                        '    strrow &= "</td>"
                        'End If
                        'GST Amount
                        If aryDoc(i)(22) IsNot Nothing Then
                            If aryDoc(i)(22).ToString.Contains("IM2") Then
                                GSTAmount = ""
                                ViewState("IM2") = "Yes"
                                ViewState("Row") = i
                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                For Each row As DataRow In strPerc
                                    strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                Next
                                If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" And Not aryDoc.Item(i)(5).ToString.Trim = "" Then
                                    GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                    'Zulham 14042015 IPP GST Stage 1
                                    GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                End If
                                'Zulham 04102018 - PAMB SST
                                strrow &= "<td align=""right"">"
                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                strrow &= "</td>"
                            Else
                                'Zulham 04102018 - PAMB SST
                                If aryDoc(i)(21) Is Nothing Then
                                    'Zulham 07052015 IPP GST Stage 1
                                    'Change request for TX4 
                                    If Not ds.Tables(0).Rows(0).Item("ID_GST_value") Is Nothing Then
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & FormatNumber(ds.Tables(0).Rows(0).Item("ID_GST_VALUE"), 2) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    Else
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If
                                Else
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                End If
                            End If
                        Else
                            'Zulham 04102018 - PAMB SST
                            If aryDoc(i)(21) Is Nothing Then
                                'Zulham 07052015 IPP GST Stage 1
                                'Change request for TX4 
                                If Not ds.Tables(0).Rows(0).Item("ID_GST_value") Is Nothing Then
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & FormatNumber(ds.Tables(0).Rows(0).Item("ID_GST_value"), 2) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                Else
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                End If
                            Else
                                If Not aryDoc(i)(21).ToString = "0" Then
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                Else
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & FormatNumber(ds.Tables(0).Rows(0).Item("ID_GST_value"), 2) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                End If
                            End If
                        End If

                        'Input Tax
                        If aryDoc(i)(22) Is Nothing Then
                            strrow &= "<td>"
                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                            'If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                            strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                                    tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                    strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                Else
                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                End If
                            Next
                        Else
                            strrow &= "<td>"
                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                            strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = aryDoc(i)(22).Trim Then
                                    tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                Else
                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                End If
                            Next
                        End If

                        ''Output Tax
                        'strrow &= "<td>"
                        'strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlOutputTax" & i & """ value="""" disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                        'strrow &= "<option value=""0"" selected=""selected"">" & "N/A" & "</option>"
                        If Not aryDoc(i)(22) Is Nothing Then
                            If aryDoc(i)(22).ToString.Contains("IM2") Then
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlOutputTax" & i & """ value="""" disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                If dsOutputTax.Tables(0).Rows.Count > 0 Then
                                    For rec As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                        If dsOutputTax.Tables(0).Rows(rec).Item(1).ToString = compOutputTaxValue_IM2 Then
                                            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(rec).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(rec).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(rec).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(rec).Item(0).ToString & "</option>"
                                        End If
                                    Next
                                End If
                            Else
                                'Output Tax
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlOutputTax" & i & """ value="""" disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                strrow &= "<option value=""0"" selected=""selected"">" & "N/A" & "</option>"
                            End If
                        Else
                            'Zulham 07052015 IPP GST Stage 1
                            'TX4 Change Request 
                            If VenOutputTaxValue.ToString.Trim.Contains("N/A") Or VenOutputTaxValue.ToString.Trim = "" Then
                                'Output Tax
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlOutputTax" & i & """ value="""" disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                strrow &= "<option value=""0"" selected=""selected"">" & "N/A" & "</option>"
                            Else
                                strrow &= "<td>"
                                If VenInputTaxValue.ToString.Trim.Contains("IM2") Or VenInputTaxValue.ToString.Trim.Contains("TX4") Then
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlOutputTax" & i & """ value="""" disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                Else
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlOutputTax" & i & """ value="""" onchange =""onClick('" & i & "');"">"
                                End If
                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                If dsOutputTax.Tables(0).Rows.Count > 0 Then
                                    For rec As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                        If dsOutputTax.Tables(0).Rows(rec).Item(1).ToString = VenOutputTaxValue.ToString.Trim Then
                                            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(rec).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(rec).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(rec).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(rec).Item(0).ToString & "</option>"
                                        End If
                                    Next
                                End If
                            End If
                        End If
                        '=====
                    End If
                End If
            End If
            'End

            'Zulham PAMB - 23042018
            'Added Gift dropdownlist
            'Zulham 27092018 - PAMB SST
            strrow &= "<td style=""display:none;"">"
            If ds.Tables(0).Rows(0).Item("ID_GIFT") IsNot DBNull.Value Then
                If aryDoc(i)(37) Is Nothing Then
                    If ds.Tables(0).Rows(0).Item("ID_GIFT") = "Yes" Then
                        strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" onchange =""onClick('" & i & "');"" name=""ddlGift" & i & """ value="""">"
                        strrow &= "<option value=""Yes"" selected=""selected"">" & "Yes" & "</option>"
                        strrow &= "<option value=""No"">" & "No" & "</option>"
                    Else
                        strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" onchange =""onClick('" & i & "');"" name=""ddlGift" & i & """ value="""">"
                        strrow &= "<option value=""No"" selected=""selected"">" & "No" & "</option>"
                        strrow &= "<option value=""Yes"">" & "Yes" & "</option>"
                    End If
                Else

                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" onchange =""onClick('" & i & "');"" name=""ddlGift" & i & """ value="""">"
                    If aryDoc(i)(37).ToString.Trim() = "Yes" Then
                        strrow &= "<option value=""Yes"" selected=""selected"">" & "Yes" & "</option>"
                        strrow &= "<option value=""No"">" & "No" & "</option>"
                    Else
                        strrow &= "<option value=""No"" selected=""selected"">" & "No" & "</option>"
                        strrow &= "<option value=""Yes"">" & "Yes" & "</option>"
                    End If

                End If
            Else
                If aryDoc(i)(37) Is Nothing Then
                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" onchange =""onClick('" & i & "');"" name=""ddlGift" & i & """ value="""">"
                    strrow &= "<option value=""No"" selected=""selected"">" & "No" & "</option>"
                    strrow &= "<option value=""Yes"">" & "Yes" & "</option>"
                Else
                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" onchange =""onClick('" & i & "');"" name=""ddlGift" & i & """ value="""">"
                    If aryDoc(i)(37).ToString.Trim() = "Yes" Then
                        strrow &= "<option value=""Yes"" selected=""selected"">" & "Yes" & "</option>"
                        strrow &= "<option value=""No"">" & "No" & "</option>"
                    Else
                        strrow &= "<option value=""No"" selected=""selected"">" & "No" & "</option>"
                        strrow &= "<option value=""Yes"">" & "Yes" & "</option>"
                    End If
                End If
            End If
            strrow &= "</td>"
            'End


            'Jules 2018.04.13 - PAMB Scrum 1 - Added Category.
            'Category
            strrow &= "<td >"

            Dim strCategory As String = ""
            If ds.Tables(0).Rows(0).Item("ID_CATEGORY") IsNot DBNull.Value Then
                strCategory = ds.Tables(0).Rows(0).Item("ID_CATEGORY")
            End If

            If aryDoc(i)(25) Is Nothing Then
                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlCategory" & i & """ value="""" onchange =""onClick('" & i & "');"">"
                If strCategory <> "" Then
                    If strCategory = "Life" Then
                        strrow &= "<option value=""Life"" selected=""selected"">" & "Life" & "</option>"
                        strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                        strrow &= "<option value=""Mixed"">" & "Mixed" & "</option>"
                    ElseIf strCategory = "Non-Life" Then
                        strrow &= "<option value=""Life"">" & "Life" & "</option>"
                        strrow &= "<option value=""Non-Life"" selected=""selected"">" & "Non-Life" & "</option>"
                        strrow &= "<option value=""Mixed"">" & "Mixed" & "</option>"
                    Else
                        strrow &= "<option value=""Life"">" & "Life" & "</option>"
                        strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                        strrow &= "<option value=""Mixed"" selected=""selected"">" & "Mixed" & "</option>"
                    End If
                Else
                    strrow &= "<option value=""Life"">" & "Life" & "</option>"
                    strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                    strrow &= "<option value=""Mixed"">" & "Mixed" & "</option>"
                End If
            Else
                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlCategory" & i & """ value="""" onchange =""onClick('" & i & "');"">"
                If aryDoc(i)(25).ToString = "Life" Then
                    strrow &= "<option value=""Life"" selected=""selected"">" & "Life" & "</option>"
                    strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                    strrow &= "<option value=""Mixed"">" & "Mixed" & "</option>"
                ElseIf aryDoc(i)(25).ToString = "Non-Life" Then
                    strrow &= "<option value=""Life"">" & "Life" & "</option>"
                    strrow &= "<option value=""Non-Life"" selected=""selected"">" & "Non-Life" & "</option>"
                    strrow &= "<option value=""Mixed"">" & "Mixed" & "</option>"
                ElseIf aryDoc(i)(25).ToString = "Mixed" Then
                    strrow &= "<option value=""Life"">" & "Life" & "</option>"
                    strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                    strrow &= "<option value=""Mixed"" selected=""selected"">" & "Mixed" & "</option>"
                ElseIf strCategory <> "" Then
                    If strCategory = "Life" Then
                        strrow &= "<option value=""Life"" selected=""selected"">" & "Life" & "</option>"
                        strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                        strrow &= "<option value=""Mixed"">" & "Mixed" & "</option>"
                    ElseIf strCategory = "Non-Life" Then
                        strrow &= "<option value=""Life"">" & "Life" & "</option>"
                        strrow &= "<option value=""Non-Life"" selected=""selected"">" & "Non-Life" & "</option>"
                        strrow &= "<option value=""Mixed"">" & "Mixed" & "</option>"
                    Else
                        strrow &= "<option value=""Life"">" & "Life" & "</option>"
                        strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                        strrow &= "<option value=""Mixed"" selected=""selected"">" & "Mixed" & "</option>"
                    End If
                End If
            End If
            strrow &= "</td>"
            'End modification.

            Dim glcode As String = ""
            If InStr(ds.Tables(0).Rows(0).Item("ID_B_GL_CODE").ToString, ":") Then
                glcode = ds.Tables(0).Rows(0).Item("ID_B_GL_CODE").ToString.Substring(0, InStr(ds.Tables(0).Rows(0).Item("ID_B_GL_CODE").ToString, ":") - 1)
            Else
                glcode = ds.Tables(0).Rows(0).Item("ID_B_GL_CODE")
            End If

            'Jules 2018.07.11 - Changed txtGLCode from readonly to enabled to allow user to delete.
            'Zulham 08102018 - PAMB SST
            If (ds.Tables(0).Rows(0).Item("ID_PAY_FOR")) = "Own Co." Then
                strrow &= "<td >"
                strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtGLCode" & i & """ name=""txtGLCode" & i & """ value=""" & ds.Tables(0).Rows(0).Item("ID_B_GL_CODE") & ":" & ds.Tables(0).Rows(0).Item("CBG_B_GL_DESC") & """>"
                strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnGLCode" & i & """ name=""btnGLCode" & i & """ onclick=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & ds.Tables(0).Rows(0).Item("ID_B_GL_CODE") & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
                strrow &= "<input type=""hidden"" id=""hidGLCode" & i & """ name=""hidGLCode" & i & """ value=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & glcode & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnGLCode onclick
                strrow &= "<input type=""hidden"" id=""hidGLCodeVal" & i & """ value=""" & glcode & """ runat=""server"" />"
                strrow &= "</td>"
            Else
                If Not aryDoc(i)(8) Is Nothing Then
                    strrow &= "<td >"
                    strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtGLCode" & i & """ name=""txtGLCode" & i & """ value=""" & aryDoc(i)(8) & """>"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnGLCode" & i & """ name=""btnGLCode" & i & """ onclick=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & aryDoc(i)(8).ToString.Split(":")(0).Trim & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                    strrow &= "<input type=""hidden"" id=""hidGLCode" & i & """ name=""hidGLCode" & i & """ value=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & aryDoc(i)(8).ToString.Split(":")(0).Trim & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnGLCode onclick
                    strrow &= "<input type=""hidden"" id=""hidGLCodeVal" & i & """ value=""" & aryDoc(i)(8).ToString.Split(":")(0).Trim & """ runat=""server"" />"
                    strrow &= "</td>"
                Else
                    strrow &= "<td >"
                    strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtGLCode" & i & """ name=""txtGLCode" & i & """ value=""" & ds.Tables(0).Rows(0).Item("ID_B_GL_CODE") & ":" & ds.Tables(0).Rows(0).Item("CBG_B_GL_DESC") & """ >"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnGLCode" & i & """ name=""btnGLCode" & i & """ onclick=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & ds.Tables(0).Rows(0).Item("ID_B_GL_CODE") & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                    strrow &= "<input type=""hidden"" id=""hidGLCode" & i & """ name=""hidGLCode" & i & """ value=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & glcode & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnGLCode onclick
                    strrow &= "<input type=""hidden"" id=""hidGLCodeVal" & i & """ value=""" & glcode & """ runat=""server"" />"
                    strrow &= "</td>"
                End If
            End If

            ''Jules 2018.04.25 - PAMB Scrum 1 - Removed Sub Description.
            ''For RuleCat
            ''If there's only one Sub Description, auto populate.
            'Dim dsCat As New DataSet : dsCat = Nothing
            'Dim objIPPMain As New IPPMain
            'If Not aryDoc(i)(8) Is Nothing Then
            '    If Not aryDoc(i)(8).ToString.Trim.Length = 0 Then
            '        dsCat = objIPPMain.getRuleCategory(aryDoc(i)(8).ToString.Split(":")(0).Trim)
            '    End If
            'End If
            'If Not dsCat Is Nothing Then
            '    If dsCat.Tables(0).Rows.Count = 1 Then
            '        strrow &= "<td >"
            '        strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text"" onkeypress=""return isNumberCharKey(event);""  id=""txtRuleCategory" & i & """ name=""txtRuleCategory" & i & """ value=""" & dsCat.Tables(0).Rows(0).Item("igc_glrule_category").ToString & """>"
            '        strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnSubDescCode" & i & """ name=""btnSubDescCode" & i & """ onclick=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "&txtid=" & "txtRuleCategory" & i & "&hidbtnid=btnSubDescCode" & i & "&hidid=hidRuleCategory2" & i & "&hidvalid=hidRuleCategoryVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
            '        strrow &= "<input type=""hidden"" id=""hidRuleCategory2" & i & """ name=""hidRuleCategory2" & i & """ value=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8).ToString & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;"" />" 'this line must be same with the line btnCostAlloc2 onclick
            '        strrow &= "<input type=""hidden"" id=""hidRuleCategoryVal" & i & """ name=""hidRuleCategoryVal" & i & """ value=""" & dsCat.Tables(0).Rows(0).Item("igc_glrule_category_index").ToString & """  runat=""server"" />"
            '        strrow &= "</td>"
            '    ElseIf dsCat.Tables(0).Rows.Count > 1 Then
            '        If aryDoc(i)(18) Is Nothing Then
            '            strrow &= "<td >"
            '            strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  onkeypress=""return isNumberCharKey(event);""  id=""txtRuleCategory" & i & """ name=""txtRuleCategory" & i & """ value="""">"
            '            strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnSubDescCode" & i & """ name=""btnSubDescCode" & i & """ onclick=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "&txtid=" & "txtRuleCategory" & i & "&hidbtnid=btnSubDescCode" & i & "&hidid=hidRuleCategory2" & i & "&hidvalid=hidRuleCategoryVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
            '            strrow &= "<input type=""hidden"" id=""hidRuleCategory2" & i & """ name=""hidRuleCategory2" & i & """ value=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8).ToString & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;"" />" 'this line must be same with the line btnCostAlloc2 onclick
            '            strrow &= "<input type=""hidden"" id=""hidRuleCategoryVal" & i & """ name=""hidRuleCategoryVal" & i & """ value=""" & aryDoc(i)(19) & """  runat=""server"" />"
            '            strrow &= "</td>"
            '        Else
            '            strrow &= "<td >"
            '            strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  onkeypress=""return isNumberCharKey(event);""  id=""txtRuleCategory" & i & """ name=""txtRuleCategory" & i & """ value=""" & aryDoc(i)(18) & """>"
            '            strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnSubDescCode" & i & """ name=""btnSubDescCode" & i & """ onclick=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "&txtid=" & "txtRuleCategory" & i & "&hidbtnid=btnSubDescCode" & i & "&hidid=hidRuleCategory2" & i & "&hidvalid=hidRuleCategoryVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
            '            strrow &= "<input type=""hidden"" id=""hidRuleCategory2" & i & """ name=""hidRuleCategory2" & i & """ value=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8).ToString & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;"" />" 'this line must be same with the line btnCostAlloc2 onclick
            '            strrow &= "<input type=""hidden"" id=""hidRuleCategoryVal" & i & """ name=""hidRuleCategoryVal" & i & """ value=""" & aryDoc(i)(19) & """  runat=""server"" />"
            '            strrow &= "</td>"
            '        End If
            '    ElseIf dsCat.Tables(0).Rows.Count = 0 Then
            '        strrow &= "<td >"
            '        strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  onkeypress=""return isNumberCharKey(event);""  id=""txtRuleCategory" & i & """ name=""txtRuleCategory" & i & """ readonly value=""" & "" & """>"
            '        strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnSubDescCode" & i & """ name=""btnSubDescCode" & i & """ onclick=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "&txtid=" & "txtRuleCategory" & i & "&hidbtnid=btnSubDescCode" & i & "&hidid=hidRuleCategory2" & i & "&hidvalid=hidRuleCategoryVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
            '        strrow &= "<input type=""hidden"" id=""hidRuleCategory2" & i & """ name=""hidRuleCategory2" & i & """ value=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8).ToString & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;"" />" 'this line must be same with the line btnCostAlloc2 onclick
            '        strrow &= "<input type=""hidden"" id=""hidRuleCategoryVal" & i & """ name=""hidRuleCategoryVal" & i & """ value=""" & "" & """  runat=""server"" />"
            '        strrow &= "</td>"
            '    Else
            '        strrow &= "<td >"
            '        strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  onkeypress=""return isNumberCharKey(event);""  id=""txtRuleCategory" & i & """ name=""txtRuleCategory" & i & """ value=""" & aryDoc(i)(18) & """>"
            '        strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnSubDescCode" & i & """ name=""btnSubDescCode" & i & """ onclick=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "&txtid=" & "txtRuleCategory" & i & "&hidbtnid=btnSubDescCode" & i & "&hidid=hidRuleCategory2" & i & "&hidvalid=hidRuleCategoryVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
            '        strrow &= "<input type=""hidden"" id=""hidRuleCategory2" & i & """ name=""hidRuleCategory2" & i & """ value=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;"" />" 'this line must be same with the line btnCostAlloc2 onclick
            '        strrow &= "<input type=""hidden"" id=""hidRuleCategoryVal" & i & """ name=""hidRuleCategoryVal" & i & """ value=""" & aryDoc(i)(19) & """  runat=""server"" />"
            '        strrow &= "</td>"
            '    End If
            'ElseIf ds.Tables(0).Rows(0).Item("ID_GLRULE_CATEGORY") IsNot DBNull.Value Then
            '    strrow &= "<td >"
            '    strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  onkeypress=""return isNumberCharKey(event);""  id=""txtRuleCategory" & i & """ name=""txtRuleCategory" & i & """ value=""" & ds.Tables(0).Rows(0).Item("ID_GLRULE_CATEGORY") & """>"
            '    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnSubDescCode" & i & """ name=""btnSubDescCode" & i & """ onclick=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "&txtid=" & "txtRuleCategory" & i & "&hidbtnid=btnSubDescCode" & i & "&hidid=hidRuleCategory2" & i & "&hidvalid=hidRuleCategoryVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
            '    strrow &= "<input type=""hidden"" id=""hidRuleCategory2" & i & """ name=""hidRuleCategory2" & i & """ value=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & glcode & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;"" />" 'this line must be same with the line btnCostAlloc2 onclick
            '    strrow &= "<input type=""hidden"" id=""hidRuleCategoryVal" & i & """ name=""hidRuleCategoryVal" & i & """ value=""" & ds.Tables(0).Rows(0).Item("ID_GLRULE_CATEGORY_INDEX") & """  runat=""server"" />"
            '    strrow &= "</td>"
            '    ViewState("ID_GLRULE_CATEGORY") = ds.Tables(0).Rows(0).Item("ID_B_GL_CODE")
            'Else
            '    strrow &= "<td >"
            '    strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  onkeypress=""return isNumberCharKey(event);""  id=""txtRuleCategory" & i & """ name=""txtRuleCategory" & i & """ value=""" & aryDoc(i)(18) & """>"
            '    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnSubDescCode" & i & """ name=""btnSubDescCode" & i & """ onclick=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "&txtid=" & "txtRuleCategory" & i & "&hidbtnid=btnSubDescCode" & i & "&hidid=hidRuleCategory2" & i & "&hidvalid=hidRuleCategoryVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
            '    strrow &= "<input type=""hidden"" id=""hidRuleCategory2" & i & """ name=""hidRuleCategory2" & i & """ value=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;"" />" 'this line must be same with the line btnCostAlloc2 onclick
            '    strrow &= "<input type=""hidden"" id=""hidRuleCategoryVal" & i & """ name=""hidRuleCategoryVal" & i & """ value=""" & aryDoc(i)(19) & """  runat=""server"" />"
            '    strrow &= "</td>"
            'End If

            ''End If
            ''End
            'End modification.

            'Jules 2018.04.10 - PAMB Scrum 1 - Commented HO/BR field.
            'Dim brcode As String
            'If InStr(ds.Tables(0).Rows(0).Item("ID_BRANCH_CODE").ToString, ":") Then
            '    brcode = ds.Tables(0).Rows(0).Item("ID_BRANCH_CODE").ToString.Substring(0, InStr(ds.Tables(0).Rows(0).Item("ID_BRANCH_CODE").ToString, ":") - 1)
            'Else
            '    brcode = ds.Tables(0).Rows(0).Item("ID_BRANCH_CODE")
            'End If
            ''aryDoc.Add(New String() {Request.Form("txtSNo" & 0), Request.Form("ddlPayFor" & 1), Request.Form("txtInvNo" & 2), Request.Form("txtDesc" & 3), Request.Form("ddlUOM" & 4), Request.Form("txtQty" & 5), Request.Form("txtUnitPrice" & 6), Request.Form("txtAmt" & 7), Request.Form("txtGLCode" & 8), Request.Form("txtAssetGroup" & 9), Request.Form("txtAssetSubGroup" & 10), Request.Form("txtCostAlloc" & 11), Request.Form("txtBranch" & i), Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i)})
            'If aryDoc(i)(9) Is Nothing Then
            '    strrow &= "<td >"
            '    strrow &= "<input style=""width:70px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtBranch" & i & """ name=""txtBranch" & i & """ value=""" & ds.Tables(0).Rows(0).Item("ID_BRANCH_CODE") & ":" & ds.Tables(0).Rows(0).Item("ID_BRANCH_CODE_NAME") & """>"
            '    strrow &= "<input type=""hidden"" id=""hidBranchVal" & i & """ value=""" & brcode & """  runat=""server"" />"
            '    strrow &= "</td>"
            'Else
            '    strrow &= "<td >"
            '    strrow &= "<input style=""width:70px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtBranch" & i & """ name=""txtBranch" & i & """ value=""" & aryDoc(i)(9) & """>"
            '    strrow &= "<input type=""hidden"" id=""hidBranchVal" & i & """ value=""" & aryDoc(i)(9).ToString.Split(":")(0).Trim & """  runat=""server"" />"
            '    strrow &= "</td>"
            'End If
            'End commented block.

            'Jules 2018.04.13 - PAMB Scrum 1 - Added Analysis Codes            
            'Analysis Codes 
            Dim analysisCode_edit As String = ""
            Dim analysisCodeDesc_edit As String = ""

            For j As Integer = 1 To 9
                If j = 6 Or j = 7 Then Continue For

                analysisCode_edit = ""
                analysisCodeDesc_edit = ""

                If ds.Tables(0).Rows(0).Item("ID_ANALYSIS_CODE" & j & "") IsNot DBNull.Value Then
                    If InStr(ds.Tables(0).Rows(0).Item("ID_ANALYSIS_CODE" & j & "").ToString, ":") Then
                        analysisCode_edit = ds.Tables(0).Rows(0).Item("ID_ANALYSIS_CODE" & j & "").ToString.Substring(0, InStr(ds.Tables(0).Rows(0).Item("ID_ANALYSIS_CODE" & j & "").ToString, ":") - 1)
                    Else
                        analysisCode_edit = ds.Tables(0).Rows(0).Item("ID_ANALYSIS_CODE" & j & "")
                    End If
                End If

                strrow &= "<td>"
                If Not aryDoc(i)(25 + j) Is Nothing Then
                    strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtAnalysisCode" & j & "_" & i & """ name=""txtAnalysisCode" & j & "_" & i & """ value=""" & aryDoc(i)(25 + j) & """>"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnAnalysisCode" & j & "_" & i & """ name=""btnAnalysisCode" & j & "_" & i & """ onclick=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & aryDoc(i)(25 + j).ToString.Split(":")(0).Trim & "&txtid=" & "txtAnalysisCode" & j & "_" & i & "&hidbtnid=btnAnalysisCode" & j & "_" & i & "&hidid=hidAnalysisCode" & j & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCode" & j & "_" & i & """ name=""hidAnalysisCode" & j & "_" & i & """ value=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & aryDoc(i)(25 + j).ToString.Split(":")(0).Trim & "&txtid=" & "txtAnalysisCode" & j & "_" & i & "&hidbtnid=btnAnalysisCode" & j & "_" & i & "&hidid=hidAnalysisCode" & j & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnAnalysisCode onclick
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCodeVal" & j & "_" & i & """ value=""" & aryDoc(i)(25 + j).ToString.Split(":")(0).Trim & """ runat=""server"" />"
                ElseIf analysisCode_edit <> "" Then
                    analysisCodeDesc_edit = objDB.GetVal("SELECT IFNULL(AC_ANALYSIS_CODE_DESC,'') 'AC_ANALYSIS_CODE_DESC' FROM analysis_code WHERE AC_ANALYSIS_CODE = '" & analysisCode_edit & "' AND AC_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'")
                    strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtAnalysisCode" & j & "_" & i & """ name=""txtAnalysisCode" & j & "_" & i & """ value=""" & analysisCode_edit & ":" & analysisCodeDesc_edit & """ >"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnAnalysisCode" & j & "_" & i & """ name=""btnAnalysisCode" & j & "_" & i & """ onclick=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & ds.Tables(0).Rows(0).Item("ID_ANALYSIS_CODE" & j & "") & "&txtid=" & "txtAnalysisCode" & j & "_" & i & "&hidbtnid=btnAnalysisCode" & j & "_" & i & "&hidid=hidAnalysisCode" & j & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCode" & j & "_" & i & """ name=""hidAnalysisCode" & j & "_" & i & """ value=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & analysisCode_edit & "&txtid=" & "txtAnalysisCode" & j & "_" & i & "&hidbtnid=btnAnalysisCode" & j & "_" & i & "&hidid=hidAnalysisCode" & j & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnAnalysisCode onclick
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCodeVal" & j & "_" & i & """ value=""" & analysisCode_edit & """ runat=""server"" />"
                Else
                    strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtAnalysisCode" & j & "_" & i & """ name=""txtAnalysisCode" & j & "_" & i & """ value=""" & aryDoc(i)(25 + j) & """>"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnAnalysisCode" & j & "_" & i & """ name=""btnAnalysisCode" & j & "_" & i & """ onclick=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & analysisCode_edit & "&txtid=" & "txtAnalysisCode" & j & "_" & i & "&hidbtnid=btnAnalysisCode" & j & "_" & i & "&hidid=hidAnalysisCode" & j & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCode" & j & "_" & i & """ name=""hidAnalysisCode" & j & "_" & i & """ value=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & analysisCode_edit & "&txtid=" & "txtAnalysisCode2_" & i & "&hidbtnid=btnAnalysisCode" & j & "_" & i & "&hidid=hidAnalysisCode" & j & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnAnalysisCode onclick
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCodeVal" & j & "_" & i & """ value=""" & analysisCode_edit & """ runat=""server"" />"
                End If
                strrow &= "</td>"
            Next
            'End modification.

            Dim cccode As String
            If InStr(ds.Tables(0).Rows(0).Item("ID_COST_CENTER").ToString, ":") Then
                cccode = ds.Tables(0).Rows(0).Item("ID_COST_CENTER").ToString.Substring(0, InStr(ds.Tables(0).Rows(0).Item("ID_COST_CENTER").ToString, ":") - 1)
            Else
                cccode = ds.Tables(0).Rows(0).Item("ID_COST_CENTER")
            End If

            'Zulham 24102018 - PAMB
            Dim costCenterDesc = objDB.GetVal("SELECT cc_CC_desc FROM cost_centre WHERE cc_coy_id = '" & Session("CompanyID") & "' and cc_cc_code = '" & cccode & "'")

            If aryDoc(i)(11) Is Nothing Then
                strrow &= "<td >"
                strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text"" id=""txtCC" & i & """ name=""txtCC" & i & """ value=""" & ds.Tables(0).Rows(0).Item("ID_COST_CENTER") & ":" & IIf(ds.Tables(0).Rows(0).Item("ID_COST_CENTER_DESC") = "", costCenterDesc, ds.Tables(0).Rows(0).Item("ID_COST_CENTER_DESC")) & """>"
                strrow &= "<input type=""hidden"" id=""hidCCVal" & i & """ value=""" & cccode & """  runat=""server"" />"
                strrow &= "</td>"
            ElseIf aryDoc(i)(11) = "" Then
                strrow &= "<td >"
                strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text"" id=""txtCC" & i & """ name=""txtCC" & i & """ value=""" & ds.Tables(0).Rows(0).Item("ID_COST_CENTER") & ":" & IIf(ds.Tables(0).Rows(0).Item("ID_COST_CENTER_DESC") = "", costCenterDesc, ds.Tables(0).Rows(0).Item("ID_COST_CENTER_DESC")) & """>"
                strrow &= "<input type=""hidden"" id=""hidCCVal" & i & """ value=""" & cccode & """  runat=""server"" />"
                strrow &= "</td>"
            Else
                strrow &= "<td >"
                strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text"" id=""txtCC" & i & """ name=""txtCC" & i & """ value=""" & aryDoc(i)(11) & """>"
                strrow &= "<input type=""hidden"" id=""hidCCVal" & i & """ value=""" & aryDoc(i)(11).ToString.Split(":")(0) & """  runat=""server"" />"
                strrow &= "</td>"
            End If

            strrow &= "</td>"


            'mimi 2018-04-13 PAMB Scrum 1 : Add Withholding tax
            Dim holdingTax As String = ""

            If ds.Tables(0).Rows(0).Item("ID_WITHHOLDING_OPT").ToString = "1" Or ds.Tables(0).Rows(0).Item("ID_WITHHOLDING_OPT").ToString = "2" Then
                'holdingTax = ds.Tables(0).Rows(0).Item("ID_WITHHOLDING_TAX").ToString
                holdingTax = ds.Tables(0).Rows(0).Item("ID_WITHHOLDING_OPT") & ":" & ds.Tables(0).Rows(0).Item("ID_WITHHOLDING_TAX").ToString
            ElseIf ds.Tables(0).Rows(0).Item("ID_WITHHOLDING_OPT").ToString = "3" AndAlso ds.Tables(0).Rows(0).Item("ID_WITHHOLDING_REMARKS") IsNot DBNull.Value Then
                'holdingTax = ds.Tables(0).Rows(0).Item("ID_WITHHOLDING_REMARKS")
                holdingTax = ds.Tables(0).Rows(0).Item("ID_WITHHOLDING_OPT") & ":" & ds.Tables(0).Rows(0).Item("ID_WITHHOLDING_REMARKS")
            End If

            If Not aryDoc(i)(24) Is Nothing Then
                strrow &= "<td >"
                'strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtWithholding" & i & """ name=""txtWithholding" & i & """ value=""" & aryDoc(i)(24) & """>"
                'strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnTaxHolding" & i & """ name=""btnTaxHolding" & i & """ onclick=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & aryDoc(i)(24).ToString.Split(":")(0).Trim & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                'strrow &= "<input type=""hidden"" id=""hidHoldingTax" & i & """ name=""hidHoldingTax" & i & """ value=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & aryDoc(i)(24).ToString.Split(":")(0).Trim & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />"
                'strrow &= "<input type=""hidden"" id=""hidHoldingTaxVal" & i & """ value=""" & aryDoc(i)(24).ToString.Split(":")(0).Trim & """ runat=""server"" />"                

                If InStr(aryDoc(i)(24).ToString, ":") Then
                    strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtWithholding" & i & """ name=""txtWithholding" & i & """ value=""" & aryDoc(i)(24).ToString.Split(":")(1).Trim & """>"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnTaxHolding" & i & """ name=""btnTaxHolding" & i & """ onclick=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & aryDoc(i)(24).ToString.Split(":")(1).Trim & "&opt=" & aryDoc(i)(24).ToString.Split(":")(0).Trim & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                    strrow &= "<input type=""hidden"" id=""hidHoldingTax" & i & """ name=""hidHoldingTax" & i & """ value=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & aryDoc(i)(24).ToString.Split(":")(1).Trim & "&opt=" & aryDoc(i)(24).ToString.Split(":")(0).Trim & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />"
                    strrow &= "<input type=""hidden"" id=""hidHoldingTaxVal" & i & """ value=""" & aryDoc(i)(24) & """ runat=""server"" />"
                    strrow &= "<input type=""hidden"" id=""hidHoldingTaxOpt" & i & """ value=""" & aryDoc(i)(24).ToString.Split(":")(0).Trim & """ runat=""server"" />"
                    strrow &= "<input style=""width:165px;margin-right:0px;display:none; "" class=""txtbox2"" type=""text""  id=""txtWithholdingOpt" & i & """ name=""txtWithholdingOpt" & i & """ value=""" & aryDoc(i)(24).ToString.Split(":")(0).Trim & """ >"
                ElseIf Not aryDoc(i)(35) Is Nothing Then
                    strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtWithholding" & i & """ name=""txtWithholding" & i & """ value=""" & aryDoc(i)(24) & """>"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnTaxHolding" & i & """ name=""btnTaxHolding" & i & """ onclick=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & aryDoc(i)(24) & "&opt=" & aryDoc(i)(35) & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                    strrow &= "<input type=""hidden"" id=""hidHoldingTax" & i & """ name=""hidHoldingTax" & i & """ value=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & aryDoc(i)(24) & "&opt=" & aryDoc(i)(35) & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />"
                    strrow &= "<input type=""hidden"" id=""hidHoldingTaxVal" & i & """ value=""" & aryDoc(i)(35) & ":" & aryDoc(i)(24) & """ runat=""server"" />"
                    strrow &= "<input type=""hidden"" id=""hidHoldingTaxOpt" & i & """ value=""" & aryDoc(i)(35) & """ runat=""server"" />"
                    strrow &= "<input style=""width:165px;margin-right:0px;display:none; "" class=""txtbox2"" type=""text""  id=""txtWithholdingOpt" & i & """ name=""txtWithholdingOpt" & i & """ value=""" & aryDoc(i)(35) & """ >"
                End If
                strrow &= "</td>"
            Else
                strrow &= "<td >"
                'strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtWithholding" & i & """ name=""txtWithholding" & i & """ value=""" & holdingTax & """ >"
                'strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnTaxHolding" & i & """ name=""btnTaxHolding" & i & """ onclick=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & ds.Tables(0).Rows(0).Item("ID_WITHHOLDING_TAX") & "&opt=" & ds.Tables(0).Rows(0).Item("ID_WITHHOLDING_OPT") & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                'strrow &= "<input type=""hidden"" id=""hidHoldingTax" & i & """ name=""hidHoldingTax" & i & """ value=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & holdingTax & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />"

                'Jules 2018.07.10
                If holdingTax = "" Then
                    strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtWithholding" & i & """ name=""txtWithholding" & i & """ value="""" >"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnTaxHolding" & i & """ name=""btnTaxHolding" & i & """ onclick=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=&opt=&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                    strrow &= "<input type=""hidden"" id=""hidHoldingTax" & i & """ name=""hidHoldingTax" & i & """ value=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=&opt=&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />"
                    strrow &= "<input type=""hidden"" id=""hidHoldingTaxVal" & i & """ value="""" runat=""server"" />"
                    strrow &= "<input type=""hidden"" id=""hidHoldingTaxOpt" & i & """ value="""" runat=""server"" />"
                    strrow &= "<input style=""width:165px;margin-right:0px;display:none; "" class=""txtbox2"" type=""text""  id=""txtWithholdingOpt" & i & """ name=""txtWithholdingOpt" & i & """ value="""" >"
                Else
                    strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtWithholding" & i & """ name=""txtWithholding" & i & """ value=""" & holdingTax.Split(":")(1).Trim & """ >"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnTaxHolding" & i & """ name=""btnTaxHolding" & i & """ onclick=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & holdingTax.Split(":")(1).Trim & "&opt=" & holdingTax.Split(":")(0).Trim & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                    strrow &= "<input type=""hidden"" id=""hidHoldingTax" & i & """ name=""hidHoldingTax" & i & """ value=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & holdingTax.Split(":")(1).Trim & "&opt=" & holdingTax.Split(":")(0).Trim & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />"
                    strrow &= "<input type=""hidden"" id=""hidHoldingTaxVal" & i & """ value=""" & holdingTax & """ runat=""server"" />"
                    strrow &= "<input type=""hidden"" id=""hidHoldingTaxOpt" & i & """ value=""" & holdingTax.Split(":")(0).Trim & """ runat=""server"" />"
                    strrow &= "<input style=""width:165px;margin-right:0px;display:none; "" class=""txtbox2"" type=""text""  id=""txtWithholdingOpt" & i & """ name=""txtWithholdingOpt" & i & """ value=""" & holdingTax.Split(":")(0).Trim & """ >"
                End If

                strrow &= "</td>"
            End If
            'end added withholding Tax   

            strrow &= "</tr>"
            'End modification.

            If Request.QueryString("doctype") <> "Invoice" And Request.QueryString("doctype") <> "Bill" And Request.QueryString("doctype") <> "Letter" Then
                'strrow &= "<table id=""table44"" style=""border:0;"">"
                'strrow &= "<tr><td style ="" width:500px;""></td><td class = ""emptycol"" style=""width: 120px; text-align:right; font-weight:bold; "">Total :</td>"
                'strrow &= "<td class = ""emptycol"" style=""width: 130px;""> <hr style=""width:130px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:130px;"" /></td></tr>"
                'strrow &= "</table>"
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then

                    'zulham 21/01/2016 - IPP STAGE 4 PHASE 2
                    'added 'total w/o gst amount'
                    'Zulham 04102018 - PAMB SST
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:540px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">Total Amount (excl.SST) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotalNoGST"" name=""sTotalNoGST"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:540px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">SST Amount Input :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTInputTotal"" name=""sGSTInputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:540px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">(SST Amount Output) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTOutputTotal"" name=""sGSTOutputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:540px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">Total (incl. SST) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                Else
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:540px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">Total (incl. SST) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                End If

            Else
                'strrow &= "<table id=""table44"" style=""border:0;"">"
                'strrow &= "<tr><td style ="" width:403px;""></td><td class = ""emptycol"" style=""width: 120px; text-align:right; font-weight:bold; "">Total :</td>"
                'strrow &= "<td class = ""emptycol"" style=""width: 130px;""> <hr style=""width:130px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:130px;"" /></td></tr>"
                'strrow &= "</table>"
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then

                    'zulham 21/01/2016 - IPP STAGE 4 PHASE 2
                    'added 'total w/o gst amount'
                    'Zulham 04102018 - PAMB SST
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:430px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">Total Amount (excl.SST) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotalNoGST"" name=""sTotalNoGST"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:430px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">SST Amount Input :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTInputTotal"" name=""sGSTInputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:430px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">(SST Amount Output) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTOutputTotal"" name=""sGSTOutputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:430px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">Total (incl. SST) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                Else
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:430px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">Total (incl. SST) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                End If
            End If

            If Request.QueryString("doctype") <> "Invoice" And Request.QueryString("doctype") <> "Bill" And Request.QueryString("doctype") <> "Letter" Then
                'mimi 2018-04-24 : remove hardcode HLB to PAMB
                If UCase(ds.Tables(0).Rows(0).Item("ID_PAY_FOR")) = "PAMB" Then
                    'If UCase(ds.Tables(0).Rows(0).Item("ID_PAY_FOR")) = "HLB" Or UCase(ds.Tables(0).Rows(0).Item("ID_PAY_FOR")) = "HLISB" Then
                    If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then

                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR and Sub Description, added Category and Analysis Code, increased table width, changed sequence.
                        'ZULHAM 24092018 - PAMB
                        'UAT - U00007
                        'Zulham 16102018 - PAMB SST
                        table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1500px;"" id=""tblIPPDocItem"">" &
                            "<tr class=""TableHeader"">" &
                            "<td style=""display:none;"" align=""right"">S/No</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Pay For</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Disb./Reimb.</td>" &
                            "<td style=""width:65px;margin-right:0px;"">Invoice No.<span id=""lblInvNo"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:60px;margin-right:0px;display:none;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:45px;margin-right:0px;display:none;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:65px;margin-right:0px;display:none;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount (excl.SST)</td>" &
                            "<td style=""width:110px;margin-right:0px;"" >SST Amount</td>" &
                            "<td style=""width:110px;margin-right:0px;"" >Input Tax Code</td>" &
                            "<td style=""width:110px;margin-right:0px;"" >Output Tax Code</td>" &
                            "<td style=""width:100px;margin-right:0px;display:none;"">Gift</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Category</td>" &
                            "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:50px;margin-right:0px;"">Fund Type(L1)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Product Type(L2)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Channel(L3)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Reinsurance Company(L4)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Asset Fund(L5)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Project Code(L8)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Person Code(L9)</td>" &
                            "<td style=""width:100px;margin-right:0px;"">Cost Centre(L7)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Withholding Tax (%)</td>" & 'mimi - 10 / 4 / 18 : Tax holding percentage
                            "</tr>" &
                            strrow &
                            "</table>"
                        '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" & _
                        '"<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" &
                        '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                        '"</tr>" & _
                        'strrow & _
                        '"</table>"
                        'End modification.
                    Else
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR and Sub Description, added Category and Analysis Code, increased table width, changed sequence.
                        'ZULHAM 24092018 - PAMB
                        'UAT - U00007
                        'Zulham 16102018 - PAMB SST
                        table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1500px;"" id=""tblIPPDocItem"">" &
                                       "<tr class=""TableHeader"">" &
                                       "<td style=""display:none;"" align=""right"">S/No</td>" &
                                       "<td style=""width:50px;margin-right:0px;"">Pay For</td>" &
                                       "<td style=""width:65px;margin-right:0px;"">Invoice No.<span id=""lblInvNo"" class=""errormsg"">*</span></td>" &
                                       "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                                       "<td style=""width:60px;margin-right:0px;display:none;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                                       "<td style=""width:30px;margin-right:0px;display:none;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                                       "<td style=""width:65px;margin-right:0px;display:none;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                                       "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount (excl.SST)</td>" &
                                       "<td style=""width:100px;margin-right:0px;display:none;"">Gift</td>" &
                                       "<td style=""width:50px;margin-right:0px;"">Category</td>" &
                                       "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                                       "<td style=""width:50px;margin-right:0px;"">Fund Type(L1)</td>" &
                                       "<td style=""width:50px;margin-right:0px;"">Product Type(L2)</td>" &
                                       "<td style=""width:50px;margin-right:0px;"">Channel(L3)</td>" &
                                       "<td style=""width:50px;margin-right:0px;"">Reinsurance Company(L4)</td>" &
                                       "<td style=""width:50px;margin-right:0px;"">Asset Fund(L5)</td>" &
                                       "<td style=""width:50px;margin-right:0px;"">Project Code(L8)</td>" &
                                       "<td style=""width:50px;margin-right:0px;"">Person Code(L9)</td>" &
                                       "<td style=""width:100px;margin-right:0px;"">Cost Centre(L7)</td>" &
                                       "<td style=""width:50px;margin-right:0px;"">Withholding Tax (%)</td>" & 'mimi - 10 / 4 / 18 :Tax holding percentage
                                       "</tr>" &
                                       strrow &
                                       "</table>"
                        '"<td style=""width:50px; margin-right:0px;"">HO/BR<span id=""lblBranch"" class=""errormsg"">*</span></td>" & _
                        '"<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" &
                        '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                        '"</tr>" & _
                        'strrow & _
                        '"</table>"
                        'End modification.

                    End If
                Else
                    'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR and Sub Description, added Category and Analysis Code, increased table width, changed sequence.
                    'ZULHAM 24092018 - PAMB
                    'UAT - U00007
                    'Zulham 16102018 - PAMB 
                    table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1500px;"" id=""tblIPPDocItem"">" &
                           "<tr class=""TableHeader"">" &
                           "<td style=""display:none;"" align=""right"">S/No</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Pay For</td>" &
                           "<td style=""width:65px;margin-right:0px;"">Invoice No.<span id=""lblInvNo"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:60px;margin-right:0px;display:none;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:30px;margin-right:0px;display:none;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:65px;margin-right:0px;display:none;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount (excl.SST)</td>" &
                           "<td style=""width:100px;margin-right:0px;display:none;"">Gift</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Category</td>" &
                           "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:50px;margin-right:0px;"">Fund Type(L1)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Product Type(L2)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Channel(L3)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Reinsurance Company(L4)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Asset Fund(L5)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Project Code(L8)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Person Code(L9)</td>" &
                           "<td style=""width:100px;margin-right:0px;"">Cost Centre(L7)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Withholding Tax (%)</td>" & 'mimi - 10 / 4 / 18 :Tax holding percentage
                           "</tr>" &
                            strrow &
                           "</table>"
                    '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" & _
                    '"<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" &
                    '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                    '"</tr>" & _
                    ' strrow & _
                    '"</table>"
                    'End modification.
                End If
            Else
                'mimi 2018-04-24 : remove hardcode HLB to PAMB
                If UCase(ds.Tables(0).Rows(0).Item("ID_PAY_FOR")) = "PAMB" Then
                    'If UCase(ds.Tables(0).Rows(0).Item("ID_PAY_FOR")) = "HLB" Or UCase(ds.Tables(0).Rows(0).Item("ID_PAY_FOR")) = "HLISB" Then
                    If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then

                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR and Sub Description, added Category and Analysis Code, increased table width, changed sequence.
                        'ZULHAM 24092018 - PAMB
                        'UAT U00007
                        'Zulham 16102018 - PAMB
                        table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1500px;"" id=""tblIPPDocItem"">" &
                               "<tr class=""TableHeader"">" &
                               "<td style=""display:none;"" align=""right"">S/No</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Pay For</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Disb./Reimb.</td>" &
                               "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:60px;margin-right:0px;display:none;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:45px;margin-right:0px;display:none;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:65px;margin-right:0px;display:none;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount (excl.SST)</td>" &
                               "<td style=""width:110px;margin-right:0px;"" >SST Amount</td>" &
                               "<td style=""width:110px;margin-right:0px;"" >Input Tax Code</td>" &
                               "<td style=""width:110px;margin-right:0px;"" >Output Tax Code</td>" &
                               "<td style=""width:100px;margin-right:0px;display:none;"">Gift</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Category</td>" &
                               "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:50px;margin-right:0px;"">Fund Type(L1)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Product Type(L2)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Channel(L3)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Reinsurance Company(L4)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Asset Fund(L5)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Project Code(L8)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Person Code(L9)</td>" &
                               "<td style=""width:100px;margin-right:0px;"">Cost Centre(L7)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Withholding Tax (%)</td>" & 'mimi - 10 / 4 / 18 :Tax holding percentage
                               "</tr>" &
                               strrow &
                               "</table>"
                        '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" &
                        '"<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" &
                        '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                        '"</tr>" &
                        'strrow &
                        '"</table>"
                        'End modification.

                    Else
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR and Sub Description, added Category and Analysis Code, increased table width, changed sequence.
                        'ZULHAM 24092018 - PAMB
                        'UAT - U00007
                        'Zulham 16102018 - PAMB
                        table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1500px;"" id=""tblIPPDocItem"">" &
                              "<tr class=""TableHeader"">" &
                              "<td style=""display:none;"" align=""right"">S/No</td>" &
                              "<td style=""width:50px;margin-right:0px;"">Pay For</td>" &
                              "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                              "<td style=""width:60px;margin-right:0px;display:none;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                              "<td style=""width:45px;margin-right:0px;display:none;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                              "<td style=""width:65px;margin-right:0px;display:none;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                              "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount (excl.SST)</td>" &
                              "<td style=""width:100px;margin-right:0px;display:none;"">Gift</td>" &
                              "<td style=""width:50px;margin-right:0px;"">Category</td>" &
                              "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Fund Type(L1)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Product Type(L2)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Channel(L3)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Reinsurance Company(L4)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Asset Fund(L5)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Project Code(L8)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Person Code(L9)</td>" &
                                  "<td style=""width:100px;margin-right:0px;"">Cost Centre(L7)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Withholding Tax (%)</td>" & 'mimi - 10 / 4 / 18 :Tax holding percentage
                                  "</tr>" &
                              strrow &
                              "</table>"
                        '"<td style=""width:50px; margin-right:0px;"">HO/BR<span id=""lblBranch"" class=""errormsg"">*</span></td>" & _
                        '"<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" &
                        '    "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                        '     "</tr>" & _
                        'strrow & _
                        '"</table>"
                        'End modification.
                    End If
                Else
                    If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then

                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR and Sub Description, added Category and Analysis Code, increased table width, changed sequence.
                        'ZULHAM 24092018 - PAMB
                        'UAT U00007
                        'Zulham 16102018 - PAMB
                        table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1500px;"" id=""tblIPPDocItem"">" &
                               "<tr class=""TableHeader"">" &
                               "<td style=""display:none;"" align=""right"">S/No</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Pay For</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Disb./Reimb.</td>" &
                               "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:60px;margin-right:0px;display:none;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:45px;margin-right:0px;display:none;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:65px;margin-right:0px;display:none;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount (excl.SST)</td>" &
                               "<td style=""width:110px;margin-right:0px;"" >SST Amount</td>" &
                               "<td style=""width:110px;margin-right:0px;"" >Input Tax Code</td>" &
                               "<td style=""width:110px;margin-right:0px;"" >Output Tax Code</td>" &
                               "<td style=""width:100px;margin-right:0px;display:none;"">Gift</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Category</td>" &
                               "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:50px;margin-right:0px;"">Fund Type(L1)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Product Type(L2)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Channel(L3)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Reinsurance Company(L4)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Asset Fund(L5)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Project Code(L8)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Person Code(L9)</td>" &
                               "<td style=""width:100px;margin-right:0px;"">Cost Centre(L7)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Withholding Tax (%)</td>" & 'mimi - 10 / 4 / 18 :Tax holding percentage
                               "</tr>" &
                               strrow &
                               "</table>"
                        '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" &
                        '"<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" &
                        '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                        '"</tr>" &
                        'strrow &
                        '"</table>"
                        'End modification.
                    Else
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR and Sub Description, added Category and Analysis Code, increased table width, changed sequence.
                        'ZULHAM 24092018 - PAMB
                        'UAT U00007
                        'Zulham 16102018 - PAMB
                        table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1500px;"" id=""tblIPPDocItem"">" &
                                  "<tr class=""TableHeader"">" &
                                  "<td style=""display:none;"" align=""right"">S/No</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Pay For</td>" &
                                  "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                                  "<td style=""width:60px;margin-right:0px;display:none;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                                  "<td style=""width:45px;margin-right:0px;display:none;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                                  "<td style=""width:65px;margin-right:0px;display:none;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                                  "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount (excl.SST)</td>" &
                                  "<td style=""width:100px;margin-right:0px;display:none;"">Gift</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Category</td>" &
                                  "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Fund Type(L1)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Product Type(L2)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Channel(L3)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Reinsurance Company(L4)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Asset Fund(L5)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Project Code(L8)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Person Code(L9)</td>" &
                                  "<td style=""width:100px;margin-right:0px;"">Cost Centre(L7)</td>" &
                                  "<td style=""width:50px;margin-right:0px;"">Withholding Tax (%)</td>" & 'mimi - 10 / 4 / 18 :Tax holding percentage
                                  "</tr>" &
                                  strrow &
                                  "</table>"
                        '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" & _
                        '"<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" &
                        '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                        ' "</tr>" & _
                        'strrow & _
                        '"</table>"
                        'End modification.

                    End If
                End If
            End If
            'strscript.Append("calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');") 'Jules  2018.07.10
            'strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');" 'Jules 2018.08.16 
        Else
            'for new
            hidMode.Value = "new"
            count = 10
            Sno = CInt(Request.QueryString("rowcount"))
            For i = 0 To count - 1
                Sno = Sno + 1
                strrow &= "<tr style=""background-color:#fdfdfd;"">"

                strrow &= "<td align=""right"">"
                strrow &= "<input type=""hidden""  id=""txtSNo" & i & """ name=""txtSNo" & i & """ value=""" & Sno & """>"
                'strrow &= "<span  class=""lbl"" id=""lblSNo" & i & """ name=""lblSNo" & i & """>" & i + 1 & "</span>"
                strrow &= "<asp:label style=""width:1%;"" class=""lbl""  id=""lblSNo" & i & """ name=""lblSNo" & i & """ value=""""/>" & Sno & ""
                strrow &= "</td>"

                strrow &= "<td>"

                'Zulham 29062015 - HLB-IPP Stage 4 (CR)
                If Common.Parse(Session("CompanyID")).ToUpper = "HLISB" Then
                    strrow &= "<select class=""ddl2"" disabled=""disabled"" style=""width:75px;margin-right:0px;"" onchange =""onClick('" & i & "');"" name=""ddlPayFor" & i & """ value="""">"
                Else
                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" onchange =""onClick('" & i & "');"" name=""ddlPayFor" & i & """ value="""">"
                End If
                strrow &= "<option value=""Own Co."" selected=""selected"">" & "Own Co." & "</option>"

                For c = 0 To dsPayFor.Tables(0).Rows.Count - 1
                    If aryDoc(i)(1) = dsPayFor.Tables(0).Rows(c).Item(0).ToString Then
                        strrow &= "<option value=""" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    Else
                        strrow &= "<option value=""" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & """>" & dsPayFor.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    End If
                Next
                strrow &= "</td>" 'Jules 2018.07.13

                'Zulham Aug 27, 2014
                'Added column: Reimbursement
                'Zulham 26/04/2018 - PAMB
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                    If strIsGst = "Yes" Then
                        'Zulham 26102018
                        'If Not aryDoc(i)(1) = "Own Co." And aryDoc(i)(1) IsNot Nothing And aryDoc(i)(1) <> "" Then
                        If Not aryDoc(i)(20) Is Nothing Then
                            If aryDoc(i)(20) = "R" Then
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                                strrow &= "<option value=""R""  selected=""selected"">" & "Reimbursement" & "</option>"
                                strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                            ElseIf aryDoc(i)(20) = "D" Then
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                strrow &= "<option value=""D""  selected=""selected"">" & "Disbursement" & "</option>"
                                strrow &= "<option value=""R"" >" & "Reimbursement" & "</option>"
                                strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                            End If
                        Else
                            strrow &= "<td>"
                            strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                            strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                            strrow &= "<option value=""R"" selected=""selected"">" & "Reimbursement" & "</option>"
                            strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                        End If
                        'Else
                        '    strrow &= "<td>"
                        '    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value=""""  disabled=""disabled"">"
                        '    strrow &= "<option value=""0"" selected=""selected"">" & "N/A" & "</option>"
                        'End If
                    Else
                        ''Zulham 180562015 - Additional requirement where foreign company should be able to select the reimbursement/disbursement
                        'Zulham 26102018 
                        If Request.QueryString("isResident").Trim.ToUpper = "FALSE" And exceedCutOffDt = "Yes" Then
                            'If Not aryDoc(i)(1) = "Own Co." And aryDoc(i)(1) IsNot Nothing And aryDoc(i)(1) <> "" Then
                            If Not aryDoc(i)(20) Is Nothing Then
                                If aryDoc(i)(20) = "R" Then
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                                    strrow &= "<option value=""R""  selected=""selected"">" & "Reimbursement" & "</option>"
                                    strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                                ElseIf aryDoc(i)(20) = "D" Then
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""D""  selected=""selected"">" & "Disbursement" & "</option>"
                                    strrow &= "<option value=""R"" >" & "Reimbursement" & "</option>"
                                    strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                                End If
                            Else
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                                strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                                strrow &= "<option value=""R"" selected=""selected"">" & "Reimbursement" & "</option>"
                                strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                            End If
                            'Else
                            '    strrow &= "<td>"
                            '    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value=""""  disabled=""disabled"">"
                            '    strrow &= "<option value=""0"" selected=""selected"">" & "N/A" & "</option>"
                            'End If
                        Else
                            strrow &= "<td>"
                            strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                            strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                            strrow &= "<option value=""R"" selected=""selected"">" & "Reimbursement" & "</option>"
                            strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                        End If
                        ''Jules 2018.07.11
                        'strrow &= "<td>"
                        'strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" disabled=""disabled"">"
                        'strrow &= "<option value=""N/A"" selected=""selected"">" & "N/A" & "</option>"
                    End If
                Else 'Jules 2018.07.11
                    strrow &= "<td>"
                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlReimbursement" & i & """ value="""" enabled=""enabled"" onchange =""onClick('" & i & "');"">"
                    strrow &= "<option value=""D"" >" & "Disbursement" & "</option>"
                    strrow &= "<option value=""R"" selected=""selected"">" & "Reimbursement" & "</option>"
                    strrow &= "<option value=""0"" >" & "N/A" & "</option>"
                End If
                'End
                strrow &= "</td>" 'Jules 2018.07.13

                If Request.QueryString("doctype") <> "Invoice" And Request.QueryString("doctype") <> "Bill" And Request.QueryString("doctype") <> "Letter" Then
                    strrow &= "<td >"
                    strrow &= "<input style=""width:85px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtInvNo" & i & """ name=""txtInvNo" & i & """ value=""" & aryDoc(i)(2) & """>"
                    strrow &= "</td>"
                End If

                strrow &= "<td >"
                strrow &= "<input style=""width:145px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtDesc" & i & """ name=""txtDesc" & i & """ value=""" & aryDoc(i)(3) & """>"
                strrow &= "</td>"

                'Zulham 24092018 - PAMB
                'UAT - U00007
                strrow &= "<td style=""display:none;"">"
                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlUOM" & i & """ name=""ddlUOM" & i & """>"
                strrow &= "<option value=""Unit"" selected=""selected"">" & "Unit" & "</option>"
                For c = 0 To dsUOM.Tables(0).Rows.Count - 1
                    If aryDoc(i)(4) = dsUOM.Tables(0).Rows(c).Item(0).ToString Then
                        strrow &= "<option value=""" & dsUOM.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsUOM.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    Else
                        strrow &= "<option value=""" & dsUOM.Tables(0).Rows(c).Item(0).ToString & """>" & dsUOM.Tables(0).Rows(c).Item(0).ToString & "</option>"
                    End If
                Next
                strrow &= "</td>" 'Jules 2018.07.13

                strrow &= "<td align=""right""  style=""display:none;"">"
                strrow &= "<span class=""qty""><input style=""width:45px;margin-right:0px;"" class=""numerictxtbox"" onkeypress=""return isDecimalKey(event);"" id=""txtQty" & i & """ onfocus = ""return focusControl('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" name=""txtQty" & i & """ value=""" & aryDoc(i)(5) & """></span>"
                strrow &= "</td>"

                strrow &= "<td align=""right""  style=""display:none;"">"
                strrow &= "<span class=""unit""><input style=""width:65px;margin-right:0px;"" class=""numerictxtbox""  onkeypress=""return isDecimalKey(event);"" id=""txtUnitPrice" & i & """ onfocus = ""return focusControl('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" name=""txtUnitPrice" & i & """ value=""" & aryDoc(i)(6) & """></span>"
                strrow &= "</td>"
                'Zulham 05102018 - PAMB SST
                strrow &= "<td align=""right"">"
                strrow &= "<span class=""amount""><input style=""width:85px;margin-right:0px; "" onblur=""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');""  class=""numerictxtbox"" id=""txtAmt" & i & """ name=""txtAmt" & i & """ value=""" & aryDoc(i)(7) & """></span>"
                strrow &= "</td>"

                'Zulham Aug 28, 2014
                'Added columns : GST Amount, Input Tax Code, Output Tax Code
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                    If strIsGst = "Yes" Or compType = "E" Then
                        If Not Request.QueryString("isResident") Is Nothing Then
                            If Request.QueryString("isResident") = True Then
                                If aryDoc(i)(20) IsNot Nothing Then
                                    If aryDoc(i)(20) = "R" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Reimbursement
                                        ''GST Amount
                                        If Not aryDoc(i)(22) Is Nothing Then '
                                            If aryDoc(i)(22) = "IM2" Then
                                                GSTAmount = ""
                                                ViewState("IM2") = "Yes"
                                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                                For Each row As DataRow In strPerc
                                                    strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                                Next
                                                If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                    GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                    'Zulham 14042015 IPP GST Stage 1
                                                    GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                                End If
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                                GSTAmount = ""
                                                ViewState("Prize") = "Yes"
                                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                                For Each row As DataRow In strPerc
                                                    strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                                Next
                                                If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                    GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                    'Zulham 14042015 IPP GST Stage 1
                                                    GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                                End If
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            Else
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If

                                        'Predefined tax codes will be default value
                                        Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")
                                        Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")

                                        'Input Tax
                                        If aryDoc(i)(22) Is Nothing Then
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            'If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                            strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                                                    tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        Else
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")

                                            'Zulham 26/04/2018 - PAMB
                                            'Zulham 06/05/2018 - PAMB
                                            'IF POB Foreign Inter-Co & Category = Life then TXRE
                                            If Not aryDoc(i)(1) Is Nothing And Not aryDoc(i)(25) Is Nothing Then
                                                If Not aryDoc(i)(1) = "" And Not aryDoc(i)(25) = "" Then
                                                    Dim strCountry = objDB.GetVal("SELECT ic_country FROM ipp_company WHERE ic_other_b_coy_code = '" & aryDoc(i)(1).ToString.Trim & "' and ic_coy_id ='" & Session("CompanyId") & "'")
                                                    'If aryDoc(i)(25) = "Life" And strCountry <> "MY" Then
                                                    '    For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                    '        If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = "TXRE" Then
                                                    '            tempStr = "TXRE"
                                                    '            'Jules 2018.08.13 - To ensure the value belongs to the displayed text.
                                                    '            'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    '            strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                    '            'End modification.
                                                    '        Else
                                                    '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    '        End If
                                                    '    Next
                                                    'Else
                                                    'Jules 2018.08.13
                                                    'If aryDoc(i)(22).ToString = "Select" Then
                                                    '    strrow &= "<option value=""Select"">---Select---</option>"
                                                    'End If
                                                    If aryDoc(i)(22).ToString = "Select" OrElse aryDoc(i)(22).ToString = "TXRE" Then
                                                            strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                            strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                        End If
                                                        'End modification.
                                                        For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        Next
                                                    'End If
                                                End If
                                            End If
                                            'End

                                            'If aryDoc(i)(22).ToString = "Select" Then
                                            '    strrow &= "<option value=""Select"">---Select---</option>"
                                            'End If
                                            'For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            '    If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                            '        tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                            '        strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                            '    Else
                                            '        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                            '    End If
                                            'Next
                                        End If
                                        strrow &= "</td>" 'Jules 2018.07.13

                                        'Output Tax
                                        If aryDoc(i)(23) Is Nothing Then
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                strrow &= "<td>"
                                                'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                If Not aryDoc(i)(22) Is Nothing Then
                                                    If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    Else
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    End If
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                If Not aryDoc(i)(23) Is Nothing Then
                                                    If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        strrow &= "<option value=""Select"">---Select---</option>"
                                                    End If
                                                    If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                                tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                                strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    Else
                                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                    End If
                                                Else
                                                    'If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    '    strrow &= "<option value=""Select"">---Select---</option>"
                                                    'End If
                                                    'strrow &= "<option value=""Select"">---Select---</option>"
                                                    If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        'For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        '    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        'Next
                                                        'strrow &= "<td>"
                                                        'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                        'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                        If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                                tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                                strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                            ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    Else
                                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                    End If
                                                End If
                                            Else
                                                If Not aryDoc(i)(23) Is Nothing Then
                                                    strrow &= "<td>"
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                    If aryDoc(i)(23).ToString = "Select" Then
                                                        strrow &= "<option value=""Select"">---Select---</option>"
                                                    End If
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                Else
                                                    strrow &= "<td>"
                                                    If VenOutputTaxValue.ToString.Contains("N/A") Then
                                                        strrow &= "<select class=""ddl2"" disabled=""disabled"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    Else
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    End If
                                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                    If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                        ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                End If
                                            End If
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        Else
                                            strrow &= "<td>"
                                            'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                strrow &= "<option value=""Select"">---Select---</option>"
                                            End If
                                            If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "IM2" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = compInputTaxValue_TX4 Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                        tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM2 Then
                                                        strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            ElseIf aryDoc(i)(22).ToString = compInputTaxValue_TX4 Then
                                                For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                                        strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            Else
                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            End If
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        End If

                                    ElseIf aryDoc(i)(20) = "D" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Disbursement
                                        If Not aryDoc(i)(1) = "HLISB" Then
                                            'Zulham 04102018 - pamb SST
                                            'Gst Amount
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                                            strrow &= "</td>"
                                            'Input TaxS
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" disabled=""disabled"" style=""width:110px;margin-right:0px;""  id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            strrow &= "</td>" 'Jules 2018.07.13

                                            'Output Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        Else

                                            ''GST Amount
                                            If Not aryDoc(i)(22) Is Nothing Then '
                                                If aryDoc(i)(22) = "IM2" Then
                                                    GSTAmount = ""
                                                    ViewState("IM2") = "Yes"
                                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                                    For Each row As DataRow In strPerc
                                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                                    Next
                                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                        GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                        'Zulham 14042015 IPP GST Stage 1
                                                        GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                                    End If
                                                    'Zulham 04102018 - PAMB SST
                                                    strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                                    GSTAmount = ""
                                                    ViewState("Prize") = "Yes"
                                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                                    For Each row As DataRow In strPerc
                                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                                    Next
                                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                        GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                        'Zulham 14042015 IPP GST Stage 1
                                                        GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                                    End If
                                                    'Zulham 04102018 - PAMB SST
                                                    strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                Else
                                                    'Zulham 04102018 - PAMB SST
                                                    strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                End If
                                            Else
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If

                                            'Predefined tax codes will be default value
                                            Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")
                                            Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")

                                            'Input Tax
                                            If aryDoc(i)(22) Is Nothing Then
                                                strrow &= "<td>"
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                'If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                                For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                    If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                                                        tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            Else
                                                strrow &= "<td>"
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                If aryDoc(i)(22).ToString = "Select" Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                    If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                                        tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            End If
                                            strrow &= "</td>" 'Jules 2018.07.13

                                            'Output Tax
                                            If aryDoc(i)(23) Is Nothing Then
                                                If Not aryDoc(i)(22) Is Nothing Then
                                                    strrow &= "<td>"
                                                    'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    If Not aryDoc(i)(22) Is Nothing Then
                                                        If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                        Else
                                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                        End If
                                                    Else
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    End If
                                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                    If Not aryDoc(i)(23) Is Nothing Then
                                                        If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                            strrow &= "<option value=""Select"">---Select---</option>"
                                                        End If
                                                        If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                            For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                                If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                                    tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                                    strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                                Else
                                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                                End If
                                                            Next
                                                        Else
                                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                        End If
                                                    Else
                                                        'If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        '    strrow &= "<option value=""Select"">---Select---</option>"
                                                        'End If
                                                        'strrow &= "<option value=""Select"">---Select---</option>"
                                                        If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                            'For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            '    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                            'Next
                                                            'strrow &= "<td>"
                                                            'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                            If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                                    tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                                    strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                                ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                                Else
                                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                                End If
                                                            Next
                                                        Else
                                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                        End If
                                                    End If
                                                    strrow &= "</td>" 'Jules 2018.07.13
                                                Else
                                                    If Not aryDoc(i)(23) Is Nothing Then
                                                        strrow &= "<td>"
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                        'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                        If aryDoc(i)(23).ToString = "Select" Then
                                                            strrow &= "<option value=""Select"">---Select---</option>"
                                                        End If
                                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                                tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                                strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    Else
                                                        strrow &= "<td>"
                                                        If VenOutputTaxValue.ToString.Contains("N/A") Then
                                                            strrow &= "<select class=""ddl2"" disabled=""disabled"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                        Else
                                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                        End If
                                                        'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                        If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                                tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                                strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                            ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    End If
                                                    strrow &= "</td>" 'Jules 2018.07.13
                                                End If
                                            Else
                                                strrow &= "<td>"
                                                'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                If Not aryDoc(i)(22) Is Nothing Then
                                                    If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                                    Else
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    End If
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "IM2" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = compInputTaxValue_TX4 Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM2 Then
                                                            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                ElseIf aryDoc(i)(22).ToString = compInputTaxValue_TX4 Then
                                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                                            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                Else
                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                End If
                                                strrow &= "</td>" 'Jules 2018.07.13
                                            End If

                                        End If
                                    ElseIf aryDoc(i)(1) = "Own Co." Then
                                        'GST Amount
                                        'Zulham 04102018 - PAMB SST
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                        'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        'strrow &= "<option value=""Select"">---Select---</option>" 'Jules commented 2018.08.10

                                        'Zulham 27/04/2018 - PAMB
                                        'Zulham 06/05/2018 - PAMB
                                        If Not aryDoc(i)(25) Is Nothing OrElse aryDoc(i)(8) IsNot Nothing OrElse aryDoc(i)(37) IsNot Nothing Then
                                            'Zulham 0910208 - PAMB SST
                                            'Dim strGLType = ""
                                            'strGLType = objDB.GetVal("SELECT IFNULL(cbg_b_gl_type,"""") 'cbg_b_gl_type' FROM company_b_gl_code  WHERE cbg_b_gl_code = '" & aryDoc(i)(8).ToString.Split(":")(0).ToString & "'")

                                            Dim blnReset As Boolean = False 'Jules 2018.08.10
                                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                'If strGLType = "CAP" Then
                                                '    ViewState("isCAPEX" & i) = True
                                                '    Select Case aryDoc(i)(25).ToString
                                                '        Case "Life"
                                                '            'Jules 2018.08.10 - To reset the tax code.
                                                '            If record = 0 Then
                                                '                If aryDoc(i)(22).ToString.Trim = "TXC1" Then
                                                '                    strrow &= "<option value=""Select"">---Select---</option>"
                                                '                Else
                                                '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                '                End If
                                                '            End If
                                                '            'End modification.
                                                '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXC1" Then
                                                '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                                '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                '            Else
                                                '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                '            End If
                                                '        Case "Non-Life"
                                                '            'Jules 2018.08.10 - To reset the tax code.
                                                '            If record = 0 Then
                                                '                If aryDoc(i)(22).ToString.Trim = "TXC2" Then
                                                '                    strrow &= "<option value=""Select"">---Select---</option>"
                                                '                Else
                                                '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                '                End If
                                                '            End If
                                                '            'End modification.
                                                '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXC2" Then
                                                '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                                '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                '            Else
                                                '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                '            End If
                                                '        Case "Mixed"
                                                '            'Jules 2018.08.10 - To reset the tax code.
                                                '            If record = 0 Then
                                                '                If aryDoc(i)(22).ToString.Trim = "TXCG" Then
                                                '                    strrow &= "<option value=""Select"">---Select---</option>"
                                                '                Else
                                                '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                '                End If
                                                '            End If
                                                '            'End modification.
                                                '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXCG" Then
                                                '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                                '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                '            Else
                                                '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                '            End If
                                                '    End Select
                                                'ElseIf strGLType = "BLC" Then
                                                '    ViewState("isCAPEX" & i) = False
                                                '    Select Case aryDoc(i)(25).ToString
                                                '        Case "Life"
                                                '            'Jules 2018.08.10 - To reset the tax code.
                                                '            If record = 0 Then
                                                '                If aryDoc(i)(22).ToString.Trim = "BK" Then
                                                '                    strrow &= "<option value=""Select"">---Select---</option>"
                                                '                Else
                                                '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                '                End If
                                                '            End If
                                                '            'End modification.
                                                '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "BK" Then
                                                '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                                '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                '            Else
                                                '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                '            End If
                                                '        Case Else
                                                '            'Jules 2018.08.10 - To reset the tax code.
                                                '            If record = 0 Then
                                                '                strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                '            End If
                                                '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                '            'End modification.
                                                '    End Select
                                                'Else
                                                'ViewState("isCAPEX" & i) = False
                                                'Jules 2018.07.09
                                                'If aryDoc(i)(37).ToString = "Yes" And aryDoc(i)(25).ToString = "Life" Then
                                                If Not aryDoc(i)(37) Is Nothing AndAlso Not aryDoc(i)(25) Is Nothing AndAlso aryDoc(i)(37).ToString = "Yes" AndAlso aryDoc(i)(25).ToString = "Life" Then
                                                    'Jules 2018.08.10 - To reset the tax code.
                                                    If record = 0 Then
                                                        If aryDoc(i)(22).ToString.Trim = "FG" Then
                                                            strrow &= "<option value=""Select"">---Select---</option>"
                                                        Else
                                                            strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                            End If
                                                        End If
                                                    'End modification.
                                                    If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "FG" Then
                                                        'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                                        'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                        'End modification.
                                                    Else
                                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Else
                                                    If Not aryDoc(i)(22) Is Nothing Then
                                                        If Not aryDoc(i)(22).ToString.Trim = "" Then
                                                            'Jules 2018.08.10 - To reset the tax code.                                                                
                                                            If record = 0 Then
                                                                If aryDoc(i)(22).ToString.Trim <> "TXC1" AndAlso aryDoc(i)(22).ToString.Trim <> "TXC2" AndAlso aryDoc(i)(22).ToString.Trim <> "TXCG" AndAlso aryDoc(i)(22).ToString.Trim <> "BK" AndAlso
                                                                                aryDoc(i)(22).ToString.Trim <> "FG" AndAlso aryDoc(i)(22).ToString.Trim <> "TXRE" AndAlso aryDoc(i)(22).ToString.Trim <> "TXRE" AndAlso aryDoc(i)(22).ToString.Trim <> "Select" Then
                                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                                Else
                                                                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                                    blnReset = True
                                                                    strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                                End If

                                                            End If
                                                            'End modification.
                                                            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then

                                                                'Jules 2018.08.10
                                                                If blnReset Then
                                                                    strrow &= "<option value=""" & aryDoc(i)(22).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                                Else
                                                                    strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                                End If
                                                                'End modification.
                                                            Else
                                                                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                            End If
                                                        Else
                                                            'Jules 2018.08.10 - To reset the tax code.
                                                            If record = 0 Then
                                                                strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                            End If
                                                            'End modification.
                                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                    Else
                                                        'Zulham 25102018
                                                        If record = 0 Then
                                                            strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                        End If
                                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                End If
                                                'End If
                                            Next
                                        Else
                                            strrow &= "<option value=""Select"">---Select---</option>" 'Jules 2018.08.13
                                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            Next
                                        End If
                                        strrow &= "</td>" 'Jules 2018.07.13

                                        'Output Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                            If Not aryDoc(i)(25) Is Nothing AndAlso aryDoc(i)(37) IsNot Nothing Then
                                                'Zulham 09102018 - PAMB SST
                                                'If aryDoc(i)(37).ToString = "Yes" And aryDoc(i)(25).ToString = "Life" Then
                                                '    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "SR-G" Then
                                                '        strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & "SR-G" & "</option>"
                                                '    Else
                                                '        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                '    End If
                                                'Else
                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                'End If
                                            Else
                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            End If
                                        Next
                                        'Zulham 27112018
                                        If record = 0 Then strrow &= "<option value=""" & 0 & """>N/A</option>"

                                        strrow &= "</td>" 'Jules 2018.07.13
                                        'End

                                    End If
                                Else

                                    'Zulham 29092015 - Set aryDoc(i)(1) to Own Comp if it's HLISB
                                    If Session("CompanyId").ToString.ToUpper = "HLISB" And aryDoc(i)(1) Is Nothing And aryDoc(i)(22) IsNot Nothing Then
                                        aryDoc(i)(1) = "Own Co."
                                    End If

                                    If aryDoc(i)(20) = "R" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Reimbursement
                                        'GST Amount
                                        'Zulham 04102018 - PAMB SST
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                        'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        strrow &= "<option value=""Select"">---Select---</option>"
                                        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Next
                                        strrow &= "</td>" 'Jules 2018.07.13
                                        'Output Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        strrow &= "</td>" 'Jules 2018.07.13
                                    ElseIf aryDoc(i)(20) = "D" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Disbursement
                                        'Gst Amount
                                        'Zulham 04102018 - PAMB SST
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                                        strrow &= "</td>"
                                        'Input TaxS
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """  disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        strrow &= "</td>" 'Jules 2018.07.13
                                        'Output Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        strrow &= "</td>" 'Jules 2018.07.13
                                    ElseIf aryDoc(i)(1) = "Own Co." Then
                                        If aryDoc(i)(22) = "IM2" Then
                                            'GST Amount - Auto Calculated
                                            'Get IM2 rate
                                            GSTAmount = ""
                                            ViewState("IM2") = "Yes"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                'Zulham 14042015 IPP GST Stage 1
                                                GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                            End If
                                            'Zulham 04102018 - PaMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                            'Input Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If dsInputTax.Tables(0).Rows(record).Item(1).ToString = aryDoc(i)(22) Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                                'strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            Next
                                            strrow &= "</td>" 'Jules 2018.07.13
                                            'Output Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            'strrow &= "<option value=""" & compOutputTaxValue & """>" & compOutputTaxValue & "</option>"
                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM2 Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                                'strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            Next
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                            'GST Amount - Auto Calculated
                                            'Get IM2 rate
                                            GSTAmount = ""
                                            ViewState("Prize") = "Yes"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                'Zulham 14042015 IPP GST Stage 1
                                                GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                            End If
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                            'Input Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If dsInputTax.Tables(0).Rows(record).Item(1).ToString = aryDoc(i)(22) Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                                'strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            Next
                                            strrow &= "</td>" 'Jules 2018.07.13
                                            'Output Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            'strrow &= "<option value=""" & compOutputTaxValue & """>" & compOutputTaxValue & "</option>"
                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                                'strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            Next
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        Else
                                            'Zulham 25/02/2016 - IPP IM5/IM6 Enhancement
                                            'Removed auto-calculation for im5
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If Not aryDoc(i)(22).ToString.Trim = "IM5" Then
                                                    'Zulham 14/01/2016 - IPP Stage 4 Phase 2
                                                    'All tax codes would have the respective GST amount to be auto-calculated
                                                    'GST amount will be disabled
                                                    If Not aryDoc.Item(i)(5) = "" And Not aryDoc.Item(i)(6) = "" Then
                                                        Dim percentage = GST.getTaxPercentage(aryDoc(i)(22).ToString.Trim)
                                                        If Not percentage.ToString.Trim.Length = 0 Then
                                                            GSTAmount = (percentage / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                            'Zulham 14042015 IPP GST Stage 1
                                                            GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                                        Else
                                                            GSTAmount = ""
                                                        End If
                                                    Else
                                                        GSTAmount = ""
                                                    End If
                                                ElseIf aryDoc(i)(22).ToString.Trim = "IM5" Then
                                                    ViewState("IM2") = ""
                                                    ViewState("Prize") = ""
                                                    If aryDoc(i)(21).ToString.Trim.Length = 0 Then GSTAmount = "" Else GSTAmount = aryDoc(i)(21)
                                                End If
                                            Else
                                                ViewState("IM2") = ""
                                                ViewState("Prize") = ""
                                                If aryDoc(i)(21).ToString.Trim.Length = 0 Then GSTAmount = "" Else GSTAmount = aryDoc(i)(21)
                                            End If

                                            'GST Amount
                                            'Zulham 04102018 - PAMB SST
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If ViewState("Prize") = "Yes" Or ViewState("IM2") = "Yes" Then
                                                    strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly=""readonly""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """ value=""" & GSTAmount & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                ElseIf aryDoc(i)(22).ToString.Trim = "IM5" Then
                                                    strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" class=""numerictxtbox"" id=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                Else
                                                    strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly=""readonly"" class=""numerictxtbox"" id=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                End If
                                            Else
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly=""readonly"" class=""numerictxtbox"" id=""txtGSTAmount" & i & """ value="""" onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value="""" onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If


                                            'Input Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If aryDoc(i)(22).ToString = "Select" Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                    If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                                        tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            Else
                                                strrow &= "<option value=""Select"">---Select---</option>"
                                                For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                Next
                                            End If
                                            strrow &= "</td>" 'Jules 2018.07.13
                                            'Output Tax
                                            strrow &= "<td>"
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If Request.QueryString("isResident") = True Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            If Not aryDoc(i)(23) Is Nothing Then
                                                'Zulham 04/03/2016 - im5/im6 
                                                'Added condiotn for im5
                                                If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" _
                                                Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Or aryDoc(i)(22).ToString.Trim = "IM5") Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                If Not (Request.QueryString("isResident") = True Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                ElseIf aryDoc(i)(22).ToString.Trim = "IM5" Then
                                                    'Zulham 04/03/2016 - IM5/IM6 Enhancement
                                                    'Hardcoded IM6 selection
                                                    'strrow &= "<option selected=""selected"" value=""" & "IM6" & """>" & "IM6 (6%)" & "</option>"
                                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = "IM6" Then
                                                            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                Else
                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                End If
                                            Else
                                                If Not aryDoc(i)(22) Is Nothing Then
                                                    If Not (Request.QueryString("isResident") = True Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        strrow &= "<option value=""Select"">---Select---</option>"
                                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        Next
                                                    ElseIf aryDoc(i)(22).ToString.Trim = "IM5" Then
                                                        'Zulham 04/03/2016 - IM5/IM6 Enhancement
                                                        'Hardcoded IM6 selection
                                                        'strrow &= "<option selected=""selected"" value=""" & "IM6" & """>" & "IM6 (6%)" & "</option>"
                                                        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM5 Then
                                                                strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    Else
                                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                    End If
                                                Else
                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                End If
                                            End If
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        End If
                                    ElseIf aryDoc(i)(1) = Nothing Then 'Initially

                                        'Jules 2018.07.09 - Allow "\" and "#"
                                        'Zulham 29052015 IPP GST Stage 1
                                        'Check for document dated before cut off date
                                        Dim documentDate = objDB.GetVal("SELECT IFNULL(im_doc_date,'') 'im_doc_date' FROM invoice_mstr WHERE im_invoice_no = '" & Common.Parse(Replace(Replace(Request.QueryString("docno"), "\", "\\"), "#", "\#")) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                                        Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")

                                        If CDate(documentDate) < CDate(_cutoffDate) Then
                                            'Gst Amount
                                            'strrow &= "<td align=""right"">"
                                            'strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                                            'strrow &= "</td>"
                                            'Zulham 02062015 IPP GST Stage 1
                                            'Changed the GST Amount value to 0.00 from N/A
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "0.00" & """ ></span>"
                                            strrow &= "</td>"
                                            'Input TaxS
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """  disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            strrow &= "</td>" 'Jules 2018.07.13
                                            'Output Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        Else
                                            'Predefined tax codes will be default value
                                            Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")
                                            Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")

                                            'Zulham 06052015 IPP GST Stage 1
                                            If VenInputTaxValue.ToString.Trim = "TX4" Then
                                                VenOutputTaxValue = compOutputTaxValue_TX4
                                            End If

                                            'zulham 24/02/2016 - IM5/IM6 Changes
                                            'GST Amount
                                            'Zulham 04102018 - PAMB SST
                                            If Not VenInputTaxValue.ToString.Trim = "IM5" Then
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(7) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            ElseIf VenInputTaxValue.ToString.Trim = "IM5" Then
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(7) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If


                                            'Input Tax
                                            strrow &= "<td>"

                                            'Jules 2018.07.09
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return RecalculateGSTafterGLCode('" & i & "','txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"">"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            'If VenInputTaxValue.ToString = "0" And Not compType = "E" Then strrow &= "<option value=""Select"">---Select---</option>"
                                            strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                                                    tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                            strrow &= "</td>" 'Jules 2018.07.13
                                            'Output Tax
                                            strrow &= "<td>"
                                            If VenOutputTaxValue.ToString.Contains("N/A") Or compType = "E" Or Request.QueryString("isResident") Then
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            If VenOutputTaxValue.ToString = "0" And Not compType = "E" Then strrow &= "<option value=""Select"">---Select---</option>"
                                            'Zulham 07112018
                                            If Not dsOutputTax.Tables(0).Rows.Count = 0 Then
                                                For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue And Not compType = "E" Then
                                                        tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                    ElseIf VenOutputTaxValue.ToString.Contains("N/A") Or compType = "E" Then
                                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            Else
                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            End If
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        End If
                                    Else 'PoB selected
                                        'Zulham 25102018
                                        'If Not aryDoc(i)(1) = "HLISB" Then
                                        '    'Zulham 04102018 - PAMB SST
                                        '    'GST Amount
                                        '    strrow &= "<td align=""right"">"
                                        '    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" disabled=""disabled"" name=""txtGSTAmount" & i & """ value=""N/A"" onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        '    strrow &= "</td>"
                                        '    'Input Tax
                                        '    strrow &= "<td>"
                                        '    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                        '    strrow &= "<option value=""" & 0 & """>" & "N/A" & "</option>"
                                        '    strrow &= "</td>" 'Jules 2018.07.13
                                        '    'Output Tax
                                        '    strrow &= "<td>"
                                        '    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                        '    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        '    strrow &= "</td>" 'Jules 2018.07.13
                                        'Else
                                        'Start
                                        ''GST Amount
                                        If Not aryDoc(i)(22) Is Nothing Then '
                                                If aryDoc(i)(22) = "IM2" Then
                                                    GSTAmount = ""
                                                    ViewState("IM2") = "Yes"
                                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                                    For Each row As DataRow In strPerc
                                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                                    Next
                                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                        GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                        'Zulham 14042015 IPP GST Stage 1
                                                        GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                                    End If
                                                    'Zulham 04102018 - PAMB SST
                                                    strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                                    GSTAmount = ""
                                                    ViewState("Prize") = "Yes"
                                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                                    For Each row As DataRow In strPerc
                                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                                    Next
                                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                        GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                        'Zulham 14042015 IPP GST Stage 1
                                                        GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                                    End If
                                                    'Zulham 04102018 - PAMB SST
                                                    strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                Else
                                                    'Zulham 04102018 - PAMB SST
                                                    strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                End If
                                            Else
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If

                                            'Predefined tax codes will be default value
                                            Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")
                                            Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")

                                            'Input Tax
                                            If aryDoc(i)(22) Is Nothing Then
                                                strrow &= "<td>"
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                'If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                                For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                    If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                                                        tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            Else
                                                strrow &= "<td>"
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                If aryDoc(i)(22).ToString = "Select" Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                    If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                                        tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            End If
                                            strrow &= "</td>" 'Jules 2018.07.13

                                            'Output Tax
                                            If aryDoc(i)(23) Is Nothing Then
                                                If Not aryDoc(i)(22) Is Nothing Then
                                                    strrow &= "<td>"
                                                    'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    If Not aryDoc(i)(22) Is Nothing Then
                                                        If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                        Else
                                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                        End If
                                                    Else
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    End If
                                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                    If Not aryDoc(i)(23) Is Nothing Then
                                                        If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                            strrow &= "<option value=""Select"">---Select---</option>"
                                                        End If
                                                        If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                            For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                                If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                                    tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                                    strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                                Else
                                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                                End If
                                                            Next
                                                        Else
                                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                        End If
                                                    Else
                                                        'If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        '    strrow &= "<option value=""Select"">---Select---</option>"
                                                        'End If
                                                        'strrow &= "<option value=""Select"">---Select---</option>"
                                                        If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                            'For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            '    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                            'Next
                                                            'strrow &= "<td>"
                                                            'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                            If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                                    tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                                    strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                                ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                                Else
                                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                                End If
                                                            Next
                                                        Else
                                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                        End If
                                                    End If
                                                    strrow &= "</td>" 'Jules 2018.07.13
                                                Else
                                                    If Not aryDoc(i)(23) Is Nothing Then
                                                        strrow &= "<td>"
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                        'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                        If aryDoc(i)(23).ToString = "Select" Then
                                                            strrow &= "<option value=""Select"">---Select---</option>"
                                                        End If
                                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                                tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                                strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    Else
                                                        strrow &= "<td>"
                                                        If VenOutputTaxValue.ToString.Contains("N/A") Then
                                                            strrow &= "<select class=""ddl2"" disabled=""disabled"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                        Else
                                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                        End If
                                                        'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                        If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                                tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                                strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                            ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    End If
                                                    strrow &= "</td>" 'Jules 2018.07.13
                                                End If
                                            Else
                                                strrow &= "<td>"
                                                'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                If Not aryDoc(i)(22) Is Nothing Then
                                                    If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                                    Else
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    End If
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "IM2" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = compInputTaxValue_TX4 Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM2 Then
                                                            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                ElseIf aryDoc(i)(22).ToString = compInputTaxValue_TX4 Then
                                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                                            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                Else
                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                End If
                                                strrow &= "</td>" 'Jules 2018.07.13
                                            End If
                                        'End

                                        'End If
                                    End If

                                End If
                            Else 'Non-Resident

                                'Zulham 06112015 - Set ary(i)(1) to own company if the login company is hlisb
                                If Common.Parse(Session("CompanyID")).ToUpper = "HLISB" _
                                And Not aryDoc(i)(22) Is Nothing Then
                                    aryDoc(i)(1) = "Own Co."
                                End If


                                If aryDoc(i)(20) = "R" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Reimbursement
                                    If Not aryDoc(i)(22) Is Nothing Then
                                        If aryDoc(i)(22) = "IM1" Or aryDoc(i)(22) = "IM3" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                            'GST Amount
                                            If ViewState("IM2") IsNot Nothing Then
                                                If ViewState("IM2") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                                    'Zulham 26092018 - PAMB SST
                                                    If i = ViewState("Row") Then
                                                        strrow &= "<td align=""right"">"
                                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                        strrow &= "</td>"
                                                        ViewState("Row") = ""
                                                        ViewState("IM2") = ""
                                                    Else
                                                        strrow &= "<td align=""right"">"
                                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                        strrow &= "</td>"
                                                    End If
                                                Else
                                                    strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                End If
                                            ElseIf ViewState("Prize") IsNot Nothing Then
                                                'Zulham 04102018 - PAMB SST
                                                If ViewState("Prize") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                                    If i = ViewState("Row") Then
                                                        strrow &= "<td align=""right"">"
                                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                        strrow &= "</td>"
                                                        ViewState("Row") = ""
                                                        ViewState("Prize") = ""
                                                    Else
                                                        strrow &= "<td align=""right"">"
                                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                        strrow &= "</td>"
                                                    End If
                                                Else
                                                    strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                End If
                                            Else
                                                'Zulham 26092018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If

                                            'Input Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If dsInputTax.Tables(0).Rows(record).Item(1).ToString = aryDoc(i)(22) Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                            strrow &= "</td>" 'Jules 2018.07.13
                                            'Output Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        ElseIf aryDoc(i)(22) = "IM2" Then
                                            'GST Amount - Auto Calculated
                                            'Get IM2 rate
                                            GSTAmount = ""
                                            ViewState("IM2") = "Yes"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                'Zulham 14042015 IPP GST Stage 1
                                                GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                            End If
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                            'Input Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If dsInputTax.Tables(0).Rows(record).Item(1).ToString = aryDoc(i)(22) Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                                'strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            Next
                                            strrow &= "</td>" 'Jules 2018.07.13
                                            'Output Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            'strrow &= "<option value=""" & compOutputTaxValue & """>" & compOutputTaxValue & "</option>"
                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM2 Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                                'strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            Next
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                            'GST Amount - Auto Calculated
                                            'Get IM2 rate
                                            GSTAmount = ""
                                            ViewState("Prize") = "Yes"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                'Zulham 14042015 IPP GST Stage 1
                                                GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                            End If
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                            'Input Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If dsInputTax.Tables(0).Rows(record).Item(1).ToString = aryDoc(i)(22) Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                                'strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            Next
                                            strrow &= "</td>" 'Jules 2018.07.13
                                            'Output Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            'strrow &= "<option value=""" & compOutputTaxValue & """>" & compOutputTaxValue & "</option>"
                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                                'strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            Next
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        Else 'reimbursement selected
                                            'GST Amount
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"

                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")

                                            'Zulham 26/04/2018 - PAMB
                                            'Zulham 06/05/2018 - PAMB
                                            'IF POB Foreign Inter-Co & Category = Life then TXRE
                                            If Not aryDoc(i)(1) Is Nothing And Not aryDoc(i)(25) Is Nothing Then
                                                If Not aryDoc(i)(1) = "" And Not aryDoc(i)(25) = "" Then
                                                    'Zulham 09102018 - PAMB SST
                                                    'Dim strCountry = objDB.GetVal("SELECT ic_country FROM ipp_company WHERE ic_other_b_coy_code = '" & aryDoc(i)(1).ToString.Trim & "' and ic_coy_id ='" & Session("CompanyId") & "'")
                                                    'If aryDoc(i)(25) = "Life" And strCountry <> "MY" Then
                                                    '    For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                    '        If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = "TXRE" Then
                                                    '            tempStr = "TXRE"
                                                    '            'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                                    '            'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    '            strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                    '            'End modification.
                                                    '        Else
                                                    '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    '        End If
                                                    '    Next
                                                    'Else
                                                    'Jules 2018.08.13
                                                    'If aryDoc(i)(22).ToString = "Select" Then
                                                    '    strrow &= "<option value=""Select"">---Select---</option>"
                                                    'End If
                                                    If aryDoc(i)(22).ToString = "Select" OrElse aryDoc(i)(22).ToString = "TXRE" Then
                                                            strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                        strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"  'Jules 2018.08.13
                                                    End If
                                                    'End modification.
                                                    For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    Next
                                                    'End If
                                                End If
                                            End If
                                            'End
                                            strrow &= "</td>" 'Jules 2018.07.13
                                            'Output Tax
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        End If
                                    Else

                                        'Predefined tax codes will be default value
                                        Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")
                                        Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")
                                        'Zulham 04102018 - PAMB SST
                                        'GST Amount
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(7) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"

                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                        'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        'If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                        strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                                                tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                            Else
                                                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            End If
                                        Next
                                        strrow &= "</td>" 'Jules 2018.07.13
                                        'Output Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                        'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                        If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                            If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                            Else
                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            End If
                                        Next
                                        strrow &= "</td>" 'Jules 2018.07.13
                                    End If
                                ElseIf aryDoc(i)(20) = "D" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Disbursement
                                    If Not aryDoc(i)(1).ToString.ToUpper = "HLISB" Then
                                        'Gst Amount
                                        'Zulham 04102018 - PAMB SST
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                                        strrow &= "</td>"
                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        strrow &= "</td>" 'Jules 2018.07.13
                                        'Output Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        strrow &= "</td>" 'Jules 2018.07.13
                                    Else

                                        'Start
                                        ''GST Amount
                                        If Not aryDoc(i)(22) Is Nothing Then '
                                            If aryDoc(i)(22) = "IM2" Then
                                                GSTAmount = ""
                                                ViewState("IM2") = "Yes"
                                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                                For Each row As DataRow In strPerc
                                                    strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                                Next
                                                If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                    GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                    'Zulham 14042015 IPP GST Stage 1
                                                    GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                                End If
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                                GSTAmount = ""
                                                ViewState("Prize") = "Yes"
                                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                                For Each row As DataRow In strPerc
                                                    strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                                Next
                                                If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                    GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                    'Zulham 14042015 IPP GST Stage 1
                                                    GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                                End If
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            Else
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If

                                        'Predefined tax codes will be default value
                                        Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")
                                        Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")

                                        'Input Tax
                                        If aryDoc(i)(22) Is Nothing Then
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            'If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                            strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                                                    tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        Else
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            If aryDoc(i)(22).ToString = "Select" Then
                                                strrow &= "<option value=""Select"">---Select---</option>"
                                            End If
                                            For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                                    tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        End If
                                        strrow &= "</td>" 'Jules 2018.07.13
                                        'Output Tax
                                        If aryDoc(i)(23) Is Nothing Then
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                strrow &= "<td>"
                                                'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                If Not aryDoc(i)(22) Is Nothing Then
                                                    If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    Else
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    End If
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                If Not aryDoc(i)(23) Is Nothing Then
                                                    If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        strrow &= "<option value=""Select"">---Select---</option>"
                                                    End If
                                                    If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                                tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                                strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    Else
                                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                    End If
                                                Else
                                                    'If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    '    strrow &= "<option value=""Select"">---Select---</option>"
                                                    'End If
                                                    'strrow &= "<option value=""Select"">---Select---</option>"
                                                    If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        'For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        '    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        'Next
                                                        'strrow &= "<td>"
                                                        'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                        'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                        If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                                tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                                strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                            ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    Else
                                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                    End If
                                                End If
                                            Else
                                                If Not aryDoc(i)(23) Is Nothing Then
                                                    strrow &= "<td>"
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                    If aryDoc(i)(23).ToString = "Select" Then
                                                        strrow &= "<option value=""Select"">---Select---</option>"
                                                    End If
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                Else
                                                    strrow &= "<td>"
                                                    If VenOutputTaxValue.ToString.Contains("N/A") Then
                                                        strrow &= "<select class=""ddl2"" disabled=""disabled"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    Else
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    End If
                                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                    If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                        ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                End If
                                            End If
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        Else
                                            strrow &= "<td>"
                                            'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                strrow &= "<option value=""Select"">---Select---</option>"
                                            End If
                                            If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "IM2" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = compInputTaxValue_TX4 Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                        tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM2 Then
                                                        strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            ElseIf aryDoc(i)(22).ToString = compInputTaxValue_TX4 Then
                                                For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                                        strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            Else
                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            End If
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        End If
                                        'End

                                    End If
                                ElseIf aryDoc(i)(1) = "Own Co." Then
                                    'GST Amount
                                    'Zulham 04102018 - PAMB SST
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(7) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'strrow &= "<option value=""Select"">---Select---</option>" 'Jules commented 2018.08.10

                                    'Zulham 27/04/2018 - PAMB
                                    'Zulham 06/05/2018 - PAMB
                                    If Not aryDoc(i)(25) Is Nothing OrElse aryDoc(i)(8) IsNot Nothing OrElse aryDoc(i)(37) IsNot Nothing Then
                                        'Zulham 09102018 - PAMB SST
                                        'Dim strGLType = ""
                                        'strGLType = objDB.GetVal("SELECT IFNULL(cbg_b_gl_type,"""") 'cbg_b_gl_type' FROM company_b_gl_code  WHERE cbg_b_gl_code = '" & aryDoc(i)(8).ToString.Split(":")(0).ToString & "'")

                                        Dim blnReset As Boolean = False 'Jules 2018.08.10
                                        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            'If strGLType = "CAP" Then
                                            '    ViewState("isCAPEX" & i) = True
                                            '    Select Case aryDoc(i)(25).ToString
                                            '        Case "Life"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "TXC1" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXC1" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '        Case "Non-Life"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "TXC2" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXC2" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '        Case "Mixed"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "TXCG" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXCG" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '    End Select
                                            'ElseIf strGLType = "BLC" Then
                                            '    ViewState("isCAPEX" & i) = False
                                            '    Select Case aryDoc(i)(25).ToString
                                            '        Case "Life"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "BK" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "BK" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '        Case Else
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            End If
                                            '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            'End modification.
                                            '    End Select
                                            'Else
                                            'ViewState("isCAPEX" & i) = False
                                            'If aryDoc(i)(37).ToString = "Yes" And aryDoc(i)(25).ToString = "Life" Then
                                            '    'Jules 2018.08.10 - To reset the tax code.
                                            '    If record = 0 Then
                                            '        If aryDoc(i)(22).ToString.Trim = "FG" Then
                                            '            strrow &= "<option value=""Select"">---Select---</option>"
                                            '        Else
                                            '            strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '        End If
                                            '    End If
                                            '    'End modification.
                                            '    If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "FG" Then
                                            '        'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '        'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '        strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '    Else
                                            '        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '    End If
                                            'Else
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If Not aryDoc(i)(22).ToString.Trim = "" Then
                                                    'Jules 2018.08.10 - To reset the tax code.                                                                
                                                    If record = 0 Then
                                                        If aryDoc(i)(22).ToString.Trim <> "TXC1" AndAlso aryDoc(i)(22).ToString.Trim <> "TXC2" AndAlso aryDoc(i)(22).ToString.Trim <> "TXCG" AndAlso aryDoc(i)(22).ToString.Trim <> "BK" AndAlso
                                                                                aryDoc(i)(22).ToString.Trim <> "FG" AndAlso aryDoc(i)(22).ToString.Trim <> "TXRE" AndAlso aryDoc(i)(22).ToString.Trim <> "Select" Then
                                                            strrow &= "<option value=""Select"">---Select---</option>"
                                                        Else
                                                            strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                            blnReset = True
                                                            strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                        End If

                                                    End If
                                                    'End modification.
                                                    If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then

                                                        'Jules 2018.08.10
                                                        If blnReset Then
                                                            strrow &= "<option value=""" & aryDoc(i)(22).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                        'End modification.
                                                    Else
                                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Else
                                                    'Jules 2018.08.10 - To reset the tax code.
                                                    If record = 0 Then
                                                        strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                        strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                    End If
                                                    'End modification.
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            End If
                                            'End If
                                            'End If
                                        Next
                                    Else
                                        strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Next
                                    End If
                                    strrow &= "</td>" 'Jules 2018.07.13

                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                        If Not aryDoc(i)(25) Is Nothing AndAlso aryDoc(i)(37) IsNot Nothing Then
                                            If aryDoc(i)(37).ToString = "Yes" And aryDoc(i)(25).ToString = "Life" Then
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "SR-G" Then
                                                    'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                                    'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & "SR-G" & "</option>"
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim & """ selected=""selected"">" & "SR-G" & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                End If
                                            Else
                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            End If
                                        Else
                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        End If
                                    Next
                                    'End
                                    strrow &= "</td>" 'Jules 2018.07.13
                                ElseIf aryDoc(i)(1) = Nothing Then 'Initially
                                    'GST Amount
                                    'Zulham 04102018 - PAMB SST
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(7) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                    'strrow &= "<option value=""Select"">---Select---</option>" 'Jules 2018.08.10 commented.

                                    'Zulham 27/04/2018 - PAMB
                                    'Zulham 06/05/2018 - PAMB
                                    If Not aryDoc(i)(25) Is Nothing OrElse aryDoc(i)(8) IsNot Nothing OrElse aryDoc(i)(37) IsNot Nothing Then
                                        'Zulham 09102018 - PAMB SST
                                        'Dim strGLType = ""
                                        'strGLType = objDB.GetVal("SELECT IFNULL(cbg_b_gl_type,"""") 'cbg_b_gl_type' FROM company_b_gl_code  WHERE cbg_b_gl_code = '" & aryDoc(i)(8).ToString.Split(":")(0).ToString & "'")

                                        Dim blnReset As Boolean = False 'Jules 2018.08.10
                                        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            'If strGLType = "CAP" Then
                                            '    ViewState("isCAPEX" & i) = True
                                            '    Select Case aryDoc(i)(25).ToString
                                            '        Case "Life"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "TXC1" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXC1" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '        Case "Non-Life"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "TXC2" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXC2" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '        Case "Mixed"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "TXCG" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "TXCG" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '    End Select
                                            'ElseIf strGLType = "BLC" Then
                                            '    ViewState("isCAPEX" & i) = False
                                            '    Select Case aryDoc(i)(25).ToString
                                            '        Case "Life"
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                If aryDoc(i)(22).ToString.Trim = "BK" Then
                                            '                    strrow &= "<option value=""Select"">---Select---</option>"
                                            '                Else
                                            '                    strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                End If
                                            '            End If
                                            '            'End modification.
                                            '            If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "BK" Then
                                            '                'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '                'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            Else
                                            '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            End If
                                            '        Case Else
                                            '            'Jules 2018.08.10 - To reset the tax code.
                                            '            If record = 0 Then
                                            '                strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '                strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '            End If
                                            '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '            'End modification.
                                            '    End Select
                                            'Else
                                            'ViewState("isCAPEX" & i) = False
                                            'If aryDoc(i)(37).ToString = "Yes" And aryDoc(i)(25).ToString = "Life" Then
                                            '    'Jules 2018.08.10 - To reset the tax code.
                                            '    If record = 0 Then
                                            '        If aryDoc(i)(22).ToString.Trim = "FG" Then
                                            '            strrow &= "<option value=""Select"">---Select---</option>"
                                            '        Else
                                            '            strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                            '        End If
                                            '    End If
                                            '    'End modification.
                                            '    If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "FG" Then
                                            '        'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                            '        'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '        strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                            '    Else
                                            '        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            '    End If
                                            'Else
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If Not aryDoc(i)(22).ToString.Trim = "" Then
                                                    'Jules 2018.08.10 - To reset the tax code.                                                                
                                                    If record = 0 Then
                                                        If aryDoc(i)(22).ToString.Trim <> "TXC1" AndAlso aryDoc(i)(22).ToString.Trim <> "TXC2" AndAlso aryDoc(i)(22).ToString.Trim <> "TXCG" AndAlso aryDoc(i)(22).ToString.Trim <> "BK" AndAlso
                                                                                aryDoc(i)(22).ToString.Trim <> "FG" AndAlso aryDoc(i)(22).ToString.Trim <> "TXRE" AndAlso aryDoc(i)(22).ToString.Trim <> "Select" Then
                                                            strrow &= "<option value=""Select"">---Select---</option>"
                                                        Else
                                                            strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                            blnReset = True
                                                            strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                        End If

                                                    End If
                                                    'End modification.
                                                    If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then

                                                        'Jules 2018.08.10
                                                        If blnReset Then
                                                            strrow &= "<option value=""" & aryDoc(i)(22).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                        'End modification.
                                                    Else
                                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Else
                                                    'Jules 2018.08.10 - To reset the tax code.
                                                    If record = 0 Then
                                                        strrow &= "<option value=""Select"" selected=""selected"">---Select---</option>"
                                                        strLiteralScript &= "calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"
                                                    End If
                                                    'End modification.
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            End If
                                            'End If
                                            'End If
                                        Next
                                    Else
                                        strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Next
                                    End If
                                    strrow &= "</td>" 'Jules 2018.07.13

                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    'Zulham 07112018
                                    If dsOutputTax.Tables(0).Rows.Count > 0 Then
                                        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                            If Not aryDoc(i)(25) Is Nothing AndAlso aryDoc(i)(37) IsNot Nothing Then
                                                If aryDoc(i)(37).ToString = "Yes" And aryDoc(i)(25).ToString = "Life" Then
                                                    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = "SR-G" Then
                                                        'Jules 2018.07.10 - To ensure the value belongs to the displayed text.
                                                        'strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & "SR-G" & "</option>"
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim & """ selected=""selected"">" & "SR-G" & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                    End If
                                                Else
                                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                End If
                                            Else
                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            End If
                                        Next
                                    Else
                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                    End If
                                    'End
                                    strrow &= "</td>" 'Jules 2018.07.13
                                ElseIf aryDoc(i)(1) <> "Own Co." And aryDoc(i)(20) = Nothing Then 'Selected Comp + Disbursement would be selected first
                                    If Not aryDoc(i)(1).ToString.ToUpper = "HLISB" Then
                                        'GST Amount
                                        'Zulham 04102018 - PAMB SST
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """ name=""txtGSTAmount" & i & """ value=""N/A"" disabled=""disabled""></span>"
                                        strrow &= "</td>"
                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"" disabled=""disabled"">"
                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        strrow &= "</td>" 'Jules 2018.07.13
                                        'Output Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                        strrow &= "</td>" 'Jules 2018.07.13
                                    Else

                                        'Start
                                        ''GST Amount
                                        If Not aryDoc(i)(22) Is Nothing Then '
                                            If aryDoc(i)(22) = "IM2" Then
                                                GSTAmount = ""
                                                ViewState("IM2") = "Yes"
                                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                                For Each row As DataRow In strPerc
                                                    strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                                Next
                                                If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                    GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                    'Zulham 14042015 IPP GST Stage 1
                                                    GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                                End If
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; "" readonly  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                                GSTAmount = ""
                                                ViewState("Prize") = "Yes"
                                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                                Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                                For Each row As DataRow In strPerc
                                                    strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                                Next
                                                If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                    GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                    'Zulham 14042015 IPP GST Stage 1
                                                    GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                                End If
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            Else
                                                'Zulham 04102018 - PAMB SST
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            'Zulham 04102018 - PAMB SST
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If

                                        'Predefined tax codes will be default value
                                        Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")
                                        Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")

                                        'Input Tax
                                        If aryDoc(i)(22) Is Nothing Then
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            'If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                            strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                            For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                                                    tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        Else
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                            'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            If aryDoc(i)(22).ToString = "Select" Then
                                                strrow &= "<option value=""Select"">---Select---</option>"
                                            End If
                                            For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                                If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                                    tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        End If
                                        strrow &= "</td>" 'Jules 2018.07.139
                                        'Output Tax
                                        If aryDoc(i)(23) Is Nothing Then
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                strrow &= "<td>"
                                                If Not aryDoc(i)(22) Is Nothing Then
                                                    If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    Else
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    End If
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                                'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                If Not aryDoc(i)(23) Is Nothing Then
                                                    If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        strrow &= "<option value=""Select"">---Select---</option>"
                                                    End If
                                                    If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                                tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                                strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    Else
                                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                    End If
                                                Else
                                                    If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                        'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                        If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                                tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                                strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                            ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                            Else
                                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                            End If
                                                        Next
                                                    Else
                                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                    End If
                                                End If
                                                strrow &= "</td>" 'Jules 2018.07.13
                                            Else
                                                If Not aryDoc(i)(23) Is Nothing Then
                                                    strrow &= "<td>"
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                    If aryDoc(i)(23).ToString = "Select" Then
                                                        strrow &= "<option value=""Select"">---Select---</option>"
                                                    End If
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                Else
                                                    strrow &= "<td>"
                                                    If VenOutputTaxValue.ToString.Contains("N/A") Then
                                                        strrow &= "<select class=""ddl2"" disabled=""disabled"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    Else
                                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    End If
                                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                    If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                        ElseIf VenOutputTaxValue.ToString.Contains("N/A") Then
                                                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                                                        Else
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                        End If
                                                    Next
                                                End If
                                                strrow &= "</td>" 'Jules 2018.07.13
                                            End If
                                        Else
                                            strrow &= "<td>"
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                strrow &= "<option value=""Select"">---Select---</option>"
                                            End If
                                            If Not (VenOutputTaxValue.ToString.Contains("N/A") Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "IM2" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = compInputTaxValue_TX4 Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                        tempStr = dsOutputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                                        strrow &= "<option value=""" & aryDoc(i)(23).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            ElseIf aryDoc(i)(22).ToString = "IM2" Then
                                                For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_IM2 Then
                                                        strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            ElseIf aryDoc(i)(22).ToString = compInputTaxValue_TX4 Then
                                                For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                                        strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    Else
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                    End If
                                                Next
                                            Else
                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            End If
                                            strrow &= "</td>" 'Jules 2018.07.13
                                        End If
                                        'End

                                    End If
                                End If
                            End If
                        End If
                    Else

                        'Jules 2018.07.09 - Allow "\" and "#"
                        'Check for document dated before cut off date
                        Dim documentDate = objDB.GetVal("SELECT IFNULL(im_doc_date,'') 'im_doc_date' FROM invoice_mstr WHERE im_invoice_no = '" & Common.Parse(Replace(Replace(Request.QueryString("docno"), "\", "\\"), "#", "\#")) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                        Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")

                        If CDate(documentDate) < CDate(_cutoffDate) Then

                            'Gst Amount
                            'strrow &= "<td align=""right"">"
                            'strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                            'strrow &= "</td>"
                            'Zulham 02062015 IPP GST Stage 1
                            'Changed the GST Amount to 0.00 from N/A
                            'Zulham 04102018 - PAMB SST
                            strrow &= "<td align=""right"">"
                            strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "0.00" & """></span>"
                            strrow &= "</td>"
                            'Input TaxS
                            strrow &= "<td>"
                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """  disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                            strrow &= "</td>" 'Jules 2018.07.13
                            'Output Tax
                            strrow &= "<td>"
                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                            strrow &= "<option value=""" & 0 & """>N/A</option>"
                            strrow &= "</td>" 'Jules 2018.07.13
                        Else
                            'Predefined tax codes will be default value
                            Dim VenInputTaxValue = "NR" 'objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")
                            Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))) & "'")

                            'GST Amount
                            If aryDoc(i)(22) IsNot Nothing Then
                                If aryDoc(i)(22).ToString.Contains("IM2") Then
                                    GSTAmount = ""
                                    ViewState("IM2") = "Yes"
                                    ViewState("Row") = i
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                    For Each row As DataRow In strPerc
                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                    Next
                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" And Not aryDoc.Item(i)(5).ToString.Trim = "" Then
                                        GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                        'Zulham 14042015 IPP GST Stage 1
                                        GSTAmount = FormatNumber(CDbl(GSTAmount), 2)
                                    End If
                                    'Zulham 04102018 - PAMB SST
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                Else
                                    'Zulham 04102018 - PAMB SST
                                    If aryDoc(i)(21) Is Nothing Then
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    Else
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If
                                End If
                            Else
                                'Zulham 26092018 - PAMB SST
                                If aryDoc(i)(21) Is Nothing Then
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                Else
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                End If
                            End If

                            'Input Tax
                            If aryDoc(i)(22) Is Nothing Then
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                'If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                    If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                                        tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                        strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                    Else
                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    End If
                                Next
                                strrow &= "</td>" 'Jules 2018.07.13
                            Else
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur =""onClick('" & i & "');"">"
                                'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                strrow &= "<option value = ""Select"">---Select---</option>" 'Jules 2018.08.10
                                For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                    If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = aryDoc(i)(22).Trim Then
                                        tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                    Else
                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    End If
                                Next
                                strrow &= "</td>" 'Jules 2018.07.13
                            End If

                            If Not aryDoc(i)(22) Is Nothing Then
                                If aryDoc(i)(22).ToString.Contains("IM2") Then
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlOutputTax" & i & """ value="""" disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                    'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                    If dsOutputTax.Tables(0).Rows.Count > 0 Then
                                        For rec As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                            If dsOutputTax.Tables(0).Rows(rec).Item(1).ToString = compOutputTaxValue_IM2 Then
                                                strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(rec).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(rec).Item(0).ToString & "</option>"
                                            Else
                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(rec).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(rec).Item(0).ToString & "</option>"
                                            End If
                                        Next
                                    End If
                                    strrow &= "</td>" 'Jules 2018.07.13
                                Else
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlOutputTax" & i & """ value="""" disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""0"" selected=""selected"">" & "N/A" & "</option>"
                                    strrow &= "</td>" 'Jules 2018.07.13
                                End If
                            Else
                                'Output Tax
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlOutputTax" & i & """ value="""" disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                strrow &= "<option value=""0"" selected=""selected"">" & "N/A" & "</option>"
                                strrow &= "</td>" 'Jules 2018.07.13
                            End If

                        End If
                    End If
                End If
                'End

                ''Zulham PAMB - 23042018
                ''Added Gift dropdownlist
                'Zulham 27092018 - PAMB SST
                strrow &= "<td style=""display:none;"">"
                If ViewState("isCAPEX" & i) Then
                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" disabled=""disabled"" onchange =""onClick('" & i & "');""  name=""ddlGift" & i & """ value="""">"
                Else
                    strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;""  onchange =""onClick('" & i & "');""  name=""ddlGift" & i & """ value="""">"
                End If

                If aryDoc(i)(37) Is Nothing Then
                    strrow &= "<option value=""No"" selected=""selected"">" & "No" & "</option>"
                    strrow &= "<option value=""Yes"">" & "Yes" & "</option>"
                ElseIf aryDoc(i)(37).ToString.Trim() = "Yes" Then
                    strrow &= "<option value=""Yes"" selected=""selected"">" & "Yes" & "</option>"
                    strrow &= "<option value=""No"">" & "No" & "</option>"
                Else
                    strrow &= "<option value=""No"" selected=""selected"">" & "No" & "</option>"
                    strrow &= "<option value=""Yes"">" & "Yes" & "</option>"
                End If
                strrow &= "</td >"

                'End

                ''Jules 2018.04.11 - PAMB Scrum 1 - Added Category.
                'strrow &= "<td >"
                'strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlCategory" & i & """ value="""" onchange =""onClick('" & i & "');"">"
                'strrow &= "<option value=""Life"" selected=""selected"">" & "Life" & "</option>"
                'strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                'strrow &= "<option value=""Mixed"">" & "Mixed" & "</option>"
                'strrow &= "</td>"
                ''End modification.
                'Zulham 26/04/2018 - PAMB
                'Added onChange attribute
                'Zulham 09112018 - PAMB
                strrow &= "<td >"
                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;""  onchange =""onClick('" & i & "');""  name=""ddlCategory" & i & """ value="""">"
                If aryDoc(i)(25) Is Nothing Then
                    strrow &= "<option value=""Life"">" & "Life" & "</option>"
                    strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                    strrow &= "<option value=""Mixed"" selected=""selected"">" & "Mixed" & "</option>"
                ElseIf aryDoc(i)(25).ToString.Trim <> "" Then
                    Select Case aryDoc(i)(25).ToString.Trim
                        Case "Life"
                            strrow &= "<option value=""Life"" selected=""selected"">" & "Life" & "</option>"
                            strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                            strrow &= "<option value=""Mixed"">" & "Mixed" & "</option>"
                        Case "Mixed"
                            strrow &= "<option value=""Life"">" & "Life" & "</option>"
                            strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                            strrow &= "<option value=""Mixed"" selected=""selected"">" & "Mixed" & "</option>"
                        Case "Non-Life"
                            strrow &= "<option value=""Life"">" & "Life" & "</option>"
                            strrow &= "<option value=""Non-Life"" selected=""selected"">" & "Non-Life" & "</option>"
                            strrow &= "<option value=""Mixed"">" & "Mixed" & "</option>"
                    End Select
                Else
                    strrow &= "<option value=""Life"">" & "Life" & "</option>"
                    strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                    strrow &= "<option value=""Mixed"" selected=""selected"">" & "Mixed" & "</option>"
                End If
                strrow &= "</td>" 'Jules 2018.07.13

                'Dim glcode As String = ""
                'If aryDoc(i)(8) <> "" Or aryDoc(i)(8) <> Nothing Then
                '    'Zulham 30/4/2018 - PAMB
                '    'Set the selected GL to ary(i)(8) after postback
                '    'Dim ss = hidGLCodeTest.Value
                '    If Not hidGLCodeTest.Value.Trim.Length < 10 Then
                '        aryDoc(i)(8) = hidGLCodeTest.Value
                '    End If
                '    'End
                '    If InStr(aryDoc(i)(8).ToString, ":") Then
                '        glcode = aryDoc(i)(8).ToString.ToString.Substring(0, InStr(aryDoc(i)(8).ToString, ":") - 1)
                '    Else
                '        glcode = aryDoc(i)(8)
                '    End If
                'End If

                'Zulham 26042018 - PAMB
                'If aryDoc(i)(1) = "Own Co." Or aryDoc(i)(1) Is Nothing Or aryDoc(i)(1) = "" Or UCase(aryDoc(i)(1)) = "HLISB" Then

                strrow &= "<td >"

                'Jules 2018.07.09
                'Zulham 08102018 - PAMB SST
                'strrow &= "<input style=""width:165px;margin-right:0px; "" onblur=""onClick('" & i & "');"" readonly=""readonly"" class=""txtbox2"" type=""text""  id=""txtGLCode" & i & """ name=""txtGLCode" & i & """ value=""" & IIf(aryDoc(i)(8) Is Nothing, "", aryDoc(i)(8)) & """>"
                strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtGLCode" & i & """ name=""txtGLCode" & i & """ value=""" & IIf(aryDoc(i)(8) Is Nothing, "", aryDoc(i)(8)) & """>"
                strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnGLCode" & i & """ name=""btnGLCode" & i & """ onclick=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & aryDoc(i)(8) & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
                strrow &= "<input type=""hidden"" id=""hidGLCode" & i & """ name=""hidGLCode" & i & """ value=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & aryDoc(i)(8) & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnGLCode onclick
                strrow &= "<input type=""hidden"" id=""hidGLCodeVal" & i & """ value=""" & aryDoc(i)(8) & """ runat=""server"" />"
                strrow &= "</td>"

                'Else
                ''Modified for IPP GST Stage 2A - CH - 5 Feb 2015
                'If strDefIPPCompID = "" Then
                '    aryDoc(i)(8) = objDB.GetVal("SELECT CONCAT(ic_con_ibs_code,':',cbg_b_gl_desc) FROM ipp_company INNER JOIN company_b_gl_code ON cbg_b_gl_code = ic_con_ibs_code AND cbg_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' WHERE ic_other_b_coy_code = '" & aryDoc(i)(1) & "' AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                'Else
                '    aryDoc(i)(8) = objDB.GetVal("SELECT CONCAT(ic_con_ibs_code,':',cbg_b_gl_desc) FROM ipp_company INNER JOIN company_b_gl_code ON cbg_b_gl_code = ic_con_ibs_code AND cbg_b_coy_id = '" & Common.Parse(strDefIPPCompID) & "' WHERE ic_other_b_coy_code = '" & aryDoc(i)(1) & "' AND ic_coy_id = '" & Common.Parse(strDefIPPCompID) & "'")
                'End If

                'strrow &= "<td >"
                'strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtGLCode" & i & """ name=""txtGLCode" & i & """ value=""" & aryDoc(i)(8) & """ disabled=""disabled"">"
                'strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnGLCode" & i & """ name=""btnGLCode" & i & """ onclick=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & glcode & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" disabled=""disabled"">"
                'strrow &= "<input type=""hidden"" id=""hidGLCode" & i & """ name=""hidGLCode" & i & """ value=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & glcode & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnGLCode onclick
                'strrow &= "<input type=""hidden"" id=""hidGLCodeVal" & i & """ value=""" & glcode & """ runat=""server"" />"
                'strrow &= "</td>"

                'End If

                ''End

                'Jules 2018.04.11 - PAMB Scrum 1 - Added Analysis Codes.               
                Dim analysisCode As String = ""
                Dim analysisCodeDesc As String = ""

                'Field for Analysis Code 1 to 9
                For j As Integer = 0 To 8
                    If aryDoc(i)(26 + j) <> "" Or aryDoc(i)(26 + j) <> Nothing Then
                        If InStr(aryDoc(i)(26 + j).ToString, ":") Then
                            analysisCode = aryDoc(i)(26 + j).ToString.ToString.Substring(0, InStr(aryDoc(i)(26 + j).ToString, ":") - 1)
                        Else
                            analysisCode = aryDoc(i)(26 + j)
                        End If
                        analysisCodeDesc = aryDoc(i)(26 + j)
                    End If


                    If j <> 5 AndAlso j <> 6 Then
                        If j = 0 Then
                            analysisCode = objDB.GetVal("SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE='ITNP' AND AC_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                            analysisCodeDesc = objDB.GetVal("SELECT CONCAT(AC_ANALYSIS_CODE,"":"",IFNULL(AC_ANALYSIS_CODE_DESC,'')) 'AC_ANALYSIS_CODE' FROM analysis_code WHERE AC_ANALYSIS_CODE='ITNP' AND AC_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")
                        End If
                        strrow &= "<td>"
                        strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtAnalysisCode" & j + 1 & "_" & i & """ name=""txtAnalysisCode" & j + 1 & "_" & i & """ value=""" & analysisCodeDesc & """>"
                        strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnAnalysisCode" & j + 1 & "_" & i & """ name=""btnAnalysisCode" & j + 1 & "_" & i & """ onclick=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & analysisCode & "&txtid=" & "txtAnalysisCode" & j + 1 & "_" & i & "&hidbtnid=btnAnalysisCode" & j + 1 & "_" & i & "&hidid=hidAnalysisCode" & j + 1 & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j + 1 & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
                        strrow &= "<input type=""hidden"" id=""hidAnalysisCode" & j + 1 & "_" & i & """ name=""hidAnalysisCode" & j + 1 & "_" & i & """ value=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & analysisCode & "&txtid=" & "txtAnalysisCode" & j + 1 & "_" & i & "&hidbtnid=btnAnalysisCode" & j + 1 & "_" & i & "&hidid=hidAnalysisCode" & j + 1 & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j + 1 & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnAnalysisCode onclick                        
                        strrow &= "<input type=""hidden"" id=""hidAnalysisCodeVal" & j + 1 & "_" & i & """ value=""" & analysisCodeDesc & """ runat=""server"" />"
                        strrow &= "</td>"
                        analysisCode = ""
                        analysisCodeDesc = ""
                    End If
                Next
                'End modification.

                'Jules 2018.04.25 - PAMB Scrum 1 - Remove Sub Description.
                ''For RuleCat
                ''If there's only one Sub Description, auto populate.
                'Dim ds As New DataSet : ds = Nothing
                'Dim objIPPMain As New IPPMain
                'If Not aryDoc(i)(8) Is Nothing Then
                '    If Not aryDoc(i)(8).ToString.Trim.Length = 0 Then
                '        ds = objIPPMain.getRuleCategory(aryDoc(i)(8).ToString.Split(":")(0).Trim)
                '    End If
                'End If
                'If Not ds Is Nothing Then
                '    If ds.Tables(0).Rows.Count = 1 Then
                '        strrow &= "<td >"
                '        strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  onkeypress=""return isNumberCharKey(event);""  id=""txtRuleCategory" & i & """ name=""txtRuleCategory" & i & """ value=""" & ds.Tables(0).Rows(0).Item("igc_glrule_category").ToString & """>"
                '        strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnSubDescCode" & i & """ name=""btnSubDescCode" & i & """ onclick=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "&txtid=" & "txtRuleCategory" & i & "&hidbtnid=btnSubDescCode" & i & "&hidid=hidRuleCategory2" & i & "&hidvalid=hidRuleCategoryVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
                '        'strrow &= "<a href=""#"" id=""btnRuleCategory2" & i & """ onclick=""window.open('../../Common/IPP/RulesCategoryDetail.aspx?GLCode=" & aryDoc(i)(8).ToString & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;""><img src=""../../Common/Plugins/images/collapse_down.gif"" style=""border-width:0px;""/> </a>"
                '        strrow &= "<input type=""hidden"" id=""hidRuleCategory2" & i & """ name=""hidRuleCategory2" & i & """ value=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8).ToString & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;"" />" 'this line must be same with the line btnCostAlloc2 onclick
                '        strrow &= "<input type=""hidden"" id=""hidRuleCategoryVal" & i & """ name=""hidRuleCategoryVal" & i & """ value=""" & ds.Tables(0).Rows(0).Item("igc_glrule_category_index").ToString & """  runat=""server"" />"
                '        strrow &= "</td>"
                '    ElseIf ds.Tables(0).Rows.Count > 1 Then
                '        strrow &= "<td >"
                '        strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  onkeypress=""return isNumberCharKey(event);""  id=""txtRuleCategory" & i & """ name=""txtRuleCategory" & i & """ value=""" & aryDoc(i)(18) & """>"
                '        'on postback triggered by ddl pay for, the textbox becomes ""
                '        strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnSubDescCode" & i & """ name=""btnSubDescCode" & i & """ onclick=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "&txtid=" & "txtRuleCategory" & i & "&hidbtnid=btnSubDescCode" & i & "&hidid=hidRuleCategory2" & i & "&hidvalid=hidRuleCategoryVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
                '        'strrow &= "<a href=""#"" id=""btnRuleCategory2" & i & """ onclick=""window.open('../../Common/IPP/RulesCategoryDetail.aspx?GLCode=" & aryDoc(i)(8).ToString & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;""><img src=""../../Common/Plugins/images/collapse_down.gif"" style=""border-width:0px;""/> </a>"
                '        strrow &= "<input type=""hidden"" id=""hidRuleCategory2" & i & """ name=""hidRuleCategory2" & i & """ value=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8).ToString & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;"" />" 'this line must be same with the line btnCostAlloc2 onclick
                '        strrow &= "<input type=""hidden"" id=""hidRuleCategoryVal" & i & """ name=""hidRuleCategoryVal" & i & """ value=""" & aryDoc(i)(19) & """  runat=""server"" />"
                '        strrow &= "</td>"
                '    Else
                '        strrow &= "<td >"
                '        strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  onkeypress=""return isNumberCharKey(event);""  id=""txtRuleCategory" & i & """ name=""txtRuleCategory" & i & """ readonly  value=""" & "" & """>"
                '        strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnSubDescCode" & i & """ name=""btnSubDescCode" & i & """ onclick=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "&txtid=" & "txtRuleCategory" & i & "&hidbtnid=btnSubDescCode" & i & "&hidid=hidRuleCategory2" & i & "&hidvalid=hidRuleCategoryVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
                '        'strrow &= "<a href=""#"" id=""btnRuleCategory2" & i & """ onclick=""window.open('../../Common/IPP/RulesCategoryDetail.aspx?GLCode=" & aryDoc(i)(8) & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;""><img src=""../../Common/Plugins/images/collapse_down.gif"" style=""border-width:0px;""/> </a>"
                '        strrow &= "<input type=""hidden"" id=""hidRuleCategory2" & i & """ name=""hidRuleCategory2" & i & """ value=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & "" & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;"" />" 'this line must be same with the line btnCostAlloc2 onclick
                '        strrow &= "<input type=""hidden"" id=""hidRuleCategoryVal" & i & """ name=""hidRuleCategoryVal" & i & """ value=""" & "" & """  runat=""server"" />"
                '        strrow &= "</td>"
                '    End If
                'Else
                '    strrow &= "<td >"
                '    strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  onkeypress=""return isNumberCharKey(event);""  id=""txtRuleCategory" & i & """ name=""txtRuleCategory" & i & """ value=""" & aryDoc(i)(18) & """>"
                '    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnSubDescCode" & i & """ name=""btnSubDescCode" & i & """ onclick=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "&SubDescCode=" & aryDoc(i)(18) & "&txtid=" & "txtRuleCategory" & i & "&hidbtnid=btnSubDescCode" & i & "&hidid=hidRuleCategory2" & i & "&hidvalid=hidRuleCategoryVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
                '    strrow &= "<input type=""hidden"" id=""hidRuleCategory2" & i & """ name=""hidRuleCategory2" & i & """ value=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;"" />" 'this line must be same with the line btnCostAlloc2 onclick
                '    strrow &= "<input type=""hidden"" id=""hidRuleCategoryVal" & i & """ name=""hidRuleCategoryVal" & i & """ value=""" & aryDoc(i)(19) & """  runat=""server"" />"
                '    strrow &= "</td>"
                'End If
                ''End
                'End modification.

                'Jules 2018.04.10 - PAMB Scrum 1 - Commented HO/BR field.
                'Dim brcode As String
                'If aryDoc(i)(12) <> "" Or aryDoc(i)(12) <> Nothing Then
                '    If InStr(aryDoc(i)(8).ToString, ":") Then
                '        brcode = aryDoc(i)(12).ToString.ToString.Substring(0, InStr(aryDoc(i)(12).ToString, ":") - 1)
                '    Else
                '        brcode = aryDoc(i)(12)
                '    End If
                'End If
                ''For employee, auto populate brcode and ccCode
                'Dim predefinedBrCode As String = ""
                Dim predefinedCCCode As String = ""
                'If compType = "E" Then
                '    If Not Request.QueryString("ic_index") Is Nothing Then
                '        If Not Request.QueryString("ic_index").Trim.Length = 0 Then
                '            predefinedBrCode = objDB.GetVal("SELECT DISTINCT IFNULL(Ic_Additional_3,'') 'IC_ADDITIONAL_3' FROM ipp_company WHERE ic_coy_type = 'E' AND ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "' and ic_index = " & Request.QueryString("ic_index"))
                '            predefinedCCCode = objDB.GetVal("SELECT DISTINCT IFNULL(Ic_Additional_4,'') 'IC_ADDITIONAL_4' FROM ipp_company WHERE ic_coy_type = 'E' AND ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "' and ic_index = " & Request.QueryString("ic_index"))
                '        Else
                '            predefinedBrCode = objDB.GetVal("SELECT DISTINCT IFNULL(Ic_Additional_3,'') 'IC_ADDITIONAL_3' FROM ipp_company WHERE ic_coy_type = 'E' AND ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "' ")
                '            predefinedCCCode = objDB.GetVal("SELECT DISTINCT IFNULL(Ic_Additional_4,'') 'IC_ADDITIONAL_4' FROM ipp_company WHERE ic_coy_type = 'E' AND ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "' ")
                '        End If
                '    Else
                '        predefinedBrCode = objDB.GetVal("SELECT DISTINCT IFNULL(Ic_Additional_3,'') 'IC_ADDITIONAL_3' FROM ipp_company WHERE ic_coy_type = 'E' AND ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                '        predefinedCCCode = objDB.GetVal("SELECT DISTINCT IFNULL(Ic_Additional_4,'') 'IC_ADDITIONAL_4' FROM ipp_company WHERE ic_coy_type = 'E' AND ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                '    End If
                'End If
                'If Not predefinedBrCode = "" Then brcode = predefinedBrCode
                'If compType = "E" And aryDoc(i)(12) Is Nothing Then
                '    If aryDoc(i)(12) = "" And Not predefinedBrCode = "" Then
                '        Dim branch = objDB.GetVal("SELECT  CONCAT(cbm_branch_code, ' : ' , cbm_branch_name) AS cbm_branch_code " _
                '       & "FROM company_branch_mstr " _
                '       & "WHERE cbm_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cbm_status = 'A' " _
                '       & "and (cbm_branch_code like '%" & Common.Parse(predefinedBrCode) & "%')")
                '        strrow &= "<td >"
                '        strrow &= "<input style=""width:70px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtBranch" & i & """ name=""txtBranch" & i & """ value=""" & branch & """>"
                '        strrow &= "<input type=""hidden"" id=""hidBranchVal" & i & """ value=""" & brcode & """  runat=""server"" />"
                '        strrow &= "</td>"
                '    Else
                '        strrow &= "<td >"
                '        strrow &= "<input style=""width:70px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtBranch" & i & """ name=""txtBranch" & i & """ value=""" & aryDoc(i)(12) & """>"
                '        strrow &= "<input type=""hidden"" id=""hidBranchVal" & i & """ value=""" & brcode & """  runat=""server"" />"
                '        strrow &= "</td>"

                '    End If
                'Else
                '    strrow &= "<td >"
                '    strrow &= "<input style=""width:70px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtBranch" & i & """ name=""txtBranch" & i & """ value=""" & aryDoc(i)(12) & """>"
                '    strrow &= "<input type=""hidden"" id=""hidBranchVal" & i & """ value=""" & brcode & """  runat=""server"" />"
                '    strrow &= "</td>"
                'End If
                'End commented block.

                Dim cccode As String
                If aryDoc(i)(13) <> "" Or aryDoc(i)(13) <> Nothing Then
                    If InStr(aryDoc(i)(8).ToString, ":") Then
                        If InStr(aryDoc(i)(13).ToString, ":") Then
                            cccode = aryDoc(i)(13).ToString.Substring(0, InStr(aryDoc(i)(13).ToString, ":") - 1)
                        Else
                            cccode = aryDoc(i)(13)
                        End If
                    Else
                        cccode = aryDoc(i)(13)
                    End If
                End If
                If Not predefinedCCCode = "" Then cccode = predefinedCCCode
                If compType = "E" And aryDoc(i)(13) Is Nothing Then
                    If aryDoc(i)(13) = "" And Not predefinedCCCode = "" Then
                        Dim CostCentre = objDB.GetVal("SELECT CONCAT(CC_CC_CODE, "" : "" , CC_CC_DESC) AS CC_CC_CODE " _
                                                       & "FROM COST_CENTRE " _
                                                       & "WHERE cc_coy_id='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND cc_status = 'A' " _
                                                       & "and cc_cc_code like '%" & predefinedCCCode & "%'")

                        strrow &= "<td >"
                        strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text"" onkeypress=""getLineNo('" & i & "');"" id=""txtCC" & i & """ name=""txtCC" & i & """ value=""" & CostCentre & """>"
                        strrow &= "<input type=""hidden"" id=""hidCCVal" & i & """ value=""" & cccode & """  runat=""server"" />"
                        strrow &= "</td>"
                    Else
                        strrow &= "<td >"
                        strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text"" onkeypress=""getLineNo('" & i & "');"" id=""txtCC" & i & """ name=""txtCC" & i & """ value=""" & aryDoc(i)(13) & """>"
                        strrow &= "<input type=""hidden"" id=""hidCCVal" & i & """ value=""" & cccode & """  runat=""server"" />"
                        strrow &= "</td>"
                    End If
                Else
                    strrow &= "<td >"
                    strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text"" onkeypress=""getLineNo('" & i & "');"" id=""txtCC" & i & """ name=""txtCC" & i & """ value=""" & aryDoc(i)(13) & """>"
                    strrow &= "<input type=""hidden"" id=""hidCCVal" & i & """ value=""" & cccode & """  runat=""server"" />"
                    strrow &= "</td>"
                End If

                'strrow &= "</td>"                
                'strrow &= "</td>"

                'mimi 10/04/18 - Withholding Tax
                Dim holdingTax As String = ""

                'Jules 2018.07.05            
                Dim holdingTaxOpt As String = ""
                If aryDoc(i)(24) <> "" Or aryDoc(i)(24) <> Nothing Then
                    If InStr(aryDoc(i)(24).ToString, ":") Then
                        Dim strHoldingTax() As String
                        strHoldingTax = Split(aryDoc(i)(24).ToString, ":")
                        If strHoldingTax.Length > 1 Then
                            holdingTax = aryDoc(i)(24).ToString.Substring(InStr(aryDoc(i)(24).ToString, ":"), strHoldingTax(1).Length)
                            holdingTaxOpt = aryDoc(i)(24).ToString.ToString.Substring(0, InStr(aryDoc(i)(24).ToString, ":") - 1)
                        End If
                    Else
                        holdingTax = aryDoc(i)(24)
                    End If
                End If

                If aryDoc(i)(35) <> "" Then
                    holdingTaxOpt = aryDoc(i)(35)
                End If
                'End modification.

                strrow &= "<td >"

                'Jules 2018.07.05
                'strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  onkeypress=""return isNumberCharKey(event);""  id=""txtWithholding" & i & """ name=""txtWithholding" & i & """ value=""" & aryDoc(i)(24) & """>"
                'strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnTaxHolding" & i & """ name=""btnTaxHolding" & i & """ onclick=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & holdingTax & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
                'strrow &= "<input type=""hidden"" id=""hidHoldingTax" & i & """ name=""hidHoldingTax" & i & """ value=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & holdingTax & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnGLCode onclick
                strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  onkeypress=""return isNumberCharKey(event);""  id=""txtWithholding" & i & """ name=""txtWithholding" & i & """ value=""" & holdingTax & """>"
                strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnTaxHolding" & i & """ name=""btnTaxHolding" & i & """ onclick=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & holdingTax & "&opt=" & holdingTaxOpt & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
                strrow &= "<input type=""hidden"" id=""hidHoldingTax" & i & """ name=""hidHoldingTax" & i & """ value=""window.open('../../Common/IPP/WithHoldingTax.aspx?id=" & holdingTax & "&opt=" & holdingTaxOpt & "&txtid=" & "txtWithholding" & i & "&hidbtnid=btnTaxHolding" & i & "&hidid=hidHoldingTax" & i & "&hidvalid=hidHoldingTaxVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnTaxHolding onclick
                strrow &= "<input type=""hidden"" id=""hidHoldingTaxOpt" & i & """ value=""" & holdingTaxOpt & """ runat=""server"" />"
                strrow &= "<input style=""width:165px;margin-right:0px;display:none; "" class=""txtbox2"" type=""text""  id=""txtWithholdingOpt" & i & """ name=""txtWithholdingOpt" & i & """ value=""" & holdingTaxOpt & """ >"
                'End modification.

                strrow &= "<input type=""hidden"" id=""hidHoldingTaxVal" & i & """ value=""" & holdingTax & """ runat=""server"" />"
                strrow &= "</td>"
                'end

                strrow &= "</tr>"

                'Jules 2018.07.09 - Recalculate GST.
                'strscript.Append("calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');")                
            Next
            If Request.QueryString("doctype") <> "Invoice" And Request.QueryString("doctype") <> "Bill" And Request.QueryString("doctype") <> "Letter" Then
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                    'zulham 21/01/2016 - IPP STAGE 4 PHASE 2
                    'added 'total w/o gst amount'
                    'Zulham 04102018 - paMB SST
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:440px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">Total Amount (excl.SST) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotalNoGST"" name=""sTotalNoGST"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:440px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">SST Amount Input :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTInputTotal"" name=""sGSTInputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:440px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">(SST Amount Output) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTOutputTotal"" name=""sGSTOutputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:440px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">Total (incl. SST) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                Else
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:440px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">Total (incl. SST) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                End If
            Else
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                    'zulham 21/01/2016 - IPP STAGE 4 PHASE 2
                    'added 'total w/o gst amount'
                    'Zulham 04102018 - PAMB SST
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:350px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">Total Amount (excl.SST) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotalNoGST"" name=""sTotalNoGST"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:350px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">SST Amount Input :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTInputTotal"" name=""sGSTInputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:350px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">(SST Amount Output) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTOutputTotal"" name=""sGSTOutputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:350px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">Total (incl. SST) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                Else
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:350px;""></td><td class = ""emptycol"" style=""width: 192px; text-align:right; font-weight:bold; "">Total (incl. SST) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                End If
            End If

            If Request.QueryString("doctype") <> "Invoice" And Request.QueryString("doctype") <> "Bill" And Request.QueryString("doctype") <> "Letter" Then
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then

                    'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR, added Category and Analysis Code, increased table width, moved Cost Centre.
                    'Zulham 24092018 - PAMB
                    'UAT - U00007
                    'Zulham 16102018 - PAMB
                    table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1500px;"" id=""tblIPPDocItem"">" &
                            "<tr class=""TableHeader"">" &
                            "<td style=""width:50px;margin-right:0px;"" align=""right"">S/No</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Pay For</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Disb./Reimb.</td>" &
                            "<td style=""width:65px;margin-right:0px;"">Invoice No.<span id=""lblInvNo"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:60px;margin-right:0px;display:none;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:45px;margin-right:0px;display:none;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:65px;margin-right:0px;display:none;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount (excl.SST)</td>" &
                            "<td style=""width:110px;margin-right:0px;"" >SST Amount</td>" &
                            "<td style=""width:110px;margin-right:0px;"" >Input Tax Code</td>" &
                            "<td style=""width:110px;margin-right:0px;"" >Output Tax Code</td>" &
                            "<td style=""width:100px;margin-right:0px;display:none;"">Gift</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Category</td>" &
                            "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:50px;margin-right:0px;"">Fund Type(L1)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Product Type(L2)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Channel(L3)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Reinsurance Company(L4)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Asset Fund(L5)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Project Code(L8)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Person Code(L9)</td>" &
                            "<td style=""width:100px;margin-right:0px;"">Cost Centre(L7)</td>" &
                            "<td style=""width:50px;margin-right:0px;"">Withholding Tax (%)</td>" & 'mimi - 10 / 4 / 18 :Tax holding percentage
                            "</tr>" &
                            strrow &
                            "</table>"

                    'Jules 2018.07.11 - Removed Sub Description
                    '"<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" &

                    '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" & _
                    '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                    '"</tr>" & _
                    'strrow & _
                    '"</table>"
                    'End modification.

                Else
                    'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR, added Category and Analysis Code, increased table width, moved Cost Centre.    
                    'ZULHAM 24092018 - PAMB
                    'UAT - U00007
                    'Zulham 16102018 - PAMB
                    table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1500px;"" id=""tblIPPDocItem"">" &
                               "<tr class=""TableHeader"">" &
                               "<td style=""width:50px;margin-right:0px;"" align=""right"">S/No</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Pay For</td>" &
                               "<td style=""width:65px;margin-right:0px;"">Invoice No.<span id=""lblInvNo"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:60px;margin-right:0px;display:none;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:45px;margin-right:0px;display:none;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:65px;margin-right:0px;display:none;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount (excl.SST)</td>" &
                               "<td style=""width:100px;margin-right:0px;display:none;"">Gift</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Category</td>" &
                               "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:50px;margin-right:0px;"">Fund Type(L1)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Product Type(L2)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Channel(L3)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Reinsurance Company(L4)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Asset Fund(L5)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Project Code(L8)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Person Code(L9)</td>" &
                               "<td style=""width:100px;margin-right:0px;"">Cost Centre(L7)</td>" &
                               "<td style=""width:50px;margin-right:0px;"">Withholding Tax (%)</td>" & 'mimi - 10 / 4 / 18 :Tax holding percentage
                               "</tr>" &
                               strrow &
                               "</table>"

                    'Jules 2018.07.11 - Removed Sub Description
                    '"<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" &
                    '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" & _
                    '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                    '"</tr>" & _
                    'strrow & _
                    '"</table>"
                    'End modification.
                End If
            Else
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                    'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR and Sub Description, added Category and Analysis Code, increased table width, changed sequence.
                    'zULHAM 24092018 - PAMB
                    'UAT - U00007 
                    'Zulham 16102018 - PAMB
                    table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1500px;"" id=""tblIPPDocItem"">" &
                           "<tr class=""TableHeader"">" &
                           "<td style=""width:50px;margin-right:0px;"" align=""right"">S/No</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Pay For</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Disb./Reimb.</td>" &
                           "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:60px;margin-right:0px;display:none;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:45px;margin-right:0px;display:none;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:65px;margin-right:0px;display:none;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount (excl.SST)</td>" &
                           "<td style=""width:110px;margin-right:0px;"" >SST Amount</td>" &
                           "<td style=""width:110px;margin-right:0px;"" >Input Tax Code</td>" &
                           "<td style=""width:110px;margin-right:0px;"" >Output Tax Code</td>" &
                           "<td style=""width:100px;margin-right:0px;display:none;"">Gift</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Category</td>" &
                           "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:50px;margin-right:0px;"">Fund Type(L1)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Product Type(L2)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Channel(L3)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Reinsurance Company(L4)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Asset Fund(L5)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Project Code(L8)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Person Code(L9)</td>" &
                           "<td style=""width:100px;margin-right:0px;"">Cost Centre(L7)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Withholding Tax (%)</td>" & 'mimi - 10 / 4 / 18 :Tax holding percentage
                           "</tr>" &
                           strrow &
                           "</table>"
                    '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" &
                    '"<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" &
                    '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                    '"</tr>" &
                    'strrow &
                    '"</table>"
                    'End modification.
                Else
                    'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR and Sub Description, added Category and Analysis Code, increased table width, changed sequence.
                    'zULHAM 24092018 - PAMB
                    'UAT - U00007
                    'Zulham 09102018 - PAMB SST
                    table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1500px;"" id=""tblIPPDocItem"">" &
                           "<tr class=""TableHeader"">" &
                           "<td style=""width:50px;margin-right:0px;"" align=""right"">S/No</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Pay For</td>" &
                           "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:60px;margin-right:0px;display:none;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:45px;margin-right:0px;display:none;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:65px;margin-right:0px;display:none;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount (excl.SST)</td>" &
                           "<td style=""width:100px;margin-right:0px;display:none;"">Gift</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Category</td>" &
                           "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:50px;margin-right:0px;"">Fund Type(L1)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Product Type(L2)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Channel(L3)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Reinsurance Company(L4)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Asset Fund(L5)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Project Code(L8)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Person Code(L9)</td>" &
                           "<td style=""width:100px;margin-right:0px;"">Cost Centre(L7)</td>" &
                           "<td style=""width:50px;margin-right:0px;"">Withholding Tax (%)</td>" & 'mimi - 10 / 4 / 18 :Tax holding percentage
                           "</tr>" &
                           strrow &
                           "</table>"
                    '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" & _
                    '"<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" &
                    '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                    '"</tr>" & _
                    'strrow & _
                    '"</table>"
                    'End modification.

                End If
            End If

        End If

        'Jules 2018.07.10
        If strLiteralScript <> "" Then
            strscript.Append(strLiteralScript)
            strscript.Append("</script>")
            RegisterStartupScript("script5", strscript.ToString())
        End If
        'End modification.

        Session("ConstructTable") = table
    End Function
    Sub buildarydoc()

        'Zulham 08102018 - PAMB SST
        Dim clsGST As New GST
        Dim taxPercent As Integer 

        Dim i As Integer
        'CH - Issue 8317 (Production Issue) - 1st April 2015
        Dim compOutputTaxValue = objDB.GetVal("SELECT IFNULL(IP_param_value,'') FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT' AND IP_COY_ID = '" & Session("CompanyId") & "'")
        'Zulham 26/02/2015 IPP GST Stage 1
        Dim compInputTaxValue_TX4 = objDB.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'GIFT_LUCKY_INPUT' AND IP_COY_ID = '" & Session("CompanyId") & "'")
        Dim compOutputTaxValue_TX4 = objDB.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'GIFT_LUCKY_OUTPUT' AND IP_COY_ID = '" & Session("CompanyId") & "'")
        'Zulham 30/03/2016 - IM5/IM6 Enhancementtxt
        Dim compOutputTaxValue_IM6 = objDB.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT_IM6' AND IP_COY_ID = '" & Session("CompanyId") & "'")

        'Zulham PAMB - 25/04/2018 - Added ddlGift into the array
        For i = 0 To 9
            If Request.Form("txtRuleCategory" & i) = Request.Form("hidRuleCategoryVal" & i) And Not Request.Form("hidRuleCategoryVal" & i) = Nothing Then
                Dim subDescIndex = objDB.GetVal("SELECT igc_glrule_category_index FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" &
                " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " &
                "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And igg_gl_code LIKE '" & Request.Form("txtGLCode" & i).ToString.Split(":")(0).Trim & "' and igc_glrule_category = '" & Request.Form("txtRuleCategory" & i) & "' GROUP BY igc_glrule_category")
                If Not Request.Form("ddlInputTax" & i) Is Nothing Then
                    If Request.Form("ddlInputTax" & i).ToString.ToUpper = "IM2" Then
                        'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                        'Zulham 25092018 - 
                        'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue, Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                        'End modification.

                        'Zulham 26/02/2015 IPP GST Stage 1
                    ElseIf Request.Form("ddlInputTax" & i).ToString.Trim.ToUpper = compInputTaxValue_TX4.ToString.Trim.ToUpper Then
                        'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                        'Zulham 25092018 - 
                        'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue_TX4, Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                        'End modification.

                        'Zulham 30/03/2016 IM5/IM6 Enhancement
                    ElseIf Request.Form("ddlInputTax" & i).ToString.Trim.ToUpper = "IM5" Then
                        'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                        'Zulham 25092018 - 
                        'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue_IM6, Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)}) '
                        'End modification.

                    Else
                        'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                        'Zulham 25092018 - 
                        'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i), Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                        'End modification.
                    End If
                Else
                    'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                    'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                    'Zulham 25092018 - 
                    'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                    aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i), Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                    'End modification.
                End If
            ElseIf Not Request.Form("txtRuleCategory" & i) = Nothing And Not Request.Form("txtGLCode" & i) = Nothing Then
                Dim subDescIndex = objDB.GetVal("SELECT igc_glrule_category_index FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" &
                " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " &
                "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And igg_gl_code LIKE '" & Request.Form("txtGLCode" & i).ToString.Split(":")(0).Trim & "' and igc_glrule_category = '" & Request.Form("txtRuleCategory" & i) & "' GROUP BY igc_glrule_category")
                If Not Request.Form("ddlInputTax" & i) Is Nothing Then
                    If Request.Form("ddlInputTax" & i).ToString.ToUpper = "IM2" Then
                        'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                        'Zulham 25092018 - 
                        'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue, Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                        'End modification.

                        'Zulham 26/02/2015 IPP GST Stage 1
                    ElseIf Request.Form("ddlInputTax" & i).ToString.Trim.ToUpper = compInputTaxValue_TX4.ToString.Trim.ToUpper Then
                        'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                        'Zulham 25092018 - 
                        'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue_TX4, Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                        'End modification.

                        'Zulham 30/03/2016 IM5/IM6 Enhancement
                    ElseIf Request.Form("ddlInputTax" & i).ToString.Trim.ToUpper = "IM5" Then
                        'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                        'Zulham 25092018 - 
                        'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue_IM6, Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                        'End modification.
                    Else
                        'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                        'Zulham 25092018 - 
                        'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i), Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                        'End modification.
                    End If
                Else
                    'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                    'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.

                    aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i), Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                    'End modification.
                End If
            Else
                If Not Request.Form("ddlInputTax" & i) Is Nothing Then
                    If Request.Form("ddlInputTax" & i).ToString.ToUpper = "IM2" Then
                        'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                        'Zulham 25092018 - 
                        'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i), Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue, Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                        'End modification.

                        'Zulham 26/02/2015 IPP GST Stage 1
                    ElseIf Request.Form("ddlInputTax" & i).ToString.Trim.ToUpper = compInputTaxValue_TX4.ToString.Trim.ToUpper Then
                        'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                        'Zulham 25092018 - 
                        'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i), Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue_TX4, Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                        'End modification.

                        'Zulham 30/03/2016 IM5/IM6 Enhancement
                    ElseIf Request.Form("ddlInputTax" & i).ToString.Trim.ToUpper = "IM5" Then
                        'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                        'Zulham 25092018 - 
                        'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i), Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue_IM6, Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                        'End modification.
                    Else
                        'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                        'Zulham 08102018 - PAMB SST
                        'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                        If Not Request.Form("ddlInputTax" & i) Is Nothing Then
                            taxPercent = clsGST.getTaxPercentage(Request.Form("ddlInputTax" & i))
                            If Request.Form("txtGSTAmount" & i) <> "" Then
                                If CDbl(Request.Form("txtGSTAmount" & i)) = 0 And taxPercent <> 0 Then
                                    'Zulham 10102018 - PAMB SST
                                    aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i), Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtAmt" & i) = "", 0, Request.Form("txtAmt" & i)) * taxPercent / 100, Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i), Request.Form("txtWithholding" & i),
                                       Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                                Else
                                    aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i), Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i), Request.Form("txtWithholding" & i),
                                        Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                                End If
                            Else
                                aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i), Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i), Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                            End If
                        End If

                    End If
                Else
                    'mimi 2018-04-13 - PAMB Scrum 1 -Add WithHolding Tax.
                    'Jules 2018.04.10 - PAMB Scrum 1 - Removed Branch Code.
                    'Zulham 25092018 - 
                    'PAMB UAT U0007 - Changed qty to 1; unit price to amount
                    aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), 1, Request.Form("txtAmt" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i), Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i), Request.Form("txtWithholding" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), "", "", Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i), Request.Form("txtWithholdingOpt" & i), "", Request.Form("ddlGift" & i)})
                    'End modification.
                End If
            End If
        Next
    End Sub
    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim strscript As New System.Text.StringBuilder
        Dim objipp As New IPPMain
        Dim venidx As String
        Dim objGST As GST

        ConstructTable()

        If validateInput() Then
            If validateField() Then
                If Not Request.QueryString("venIdx") Is Nothing Then
                    venidx = Request.QueryString("venIdx")
                Else
                    'Jules 2018.07.09 - Allow "\" and "#"
                    'IPP Gst Stage 2A - CH - 11 Feb 2015
                    'venidx = objDB.GetVal("select im_s_coy_id from invoice_mstr where im_b_coy_id = 'hlb' and im_invoice_no='" & Common.Parse(Request.QueryString("docno")) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                    venidx = objDB.GetVal("select im_s_coy_id from invoice_mstr where im_b_coy_id = '" & Session("CompanyId") & "' and im_invoice_no='" & Common.Parse(Replace(Replace(Request.QueryString("docno"), "\", "\\"), "#", "\#")) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                End If

                If Request.QueryString("action") = "edit" Then

                    'Zulham 28/01/2016 - IPP Stage 4 Phase 2
                    'Check for gst amount differences.
                    'do some adjustment if there's any
                    '~0.1 cent
                    'Zulham 24/05/2016
                    'Changed the adjustment value to -2---0---2 under HLB request
                    Dim gstAmt As Decimal = 0.0
                    Dim diff As Decimal = 0.0
                    Dim diff2 As Decimal = 0.0
                    Dim diff3 As Decimal = 0.0

                    Dim invoiceDtlsGSTAmt As Decimal = 0.0
                    For i As Integer = 0 To aryDoc.Count - 1
                        'If Not aryDoc(i)(21) = "" And Not aryDoc(i)(21) Is Nothing Then
                        '    gstAmt += CDec(aryDoc(i)(21))
                        'End If
                        'Zulham 08/09/2016
                        'Exclude GST amount if input and output tax having similar percentage (6:6, 0:0)  
                        Dim gstOutput
                        Dim gstInput
                        If Not aryDoc(i)(22) Is Nothing Then
                            gstInput = objDB.GetVal("Select IFNULL(tax_perc, 0) 'percentage' FROM tax_mstr, tax WHERE tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_tax_code = '" & aryDoc(i)(22).ToString.Trim & "' AND tax_code = tm_tax_rate LIMIT 1")
                            If Not aryDoc(i)(23) Is Nothing Then
                                gstOutput = objDB.GetVal("Select IFNULL(tax_perc, 0) 'percentage' FROM tax_mstr, tax WHERE tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_tax_code = '" & aryDoc(i)(23).ToString.Trim & "' AND tax_code = tm_tax_rate LIMIT 1")
                            Else
                                gstOutput = "0"
                            End If
                        Else
                            gstInput = "0"
                            gstOutput = "0"
                        End If


                        If Not aryDoc(i)(21) = "" And Not aryDoc(i)(21) Is Nothing Then
                            If Not aryDoc(i)(22) Is Nothing Then
                                If (Not gstInput = gstOutput) Or aryDoc(i)(22).ToString.Trim = "TX4" Then
                                    gstAmt += CDec(aryDoc(i)(21))
                                End If
                            Else
                                If (Not gstInput = gstOutput) Then
                                    gstAmt += CDec(aryDoc(i)(21))
                                End If
                            End If
                        End If

                    Next

                    'Zulham 25102018
                    ''Zulham 15/02/2016 - IPP Stage 4 Phase 2
                    ''get the existing amount from invoice_details
                    ''if there's any, add the existing amount to the newly inserted amount
                    ''then, check for difference()
                    'invoiceDtlsGSTAmt = objipp.getItemDetailGSTAmount(Request.QueryString("olddocno"), venidx, "edit", aryDoc(0)(0))

                    'If Not invoiceDtlsGSTAmt = 0.0 Then
                    '    invoiceDtlsGSTAmt = invoiceDtlsGSTAmt + gstAmt
                    'Else
                    '    invoiceDtlsGSTAmt = gstAmt
                    'End If

                    'diff = CDec(hidHeaderGstAmount.Value) - CDec(gstAmt)
                    'diff2 = CDec(gstAmt) - CDec(hidHeaderGstAmount.Value)
                    'diff3 = CDec(hidHeaderGstAmount.Value) - CDec(invoiceDtlsGSTAmt)
                    'If ((CDec(FormatNumber(diff, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff, 2)) > CDec(FormatNumber(0, 2))) _
                    'Or (CDec(FormatNumber(diff2, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff2, 2)) > CDec(FormatNumber(0, 2))) _
                    'Or (CDec(FormatNumber(diff3, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff3, 2)) > CDec(FormatNumber(0, 2))) _
                    'Or (diff >= "-2.00" And FormatNumber(diff, 2) < FormatNumber(0, 2)) _
                    'Or (diff2 >= "-2.00" And FormatNumber(diff2, 2) < FormatNumber(0, 2)) _
                    'Or (diff3 >= "-2.00" And FormatNumber(diff3, 2) < FormatNumber(0, 2))) And Session("Clicked") Is Nothing Then
                    '    If Session("Clicked") Is Nothing Then
                    '        Dim vbs As String
                    '        'sGSTOutputTotal
                    '        vbs = vbs & "<script language=""vbs"">"
                    '        vbs = vbs & vbCrLf & "set i = document.getElementById(""sGSTInputTotal"")"
                    '        vbs = vbs & vbCrLf & "set j = document.getElementById(""hidInputGST"")"
                    '        vbs = vbs & vbCrLf & "i.innerHTML = j.Value"
                    '        vbs = vbs & vbCrLf & "set i = document.getElementById(""sGSTOutputTotal"")"
                    '        vbs = vbs & vbCrLf & "set j = document.getElementById(""hidOutputGST"")"
                    '        vbs = vbs & vbCrLf & "i.innerHTML = j.Value"
                    '        vbs = vbs & vbCrLf & "set i = document.getElementById(""sTotalNoGST"")"
                    '        vbs = vbs & vbCrLf & "set j = document.getElementById(""hidTotalNoGST"")"
                    '        vbs = vbs & vbCrLf & "i.innerHTML = j.Value"
                    '        vbs = vbs & vbCrLf & "set i = document.getElementById(""sTotal"")"
                    '        vbs = vbs & vbCrLf & "set j = document.getElementById(""hidTotal"")"
                    '        vbs = vbs & vbCrLf & "i.innerHTML = FormatNumber(j.Value,2)"
                    '        vbs = vbs & vbCrLf & "a = MsgBox(""" & "GST Amount adjustment will be done on the edited item." & """," & 1 & ",""" & "eProcurement" & """)"
                    '        vbs = vbs & vbCrLf & "If a = 1 Then"
                    '        vbs = vbs & vbCrLf & "set btnTest = document.getElementById(""hidButtonTest"")"
                    '        vbs = vbs & vbCrLf & "btnTest.Click"
                    '        vbs = vbs & vbCrLf & "Else"
                    '        vbs = vbs & vbCrLf & "set btnCancel = document.getElementById(""hidButtonCancel"")"
                    '        vbs = vbs & vbCrLf & "btnCancel.Click"
                    '        vbs = vbs & vbCrLf & "End If"
                    '        vbs = vbs & "</script>"
                    '        Dim rndKey As New Random
                    '        Me.RegisterStartupScript(rndKey.Next.ToString, vbs)
                    '        Session("Clicked") = "Yes"
                    '        Exit Sub
                    '    End If
                    'End If

                    'Dim idx
                    'For i As Integer = 0 To aryDoc.Count - 1
                    '    If aryDoc(i)(21) Is Nothing Then
                    '        idx = i
                    '        Exit For
                    '    ElseIf aryDoc(i)(21) = "" Then
                    '        idx = i
                    '        Exit For
                    '    End If
                    'Next

                    'If Not idx = 0 Then
                    '    If CDec(FormatNumber(diff, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff, 2)) > CDec(FormatNumber(0, 2)) Then
                    '        aryDoc(idx - 1)(21) = aryDoc(idx - 1)(21) + diff
                    '    ElseIf CDec(FormatNumber(diff3, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff3, 2)) > CDec(FormatNumber(0, 2)) Then
                    '        aryDoc(idx - 1)(21) = aryDoc(idx - 1)(21) + diff3
                    '    ElseIf CDec(FormatNumber(diff2, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff2, 2)) > CDec(FormatNumber(0, 2)) Then
                    '        aryDoc(idx - 1)(21) = aryDoc(idx - 1)(21) - diff2
                    '    ElseIf diff >= "-2.00" And FormatNumber(diff, 2) < FormatNumber(0, 2) Then
                    '        aryDoc(idx - 1)(21) = aryDoc(idx - 1)(21) + diff
                    '    ElseIf diff3 >= "-2.00" And FormatNumber(diff3, 2) < FormatNumber(0, 2) Then
                    '        aryDoc(idx - 1)(21) = aryDoc(idx - 1)(21) + diff3
                    '    ElseIf diff2 >= "-2.00" And FormatNumber(diff2, 2) < FormatNumber(0, 2) Then
                    '        aryDoc(idx - 1)(21) = aryDoc(idx - 1)(21) - diff2
                    '    End If
                    'ElseIf idx = 0 Then
                    '    If CDec(FormatNumber(diff, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff, 2)) > CDec(FormatNumber(0, 2)) Then
                    '        aryDoc(idx)(21) = aryDoc(idx)(21) + diff
                    '    ElseIf CDec(FormatNumber(diff3, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff3, 2)) > CDec(FormatNumber(0, 2)) Then
                    '        aryDoc(idx)(21) = aryDoc(idx)(21) + diff3
                    '    ElseIf CDec(FormatNumber(diff2, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff2, 2)) > CDec(FormatNumber(0, 2)) Then
                    '        aryDoc(idx)(21) = aryDoc(idx)(21) - diff2
                    '    ElseIf diff >= "-2.00" And FormatNumber(diff, 2) < FormatNumber(0, 2) Then
                    '        aryDoc(idx)(21) = aryDoc(idx)(21) + diff
                    '    ElseIf diff3 >= "-2.00" And FormatNumber(diff3, 2) < FormatNumber(0, 2) Then
                    '        aryDoc(idx)(21) = aryDoc(idx)(21) + diff3
                    '    ElseIf diff2 >= "-2.00" And FormatNumber(diff2, 2) < FormatNumber(0, 2) Then
                    '        aryDoc(idx)(21) = aryDoc(idx)(21) - diff2
                    '    End If
                    'End If

                    objipp.UpdateIPPDocDetail(aryDoc, Request.QueryString("docno"), venidx, Request.QueryString("olddocno"), IIf(Request.QueryString("MasterDoc") = "Yes", "Y", "N"), strIsGst)
                    Session("Clicked") = Nothing
                Else
                    'Zulham 28/01/2016 - IPP Stage 4 Phase 2
                    'Check for gst amount differences.
                    'do some adjustment if there's any
                    '~0.1 cent
                    'Zulham 24/05/2016
                    'Changed the adjustment value to rm2
                    Dim gstAmt As Decimal = 0.0
                    Dim diff As Decimal = 0.0
                    Dim diff2 As Decimal = 0.0
                    Dim diff3 As Decimal = 0.0

                    Dim invoiceDtlsGSTAmt As Decimal = 0.0
                    For i As Integer = 0 To aryDoc.Count - 1

                        'Zulham 08/09/2016
                        'Exclude GST amount if input and output tax having similar percentage (6:6, 0:0)  
                        Dim gstInput
                        If Not aryDoc(i)(22) Is Nothing Then
                            objDB.GetVal("Select IFNULL(tax_perc, 0) 'percentage' FROM tax_mstr, tax WHERE tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_tax_code = '" & aryDoc(i)(22).ToString.Trim & "' AND tax_code = tm_tax_rate LIMIT 1")
                        Else
                            gstInput = "0"
                        End If

                        Dim gstOutput
                        If Not aryDoc(i)(23) Is Nothing Then
                            gstOutput = objDB.GetVal("Select IFNULL(tax_perc, 0) 'percentage' FROM tax_mstr, tax WHERE tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_tax_code = '" & aryDoc(i)(23).ToString.Trim & "' AND tax_code = tm_tax_rate LIMIT 1")
                        Else
                            gstOutput = "0"
                        End If

                        If Not aryDoc(i)(21) = "" And Not aryDoc(i)(21) Is Nothing Then
                            If Not aryDoc(i)(22) Is Nothing Then
                                If (Not gstInput = gstOutput) Or aryDoc(i)(22).ToString.Trim = "TX4" Then
                                    gstAmt += CDec(aryDoc(i)(21))
                                End If
                            End If
                        End If
                    Next

                    'Zulham 25102018
                    ''Zulham 15/02/2016 - IPP Stage 4 Phase 2
                    ''get the existing amount from invoice_details
                    ''if there's any, add the existing amount to the newly inserted amount
                    ''then, check for difference()
                    'invoiceDtlsGSTAmt = objipp.getItemDetailGSTAmount(Request.QueryString("olddocno"), venidx)

                    'If Not invoiceDtlsGSTAmt = 0.0 Then
                    '    invoiceDtlsGSTAmt = invoiceDtlsGSTAmt + gstAmt
                    'Else
                    '    invoiceDtlsGSTAmt = gstAmt
                    'End If

                    'diff = CDec(hidHeaderGstAmount.Value) - CDec(gstAmt)
                    'diff2 = CDec(gstAmt) - CDec(hidHeaderGstAmount.Value)
                    'diff3 = CDec(hidHeaderGstAmount.Value) - CDec(invoiceDtlsGSTAmt)

                    'If ((CDec(FormatNumber(diff, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff, 2)) > CDec(FormatNumber(0, 2))) _
                    'Or (CDec(FormatNumber(diff2, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff2, 2)) > CDec(FormatNumber(0, 2))) _
                    'Or (CDec(FormatNumber(diff3, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff3, 2)) > CDec(FormatNumber(0, 2))) _
                    'Or (diff >= "-2.00" And FormatNumber(diff, 2) < FormatNumber(0, 2)) _
                    'Or (diff2 >= "-2.00" And FormatNumber(diff2, 2) < FormatNumber(0, 2)) _
                    'Or (diff3 >= "-2.00" And FormatNumber(diff3, 2) < FormatNumber(0, 2))) And Session("Clicked") Is Nothing Then
                    '    If Session("Clicked") Is Nothing Then
                    '        Dim vbs As String
                    '        'sGSTOutputTotal
                    '        vbs = vbs & "<script language=""vbs"">"
                    '        vbs = vbs & vbCrLf & "set i = document.getElementById(""sGSTInputTotal"")"
                    '        vbs = vbs & vbCrLf & "set j = document.getElementById(""hidInputGST"")"
                    '        vbs = vbs & vbCrLf & "i.innerHTML = j.Value"
                    '        vbs = vbs & vbCrLf & "set i = document.getElementById(""sGSTOutputTotal"")"
                    '        vbs = vbs & vbCrLf & "set j = document.getElementById(""hidOutputGST"")"
                    '        vbs = vbs & vbCrLf & "i.innerHTML = j.Value"
                    '        vbs = vbs & vbCrLf & "set i = document.getElementById(""sTotalNoGST"")"
                    '        vbs = vbs & vbCrLf & "set j = document.getElementById(""hidTotalNoGST"")"
                    '        vbs = vbs & vbCrLf & "i.innerHTML = j.Value"
                    '        vbs = vbs & vbCrLf & "set i = document.getElementById(""sTotal"")"
                    '        vbs = vbs & vbCrLf & "set j = document.getElementById(""hidTotal"")"
                    '        vbs = vbs & vbCrLf & "i.innerHTML = FormatNumber(j.Value,2)"
                    '        vbs = vbs & vbCrLf & "a = MsgBox(""" & "GST Amount adjustment will be done on the last item." & """," & 1 & ",""" & "eProcurement" & """)"
                    '        vbs = vbs & vbCrLf & "If a = 1 Then"
                    '        vbs = vbs & vbCrLf & "set btnTest = document.getElementById(""hidButtonTest"")"
                    '        vbs = vbs & vbCrLf & "btnTest.Click"
                    '        vbs = vbs & vbCrLf & "Else"
                    '        vbs = vbs & vbCrLf & "set btnCancel = document.getElementById(""hidButtonCancel"")"
                    '        vbs = vbs & vbCrLf & "btnCancel.Click"
                    '        vbs = vbs & vbCrLf & "End If"
                    '        vbs = vbs & "</script>"
                    '        Dim rndKey As New Random
                    '        Me.RegisterStartupScript(rndKey.Next.ToString, vbs)
                    '        Session("Clicked") = "Yes"
                    '        Exit Sub
                    '    End If
                    'End If

                    'Dim idx = 10
                    'For i As Integer = 0 To aryDoc.Count - 1
                    '    If aryDoc(i)(21) Is Nothing Then
                    '        idx = i
                    '        Exit For
                    '    ElseIf aryDoc(i)(21) = "" Then
                    '        idx = i
                    '        Exit For
                    '    End If
                    'Next

                    'If Not idx = 0 Then
                    '    If CDec(FormatNumber(diff, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff, 2)) > CDec(FormatNumber(0, 2)) Then
                    '        aryDoc(idx - 1)(21) = aryDoc(idx - 1)(21) + diff
                    '    ElseIf CDec(FormatNumber(diff3, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff3, 2)) > CDec(FormatNumber(0, 2)) Then
                    '        aryDoc(idx - 1)(21) = aryDoc(idx - 1)(21) + diff3
                    '    ElseIf CDec(FormatNumber(diff2, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff2, 2)) > CDec(FormatNumber(0, 2)) Then
                    '        aryDoc(idx - 1)(21) = aryDoc(idx - 1)(21) - diff2
                    '    ElseIf diff >= "-2.00" And FormatNumber(diff, 2) < FormatNumber(0, 2) Then
                    '        aryDoc(idx - 1)(21) = aryDoc(idx - 1)(21) + diff
                    '    ElseIf diff3 >= "-2.00" And FormatNumber(diff3, 2) < FormatNumber(0, 2) Then
                    '        aryDoc(idx - 1)(21) = aryDoc(idx - 1)(21) + diff3
                    '    ElseIf diff2 >= "-2.00" And FormatNumber(diff2, 2) < FormatNumber(0, 2) Then
                    '        aryDoc(idx - 1)(21) = aryDoc(idx - 1)(21) - diff2
                    '    End If
                    'ElseIf idx = 0 Then
                    '    If CDec(FormatNumber(diff, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff, 2)) > CDec(FormatNumber(0, 2)) Then
                    '        aryDoc(idx)(21) = aryDoc(idx)(21) + diff
                    '    ElseIf CDec(FormatNumber(diff3, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff3, 2)) > CDec(FormatNumber(0, 2)) Then
                    '        aryDoc(idx)(21) = aryDoc(idx)(21) + diff3
                    '    ElseIf CDec(FormatNumber(diff2, 2)) <= CDec(FormatNumber(2, 2)) And CDec(FormatNumber(diff2, 2)) > CDec(FormatNumber(0, 2)) Then
                    '        aryDoc(idx)(21) = aryDoc(idx)(21) - diff2
                    '    ElseIf diff >= "-2.00" And FormatNumber(diff, 2) < FormatNumber(0, 2) Then
                    '        aryDoc(idx)(21) = aryDoc(idx)(21) + diff
                    '    ElseIf diff3 >= "-2.00" And FormatNumber(diff3, 2) < FormatNumber(0, 2) Then
                    '        aryDoc(idx)(21) = aryDoc(idx)(21) + diff3
                    '    ElseIf diff2 >= "-2.00" And FormatNumber(diff2, 2) < FormatNumber(0, 2) Then
                    '        aryDoc(idx)(21) = aryDoc(idx)(21) - diff2
                    '    End If
                    'End If

                    'Jules 2018.07.09 - Allow "\" and "#"
                    objipp.SaveIPPDocDetail(aryDoc, Replace(Replace(Request.QueryString("docno"), "\", "\\"), "#", "\#"), venidx, IIf(Request.QueryString("MasterDoc") = "Yes", "Y", "N"))
                    Session("Clicked") = Nothing

                End If
                Session("SelectedSubDesc") = Nothing
                Session("rowIdx") = Nothing
                Response.Write("<script language=""javascript"">window.close();</script>")
            Else
            End If
        End If
    End Sub
    Private Function validateInput() As Boolean
        Dim count, i, k As Integer
        Dim ds, dsCAD, dsRuleCategory As New DataSet
        Dim compidx As Integer
        Dim dsIPPDocDetailsCount, j As Integer
        Dim strscript As New System.Text.StringBuilder
        Dim invidx As Integer
        Dim intCostAllocIndex As Integer
        Dim decTtlPct As Decimal
        Dim objIPPMain As New IPPMain
        Dim strSql As String = ""
        Dim GST As New GST
        Dim totalInvAmount = 0.0
        Dim totalItemAmount = 0.0

        ''Zulham 13052015 IPP GST Stage 1
        ''Limit the excel items to 500
        'Dim venIdx = objDB.GetVal("SELECT IFNULL(ic_index,'') 'ic_index' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
        'Dim itemCount = objDB.GetVal("Select count(*) from invoice_details where id_invoice_no = '" & Request.QueryString("docno") & "' and id_s_coy_id = '" & venIdx & "'")
        'If itemCount.ToString.Trim.Length <> 0 Then
        '    If CInt(itemCount) = CInt(500) Then
        '        strMsg = "Maximum number allowed for invoice items is 500."
        '        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
        '        Return False
        '    End If
        'End If

        For i = 0 To aryDoc.Count - 1

            'Zulham 27072015 - IPP-GST Stage 4(CR)
            If aryDoc.Item(i)(1) Is Nothing And Session("CompanyID").ToString.ToUpper = "HLISB" Then
                'Zulham 05102015
                'aryDoc.Item(i)(1) = "HLISB"
                aryDoc.Item(i)(1) = "Own Co."
            End If

            If aryDoc.Item(i)(3) <> "" And aryDoc.Item(i)(5) <> "" And aryDoc.Item(i)(6) <> "" And aryDoc.Item(i)(8) <> "" Then

                'Jules 2018.07.09 - Allow "\" and "#"
                'Zulham 11/03/2015 IPP GST Stage 2b
                Dim paymentTerm = objDB.GetVal("SELECT IFNULL(im_payment_term,'') 'im_payment_term' FROM invoice_mstr WHERE im_invoice_no = '" & Common.Parse(Replace(Replace(Request.QueryString("docno"), "\", "\\"), "#", "\#")) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                If Not aryDoc.Item(i)(1) = "Own Co." And paymentTerm.toupper = "NOSTRO" Then
                    strMsg = "Nostro expenses is only applicable to own company."
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Return False
                End If

                If aryDoc.Item(i)(1) = "Own Co." Then
                    aryDoc.Item(i)(1) = Common.Parse(HttpContext.Current.Session("CompanyID"))
                End If

                If aryDoc.Item(i)(11) <> "" Then
                    'intCostAllocIndex = CInt(objDB.GetVal("SELECT cam_index FROM cost_alloc_mstr WHERE cam_ca_code = '" & aryDoc.Item(i)(11) & "' AND cam_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"))
                End If

                If InStr(aryDoc.Item(i)(8), ":") Then
                    aryDoc.Item(i)(8) = aryDoc.Item(i)(8).Substring(0, InStr(aryDoc.Item(i)(8), ":") - 1)
                End If

                If aryDoc.Item(i)(24) <> "" Then
                    If InStr(aryDoc.Item(i)(24), ":") = 1 Then
                        If aryDoc.Item(i)(24).Substring(0, 1) = 1 Or aryDoc.Item(i)(24).Substring(0, 1) = 2 Then
                            'aryDoc.Item(i)(24) = aryDoc.Item(i)(24).Substring(0, InStr(aryDoc.Item(i)(24), ":") - 1)
                            aryDoc.Item(i)(35) = aryDoc.Item(i)(24).Substring(0, 1)
                            aryDoc.Item(i)(24) = aryDoc.Item(i)(24).ToString.Split(":")(1)
                        Else
                            aryDoc.Item(i)(36) = aryDoc.Item(i)(24).ToString.Split(":")(1)
                            aryDoc.Item(i)(35) = aryDoc.Item(i)(24).Substring(0, 1)
                        End If
                    Else
                        If aryDoc.Item(i)(35) <> "" Then
                            If aryDoc.Item(i)(35) = 1 Or aryDoc.Item(i)(35) = 2 Then
                                aryDoc.Item(i)(24) = aryDoc.Item(i)(24)
                            Else
                                aryDoc.Item(i)(36) = aryDoc.Item(i)(24)
                                aryDoc.Item(i)(24) = "NULL" 'Jules 2018.07.05
                            End If
                        End If
                    End If

                End If
                ''mimi 2018-04-17 : withholding tax
                'If InStr(aryDoc.Item(i)(24), ":") Then
                '    aryDoc.Item(i)(24) = aryDoc.Item(i)(24).Substring(0, InStr(aryDoc.Item(i)(24), ":") - 1)
                'End If

                ds = objDoc.ReqCCnAG(aryDoc.Item(i)(8))

                If aryDoc.Item(i)(8) <> "" Then
                    If ds.Tables(0).Rows(0)("CBG_AG_REQ") = "Y" And aryDoc.Item(i)(9) = "" Then
                        strMsg = objGlobal.GetErrorMessage("00036")
                        strMsg = strMsg & "[" & aryDoc.Item(i)(8) & "]"
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        Return False
                    End If
                End If
                '''''''

                Try
                    strSql = "SELECT igc_glrule_category_index, igc_glrule_category, igc_glrule_category_remark FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" &
                    " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " &
                    "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And igg_gl_code LIKE '" & aryDoc.Item(i)(8) & "' GROUP BY igc_glrule_category"
                    dsRuleCategory = objDB.FillDs(strSql)
                    Dim _boolExist As Boolean = False
                    If dsRuleCategory.Tables(0).Rows.Count > 0 Then
                        If aryDoc.Item(i)(18).ToString = "" Then
                            strMsg = "Sub Description for GLCode " & aryDoc.Item(i)(8) & " is required."
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                            Return False
                        Else
                            For Each row As DataRow In dsRuleCategory.Tables(0).Rows
                                If row("igc_glrule_category").ToString.Trim = aryDoc.Item(i)(18).ToString.Trim Then
                                    _boolExist = True
                                    Exit For
                                End If
                            Next
                            If _boolExist = False Then
                                strMsg = "Sub Description is not valid."
                                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                Return False
                            End If
                        End If
                    End If
                Catch ex As Exception

                End Try

                'Jules 2018.04.10 - PAMB Scrum 1 - Commented.
                'If aryDoc.Item(i)(12) <> "" Then 'Branch
                '    If InStr(aryDoc.Item(i)(12), ":") Then
                '        aryDoc.Item(i)(12) = aryDoc.Item(i)(12).Substring(0, InStr(aryDoc.Item(i)(12), ":") - 1)
                '    End If
                '    If objDB.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                '        If objDB.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iub_branch_code = '" & aryDoc.Item(i)(12) & "'") = 0 Then
                '            strMsg = "You have no permission to use this HO/BR Code."
                '            strMsg = strMsg & "[" & aryDoc.Item(i)(12) & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        End If
                '    Else
                '        If objDB.Exist("SELECT '*' FROM company_branch_mstr WHERE cbm_branch_code = '" & aryDoc.Item(i)(12) & "' and cbm_status = 'A' and cbm_coy_id = '" & aryDoc.Item(i)(1) & "'") = 0 Then
                '            strMsg = "Invalid HO/BR Code."
                '            strMsg = strMsg & "[" & aryDoc.Item(i)(12) & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        End If
                '    End If
                'ElseIf aryDoc.Item(i)(12) = "" And (UCase(aryDoc.Item(i)(1)) = "HLB" Or UCase(aryDoc.Item(i)(1)) = "HLISB") Then
                '    strMsg = "HO/BR is required."
                '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '    Return False
                'End If
                'End commented block.

                If aryDoc.Item(i)(13) <> "" Then 'cost center
                    If InStr(aryDoc.Item(i)(13), ":") Then
                        aryDoc.Item(i)(13) = aryDoc.Item(i)(13).Substring(0, InStr(aryDoc.Item(i)(13), ":") - 1)
                    End If
                    If objDB.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                        If objDB.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '" & aryDoc.Item(i)(13) & "'") = 0 Then
                            strMsg = "You have no permission to use this Cost Center."
                            strMsg = strMsg & "[" & aryDoc.Item(i)(13) & "]"
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                            Return False
                        End If
                    Else
                        If Not aryDoc.Item(i)(13).ToString.Trim = "" Then
                            'Dim getSQL = "SELECT '*' FROM cost_centre WHERE cc_cc_code = '" & aryDoc.Item(i)(13).ToString.Trim & "' and cc_status = 'A' and cc_coy_id = '" & aryDoc.Item(i)(1) & "'"
                            'If objDB.Exist("SELECT '*' FROM cost_centre WHERE cc_cc_code = '" & aryDoc.Item(i)(13).ToString.Trim & "' and cc_status = 'A' and cc_coy_id = '" & aryDoc.Item(i)(1) & "'") = 0 Then
                            '    strMsg = "Invalid Cost Center."
                            '    strMsg = strMsg & "[" & aryDoc.Item(i)(13) & "]"
                            '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                            '    Return False
                            'End If
                        End If
                    End If

                Else 'to prevent user no enter for CC without permission to charge 000 
                    If objDB.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                        'Zulham 17102018 - PAMB
                        If objDB.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' ") = 0 Then
                            strMsg = "Cost Center is required."
                            strMsg = strMsg & "[" & aryDoc.Item(i)(13) & "]"
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                            Return False
                        End If
                    End If
                    'End If
                End If

                'Jules 2018.04.10 - PAMB Scrum 1 - Commented.
                ''Check For BranchCode and CC combination
                'If aryDoc.Item(i)(12) <> "" And aryDoc.Item(i)(13) <> "" Then
                '    'Zulham 01062015 IPP GST Stage 1
                '    'Use payfor company instead of login company for bcc_coy_id
                '    'Dim sqlStr = "SELECT CC_CC_CODE, CC_CC_DESC FROM BRANCH_COST_CENTRE " & _
                '    '    "INNER JOIN COST_CENTRE ON BCC_CC_CODE = CC_CC_CODE AND BCC_COY_ID = CC_COY_ID " & _
                '    '    "WHERE BCC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND BCC_BRANCH_CODE = '" & Common.Parse(aryDoc.Item(i)(12)) & "' and CC_CC_CODE = '" & Common.Parse(aryDoc.Item(i)(13)) & "'" & _
                '    '    "ORDER BY CC_CC_CODE "
                '    Dim sqlStr = "SELECT CC_CC_CODE, CC_CC_DESC FROM BRANCH_COST_CENTRE " & _
                '    "INNER JOIN COST_CENTRE ON BCC_CC_CODE = CC_CC_CODE AND BCC_COY_ID = CC_COY_ID " & _
                '    "WHERE BCC_COY_ID = '" & Common.Parse(aryDoc.Item(i)(1)) & "' AND BCC_BRANCH_CODE = '" & Common.Parse(aryDoc.Item(i)(12)) & "' and CC_CC_CODE = '" & Common.Parse(aryDoc.Item(i)(13)) & "'" & _
                '    "ORDER BY CC_CC_CODE "
                '    Dim dsCombination = objDB.FillDs(sqlStr)
                '    If CType(dsCombination, DataSet).Tables(0).Rows.Count > 0 Then
                '        strMsg = "Invalid Cost Center."
                '        strMsg = strMsg & "[" & aryDoc.Item(i)(12) & "]"
                '        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '        Return False
                '    End If
                'End If
                ''End

                'If aryDoc.Item(i)(8) <> "" And aryDoc.Item(i)(12) <> "" Then
                '    If InStr(1, aryDoc.Item(i)(8).ToString, "7") = 1 _
                '    And objDB.GetVal("SELECT cbm_branch_type FROM company_branch_mstr WHERE cbm_branch_code = '" & aryDoc.Item(i)(12) & "' and cbm_status = 'A' and cbm_coy_id = '" & aryDoc.Item(i)(1) & "'") = "HO" _
                '        And aryDoc.Item(i)(12).ToString.Trim = "900" _
                '        And Common.parseNull(aryDoc.Item(i)(13)) = "" Then

                '        strMsg = objGlobal.GetErrorMessage("00037")
                '        strMsg = strMsg & "[" & aryDoc.Item(i)(8) & "]"
                '        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '        Return False
                '    End If
                'End If
                'End commented block.

                'Check Invoice validity for CN and DN
                If Not aryDoc.Item(i)(2) Is Nothing Then
                    If Not aryDoc.Item(i)(2) = "" Then
                        'IPP GST Stage 2A - CH - 11 Feb 2015
                        'Dim isValidInvNo = objDB.GetVal("select '*' from invoice_mstr where im_b_coy_id = 'hlb' and im_invoice_no='" & Common.Parse(aryDoc.Item(i)(2)) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "' AND (invoice_mstr.im_invoice_status = '4' OR invoice_mstr.im_invoice_status = '13')")
                        'Dim isValidInvNo = objDB.GetVal("select '*' from invoice_mstr where im_b_coy_id = '" & Session("CompanyId") & "' and im_invoice_no='" & Common.Parse(aryDoc.Item(i)(2)) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "' AND (invoice_mstr.im_invoice_status = '4' OR invoice_mstr.im_invoice_status = '13')")
                        'Zulham 05/01/2016 - IPP STAGE 4 PHASE 2 (CR)
                        'Allow to raise Debit Note and Credit Note against the Invoice with 'Submitted' status.
                        Dim isValidInvNo = objDB.GetVal("select '*' from invoice_mstr where im_b_coy_id = '" & Session("CompanyId") & "' and im_invoice_no='" & Common.Parse(aryDoc.Item(i)(2)) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "' AND (invoice_mstr.im_invoice_status not in ('10','15'))")
                        If isValidInvNo = "" Then
                            strMsg = "Invalid invoice no."
                            strMsg = strMsg & "[" & aryDoc.Item(i)(2) & "]"
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                            Return False
                        End If

                        'IPP GST Stage 2A - CH - 11 Feb 2015
                        'Dim invAmount = objDB.GetVal("select IF(im_payment_tERM = 'TT',im_invoice_totaL*im_exchange_rATE, IM_INVOICE_TOTAL) 'IM_INVOICE_TOTAL'  from invoice_mstr where im_b_coy_id = 'hlb' and im_invoice_no='" & Common.Parse(aryDoc.Item(i)(2)) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "' AND (invoice_mstr.im_invoice_status = '4' OR invoice_mstr.im_invoice_status = '13')")
                        'Dim invAmount = objDB.GetVal("select IF(im_payment_tERM = 'TT',im_invoice_totaL*im_exchange_rATE, IM_INVOICE_TOTAL) 'IM_INVOICE_TOTAL'  from invoice_mstr where im_b_coy_id = '" & Session("CompanyId") & "' and im_invoice_no='" & Common.Parse(aryDoc.Item(i)(2)) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "' AND (invoice_mstr.im_invoice_status = '4' OR invoice_mstr.im_invoice_status = '13')")
                        'Zulham 05/01/2016 - IPP STAGE 4 PHASE 2 (CR)
                        'Allow to raise Debit Note and Credit Note against the Invoice with 'Submitted' status.
                        Dim invAmount = objDB.GetVal("select IF(im_payment_tERM = 'TT',im_invoice_totaL*ifnull(im_exchange_rATE,1), IM_INVOICE_TOTAL) 'IM_INVOICE_TOTAL'  from invoice_mstr where im_b_coy_id = '" & Session("CompanyId") & "' and im_invoice_no='" & Common.Parse(aryDoc.Item(i)(2)) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "' AND (invoice_mstr.im_invoice_status not in ('10','15'))")
                        totalInvAmount += invAmount
                        If Not aryDoc.Item(i)(2).ToString.Length = 0 Then totalItemAmount += CDec(aryDoc.Item(i)(7)) + CDec(IIf(aryDoc.Item(i)(21) = "", 0, aryDoc.Item(i)(21)))

                        'Zulham 08/06/2015 IPP GST Stage 1
                        'Deduct GST Amount from the total if OutputTaxCode is not nothing
                        If Not aryDoc.Item(i)(23) Is Nothing Then
                            totalItemAmount = CDec(totalItemAmount) - CDec(IIf(aryDoc.Item(i)(21) = "", 0, aryDoc.Item(i)(21)))
                        End If

                    End If
                End If
                'End

                'Zulham 26052015 IPP GST Stage 1
                'Skip item with taxcode = Nothing
                If Not aryDoc.Item(i)(22) Is Nothing Then
                    'Zulham 25102018
                    If InStr(aryDoc.Item(i)(22), "Select") Then
                        strMsg = "Invalid SST Tax Code."
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        Return False
                    End If

                    'Zulham 31102018
                    ''Zulham 18052015 IPP GST Stage 1
                    ''Validity of gst amount based on input tax percentage
                    'Dim dsTaxCodeInfo As New DataSet : dsTaxCodeInfo = GST.GetTaxInfoByTaxCode_forIPP(aryDoc.Item(i)(22))
                    '    If Not dsTaxCodeInfo.Tables(0).Rows.Count = 0 Then
                    '        If Not dsTaxCodeInfo.Tables(0).Rows(0).Item(0).ToString.Trim.Contains("0") Then
                    '            If aryDoc.Item(i)(21).ToString.Trim.Length = 0 Then
                    '                strMsg = "Invalid GST Amount."
                    '                strMsg = strMsg & "[" & aryDoc.Item(i)(21) & "]"
                    '                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    '                Return False
                    '            ElseIf FormatNumber(CDec(aryDoc.Item(i)(21)), 2) = FormatNumber(CDec(0), 2) Then
                    '                strMsg = "Invalid GST Amount."
                    '                strMsg = strMsg & "[" & aryDoc.Item(i)(21) & "]"
                    '                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    '                Return False
                    '            End If
                    '        Else
                    '            'Zulham 08102018 - PAMB SST
                    '            Dim taxPercent = GST.getTaxPercentage(aryDoc.Item(i)(22))
                    '            If Not aryDoc.Item(i)(21).ToString.Trim.Length = 0 And taxPercent = 0 Then
                    '                If FormatNumber(CDec(aryDoc.Item(i)(21)), 2) > FormatNumber(CDec(0), 2) Then
                    '                    strMsg = "Invalid GST Amount."
                    '                    strMsg = strMsg & "[" & aryDoc.Item(i)(21) & "]"
                    '                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    '                    Return False
                    '                End If
                    '            End If
                    '        End If
                    '    End If
                    'Else
                    '    'InputTax n/A but the gst amount has value > 0
                    '    'Zulham 02062015 IPP GST Stage 1
                    '    'To skip gst amount with "" value
                    '    If Not aryDoc.Item(i)(21).ToString.Trim.Length = 0 Then
                    '    If FormatNumber(CDec(aryDoc.Item(i)(21)), 2) > FormatNumber(CDec(0), 2) Then
                    '        strMsg = "Invalid GST Amount."
                    '        strMsg = strMsg & "[" & aryDoc.Item(i)(21) & "]"
                    '        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    '        Return False
                    '    End If
                    'End If
                End If

                'Zulham 31102018
                ''Zulham 27/10/2015 - Check for aryDoc.Item(i)(22) data
                'If Not aryDoc.Item(i)(22) Is Nothing Then
                '    'Check the value for IM2 tax amount
                '    If aryDoc.Item(i)(22).ToString.Trim = "IM2" Then
                '        Dim strGSTPerc As String
                '        Dim GSTAmount As Double = 0.0
                '        Dim dsInputTax = GST.GetTaxCode_forIPP("", "P")
                '        Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                '        For Each row As DataRow In strPerc
                '            strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                '        Next
                '        If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                '            GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                '            'Zulham 15042015 IPP GST Stage 1
                '            GSTAmount = FormatNumber(GSTAmount, 2)
                '        End If
                '        'Im2 shouldnt be a valid selection for residentComp
                '        If Request.QueryString("isResident") = True Then
                '            strMsg = "Invalid selection for GST Input Tax[Line " & i + 1 & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        End If
                '        If CDec(GSTAmount) <> CDec(aryDoc.Item(i)(21)) Then
                '            strMsg = "Invalid GST Amount."
                '            strMsg = strMsg & "[" & aryDoc.Item(i)(21) & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        End If
                '    End If
                '    'End
                'End If

                If aryDoc.Item(i)(8) <> "" And aryDoc.Item(i)(11) <> "" Then

                    'decTtlPct = objDoc.GetSUMCostAllocDetail(intCostAllocIndex)

                    'If decTtlPct < 100 Then
                    '    strMsg = objGlobal.GetErrorMessage("00052")
                    '    strMsg = strMsg & " [" & aryDoc.Item(i)(11) & "]"
                    '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    '    Return False
                    'End If

                    'dsCAD = objDoc.CheckCostAllocDetail(intCostAllocIndex)

                    'For k = 0 To dsCAD.Tables(0).Rows.Count - 1

                    '    If objDB.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                    '        If objDB.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_branch ON iub_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iub_branch_code = '" & dsCAD.Tables(0).Rows(k)("CAD_BRANCH_CODE") & "'") = 0 Then
                    '            strMsg = "You have no permission to use this HO/BR Code." 'objGlobal.GetErrorMessage("00037")
                    '            strMsg = strMsg & "[" & dsCAD.Tables(0).Rows(k)("CAD_BRANCH_CODE") & "]"
                    '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    '            Return False
                    '        End If
                    '    Else
                    '        If objDB.Exist("SELECT '*' FROM company_branch_mstr WHERE cbm_branch_code = '" & dsCAD.Tables(0).Rows(k)("CAD_BRANCH_CODE") & "' and cbm_status = 'A' and cbm_coy_id = '" & aryDoc.Item(i)(1) & "'") = 0 Then
                    '            strMsg = "Invalid HO/BR Code." 'objGlobal.GetErrorMessage("00037")
                    '            strMsg = strMsg & "[" & dsCAD.Tables(0).Rows(k)("CAD_BRANCH_CODE") & "]"
                    '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    '            Return False
                    '        End If
                    '    End If

                    '    If objDB.Exist("SELECT '*' FROM ipp_usrgrp_user WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "'") > 0 Then
                    '        If objDB.Exist("SELECT '*' FROM ipp_usrgrp_user INNER JOIN ipp_usrgrp_cc  ON iuc_grp_index = iuu_grp_index WHERE iuu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' AND iuc_cc_code = '" & dsCAD.Tables(0).Rows(k)("CAD_CC_CODE") & "'") = 0 Then
                    '            strMsg = "You have no permission to use this Cost Center." 'objGlobal.GetErrorMessage("00037")
                    '            strMsg = strMsg & "[" & dsCAD.Tables(0).Rows(k)("CAD_CC_CODE") & "]"
                    '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    '            Return False
                    '        End If
                    '    Else
                    '        If objDB.Exist("SELECT '*' FROM cost_centre WHERE cc_cc_code = '" & dsCAD.Tables(0).Rows(k)("CAD_CC_CODE") & "' and cc_status = 'A' and cc_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") = 0 Then
                    '            strMsg = "Invalid Cost Center." 'objGlobal.GetErrorMessage("00037")
                    '            strMsg = strMsg & "[" & dsCAD.Tables(0).Rows(k)("CAD_CC_CODE") & "]"
                    '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    '            Return False
                    '        End If
                    '    End If

                    '    If InStr(1, aryDoc.Item(i)(8).ToString, "7") = 1 _
                    '        And objDB.GetVal("SELECT cbm_branch_type FROM company_branch_mstr WHERE cbm_branch_code = '" & dsCAD.Tables(0).Rows(k)("CAD_BRANCH_CODE").ToString & "' and cbm_status = 'A' and cbm_coy_id = '" & aryDoc.Item(i)(1) & "'") = "HO" _
                    '        And dsCAD.Tables(0).Rows(k)("CAD_BRANCH_CODE").ToString = "900" _
                    '        And Common.parseNull(dsCAD.Tables(0).Rows(k)("CAD_CC_CODE")) = "" Then

                    '        strMsg = objGlobal.GetErrorMessage("00037")
                    '        strMsg = strMsg & "[" & aryDoc.Item(i)(11) & "]"
                    '        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    '        Return False
                    '    End If

                    'Next
                End If

                'Zulham 11092014
                'Both Input Tax Code and Output Tax Code must be of the same range of tax percentage.
                Dim outputTaxRate = 0
                Dim inputTaxRate = 0

                'Zulham 31102018
                ''If both Input Tax Code and Output Tax Code selected as 0%, GST amount must be zero.
                'If exceedCutOffDt = "Yes" And (strIsGst = "Yes" Or compType = "E") Then
                '    If Not aryDoc.Item(i)(22) Is Nothing Then
                '        If aryDoc.Item(i)(22) = "Select" Then
                '            strMsg = "Please Select GST Input Tax.[Line " & i + 1 & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        End If
                '    End If
                '    If Not aryDoc.Item(i)(23) Is Nothing Then
                '        If aryDoc.Item(i)(23) = "Select" Then
                '            strMsg = "Please Select GST Output Tax.[Line " & i + 1 & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        End If
                '    End If
                '    'Zulham 27/10/2015 - Check for aryDoc.Item(i)(22) item
                '    If Not aryDoc.Item(i)(22) Is Nothing Then
                '        If aryDoc.Item(i)(22).ToString.Trim = "NR" Then
                '            strMsg = "Invalid selection for GST Input Tax[Line " & i + 1 & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        End If
                '    End If
                'ElseIf exceedCutOffDt = "Yes" And strIsGst = "No" And hidResidentType.Value = "Y" Then
                '    'Check for non-GST-registered Company. They can only select NR
                '    If Not aryDoc.Item(i)(22) Is Nothing Then
                '        If aryDoc.Item(i)(22).ToString.Trim = "Select" Then
                '            strMsg = "Please Select GST Input Tax.[Line " & i + 1 & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        End If
                '    End If
                '    If Not aryDoc.Item(i)(23) Is Nothing Then
                '        If aryDoc.Item(i)(23).ToString.Trim = "Select" Then
                '            strMsg = "Please Select GST Output Tax.[Line " & i + 1 & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        End If
                '    End If
                '    If Not aryDoc.Item(i)(22) Is Nothing Then
                '        If aryDoc.Item(i)(22).ToString.Trim <> "NR" Then
                '            strMsg = "Invalid selection for GST Input Tax[Line " & i + 1 & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        ElseIf aryDoc.Item(i)(22) = "NR" Then
                '            If Not aryDoc.Item(i)(21) Is Nothing Then
                '                If Not aryDoc.Item(i)(21).ToString.Length = 0 Then
                '                    If CDec(aryDoc.Item(i)(21)) > CDec(0) Then
                '                        strMsg = "GST Amount must be 0.[Line " & i + 1 & "]"
                '                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '                        Return False
                '                    End If
                '                End If
                '            End If
                '        End If
                '    End If
                'End If

                'Jules 2018.07.10 - Ignore output tax.
                'If Not aryDoc.Item(i)(22) Is Nothing Then
                '    'IPP Gst Stage 2A - CH - 11 Feb 2015
                '    'inputTaxRate = objDB.GetVal("SELECT TAX_PERC AS 'GSTRate' FROM tax_mstr, tax WHERE tm_coy_id = 'hlb' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code AND tm_tax_code = '" & aryDoc.Item(i)(22) & "'")
                '    inputTaxRate = objDB.GetVal("SELECT TAX_PERC AS 'GSTRate' FROM tax_mstr, tax WHERE tm_coy_id = '" & Session("CompanyId") & "' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code AND tm_tax_code = '" & aryDoc.Item(i)(22) & "'")
                '    If Not aryDoc.Item(i)(23) Is Nothing Then
                '        outputTaxRate = objDB.GetVal("SELECT TAX_PERC AS 'GSTRate' FROM tax_mstr, tax WHERE tm_coy_id = '" & Session("CompanyId") & "' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code AND tm_tax_code = '" & aryDoc.Item(i)(23) & "'")
                '    End If
                '    If Not aryDoc.Item(i)(22) Is Nothing And Not aryDoc.Item(i)(23) Is Nothing Then
                '        If Not inputTaxRate = outputTaxRate Then
                '            strMsg = "Selected input and output tax must be the same[Line " & i + 1 & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        End If
                '    End If
                'End If

                'Check for invalid input & output tax code combination
                'If Not aryDoc.Item(i)(22) Is Nothing And Not aryDoc.Item(i)(23) Is Nothing Then
                '    If Not (aryDoc.Item(i)(22) = "IM1" Or aryDoc.Item(i)(22) = "IM3" Or Request.QueryString("isResident") = True) Then
                '        If aryDoc.Item(i)(22).ToString.Contains("(") And Not aryDoc.Item(i)(23).ToString.Contains("(") Then
                '            strMsg = "Selected input and output tax must be the same[Line " & i + 1 & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        ElseIf aryDoc.Item(i)(22).ToString.Split("(")(1).Substring(0, 1) <> aryDoc.Item(i)(23).ToString.Split("(")(1).Substring(0, 1) Then
                '            strMsg = "Selected input and output tax must be the same[Line " & i + 1 & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        ElseIf Not aryDoc.Item(i)(22).ToString.Contains("(") And aryDoc.Item(i)(23).ToString.Contains("(") Then
                '            strMsg = "Selected input and output tax must be the same[Line " & i + 1 & "]"
                '            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '            Return False
                '        End If
                '    End If
                'End If

                'If both Input Tax Code and Output Tax Code are 0% selected, GST amount must be zero.
                'If CType(aryDoc.Item(i)(21), Double) > 0 And inputTaxRate = "" Then
                '    strMsg = "Invalid GST Amount"
                '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '    Return False
                'End If

                'If CType(aryDoc.Item(i)(21), Double) > 0 And CDec(inputTaxRate) = 0 Then
                '    strMsg = "Invalid GST Amount"
                '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '    Return False
                'End If

                'Invoice header amount must be equal to the detail amount inclusive of GST amount.
                'Invoice header amount must be equal to the Sub document amount inclusive of GST amount.
                'All payment to Employee will be performed as vendor’s and will require user to enter the GST Input Tax Code.

                'End

                'Jules 2018.04.16 - PAMB Scrum 1 - Analysis Codes
                Dim dsMatrix As DataSet = Nothing
                dsMatrix = objIPPMain.getGLCodeAnalysisCodeMatrix(aryDoc.Item(i)(8))
                If dsMatrix IsNot Nothing Then
                    For ac As Integer = 1 To 9
                        If ac <> 6 AndAlso ac <> 7 Then
                            'Zulham 07122018
                            Dim exist As Integer = 2
                            Dim sqlQuery = ""
                            If Not aryDoc.Item(i)(25 + ac) Is Nothing AndAlso Not aryDoc.Item(i)(25 + ac) = "" Then
                                sqlQuery = "select * from analysis_code where ac_analysis_code ='" & aryDoc.Item(i)(25 + ac).ToString.Split(":")(0).Trim & "' and ac_dept_code = 'L" & ac & "'"
                                exist = objDB.Exist(sqlQuery)
                            End If
                            If exist = 0 Then
                                Select Case ac
                                    Case 1
                                        vldsum.InnerHtml = "<li>Invalid Fund Type [Line " & i + 1 & "]</li>"
                                    Case 2
                                        vldsum.InnerHtml = "<li>Invalid Product Type [Line " & i + 1 & "]</li>"
                                    Case 3
                                        vldsum.InnerHtml = "<li>Invalid Channel [Line " & i + 1 & "]</li>"
                                    Case 4
                                        vldsum.InnerHtml = "<li>Invalid Reinsurance Company [Line " & i + 1 & "]</li>"
                                    Case 5
                                        vldsum.InnerHtml = "<li>Invalid Asset Fund [Line " & i + 1 & "]</li>"
                                    Case 8
                                        vldsum.InnerHtml = "<li>Invalid Project Code [Line " & i + 1 & "]</li>"
                                    Case 9
                                        vldsum.InnerHtml = "<li>Invalid Person Code [Line " & i + 1 & "]</li>"
                                End Select
                                Return False
                            End If

                            If dsMatrix.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE" & ac & "").ToString = "M" Then
                                If aryDoc.Item(i)(25 + ac) Is Nothing OrElse aryDoc.Item(i)(25 + ac) = "" Then
                                    'strMsg = "Analysis Code " & ac & "is required [Line " & i + 1 & "]"
                                    'Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)

                                    If ac = 1 Then
                                        vldsum.InnerHtml = "<li>Fund Type " & objGlobal.GetErrorMessage("00001") & " [Line " & i + 1 & "]</li>"
                                    ElseIf ac = 2 Then
                                        vldsum.InnerHtml = "<li>Product Type " & objGlobal.GetErrorMessage("00001") & " [Line " & i + 1 & "]</li>"
                                    ElseIf ac = 3 Then
                                        vldsum.InnerHtml = "<li>Channel " & objGlobal.GetErrorMessage("00001") & " [Line " & i + 1 & "]</li>"
                                    ElseIf ac = 4 Then
                                        vldsum.InnerHtml = "<li>Reinsurance Company " & objGlobal.GetErrorMessage("00001") & " [Line " & i + 1 & "]</li>"
                                    ElseIf ac = 5 Then
                                        vldsum.InnerHtml = "<li>Asset Fund " & objGlobal.GetErrorMessage("00001") & " [Line " & i + 1 & "]</li>"
                                    ElseIf ac = 8 Then
                                        vldsum.InnerHtml = "<li>Project Code " & objGlobal.GetErrorMessage("00001") & " [Line " & i + 1 & "]</li>"
                                    ElseIf ac = 9 Then
                                        vldsum.InnerHtml = "<li>Person Code " & objGlobal.GetErrorMessage("00001") & " [Line " & i + 1 & "]</li>"
                                    End If
                                    Return False
                                End If
                            End If
                            If aryDoc.Item(i)(25 + ac) IsNot Nothing Then
                                If InStr(aryDoc.Item(i)(25 + ac), ":") Then
                                    aryDoc.Item(i)(25 + ac) = aryDoc.Item(i)(25 + ac).Substring(0, InStr(aryDoc.Item(i)(25 + ac), ":") - 1)
                                End If
                            End If
                        ElseIf ac = 6 Then 'Tax Code
                            If aryDoc.Item(i)(22) Is Nothing AndAlso aryDoc.Item(i)(23) Is Nothing Then
                                vldsum.InnerHtml = "<li>Tax Code " & objGlobal.GetErrorMessage("00001") & " [Line " & i + 1 & "]</li>"
                                Return False
                            End If
                        ElseIf ac = 7 Then 'Cost Centre
                            If aryDoc.Item(i)(13) Is Nothing OrElse aryDoc.Item(i)(13) = "" Then
                                vldsum.InnerHtml = "<li>Cost Centre " & objGlobal.GetErrorMessage("00001") & " [Line " & i + 1 & "]</li>"
                                Return False
                            End If
                        End If
                    Next
                End If
                'End modification.
            End If
        Next

        'Jules 2018.07.09 - Allow "\" and "#"
        'IPP Gst Stage 2A - CH - 11 Feb 2015
        'Dim paymentType = objDB.GetVal("select IFNULL(im_invoice_type,'') 'im_invoice_type' from invoice_mstr where im_b_coy_id = 'hlb' and im_invoice_no='" & Common.Parse(Request.QueryString("docno")) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
        Dim paymentType = objDB.GetVal("select IFNULL(im_invoice_type,'') 'im_invoice_type' from invoice_mstr where im_b_coy_id = '" & Session("CompanyId") & "' and im_invoice_no='" & Common.Parse(Replace(Replace(Request.QueryString("docno"), "\", "\\"), "#", "\#")) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
        If paymentType.ToString.Contains("CN") Then
            If Not totalInvAmount = 0.0 And Not totalItemAmount = 0.0 Then
                If CDec(totalItemAmount) > CDec(totalInvAmount) Then
                    strMsg = "Credit Note total amount must be lesser than the invoice total amount."
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Return False

                    'IPP Gst Stage 1 - YapCL - 21 May 2015: The amt can be lesser or same amt with inv amt.
                    'ElseIf CDec(totalItemAmount) = CDec(totalInvAmount) Then
                    '    strMsg = "Credit Note total amount must be lesser than the invoice total amount."
                    '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    '    Return False
                End If
            End If
        End If

        Return True
    End Function
    Private Function validateField() As Boolean
        Dim count, i As Integer
        Dim ds As New DataSet

        For i = 0 To aryDoc.Count - 1
            If aryDoc.Item(0)(3) = "" And aryDoc.Item(0)(5) = "" And aryDoc.Item(0)(6) = "" And aryDoc.Item(0)(8) = "" Then
                vldsum.InnerHtml = "<li>Document Line Item must start from no.1.</li>"
                Return False
            End If

            'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR.
            'Zulham 25092018 - PAMB
            'UAT U00007 - Removed checking for qty & unit price
            If aryDoc.Item(i)(3) <> "" Or aryDoc.Item(i)(8) <> "" Then
                If Not compType = "E" Then
                    If aryDoc.Item(i)(3) = "" Then
                        vldsum.InnerHtml = "<li>Description " & objGlobal.GetErrorMessage("00001") & "</li>"
                        Return False
                    ElseIf CType(aryDoc.Item(i)(7), Double) < 0 Then
                        vldsum.InnerHtml = "<li>Amount must not be in negative value.</li>"
                        Return False
                    ElseIf aryDoc.Item(i)(8) = "" Then
                        vldsum.InnerHtml = "<li>GL Code " & objGlobal.GetErrorMessage("00001") & "</li>"
                        Return False

                        'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR.
                        'IPP GST Stage 2A - CH - 11 Feb 2015
                        'ElseIf aryDoc.Item(i)(12) = "" And (aryDoc.Item(i)(1) = "Own Co." Or UCase(aryDoc.Item(i)(1)) = "HLISB" Or UCase(aryDoc.Item(i)(1)) = "HLB") And aryDoc.Item(i)(11) = "" Then
                        '    vldsum.InnerHtml = "<li>HO/BR " & objGlobal.GetErrorMessage("00001") & "</li>"
                        '    Return False
                        'End modification.
                    End If
                Else
                    If Not (aryDoc.Item(i)(3) = "" And aryDoc.Item(i)(8) = "") Then
                        If aryDoc.Item(i)(3) = "" Then
                            vldsum.InnerHtml = "<li>Description " & objGlobal.GetErrorMessage("00001") & "</li>"
                            Return False
                        ElseIf CType(aryDoc.Item(i)(7), Double) < 0 Then
                            vldsum.InnerHtml = "<li>Amount must not be in negative value.</li>"
                            Return False
                        ElseIf aryDoc.Item(i)(8) = "" Then
                            vldsum.InnerHtml = "<li>GL Code " & objGlobal.GetErrorMessage("00001") & "</li>"
                            Return False

                            'Jules 2018.04.10 - PAMB Scrum 1 - Removed HO/BR
                            'IPP GST Stage 2A - CH - 11 Feb 2015
                            'ElseIf aryDoc.Item(i)(12) = "" And (aryDoc.Item(i)(1) = "Own Co." Or UCase(aryDoc.Item(i)(1)) = "HLISB" Or UCase(aryDoc.Item(i)(1)) = "HLB") And aryDoc.Item(i)(11) = "" Then
                            '    vldsum.InnerHtml = "<li>HO/BR " & objGlobal.GetErrorMessage("00001") & "</li>"
                            '    Return False
                            'End modification.
                        End If
                    End If
                End If
            Else
                Return True
            End If

        Next
        Return True
    End Function

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Write("<script language=""javascript"">window.close();</script>")
    End Sub
    Private Sub hidButton_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidButton.ServerClick
        ConstructTable()
        PopulateTypeAhead()
    End Sub
    Private Function getUserRole(ByVal blnIPPOfficer As Boolean, ByVal blnIPPOfficerS As Boolean) As Integer
        If blnIPPOfficer = False And blnIPPOfficerS = True Then
            getUserRole = 2
        ElseIf blnIPPOfficer = True And blnIPPOfficerS = False Then
            getUserRole = 1
        End If
    End Function

    Private Sub hidButtonTest_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidButtonTest.ServerClick
        cmdSave_Click(cmdSave, e)
        Session("Clicked") = Nothing
    End Sub
    Private Sub hidButtonCancel_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles hidButtonCancel.ServerClick
        Session("Clicked") = Nothing
    End Sub
End Class
