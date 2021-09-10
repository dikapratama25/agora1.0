<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportFormat14.aspx.vb" Inherits="eProcure.ReportFormat14" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
  <head>
		<title>Report</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
		     Dim dDispatcher As New AgoraLegacy.dispatcher
		     Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
		</script>
		<% Response.Write(sCSS) %>
		<%Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		
		</script>
  </head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td class="header" style="HEIGHT: 16px" colspan="2"><font size="1">&nbsp;</font><asp:label id="lblHeader" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4"></td>
				</tr>
				<tr>
				    <td class="EmptyCol" colspan="4">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Microsoft Excel is required in order to open the report in Excel format."
					    ></asp:label>

				    </td>
			    </tr>
			    <tr><td class="emptycol" colspan="4"></td></tr>
				<tr>
					<td class="tableheader" colspan="4" style="height: 19px"><font size="1">&nbsp;</font><asp:label id="label2" runat="server">Report Criteria</asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" nowrap style="width: 120px; height: 20px;">&nbsp;<strong>Buyer ID</strong><strong>&nbsp;</strong>:<strong>
						</strong>
					</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
                        &nbsp;<asp:TextBox ID="txtBuyerID" CssClass="txtbox" runat="server"></asp:TextBox></td>
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Buyer Name</strong><strong>&nbsp;</strong>:<strong>
						</strong>
					</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
                        <asp:TextBox ID="txtBuyerName" CssClass="txtbox" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td class="tablecol" nowrap style="height: 20px; width: 120px;">&nbsp;<strong>Storekeeper
                        ID</strong><strong>&nbsp;</strong>:<strong>
						</strong>
					</td>
					<td class="TableInput" nowrap style="height: 20px">
                        &nbsp;<asp:TextBox ID="txtSKID" CssClass="txtbox" runat="server"></asp:TextBox></td>
					<td class="tablecol" nowrap style="height: 20px">&nbsp;<strong>Storekeeper Name</strong><strong>&nbsp;</strong>:<strong>
						</strong>
					</td>
					<td class="TableInput"  nowrap style="height: 20px">
                        <asp:TextBox ID="txtSKName" CssClass="txtbox" runat="server"></asp:TextBox></td>
				</tr>
				<tr>
					<td class="tablecol" style="width: 120px">&nbsp;<strong>Report Type</strong><asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:<strong>
						</strong>
					</td>
					<td colspan="3" class="TableInput" style="HEIGHT: 17px" >&nbsp;<asp:dropdownlist id="cboReportType" runat="server" CssClass="txtbox" Width="128px">
                            <asp:ListItem Selected="True">Excel</asp:ListItem>
                        <asp:ListItem>PDF</asp:ListItem>
                        </asp:dropdownlist><asp:requiredfieldvalidator id="ValReportType" runat="server" ErrorMessage="Report Type is required." ControlToValidate="cboReportType"
							Display="None"></asp:requiredfieldvalidator></td>
				</tr>
				<tr>
					<td colspan="4" style="height: 19px"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>indicates 
						required field
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4" style="height: 19px"></td>
				</tr>
				<tr>
					<td colspan="4"><asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;<input class="button" id="cmdClear" type="button" value="Clear" onclick="ValidatorReset();"
							runat="server" name="cmdClear"/></td>
				</tr>
				<tr>
					<td colspan="4"><br/>
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">
					    <asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="#"><strong>&lt; Back</strong></asp:hyperlink>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
