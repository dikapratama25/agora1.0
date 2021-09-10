<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportFormat12.aspx.vb" Inherits="eAdmin.ReportFormat12" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Report</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher

            Dim sEndDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEDate") & "','cal','width=180,height=155,left=290,top=240')""><IMG height=""16"" src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"

		</script>
		<%Response.Write(Session("WheelScript"))%>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" colspan="2"><FONT size="1">&nbsp;</FONT><asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="2"></TD>
				</TR>
				<TR>
					<TD class="tableheader" colSpan="2"><FONT size="1">&nbsp;</FONT><asp:label id="label10" runat="server">Report Criteria</asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" colspan="1" width="1%" nowrap>
						<asp:Label id="Label2" runat="server" Font-Bold="True">End Date</asp:Label><STRONG> </STRONG>
					</TD>
					<TD class="TableInput" width="90%"><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG></STRONG>
						<asp:textbox id="txtEDate" runat="server" CssClass="txtbox" MaxLength="10" contentEditable="false" ></asp:textbox><% response.write (sEndDt) %>
						<asp:requiredfieldvalidator id="ValEDate" runat="server" ControlToValidate="txtEDate" ErrorMessage="End Date is required."
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				</TR>
				<TR>
					<TD style="HEIGHT: 7px" colspan="2"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="2"></TD>
				</TR>
				<TR>
					<TD colspan="2">
						<asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;
						<INPUT class="button" id="cmdClear" type="button" value="Clear" runat="server" onclick="ValidatorReset();">
					</TD>
				</TR>
				<TR>
					<TD colspan="2"><BR>
						<asp:validationsummary id="Validationsummary1" runat="server" CssClass="errormsg"></asp:validationsummary>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="2">
    				    <asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="#"><STRONG>&lt; Back</STRONG></asp:hyperlink>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
