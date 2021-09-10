<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HolidayCalendarAdd.aspx.vb" Inherits="eAdmin.HolidayCalendarAdd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Holiday Calendar Add</title>
    <script runat="server">
        Dim dDispatcher As New AgoraLegacy.dispatcher
        ' Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
    </script>
   <%-- <% Response.Write(sCSS)%>--%>
    <% Response.Write(Session("WheelScript"))%>
	
	
	<script type="text/javascript" >
	<!--		
	
	function selectAll()
	{
		SelectAllG("dgHoliday<% Response.Write(Session("DataGridClientCheckAllId"))%>","chkSelection");
	}
			
	function checkChild(id)
	{
		checkChildG(id,"dgHoliday<% Response.Write(Session("DataGridClientCheckAllId"))%>","chkSelection");
	}
			
	function Reset(){
		var oform = document.forms(0);					
		oform.txtDate0.value="";
		oform.txtDate1.value="";
		oform.txtDate2.value="";
		oform.txtDate3.value="";
		oform.txtDate4.value="";
		oform.txtDate5.value="";
		oform.txtDate6.value="";
		oform.txtDate7.value="";
		oform.txtDate8.value="";
		oform.txtDate9.value="";
		oform.txtDesc0.value="";
		oform.txtDesc1.value="";
		oform.txtDesc2.value="";
		oform.txtDesc3.value="";
		oform.txtDesc4.value="";
		oform.txtDesc5.value="";
		oform.txtDesc6.value="";
		oform.txtDesc7.value="";
		oform.txtDesc8.value="";
		oform.txtDesc9.value="";
	}
	

	-->
	</script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
		<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
			<tr>			
				<td class="emptycol"><span class="header" >Holiday Calendar Maintainance</span></td>
			</tr>
			<tr>
				<td class="emptycol" style="HEIGHT: 16px"></td>
			</tr>
			</table>
		<%--	<tr>
				<td class="emptycol">--%>
					<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0" width="100%">
				<%--		<tr>
							<td id="lblTitle" class="tableheader" colspan="4">&nbsp;Search Criteria</td>
						</tr>--%>
				<tr>
					<td class="tableheader" colspan="4"><strong>
                        <asp:Label ID="lblTitle" runat="server" Text="Add Holiday Calendar" ></asp:Label></strong></td>
				</tr>	
	
						
			<tr>
				<td class="tablecol">
				 <asp:Label ID="Label1" runat="server" Text="Country: " ></asp:Label>
				  <asp:Label ID="lblCountry" runat="server" Text="" ></asp:Label>
				  <asp:Label ID="Label3" runat="server" Text="State: " ></asp:Label>
				  <asp:Label ID="lblState" runat="server" Text="" ></asp:Label>
				</td>
				
			</tr>
				  
			</table>
	 <br />
 <div id="divInvDetail" runat="server">
			<% response.write(Session("ConstructTable"))%>
			</div> 
			 <br />    
<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0">
			  
			<tr>
				<td class="emptycol">
				<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;				
				<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear" runat="server"/>&nbsp;
				</td>
			</tr>
			<tr>
				<td class="emptycol"></td>
			</tr>
				<tr>
				<td class="emptycol"><ul class="errormsg" id="vldsum" runat="server">
				</ul></td>
				</tr>
		
			<!--<tr>
				<td class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" NavigateUrl="javascript: history.back()" ForeColor="blue">
						<StrONG>&lt; Back</StrONG></asp:hyperlink></td>
			</tr>--></table>
	</form>
</body>
</html>
