<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportFormat10.aspx.vb" Inherits="eProcure.ReportFormat10"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Report</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../css/Styles.css" type="text/css" rel="stylesheet">
		<!--#include file = "../include/WheelScript.js"-->
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
					<TD class="tablecol" colspan="1" width="20%" nowrap style="HEIGHT: 29px">&nbsp;<STRONG>Start 
							Date</STRONG><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput" width="80%" style="HEIGHT: 29px">&nbsp;
						<asp:textbox id="txtSDate" runat="server" CssClass="txtbox" MaxLength="10" ReadOnly="True"></asp:textbox><A onclick="window.open('../Calendar/viewCalendar.aspx?TextBox=txtSDate','cal','width=180,height=155,left=290,top=240')"><IMG style="CURSOR: hand" height="16" src="../Images/i_calendar2.gif" align="absBottom"
								vspace="0"></A><asp:requiredfieldvalidator id="ValSDate" runat="server" ControlToValidate="txtSDate" ErrorMessage="Start Date is required."
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD class="tablecol" colspan="1" style="HEIGHT: 29px">&nbsp;<STRONG>End Date</STRONG><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput" style="HEIGHT: 29px">&nbsp;
						<asp:textbox id="txtEndDate" runat="server" CssClass="txtbox" MaxLength="10" ReadOnly="True"></asp:textbox><A onclick="window.open('../Calendar/viewCalendar.aspx?TextBox=txtEndDate','cal','width=180,height=155,left=290,top=240')"><IMG style="CURSOR: hand" height="16" src="../Images/i_calendar2.gif" align="absBottom"
								vspace="0"></A><asp:requiredfieldvalidator id="ValEDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="End Date is required."
							Display="None"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="cvDate" runat="server" ErrorMessage="Start Date should be <= End Date." ControlToValidate="txtEndDate"
							Display="None" Type="Date" Operator="GreaterThanEqual" ControlToCompare="txtSDate"></asp:comparevalidator></TD>
				</TR>
				<TR>
					<TD class="tablecol" style="HEIGHT: 33px">&nbsp;<STRONG><asp:Label id="lblPerfectDay_time" runat="server">Label</asp:Label></STRONG><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>:<STRONG></STRONG>
					</TD>
					<TD class="TableInput" style="HEIGHT: 33px" colSpan="1">&nbsp;
						<asp:TextBox id="txtPerfectDay_time" runat="server" CssClass="txtbox"></asp:TextBox><asp:requiredfieldvalidator id="ValPerfectDay_time" runat="server" ErrorMessage="Days/Time is required." ControlToValidate="txtPerfectDay_time"
							Display="None"></asp:requiredfieldvalidator></TD>
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
						<INPUT class="button" id="cmdClear" type="button" value="Clear" runat="server" onclick="ValidatorReset();"
							NAME="cmdClear">
					</TD>
				</TR>
				<TR>
					<TD colspan="2"><BR>
						<asp:validationsummary id="Validationsummary1" runat="server" CssClass="errormsg"></asp:validationsummary>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="2">
						<asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
