<%@ LANGUAGE="VBSCRIPT" %>
<%
reportname = Request.QueryString("rptname")
'response.Write reportname 
'RESPONSE.END

%>
<!-- #include file="AlwaysRequiredSteps.asp" -->
  

<%
'sUserID = request.QueryString("User_ID")
server.ScriptTimeout = 300
'response.Write request.QueryString("sf") 
if request.QueryString("sf") <> "" then
	session("oRpt").RecordSelectionFormula = request.QueryString("sf")
end if

'Set Paramter Fields
dim a
'Session("oRpt").ParameterFields.GetItemByName("ExampleStringParameter").AddCurrentValue(CStr("I am a string"))
for a = 1 to request.QueryString.Count 
	if isdate(request.QueryString.Item(a)) then
			Session("oRpt").ParameterFields.GetItemByName(Cstr(request.QueryString.key(a))).AddCurrentValue(Cdate(request.QueryString.Item(a)))
			'Response.Write "date"
	elseif isnumeric(request.QueryString.Item(a)) then
			Session("oRpt").ParameterFields.GetItemByName(Cstr(request.QueryString.key(a))).AddCurrentValue(Cint(request.QueryString.Item(a)))
	else
			Session("oRpt").ParameterFields.GetItemByName(Cstr(request.QueryString.key(a))).AddCurrentValue(Cstr(request.QueryString.Item(a)))
	end if
	'Response.Write Cstr(request.QueryString.key(a)) & "=" & Cstr(request.QueryString.Item(a)) & "<br>"
next
'Session("oRpt").ParameterFields.GetItemByName("moo").AddCurrentValue(CStr("Iamastring"))
%>
<!-- #include file="OpenServerConn.asp" -->
<!-- #include file="MoreRequiredSteps.asp" -->
<!-- #include file="SmartViewerActiveX.asp" --> 


