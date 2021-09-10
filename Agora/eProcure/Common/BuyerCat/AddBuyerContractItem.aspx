<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AddBuyerContractItem.aspx.vb" Inherits="eProcure.AddBuyerContractItem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>AddBuyerContractItem</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
	<%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		function selectAll()
		{
			SelectAllG("dtgItem_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgItem_ctl02_chkAll","chkSelection");
		}
		
		function Reset()
		{
			ValidatorReset();
			Form1.cboVendor.selectedIndex = 0;
			Form1.cboContract.selectedIndex = 0;
		}
		
		function enableCboContract(i)
		{
			if (parseInt(i) == 1) {
				Form1.cboContract.disabled = true;
			}
			else {
				Form1.cboContract.disabled = false;
			}
		}
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header" colSpan="2">
						<asp:Label id="lblTitle" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="tableheader" colSpan="2">&nbsp;<asp:label id="lblHeader" runat="server">Buyer Catalogue Header</asp:label>
					</TD>
				</TR>
				<TR>
					<TD class="TableCol" width="15%"><STRONG>&nbsp;Type </STRONG>:</TD>
					<TD class="TableCol" width="85%"><asp:radiobutton id="rdAll" runat="server" GroupName="Type" Text="Add All Contract Item" AutoPostBack="True"></asp:radiobutton>&nbsp;
						<asp:radiobutton id="rdSelected" runat="server" GroupName="Type" Text="Add Selected Contract Item"
							AutoPostBack="True"></asp:radiobutton></TD>
				</TR>
				<TR>
					<TD class="TableCol" noWrap><STRONG>&nbsp;Vendor Name<asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>
						</STRONG>:</TD>
					<TD class="TableCol"><asp:dropdownlist id="cboVendor" runat="server" AutoPostBack="True" CssClass="ddl"></asp:dropdownlist>
						<asp:RequiredFieldValidator id="rfvVendor" runat="server" ErrorMessage="Vendor Name is required." ControlToValidate="cboVendor"
							Display="None"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR>
					<TD class="TableCol" noWrap><STRONG>&nbsp;Contract<asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>
						</STRONG>:</TD>
					<TD class="TableCol"><asp:dropdownlist id="cboContract" runat="server" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist>
						<asp:RequiredFieldValidator id="rfvContract" runat="server" ErrorMessage="Contract is required." ControlToValidate="cboContract"
							Display="None"></asp:RequiredFieldValidator></TD>
				</TR>
				<TR style="DISPLAY: none">
					<TD class="TableCol" colSpan="2"><asp:button id="cmdSearch" runat="server" Text="Search" CssClass="button" Visible="False"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"></TD>
				</TR>
				<TR>
					<TD class="emptyCol" colSpan="2">&nbsp;<asp:ValidationSummary id="vldSummary" runat="server"></asp:ValidationSummary></TD>
				</TR>
				<TR>
					<TD class="emptyCol" colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"><asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Vendor_Item_Code" SortExpression="Vendor_Item_Code"  readonly="true"    headerText="Vendor Item Code">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDI_PRODUCT_DESC" SortExpression="CDI_PRODUCT_DESC"  readonly="true"    HeaderText="Item Description">
									<HeaderStyle Width="35%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDI_UOM" SortExpression="CDI_UOM"  readonly="true"    HeaderText="UOM">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDI_CURRENCY_CODE" SortExpression="CDI_CURRENCY_CODE"  readonly="true"  
									HeaderText="Currency">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SC_Unit_Cost" SortExpression="SC_Unit_Cost"  readonly="true"   HeaderText="Price"
									DataFormatString="{0:N4}">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Product_Code" SortExpression="Product_Code" Visible="False"></asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2">&nbsp;</TD>
				</TR>
				<tr>
					<td colSpan="2"><asp:button id="cmdSave" runat="server" Text="Save" CssClass="button" CausesValidation="False"></asp:button>&nbsp;<INPUT class="button" id="cmd_Reset" onclick="DeselectAllG('dtgItem_ctl02_chkAll','chkSelection')"
							type="button" value="Reset" name="cmd_Reset" runat="server">
					</td>
				</tr>
				<TR>
					<TD class="emptycol" colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"><asp:hyperlink id="lnkBack" Runat="server"><STRONG>&lt; 
								Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
