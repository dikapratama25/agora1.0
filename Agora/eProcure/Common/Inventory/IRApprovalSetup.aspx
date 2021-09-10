<%@ Page Language="vb" AutoEventWireup="false" Codebehind="IRApprovalSetup.aspx.vb" Inherits="eProcure.IRApprovalSetup" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>IR Approval Setup</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		
		<% Response.Write(Session("WheelScript"))%>
	</head>
	<body onload="">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_InventoryReq_tabs"))%>
			<table class="alltable" id="Table1" width="100%" cellspacing="0" cellpadding="0" border="0">
            <tr>
					<td class="linespacing1" colspan="2" ></td>
			</tr>
        		<tr valign="top">
					<td class="header" colspan="2" ><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></td>
				</tr>
			<tr>
					<td class="linespacing2" colspan="2" ></td>
            </tr>
				<tr>
					<td align="left" colspan="2" >
						<asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Please select the approval workflow from the approval list and submit the IR."></asp:label>
                        
					</td>
				</tr>
            <tr>
					<td class="linespacing2" colspan="6" ></td>
			</tr>
				<tr valign="top">
					<td class="TableHeader" colspan="2">&nbsp;Approval Setup Header</td>
				</tr>
				<tr valign="top">
					<td class="tablecol" align="left" width="20%" nowrap>&nbsp;<strong>IR Number</strong>&nbsp;:</td>
					<td class="TableInput" width="80%"><asp:label id="lblIR" runat="server" Width="290px"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tablecol" nowrap>&nbsp;<strong>Approval List</strong><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					<td class="TableInput"><asp:dropdownlist id="cboApproval" runat="server" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist>
						<asp:RequiredFieldValidator id="rfvApproval" runat="server" ErrorMessage="Approval List is required." ControlToValidate="cboApproval"
							Display="None"></asp:RequiredFieldValidator><asp:label id="lblMsg" runat="server" Width="100%"></asp:label></td>
				</tr>
				<tr class="emptycol">
					<td colspan="4"><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2">&nbsp;&nbsp;&nbsp;</td>
				</tr>
				<tr>
					<td colspan="2"><asp:datagrid id="dtgAO" runat="server" OnPageIndexChanged="dtgAO_Page" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="AGA_SEQ"  readonly="true"   HeaderText="Level">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="AGA_AO"  readonly="true"   HeaderText="Main AO"></asp:BoundColumn>
								<asp:BoundColumn DataField="AO_NAME"  readonly="true"   HeaderText="Main AO">
									<HeaderStyle Width="40%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="AGA_A_AO"  readonly="true"   HeaderText="Alternative AO"></asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME"  readonly="true"   HeaderText="Alternative AO">
									<HeaderStyle Width="40%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Type*">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label ID="lblType" Runat="server"></asp:Label>
										<asp:Label ID="lblRelief" Runat="server" Visible="False"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2">&nbsp;</td>
				</tr>
				<tr valign="top" runat="server" id="trConsolidator">
					<td class="tablecol" align="left" colspan="2">&nbsp;<strong>Consolidator</strong>&nbsp;:&nbsp;<asp:label id="lblConsolidator" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2" style="height: 19px">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2"><asp:label id="lblRemark" runat="server" Width="100%"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2">&nbsp;</td>
				</tr>
				<tr>
					<td colspan="2"><asp:button id="cmdSubmit" runat="server" CssClass="Button" Text="Submit"></asp:button></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary>
					</td>
				</tr>
				<tr>
					<td colspan="2">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="2"><asp:hyperlink id="lnkBack" Runat="server">
							<strong>&lt; Back</strong></asp:hyperlink></td>
				</tr>
			</table>
		</form>
	</body>
</html>
