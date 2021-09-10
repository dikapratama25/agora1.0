<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HubAddDiscountItem.aspx.vb" Inherits="eAdmin.HubAddDiscountItem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Add Contract Item</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
            Dim calStartDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtStartDate") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
            Dim calEndDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEndDate") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
            </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
		<!--
		
			function cmdAddClick()
			{
				var result = confirm("Save PR?", "Yes", "No");
				if(result == true)
					Form1.hidAddItem.value = "1";
				else 
					Form1.hidAddItem.value = "0";
			}
			
			function selectAll()
			{
				SelectAllG("dtgCatalogue__ctl2_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCatalogue__ctl2_chkAll","chkSelection");
			}
			
			function Reset()
			{
				Form1.txtVendorItemCode.value = "";
				Form1.txtItemCode.value = "";
				Form1.txtItemDesc.value = "";
			}

		-->
		</script>
	</HEAD>
	<body onload="" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="TableHeader" colSpan="2">&nbsp;<asp:Label id="lblHeader" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="left">
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR vAlign="top">
								<TD class="tablecol" width="15%" nowrap>&nbsp;<STRONG><asp:label id="lblCodeLabel" runat="server"></asp:label></STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="30%"><asp:label id="lblCode" runat="server"></asp:label></TD>
								<TD class="tablecol" width="15%" nowrap>&nbsp;<STRONG>Description</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="40%"><asp:label id="lblDesc" runat="server"></asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" nowrap>&nbsp;<STRONG>Start Date</STRONG>&nbsp;:</TD>
								<TD class="TableInput"><asp:label id="lblStartDate" runat="server"></asp:label><% Response.Write(calStartDate)%></TD>
								<TD class="tablecol" nowrap>&nbsp;<STRONG>End Date</STRONG>:</TD>
								<TD class="TableInput"><asp:label id="lblEndDate" runat="server"></asp:label><% Response.Write(calEndDate)%></TD>
							</TR>
							<TR vAlign="top" runat="server" id="trBuyer">
								<TD class="tablecol" nowrap>&nbsp;<STRONG>Buyer Company</STRONG>&nbsp;:</TD>
								<TD class="TableInput" colSpan="3"><asp:label id="lblBuyer" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tableheader">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
					<TD class="tablecol">&nbsp;<STRONG>Vendor Item Code</STRONG> :
						<asp:textbox id="txtVendorItemCode" runat="server" CssClass="txtbox" Width="100px"></asp:textbox>&nbsp;<STRONG>Item 
							ID</STRONG> :
						<asp:textbox id="txtItemCode" runat="server" CssClass="txtbox" Width="100px"></asp:textbox>&nbsp;<STRONG>Item 
							Desc.</STRONG> :
						<asp:textbox id="txtItemDesc" runat="server" CssClass="txtbox" Width="100px"></asp:textbox>&nbsp;<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
						<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<tr>
					<td><asp:datagrid id="dtgCatalogue" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
							Width="100%">
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
								<asp:TemplateColumn SortExpression="PM_Product_Code" HeaderText="Item ID">
									<HeaderStyle HorizontalAlign="Left" Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PM_Vendor_Item_Code" SortExpression="PM_Vendor_Item_Code" ReadOnly="True"
									HeaderText="Vendor Item Code">
									<HeaderStyle Width="18%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_Product_Desc" SortExpression="PM_Product_Desc" ReadOnly="True" HeaderText="Item Description">
									<HeaderStyle Width="38%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_Currency_Code" SortExpression="PM_Currency_Code" ReadOnly="True" HeaderText="Currency">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_Unit_Cost" SortExpression="PM_Unit_Cost" ReadOnly="True" HeaderText="Price">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM" ReadOnly="True" HeaderText="UOM">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:button id="cmdContract" runat="server" CssClass="Button" Width="96px" Text="Contract Price"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					</TD>
				</TR>
				<TR>
					<TD>&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
