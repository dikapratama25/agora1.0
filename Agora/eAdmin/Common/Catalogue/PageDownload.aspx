<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PageDownload.aspx.vb" Inherits="eAdmin.PageDownload"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PageDownload</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
       <script runat="server">
           Dim dDispatcher As New AgoraLegacy.dispatcher
           Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
       </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table5" cellSpacing="0" cellPadding="0" width="300" border="0">
				<TBODY>
					<TR>
						<TD class="header" style="HEIGHT: 17px"><asp:label id="lblTitle" runat="server" CssClass="header"> Download</asp:label></TD>
					</TR>
					<TR>
						<TD class="emptycol"></TD>
					</TR>
					<TR>
						<td>
							<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0">
								<TR>
									<TD class="tableheader" colSpan="3">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></TD>
								</TR>
								<TR class="tablecol">
									<TD style="WIDTH: 92px; HEIGHT: 6px" width="92"></TD>
									<TD style="HEIGHT: 6px"></TD>
								</TR>
								<TR class="tablecol">
									<TD>&nbsp;<B>File Location :</B></TD>
									<TD>&nbsp;&nbsp;<input class="button" id="cmdBrowse" style="FONT-SIZE: 8pt; WIDTH: 340px; FONT-FAMILY: verdana; BACKGROUND-COLOR: white"
											type="file" name="cmdBrowse" runat="server">&nbsp;</TD>
									<TD></TD>
								</TR>
								<TR class="tablecol">
									<TD>&nbsp;</TD>
									<TD style="HEIGHT: 5px">&nbsp;&nbsp;<asp:label id="lblPath" CssClass="txtbox" Runat="server" Height="5px"></asp:label></TD>
								</TR>
								<TR class="tablecol">
									<TD style="WIDTH: 92px; HEIGHT: 6px" width="92"></TD>
									<TD style="HEIGHT: 6px"></TD>
								</TR>
							</TABLE>
						</td>
					</TR>
					<TR>
						<TD class="emptycol"></TD>
					</TR>
				</TBODY>
			</TABLE>
			<asp:button id="cmdDownload" style="Z-INDEX: 101; LEFT: 304px; POSITION: absolute; TOP: 232px"
				runat="server" CssClass="button" Text="Download"></asp:button>
			<TABLE BORDER="0" CELLSPACING="0" CELLPADDING="0" class="alltable">
				<TR class="emptycol">
					<TD></TD>
				</TR>
				<TR class="emptycol">
					<TD><asp:Label id="lblMsg" runat="server" CssClass="errormsg"></asp:Label></TD>
				</TR>
				<TR class="emptycol">
					<TD></TD>
				</TR>
				<TR class="emptycol">
					<TD>
						<asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
