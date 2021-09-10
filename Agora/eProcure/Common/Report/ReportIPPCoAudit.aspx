<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportIPPCoAudit.aspx.vb" Inherits="eProcure.ReportIPPCoAudit" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head>
		<title>ReportIPPCoAudit</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
		     Dim dDispatcher As New AgoraLegacy.dispatcher
		     Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
		     Dim sStartDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtSDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"
             Dim sEndDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEndDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"
		</script>
		<% Response.Write(sCSS) %>
		<%Response.Write(Session("WheelScript"))%>
		
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td class="header" style="HEIGHT: 16px" colspan="2"><font size="1">&nbsp;</font><asp:label id="lblHeader" runat="server"></asp:label></td>
				</tr>
				<tr><td class="emptycol" colspan="2"></td></tr>
			    <tr>
				    <td class="EmptyCol" colspan="2">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Microsoft Excel is required in order to open the report in Excel format."
					    ></asp:label>

				    </td>
			    </tr>
			    <tr><td class="emptycol" colspan="2"></td></tr>
				<tr>
					<td class="tableheader" colspan="2"><FONT size="1">&nbsp;</FONT><asp:label id="label2" runat="server">Report Criteria</asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" width="20%" nowrap>&nbsp;<strong>Start Date</strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:<strong>
						</strong>
					</td>
					<td class="TableInput" width="80%">&nbsp;
						<asp:textbox id="txtSDate" runat="server" CssClass="txtbox" MaxLength="10" contentEditable="false"></asp:textbox><% response.write (sStartDt) %>
						<asp:requiredfieldvalidator id="ValSDate" runat="server" ControlToValidate="txtSDate" ErrorMessage="Start Date is required."
							Display="None"></asp:requiredfieldvalidator></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>End Date</strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:<strong>
						</strong>
					</td>
					<td class="TableInput" style="HEIGHT: 17px" colspan="1">&nbsp;
						<asp:textbox id="txtEndDate" runat="server" CssClass="txtbox" MaxLength="10" contentEditable="false" ></asp:textbox><% response.write (sEndDt) %>
						<asp:requiredfieldvalidator id="ValEDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="End Date is required."
							Display="None"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="cvDate" runat="server" ErrorMessage="Start Date should be <= End Date." ControlToValidate="txtEndDate"
							Display="None" Type="Date" Operator="GreaterThanEqual" ControlToCompare="txtSDate"></asp:comparevalidator></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Audit on</strong>&nbsp;:
					</td>
					<td class="TableInput" style="HEIGHT: 17px" colspan="1">&nbsp;
						<asp:dropdownlist id="cboAudit" runat="server" CssClass="txtbox" Width="128px">
						    <asp:ListItem Value="CC" Selected="True">Company Category</asp:ListItem>
                            <asp:ListItem Value="RT">Resident Type</asp:ListItem>
                        </asp:dropdownlist></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Report Type</strong><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:<strong>
						</strong>
					</td>
					<td class="TableInput" style="HEIGHT: 17px" colspan="1">&nbsp;
						<asp:dropdownlist id="cboReportType" runat="server" CssClass="txtbox" Width="128px">
                            <asp:ListItem Selected="True">Excel</asp:ListItem>
                            <asp:ListItem>PDF</asp:ListItem>
                        </asp:dropdownlist><asp:requiredfieldvalidator id="ValReportType" runat="server" ErrorMessage="Report Type is required." ControlToValidate="cboReportType"
							Display="None"></asp:requiredfieldvalidator></td>
				</tr>
				<tr>
					<td colspan="2"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2"></td>
				</tr>
				<tr>
					<td colspan="2"><asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;<input class="button" id="cmdClear" type="button" value="Clear" onclick="ValidatorReset();"
							runat="server" name="cmdClear"/></td>
				</tr>
				<tr>
					<td colspan="2"><br/>
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2">
                        <asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="#"><strong>&lt; Back</strong></asp:hyperlink>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>