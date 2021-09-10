<%@ Page CodeBehind="Index.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="eAdmin.Index" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Frameset//EN">
<HTML>
  <HEAD><TITLE>eAdmin</TITLE>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sHead As String = "<frame name=""header"" src=""" & dDispatcher.direct("Initial", "Header.aspx") & """ frameBorder=""0"" noResize width=""800"" scrolling=""no"">"
            Dim sMenu As String = "<frame id=""menu"" name=""menu"" src=""" & dDispatcher.direct("Initial", "menu.aspx") & """ frameBorder=""0""  scrolling=""auto""  noResize>"
            Dim sHome As String = "<FRAME name=""body"" src=""" & dDispatcher.direct("Initial", "Homepage.aspx") & """ frameBorder=""0"" noResize scrolling=""auto"">"
        </script>
        
        
  </HEAD>
	<frameset border="0" frameSpacing="0" rows="53,75%" frameBorder="0">
		<%--<frame name="header" src="Header.aspx" frameBorder="0" noResize width="800" scrolling="no">--%>
		<% Response.Write(sHead)%>
		 <%--<FRAME id="description" name="desc" src="Info.aspx" frameBorder="no" noResize scrolling="no">--%>
		<frameset id="second" border="0" frameSpacing="0" frameBorder="0" cols="200,*" >
			<%--<frame id="menu" name="menu" src="menu.aspx" frameBorder="0"  scrolling="auto"  NORESIZE>--%>
			<% Response.Write(sMenu)%>
			<%--<FRAME name="body" src="Homepage.aspx" frameBorder="0" noResize scrolling="auto">--%>
			<% Response.Write(sHome)%>
		</frameset>
		<noframes>
			<pre id="p2">
================================================================
INSTRUCTIONS FOR COMPLETING THIS HEADER FRAMESET
1. Add the URL of your src="" page for the "header" frame.
2. Add the URL of your src="" page for the "main" frame.
3. Add a BASE target="main" element to the HEAD of  
	your "header" page, to set "main" as the default   
	frame where its links will display other pages. 
================================================================
</pre>
			<p id="p1">
				This HTML frameset displays multiple Web pages. To view this frameset, use a 
				Web browser that supports HTML 4.0 and later.
			</p>
		</noframes>
	</frameset>
</HTML>
