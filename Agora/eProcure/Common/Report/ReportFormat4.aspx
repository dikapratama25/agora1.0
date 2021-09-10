<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportFormat4.aspx.vb" Inherits="eProcure.ReportFormat4" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Report</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="HEIGHT: 16px" colspan="2"><FONT size="1">&nbsp;</FONT><asp:label id="lblHeader" runat="server">Vendor Listing</asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tableheader" colSpan="3" style="HEIGHT: 16px"><FONT size="1">&nbsp;</FONT><asp:label id="label2" runat="server">Report Criteria</asp:label></TD>
				</TR>
				<TR>
					<TD class="TableCol" colSpan="3"><asp:radiobuttonlist id="oplList" runat="server" AutoPostBack="True">
							<asp:ListItem Value="A" Selected="True">Approved Vendor</asp:ListItem>
							<asp:ListItem Value="NA">Not Approved Vendor</asp:ListItem>
						</asp:radiobuttonlist></TD>
				</TR>
				<TR>
					<TD class="tablecol" colspan="1" width="12%">&nbsp;<STRONG><asp:label id="Label3" runat="server" Font-Bold="True">Report Type</asp:label></STRONG>&nbsp;:
					</TD>
					<TD class="TableInput" style="HEIGHT: 17px" colSpan="1">&nbsp;
						<asp:dropdownlist id="cboReportType" runat="server" CssClass="txtbox" Width="128px">
                            <asp:ListItem Selected="True">Excel</asp:ListItem>
                            <asp:ListItem>PDF</asp:ListItem>
                        </asp:dropdownlist><asp:requiredfieldvalidator id="ValReportType" runat="server" ErrorMessage="Report Type is required." ControlToValidate="cboReportType"
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD>
						<BR>
						<asp:button id="cmdSubmit" runat="server" Text="Submit" CssClass="button"></asp:button></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="2"><BR>
						<asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
