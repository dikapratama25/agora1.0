<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HolidayCalendar.aspx.vb" Inherits="eAdmin.HolidayCalendar" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Holiday Calendar</title>
    <script runat="server">
        Dim dDispatcher As New AgoraLegacy.dispatcher
        ' Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
    </script>

    <% Response.Write(Session("JQuery")) %> 
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
	   // var Date = new Date();
		var oform = document.forms(0);					
		oform.cboCountry.value="MY";
		oform.cboState.value="";
		//oform.txtYear.value=Date.getFullYear();
	}
	function refreshgrid()
		{
		 document.all("btnhidden").click();
		}
	function PopWindow(myLoc)
			{
				window.open(myLoc,"Wheel","width=800,height=600,location=no,toolbar=no,menubar=no,scrollbars=yes,resizable=no");
				return false;
			}	
	-->
	</script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
		<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
			<tr>			
				<td class="emptycol"><span class="header" id="lblTitle">Holiday Calendar</span></td>
			</tr>
			<tr>
				<td class="emptycol" style="HEIGHT: 16px"></td>
			</tr>
			</table>
		<%--	<tr>
				<td class="emptycol">--%>
					<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0" width="100%">
						<tr>
							<td class="tableheader" colspan="4">&nbsp;Search Criteria</td>
						</tr>
	
						<tr class="tablecol">
				<td class="tablecol"><strong>&nbsp;Country</strong><span class="errorMsg">*</span>&nbsp; :</td>
				<td class="tablecol"><asp:dropdownlist id="cboCountry" Width="160" CssClass="ddl" runat="server" AutoPostBack="True"></asp:dropdownlist></td>			
					<td class="tablecol" colspan="2"></td>
			</tr>
			<tr>
				<td class="tablecol"><strong>&nbsp;State</strong><span class="errorMsg"></span>&nbsp;:</td>
				<td class="tablecol"><asp:dropdownlist id="cboState" Width="160" CssClass="ddl" runat="server"></asp:dropdownlist></td>
				<td class="tablecol" colspan="2"></td>
			</tr>
			<tr>
				<td class="tablecol"><strong>&nbsp;Year</strong><span class="errorMsg">*</span>&nbsp;:</td>
				<td class="tablecol"><asp:textbox id="txtYear" runat="server" Width="160px" MaxLength="50" CssClass="txtbox"></asp:textbox></td>
				<%--<td class="tablecol"><asp:requiredfieldvalidator id="rfv_txtYear" runat="server" ControlToValidate="txtYear" ErrorMessage="Year Required"
						Display="None"></asp:requiredfieldvalidator></td>--%>
				<td class="tablecol" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
				<%--<input class="button" id="cmdClear" type="button" value="Clear" name="cmdClear" />&nbsp;</td>--%>
				<asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear"></asp:button></td>
				<asp:button id="btnhidden" runat="server" CssClass="Button"  Text="btnhidden" style=" display :none"></asp:button>
			</tr>
				  
			</table>
	 <br />
		<%--		</td>
			</tr>--%>
			<%--<tr>
				<td class="emptycol"><asp:label id="lbl_result" runat="server" ForeColor="Red"></asp:label></td>
			</tr>--%>
<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0">
			<tr>
				<td class="emptycol" width="100%"><asp:datagrid id="dgHoliday" runat="server">
						<Columns>
							<asp:TemplateColumn HeaderText="Delete">
								<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<HeaderTemplate>
									<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
								</HeaderTemplate>
								<ItemTemplate>
									<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
								</ItemTemplate>
							</asp:TemplateColumn>									
								<asp:BoundColumn DataField="hm_date" SortExpression="hm_date" HeaderText="Holiday Date">
								<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundColumn>
								<asp:BoundColumn DataField="hm_day" SortExpression="hm_day" HeaderText="Day">
								<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="hm_desc" SortExpression="hm_desc" HeaderText="Description">
								<HeaderStyle HorizontalAlign="Left" Width="60%"></HeaderStyle>
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundColumn>
							
								<asp:BoundColumn DataField="hm_index" SortExpression="hm_index" Visible = "false">
								<HeaderStyle Width="6%"></HeaderStyle>
							</asp:BoundColumn>
						</Columns>
					</asp:datagrid></td>
			</tr>
	<%--		<tr>
				<td class="emptycol">&nbsp;&nbsp;</td>
			</tr>--%>
			<tr>
				<td class="emptycol">
				<asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add"></asp:button>&nbsp;
				<asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify" Enabled="False"></asp:button>&nbsp;
				<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Enabled="False"></asp:button>&nbsp;
				</td>
			</tr>
			</table>
			<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" border="0">
			<%--	<tr>
				<td class="emptycol"><asp:validationsummary id="vldsumm" runat="server" Width="696px" cssclass ="errormsg"></asp:validationsummary></td>
			</tr>--%>
			<tr>
				<td class="emptycol"><ul class="errormsg" id="vldsum" runat="server">
				</ul></td>
				</tr>
			<tr>
				<td class="emptycol"></td>
			</tr>
			<!--<tr>
				<td class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" NavigateUrl="javascript: history.back()" ForeColor="blue">
						<StrONG>&lt; Back</StrONG></asp:hyperlink></td>
			</tr>--></table>
	</form>
</body>
</html>
