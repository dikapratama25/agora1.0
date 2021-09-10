<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Unauthorized.aspx.vb" Inherits="eAdmin.Unauthorized" %>
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
           Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
       </script>
        <% Response.Write(css)%>   
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
			<h5>
				<FONT color="#000000">Your Session Login Has Expired. </FONT>
			</h5>
				<FONT color="#000000">Your Are Not An Authorized Person.</FONT>
				<h4>Please <a href="login.aspx">Login</a> Again
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
	</BODY>
</HTML>
