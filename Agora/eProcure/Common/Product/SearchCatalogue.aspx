<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchCatalogue.aspx.vb" Inherits="eProcure.SearchCatalogue" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Catalogue Search</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		function selectAll()
		{
			SelectAllG("dtgProduct_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgProduct_ctl02_chkAll","chkSelection");
		}
		
		function Test(f)
		{
			alert(f.name);
		}				
		
		function PopWindow(myLoc)
		{
			//window.open(myLoc,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight: 255px; dialogWidth: 300px");
			window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
			return false;
		}
		
		function ResetSearch(){
				if (Form1.txtRole.value!="PM"){
					//Form1.cboAppVendor.selectedIndex=0;
					document.getElementById("cboAppVendor").style.display="none";
					document.getElementById("txtSearchVal").style.display="inline";
					Form1.txtSearchVal.value = "";}
				else
					Form1.txtSearchVal.value = "";
					
				Form1.cboSearchType.selectedIndex = 0;
		}
				
		function ResetAdv(){
				Form1.txtProdId.value = "";
				Form1.txtProdModel.value= "";
				Form1.txtProdDesc.value = "";
				if (Form1.txtRole.value!="PM")
					Form1.cboAppVendor1.selectedIndex=0;
				else
					Form1.txtVendor.value= "";				
				Form1.txtProdCat.value = "";
				Form1.txtVIC.value= "";
				Form1.txtProdBrand.value = "";
				Form1.txtBIC.value= "";
		}
		
		function Test()
		{
			if (document.getElementById("cboSearchType").selectedIndex==1)
				document.forms[0].submit();
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
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header"><FONT size="1">&nbsp;</FONT><asp:label id="lblTitle" Runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR id="divNormal" runat="server">
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Search By</STRONG> :
									<asp:dropdownlist id="cboSearchType" Runat="server" CssClass="ddl" Width="132px" AutoPostBack="True"></asp:dropdownlist>&nbsp;
									<asp:textbox id="txtSearchVal" runat="server" CssClass="txtbox" Width="211px"></asp:textbox><asp:dropdownlist id="cboAppVendor" Runat="server" CssClass="ddl" Width="211px"></asp:dropdownlist>
									<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
									<INPUT class="button" id="cmd_Clear" onclick="ResetSearch();" type="button" value="Clear">
									<asp:hyperlink id="lnkAdv" Runat="server" NavigateUrl="#">Click Here for advance search</asp:hyperlink></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR id="divAdv" runat="server">
					<TD class="emptycol">
						<TABLE class="alltable" id="Table5" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader" colSpan="4">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Item ID</STRONG> :</TD>
								<TD class="tablecol"><asp:textbox id="txtProdId" runat="server" CssClass="txtbox" Width="211px"></asp:textbox></TD>
								<TD class="tablecol">&nbsp;<STRONG>Item Model</STRONG> :</TD>
								<TD class="tablecol"><asp:textbox id="txtProdModel" runat="server" CssClass="txtbox" Width="211px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Item Description</STRONG> :</TD>
								<TD class="tablecol"><asp:textbox id="txtProdDesc" runat="server" CssClass="txtbox" Width="211px"></asp:textbox></TD>
								<TD class="tablecol">&nbsp;<STRONG>Vendor</STRONG> :</TD>
								<TD class="tablecol"><asp:textbox id="txtVendor" runat="server" CssClass="txtbox" Width="211px"></asp:textbox><asp:dropdownlist id="cboAppVendor1" Runat="server" CssClass="ddl" Width="211px"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Item Category</STRONG> :</TD>
								<TD class="tablecol"><asp:textbox id="txtProdCat" runat="server" CssClass="txtbox" Width="211px"></asp:textbox></TD>
								<TD class="tablecol">&nbsp;<STRONG>Vendor Item Code</STRONG> :</TD>
								<TD class="tablecol"><asp:textbox id="txtVIC" runat="server" CssClass="txtbox" Width="211px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Item Brand</STRONG> :</TD>
								<TD class="tablecol"><asp:textbox id="txtProdBrand" runat="server" CssClass="txtbox" Width="211px"></asp:textbox></TD>
								<TD class="tablecol">&nbsp;<STRONG>Buyer Item Code</STRONG> :</TD>
								<TD class="tablecol"><asp:textbox id="txtBIC" runat="server" CssClass="txtbox" Width="211px"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tablecol" colSpan="4"><asp:button id="cmdAdvSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
									<INPUT class="Button" id="cmd_Adv_Clear" onclick="ResetAdv();" type="button" value="Clear"
										name="Clear" runat="server">&nbsp;
									<asp:hyperlink id="lnkNormal" Runat="server" NavigateUrl="#">Click Here for normal search</asp:hyperlink></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;&nbsp;&nbsp;<u>Search Tips examples:</u></TD>
				</TR>
				<TR>
					<TD class="emptycol">1. To search for item with item attribute "paper", just key in 
						"paper"</TD>
				</TR>
				<TR>
					<TD class="emptycol">2. To search for item which starts with "paper", just key in 
						"paper*"</TD>
				</TR>
				<TR>
					<TD class="emptycol">3. To search for item which ends with "paper", just key in 
						"*paper"</TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="tblSearchResult" style="DISPLAY: none" width="100%" cellSpacing="0"
							cellPadding="0" border="0" runat="server">
							<TR>
								<TD class="tableheader" colSpan="5">&nbsp;Result of Search</TD>
							</TR>
							<tr align="center">
								<td><asp:hyperlink id="lnkCont" runat="server">[Contract Item]</asp:hyperlink></td>
								<td><asp:hyperlink id="lnkDist" runat="server">[Discount Item]</asp:hyperlink></td>
								<td><asp:hyperlink id="lnkList" runat="server">[List Item]</asp:hyperlink></td>
								<td><asp:hyperlink id="lnkAll" runat="server">[All]</asp:hyperlink></td>
								<td width="30%">&nbsp;</td>
							</tr>
							<tr align="center">
								<td><asp:label id="lblCont" Runat="server">(0 Items)</asp:label></td>
								<td><asp:label id="lblDist" Runat="server">(0 Items)</asp:label></td>
								<td><asp:label id="lblList" Runat="server">(0 Items)</asp:label></td>
								<td><asp:label id="lblAll" Runat="server">(0 Items)</asp:label></td>
								<td width="30%">&nbsp;</td>
							</tr>
							<TR>
								<TD class="emptycol" colSpan="5"></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="5">(-) indicates user not allowed to view/buy items 
									for that category.
								</TD>
							</TR>
							<TR id="trLegend" runat="server">
								<TD class="emptycol" colSpan="5"><STRONG>*</STRONG>&nbsp;<STRONG>CP</STRONG>&nbsp;&nbsp;&nbsp;Contract 
									Price Item &nbsp;&nbsp;&nbsp;<STRONG>DP</STRONG>&nbsp;&nbsp;&nbsp;Discount 
									Price Item &nbsp;&nbsp;&nbsp;<STRONG>LP</STRONG>&nbsp;&nbsp;&nbsp;List Price 
									Item
								</TD>
							</TR>
							<TR>
								<TD colSpan="5"><asp:datagrid id="dtgProduct" runat="server" DataKeyField="PM_PRODUCT_CODE" OnSortCommand="SortCommand_Click">
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
											<asp:BoundColumn DataField="CAT" SortExpression="CAT" HeaderText="Type*">
												<HeaderStyle Width="5%" VerticalAlign="Bottom"></HeaderStyle>
												<ItemStyle></ItemStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn  HeaderText="Quantity">
												<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
												<ItemTemplate>
													<asp:TextBox id="txtQty" CssClass="numerictxtbox" Width="50px" Runat="server"></asp:TextBox>
													<asp:RegularExpressionValidator id="revQty" ValidationExpression="^\d+$" ControlToValidate="txtQty" Runat="server"></asp:RegularExpressionValidator>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="PM_PRODUCT_CODE" HeaderText="Item&lt;BR&gt;ID">
												<HeaderStyle Width="7%" VerticalAlign="Bottom"></HeaderStyle>
												<ItemStyle></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkProductCode"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="CBC_B_ITEM_CODE" SortExpression="CBC_B_ITEM_CODE" HeaderText="Buyer&lt;BR&gt;Item Code">
												<HeaderStyle Width="7%" VerticalAlign="Bottom"></HeaderStyle>
												<ItemStyle></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor">
												<HeaderStyle Width="7%" VerticalAlign="Bottom"></HeaderStyle>
												<ItemStyle></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PM_VENDOR_ITEM_CODE" SortExpression="PM_VENDOR_ITEM_CODE" HeaderText="Vendor&lt;BR&gt;Item Code">
												<HeaderStyle Width="7%" VerticalAlign="Bottom"></HeaderStyle>
												<ItemStyle></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PM_PRODUCT_BRAND" SortExpression="PM_PRODUCT_BRAND" HeaderText="Item&lt;BR&gt;Brand">
												<HeaderStyle Width="7%" VerticalAlign="Bottom"></HeaderStyle>
												<ItemStyle></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PM_PRODUCT_MODEL" SortExpression="PM_PRODUCT_MODEL" HeaderText="Item&lt;BR&gt;Model">
												<HeaderStyle Width="7%" VerticalAlign="Bottom"></HeaderStyle>
												<ItemStyle></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CM_CATEGORY_DESC" SortExpression="CM_CATEGORY_DESC" HeaderText="Item&lt;BR&gt;Category">
												<HeaderStyle Width="15%" VerticalAlign="Bottom"></HeaderStyle>
												<ItemStyle></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC" HeaderText="Item Description">
												<HeaderStyle Width="20%" VerticalAlign="Bottom"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM" HeaderText="UOM">
												<HeaderStyle Width="5%" VerticalAlign="Bottom"></HeaderStyle>
												<ItemStyle></ItemStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn SortExpression="PM_UNIT_COST" HeaderText="Item&lt;BR&gt;Price">
												<HeaderStyle Width="7%" VerticalAlign="Bottom"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right" Width="70px"></ItemStyle>
												<ItemTemplate>
													<asp:Label Runat="server" ID="lblCost"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="PM_UNIT_COST" SortExpression="PM_UNIT_COST" HeaderText="Item&lt;BR&gt;Price"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="CM_COY_ID"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PM_CURRENCY_CODE"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="CDM_GROUP_INDEX"></asp:BoundColumn>
										</Columns>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="5"></TD>
							</TR>
							<TR id="tdBFHeader" runat="server">
								<TD class="tableheader" colSpan="5">&nbsp;Favourite List/Buyer Catalogue</TD>
							</TR>
							<TR id="tdFavList" runat="server">
								<TD class="tablecol" colSpan="5">&nbsp;<STRONG>Add Items to Favourite List</STRONG> 
									&nbsp;:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:dropdownlist id="cboFavourite" Runat="server" CssClass="ddl" Width="258px"></asp:dropdownlist>&nbsp;
									<asp:button id="cmdAddFav" runat="server" CssClass="button" Width="147px" Text="Add To Favourite List"></asp:button>&nbsp;
								</TD>
							</TR>
							<TR id="tdBuyerCat" runat="server">
								<TD class="tablecol" colSpan="5">&nbsp;<STRONG>Add Items to Buyer Catalogue</STRONG>
									&nbsp;:&nbsp;
									<asp:dropdownlist id="cboBuyCat" Runat="server" CssClass="ddl" Width="258px"></asp:dropdownlist>&nbsp;
									<asp:button id="cmdAddBuyC" runat="server" CssClass="button" Width="147px" Text="Add To Buyer Catalogue"></asp:button>&nbsp;
								</TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="5">&nbsp;&nbsp;</TD>
							</TR>
							<TR>
								<TD colSpan="5"><asp:button id="cmdAddSC" runat="server" CssClass="button" Width="147px" Text="Add To Shopping Cart"></asp:button>&nbsp;
									<INPUT class="button" id="cmdReset" style="DISPLAY: none" onclick="DeselectAllG('dtgProduct_ctl02_chkAll','chkSelection')"
										type="button" value="Reset" name="cmdReset" runat="server">&nbsp; <INPUT type="hidden" id="txtRole" runat="server"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
