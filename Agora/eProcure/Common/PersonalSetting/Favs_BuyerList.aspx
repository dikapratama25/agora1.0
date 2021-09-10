<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Favs_BuyerList.aspx.vb" Inherits="eProcure.Favs_BuyerList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Favs_BuyerList</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!--#include file = "../include/WheelScript.js"-->
		<script language="javascript">
		<!--		
		
		function selectAll()
		{
		SelectAllG("dtgBuyer_ctl02_chkAll","chkSelection");
		}
			
		function selectAllF()
		{
		SelectAllG("dtgFavs_ctl02_chkA","chkS");
		}
				
		function checkChild(id)
		{
		checkChildG(id,"dtgBuyer_ctl02_chkAll","chkSelection");
		}
		
		function checkC(id)
		{
		checkChildG(id,"dtgFavs_ctl02_chkA","chkS");
		}
		function DelExpItem(id)
		{
			var result = confirm('Are you sure that you want to permanently delete this item(s) ?');
						if (result == true){	
							document.Form1.hidIndex.value=id;
							return true;
						}
						else {
						    return false;
						}							
		}	
		
		function ResetSearch(){
				Form1.txtDesc.value = "";
				Form1.cboCatalogueFav.selectedIndex = 0;
				Form1.cboCatalogueBuyer.selectedIndex = 0;
				document.getElementById('txtVendorItem').value = "";
				document.getElementById('txtBuyerItem').value = "";
				document.getElementById('txtVendorName').value = "";
				document.getElementById('txtDesc').value = "";
		}		
		
		function reCheckAtLeastOne(pChkSelName,pButFunc)
		{
			var formValidate;		
			if (typeof(Page_Validators) == "undefined")
				formValidate = true;
			else	
				formValidate = Page_ClientValidate();	
				
			if (formValidate == true)
				return CheckAtLeastOne(pChkSelName,pButFunc);
			else
				return false;
		}
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Tab1" cellSpacing="0" cellPadding="0" width="100%">
				<TR>
					<TD class="header" colspan="3">Favourites / Buyer Catalogue Search</TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="3">
					</TD>
				</TR>
				<TR>
					<TD class="tableheader" colspan="3">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
					<TD class="TableCol" width="6%" nowrap><STRONG>&nbsp;Search From </STRONG>:</TD>
					<TD class="TableCol" colspan="2"><asp:radiobuttonlist id="rdSearch" runat="server" RepeatDirection="Horizontal" AutoPostBack="True">
							<asp:ListItem Value="F" Selected="True">Favourite List</asp:ListItem>
							<asp:ListItem Value="B">Buyer Catalogue</asp:ListItem>
						</asp:radiobuttonlist></TD>
				</TR>
				<TR>
					<TD class="TableCol" width="6%" nowrap><STRONG>&nbsp;Buyer catalogue </STRONG>:</TD>
					<TD class="TableCol" colspan="2"><asp:dropdownlist id="cboCatalogueBuyer" runat="server" CssClass="txtbox" Width="300px" AutoPostBack="True"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="TableCol" width="6%" nowrap><STRONG>&nbsp;Favourite List </STRONG>:</TD>
					<TD class="TableCol" colspan="2"><asp:dropdownlist id="cboCatalogueFav" runat="server" CssClass="txtbox" Width="300px" AutoPostBack="True"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="TableCol" width="6%" nowrap>&nbsp;<strong>Vendor Item Code </strong>:&nbsp;</TD>
					<TD class="TableCol" colSpan="2">
						<asp:TextBox id="txtVendorItem" runat="server" CssClass="txtbox"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD class="TableCol" noWrap width="6%">&nbsp;<strong>Buyer Item Code</strong> :</TD>
					<TD class="TableCol" colSpan="2">
						<asp:TextBox id="txtBuyerItem" runat="server" CssClass="txtbox"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD class="TableCol" noWrap width="6%">&nbsp;<strong>Vendor Name</strong> :</TD>
					<TD class="TableCol" colSpan="2">
						<asp:TextBox id="txtVendorName" runat="server" CssClass="txtbox"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD class="TableCol" width="6%" nowrap><STRONG>&nbsp;Item Description </STRONG>:</TD>
					<TD class="TableCol"><asp:TextBox id="txtDesc" runat="server" Width="300px" CssClass="txtbox"></asp:TextBox>&nbsp;
						<asp:Button id="cmdSearch" runat="server" CssClass="button" Text="Search" Enabled="False"></asp:Button>&nbsp;<INPUT class="button" id="cmd_Clear" onclick="ResetSearch();" type="button" value="Clear"
							name="cmd_Clear" runat="server" disabled></TD>
					<TD class="TableCol" nowrap>&nbsp;</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="3"></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="3">&nbsp;<U>Search Tips Examples:</U></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="3">&nbsp;1. To Search for item with item attribute 
						"paper", just key in "paper".</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="3">&nbsp;2. To Search for item which starts with 
						"paper", just key in "paper*".</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="3">&nbsp;3. To Search for item which ends with 
						"paper", just key in "*paper".</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="3"></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="3">&nbsp;<U>Legend:</U></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="3">&nbsp;LP :&nbsp;List Price 
						Item&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; CP : Contract Price 
						Item&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; DP : Discount Price Item</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="3">&nbsp;
						<asp:label id="Label1" runat="server" Width="24px" BackColor="#ff99cc"></asp:label>&nbsp;Contract 
						Price Item has Expired/Removed from the Contract Catalogue</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="3">&nbsp;
						<asp:label id="Label2" runat="server" Width="24px" BackColor="PaleTurquoise"></asp:label>&nbsp;Invalid 
						Vendor Status</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="3">&nbsp;
						<asp:label id="Label3" runat="server" Width="24px" BackColor="LightSteelBlue"></asp:label>
						&nbsp;Deactived Vendor</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colspan="3">
						<div id="Div_Buyer" style="DISPLAY: none" runat="server">&nbsp;
							<asp:datagrid id="dtgBuyer" runat="server" OnPageIndexChanged="MyDataGrid_Page" OnSortCommand="SortCommand_Click"
								AutoGenerateColumns="False">
								<Columns>
									<asp:TemplateColumn HeaderText="Delete">
										<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<HeaderTemplate>
											<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
										</HeaderTemplate>
										<ItemTemplate>
											<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Quantity">
										<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
										<ItemTemplate>
											<asp:TextBox id="txtQty" CssClass="numerictxtbox" Width="50px" Runat="server"></asp:TextBox>
											<asp:RegularExpressionValidator id="revQty" ValidationExpression="^\d+$" ControlToValidate="txtQty" Runat="server"></asp:RegularExpressionValidator>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="BCU_SOURCE" SortExpression="BCU_SOURCE"  readonly="true"    HeaderText="Type">
										<HeaderStyle Width="3%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_VENDOR_ITEM_CODE" SortExpression="PM_VENDOR_ITEM_CODE"  readonly="true"   
										HeaderText="Vendor Item Code">
										<HeaderStyle Width="8%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CBC_B_ITEM_CODE" SortExpression="CBC_B_ITEM_CODE"  readonly="true"   HeaderText="Buyer Item Code">
										<HeaderStyle Width="8%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" readonly="true"  HeaderText="Vendor Name">
										<HeaderStyle Width="22%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC"  readonly="true"  HeaderText="Item Description">
										<HeaderStyle Width="30%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM"  readonly="true"   HeaderText="UOM">
										<HeaderStyle Width="10%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_CURRENCY_CODE" SortExpression="PM_CURRENCY_CODE"  readonly="true"   HeaderText="Currency">
										<HeaderStyle Width="5%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_UNIT_COST" SortExpression="PM_UNIT_COST" readonly="true"   HeaderText="Price"
										DataFormatString=" {0:N4}">
										<HeaderStyle Width="9%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="BCU_PRODUCT_CODE" SortExpression="BCU_PRODUCT_CODE"  readonly="true"   >
										<HeaderStyle Width="60px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="CDM_END_DATE" SortExpression="CDM_END_DATE"  readonly="true"  >
										<HeaderStyle Width="60px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="PM_S_COY_ID"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="BCU_CD_GROUP_INDEX"></asp:BoundColumn>
									<asp:BoundColumn Visible="False">
										<HeaderStyle Width="30px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
							</asp:datagrid></div>
						<div id="Div_Fav" style="DISPLAY: none" runat="server">&nbsp;
							<asp:datagrid id="dtgFavs" runat="server" CssClass="Grid" OnPageIndexChanged="MyData_Page" OnSortCommand="SortCommandFav_Click"
								AutoGenerateColumns="False" AllowSorting="True">
								<Columns>
									<asp:TemplateColumn HeaderText="Delete">
										<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<HeaderTemplate>
											<asp:checkbox id="chkA" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
										</HeaderTemplate>
										<ItemTemplate>
											<asp:checkbox id="chkS" Runat="server"></asp:checkbox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Quantity">
										<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
										<ItemTemplate>
											<asp:TextBox id="txtQty2" CssClass="numerictxtbox" Width="50px" Runat="server"></asp:TextBox>
											<asp:RegularExpressionValidator id="revQty2" ValidationExpression="^\d+$" ControlToValidate="txtQty2" Runat="server"></asp:RegularExpressionValidator>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="FLI_SOURCE" SortExpression="FLI_SOURCE"  readonly="true"   HeaderText="Type">
										<HeaderStyle Width="3%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_VENDOR_ITEM_CODE" SortExpression="PM_VENDOR_ITEM_CODE"  readonly="true"  
										HeaderText="Vendor Item Code">
										<HeaderStyle Width="9%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CBC_B_ITEM_CODE" SortExpression="CBC_B_ITEM_CODE"  readonly="true"   HeaderText="Buyer Item Code">
										<HeaderStyle Width="9%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" readonly="true"  HeaderText="Vendor Name">
										<HeaderStyle Width="9%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC" readonly="true"   HeaderText="Item Description">
										<HeaderStyle Width="38%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM"  readonly="true"  HeaderText="UOM">
										<HeaderStyle Width="7%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_CURRENCY_CODE" SortExpression="PM_CURRENCY_CODE"  readonly="true"   HeaderText="Currency">
										<HeaderStyle Width="10%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_UNIT_COST" SortExpression="PM_UNIT_COST"  readonly="true"  HeaderText="Price"
										DataFormatString=" {0:N4}">
										<HeaderStyle Width="10%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="FLI_PRODUCT_CODE" SortExpression="FLI_PRODUCT_CODE"  readonly="true"  >
										<HeaderStyle Width="60px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="CDM_END_DATE" SortExpression="CDM_END_DATE"  readonly="true"   >
										<HeaderStyle Width="60px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="PM_S_COY_ID"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="FLI_CD_GROUP_INDEX"></asp:BoundColumn>
									<asp:BoundColumn Visible="False">
										<HeaderStyle Width="30px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
							</asp:datagrid></div>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="3"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="3"><asp:button id="cmd_Add" runat="server" CssClass="button" Width="120px" Text="Add To Shopping Cart"></asp:button>&nbsp;<asp:button id="cmd_Remove" runat="server" CssClass="button" Text="Remove"></asp:button>&nbsp;<asp:button id="cmd_RemoveAll" runat="server" CssClass="button" Width="168px" Text="Remove All Unavailable Item"></asp:button><INPUT id="hidIndex" style="WIDTH: 49px; HEIGHT: 22px" type="hidden" size="2" name="hidIndex"
							runat="server"><INPUT class="txtbox" id="hidDelete" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidDelete" runat="server"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
