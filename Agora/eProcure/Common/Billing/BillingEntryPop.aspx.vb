'Zulham 30-01-2015 IPP-GST Stage 2A
Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions

Imports System.Drawing

Public Class BillingEntryPop
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
    Protected WithEvents hidMode As System.Web.UI.WebControls.HiddenField


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
            lbltitle.Text = "Modify Billing Document Line"
        End If
        blnIPPOfficer = objUsers.checkUserFixedRole("'IPP Officer(F)'")
        blnIPPOfficerS = objUsers.checkUserFixedRole("'IPP Officer'")
        ViewState("IPPOfficer") = blnIPPOfficer
        ViewState("IPPOfficerS") = blnIPPOfficerS
        ViewState("role") = getUserRole(blnIPPOfficer, blnIPPOfficerS)
        'Check for GST
        Dim gst As New GST
        compType = objDB.GetVal("SELECT IFNULL(ic_coy_type,'') 'ic_coy_type' FROM ipp_company WHERE ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
        Dim documentDate = objDB.GetVal("SELECT IFNULL(bm_created_on,'') 'bm_doc_date' FROM billing_mstr WHERE bm_invoice_no = '" & Common.Parse(Request.QueryString("docno")) & "' and bm_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
        Dim createdDate = Common.FormatWheelDate(WheelDateFormat.LongDate, Date.Today)
        Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")
        If CDate(createdDate) >= CDate(_cutoffDate) Then
            exceedCutOffDt = "Yes"
            hidexceedCutOffDt.Value = "Yes"
            If Request.QueryString("vencomp") IsNot Nothing Then
                Dim GSTRegNo = objDB.GetVal("SELECT IFNULL(IC_TAX_REG_NO, '') FROM IPP_COMPANY WHERE ic_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                If (GSTRegNo <> "" And CDate(documentDate) >= CDate(_cutoffDate)) Or (CDate(documentDate) >= CDate(_cutoffDate) And compType = "E") Then
                    strIsGst = "Yes"
                    hidIsGST.Value = "Yes"
                Else
                    strIsGst = "No"
                    hidIsGST.Value = "No"
                End If
            Else
                strIsGst = "Yes"
                hidIsGST.Value = "Yes"
            End If
        Else
            strIsGst = "No"
            hidIsGST.Value = "No"
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
        Dim vtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&module=billing")
        Dim gltypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=GLCode")
        Dim agtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AssetGroup")
        Dim asgtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AssetSubGroup")
        Dim catypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=CostAlloc")
        Dim rulecattypeahead As String = ""
        rulecattypeahead = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=RuleCategory")

        'Jules 2018.04.23 - PAMB Scrum 2
        Dim actypeahead1 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L1")
        Dim actypeahead2 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L2")
        Dim actypeahead3 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L3")
        Dim actypeahead4 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L4")
        Dim actypeahead5 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L5")
        Dim actypeahead6 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L6")
        Dim actypeahead7 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L7")
        Dim actypeahead8 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L8")
        Dim actypeahead9 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=AnalysisCode&deptcode=L9")
        'End modification.

        Dim venidx As String
        Dim objipp As New IPPMain
        Dim objBill As New Billing

        'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR.
        'Dim cctypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=CostCentre&role=" & ViewState("role") & "")
        Dim cctypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=CostCentre_wo_Branch&role=" & ViewState("role") & "")
        'End modification.

        Dim brtypeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Branch&role=" & ViewState("role") & "")
        'venidx = objipp.getIPPCompanyIndex(Common.Parse(Request.QueryString("vencomp")), ViewState("IPPOfficer"), "I")
        venidx = objBill.getIPPCompIndex(Common.Parse(Request.QueryString("vencomp")), Common.Parse(Request.QueryString("coytype")), "I")
        If Request.QueryString("action") = "edit" Then
            count = 1
        Else
            count = 10
        End If

        For i = 0 To count - 1
            strCompID = Request.Form("ddlPayFor" & i) '"Own Co."

            'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR, added Category and Analysis Codes.
            content &= "$(""#txtGLCode" & i & """).autocomplete(""" & gltypeahead & """, {" & vbCrLf &
            "width: 180," & vbCrLf &
            "scroll: true," & vbCrLf &
            "selectFirst: false" & vbCrLf &
            "}).result(function(event,data,item) {" & vbCrLf &
            "if (data)" & vbCrLf &
            "$(""#hidGLCodeVal" & i & """).val(data[1]);" & vbCrLf &
            "$(""#txtGLCode" & i & """).val(data[1]);" & vbCrLf &
            "//$(""#hidButton" & i & """).trigger('click');" & vbCrLf &
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
            "//$(""#txtCostAlloc" & i & """).autocomplete(""" & catypeahead & """, {" & vbCrLf &
            "//width: 100," & vbCrLf &
            "//scroll: true," & vbCrLf &
            "//selectFirst: false" & vbCrLf &
            "//}).result(function(event,data, item) {" & vbCrLf &
            "//if (data)" & vbCrLf &
            "//$(""#hidCostAllocVal" & i & """).val(data[1]);" & vbCrLf &
            "//$(""#txtBranch" & i & """).attr('disabled','disabled');" & vbCrLf &
            "//$(""#txtCC" & i & """).attr('disabled','disabled');" & vbCrLf &
            "//$(""#txtBranch" & i & """).val('');" & vbCrLf &
            "//$(""#txtCC" & i & """).val('');" & vbCrLf &
            "//var clickevent = $(""#hidCostAlloc" & i & """).val();" & vbCrLf &
            "//var changeclick = updateparam(clickevent,""CostAllocCodeSearch.aspx?"",""CostAllocCodeSearch.aspx?CostAllocCode="" + encodeURIComponent($(""#txtCostAlloc" & i & """).val()) + ""&"");" & vbCrLf &
            "//var newclick = Function(changeclick);" & vbCrLf &
            "//document.getElementById(""btnCostAlloc" & i & """).onclick = newclick;" & vbCrLf &
            "//var CostAlloc = document.getElementById(""txtCostAlloc" & i & """).value;" & vbCrLf &
            "//updatebtnCostAlloc(CostAlloc,'hidCostAlloc2" & i & "','btnCostAlloc2" & i & "','CostAllocDetail.aspx?','CostAllocDetail.aspx?VenIdx=" & venidx & "&InvLine=" & i + 1 & "&InvIdx=" & Session("DocNo") & "&CostAllocCode');" & vbCrLf &
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
            "//type:""POST"",url: ""../../Common/Initial/TypeAhead.aspx?from=IPPCostCentre&branchCode="" + encodeURIComponent($(""#txtBranch" & i & """).val()) + ""&i="" + encodeURIComponent((""" & i & """)) + ""&""," &
            "//cache: false,success: function(data){var a = data.split('|');var dataCount = a.length;if(dataCount<3 && dataCount>1){document.getElementById(""txtCC" & i & """).readOnly=true;$(""#txtCC" & i & """).val(a[0]);$(""#hidCCVal" & i & """).val(a[1]);}" & vbCrLf &
            "//else if(dataCount>2){" & vbCrLf &
            "//$(""#txtCC" & i & """).val("""");$(""#hidCCVal" & i & """).val("""");document.getElementById(""txtCC" & i & """).readOnly=false;" & vbCrLf &
            "//$(""#txtCC" & i & """).autocomplete(""" & cctypeahead & "&i=" & i & "&branchCode="" + encodeURIComponent($(""#txtBranch" & i & """).val()) + """ & """, {" & vbCrLf &
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
            "//$(""#txtCostAlloc" & i & """).blur(function() {" & vbCrLf &
            "//if ($(""#txtCostAlloc" & i & """).val() == """") {" & vbCrLf &
            "//$(""#txtBranch" & i & """).removeAttr('disabled');" & vbCrLf &
            "//$(""#txtCC" & i & """).removeAttr('disabled'); }" & vbCrLf &
            "//});" & vbCrLf &
            "//$(""#txtCostAlloc" & i & """).keyup(function() {" & vbCrLf &
            "//if ($(""#txtCostAlloc" & i & """).val() == """") {" & vbCrLf &
            "//$(""#txtBranch" & i & """).removeAttr('disabled');" & vbCrLf &
            "//$(""#txtCC" & i & """).removeAttr('disabled'); }" & vbCrLf &
            "//});" & vbCrLf &
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
            "//$(""#txtCostAlloc" & i & """).blur(function() {" & vbCrLf &
            "//var hidcaval = document.getElementById(""hidCostAllocVal" & i & """).value;" & vbCrLf &
            "//if(hidcaval == """")" & vbCrLf &
            "//{" & vbCrLf &
            "//$(""#txtCostAlloc" & i & """).val("""");" & vbCrLf &
            "//}" & vbCrLf &
            "//});" & vbCrLf &
             "$(""#txtBranch" & i & """).blur(function() {" & vbCrLf &
            "var hidbrval = document.getElementById(""hidBranchVal" & i & """).value;" & vbCrLf &
            "if(hidbrval == """")" & vbCrLf &
            "{" & vbCrLf &
            "$(""#txtBranch" & i & """).val("""");" & vbCrLf &
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
            "//$(""#txtCostAlloc" & i & """).keyup(function() {" & vbCrLf &
            "//if ($(""#txtCostAlloc" & i & """).val() == """") {" & vbCrLf &
            "//$(""#hidCostAllocVal" & i & """).val(""""); }" & vbCrLf &
            "//});" & vbCrLf &
            "//$(""#txtBranch" & i & """).keyup(function() {" & vbCrLf &
            "//if ($(""#txtBranch" & i & """).val() == """") {" & vbCrLf &
            "//$(""#hidBranchVal" & i & """).val(""""); }" & vbCrLf &
            "//});" & vbCrLf &
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
            "});" & vbCrLf
            '
            ' for edit purpose
            If Request.QueryString("action") = "edit" Then
                content2 = "var clickevent = $(""#hidGLCode" & i & """).val();" & vbCrLf & _
                            "var a = document.getElementById(""hidGLCodeVal" & i & """).value; var changeclick = updateparam(clickevent,""GLCodeSearch.aspx?"",""GLCodeSearch.aspx?GLCode="" + encodeURIComponent(a) + ""&"");" & vbCrLf & _
                            "var newclick = Function(changeclick);" & vbCrLf & _
                            "document.getElementById(""btnGLCode" & i & """).onclick = newclick;" & vbCrLf & _
                            "//var clickevent = $(""#hidCostAlloc" & i & """).val();" & vbCrLf & _
                            "//var changeclick = updateparam(clickevent,""CostAllocCodeSearch.aspx?"",""CostAllocCodeSearch.aspx?CostAllocCode="" + encodeURIComponent($(""#txtCostAlloc" & i & """).val()) + ""&"");" & vbCrLf & _
                            "//var newclick = Function(changeclick);" & vbCrLf & _
                            "//document.getElementById(""btnCostAlloc" & i & """).onclick = newclick;" & vbCrLf & _
                            "//var CostAlloc = document.getElementById(""txtCostAlloc" & i & """).value" & vbCrLf & _
                            "//updatebtnCostAlloc(CostAlloc,'hidCostAlloc2" & i & "','btnCostAlloc2" & i & "','CostAllocDetail.aspx?','CostAllocDetail.aspx?VenIdx=" & venidx & "&InvLine=" & i + 1 & "&InvIdx=" & Session("DocNo") & "&CostAllocCode');" & vbCrLf & _
                            "//if ($(""#txtCostAlloc" & i & """).val() != """") {" & vbCrLf & _
                            "//$(""#txtBranch" & i & """).attr('disabled','disabled');" & vbCrLf & _
                            "//$(""#txtCC" & i & """).attr('disabled','disabled');" & vbCrLf & _
                            "//};" & vbCrLf & _
                            "$(""#txtGLCode" & i & """).blur(function() {" & vbCrLf & _
                            "var hidglcodeval = document.getElementById(""hidGLCodeVal" & i & """).value;" & vbCrLf & _
                            "if(hidglcodeval == """")" & vbCrLf & _
                            "{" & vbCrLf & _
                            "$(""#txtGLCode" & i & """).val("""");" & vbCrLf & _
                            "}" & vbCrLf & _
                            "});" & vbCrLf & _
                            "//$(""#txtCostAlloc" & i & """).blur(function() {" & vbCrLf & _
                            "//var hidcaval = document.getElementById(""hidCostAllocVal" & i & """).value;" & vbCrLf & _
                            "//if(hidcaval == """")" & vbCrLf & _
                            "//{" & vbCrLf & _
                            "//$(""#txtCostAlloc" & i & """).val("""");" & vbCrLf & _
                            "//}" & vbCrLf & _
                            "//});" & vbCrLf & _
                             "$(""#txtBranch" & i & """).blur(function() {" & vbCrLf & _
                            "var hidbrval = document.getElementById(""hidBranchVal" & i & """).value;" & vbCrLf & _
                            "if(hidbrval == """")" & vbCrLf & _
                            "{" & vbCrLf & _
                            "$(""#txtBranch" & i & """).val("""");" & vbCrLf & _
                            "}" & vbCrLf & _
                            "});" & vbCrLf & _
                            "$(""#txtCC" & i & """).blur(function() {" & vbCrLf & _
                            "var hidccval = document.getElementById(""hidCCVal" & i & """).value;" & vbCrLf & _
                            "if(hidccval == """")" & vbCrLf & _
                            "{" & vbCrLf & _
                            "$(""#txtCC" & i & """).val("""");" & vbCrLf & _
                            "}" & vbCrLf & _
                            "});" & vbCrLf & _
                            "$(""#txtGLCode" & i & """).keyup(function(event) {" & vbCrLf & _
                             "if(event.keyCode != ""13"")" & vbCrLf & _
                            "{ $(""#hidGLCodeVal" & i & """).val("""");}" & vbCrLf & _
                            "if ($(""#txtGLCode" & i & """).val() == """") {" & vbCrLf & _
                            "$(""#hidGLCodeVal" & i & """).val(""""); }" & vbCrLf & _
                            "});" & vbCrLf & _
                            "//$(""#txtCostAlloc" & i & """).keyup(function() {" & vbCrLf & _
                            "//if ($(""#txtCostAlloc" & i & """).val() == """") {" & vbCrLf & _
                            "//$(""#hidCostAllocVal" & i & """).val(""""); }" & vbCrLf & _
                            "//});" & vbCrLf & _
                           "$(""#txtBranch" & i & """).keyup(function() {" & vbCrLf & _
                        "if ($(""#txtBranch" & i & """).val() == """") {" & vbCrLf & _
                        "$(""#hidBranchVal" & i & """).val(""""); }" & vbCrLf & _
                        "});" & vbCrLf & _
                           "$(""#txtCC" & i & """).keyup(function() {" & vbCrLf & _
                        "if ($(""#txtCC" & i & """).val() == """") {" & vbCrLf & _
                        "$(""#hidCCVal" & i & """).val(""""); }" & vbCrLf & _
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

    End Sub
    Private Function ConstructTable()
        Dim GST As New GST
        Dim billing As New Billing
        Dim strrow, GSTAmount, strGSTPerc, strGSTID As String
        Dim i, c, count, Sno, record As Integer
        Dim table As String
        Dim item2nd As Boolean = False
        Dim found As Boolean = False
        Dim dsDoc_temp As New ArrayList
        Dim objCAD As New IPP
        Dim objipp As New IPPMain
        Dim objGST As New GST
        Dim objBill As New Billing
        Dim objGlobal As New AgoraLegacy.AppGlobals
        Dim compOutputTaxValue_IM2, compOutputTaxValue_TX4, compInputTaxValue_TX4, compInputTaxValue_Block As String
        Dim dsPayFor, dsUOM, dsInputTax, dsOutputTax As DataSet
        dsPayFor = objDoc.PopPayFor
        dsUOM = objDoc.PopUOM
        buildarydoc()
        compOutputTaxValue_IM2 = objDB.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT'")
        compOutputTaxValue_TX4 = objDB.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'GIFT_LUCKY_OUTPUT'")
        compInputTaxValue_TX4 = objDB.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'GIFT_LUCKY_INPUT'")
        compInputTaxValue_Block = objDB.GetVal("SELECT ifnull(IP_param_value,'') 'IP_param_value' FROM ipp_parameter WHERE ip_param = 'BLOCK'")
        If Request.QueryString("action") = "edit" Then
            hidMode.Value = "edit"
            'get detail from database
            Dim ds As DataSet
            Dim venidx As String
            Sno = Request.QueryString("Lineno")
            'venidx = objipp.getIPPCompanyIndex(Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))), ViewState("IPPOfficer"), "I")
            'Zulham 15042015 IPP GST Stage 2A
            'get vendor index using billing invoiceNo since it's unique
            'Zulham Method to get index
            'venidx = objDB.GetVal("select bm_s_coy_id from billing_mstr where bm_invoice_no = '" & Request.QueryString("docno") & "'")
            'CH Method to get index
            venidx = billing.getIPPCompIndex(Common.Parse(Server.UrlDecode(Request.QueryString("vencomp"))), Common.Parse(Request.QueryString("coytype")), "I")
            ds = billing.GetBillingDocDetails(Request.QueryString("docno"), venidx, Sno, Request.QueryString("olddocno"))
            strrow &= "<tr style=""background-color:#fdfdfd;"">"

            strrow &= "<td align=""right"" style=""display:none;"">"
            strrow &= "<input type=""hidden""  id=""txtSNo" & i & """ name=""txtSNo" & i & """ value=""" & Sno & """>"
            strrow &= "<asp:label style=""width:1%;"" class=""lbl""  id=""lblSNo" & i & """ name=""lblSNo" & i & """ value=""""/>" & Sno & ""
            strrow &= "</td>"

            'Zulham 19/06./2017
            'IPP Stage 3
            If Request.QueryString("doctype") <> "Invoice" And Request.QueryString("doctype") <> "Non-Invoice" And Request.QueryString("doctype") <> "Debit Advice" _
            And Request.QueryString("doctype") <> "Credit Advice" Then
                strrow &= "<td >"
                strrow &= "<input style=""width:85px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtInvNo" & i & """ name=""txtInvNo" & i & """ value=""" & ds.Tables(0).Rows(0).Item("bm_ref_no") & """>"
                strrow &= "</td>"
            End If

            If aryDoc(i)(3) Is Nothing Then
                strrow &= "<td >"
                strrow &= "<input style=""width:145px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtDesc" & i & """ name=""txtDesc" & i & """ value=""" & ds.Tables(0).Rows(0).Item("bm_PRODUCT_DESC") & """>"
                strrow &= "</td>"
            Else
                strrow &= "<td >"
                strrow &= "<input style=""width:145px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtDesc" & i & """ name=""txtDesc" & i & """ value=""" & aryDoc(i)(3) & """>"
                strrow &= "</td>"
            End If

            strrow &= "<td>"
            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlUOM" & i & """ name=""ddlUOM" & i & """>"
            strrow &= "<option value=""Unit"" selected=""selected"">" & "Unit" & "</option>"

            For c = 0 To dsUOM.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(0).Item("bm_UOM") = dsUOM.Tables(0).Rows(c).Item(0).ToString Then
                    strrow &= "<option value=""" & dsUOM.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsUOM.Tables(0).Rows(c).Item(0).ToString & "</option>"
                ElseIf aryDoc(i)(4) IsNot Nothing Then
                    strrow &= "<option value=""" & aryDoc(i)(4).ToString.Trim & """ selected=""selected"">" & dsUOM.Tables(0).Rows(c).Item(0).ToString & "</option>"
                Else
                    strrow &= "<option value=""" & dsUOM.Tables(0).Rows(c).Item(0).ToString & """>" & dsUOM.Tables(0).Rows(c).Item(0).ToString & "</option>"
                End If
            Next
            If aryDoc(i)(5) Is Nothing Then
                strrow &= "<td align=""right"">"
                strrow &= "<span class=""qty""><input style=""width:100px;margin-right:0px;"" class=""numerictxtbox"" onkeypress=""return isDecimalKey(event);"" id=""txtQty" & i & """ onfocus = ""return focusControl('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" name=""txtQty" & i & """ value=""" & ds.Tables(0).Rows(0).Item("bm_RECEIVED_QTY") & """></span>"
                strrow &= "</td>"
            Else
                strrow &= "<td align=""right"">"
                strrow &= "<span class=""qty""><input style=""width:100px;margin-right:0px;"" class=""numerictxtbox"" onkeypress=""return isDecimalKey(event);"" id=""txtQty" & i & """ onfocus = ""return focusControl('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" name=""txtQty" & i & """ value=""" & aryDoc(i)(5) & """></span>"
                strrow &= "</td>"
            End If

            If aryDoc(i)(6) Is Nothing Then
                strrow &= "<td align=""right"">"
                strrow &= "<span class=""unit""><input style=""width:100px;margin-right:0px;"" class=""numerictxtbox""  onkeypress=""return isDecimalKey(event);"" id=""txtUnitPrice" & i & """ onfocus = ""return focusControl('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" name=""txtUnitPrice" & i & """ value=""" & Format(ds.Tables(0).Rows(0).Item("bm_UNIT_COST"), "#.00") & """></span>"
                strrow &= "</td>"
            Else
                strrow &= "<td align=""right"">"
                strrow &= "<span class=""unit""><input style=""width:100px;margin-right:0px;"" class=""numerictxtbox""  onkeypress=""return isDecimalKey(event);"" id=""txtUnitPrice" & i & """ onfocus = ""return focusControl('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" name=""txtUnitPrice" & i & """ value=""" & aryDoc(i)(6) & """></span>"
                strrow &= "</td>"
            End If

            If aryDoc(i)(7) Is Nothing Then
                strrow &= "<td align=""right"">"
                strrow &= "<span class=""amount""><input style=""width:100px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtAmt" & i & """ name=""txtAmt" & i & """ value=""" & (Format(CDbl(ds.Tables(0).Rows(0).Item("bm_AMOUNT")), "#,###,###.00")) & """></span>"
                'strrow &= "<asp:label style=""width:100%;margin-right:0px; "" class=""lbl"" type=""text""  id=""lblAmt" & i & """ name=""lblAmt" & i & """ value=""" & aryDoc(i)(7) & """>"
                strrow &= "</td>"
            Else
                strrow &= "<td align=""right"">"
                strrow &= "<span class=""amount""><input style=""width:100px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtAmt" & i & """ name=""txtAmt" & i & """ value=""" & (Format(CDbl(aryDoc(i)(7)), "#,###,###.00")) & """></span>"
                strrow &= "</td>"
            End If

            'Zulham Aug 28, 2014
            'Added columns : GST Amount, Input Tax Code, Output Tax Code
            If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                If strIsGst = "Yes" Or compType = "E" Then
                    If Not Request.QueryString("isResident") Is Nothing Then
                        If Request.QueryString("isResident") = True Then
                            If aryDoc(i)(20) IsNot Nothing Then
                                If aryDoc(i)(20) = "R" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Reimbursement
                                    'GST Amount
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")

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

                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                    End If

                                    'strrow &= "<option value=""Select"">---Select---</option>"
                                    'For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                    '    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    'Next
                                ElseIf aryDoc(i)(20) = "D" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Disbursement
                                    'Gst Amount
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
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
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    strrow &= "<option value=""Select"">---Select---</option>"
                                    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Next
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                End If
                            Else
                                If aryDoc(i)(20) = "R" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Reimbursement
                                    'GST Amount
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
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
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
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
                                            ViewState("IM2") = "Yes"
                                            ViewState("Row") = i
                                            dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                'CH - 7 Apr 2015 - Prod issue
                                                GSTAmount = Format(CDec(GSTAmount), "##0.00")
                                            End If
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                            ViewState("Prize") = "Yes"
                                            ViewState("Row") = i
                                            dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE ='" & compInputTaxValue_TX4 & "'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                'CH - 7 Apr 2015 - Prod issue
                                                GSTAmount = Format(CDec(GSTAmount), "##0.00")
                                            End If
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        Else
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")

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

                                    '----
                                    'Output Tax
                                    If aryDoc(i)(23) Is Nothing Then
                                        If Not aryDoc(i)(22) Is Nothing Then
                                            strrow &= "<td>"
                                            'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            If Not aryDoc(i)(22) Is Nothing Then
                                                If aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" _
                                                Or aryDoc(i)(22).ToString = compInputTaxValue_TX4 Or aryDoc(i)(22).ToString = "IM2" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                Else
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                End If
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                            dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            If Not aryDoc(i)(23) Is Nothing Then
                                                If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
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
                                                dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                strrow &= "<option value=""Select"">---Select---</option>"
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                Next
                                            End If
                                        End If
                                    Else
                                        strrow &= "<td>"
                                        'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                        If Not aryDoc(i)(22) Is Nothing Then
                                            If aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            ElseIf aryDoc(i)(22).ToString = "IM2" Or aryDoc(i)(22).ToString = compInputTaxValue_TX4 Then
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                        Else
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                        End If
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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

                                    If Not ds.Tables(0).Rows(0).Item("BM_GST_VALUE") Is DBNull.Value Then
                                        If aryDoc(i)(21).ToString.Trim = "0" Then
                                            'GST Amount
                                            If ds.Tables(0).Rows(0).Item("bm_GST_VALUE").ToString.Substring(0, 1).ToString = "0" Then
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & CDec(ds.Tables(0).Rows(0).Item("BM_GST_VALUE")) & """ readonly ></span>"
                                                strrow &= "</td>"
                                            Else
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & Format(ds.Tables(0).Rows(0).Item("BM_GST_VALUE"), "#.00") & """ readonly ></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            If Not aryDoc(i)(23) Is Nothing Then
                                                If Not aryDoc(i)(23).ToString.Trim = "" Then
                                                    'Get percentage
                                                    Dim percentage = objDB.GetVal("SELECT IFNULL(TAX_PERC,0) AS GST FROM tax_mstr, tax WHERE tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code  AND TM_TAX_CODE = '" & aryDoc(i)(23).ToString.Trim & "' ORDER BY TM_TAX_CODE ")
                                                    'Zulham 02062015 IPP GST Stage 2A
                                                    'Set the percentage to 0 if it is ""
                                                    If percentage.ToString.Trim.Length = 0 Then percentage = 0
                                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                        GSTAmount = (percentage / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                        'CH - 7 Apr 2015 - Prod issue
                                                        GSTAmount = Format(CDec(GSTAmount), "##0.00")
                                                    Else
                                                        GSTAmount = ""
                                                    End If
                                                End If
                                            End If

                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ readonly ></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        'GST Amount
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ readonly ></span>"
                                        strrow &= "</td>"
                                    End If

                                    'Input Tax
                                    'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""" & 0 & """>" & "N/A" & "</option>"
                                    'If aryDoc(i)(22) Is Nothing Then
                                    '    If Not ds.Tables(0).Rows(0).Item("bm_GST_INPUT_TAX_CODE") Is DBNull.Value Then
                                    '        strrow &= "<td>"
                                    '        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    '        For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                    '            If ds.Tables(0).Rows(0).Item("bm_GST_INPUT_TAX_CODE").ToString.Trim = dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim Then
                                    '                strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    '            Else
                                    '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    '            End If
                                    '        Next
                                    '    Else
                                    '        strrow &= "<td>"
                                    '        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    '        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    '    End If
                                    'Else
                                    '    ''From Start ***
                                    '    strrow &= "<td>"
                                    '    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    '    If Not aryDoc(i)(22) Is Nothing Then
                                    '        If aryDoc(i)(22).ToString = "Select" Then
                                    '            strrow &= "<option value=""Select"">---Select---</option>"
                                    '        End If
                                    '        For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                    '            If dsInputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(22).ToString.Trim Then
                                    '                tempStr = dsInputTax.Tables(0).Rows(row).Item(0).ToString.Trim
                                    '                strrow &= "<option value=""" & aryDoc(i)(22).ToString & """ selected=""selected"">" & tempStr & "</option>"
                                    '            Else
                                    '                strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                    '            End If
                                    '        Next
                                    '    Else
                                    '        strrow &= "<option value=""Select"">---Select---</option>"
                                    '        For row As Integer = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                    '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                    '        Next
                                    '    End If
                                    '    ''To End ***
                                    'End If

                                    'Output Tax
                                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                    If Not aryDoc(i)(23) Is Nothing Then
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                        If Not ds.Tables(0).Rows(0).Item("BM_GST_OUTPUT_TAX_CODE") Is DBNull.Value Then
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                            dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = ds.Tables(0).Rows(0).Item("BM_GST_OUTPUT_TAX_CODE") Then
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """ selected=""selected"">" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        Else
                                            strrow &= "<td>"
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                            dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                            strrow &= "<option value=""Select"">---Select---</option>"
                                            For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                            Next
                                        End If
                                    End If
                                    'End Output Tax

                                Else 'PoB selected
                                    'GST Amount
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" disabled=""disabled"" name=""txtGSTAmount" & i & """ value=""N/A"" onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""" & 0 & """>" & "N/A" & "</option>"
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
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                                ViewState("Row") = ""
                                                ViewState("IM2") = ""
                                            Else
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    ElseIf ViewState("Prize") IsNot Nothing Then
                                        If ViewState("Prize") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                            If i = ViewState("Row") Then
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                                ViewState("Row") = ""
                                                ViewState("Prize") = ""
                                            Else
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
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
                                    ViewState("IM2") = "Yes"
                                    ViewState("Row") = i
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                    For Each row As DataRow In strPerc
                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                    Next
                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                        GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                        'CH - 7 Apr 2015 - Prod issue
                                        GSTAmount = Format(CDec(GSTAmount), "##0.00")
                                    End If
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    strrow &= "<option value=""Select"">---Select---</option>"
                                    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Next

                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    'strrow &= "<option value=""" & compOutputTaxValue & """>" & compOutputTaxValue & "</option>"
                                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                    ViewState("Prize") = "Yes"
                                    ViewState("Row") = i
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                    For Each row As DataRow In strPerc
                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                    Next
                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                        GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                        'CH - 7 Apr 2015 - Prod issue
                                        GSTAmount = Format(CDec(GSTAmount), "##0.00")
                                    End If
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    strrow &= "<option value=""Select"">---Select---</option>"
                                    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Next

                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    'strrow &= "<option value=""" & compOutputTaxValue & """>" & compOutputTaxValue & "</option>"
                                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                        If ViewState("IM2") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                            If i = ViewState("Row") Then
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            Else
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    ElseIf ViewState("Prize") IsNot Nothing Then
                                        If ViewState("Prize") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                            If i = ViewState("Row") Then
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            Else
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
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
                                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                'Gst Amount
                                strrow &= "<td align=""right"">"
                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                                strrow &= "</td>"
                                'Input Tax
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """  disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                'Output Tax
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                            ElseIf aryDoc(i)(1) = "Own Co." Then
                                If aryDoc(i)(22) = "IM1" Or aryDoc(i)(22) = "IM3" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                    'GST Amount
                                    If ViewState("IM2") IsNot Nothing Then
                                        If ViewState("IM2") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                            If i = ViewState("Row") Then
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                                ViewState("Row") = ""
                                                ViewState("IM2") = ""
                                            Else
                                                strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
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
                                    ViewState("IM2") = "Yes"
                                    ViewState("Row") = i
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                    For Each row As DataRow In strPerc
                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                    Next
                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                        GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                        'CH - 7 Apr 2015 - Prod issue
                                        GSTAmount = Format(CDec(GSTAmount), "##0.00")
                                    End If
                                    'GST Amount
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
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
                                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                    ViewState("Prize") = "Yes"
                                    ViewState("Row") = i
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                    For Each row As DataRow In strPerc
                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                    Next
                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                        GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                        'CH - 7 Apr 2015 - Prod issue
                                        GSTAmount = Format(CDec(GSTAmount), "##0.00")
                                    End If
                                    'GST Amount
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
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
                                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
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
                                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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

                                'GST Amount
                                If Not ds.Tables(0).Rows(0).Item("BM_GST_VALUE") Is DBNull.Value Then
                                    If aryDoc(i)(21).ToString.Trim = "0" Then
                                        If ds.Tables(0).Rows(0).Item("bm_GST_VALUE").ToString.Substring(0, 1).ToString = "0" Then
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & CDec(ds.Tables(0).Rows(0).Item("BM_GST_VALUE")) & """ readonly ></span>"
                                            strrow &= "</td>"
                                        Else
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & Format(ds.Tables(0).Rows(0).Item("BM_GST_VALUE"), "#.00") & """ readonly ></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        If Not aryDoc(i)(23) Is Nothing Then
                                            If Not aryDoc(i)(23).ToString.Trim = "" Then
                                                'Get percentage
                                                Dim percentage = objDB.GetVal("SELECT IFNULL(TAX_PERC,0) AS GST FROM tax_mstr, tax WHERE tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code  AND TM_TAX_CODE = '" & aryDoc(i)(23).ToString.Trim & "' ORDER BY TM_TAX_CODE ")
                                                If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                    GSTAmount = (percentage / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                    'CH - 7 Apr 2015 - Prod issue
                                                    GSTAmount = Format(CDec(GSTAmount), "##0.00")
                                                Else
                                                    GSTAmount = ""
                                                End If
                                            End If
                                        End If

                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ readonly ></span>"
                                        strrow &= "</td>"
                                    End If
                                Else
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ readonly ></span>"
                                    strrow &= "</td>"
                                End If
                                'End GST Amount

                                'Input Tax
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                strrow &= "<option value=""" & 0 & """>" & "N/A" & "</option>"
                                'End Input Tax


                                'Output Tax
                                dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                If Not aryDoc(i)(23) Is Nothing Then
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                    If Not ds.Tables(0).Rows(0).Item("BM_GST_OUTPUT_TAX_CODE") Is DBNull.Value Then
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                            If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = ds.Tables(0).Rows(0).Item("BM_GST_OUTPUT_TAX_CODE") Then
                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """ selected=""selected"">" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                            Else
                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                            End If
                                        Next
                                    Else
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                        strrow &= "<option value=""Select"">---Select---</option>"
                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                        Next
                                    End If
                                End If

                                'End Output Tax

                            ElseIf aryDoc(i)(1) <> "Own Co." And aryDoc(i)(20) = Nothing Then 'Selected Comp + Disbursement would be selected first
                                'GST Amount
                                strrow &= "<td align=""right"">"
                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """ name=""txtGSTAmount" & i & """ value=""N/A"" disabled=""disabled""></span>"
                                strrow &= "</td>"
                                'Input Tax
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"" disabled=""disabled"">"
                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                'Output Tax
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                            End If
                        End If
                    End If
                Else

                    '===============
                    'Check for document dated before cut off date
                    Dim documentDate = objDB.GetVal("SELECT IFNULL(bm_doc_date,'') 'bm_doc_date' FROM billing_mstr WHERE bm_invoice_no = '" & Common.Parse(Request.QueryString("docno")) & "' and bm_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                    Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")

                    'If CDate(documentDate) < CDate(_cutoffDate) Then

                    '    'Gst Amount
                    '    strrow &= "<td align=""right"">"
                    '    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                    '    strrow &= "</td>"
                    '    'Input TaxS
                    '    strrow &= "<td>"
                    '    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """  disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                    '    strrow &= "<option value=""" & 0 & """>N/A</option>"
                    '    'Output Tax
                    '    strrow &= "<td>"
                    '    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                    '    strrow &= "<option value=""" & 0 & """>N/A</option>"

                    'Else

                    '***************

                    'Predefined tax codes will be default value
                    Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")
                    Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")

                    'GST Amount
                    If Not aryDoc(i)(23) Is Nothing Then
                        If Not aryDoc(i)(23).ToString.Trim = "Select" Then
                            If Not aryDoc(i)(23).ToString.Trim = "" Then
                                'Get percentage
                                Dim percentage = objDB.GetVal("SELECT IFNULL(TAX_PERC,0) AS GST FROM tax_mstr, tax WHERE tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code  AND TM_TAX_CODE = '" & aryDoc(i)(23).ToString.Trim & "' ORDER BY TM_TAX_CODE ")

                                If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                    If Not percentage = "" Then
                                        GSTAmount = (percentage / 100) * CType(IIf(aryDoc.Item(i)(5).ToString.Trim = "", 0, aryDoc.Item(i)(5)), Double) * CType(IIf(aryDoc.Item(i)(6).ToString.Trim = "", 0, aryDoc.Item(i)(6)), Double)
                                        'CH - 7 Apr 2015 - Prod issue
                                        GSTAmount = Format(CDec(GSTAmount), "##0.00")
                                    Else
                                        GSTAmount = ""
                                    End If
                                Else
                                    GSTAmount = ""
                                End If
                            End If
                        Else
                            GSTAmount = ""
                        End If
                        'Zulham 15042015 IPP GST Stage 2A
                        strrow &= "<td align=""right"">"
                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ ></span>"
                        strrow &= "</td>"
                    Else
                        'GSTAmount = ""
                        If Not ds.Tables(0).Rows(0).Item("BM_GST_VALUE") Is DBNull.Value Then
                            If aryDoc(i)(21).ToString.Trim = "0" Then
                                If ds.Tables(0).Rows(0).Item("bm_GST_VALUE").ToString.Substring(0, 1).ToString = "0" Then
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & CDec(ds.Tables(0).Rows(0).Item("BM_GST_VALUE")) & """ ></span>"
                                    strrow &= "</td>"
                                Else
                                    strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & Format(ds.Tables(0).Rows(0).Item("BM_GST_VALUE"), "#.00") & """ ></span>"
                                    strrow &= "</td>"
                                End If
                            Else
                                If Not aryDoc(i)(23) Is Nothing Then
                                    If Not aryDoc(i)(23).ToString.Trim = "" Then
                                        'Get percentage
                                        Dim percentage = objDB.GetVal("SELECT IFNULL(TAX_PERC,0) AS GST FROM tax_mstr, tax WHERE tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code  AND TM_TAX_CODE = '" & aryDoc(i)(23).ToString.Trim & "' ORDER BY TM_TAX_CODE ")
                                        If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                            GSTAmount = (percentage / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                            'CH - 7 Apr 2015 - Prod issue
                                            GSTAmount = Format(CDec(GSTAmount), "##0.00")
                                        Else
                                            GSTAmount = ""
                                        End If
                                    End If
                                End If

                                strrow &= "<td align=""right"">"
                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                strrow &= "</td>"
                            End If
                        Else
                            strrow &= "<td align=""right"">"
                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                            strrow &= "</td>"
                        End If
                    End If

                    'Input Tax
                    'Zulham Case 8713 25/02/2015 
                    'Input Tax
                    strrow &= "<td>"
                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                    strrow &= "<option value=""" & 0 & """>" & "N/A" & "</option>"

                    'Output Tax
                    strrow &= "<td>"
                    If VenOutputTaxValue.ToString.Contains("N/A") Or compType = "E" Then
                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                    Else
                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                    End If
                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                    If VenOutputTaxValue.ToString = "0" And Not compType = "E" Then strrow &= "<option value=""Select"">---Select---</option>"
                    If aryDoc(i)(23) Is Nothing Then
                        'For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                        '    If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                        '        tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                        '        strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                        '    Else
                        '        If record = 0 Then
                        '            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                        '        Else
                        '            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                        '        End If
                        '    End If
                        'Next
                        If Not ds.Tables(0).Rows(0).Item("BM_GST_OUTPUT_TAX_CODE") Is DBNull.Value Then
                            'strrow &= "<td>"
                            'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                            For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = ds.Tables(0).Rows(0).Item("BM_GST_OUTPUT_TAX_CODE") Then
                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """ selected=""selected"">" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                Else
                                    If row = 0 Then
                                        strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                    Else
                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                    End If
                                End If
                            Next
                        Else
                            'strrow &= "<td>"
                            'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                            'dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                            'strrow &= "<option value=""Select"">---Select---</option>"
                            For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                If row = 0 Then
                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                Else
                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                End If
                            Next
                        End If
                    ElseIf aryDoc(i)(23).ToString.Trim = "" Then
                        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                            If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                            Else
                                If record = 0 Then
                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                Else
                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                End If
                            End If
                        Next
                    Else
                        If Not ds.Tables(0).Rows(0).Item("BM_GST_OUTPUT_TAX_CODE") Is DBNull.Value Then
                            'Zulham 15042015 IPP GST Stage 2A
                            'Commented 2 lines below
                            'strrow &= "<td>"
                            'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                            dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                            For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                If aryDoc(i)(23).ToString.Trim = "" Then
                                    If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = ds.Tables(0).Rows(0).Item("BM_GST_OUTPUT_TAX_CODE") Then
                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """ selected=""selected"">" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                    Else
                                        If row = 0 Then
                                            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                        End If
                                    End If
                                Else
                                    If dsOutputTax.Tables(0).Rows(row).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """ selected=""selected"">" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                    Else
                                        If row = 0 Then
                                            strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                        End If
                                    End If
                                End If
                            Next
                        Else
                            strrow &= "<td>"
                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                            dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                            strrow &= "<option value=""Select"">---Select---</option>"
                            For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                If row = 0 Then
                                    strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                Else
                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                End If
                            Next
                        End If
                    End If

                    '***************
                    'End If
                End If
        End If
            'End

            'Jules 2018.04.23 - PAMB Scrum 2 - Added Category.
            'Category
            strrow &= "<td >"

            Dim strCategory As String = ""
            If ds.Tables(0).Rows(0).Item("BM_CATEGORY") IsNot DBNull.Value Then
                strCategory = ds.Tables(0).Rows(0).Item("BM_CATEGORY")
            End If

            If aryDoc(i)(24) Is Nothing Then
                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlCategory" & i & """ value="""">"
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
                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlCategory" & i & """ value="""">"
                If aryDoc(i)(24).ToString = "Life" Then
                    strrow &= "<option value=""Life"" selected=""selected"">" & "Life" & "</option>"
                    strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                    strrow &= "<option value=""Mixed"">" & "Mixed" & "</option>"
                ElseIf aryDoc(i)(24).ToString = "Non-Life" Then
                    strrow &= "<option value=""Life"">" & "Life" & "</option>"
                    strrow &= "<option value=""Non-Life"" selected=""selected"">" & "Non-Life" & "</option>"
                    strrow &= "<option value=""Mixed"">" & "Mixed" & "</option>"
                ElseIf aryDoc(i)(24).ToString = "Mixed" Then
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
            If InStr(ds.Tables(0).Rows(0).Item("bm_B_GL_CODE").ToString, ":") Then
                glcode = ds.Tables(0).Rows(0).Item("bm_B_GL_CODE").ToString.Substring(0, InStr(ds.Tables(0).Rows(0).Item("bm_B_GL_CODE").ToString, ":") - 1)
            Else
                glcode = ds.Tables(0).Rows(0).Item("bm_B_GL_CODE")
            End If

            If Not aryDoc(i)(8) Is Nothing Then
                strrow &= "<td >"
                strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtGLCode" & i & """ name=""txtGLCode" & i & """ value=""" & aryDoc(i)(8) & """>"
                strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnGLCode" & i & """ name=""btnGLCode" & i & """ onclick=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & aryDoc(i)(8).ToString.Split(":")(0).Trim & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                strrow &= "<input type=""hidden"" id=""hidGLCode" & i & """ name=""hidGLCode" & i & """ value=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & aryDoc(i)(8).ToString.Split(":")(0).Trim & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnGLCode onclick
                strrow &= "<input type=""hidden"" id=""hidGLCodeVal" & i & """ value=""" & aryDoc(i)(8).ToString.Split(":")(0).Trim & """ runat=""server"" />"
                strrow &= "</td>"
            Else
                strrow &= "<td >"
                strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtGLCode" & i & """ name=""txtGLCode" & i & """ value=""" & ds.Tables(0).Rows(0).Item("bm_B_GL_CODE") & ":" & ds.Tables(0).Rows(0).Item("CBG_B_GL_DESC") & """ >"
                strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnGLCode" & i & """ name=""btnGLCode" & i & """ onclick=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & ds.Tables(0).Rows(0).Item("bm_B_GL_CODE") & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                strrow &= "<input type=""hidden"" id=""hidGLCode" & i & """ name=""hidGLCode" & i & """ value=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & glcode & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnGLCode onclick
                strrow &= "<input type=""hidden"" id=""hidGLCodeVal" & i & """ value=""" & glcode & """ runat=""server"" />"
                strrow &= "</td>"
            End If
            'End If

            'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR and Sub Description, added Category and Analysis Codes.
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
            'ElseIf ds.Tables(0).Rows(0).Item("bm_GLRULE_CATEGORY") IsNot DBNull.Value Then
            '    strrow &= "<td >"
            '    strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  onkeypress=""return isNumberCharKey(event);""  id=""txtRuleCategory" & i & """ name=""txtRuleCategory" & i & """ value=""" & ds.Tables(0).Rows(0).Item("bm_GLRULE_CATEGORY") & """>"
            '    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnSubDescCode" & i & """ name=""btnSubDescCode" & i & """ onclick=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & aryDoc(i)(8) & "&txtid=" & "txtRuleCategory" & i & "&hidbtnid=btnSubDescCode" & i & "&hidid=hidRuleCategory2" & i & "&hidvalid=hidRuleCategoryVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
            '    strrow &= "<input type=""hidden"" id=""hidRuleCategory2" & i & """ name=""hidRuleCategory2" & i & """ value=""window.open('../../Common/IPP/SubDescriptionSearch.aspx?GLCode=" & glcode & "','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no'); return false;"" />" 'this line must be same with the line btnCostAlloc2 onclick
            '    strrow &= "<input type=""hidden"" id=""hidRuleCategoryVal" & i & """ name=""hidRuleCategoryVal" & i & """ value=""" & ds.Tables(0).Rows(0).Item("bm_GLRULE_CATEGORY_INDEX") & """  runat=""server"" />"
            '    strrow &= "</td>"
            '    ViewState("bm_GLRULE_CATEGORY") = ds.Tables(0).Rows(0).Item("bm_B_GL_CODE")
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

            'Dim brcode As String
            'If InStr(ds.Tables(0).Rows(0).Item("bm_BRANCH_CODE").ToString, ":") Then
            '    brcode = ds.Tables(0).Rows(0).Item("bm_BRANCH_CODE").ToString.Substring(0, InStr(ds.Tables(0).Rows(0).Item("bm_BRANCH_CODE").ToString, ":") - 1)
            'Else
            '    brcode = ds.Tables(0).Rows(0).Item("bm_BRANCH_CODE")
            'End If
            'aryDoc.Add(New String() {Request.Form("txtSNo" & 0), Request.Form("ddlPayFor" & 1), Request.Form("txtInvNo" & 2), Request.Form("txtDesc" & 3), Request.Form("ddlUOM" & 4), Request.Form("txtQty" & 5), Request.Form("txtUnitPrice" & 6), Request.Form("txtAmt" & 7), Request.Form("txtGLCode" & 8), Request.Form("txtAssetGroup" & 9), Request.Form("txtAssetSubGroup" & 10), Request.Form("txtCostAlloc" & 11), Request.Form("txtBranch" & i), Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i)})

            'If aryDoc(i)(9) Is Nothing Then
            '    strrow &= "<td >"
            '    strrow &= "<input style=""width:70px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtBranch" & i & """ name=""txtBranch" & i & """ value=""" & ds.Tables(0).Rows(0).Item("bm_BRANCH_CODE") & ":" & ds.Tables(0).Rows(0).Item("bm_BRANCH_CODE_NAME") & """>"
            '    strrow &= "<input type=""hidden"" id=""hidBranchVal" & i & """ value=""" & brcode & """  runat=""server"" />"
            '    strrow &= "</td>"
            'Else
            '    strrow &= "<td >"
            '    strrow &= "<input style=""width:70px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtBranch" & i & """ name=""txtBranch" & i & """ value=""" & aryDoc(i)(9) & """>"
            '    strrow &= "<input type=""hidden"" id=""hidBranchVal" & i & """ value=""" & aryDoc(i)(9).ToString.Split(":")(0).Trim & """  runat=""server"" />"
            '    strrow &= "</td>"
            'End If  
            'End modification.

            'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR, added Analysis Codes.            
            'Analysis Codes 
            Dim analysisCode_edit As String = ""
            Dim analysisCodeDesc_edit As String = ""

            For j As Integer = 1 To 9
                If j = 6 Or j = 7 Then Continue For

                analysisCode_edit = ""
                analysisCodeDesc_edit = ""

                If ds.Tables(0).Rows(0).Item("BM_ANALYSIS_CODE" & j & "") IsNot DBNull.Value Then
                    If InStr(ds.Tables(0).Rows(0).Item("BM_ANALYSIS_CODE" & j & "").ToString, ":") Then
                        analysisCode_edit = ds.Tables(0).Rows(0).Item("BM_ANALYSIS_CODE" & j & "").ToString.Substring(0, InStr(ds.Tables(0).Rows(0).Item("BM_ANALYSIS_CODE" & j & "").ToString, ":") - 1)
                    Else
                        analysisCode_edit = ds.Tables(0).Rows(0).Item("BM_ANALYSIS_CODE" & j & "")
                    End If
                End If

                strrow &= "<td>"
                If Not aryDoc(i)(24 + j) Is Nothing Then
                    strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtAnalysisCode" & j & "_" & i & """ name=""txtAnalysisCode" & j & "_" & i & """ value=""" & aryDoc(i)(24 + j) & """>"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnAnalysisCode" & j & "_" & i & """ name=""btnAnalysisCode" & j & "_" & i & """ onclick=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & aryDoc(i)(24 + j).ToString.Split(":")(0).Trim & "&txtid=" & "txtAnalysisCode" & j & "_" & i & "&hidbtnid=btnAnalysisCode" & j & "_" & i & "&hidid=hidAnalysisCode" & j & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCode" & j & "_" & i & """ name=""hidAnalysisCode" & j & "_" & i & """ value=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & aryDoc(i)(24 + j).ToString.Split(":")(0).Trim & "&txtid=" & "txtAnalysisCode" & j & "_" & i & "&hidbtnid=btnAnalysisCode" & j & "_" & i & "&hidid=hidAnalysisCode" & j & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnAnalysisCode onclick
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCodeVal" & j & "_" & i & """ value=""" & aryDoc(i)(24 + j).ToString.Split(":")(0).Trim & """ runat=""server"" />"
                ElseIf analysisCode_edit <> "" Then
                    analysisCodeDesc_edit = objDB.GetVal("SELECT IFNULL(AC_ANALYSIS_CODE_DESC,'') 'AC_ANALYSIS_CODE_DESC' FROM analysis_code WHERE AC_ANALYSIS_CODE = '" & analysisCode_edit & "' AND AC_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'")
                    strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtAnalysisCode" & j & "_" & i & """ name=""txtAnalysisCode" & j & "_" & i & """ value=""" & analysisCode_edit & ":" & analysisCodeDesc_edit & """ >"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnAnalysisCode" & j & "_" & i & """ name=""btnAnalysisCode" & j & "_" & i & """ onclick=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & ds.Tables(0).Rows(0).Item("BM_ANALYSIS_CODE" & j & "") & "&txtid=" & "txtAnalysisCode" & j & "_" & i & "&hidbtnid=btnAnalysisCode" & j & "_" & i & "&hidid=hidAnalysisCode" & j & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" readonly=""readonly"">"
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCode" & j & "_" & i & """ name=""hidAnalysisCode" & j & "_" & i & """ value=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & analysisCode_edit & "&txtid=" & "txtAnalysisCode" & j & "_" & i & "&hidbtnid=btnAnalysisCode" & j & "_" & i & "&hidid=hidAnalysisCode" & j & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnAnalysisCode onclick
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCodeVal" & j & "_" & i & """ value=""" & analysisCode_edit & """ runat=""server"" />"
                Else
                    strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtAnalysisCode" & j & "_" & i & """ name=""txtAnalysisCode" & j & "_" & i & """ value=""" & aryDoc(i)(24 + j) & """>"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnAnalysisCode" & j & "_" & i & """ name=""btnAnalysisCode" & j & "_" & i & """ onclick=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & analysisCode_edit & "&txtid=" & "txtAnalysisCode" & j & "_" & i & "&hidbtnid=btnAnalysisCode" & j & "_" & i & "&hidid=hidAnalysisCode" & j & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCode" & j & "_" & i & """ name=""hidAnalysisCode" & j & "_" & i & """ value=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & analysisCode_edit & "&txtid=" & "txtAnalysisCode2_" & i & "&hidbtnid=btnAnalysisCode" & j & "_" & i & "&hidid=hidAnalysisCode" & j & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnAnalysisCode onclick
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCodeVal" & j & "_" & i & """ value=""" & analysisCode_edit & """ runat=""server"" />"
                End If
                strrow &= "</td>"
            Next
            'End modification.

            Dim cccode As String
            If InStr(ds.Tables(0).Rows(0).Item("bm_COST_CENTER").ToString, ":") Then
                cccode = ds.Tables(0).Rows(0).Item("bm_COST_CENTER").ToString.Substring(0, InStr(ds.Tables(0).Rows(0).Item("bm_COST_CENTER").ToString, ":") - 1)
            Else
                cccode = ds.Tables(0).Rows(0).Item("bm_COST_CENTER")
            End If
            If Common.parseNull(aryDoc(i)(11)) = "" Then
                strrow &= "<td >"
                strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text"" id=""txtCC" & i & """ name=""txtCC" & i & """ value=""" & ds.Tables(0).Rows(0).Item("bm_COST_CENTER") & ":" & ds.Tables(0).Rows(0).Item("bm_COST_CENTER_DESC") & """>"
                strrow &= "<input type=""hidden"" id=""hidCCVal" & i & """ value=""" & cccode & """  runat=""server"" />"
                strrow &= "</td>"
            Else
                strrow &= "<td >"
                strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text"" id=""txtCC" & i & """ name=""txtCC" & i & """ value=""" & aryDoc(i)(11) & """>"
                strrow &= "<input type=""hidden"" id=""hidCCVal" & i & """ value=""" & aryDoc(i)(11).ToString.Split(":")(0) & """  runat=""server"" />"
                strrow &= "</td>"
            End If

            'strrow &= "</td>" 'Jules 2018.04.26 - Commented - doesn't seem to have a starting tag.
            'End modification.

            strrow &= "</tr>"

            If Request.QueryString("doctype") <> "Invoice" And Request.QueryString("doctype") <> "Bill" And Request.QueryString("doctype") <> "Letter" Then
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">GST Amount Input :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTInputTotal"" name=""sGSTInputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">(GST Amount Output) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTOutputTotal"" name=""sGSTOutputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">Total :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                Else
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">Total :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                End If

            Else
                'strrow &= "<table id=""table44"" style=""border:0;"">"
                'strrow &= "<tr><td style ="" width:403px;""></td><td class = ""emptycol"" style=""width: 120px; text-align:right; font-weight:bold; "">Total :</td>"
                'strrow &= "<td class = ""emptycol"" style=""width: 130px;""> <hr style=""width:130px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:130px;"" /></td></tr>"
                'strrow &= "</table>"
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">GST Amount Input :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTInputTotal"" name=""sGSTInputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">(GST Amount Output) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTOutputTotal"" name=""sGSTOutputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">Total :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                Else
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">Total :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                End If
            End If
            If Request.QueryString("doctype") <> "Invoice" And Request.QueryString("doctype") <> "Bill" And Request.QueryString("doctype") <> "Letter" Then
                'If UCase(ds.Tables(0).Rows(0).Item("bm_PAY_FOR")) = "HLB" Or UCase(ds.Tables(0).Rows(0).Item("bm_PAY_FOR")) = "HLISB" Then
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then

                    'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR and Sub Description, added Category and Analysis Codes, changed sequence.
                    'Zulham 21/06/2017
                    'IPP Stage 3
                    If Request.QueryString("doctype") <> "Credit Note" And Request.QueryString("doctype") <> "Debit Note" _
                     And Request.QueryString("doctype") <> "Credit Note(Non-Invoice)" And Request.QueryString("doctype") <> "Debit Note(Non-Invoice)" Then
                        table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1000px;"" id=""tblIPPDocItem"">" &
                                                    "<tr class=""TableHeader"">" &
                                                    "<td style=""display:none;"" align=""right"">S/No</td>" &
                                                    "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                                                    "<td style=""width:60px;margin-right:0px;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                                                    "<td style=""width:45px;margin-right:0px;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                                                    "<td style=""width:65px;margin-right:0px;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                                                    "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount</td>" &
                                                    "<td style=""width:110px;margin-right:0px;"" >GST Amount</td>" &
                                                    "<td style=""width:110px;margin-right:0px;"" >Input Tax Code</td>" &
                                                    "<td style=""width:110px;margin-right:0px;"" >Output Tax Code</td>" &
                                                    "<td style=""width:50px; margin-right:0px;"">Category</td>" &
                                                    "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                                                    "<td style=""width:50px; margin-right:0px;"">Fund Type</td>" &
                                                    "<td style=""width:50px; margin-right:0px;"">Product Type</td>" &
                                                    "<td style=""width:50px; margin-right:0px;"">Channel</td>" &
                                                    "<td style=""width:50px; margin-right:0px;"">Reinsurance Company</td>" &
                                                    "<td style=""width:50px; margin-right:0px;"">Asset Fund</td>" &
                                                    "<td style=""width:50px; margin-right:0px;"">Project Code</td>" &
                                                    "<td style=""width:50px; margin-right:0px;"">Person Code</td>" &
                                                    "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                                                    "</tr>" &
                                                    strrow &
                                                    "</table>"
                        '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" &
                        '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                        '"</tr>" &
                        'strrow &
                        '"</table>"
                    Else
                        'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR and Sub Description, added Category and Analysis Codes, changed sequence.
                        table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1000px;"" id=""tblIPPDocItem"">" &
                                                   "<tr class=""TableHeader"">" &
                                                   "<td style=""display:none;"" align=""right"">S/No</td>" &
                                                   "<td style=""width:65px;margin-right:0px;"">Billing No.<span id=""lblInvNo"" class=""errormsg"">*</span></td>" &
                                                   "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                                                   "<td style=""width:60px;margin-right:0px;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                                                   "<td style=""width:45px;margin-right:0px;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                                                   "<td style=""width:65px;margin-right:0px;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                                                   "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount</td>" &
                                                   "<td style=""width:110px;margin-right:0px;"" >GST Amount</td>" &
                                                   "<td style=""width:110px;margin-right:0px;"" >Input Tax Code</td>" &
                                                   "<td style=""width:110px;margin-right:0px;"" >Output Tax Code</td>" &
                                                   "<td style=""width:50px; margin-right:0px;"">Category</td>" &
                                                   "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                                                   "<td style=""width:50px; margin-right:0px;"">Fund Type</td>" &
                                                   "<td style=""width:50px; margin-right:0px;"">Product Type</td>" &
                                                   "<td style=""width:50px; margin-right:0px;"">Channel</td>" &
                                                   "<td style=""width:50px; margin-right:0px;"">Reinsurance Company</td>" &
                                                   "<td style=""width:50px; margin-right:0px;"">Asset Fund</td>" &
                                                   "<td style=""width:50px; margin-right:0px;"">Project Code</td>" &
                                                   "<td style=""width:50px; margin-right:0px;"">Person Code</td>" &
                                                   "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                                                   "</tr>" &
                                                   strrow &
                                                   "</table>"
                        '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" &
                        '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &                        
                        '"</tr>" &
                        'strrow &
                        '"</table>"
                    End If
                Else
                    'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR and Sub Description, added Category and Analysis Codes, changed sequence.
                    table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1000px;"" id=""tblIPPDocItem"">" &
                                       "<tr class=""TableHeader"">" &
                                       "<td style=""display:none;"" align=""right"">S/No</td>" &
                                       "<td style=""width:65px;margin-right:0px;"">Billing No.<span id=""lblInvNo"" class=""errormsg"">*</span></td>" &
                                       "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                                       "<td style=""width:60px;margin-right:0px;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                                       "<td style=""width:30px;margin-right:0px;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                                       "<td style=""width:65px;margin-right:0px;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                                       "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount</td>" &
                                       "<td style=""width:50px; margin-right:0px;"">Category</td>" &
                                       "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                                       "<td style=""width:50px; margin-right:0px;"">Fund Type</td>" &
                                       "<td style=""width:50px; margin-right:0px;"">Product Type</td>" &
                                       "<td style=""width:50px; margin-right:0px;"">Channel</td>" &
                                       "<td style=""width:50px; margin-right:0px;"">Reinsurance Company</td>" &
                                       "<td style=""width:50px; margin-right:0px;"">Asset Fund</td>" &
                                       "<td style=""width:50px; margin-right:0px;"">Project Code</td>" &
                                       "<td style=""width:50px; margin-right:0px;"">Person Code</td>" &
                                       "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                                       "</tr>" &
                                       strrow &
                                       "</table>"
                    '"<td style=""width:50px; margin-right:0px;"">HO/BR<span id=""lblBranch"" class=""errormsg"">*</span></td>" & _
                    '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                    '"</tr>" & _
                    'strrow & _
                    '"</table>"
                End If
                'Else
                '    table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1000px;"" id=""tblIPPDocItem"">" & _
                '               "<tr class=""TableHeader"">" & _
                '               "<td style=""display:none;"" align=""right"">S/No</td>" & _
                '               "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" & _
                '               "<td style=""width:60px;margin-right:0px;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" & _
                '               "<td style=""width:30px;margin-right:0px;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" & _
                '               "<td style=""width:65px;margin-right:0px;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" & _
                '               "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount</td>" & _
                '               "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" & _
                '               "<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" & _
                '               "<td style=""width:50px; margin-right:0px;"">HO/BR</td>" & _
                '               "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                '               "</tr>" & _
                '                strrow & _
                '               "</table>"
                'End If
            Else
                'If UCase(ds.Tables(0).Rows(0).Item("bm_PAY_FOR")) = "HLB" Or UCase(ds.Tables(0).Rows(0).Item("bm_PAY_FOR")) = "HLISB" Then
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                    'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR and Sub Description, added Category and Analysis Codes, changed sequence.
                    table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1000px;"" id=""tblIPPDocItem"">" &
                           "<tr class=""TableHeader"">" &
                           "<td style=""display:none;"" align=""right"">S/No</td>" &
                           "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:60px;margin-right:0px;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:45px;margin-right:0px;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:65px;margin-right:0px;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount</td>" &
                           "<td style=""width:110px;margin-right:0px;"" >GST Amount</td>" &
                           "<td style=""width:110px;margin-right:0px;"" >Input Tax Code</td>" &
                           "<td style=""width:110px;margin-right:0px;"" >Output Tax Code</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Category</td>" &
                           "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:50px; margin-right:0px;"">Fund Type</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Product Type</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Channel</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Reinsurance Company</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Asset Fund</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Project Code</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Person Code</td>" &
                           "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                           "</tr>" &
                           strrow &
                           "</table>"
                    '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" &
                    '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                    '"</tr>" &
                    'strrow &
                    '"</table>"
                Else
                    'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR and Sub Description, added Category and Analysis Codes, changed sequence.
                    table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1000px;"" id=""tblIPPDocItem"">" &
                          "<tr class=""TableHeader"">" &
                          "<td style=""display:none;"" align=""right"">S/No</td>" &
                          "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                          "<td style=""width:60px;margin-right:0px;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                          "<td style=""width:45px;margin-right:0px;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                          "<td style=""width:65px;margin-right:0px;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                          "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount</td>" &
                          "<td style=""width:50px; margin-right:0px;"">Category</td>" &
                          "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:50px; margin-right:0px;"">Fund Type</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Product Type</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Channel</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Reinsurance Company</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Asset Fund</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Project Code</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Person Code</td>" &
                           "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                           "</tr>" &
                           strrow &
                           "</table>"
                    '"<td style=""width:50px; margin-right:0px;"">HO/BR<span id=""lblBranch"" class=""errormsg"">*</span></td>" & _
                    '    "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                    '     "</tr>" & _
                    'strrow & _
                    '"</table>"

                End If
                'Else
                '    If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                '        table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1000px;"" id=""tblIPPDocItem"">" & _
                '               "<tr class=""TableHeader"">" & _
                '               "<td style=""display:none;"" align=""right"">S/No</td>" & _
                '               "<td style=""width:50px;margin-right:0px;"">Pay For</td>" & _
                '               "<td style=""width:50px;margin-right:0px;"">Disb./Reimb.</td>" & _
                '               "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" & _
                '               "<td style=""width:60px;margin-right:0px;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" & _
                '               "<td style=""width:45px;margin-right:0px;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" & _
                '               "<td style=""width:65px;margin-right:0px;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" & _
                '               "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount</td>" & _
                '               "<td style=""width:110px;margin-right:0px;"" >GST Amount</td>" & _
                '               "<td style=""width:110px;margin-right:0px;"" >Input Tax Code</td>" & _
                '               "<td style=""width:110px;margin-right:0px;"" >Output Tax Code</td>" & _
                '               "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" & _
                '               "<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" & _
                '               "<td style=""width:50px; margin-right:0px;"">HO/BR</td>" & _
                '               "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                '               "</tr>" & _
                '               strrow & _
                '               "</table>"
                '    Else
                '        table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1000px;"" id=""tblIPPDocItem"">" & _
                '                  "<tr class=""TableHeader"">" & _
                '                  "<td style=""display:none;"" align=""right"">S/No</td>" & _
                '                  "<td style=""width:50px;margin-right:0px;"">Pay For</td>" & _
                '                  "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" & _
                '                  "<td style=""width:60px;margin-right:0px;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" & _
                '                  "<td style=""width:45px;margin-right:0px;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" & _
                '                  "<td style=""width:65px;margin-right:0px;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" & _
                '                  "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount</td>" & _
                '                  "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" & _
                '                  "<td style=""width:110px;margin-right:0px;"">Sub Description<span id=""lblRuleCategory"" class=""errormsg"">*</span></td>" & _
                '                  "<td style=""width:50px; margin-right:0px;"">HO/BR</td>" & _
                '                  "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                '                   "</tr>" & _
                '                  strrow & _
                '                  "</table>"
                '    End If
                'End If
            End If
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

                'Zulham 19/06./2017
                'IPP Stage 3
                If Request.QueryString("doctype") <> "Invoice" And Request.QueryString("doctype") <> "Non-Invoice" And Request.QueryString("doctype") <> "Debit Advice" _
                And Request.QueryString("doctype") <> "Credit Advice" Then
                    strrow &= "<td >"
                    strrow &= "<input style=""width:85px;margin-right:0px; ""  class=""txtbox2"" type=""text""  id=""txtInvNo" & i & """ name=""txtInvNo" & i & """ value=""" & aryDoc(i)(2) & """>"
                    strrow &= "</td>"
                End If

            strrow &= "<td >"
            strrow &= "<input style=""width:145px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtDesc" & i & """ name=""txtDesc" & i & """ value=""" & aryDoc(i)(3) & """>"
            strrow &= "</td>"

            strrow &= "<td>"
            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlUOM" & i & """ name=""ddlUOM" & i & """>"
            strrow &= "<option value=""Unit"" selected=""selected"">" & "Unit" & "</option>"
            For c = 0 To dsUOM.Tables(0).Rows.Count - 1
                If aryDoc(i)(4) = dsUOM.Tables(0).Rows(c).Item(0).ToString Then
                    strrow &= "<option value=""" & dsUOM.Tables(0).Rows(c).Item(0).ToString & """ selected=""selected"">" & dsUOM.Tables(0).Rows(c).Item(0).ToString & "</option>"
                Else
                    strrow &= "<option value=""" & dsUOM.Tables(0).Rows(c).Item(0).ToString & """>" & dsUOM.Tables(0).Rows(c).Item(0).ToString & "</option>"
                End If
            Next

            strrow &= "<td align=""right"">"
            strrow &= "<span class=""qty""><input style=""width:45px;margin-right:0px;"" class=""numerictxtbox"" onkeypress=""return isDecimalKey(event);"" id=""txtQty" & i & """ onfocus = ""return focusControl('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" name=""txtQty" & i & """ value=""" & aryDoc(i)(5) & """></span>"
            strrow &= "</td>"

            strrow &= "<td align=""right"">"
            strrow &= "<span class=""unit""><input style=""width:65px;margin-right:0px;"" class=""numerictxtbox""  onkeypress=""return isDecimalKey(event);"" id=""txtUnitPrice" & i & """ onfocus = ""return focusControl('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" onblur = ""return calculateTotal('txtQty" & i & "','txtUnitPrice" & i & "','txtAmt" & i & "');"" name=""txtUnitPrice" & i & """ value=""" & aryDoc(i)(6) & """></span>"
            strrow &= "</td>"

            strrow &= "<td align=""right"">"
            strrow &= "<span class=""amount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtAmt" & i & """ name=""txtAmt" & i & """ value=""" & aryDoc(i)(7) & """></span>"
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
                                            ViewState("IM2") = "Yes"
                                            dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                    'CH - 7 Apr 2015 - Prod issue
                                                    GSTAmount = Format(CDec(GSTAmount), "##0.00")
                                                End If
                                            strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        ElseIf aryDoc(i)(22) = compInputTaxValue_TX4 Then
                                            ViewState("Prize") = "Yes"
                                            dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                            Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                            For Each row As DataRow In strPerc
                                                strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                            Next
                                            If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                    GSTAmount = (strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)
                                                    'CH - 7 Apr 2015 - Prod issue
                                                    GSTAmount = Format(CDec(GSTAmount), "##0.00")
                                            End If
                                            strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        Else
                                            strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If

                                    'Predefined tax codes will be default value
                                    Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")
                                    Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")

                                    'Input Tax
                                    If aryDoc(i)(22) Is Nothing Then
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
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
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        dsInputTax = GST.GetTaxCode_forIPP("", "P")
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
                                            dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                End If
                                                'strrow &= "<option value=""Select"">---Select---</option>"
                                                If Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                    'For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    '    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    'Next
                                                    strrow &= "<td>"
                                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                                    If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                            tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                            strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
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
                                                dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                                dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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

                                ElseIf aryDoc(i)(20) = "D" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Disbursement
                                    'Gst Amount
                                    strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                                    strrow &= "</td>"
                                    'Input TaxS
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" disabled=""disabled"" style=""width:110px;margin-right:0px;""  id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                ElseIf aryDoc(i)(1) = "Own Co." Then
                                    'GST Amount
                                    strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    strrow &= "<option value=""Select"">---Select---</option>"
                                    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Next
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                End If
                            Else
                                If aryDoc(i)(20) = "R" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Reimbursement
                                    'GST Amount
                                    strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
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
                                    strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                                    strrow &= "</td>"
                                    'Input TaxS
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """  disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                    strrow &= "<option value=""" & 0 & """>N/A</option>"
                                ElseIf Not aryDoc(i)(22) Is Nothing Then
                                    If aryDoc(i)(22) = "IM2" Then
                                        'GST Amount - Auto Calculated
                                        'Get IM2 rate
                                        ViewState("IM2") = "Yes"
                                        dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                        For Each row As DataRow In strPerc
                                            strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                        Next
                                        If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = Format(CDec((strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)), "##0.00")
                                        End If

                                        strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
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
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                        ViewState("Prize") = "Yes"
                                        dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                        For Each row As DataRow In strPerc
                                            strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                        Next
                                        If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = Format(CDec((strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)), "##0.00")
                                        End If

                                        strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
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
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                        If ViewState("Prize") = "Yes" Or ViewState("IM2") = "Yes" Then
                                            strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        Else
                                            strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If

                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        dsInputTax = GST.GetTaxCode_forIPP("", "P")
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
                                        'Output Tax
                                        strrow &= "<td>"
                                        If Not aryDoc(i)(22) Is Nothing Then
                                            If aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            Else
                                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                            End If
                                        Else
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                        End If
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                        If Not aryDoc(i)(23) Is Nothing Then
                                            If aryDoc(i)(23).ToString = "Select" And Not (aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
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
                                            Else
                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            End If
                                        Else
                                            If Not (Request.QueryString("isResident") = True Or aryDoc(i)(22).ToString = "IM1" Or aryDoc(i)(22).ToString = "IM3" Or aryDoc(i)(22).ToString = "NR" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block) Then
                                                strrow &= "<option value=""Select"">---Select---</option>"
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                Next
                                            Else
                                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                            End If
                                        End If
                                    End If
                                ElseIf aryDoc(i)(1) = Nothing Then 'Initially

                                    'Predefined tax codes will be default value
                                    Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")
                                    Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")

                                        'Zulham 25/10/2017 - IPP Stage 3
                                        'default value
                                        If Request.QueryString("doctype") = "Credit Advice" Or Request.QueryString("doctype") = "Debit Advice" Then
                                            VenOutputTaxValue = "NA2"
                                        End If

                                        'GST Amount
                                        If Not aryDoc(i)(23) Is Nothing Then
                                            If Not aryDoc(i)(23).ToString.Trim = "Select" Then
                                                If Not aryDoc(i)(23).ToString.Trim = "" Then
                                                    'Get percentage
                                                    Dim percentage = objDB.GetVal("SELECT IFNULL(TAX_PERC,0) AS GST FROM tax_mstr, tax WHERE tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code  AND TM_TAX_CODE = '" & aryDoc(i)(23).ToString.Trim & "' ORDER BY TM_TAX_CODE ")
                                                    'Zulham 02062015 IPP SGT Stage 2A
                                                    'Set percentage to 0 if it's ""
                                                    If percentage.ToString.Trim.Length = 0 Then percentage = 0
                                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                        GSTAmount = Format(CDec((percentage / 100) * CType(IIf(aryDoc.Item(i)(5).ToString.Trim = "", 0, aryDoc.Item(i)(5)), Double) * CType(IIf(aryDoc.Item(i)(6).ToString.Trim = "", 0, aryDoc.Item(i)(6)), Double)), "##0.00")
                                                    Else
                                                        GSTAmount = ""
                                                    End If
                                                End If
                                            Else
                                                GSTAmount = ""
                                            End If
                                        Else
                                            GSTAmount = ""
                                        End If

                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; "" readonly class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ ></span>"
                                        strrow &= "</td>"

                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        strrow &= "<option value=""" & 0 & """>" & "N/A" & "</option>"

                                        'Output Tax
                                        strrow &= "<td>"
                                        If VenOutputTaxValue.ToString.Contains("N/A") Or compType = "E" Then
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        Else
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        End If
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                        If VenOutputTaxValue.ToString = "0" And Not compType = "E" Then strrow &= "<option value=""Select"">---Select---</option>"
                                        If aryDoc(i)(23) Is Nothing Then
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                    tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        ElseIf aryDoc(i)(23).ToString.Trim = "" Then
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                                    tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        Else
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                    tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & aryDoc(i)(23).ToString.Trim & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        End If

                                    Else 'PoB selected
                                        'GST Amount
                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" disabled=""disabled"" name=""txtGSTAmount" & i & """ value=""N/A"" onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        strrow &= "<option value=""" & 0 & """>" & "N/A" & "</option>"
                                        'Output Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                        strrow &= "<option value=""" & 0 & """>N/A</option>"
                                    End If

                            End If
                        Else 'Non-Resident
                            If aryDoc(i)(20) = "R" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Reimbursement
                                If Not aryDoc(i)(22) Is Nothing Then
                                    If aryDoc(i)(22) = "IM1" Or aryDoc(i)(22) = "IM3" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                        'GST Amount
                                        If ViewState("IM2") IsNot Nothing Then
                                            If ViewState("IM2") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                                If i = ViewState("Row") Then
                                                    strrow &= "<td align=""right"">"
                                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                    ViewState("Row") = ""
                                                    ViewState("IM2") = ""
                                                Else
                                                    strrow &= "<td align=""right"">"
                                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                End If
                                            Else
                                                strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        ElseIf ViewState("Prize") IsNot Nothing Then
                                            If ViewState("Prize") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                                If i = ViewState("Row") Then
                                                    strrow &= "<td align=""right"">"
                                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
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
                                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If

                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        dsInputTax = GST.GetTaxCode_forIPP("", "P")
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
                                        ViewState("IM2") = "Yes"
                                        dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                        For Each row As DataRow In strPerc
                                            strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                        Next
                                        If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = Format(CDec((strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)), "##0.00")
                                        End If

                                        strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
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
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                        ViewState("Prize") = "Yes"
                                        dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                        For Each row As DataRow In strPerc
                                            strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                        Next
                                        If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                GSTAmount = Format(CDec((strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)), "##0.00")
                                        End If

                                        strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
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
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                            If ViewState("IM2") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                                If i = ViewState("Row") Then
                                                    strrow &= "<td align=""right"">"
                                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                Else
                                                    strrow &= "<td align=""right"">"
                                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                End If
                                            Else
                                                strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        ElseIf ViewState("Prize") IsNot Nothing Then
                                            If ViewState("Prize") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                                If i = ViewState("Row") Then
                                                    strrow &= "<td align=""right"">"
                                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                Else
                                                    strrow &= "<td align=""right"">"
                                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                    strrow &= "</td>"
                                                End If
                                            Else
                                                strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        If aryDoc(i)(22) IsNot Nothing Then
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

                                        'Output Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                                        strrow &= "<option value=""Select"">---Select---</option>"
                                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        Next
                                                    End If
                                                Else
                                                    strrow &= "<option value=""Select"">---Select---</option>"
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
                                                        strrow &= "<option value=""Select"">---Select---</option>"
                                                        For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                        Next
                                                    End If
                                                Else
                                                    strrow &= "<option value=""Select"">---Select---</option>"
                                                    For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                    Next
                                                End If
                                            Else
                                                strrow &= "<option value=""Select"">---Select---</option>"
                                                For row As Integer = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(row).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(row).Item(0).ToString & "</option>"
                                                Next
                                            End If
                                        End If
                                    End If
                                Else

                                    'Predefined tax codes will be default value
                                    Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")
                                    Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")

                                    'GST Amount
                                    strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(7) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    If VenInputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenInputTaxValue.Trim Then
                                            tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                            strrow &= "<option value=""" & VenInputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        End If
                                    Next

                                    'Output Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ >"
                                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                    If VenOutputTaxValue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                    For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                        If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                            tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                            strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        End If
                                    Next

                                End If
                            ElseIf aryDoc(i)(20) = "D" And aryDoc(i)(20) <> "" And Not aryDoc(i)(1) = "Own Co." Then 'P.o.B + Disbursement
                                'Gst Amount
                                strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                                strrow &= "</td>"
                                'Input Tax
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                'Output Tax
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                            ElseIf aryDoc(i)(1) = "Own Co." Then
                                If aryDoc(i)(22) = "IM1" Or aryDoc(i)(22) = "IM3" Or aryDoc(i)(22).ToString = "TX5" Or aryDoc(i)(22).ToString = compInputTaxValue_Block Then
                                    'GST Amount
                                    If ViewState("IM2") IsNot Nothing Then
                                        If ViewState("IM2") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                            If i = ViewState("Row") Then
                                                strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                                ViewState("Row") = ""
                                                ViewState("IM2") = ""
                                            Else
                                                strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    ElseIf ViewState("Prize") IsNot Nothing Then
                                        If ViewState("Prize") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                            If i = ViewState("Row") Then
                                                strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                                ViewState("Row") = ""
                                                ViewState("Prize") = ""
                                            Else
                                                strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
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
                                    ViewState("IM2") = "Yes"
                                    ViewState("Row") = i
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                    For Each row As DataRow In strPerc
                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                    Next
                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                            GSTAmount = Format(CDec((strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)), "##0.00")
                                    End If

                                    'GST Amount
                                    strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"
                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
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
                                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                    ViewState("Prize") = "Yes"
                                    ViewState("Row") = i
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                    For Each row As DataRow In strPerc
                                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                    Next
                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                            GSTAmount = Format(CDec((strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)), "##0.00")
                                    End If

                                    'GST Amount
                                    strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                    strrow &= "</td>"

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        If dsInputTax.Tables(0).Rows(record).Item(1).ToString = aryDoc(i)(22) Then
                                            strrow &= "<option selected=""selected"" value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        Else
                                            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        End If
                                    Next

                                    'Output Tax
                                    Try
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                        For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                            If dsOutputTax.Tables(0).Rows(record).Item(1).ToString = compOutputTaxValue_TX4 Then
                                                strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            Else
                                                strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                            End If
                                        Next
                                    Catch ex As Exception
                                        Throw New Exception(ex.ToString)
                                    End Try
                                Else
                                    If ViewState("IM2") IsNot Nothing Then
                                        If ViewState("IM2") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                            If i = ViewState("Row") Then
                                                strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            Else
                                                strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    ElseIf ViewState("Prize") IsNot Nothing Then
                                        If ViewState("Prize") = "Yes" And Not CType(ViewState("Row"), String) = "" Then
                                            If i = ViewState("Row") Then
                                                strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            Else
                                                strrow &= "<td align=""right"">"
                                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                                strrow &= "</td>"
                                            End If
                                        Else
                                            strrow &= "<td align=""right"">"
                                                strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        End If
                                    Else
                                        strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                    End If

                                    'Input Tax
                                    strrow &= "<td>"
                                    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                    dsInputTax = GST.GetTaxCode_forIPP("", "P")
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
                                    dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                            ElseIf aryDoc(i)(1) = Nothing Then 'Initially
                                    '    'Predefined tax codes will be default value
                                    Dim veninputtaxvalue = objDB.GetVal("select if(ic_gst_input_tax_code is null, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' from ipp_company where ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")
                                    Dim venoutputtaxvalue = objDB.GetVal("select if(ic_gst_output_tax_code is null, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' from ipp_company where ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")

                                    'Zulham 25/10/2017 - IPP Stage 3
                                    'default value
                                    If Request.QueryString("doctype") = "Credit Advice" Or Request.QueryString("doctype") = "Debit Advice" Then
                                        venoutputtaxvalue = "NA2"
                                    End If

                                    'Zulham 15/02/2015
                                    If aryDoc(i)(22) = "IM2" Then
                                        'GST Amount - Auto Calculated
                                        'Get IM2 rate
                                        ViewState("IM2") = "Yes"
                                        dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                                        For Each row As DataRow In strPerc
                                            strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                        Next
                                        If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                            GSTAmount = Format(CDec((strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)), "##0.00")
                                        End If

                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ readonly ></span>"
                                        strrow &= "</td>"
                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
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
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                        ViewState("Prize") = "Yes"
                                        dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = '" & compInputTaxValue_TX4 & "'")
                                        For Each row As DataRow In strPerc
                                            strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                                        Next
                                        If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                            GSTAmount = Format(CDec((strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)), "###0.00")
                                        End If

                                        strrow &= "<td align=""right"">"
                                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                        strrow &= "</td>"
                                        'Input Tax
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
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
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
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
                                        If ViewState("Prize") = "Yes" Or ViewState("IM2") = "Yes" Then
                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & "" & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            strrow &= "</td>"
                                        Else
                                            'strrow &= "<td align=""right"">"
                                            'strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & aryDoc(i)(21) & """ onblur = ""return calculateTotalWithGST('ddlInputTax" & i & "','ddlOutputTax" & i & "','txtGSTAmount" & i & "');""></span>"
                                            'strrow &= "</td>"
                                            If Not aryDoc(i)(23) Is Nothing Then
                                                If Not aryDoc(i)(23).ToString.Trim = "Select" Then
                                                    If Not aryDoc(i)(23).ToString.Trim = "" Then
                                                        'Get percentage
                                                        Dim percentage = objDB.GetVal("SELECT IFNULL(TAX_PERC,0) AS GST FROM tax_mstr, tax WHERE tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code  AND TM_TAX_CODE = '" & aryDoc(i)(23).ToString.Trim & "' ORDER BY TM_TAX_CODE ")

                                                        If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                                            GSTAmount = Format(CDec((percentage / 100) * CType(IIf(aryDoc.Item(i)(5).ToString.Trim = "", 0, aryDoc.Item(i)(5)), Double) * CType(IIf(aryDoc.Item(i)(6).ToString.Trim = "", 0, aryDoc.Item(i)(6)), Double)), "##0.00")
                                                        Else
                                                            GSTAmount = ""
                                                        End If
                                                    End If
                                                Else
                                                    GSTAmount = ""
                                                End If
                                            Else
                                                GSTAmount = ""
                                            End If

                                            strrow &= "<td align=""right"">"
                                            strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ readonly ></span>"
                                            strrow &= "</td>"
                                        End If

                                        'Input Tax
                                        'strrow &= "<td>"
                                        'strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        'dsInputTax = GST.GetTaxCode_forIPP("", "P")
                                        'If Not aryDoc(i)(22) Is Nothing Then
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
                                        '    If veninputtaxvalue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                        '    For record = 0 To dsInputTax.Tables(0).Rows.Count - 1
                                        '        If dsInputTax.Tables(0).Rows(record).Item(1).ToString.Trim = veninputtaxvalue.Trim Then
                                        '            tempStr = dsInputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                        '            strrow &= "<option value=""" & veninputtaxvalue & """ selected=""selected"">" & tempStr & "</option>"
                                        '        Else
                                        '            strrow &= "<option value=""" & dsInputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsInputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                        '        End If
                                        '    Next
                                        'End If
                                        strrow &= "<td>"
                                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"" disabled=""disabled"">"
                                        strrow &= "<option value=""" & 0 & """>N/A</option>"

                                        'Output Tax
                                        strrow &= "<td>"
                                        If venoutputtaxvalue.ToString.Contains("N/A") Then
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        Else
                                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                                        End If
                                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                                        If venoutputtaxvalue.ToString = "0" Then strrow &= "<option value=""Select"">---Select---</option>"
                                        If aryDoc(i)(23) Is Nothing Then
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = venoutputtaxvalue Then
                                                    tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & venoutputtaxvalue & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        ElseIf aryDoc(i)(23).ToString.Trim = "" Then
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = venoutputtaxvalue Then
                                                    tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & venoutputtaxvalue & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        Else
                                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                                    tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                                    strrow &= "<option value=""" & aryDoc(i)(23).ToString.Trim & """ selected=""selected"">" & tempStr & "</option>"
                                                Else
                                                    strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                                End If
                                            Next
                                        End If
                                    End If
                                    'End

                            ElseIf aryDoc(i)(1) <> "Own Co." And aryDoc(i)(20) = Nothing Then 'Selected Comp + Disbursement would be selected first
                                'GST Amount
                                strrow &= "<td align=""right"">"
                                    strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """ name=""txtGSTAmount" & i & """ value=""N/A"" disabled=""disabled""></span>"
                                strrow &= "</td>"
                                'Input Tax
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"" disabled=""disabled"">"
                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                                'Output Tax
                                strrow &= "<td>"
                                strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                                strrow &= "<option value=""" & 0 & """>N/A</option>"
                            End If
                        End If
                    End If
                Else

                    'Check for document dated before cut off date
                    Dim documentDate = objDB.GetVal("SELECT IFNULL(bm_doc_date,'') 'bm_doc_date' FROM billing_mstr WHERE bm_invoice_no = '" & Common.Parse(Request.QueryString("docno")) & "' and bm_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                    Dim _cutoffDate = System.Configuration.ConfigurationManager.AppSettings.Get("GstCutOffDate")

                        'If CDate(documentDate) < CDate(_cutoffDate) Then

                        '    'Gst Amount
                        '    strrow &= "<td align=""right"">"
                        '    strrow &= "<span class=""GSTamount""><input style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  name=""txtGSTAmount" & i & """ value=""" & "N/A" & """ disabled=""disabled""></span>"
                        '    strrow &= "</td>"
                        '    'Input TaxS
                        '    strrow &= "<td>"
                        '    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;""  id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """  disabled=""disabled"" onchange =""onClick('" & i & "');"">"
                        '    strrow &= "<option value=""" & 0 & """>N/A</option>"
                        '    'Output Tax
                        '    strrow &= "<td>"
                        '    strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ disabled=""disabled"">"
                        '    strrow &= "<option value=""" & 0 & """>N/A</option>"

                        'Else
                        '*******
                        'Predefined tax codes will be default value
                        Dim VenInputTaxValue = objDB.GetVal("SELECT IF(ic_gst_input_tax_code IS NULL, 0, ic_gst_input_tax_code) 'ic_gst_input_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")
                        Dim VenOutputTaxValue = objDB.GetVal("SELECT IF(ic_gst_output_tax_code IS NULL, 0, ic_gst_output_tax_code) 'ic_gst_output_tax_code' FROM ipp_company WHERE ic_coy_name = '" & Server.UrlDecode(Request.QueryString("vencomp")) & "'")

                        'GST Amount
                        If Not aryDoc(i)(23) Is Nothing Then
                            If Not aryDoc(i)(23).ToString.Trim = "Select" Then
                                If Not aryDoc(i)(23).ToString.Trim = "" Then
                                    'Get percentage
                                    Dim percentage = objDB.GetVal("SELECT IFNULL(TAX_PERC,0) AS GST FROM tax_mstr, tax WHERE tm_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code  AND TM_TAX_CODE = '" & aryDoc(i)(23).ToString.Trim & "' ORDER BY TM_TAX_CODE ")

                                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                                        'Zulham 15042015 IPP GST Stage 2A
                                        If Not percentage = "" Then
                                            GSTAmount = Format(CDec((percentage / 100) * CType(IIf(aryDoc.Item(i)(5).ToString.Trim = "", 0, aryDoc.Item(i)(5)), Double) * CType(IIf(aryDoc.Item(i)(6).ToString.Trim = "", 0, aryDoc.Item(i)(6)), Double)), "###0.00")
                                        Else
                                            GSTAmount = ""
                                        End If
                                    Else
                                        GSTAmount = ""
                                    End If
                                End If
                            Else
                                GSTAmount = ""
                            End If
                        Else
                            GSTAmount = ""
                        End If

                        strrow &= "<td align=""right"">"
                        strrow &= "<span class=""GSTamount""><input readonly style=""width:85px;margin-right:0px; ""  class=""numerictxtbox"" id=""txtGSTAmount" & i & """  onkeypress=""return isDecimalKey(event);"" name=""txtGSTAmount" & i & """ value=""" & GSTAmount & """ ></span>"
                        strrow &= "</td>"

                        'Input Tax
                        'Zulham Case 8713 25/02/2015 
                        'Input Tax
                        strrow &= "<td>"
                        strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" disabled=""disabled"" id=""ddlInputTax" & i & """ name=""ddlInputTax" & i & """ onchange =""onClick('" & i & "');"">"
                        strrow &= "<option value=""" & 0 & """>" & "N/A" & "</option>"

                        'Output Tax

                        'Zulham 25/10/2017 - IPP Stage 3
                        'default value
                        If Request.QueryString("doctype") = "Credit Advice" Or Request.QueryString("doctype") = "Debit Advice" Then
                            VenOutputTaxValue = "NA2"
                        End If

                        strrow &= "<td>"
                        If VenOutputTaxValue.ToString.Contains("N/A") Or compType = "E" Then
                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                        Else
                            strrow &= "<select class=""ddl2"" style=""width:110px;margin-right:0px;"" id=""ddlOutputTax" & i & """ name=""ddlOutputTax" & i & """ onchange =""onClick('" & i & "');"">"
                        End If
                        dsOutputTax = GST.GetTaxCode_forIPP("", "S")
                        If VenOutputTaxValue.ToString = "0" And Not compType = "E" Then strrow &= "<option value=""Select"">---Select---</option>"
                        If aryDoc(i)(23) Is Nothing Then
                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                    tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                    strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                Else
                                    If record = 0 Then
                                        strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Else
                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    End If
                                End If
                            Next
                        ElseIf aryDoc(i)(23).ToString.Trim = "" Then
                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = VenOutputTaxValue Then
                                    tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                    strrow &= "<option value=""" & VenOutputTaxValue & """ selected=""selected"">" & tempStr & "</option>"
                                Else
                                    If record = 0 Then
                                        strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Else
                                        strrow &= "<option value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    End If
                                End If
                            Next
                        Else
                            For record = 0 To dsOutputTax.Tables(0).Rows.Count - 1
                                If dsOutputTax.Tables(0).Rows(record).Item(1).ToString.Trim = aryDoc(i)(23).ToString.Trim Then
                                    tempStr = dsOutputTax.Tables(0).Rows(record).Item(0).ToString.Trim
                                    strrow &= "<option value=""" & aryDoc(i)(23).ToString.Trim & """ selected=""selected"">" & tempStr & "</option>"
                                Else
                                    If record = 0 Then
                                        strrow &= "<option selected=""selected"" value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    Else
                                        strrow &= "<option  value=""" & dsOutputTax.Tables(0).Rows(record).Item(1).ToString & """>" & dsOutputTax.Tables(0).Rows(record).Item(0).ToString & "</option>"
                                    End If
                                End If
                            Next
                        End If
                        '*******
                    End If
                End If
                'End If
                'End

                'Jules 2018.04.23 - PAMB Scrum 2 - Added Category.
                'strrow &= "</td>"
                strrow &= "<td >"
                strrow &= "<select class=""ddl2"" style=""width:75px;margin-right:0px;"" name=""ddlCategory" & i & """ value="""">"
                strrow &= "<option value=""Life"" selected=""selected"">" & "Life" & "</option>"
                strrow &= "<option value=""Non-Life"">" & "Non-Life" & "</option>"
                strrow &= "<option value=""Mixed"">" & "Mixed" & "</option>"
                strrow &= "</td>"
                'End modification.

                Dim glcode As String = ""
                If aryDoc(i)(8) <> "" Or aryDoc(i)(8) <> Nothing Then
                    If InStr(aryDoc(i)(8).ToString, ":") Then
                        glcode = aryDoc(i)(8).ToString.ToString.Substring(0, InStr(aryDoc(i)(8).ToString, ":") - 1)
                    Else
                        glcode = aryDoc(i)(8)
                    End If
                End If

                If aryDoc(i)(1) = "Own Co." Or aryDoc(i)(1) Is Nothing Or aryDoc(i)(1) = "" Or UCase(aryDoc(i)(1)) = "HLISB" Then

                    strrow &= "<td >"
                    strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtGLCode" & i & """ name=""txtGLCode" & i & """ value=""" & IIf(aryDoc(i)(8) Is Nothing, "", aryDoc(i)(8)) & """>"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnGLCode" & i & """ name=""btnGLCode" & i & """ onclick=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & glcode & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
                    strrow &= "<input type=""hidden"" id=""hidGLCode" & i & """ name=""hidGLCode" & i & """ value=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & glcode & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnGLCode onclick
                    strrow &= "<input type=""hidden"" id=""hidGLCodeVal" & i & """ value=""" & glcode & """ runat=""server"" />"
                    strrow &= "</td>"
                Else
                    aryDoc(i)(8) = objDB.GetVal("SELECT CONCAT(ic_con_ibs_code,':',cbg_b_gl_desc) FROM ipp_company INNER JOIN company_b_gl_code ON cbg_b_gl_code = ic_con_ibs_code AND cbg_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' WHERE ic_other_b_coy_code = '" & aryDoc(i)(1) & "' AND ic_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'")

                    strrow &= "<td >"
                    strrow &= "<input style=""width:165px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtGLCode" & i & """ name=""txtGLCode" & i & """ value=""" & aryDoc(i)(8) & """ disabled=""disabled"">"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnGLCode" & i & """ name=""btnGLCode" & i & """ onclick=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & glcode & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"" disabled=""disabled"">"
                    strrow &= "<input type=""hidden"" id=""hidGLCode" & i & """ name=""hidGLCode" & i & """ value=""window.open('../../Common/IPP/GLCodeSearch.aspx?id=" & glcode & "&txtid=" & "txtGLCode" & i & "&hidbtnid=btnGLCode" & i & "&hidid=hidGLCode" & i & "&hidvalid=hidGLCodeVal" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnGLCode onclick
                    strrow &= "<input type=""hidden"" id=""hidGLCodeVal" & i & """ value=""" & glcode & """ runat=""server"" />"
                    strrow &= "</td>"

                End If

                'Jules 2018.04.23 - PAMB Scrum 2 - Removed Sub Description and HO/BR.
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

                'Dim brcode As String
                'If aryDoc(i)(12) <> "" Or aryDoc(i)(12) <> Nothing Then
                '    If InStr(aryDoc(i)(8).ToString, ":") Then
                '        brcode = aryDoc(i)(12).ToString.ToString.Substring(0, InStr(aryDoc(i)(12).ToString, ":") - 1)
                '    Else
                '        brcode = aryDoc(i)(12)
                '    End If
                'End If

                'strrow &= "<td >"
                'strrow &= "<input style=""width:70px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtBranch" & i & """ name=""txtBranch" & i & """ value=""" & aryDoc(i)(12) & """>"
                'strrow &= "<input type=""hidden"" id=""hidBranchVal" & i & """ value=""" & brcode & """  runat=""server"" />"
                'strrow &= "</td>"
                'End modification.               

                'Jules 2018.04.23 - PAMB Scrum 2 - Added Analysis Code.
                'strrow &= "</td>"                

                Dim analysisCode As String = ""

                For j As Integer = 0 To 8

                    If j = 5 Or j = 6 Then Continue For

                    If aryDoc(i)(25 + j) <> "" Or aryDoc(i)(25 + j) <> Nothing Then
                        If InStr(aryDoc(i)(8).ToString, ":") Then
                            analysisCode = aryDoc(i)(25 + j).ToString.ToString.Substring(0, InStr(aryDoc(i)(25 + j).ToString, ":") - 1)
                        Else
                            analysisCode = aryDoc(i)(25 + j)
                        End If
                    End If

                    strrow &= "<td>"
                    strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text""  id=""txtAnalysisCode" & j + 1 & "_" & i & """ name=""txtAnalysisCode" & j + 1 & "_" & i & """ value=""" & aryDoc(i)(25 + j) & """>"
                    strrow &= "<input style=""width:15px;margin-right:0px; "" class=""button"" type=""button""  id=""btnAnalysisCode" & j + 1 & "_" & i & """ name=""btnAnalysisCode" & j + 1 & "_" & i & """ onclick=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & analysisCode & "&txtid=" & "txtAnalysisCode" & j + 1 & "_" & i & "&hidbtnid=btnAnalysisCode" & j + 1 & "_" & i & "&hidid=hidAnalysisCode" & j + 1 & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j + 1 & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" value="">"">"
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCode" & j + 1 & "_" & i & """ name=""hidAnalysisCode" & j + 1 & "_" & i & """ value=""window.open('../../Common/IPP/AnalysisCodeSearch.aspx?id=" & analysisCode & "&txtid=" & "txtAnalysisCode" & j + 1 & "_" & i & "&hidbtnid=btnAnalysisCode" & j + 1 & "_" & i & "&hidid=hidAnalysisCode" & j + 1 & "_" & i & "&hidvalid=hidAnalysisCodeVal" & j + 1 & "_" & i & "&pageid=','', 'scrollbars=yes,resizable=yes,width=600,height=400,status=no,menubar=no');"" />" 'this line must be same with the line btnAnalysisCode onclick
                    strrow &= "<input type=""hidden"" id=""hidAnalysisCodeVal" & j + 1 & "_" & i & """ value=""" & analysisCode & """ runat=""server"" />"
                    strrow &= "</td>"
                    analysisCode = ""
                Next
                'End modification.

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

                strrow &= "<td >"
                strrow &= "<input style=""width:85px;margin-right:0px; "" class=""txtbox2"" type=""text"" onkeypress=""getLineNo('" & i & "');"" id=""txtCC" & i & """ name=""txtCC" & i & """ value=""" & aryDoc(i)(13) & """>"
                strrow &= "<input type=""hidden"" id=""hidCCVal" & i & """ value=""" & cccode & """  runat=""server"" />"
                strrow &= "</td>"
                strrow &= "</tr>"

            Next
            If Request.QueryString("doctype") <> "Invoice" And Request.QueryString("doctype") <> "Bill" And Request.QueryString("doctype") <> "Letter" Then
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">GST Amount Input :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTInputTotal"" name=""sGSTInputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">(GST Amount Output) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTOutputTotal"" name=""sGSTOutputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">Total :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                Else
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">Total :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                End If
            Else
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">GST Amount Input :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTInputTotal"" name=""sGSTInputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">(GST Amount Output) :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sGSTOutputTotal"" name=""sGSTOutputTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"

                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">Total :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                Else
                    strrow &= "<table id=""table44"" style=""border:0;"">"
                    strrow &= "<tr><td style ="" width:385px;""></td><td class = ""emptycol"" style=""width: 80px; text-align:right; font-weight:bold; "">Total :</td>"
                    strrow &= "<td class = ""emptycol"" style=""width: 100px;""> <hr style=""width:100px;"" /><div id=""sTotal"" name=""sTotal"" style=""text-align:right;margin-right: 10px;"" runat = ""server""></div><hr style=""width:100px;"" /></td></tr>"
                    strrow &= "</table>"
                End If
            End If

            If Request.QueryString("doctype") <> "Invoice" And Request.QueryString("doctype") <> "Bill" And Request.QueryString("doctype") <> "Letter" Then
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                    If Request.QueryString("doctype") <> "Debit Note" And Request.QueryString("doctype") <> "Credit Note" _
                     And Request.QueryString("doctype") <> "Credit Note(Non-Invoice)" And Request.QueryString("doctype") <> "Debit Note(Non-Invoice)" Then

                        'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR and Sub Description, added Category and Analysis Codes, changed sequence.
                        table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1000px;"" id=""tblIPPDocItem"">" &
                                "<tr class=""TableHeader"">" &
                                "<td style=""width:50px;margin-right:0px;"" align=""right"">S/No</td>" &
                                "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                                "<td style=""width:60px;margin-right:0px;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                                "<td style=""width:45px;margin-right:0px;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                                "<td style=""width:65px;margin-right:0px;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                                "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount</td>" &
                                "<td style=""width:110px;margin-right:0px;"" >GST Amount</td>" &
                                "<td style=""width:110px;margin-right:0px;"" >Input Tax Code</td>" &
                                "<td style=""width:110px;margin-right:0px;"" >Output Tax Code</td>" &
                                "<td style=""width:50px; margin-right:0px;"">Category</td>" &
                                "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                                "<td style=""width:50px; margin-right:0px;"">Fund Type</td>" &
                                "<td style=""width:50px; margin-right:0px;"">Product Type</td>" &
                                "<td style=""width:50px; margin-right:0px;"">Channel</td>" &
                                "<td style=""width:50px; margin-right:0px;"">Reinsurance Company</td>" &
                                "<td style=""width:50px; margin-right:0px;"">Asset Fund</td>" &
                                "<td style=""width:50px; margin-right:0px;"">Project Code</td>" &
                                "<td style=""width:50px; margin-right:0px;"">Person Code</td>" &
                                "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                                "</tr>" &
                                strrow &
                                "</table>"
                        '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" &
                        '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                        '"</tr>" &
                        'strrow &
                        '"</table>"
                    Else
                        'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR and Sub Description, added Category and Analysis Codes, changed sequence.
                        table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1000px;"" id=""tblIPPDocItem"">" &
                            "<tr class=""TableHeader"">" &
                            "<td style=""width:50px;margin-right:0px;"" align=""right"">S/No</td>" &
                            "<td style=""width:65px;margin-right:0px;"">Billing No.<span id=""lblInvNo"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:60px;margin-right:0px;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:45px;margin-right:0px;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:65px;margin-right:0px;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount</td>" &
                            "<td style=""width:110px;margin-right:0px;"" >GST Amount</td>" &
                            "<td style=""width:110px;margin-right:0px;"" >Input Tax Code</td>" &
                            "<td style=""width:110px;margin-right:0px;"" >Output Tax Code</td>" &
                            "<td style=""width:50px; margin-right:0px;"">Category</td>" &
                            "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                            "<td style=""width:50px; margin-right:0px;"">Fund Type</td>" &
                            "<td style=""width:50px; margin-right:0px;"">Product Type</td>" &
                            "<td style=""width:50px; margin-right:0px;"">Channel</td>" &
                            "<td style=""width:50px; margin-right:0px;"">Reinsurance Company</td>" &
                            "<td style=""width:50px; margin-right:0px;"">Asset Fund</td>" &
                            "<td style=""width:50px; margin-right:0px;"">Project Code</td>" &
                            "<td style=""width:50px; margin-right:0px;"">Person Code</td>" &
                            "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                            "</tr>" &
                            strrow &
                            "</table>"
                        '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" & _
                        '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                        '"</tr>" & _
                        'strrow & _
                        '"</table>"
                    End If

                Else
                    'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR and Sub Description, added Category and Analysis Codes, changed sequence.
                    table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1000px;"" id=""tblIPPDocItem"">" &
                               "<tr class=""TableHeader"">" &
                               "<td style=""width:50px;margin-right:0px;"" align=""right"">S/No</td>" &
                               "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:60px;margin-right:0px;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:45px;margin-right:0px;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:65px;margin-right:0px;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount</td>" &
                               "<td style=""width:50px; margin-right:0px;"">Category</td>" &
                               "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                               "<td style=""width:50px; margin-right:0px;"">Fund Type</td>" &
                               "<td style=""width:50px; margin-right:0px;"">Product Type</td>" &
                               "<td style=""width:50px; margin-right:0px;"">Channel</td>" &
                               "<td style=""width:50px; margin-right:0px;"">Reinsurance Company</td>" &
                               "<td style=""width:50px; margin-right:0px;"">Asset Fund</td>" &
                               "<td style=""width:50px; margin-right:0px;"">Project Code</td>" &
                               "<td style=""width:50px; margin-right:0px;"">Person Code</td>" &
                               "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                               "</tr>" &
                               strrow &
                               "</table>"
                    '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" & _
                    '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                    ' "</tr>" & _
                    'strrow & _
                    '"</table>"
                End If
            Else
                If exceedCutOffDt = "Yes" Or strIsGst = "Yes" Then
                    'Jules 2018.04.23 - Removed HO/BR and Sub Description, added Category and Analysis Codes, changed sequence.
                    table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1000px;"" id=""tblIPPDocItem"">" &
                           "<tr class=""TableHeader"">" &
                           "<td style=""width:50px;margin-right:0px;"" align=""right"">S/No</td>" &
                           "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:60px;margin-right:0px;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:45px;margin-right:0px;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:65px;margin-right:0px;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount</td>" &
                           "<td style=""width:110px;margin-right:0px;"" >GST Amount</td>" &
                           "<td style=""width:110px;margin-right:0px;"" >Input Tax Code</td>" &
                           "<td style=""width:110px;margin-right:0px;"" >Output Tax Code</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Category</td>" &
                           "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:50px; margin-right:0px;"">Fund Type</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Product Type</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Channel</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Reinsurance Company</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Asset Fund</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Project Code</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Person Code</td>" &
                           "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                           "</tr>" &
                           strrow &
                           "</table>"
                    '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" &
                    '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                    '"</tr>" &
                    'strrow &
                    '"</table>"
                Else
                    'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR and Sub Description, added Category and Analysis Codes, changed sequence.
                    table = "<table class=""grid"" style=""margin-top:20px; border-collapse:collapse; line-height:20px; width:1000px;"" id=""tblIPPDocItem"">" &
                           "<tr class=""TableHeader"">" &
                           "<td style=""width:50px;margin-right:0px;"" align=""right"">S/No</td>" &
                           "<td style=""width:145px;margin-right:0px;"">Transaction Description<span id=""lblDesc"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:60px;margin-right:0px;"">UOM<span id=""lblUOM"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:45px;margin-right:0px;"" align=""right"">QTY<span id=""lblQty"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:65px;margin-right:0px;"" align=""right"">Unit Price<span id=""lblUnitPrice"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:40px;margin-right:0px;"" align=""right"">Amount</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Category</td>" &
                           "<td style=""width:110px;margin-right:0px;"">GL Code<span id=""lblGLCode"" class=""errormsg"">*</span></td>" &
                           "<td style=""width:50px; margin-right:0px;"">Fund Type</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Product Type</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Channel</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Reinsurance Company</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Asset Fund</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Project Code</td>" &
                           "<td style=""width:50px; margin-right:0px;"">Person Code</td>" &
                           "<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" &
                           "</tr>" &
                           strrow &
                           "</table>"
                    '"<td style=""width:50px; margin-right:0px;"">HO/BR</td>" & _
                    '"<td style=""width:100px;margin-right:0px;"">Cost Centre</td>" & _
                    '"</tr>" & _
                    'strrow & _
                    '"</table>"
                End If
            End If

        End If

        Session("ConstructTable") = table
    End Function
    Sub buildarydoc()
        Dim i As Integer
        Dim compOutputTaxValue = objDB.GetVal("SELECT IP_param_value FROM ipp_parameter WHERE ip_param = 'REVERSE_CHARGE_OUTPUT'")
        For i = 0 To 9
            If Request.Form("txtRuleCategory" & i) = Request.Form("hidRuleCategoryVal" & i) And Not Request.Form("hidRuleCategoryVal" & i) = Nothing Then
                Dim subDescIndex = objDB.GetVal("SELECT igc_glrule_category_index FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" & _
                " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " & _
                "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And igg_gl_code LIKE '" & Request.Form("txtGLCode" & i).ToString.Split(":")(0).Trim & "' and igc_glrule_category = '" & Request.Form("txtRuleCategory" & i) & "' GROUP BY igc_glrule_category")
                If Not Request.Form("ddlInputTax" & i) Is Nothing Then
                    If Request.Form("ddlInputTax" & i).ToString.ToUpper = "IM2" Then
                        'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR, added Category and Analysis Codes.
                        'aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", Request.Form("txtBranch" & i), Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue})
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue,
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), Request.Form("txtAnalysisCode6_" & i), Request.Form("txtAnalysisCode7_" & i), Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i)})
                    Else
                        'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR, added Category and Analysis Codes.
                        'aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", Request.Form("txtBranch" & i), Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i)})
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), Request.Form("txtAnalysisCode6_" & i), Request.Form("txtAnalysisCode7_" & i), Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i)})
                    End If
                Else
                    'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR, added Category and Analysis Codes.
                    'aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", Request.Form("txtBranch" & i), Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i)})
                    aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), Request.Form("txtAnalysisCode6_" & i), Request.Form("txtAnalysisCode7_" & i), Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i)})
                End If
            ElseIf Not Request.Form("txtRuleCategory" & i) = Nothing And Not Request.Form("txtGLCode" & i) = Nothing Then
                Dim subDescIndex = objDB.GetVal("SELECT igc_glrule_category_index FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" & _
                " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " & _
                "AND ig_glrule_index = igg_glrule_index AND igc_glrule_category_active = 'Y' And igg_gl_code LIKE '" & Request.Form("txtGLCode" & i).ToString.Split(":")(0).Trim & "' and igc_glrule_category = '" & Request.Form("txtRuleCategory" & i) & "' GROUP BY igc_glrule_category")
                If Not Request.Form("ddlInputTax" & i) Is Nothing Then
                    If Request.Form("ddlInputTax" & i).ToString.ToUpper = "IM2" Then
                        'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR, added Category and Analysis Codes.
                        'aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", Request.Form("txtBranch" & i), Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue})
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue,
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), Request.Form("txtAnalysisCode6_" & i), Request.Form("txtAnalysisCode7_" & i), Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i)})
                    Else
                        'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR, added Category and Analysis Codes.
                        'aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", Request.Form("txtBranch" & i), Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i)})
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), Request.Form("txtAnalysisCode6_" & i), Request.Form("txtAnalysisCode7_" & i), Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i)})
                    End If
                Else
                    'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR, added Category and Analysis Codes.
                    'aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", Request.Form("txtBranch" & i), Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i)})
                    aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), subDescIndex, Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), Request.Form("txtAnalysisCode6_" & i), Request.Form("txtAnalysisCode7_" & i), Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i)})
                End If
            Else
                If Not Request.Form("ddlInputTax" & i) Is Nothing Then
                    If Request.Form("ddlInputTax" & i).ToString.ToUpper = "IM2" Then
                        'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR, added Category and Analysis Codes.
                        'aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", Request.Form("txtBranch" & i), Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i), Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue})
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i), Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), compOutputTaxValue,
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), Request.Form("txtAnalysisCode6_" & i), Request.Form("txtAnalysisCode7_" & i), Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i)})
                    Else
                        'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR, added Category and Analysis Codes.
                        'aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", Request.Form("txtBranch" & i), Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i), Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i)})
                        aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i), Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), Request.Form("txtAnalysisCode6_" & i), Request.Form("txtAnalysisCode7_" & i), Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i)})
                    End If
                Else
                    'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR, added Category and Analysis Codes.
                    'aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", Request.Form("txtBranch" & i), Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i), Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i)})
                    aryDoc.Add(New String() {Request.Form("txtSNo" & i), Request.Form("ddlPayFor" & i), Request.Form("txtInvNo" & i), Request.Form("txtDesc" & i), Request.Form("ddlUOM" & i), Request.Form("txtQty" & i), Request.Form("txtUnitPrice" & i), Request.Form("txtAmt" & i), Request.Form("txtGLCode" & i), Request.Form("txtAssetGroup" & i), Request.Form("txtAssetSubGroup" & i), "", "", Request.Form("txtCC" & i), "", "", "", "", Request.Form("txtRuleCategory" & i), Request.Form("hidRuleCategoryVal" & i), Request.Form("ddlReimbursement" & i), IIf(Request.Form("txtGSTAmount" & i) Is Nothing, 0.0, Request.Form("txtGSTAmount" & i)), Request.Form("ddlInputTax" & i), Request.Form("ddlOutputTax" & i),
                                   Request.Form("ddlCategory" & i), Request.Form("txtAnalysisCode1_" & i), Request.Form("txtAnalysisCode2_" & i), Request.Form("txtAnalysisCode3_" & i), Request.Form("txtAnalysisCode4_" & i), Request.Form("txtAnalysisCode5_" & i), Request.Form("txtAnalysisCode6_" & i), Request.Form("txtAnalysisCode7_" & i), Request.Form("txtAnalysisCode8_" & i), Request.Form("txtAnalysisCode9_" & i)})
                End If
            End If
        Next
    End Sub
    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim strscript As New System.Text.StringBuilder
        Dim objipp As New Billing
        Dim venidx As String
        ConstructTable()
        If validateInput() Then
            If validateField() Then
                If Not Request.QueryString("venIdx") Is Nothing Then
                    venidx = Request.QueryString("venIdx")
                Else
                    venidx = objDB.GetVal("select bm_s_coy_id from billing_mstr where bm_b_coy_id = '" & Common.Parse(Session("CompanyID")) & "' and bm_invoice_no='" & Common.Parse(Request.QueryString("docno")) & "' and bm_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                End If

                If Request.QueryString("action") = "edit" Then
                    objipp.UpdateBillingDocDetail(aryDoc, Request.QueryString("docno"), venidx, Request.QueryString("olddocno"), IIf(Request.QueryString("MasterDoc") = "Yes", "Y", "N"), strIsGst)
                Else
                    objipp.SaveBillingDocDetail (aryDoc, Request.QueryString("docno"), venidx, IIf(Request.QueryString("MasterDoc") = "Yes", "Y", "N"))
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

        For i = 0 To aryDoc.Count - 1
            If aryDoc.Item(i)(3) <> "" And aryDoc.Item(i)(5) <> "" And aryDoc.Item(i)(6) <> "" And aryDoc.Item(i)(8) <> "" Then

                If aryDoc.Item(i)(1) = "Own Co." Then
                    aryDoc.Item(i)(1) = Common.Parse(HttpContext.Current.Session("CompanyID"))
                End If
                If aryDoc.Item(i)(11) <> "" Then
                    'intCostAllocIndex = CInt(objDB.GetVal("SELECT cam_index FROM cost_alloc_mstr WHERE cam_ca_code = '" & aryDoc.Item(i)(11) & "' AND cam_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"))
                End If

                If InStr(aryDoc.Item(i)(8), ":") Then
                    aryDoc.Item(i)(8) = aryDoc.Item(i)(8).Substring(0, InStr(aryDoc.Item(i)(8), ":") - 1)
                End If

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
                    strSql = "SELECT igc_glrule_category_index, igc_glrule_category, igc_glrule_category_remark FROM ipp_glrule,ipp_glrule_category, Ipp_glrule_gl" & _
                    " WHERE ig_glrule_index = igc_glrule_index And ig_coy_id = igc_coy_id " & _
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

                'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR.
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
                '    End If
                'ElseIf aryDoc.Item(i)(12) = "" And (UCase(aryDoc.Item(i)(1)) = "HLB" Or UCase(aryDoc.Item(i)(1)) = "HLISB") Then
                '    strMsg = "HO/BR is required."
                '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                '    Return False
                'End If
                'End modification.

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
                    End If
                End If

                'Jules 2018.04.23 - PAMB Scrum 2 - Removed HO/BR.
                ''Check For BranchCode and CC combination
                'If aryDoc.Item(i)(12) <> "" And aryDoc.Item(i)(13) <> "" Then
                '    Dim sqlStr = "SELECT CC_CC_CODE, CC_CC_DESC FROM BRANCH_COST_CENTRE " & _
                '        "INNER JOIN COST_CENTRE ON BCC_CC_CODE = CC_CC_CODE AND BCC_COY_ID = CC_COY_ID " & _
                '        "WHERE BCC_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND BCC_BRANCH_CODE = '" & Common.Parse(aryDoc.Item(i)(12)) & "' and CC_CC_CODE = '" & Common.Parse(aryDoc.Item(i)(13)) & "'" & _
                '        "ORDER BY CC_CC_CODE "
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
                'End modification.

                'Zulham 17/08/2017 - IPP Stage 3
                'Zulham 21/06/2017
                'IPP Stage 3
                Dim docType = objDB.GetVal("SELECT IFNULL(bm_invoice_type,'') 'bm_invoice_type' FROM billing_mstr WHERE bm_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND bm_invoice_no='" & Common.Parse(Request.QueryString("docno")) & "' AND bm_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
                If docType.ToString.Contains("CN") Or docType.ToString.Contains("CNN") Or docType.ToString.Contains("DN") Or docType.ToString.Contains("DNN") Then
                    'Check for empty ref no field
                    If Not aryDoc.Item(i)(2) Is Nothing Then
                        If Trim(aryDoc.Item(i)(2)) = "" Then
                            strMsg = "Billing No. cannot be empty. "
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                            Return False
                        End If
                    End If
                End If
                ''''
                'Zulham 19/06/2017
                'IPP Stage 3
                'Check Invoice validity for CN and DN
                If Not aryDoc.Item(i)(2) Is Nothing Then
                    If Not aryDoc.Item(i)(2) = "" Then
                        Dim isValidInvNo = objDB.GetVal("select '*' from billing_mstr where bm_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' and bm_invoice_no='" & Common.Parse(aryDoc.Item(i)(2)) & "' and bm_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "' AND (billing_mstr.bm_invoice_status = '2' OR billing_mstr.bm_invoice_status in ('3','6'))")
                        If isValidInvNo = "" Then
                            strMsg = "Invalid billing no."
                            strMsg = strMsg & "[" & aryDoc.Item(i)(2) & "]"
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                            Return False
                        End If

                        'Dim invAmount = objDB.GetVal("select IF(im_payment_tERM = 'TT',im_invoice_totaL*im_exchange_rATE, IM_INVOICE_TOTAL) 'IM_INVOICE_TOTAL'  from invoice_mstr where im_b_coy_id = 'hlb' and im_invoice_no='" & Common.Parse(aryDoc.Item(i)(2)) & "' and im_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "' AND (invoice_mstr.im_invoice_status = '4' OR invoice_mstr.im_invoice_status = '13')")
                        Dim invAmount = objDB.GetVal("SELECT IF(IFNULL(bm_exchange_rate,0) <> 0,bm_invoice_totaL*bm_exchange_rATE, BM_INVOICE_TOTAL) 'BM_INVOICE_TOTAL' FROM billing_mstr WHERE bm_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND bm_invoice_no='" & Common.Parse(aryDoc.Item(i)(2)) & "' AND bm_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "' AND (bm_invoice_status = '2' OR bm_invoice_status in ('3','6'))")
                        totalInvAmount += invAmount
                        If Not aryDoc.Item(i)(2).ToString.Length = 0 Then totalItemAmount += CDec(aryDoc.Item(i)(7))

                    End If
                End If
                'End

                'Check the value for IM2 tax amount
                If aryDoc.Item(i)(22) = "IM2" Then
                    Dim strGSTPerc As String
                    Dim GSTAmount As Double = 0.0
                    Dim dsInputTax = GST.GetTaxCode_forIPP("", "P")
                    Dim strPerc() = dsInputTax.Tables(0).Select("TM_TAX_CODE = 'IM2'")
                    For Each row As DataRow In strPerc
                        strGSTPerc = row(0).ToString.Split("(")(1).Substring(0, 1)
                    Next
                    If Not aryDoc.Item(i)(5) Is Nothing And aryDoc.Item(i)(5) <> "" Then
                        GSTAmount = Format(CDec((strGSTPerc / 100) * CType(aryDoc.Item(i)(5), Double) * CType(aryDoc.Item(i)(6), Double)), "###0.00")
                    End If
                    If CDec(GSTAmount) <> CDec(aryDoc.Item(i)(21)) Then
                        strMsg = "Invalid GST Amount."
                        strMsg = strMsg & "[" & aryDoc.Item(i)(21) & "]"
                        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        Return False
                    End If
                End If
                'End

                If aryDoc.Item(i)(8) <> "" And aryDoc.Item(i)(11) <> "" Then

                End If

                'Zulham 11092014
                '•	Both Input Tax Code and Output Tax Code must be of the same range of tax percentage.
                Dim outputTaxRate = 0
                Dim inputTaxRate = 0

                '•	If both Input Tax Code and Output Tax Code selected as 0%, GST amount must be zero.
                If exceedCutOffDt = "Yes" And (strIsGst = "Yes" Or compType = "E") Then
                    If Not aryDoc.Item(i)(22) Is Nothing Then
                        'If aryDoc.Item(i)(22) = "Select" Then
                        '    strMsg = "Please Select GST Input Tax.[Line " & i + 1 & "]"
                        '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        '    Return False
                        'End If
                    End If
                    If Not aryDoc.Item(i)(23) Is Nothing Then
                        If aryDoc.Item(i)(23) = "Select" Then
                            strMsg = "Please Select GST Output Tax.[Line " & i + 1 & "]"
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                            Return False
                        End If
                    End If
                    'If aryDoc.Item(i)(22) = "NR" Then
                    '    strMsg = "Invalid selection for GST Input Tax[Line " & i + 1 & "]"
                    '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    '    Return False
                    'End If
                ElseIf exceedCutOffDt = "Yes" And strIsGst = "No" Then
                    'Check for non-GST-registered Company. They can only select NR
                    If Not aryDoc.Item(i)(22) Is Nothing Then
                        'If aryDoc.Item(i)(22) = "Select" Then
                        '    strMsg = "Please Select GST Input Tax.[Line " & i + 1 & "]"
                        '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        '    Return False
                        'End If
                    End If
                    If Not aryDoc.Item(i)(23) Is Nothing Then
                        If aryDoc.Item(i)(23) = "Select" Then
                            strMsg = "Please Select GST Output Tax.[Line " & i + 1 & "]"
                            Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                            Return False
                        End If
                    End If
                    If Not aryDoc.Item(i)(22) Is Nothing Then
                        'If aryDoc.Item(i)(22) <> "NR" Then
                        '    strMsg = "Invalid selection for GST Input Tax[Line " & i + 1 & "]"
                        '    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        '    Return False
                        'ElseIf aryDoc.Item(i)(22) = "NR" Then
                        '    If Not aryDoc.Item(i)(21) Is Nothing Then
                        '        If Not aryDoc.Item(i)(21).ToString.Length = 0 Then
                        '            If CDec(aryDoc.Item(i)(21)) > CDec(0) Then
                        '                strMsg = "GST Amount must be 0.[Line " & i + 1 & "]"
                        '                Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                        '                Return False
                        '            End If
                        '        End If
                        '    End If
                        'End If
                    End If
                End If

                If Not aryDoc.Item(i)(22) Is Nothing Then
                    'inputTaxRate = objDB.GetVal("SELECT TAX_PERC AS 'GSTRate' FROM tax_mstr, tax WHERE tm_coy_id = 'hlb' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code AND tm_tax_code = '" & aryDoc.Item(i)(22) & "'")
                    'If Not aryDoc.Item(i)(23) Is Nothing Then
                    '    outputTaxRate = objDB.GetVal("SELECT TAX_PERC AS 'GSTRate' FROM tax_mstr, tax WHERE tm_coy_id = 'hlb' AND tm_deleted <> 'Y' AND tm_tax_rate = tax_code AND tm_tax_code = '" & aryDoc.Item(i)(23) & "'")
                    'End If
                    'If Not aryDoc.Item(i)(22) Is Nothing And Not aryDoc.Item(i)(23) Is Nothing Then
                    '    If Not inputTaxRate = outputTaxRate Then
                    '        strMsg = "Selected input and output tax must be the same[Line " & i + 1 & "]"
                    '        Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    '        Return False
                    '    End If
                    'End If
                End If

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

                '•	If both Input Tax Code and Output Tax Code are 0% selected, GST amount must be zero.
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

                '•	Invoice header amount must be equal to the detail amount inclusive of GST amount.
                '•	Invoice header amount must be equal to the Sub document amount inclusive of GST amount.
                '•	All payment to Employee will be performed as vendor’s and will require user to enter the GST Input Tax Code.

                'End

                'Jules 2018.04.23 - PAMB Scrum 2 - Analysis Codes
                Dim dsMatrix As DataSet = Nothing
                dsMatrix = objIPPMain.getGLCodeAnalysisCodeMatrix(aryDoc.Item(i)(8))
                If dsMatrix IsNot Nothing Then
                    For ac As Integer = 1 To 9
                        If ac <> 6 AndAlso ac <> 7 Then
                            If dsMatrix.Tables(0).Rows(0)("CBGCAC_ANALYSIS_CODE" & ac & "").ToString = "M" Then
                                If aryDoc.Item(i)(25 + ac) Is Nothing OrElse aryDoc.Item(i)(25 + ac) = "" Then
                                    'strMsg = "Analysis Code " & ac & "is required [Line " & i + 1 & "]"
                                    'Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                                    vldsum.InnerHtml = "<li>Analysis Code " & ac & " " & objGlobal.GetErrorMessage("00001") & " [Line " & i + 1 & "]</li>"
                                    Return False
                                End If
                            End If
                            If aryDoc.Item(i)(24 + ac) IsNot Nothing Then
                                If InStr(aryDoc.Item(i)(24 + ac), ":") Then
                                    aryDoc.Item(i)(24 + ac) = aryDoc.Item(i)(24 + ac).Substring(0, InStr(aryDoc.Item(i)(24 + ac), ":") - 1)
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

        'Zulham 21/06/2017
        'IPP Stage 3
        Dim paymentType = objDB.GetVal("SELECT IFNULL(bm_invoice_type,'') 'bm_invoice_type' FROM billing_mstr WHERE bm_b_coy_id = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND bm_invoice_no='" & Common.Parse(Request.QueryString("docno")) & "' AND bm_s_coy_name = '" & Common.Parse(Request.QueryString("vencomp")) & "'")
        If paymentType.ToString.Contains("CN") Or paymentType.ToString.Contains("CNN") Then
            If Not totalInvAmount = 0.0 And Not totalItemAmount = 0.0 Then
                If CDec(totalItemAmount) > CDec(totalInvAmount) Then
                    strMsg = "Credit Note total amount must be lesser than the invoice total amount."
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Return False
                ElseIf CDec(totalItemAmount) = CDec(totalInvAmount) Then
                    strMsg = "Credit Note total amount must be lesser than the invoice total amount."
                    Common.NetMsgbox(Me, strMsg, MsgBoxStyle.Information)
                    Return False
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
            If aryDoc.Item(i)(3) <> "" Or aryDoc.Item(i)(5) <> "" Or aryDoc.Item(i)(6) <> "" Or aryDoc.Item(i)(8) <> "" Or aryDoc.Item(i)(12) <> "" Then

                If aryDoc.Item(i)(3) = "" Then
                    vldsum.InnerHtml = "<li>Description " & objGlobal.GetErrorMessage("00001") & "</li>"
                    Return False
                    'ElseIf aryDoc.Item(i)(4) = "---Select---" Then
                    '    vldsum.InnerHtml = "<li>UOM " & objGlobal.GetErrorMessage("00001") & "</li>"
                    '    Return False
                ElseIf aryDoc.Item(i)(5) = "" Then
                    vldsum.InnerHtml = "<li>Quantity " & objGlobal.GetErrorMessage("00001") & "</li>"
                    Return False
                ElseIf aryDoc.Item(i)(6) = "" Then
                    vldsum.InnerHtml = "<li>Unit Price " & objGlobal.GetErrorMessage("00001") & "</li>"
                    Return False
                ElseIf CType(aryDoc.Item(i)(7), Double) < 0 Then
                    vldsum.InnerHtml = "<li>Amount must not be in negative value.</li>"
                    Return False
                ElseIf aryDoc.Item(i)(8) = "" Then
                    vldsum.InnerHtml = "<li>GL Code " & objGlobal.GetErrorMessage("00001") & "</li>"
                    Return False
                ElseIf aryDoc.Item(i)(12) = "" And (aryDoc.Item(i)(1) = "Own Co." Or UCase(aryDoc.Item(i)(1)) = "HLISB") And aryDoc.Item(i)(11) = "" Then
                    vldsum.InnerHtml = "<li>HO/BR " & objGlobal.GetErrorMessage("00001") & "</li>"
                    Return False
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

End Class
