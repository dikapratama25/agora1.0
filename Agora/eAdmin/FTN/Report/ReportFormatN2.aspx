<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportFormatN2.aspx.vb" Inherits="eAdmin.ReportFormatN21" %>

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
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
		</script>
		<% Response.Write(sCSS) %>
		<%Response.Write(Session("WheelScript"))%>
  </HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="HEIGHT: 16px" colspan="2"><FONT size="1">&nbsp;</FONT><asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="4"></TD>
				</TR>
				<TR>
				    <TD class="EmptyCol" colspan="4">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Microsoft Excel is required in order to open the report in Excel format."
					    ></asp:label>

				    </TD>
			    </TR>
			    <tr><TD class="emptycol" colSpan="4"></TD></tr>
				<TR>
					<TD class="tableheader" colSpan="4"><FONT size="1">&nbsp;</FONT><asp:label id="label2" runat="server">Report Criteria</asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" width="20%" nowrap>&nbsp;<STRONG>Month From</STRONG><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput" width="30%" nowrap style="height: 20px">
                        &nbsp;<asp:dropdownlist id="cmbMonthFrom" runat="server" CssClass="txtbox"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvMonthFrom" runat="server" ErrorMessage="Month From is required."
							ControlToValidate="cmbMonthFrom" Display="None"></asp:requiredfieldvalidator></TD>
					<TD class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<STRONG>Year From</STRONG><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput" width="30%" nowrap style="height: 20px">
						<asp:dropdownlist id="cmbYearFrom" runat="server" CssClass="txtbox"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvYearFrom" runat="server" ErrorMessage="Year From is required." ControlToValidate="cmbYearFrom"
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD class="tablecol" nowrap style="height: 20px">&nbsp;<STRONG>Month To</STRONG><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput" nowrap style="height: 20px">
                        &nbsp;<asp:dropdownlist id="cmbMonthTo" runat="server" CssClass="txtbox"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvMonthTo" runat="server" ErrorMessage="Month To is required." ControlToValidate="cmbMonthTo"
							Display="None"></asp:requiredfieldvalidator></TD>
					<TD class="tablecol" nowrap style="height: 20px">&nbsp;<STRONG>Year To</STRONG><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput"  nowrap style="height: 20px">
						<asp:dropdownlist id="cmbYearTo" runat="server" CssClass="txtbox"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvYearTo" runat="server" ErrorMessage="Year To is required." ControlToValidate="cmbYearTo"
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD class="tablecol">&nbsp;<STRONG>Report Type</STRONG><asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD colspan="3" class="TableInput" style="HEIGHT: 17px" colSpan="1">&nbsp;<asp:dropdownlist id="cboReportType" runat="server" CssClass="txtbox" Width="128px">
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
					    <asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="#"><STRONG>&lt; Back</STRONG></asp:hyperlink>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
