<%@ Page Language="vb" AutoEventWireup="false" Codebehind="viewvendorlist.aspx.vb" Inherits="eProcure.viewvendorlist" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>viewvendorlist</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header"><FONT size="1">&nbsp;</FONT><asp:label id="lblTitle" runat="server">Vendor List Details</asp:label></TD>
				</TR>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<TR>
					<TD class="emptycol">&nbsp;<strong>Name of Vendor List</strong>&nbsp;:<asp:label id="lbl_list" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_vendor" runat="server" OnSortCommand="SortCommand_Click" CssClass="grid"
							AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="CM_COY_NAME" HeaderText="Vendor Company Name">
									<HeaderStyle Width="50%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Contact Details ">
									<HeaderStyle Width="50%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_adds" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol"><A id="back" runat="server" href="#"><STRONG>&lt; Back</STRONG></A></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
