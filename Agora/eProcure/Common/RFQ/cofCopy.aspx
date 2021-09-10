<%@ Page Language="vb" AutoEventWireup="false" Codebehind="cofCopy.aspx.vb" Inherits="eProcure.cofCopy" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>cofCopy</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table2" cellPadding="0" width="100%" border="0" class="alltable">
				<TR>
					<TD class="header">Copy RFQ Draft</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tablecol"><STRONG>Successfully Copied into RFQ Draft Folder. </STRONG>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<asp:datagrid id="dtg_rfqcopy" runat="server" CssClass="grid" AutoGenerateColumns="False" Width="300px">
							<Columns>
								<asp:BoundColumn DataField="RM_RFQ_No" HeaderText="RFQ Number">
									<HeaderStyle Width="45%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RM_RFQ_NAME" HeaderText="RFQ Name">
									<HeaderStyle HorizontalAlign="Left" Width="55%" VerticalAlign="Middle"></HeaderStyle>
									<FooterStyle HorizontalAlign="Left" VerticalAlign="Middle"></FooterStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol"><A runat="server" id="cmd_pre" href="#"><STRONG>&lt; Back</STRONG></A></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
