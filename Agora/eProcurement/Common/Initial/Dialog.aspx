<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Dialog.aspx.vb" Inherits="eProcurement.Dialog" %>


<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<META NAME="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<TITLE>Policy Agreement</TITLE>
	</HEAD>

	<frameset rows="0,*">
		<frame name="header" src="" scrolling="no" noresize>
		response.write (Request.QueryString("Page"))
		<frame name="main" <% response.write("src=" & Request.QueryString("Page") & ">")  %>
	</frameset>
</HTML>