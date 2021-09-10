<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Header.aspx.vb" Inherits="eProcure.HeaderFTN1" %>
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
        </script>
        <% Response.Write(sCSS)%>
		<% Response.Write(Session("PNGFix"))%>

	</HEAD>
	<body leftMargin="0" topMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server" style="border-bottom: 2px double #666;">
		<div style="height:50px; width:100%;" class="hd_bg">
            <div style="width:230px; float: left; margin: 2px 10px;">
                <asp:image id="Image1" runat="server" height="48" width="177" style="margin:0px;"></asp:image>
                <asp:label id="lblLogo" runat="server">No Logo</asp:label>
            </div>
            <div style="float:left; margin:7px 0px;">
                <div class="login_info"><asp:Label Runat="server" ID="lblWelcome"></asp:Label></div>
	            <div class="login_info"><asp:Label Runat="server" ID="lblLastLogin"></asp:Label><asp:HyperLink ID="lnkstatus" runat="server" Target="_parent" style="font-size:14px; font-weight:bold; color:Red; margin-left:10px; "></asp:HyperLink></div>
	        </div>
	        <div style="float:right; width:200px;"><% Response.Write(Session("sHeadSideImg"))%></div>
            <div style="clear:both;"></div>
        </div>
        
			<%--<table height="50" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td align="left" width="22%" style="overflow:hidden;"><asp:image id="Image1" runat="server" height="48" width="177px" style="margin:0px;"></asp:image><asp:label id="lblLogo" runat="server">No Logo</asp:label></td>
					<td width="73%" style="HEIGHT: 100%;" class="hd_bg">
					    <div class="login_info"><asp:Label Runat="server" ID="lblWelcome"></asp:Label></div>
					    <div class="login_info"><asp:Label Runat="server" ID="lblLastLogin"></asp:Label><asp:HyperLink ID="lnkstatus" runat="server" Target="_parent" style="font-size:14px; font-weight:bold; color:Red; margin-left:10px; "></asp:HyperLink></div>					    
					</td>
					<td bgcolor="#acbddb" width="160px"><% Response.Write(sHeadSideImg)%></td>
				</tr>
			</table>--%>
		</form>
	</body>
</HTML>
