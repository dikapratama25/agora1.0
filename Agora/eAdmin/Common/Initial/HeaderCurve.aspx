<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HeaderCurve.aspx.vb" Inherits="Wheel.HubAdmin.HeaderCurve" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>HeaderCurve</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sImg As String = "<IMG src=""" & dDispatcher.direct("Plugins/Images", "bg1.JPG") & """>"
        </script>
	</HEAD>
	<body background="Images\bg.JPG" topMargin="0" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div align="right">
				<% Response.Write(sImg)%></div>
		</form>
	</body>
</HTML>
