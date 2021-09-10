<%@ Page Language="vb" AutoEventWireup="false" Codebehind="errorpage.aspx.vb" Inherits="eProcure.errorpage2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>errorpage</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="HEIGHT: 19px"><FONT size="1"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></FONT></TD>
				</TR>				
				<TR runat="server" id="trConfirm">
					<TD class="emptycol">&nbsp;</TD>
				</TR>
				<TR runat="server" id="trExistHeader">
					<TD class="tableheader">&nbsp;<asp:label id="lbl_Rfq_num" runat="server" Width="505px"></asp:label></TD>
				</TR>
				<TR runat="server" id="trExistItem">
					<TD class="tablecol">&nbsp;<asp:label id="lblItemExist" runat="server" Width="505px"></asp:label>
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
