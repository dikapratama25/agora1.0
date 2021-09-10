<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportFormat6.aspx.vb" Inherits="eProcure.ReportFormat6"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>Report</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<%Response.Write(Session("WheelScript"))%>
  </HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" colspan="4"><FONT size="1">&nbsp;</FONT><asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="4"></TD>
				</TR>
				<TR>
					<TD class="tableheader" colSpan="4"><FONT size="1">&nbsp;</FONT><asp:label id="label2" runat="server">Report Criteria</asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" width="20%" nowrap>&nbsp;<STRONG>Invoice Month From</STRONG><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput" width="30%" nowrap>
						<asp:dropdownlist id="cboMonthFrom" runat="server" CssClass="txtbox"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvMonthFrom" runat="server" ErrorMessage="Invoice Month From is required."
							ControlToValidate="cboMonthFrom" Display="None"></asp:requiredfieldvalidator></TD>
					<TD class="tablecol" width="20%" nowrap>&nbsp;<STRONG>Invoice Year From</STRONG><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput" width="30%" nowrap>
						<asp:dropdownlist id="cboYearFrom" runat="server" CssClass="txtbox"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvYearFrom" runat="server" ErrorMessage="Invoice Year From is required." ControlToValidate="cboYearFrom"
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD class="tablecol" nowrap>&nbsp;<STRONG>Invoice Month To</STRONG><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput" nowrap>
						<asp:dropdownlist id="cboMonthTo" runat="server" CssClass="txtbox"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvMonthTo" runat="server" ErrorMessage="Invoice Month To is required." ControlToValidate="cboMonthTo"
							Display="None"></asp:requiredfieldvalidator></TD>
					<TD class="tablecol" nowrap>&nbsp;<STRONG>Invoice Year To</STRONG><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput"  nowrap>
						<asp:dropdownlist id="cboYearTo" runat="server" CssClass="txtbox"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvYearTo" runat="server" ErrorMessage="Invoice Year To is required." ControlToValidate="cboYearTo"
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD class="tablecol" colspan="1" width="10%">&nbsp;<STRONG><asp:label id="Label6" runat="server" Font-Bold="True">Report Type</asp:label></STRONG>&nbsp;:
					</TD>
					<TD class="TableInput" style="HEIGHT: 17px" colSpan="1">&nbsp;
						<asp:dropdownlist id="cboReportType" runat="server" CssClass="txtbox" Width="128px">
                            <asp:ListItem Selected="True">Excel</asp:ListItem>
                            <asp:ListItem>PDF</asp:ListItem>
                        </asp:dropdownlist><asp:requiredfieldvalidator id="ValReportType" runat="server" ErrorMessage="Report Type is required." ControlToValidate="cboReportType"
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD colSpan="4"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>indicates 
						required field
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<TR>
					<TD colSpan="4"><asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" type="button" value="Clear" onclick="ValidatorReset();"
							runat="server" NAME="cmdClear"></TD>
				</TR>
				<TR>
					<TD colspan="4"><BR>
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="4">
						<asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
