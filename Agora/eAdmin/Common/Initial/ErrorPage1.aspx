<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ErrorPage1.aspx.vb" Inherits="eAdmin.ErrorPage1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ErrorPage</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellPadding="1" width="100%" border="0">
				<TR>
					<TD><H2>The following error occured, please make change</H2>
					</TD>
				</TR>
				<TR>
					<TD><asp:Label ID="lblMessage" Runat="server" ForeColor="Red"></asp:Label></TD>
				</TR>
				<TR>
					<TD><asp:Button ID="cmdSend" Text="Send Mail" Runat="server"></asp:Button>
						<INPUT type="button" value="Back" onclick="history.back();"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
