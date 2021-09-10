<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportFormat3.aspx.vb" Inherits="eProcure.ReportFormat3" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Report</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<%Response.Write(Session("WheelScript"))%>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table4" cellSpacing="0" cellpadding="0" border="0">
				<tr>
					<td class="header" style="HEIGHT: 16px" colspan="2"><FONT size="1">&nbsp;</FONT><asp:label id="lblHeader" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2"></td>
				</tr>
				<tr>
					<td class="tableheader" colspan="2"><FONT size="1">&nbsp;</FONT><asp:label id="label2" runat="server">Report Criteria</asp:label></td>
				</tr>
				<tr id="tr1" runat="server">
				    <td align="left" colspan="2" class="tablecol">					    
                    <asp:radiobuttonlist ID="dtRadioBtn" CssClass="rbtn" RepeatDirection="Horizontal" runat="server"> 
                    <asp:ListItem Value="opex" Selected="True">Opex</asp:ListItem>
						<asp:ListItem Value="capex">Capex</asp:ListItem>
					</asp:radiobuttonlist></td>
		        </tr>		        
				<tr id="trMth" runat="server"><%--Jules 2016.01.27 - FITR enhancement--%>
					<td class="tablecol" width="20%" nowrap>&nbsp;<strong>Month</strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:<strong>
						</strong>
					</td>
					<td class="TableInput" width="80%">&nbsp;
						<asp:dropdownlist id="cboMonth" runat="server" CssClass="txtbox" Width="128px"></asp:dropdownlist><asp:requiredfieldvalidator id="ValMth" runat="server" ErrorMessage="Month is required." ControlToValidate="cboMonth"
							Display="None"></asp:requiredfieldvalidator></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Year</strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:<strong>
						</strong>
					</td>
					<td class="TableInput" style="HEIGHT: 17px" colspan="1">&nbsp;
						<asp:dropdownlist id="cboYear" runat="server" CssClass="txtbox" Width="128px"></asp:dropdownlist><asp:requiredfieldvalidator id="ValYr" runat="server" ErrorMessage="Year is required." ControlToValidate="cboYear"
							Display="None"></asp:requiredfieldvalidator></td>
				</tr>
				<tr>
					<td class="tablecol" colspan="1" width="10%">&nbsp;<strong><asp:label id="Label3" runat="server" Font-Bold="True">Report Type</asp:label></strong>&nbsp;:
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
					<td colspan="2"><asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" type="button" value="Clear" onclick="ValidatorReset();"
							runat="server"></td>
				</tr>
				<tr>
					<td colspan="2"><br/>
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2">
						<asp:hyperlink id="lnkBack" Runat="server">
							<strong>&lt; Back</strong></asp:hyperlink>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
