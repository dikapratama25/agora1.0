<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RFQSubmit.aspx.vb" Inherits="eProcure.RFQSubmit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RFQSubmit</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" width="100%" cellSpacing="0" cellPadding="0" border="0"
				runat="server">
				<TR>
					<TD class="tableheader">Sent RFQ</TD>
				</TR>
				<TR>
					<TD class="tablecol">
						<P><STRONG>No.Of RFQ successfully Sent</STRONG> :
							<asp:label id="lbl_sent" runat="server"></asp:label><br>
							<STRONG>No.Of RFQ with no vendors assigned</STRONG> :
							<asp:label id="lbl_vendor" runat="server"></asp:label><br>
							<STRONG>No.Of RFQ with no items in RFQ</STRONG> :
							<asp:label id="lbl_item" runat="server"></asp:label><br>
							<STRONG>No.Of RFQ with RFQ expiry date before current date</STRONG> :
							<asp:label id="lbl_expire" runat="server"></asp:label><br>
							<STRONG>No.Of RFQ with no RFQ expiry date assigned </STRONG>:
							<asp:label id="lbl_Noexpire" runat="server"></asp:label></P>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0"
							runat="server">
							<TR>
								<TD class="emptycol"></TD>
							</TR>
							<TR>
								<TD class="emptycol"><asp:label id="lbl_duplicatesend" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD class="emptycol"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"><STRONG><FONT color="#000000">RFQ Sent</FONT></STRONG></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_send" runat="server" AutoGenerateColumns="False" CssClass="grid">
							<Columns>
								<asp:BoundColumn DataField="RM_RFQ_No" HeaderText="RFQ Number">
									<HeaderStyle Font-Underline="True" Font-Bold="True" Width="40%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RM_RFQ_Name" HeaderText="RFQ Name">
									<HeaderStyle Font-Underline="True" Font-Bold="True" Width="60%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><STRONG>No Vendors Assigned</STRONG></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_Vendor" runat="server" AutoGenerateColumns="False" CssClass="grid">
							<Columns>
								<asp:BoundColumn HeaderText="RFQ Number">
									<HeaderStyle Font-Underline="True" Font-Bold="True" Width="40%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="RFQ Name">
									<HeaderStyle Font-Underline="True" Font-Bold="True" Width="60%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><STRONG>No Item In RFQ</STRONG></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_item" runat="server" AutoGenerateColumns="False" CssClass="grid">
							<Columns>
								<asp:BoundColumn HeaderText="RFQ Number">
									<HeaderStyle Font-Underline="True" Font-Bold="True" Width="40%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="RFQ Name">
									<HeaderStyle Font-Underline="True" Font-Bold="True" Wrap="False" Width="60%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><STRONG>Expired Date</STRONG></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_expire" runat="server" AutoGenerateColumns="False" CssClass="grid">
							<Columns>
								<asp:BoundColumn DataField="RM_RFQ_No" HeaderText="RFQ Number">
									<HeaderStyle Font-Underline="True" Font-Bold="True" Width="40%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RM_RFQ_Name" HeaderText="RFQ Name">
									<HeaderStyle Font-Underline="True" Font-Bold="True" Width="60%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 14px"><STRONG>No Expired Date assigned</STRONG>&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 15px"><asp:datagrid id="dtg_Noexpire" runat="server" AutoGenerateColumns="False" CssClass="grid">
							<Columns>
								<asp:BoundColumn DataField="RM_RFQ_No" HeaderText="RFQ Number">
									<HeaderStyle Font-Underline="True" Font-Bold="True" Width="40%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RM_RFQ_Name" HeaderText="RFQ Name">
									<HeaderStyle Font-Underline="True" Font-Bold="True" Width="60%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 15px"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><A id="cmd_pre" runat="server" href="#"><STRONG>&lt; Back</STRONG></A></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
