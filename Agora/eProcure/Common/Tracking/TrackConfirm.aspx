<%@ Page Language="vb" AutoEventWireup="false" Codebehind="TrackConfirm.aspx.vb" Inherits="eProcure.TrackConfirm" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>View Shopping Cart</title>
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
				<TR runat="server" id="trHeader">
					<TD class="tableheader">&nbsp;<asp:label id="lblPR" runat="server" Width="505px"></asp:label></TD>
				</TR>
				<TR runat="server" id="trItem">
					<TD class="tablecol">&nbsp;<asp:label id="lblItem" runat="server" Width="505px"></asp:label>
					</TD>
				</TR>
				<TR runat="server" id="trSpace">
					<TD class="emptycol"></TD>
				</TR>
				<TR runat="server" id="trHeaderFail">
					<TD class="tableheader">&nbsp;<asp:label id="lblPRFail" runat="server" Width="505px"></asp:label></TD>
				</TR>
				<TR runat="server" id="trItemFail">
					<TD class="tablecol">&nbsp;<asp:label id="lblItemFail" runat="server" Width="505px"></asp:label>
					</TD>
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
