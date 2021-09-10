<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FramePDF.aspx.vb" Inherits="eProcure.FramePDF" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title></title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="ProgId" content="VisualStudio.HTML">
		<meta name="Originator" content="Microsoft Visual Studio .NET 7.1">
	    <script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sFrameMenu As String = "<FRAME name=""Menu"" src=""" & dDispatcher.direct("ExtraFunc", "MessagePDF.htm") & """ />"
		    Dim sFrameLoop As String = "<FRAME name=""Loop"" src=""" & dDispatcher.direct("ExtraFunc", "GeneratePDF.aspx") & """  />"
        </script>
	</head>
	<frameset rows="50%,*" border="0">	
		<% Response.Write(sFrameMenu)%>
		<% Response.Write(sFrameLoop)%>
	</frameset>
	<body>
	</body>
</html>