<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Unauthorized.aspx.vb" Inherits="eProcure.Unauthorized" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Unauthorized</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sCSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
        </script>

		
		<% Response.Write(sCSS)%> 
	</HEAD>
	<BODY>
		<center>
			<BR>
			<BR>
			<BR>
			<h3>Sorry,You Are Not Allowed To View This Page
			</h3>
			<h4><FONT color="#000000">This Will Occur Because Of Either One Of The Following 
					Reason.</FONT></h4>
			<li>
				<FONT color="#000000">Your Session Login Has Expired. </FONT>
			<li>
				<FONT color="#000000">Your Are Not An Authorized Person.</FONT>
				<h4>Please <a href="/spp/eProcurement/Common/Initial/login.aspx">Login</a> Again
					<br>
					Thank You.</h4>
				<BR>
				<BR>
				<BR>
				<BR>
				<BR>
				<BR>
				<BR>
				<BR>
		</center>
		</LI>
	</BODY>
</HTML>
