<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Comlist.aspx.vb" Inherits="eProcure.Comlist" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Comlist</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		 <script language="javascript">
		<!--
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}	
		-->
		</script>	
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
					<TD class="emptycol">&nbsp;<STRONG>Vendor List</STRONG> :
						<asp:label id="lbl_list" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_vendor" runat="server" AutoGenerateColumns="False" CssClass="grid" OnSortCommand="SortCommand_Click">
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
								<asp:BoundColumn DataField="CM_COY_ID" HeaderText="CM_COY_ID" Visible="false">									
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<tr><td class=emptycol>&nbsp;</td></tr>
				<TR>
					<TD class="emptycol"><a href="#" onclick="history.back();"><STRONG>&lt; Back</STRONG></a></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
