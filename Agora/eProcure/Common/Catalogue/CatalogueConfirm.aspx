<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CatalogueConfirm.aspx.vb" Inherits="eProcure.CatalogueConfirm" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Confirm Contract Catalogue</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header"><FONT size="1"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR runat="server" id="trCode">
					<TD class="emptycol"><asp:label id="lblCode" runat="server" Width="505px"></asp:label></TD>
				</TR>
				<TR runat="server" id="trHeader">
					<TD class="emptycol"><asp:label id="lblPR" runat="server" Width="505px"></asp:label></TD>
				</TR>
				<TR runat="server" id="trDetails">
					<TD class="emptycol"><asp:label id="lblDetail" runat="server" Width="505px"></asp:label></TD>
				</TR>
				<TR runat="server" id="trHeaderBlank">
					<TD class="emptycol"></TD>
				</TR>
				<TR runat="server" id="trNotHeader">
					<TD class="emptycol"><asp:label id="lblNotHeader" runat="server" Width="505px"></asp:label></TD>
				</TR>
				<TR runat="server" id="trNotDetail">
					<TD class="emptycol"><asp:label id="lblNotDetail" runat="server" Width="505px"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol">
					</TD>
				</TR>
				<TR runat="server" id="trRemark">
					<TD class="emptycol"><asp:label id="lblRemark" runat="server" Width="505px"></asp:label></TD>
				</TR>
				<TR>
					<TD><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
