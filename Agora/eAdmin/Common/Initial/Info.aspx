<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Info.aspx.vb" Inherits="eAdmin.Info" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Logout</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
        </script>
        <% Response.Write(sCSS)%>
		
	</HEAD>
	<body MS_POSITIONING="GridLayout" topmargin="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">
			<table width="100%" cellSpacing="0" cellPadding="0" border="0">
				<tr class="body_login_info">
					<td class="login_info">
						<asp:Label Runat="server" ID="lblUserAndComp"></asp:Label>
					</td>
					<td><FONT size="1"></FONT></td>
					<td align="right" class="login_info">Last Log On
						<asp:label id="lblLastLogOn" Runat="server">N/A</asp:label>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
