<%@ Page Language="vb" AutoEventWireup="false" Codebehind="POLineListing.aspx.vb" Inherits="eProcure.POLineListing" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>POLineListing</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="tblPOHeader1" cellspacing="0" cellpadding="0" width="100%"
				border="0">
				<tr>
					<td class="header" colspan="2"><FONT size="1">&nbsp;</FONT>PO Line Listing</td>
					<td align="center" colspan="2"></td>
				</tr>
				<tr>
					<td align="center">&nbsp;</td>
					<td colspan="6"></td>
				</tr>
				<tr>
					<td class="tablecol" align="left" width="25%"><strong>&nbsp;PO Number</strong> :&nbsp;</td>
					<td class="TableInput" width="25%">&nbsp;<asp:label id="lblPONo" runat="server"></asp:label></td>
					<td class="tablecol" width="25%"><strong>&nbsp;Order Date</strong> :&nbsp;</td>
					<td class="TableInput" width="25%" colspan="2">&nbsp;<asp:label id="lblOrderDate" runat="server"></asp:label></td>
				</tr>
			</table>
			<hr/>
			<table class="alltable" id="tblPOHeader2" cellspacing="0" cellpadding="0" width="100%"
				border="0">
				<tr>
					<td class="emptycol" colspan="4">&nbsp;&nbsp;</td>
				</tr>
				<tr>
					<td class="tablecol" align="left" width="25%" colspan="1"><strong>&nbsp;Line</strong>
						:&nbsp;</td>
					<td class="TableInput" width="75%" colspan="3">&nbsp;<asp:label id="Lblline" runat="server"></asp:label></td>
				</tr>
				<tr id="Lno" runat="server">
					<td class="tablecol" align="left" width="25%" colspan="1"><strong>&nbsp;Line</strong>
						:&nbsp;</td>
					<td class="TableInput" width="75%" colspan="3">&nbsp;<asp:label id="lblLineNo" runat="server" Visible="False"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" style="WIDTH: 186px" align="left" width="186" colspan="1"><strong>&nbsp;Item Code</strong> :&nbsp;</td>
					<td class="TableInput" width="70%" colspan="3">&nbsp;<asp:label id="lblVItemCode" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" style="WIDTH: 186px" align="left" width="186" colspan="1"><strong>&nbsp;Description</strong>
						:&nbsp;</td>
					<td class="TableInput" width="70%" colspan="3">&nbsp;<asp:label id="lblDesc" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" style="WIDTH: 186px" align="left" width="186" colspan="1"><strong>&nbsp;Remarks</strong>
						:&nbsp;</td>
					<td class="TableInput" width="70%" colspan="3">&nbsp;<asp:label id="lblRemarks" runat="server"></asp:label></td>
				</tr>
			</table>
			<hr/>
			<table class="alltable" id="tblPOLineList" cellspacing="0" cellpadding="0" width="100%"
				border="0">
				<tr>
					<td class="tablecol" align="left" width="25%"><strong>&nbsp;Currency</strong> :&nbsp;</td>
					<td class="TableInput" width="25%">&nbsp;<asp:label id="lblCurrency" runat="server"></asp:label></td>
					<td class="TableInput" colspan="2"></td>
				</tr>
				<tr>
					<td class="tablecol" align="left" width="25%"><strong>&nbsp;Quantity</strong> :&nbsp;</td>
					<td class="TableInput" width="25%">&nbsp;<asp:label id="lblQty" runat="server"></asp:label></td>
					<td class="tablecol" align="left" width="25%"><strong>&nbsp;Received</strong> :&nbsp;</td>
					<td class="TableInput" width="25%">&nbsp;<asp:label id="lblReceived" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" align="left"><strong>&nbsp;Unit Cost</strong> :&nbsp;</td>
					<td class="TableInput">&nbsp;<asp:label id="lblUnitCost" runat="server"></asp:label></td>
					<td class="tablecol"><strong>&nbsp;Rejected</strong> :&nbsp;</td>
					<td class="TableInput">&nbsp;<asp:label id="lblRejected" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" align="left"><strong>&nbsp;Total Cost</strong> :&nbsp;</td>
					<td class="TableInput">&nbsp;<asp:label id="lblTotCost" runat="server"></asp:label></td>
					<td class="tablecol" align="left"><strong>&nbsp;Total Received Cost</strong> :&nbsp;</td>
					<td class="TableInput">&nbsp;<asp:label id="lblTotRecCost" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" align="left"><strong>&nbsp;<asp:label id="lbl_GST" runat="server" Text="Tax"></asp:label></strong> :&nbsp;</td>
					<td class="TableInput">&nbsp;<asp:label id="lblGST" runat="server"></asp:label></td>
					<td class="TableInput" colspan="2"></td>
				</tr>
				<tr>
					<td class="tablecol" align="left"><strong>&nbsp;<asp:label id="lbl_TotCostGST" runat="server" Text="Total Cost w/Tax"></asp:label></strong> :&nbsp;</td>
					<td class="TableInput">&nbsp;<asp:label id="lblTotCostGST" runat="server"></asp:label></td>
					<td class="TableInput" colspan="2"></td>
				</tr>
				<tr>
					<td colspan="4">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">&nbsp;<strong><a id="back" onclick="history.back();" href="#" runat="server">&lt; 
								Back</a></strong></td>
				</tr>
				<tr>
					<td colspan="4">&nbsp;</td>
				</tr>
			</table>
		</form>
	</body>
</html>
