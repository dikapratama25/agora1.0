<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Index.aspx.vb" Inherits="eProcure.Index2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD><TITLE>eProcure</TITLE>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sFrameMenu As String = "<FRAME name=""menu"" src=""" & dDispatcher.direct("Initial", "Menu.aspx") & """ frameBorder=""0"" noResize scrolling=""auto"" />"
            Dim sFrameBody As String = "<FRAME name=""body"" src=""" & dDispatcher.direct("Initial", "Homepage.aspx") & """ frameBorder=""0"" noResize scrolling=""auto"" />"
        </script>
        
  </HEAD>
    <%--Zulham 09112018--%>
	<frameset border="0" frameSpacing="0" rows="140,75%" frameBorder="0">
		<frame id="header"  name="header" src="header.aspx" frameBorder="0" noResize width="800" scrolling="no">
        <%--<frame id="info"  name="info" src="Info.aspx" frameBorder="no" noResize width="800" scrolling="no">--%>
		<frameset id="second" border="0" frameSpacing="0" frameBorder="0" cols="200,*" >
			<% Response.Write(sFrameMenu)%>
			<% Response.Write(sFrameBody)%>
		</frameset>
	</frameset>
</HTML>
