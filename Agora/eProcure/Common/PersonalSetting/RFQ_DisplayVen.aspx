<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RFQ_DisplayVen.aspx.vb" Inherits="eProcure.RFQ_DisplayVen" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Vendor Selection</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">		
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		function selectAll()
		{
			SelectAllG("dtgVenSelection_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgVenSelection_ctl02_chkAll","chkSelection");
		}
									
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form2" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TBODY>
					<TR>
						<TD class="header"><FONT size="1">&nbsp;</FONT>Vendor Selection</TD>
					</TR>
					<TR>
						<TD class="emptycol"></TD>
					</TR>
					<TR>
						<TD class="emptycol" style="WIDTH: 529px">&nbsp;<STRONG>List Name </STRONG>:&nbsp;<asp:label id="lblDisplay" runat="server" Width="128px" CssClass="label"></asp:label></TD>
					</TR>
					<TR>
						<TD class="emptycol"></TD>
					</TR>
					<TR>
						<TD></TD>
					</TR>
					<TR>
						<TD><asp:datagrid id="dtgVenSelection" runat="server" CssClass="Grid" AutoGenerateColumns="False"
								OnSortCommand="SortCommand_Click" AllowSorting="True" DataKeyField="CM_Coy_Id" OnPageIndexChanged="MyDataGrid_Page">
								<Columns>
									<asp:TemplateColumn HeaderText="Delete">
										<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<HeaderTemplate>
											<asp:checkbox id="chkAll" runat="server" ToolTip="Select/Deselect All" AutoPostBack="false"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
										</HeaderTemplate>
										<ItemTemplate>
											<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor Company Name">
										<HeaderStyle Width="40%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Contact Details ">
										<HeaderStyle Width="55%"></HeaderStyle>
										<ItemTemplate>
											<asp:Label id="lbl_adds" runat="server"></asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
							</asp:datagrid></TD>
					</TR>
					<TR>
						<TD class="emptycol"></TD>
					</TR>
					<TR>
						<TD><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" CausesValidation="False"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" onclick="DeselectAllG('dtgCustomField_ctl02_chkAll','chkSelection')"
								type="reset" value="Reset" runat="server" style="DISPLAY:none">&nbsp; <INPUT id="hidMode" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="hidMode"
								runat="server"> <INPUT id="hidIndex" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="Hidden1"
								runat="server">
						</TD>
					</TR>
					<TR>
						<TD class="emptycol"></TD>
					</TR>
					<TR>
						<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
								<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
					</TR>
				</TBODY>
			</TABLE>
		</form>
		<DIV></DIV>
		</TR></TBODY></TABLE></FORM>
	</body>
</HTML>
