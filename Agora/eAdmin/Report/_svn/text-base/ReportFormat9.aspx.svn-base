<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportFormat9.aspx.vb" Inherits="eAdmin.ReportFormat9"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Report</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
		    Dim dDispatcher As New Dispatcher.dispatcher
	        Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
	        
	        Dim sStartDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtSDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"
	        Dim sEndDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEndDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"

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
					<TD class="emptycol" colspan="2"></TD>
				</TR>
				<TR>
					<TD class="tableheader" colSpan="2"><FONT size="1">&nbsp;</FONT><asp:label id="label2" runat="server">Report Criteria</asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" width="20%" nowrap style="HEIGHT: 31px">&nbsp;<STRONG>Company Name</STRONG><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput" width="80%" style="HEIGHT: 31px">&nbsp;
						<asp:dropdownlist id="cboCompanyName" runat="server" CssClass="txtbox" AutoPostBack="True"></asp:dropdownlist><asp:requiredfieldvalidator id="ValCompanyName" runat="server" ErrorMessage="Company Name is required." ControlToValidate="cboCompanyName"
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD class="tablecol" style="HEIGHT: 26px">&nbsp;<STRONG>User Name</STRONG><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput" style="HEIGHT: 26px" colSpan="1">&nbsp;
						<asp:dropdownlist id="cboUserName" runat="server" CssClass="txtbox"></asp:dropdownlist><asp:requiredfieldvalidator id="ValUserName" runat="server" ErrorMessage="User name is required." ControlToValidate="cboUserName"
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD class="tablecol" width="20%" nowrap>&nbsp;<STRONG>Start Date</STRONG><asp:label id="Label8" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;
						</STRONG>:
					</TD>
					<TD class="TableInput" width="80%">&nbsp;
						<asp:textbox id="txtSDate" runat="server" Width="128px" CssClass="txtbox" MaxLength="10" contentEditable="false" ></asp:textbox><% response.write (sStartDt) %>
						<asp:requiredfieldvalidator id="ValSDate" runat="server" ErrorMessage="Start Date is required." ControlToValidate="txtSDate"
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD class="tablecol">&nbsp;<STRONG>End Date</STRONG><asp:label id="Label9" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput">&nbsp;
						<asp:textbox id="txtEndDate" runat="server" Width="128px" CssClass="txtbox" MaxLength="50" contentEditable="false" ></asp:textbox><% response.write (sEndDt) %>
						<asp:requiredfieldvalidator id="ValEDate" runat="server" ErrorMessage="End Date is required." ControlToValidate="txtEndDate"
							Display="None"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="cvDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="Start Date should be <= End Date."
							ControlToCompare="txtSDate" Operator="GreaterThanEqual" Type="Date" Display="None"></asp:comparevalidator></TD>
				</TR>
				<TR>
					<TD colSpan="2"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"></TD>
				</TR>
				<TR>
					<TD colSpan="2"><asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" type="button" value="Clear" onclick="ValidatorReset();"
							runat="server" NAME="cmdClear"></TD>
				</TR>
				<TR>
					<TD colspan="2"><BR>
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
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
