<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Header.aspx.vb" Inherits="eAdmin.Header" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Header</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
            Dim sHeadImg As String = "<IMG src=""" & dDispatcher.direct("Plugins/Images", "strateqgrp.jpg") & """ height=""48"" style=""margin:0px;"">"
            Dim sHeadSideImg As String = "<IMG src=""" & dDispatcher.direct("Plugins/Images", "MyFairTradeNet_header.png") & """ style=""margin:0px;"">"
        </script>
        <% Response.Write(sCSS)%>
        <% Response.Write(Session("PNGFix"))%>
        
	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server" style="border-bottom: 2px double #666;">
			<table height="50" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td width="22%" align="center"><% Response.Write(sHeadImg)%></td>
					<TD width="73%" style="HEIGHT: 100%;" class="hd_bg">
					    <div class="login_info"><asp:Label Runat="server" ID="lblWelcome"></asp:Label></div>
					    <div class="login_info"><asp:Label Runat="server" ID="lblLastLogin"></asp:Label></div>
					</TD>
					<td bgcolor="#acbddb" width="5%"><% Response.Write(sHeadSideImg)%></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
