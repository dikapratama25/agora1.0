<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DiscountCatalogueList.aspx.vb" Inherits="eProcure.DiscountCatalogueList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Catalogue Approval</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		 <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
           Dim calStartDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
           Dim calEndDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
      </script>
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
			function Reset(){
				var oform = document.forms(0);
				oform.txtCode.value="";
				oform.txtDesc.value="";
				oform.txtDateFr.value="";
				oform.txtDateTo.value="";
			}
		
			function selectAll()
			{
				SelectAllG("dtgCatalogue_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCatalogue_ctl02_chkAll","chkSelection");
			}
				
		-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="header" colspan="6"><FONT size="1"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="6">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="tableheader" colspan="6">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
					<TD class="tablecol" width="18%" nowrap>&nbsp;<STRONG>Discount Group Code</STRONG>&nbsp;:&nbsp;</TD>
					<TD class="tablecol" width="199" nowrap style="WIDTH: 199px"><asp:textbox id="txtCode" runat="server" CssClass="txtbox" Width="120px"></asp:textbox></TD>
					<TD class="tablecol" width="13%" nowrap>&nbsp;<STRONG>Description</STRONG>&nbsp;:&nbsp;</TD>
					<TD class="tablecol" width="30%" nowrap><asp:textbox id="txtDesc" runat="server" CssClass="txtbox" Width="250px"></asp:textbox></TD>
					<TD class="tablecol" width="12%" nowrap>&nbsp;</TD>
					<TD class="tablecol" width="7%" nowrap>&nbsp;</TD>
				</TR>
				<tr class="tablecol">
					<TD class="tablecol" nowrap>&nbsp;<STRONG>Start Date</STRONG>&nbsp;:</TD>
					<TD class="tablecol" nowrap style="WIDTH: 199px"><asp:textbox id="txtDateFr" runat="server" Width="90px" CssClass="txtbox"  contentEditable="false" ></asp:textbox><% Response.Write(calStartDate)%></TD>
					<TD class="tablecol" nowrap>&nbsp;<STRONG>End Date</STRONG>&nbsp;:</TD>
					<TD class="tablecol" nowrap><asp:textbox id="txtDateTo" runat="server" Width="90px" CssClass="txtbox"  contentEditable="false" ></asp:textbox><% Response.Write(calEndDate)%></TD>
					<td colspan="2">&nbsp;</td>
				</tr>
				<tr class="tablecol">
					<td colspan="6"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"><asp:textbox id="txtBuyer" runat="server" CssClass="txtbox" Width="100px" Visible="False"></asp:textbox>
						<asp:comparevalidator id="cvDate" runat="server" Type="Date" Operator="GreaterThanEqual" ControlToCompare="txtDateFr"
							Display="None" ErrorMessage="Start Date should be <= End Date." ControlToValidate="txtDateTo"></asp:comparevalidator></td>
				</tr>
				<TR>
					<TD class="emptycol" colspan="6"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
							ShowSummary="False"></asp:validationsummary></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="6">&nbsp;</TD>
				</TR>
				<TR>
					<TD colspan="6"><asp:datagrid id="dtgCatalogue" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
							DataKeyField="CDM_GROUP_INDEX" Width="100%">
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
								<asp:TemplateColumn SortExpression="CDM_GROUP_CODE" HeaderText="Discount Group Code">
									<HeaderStyle HorizontalAlign="Left" Width="24%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
										<asp:Label ID="lblIndex" Runat="server" Visible="False"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CDM_GROUP_DESC" SortExpression="CDM_GROUP_DESC"  readonly="true"  HeaderText="Description">
									<HeaderStyle Width="49%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_START_DATE" SortExpression="CDM_START_DATE"  readonly="true"   HeaderText="Start Date">
									<HeaderStyle Width="11%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_END_DATE" SortExpression="CDM_END_DATE"  readonly="true"  HeaderText="End Date">
									<HeaderStyle Width="11%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="6"></TD>
				</TR>
				<tr runat="server" id="trDiscount">
					<td colspan="6">
						<asp:button id="cmdAdd" runat="server" CssClass="Button" Text="Add"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" CssClass="Button" Text="Modify"></asp:button>&nbsp;<asp:button id="cmdCompany" runat="server" CssClass="Button" Width="128px" Text="Company Assignment"></asp:button>&nbsp;<asp:button id="cmdItem" runat="server" CssClass="Button" Width="120px" Text="Discount Group Item"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="Button" Text="Delete"></asp:button></td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
