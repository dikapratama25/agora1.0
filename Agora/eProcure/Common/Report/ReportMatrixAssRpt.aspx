<%--Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.--%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportMatrixAssRpt.aspx.vb" Inherits="eProcure.ReportMatrixAssRpt" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>ReportMatrixAssRpt</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
            <tr>
					<td class="header" style="padding:0" ><asp:label id ="lblTitle" runat="server" Text="Report Assignment - By Report"></asp:label></td>
				</tr>
				<tr>
					<td class="linespacing2" colspan="6"></td>
			    </tr>
			<tr>
	                <td colspan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Click Assign button to tie report to the users or Remove button to remove report from the users."
		                ></asp:label>

	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colspan="6"></td>
			    </tr>
						<tr>
								<td class="tableheader">&nbsp;Assignment By Report</td>
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0" >
							<tr>
								<td class="tablecol" nowrap style="height: 25px">&nbsp;<strong>Report</strong> :&nbsp;<asp:Label
                                    ID="lblReportName" runat="server"></asp:Label>
                                    &nbsp;&nbsp;
                                   </td>
							</tr>
			</table>
			</table>
						<div id="Div_AA" style="DISPLAY: inline" runat="server">
							<table class="alltable" cellspacing="0" cellpadding="0" border="0">
								<tr>
									<td class="TableCol" style="WIDTH: 314px">&nbsp;<strong>&nbsp;Available Users </strong>:</td>
									<td class="TableCol" style="WIDTH: 81px"></td>
									<td class="TableCol" style="width: 353px"><strong>Selected Users </strong>:</td>
								</tr>
								<tr>
									<td class="TableCol" style="WIDTH: 314px">&nbsp;
										<asp:listbox id="lstbox1" runat="server" Width="300px" SelectionMode="Multiple" cssClass="listbox"></asp:listbox></td>
									<td class="TableCol" style="WIDTH: 81px" >
										<P><asp:button id="cmdAdd1" runat="server" CssClass="button" Text="Assign"></asp:button></P>
										<P><asp:button id="cmdAdd2" runat="server" CssClass="button" Text="Remove"></asp:button></P>
									</td>
									<td class="TableCol" style="width: 353px"><asp:listbox id="lstBox2" runat="server" Width="300px" SelectionMode="Multiple" cssClass="listbox"></asp:listbox></td>
								</tr>
							</table>
							<table class="alltable" cellspacing="0" cellpadding="0" border="0">
								<tr>
									<td class="emptycol"></td>
								</tr>
								<tr>
									<td><asp:button id="cmdsave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdReset" runat="server" CssClass="button" Text="Reset"></asp:button></td>
								</tr>
                                <tr>
					                <td class="linespacing2" colspan="6"></td>
			                    </tr>
							</table>
						</div>
			<table class="alltable" cellspacing="0" cellpadding="0">
				<tr>
					<td class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<strong>&lt; Back</strong></asp:hyperlink></td>
				</tr>
			</table>
		</form>
	</body>
</html>
