<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CRCof.aspx.vb" Inherits="eProcure.CRCof" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CRCof</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="HEIGHT: 19px"><FONT size="1"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR id="trHeader" runat="server">
					<TD class="tableheader">&nbsp;PO Cancellation</TD>
				</TR>
				<TR id="trItem" runat="server">
					<TD class="tablecol">&nbsp;<asp:label id="lbl_Rfq_num" runat="server"></asp:label>
					</TD>
				</TR>
				<TR id="trConfirm" runat="server">
					<TD class="emptycol">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<asp:Button id="cmd_cr" runat="server" CssClass="BUTTON" Text="View CR"></asp:Button></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
