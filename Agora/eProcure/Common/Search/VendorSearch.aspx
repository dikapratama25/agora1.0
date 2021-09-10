<%@ Page Language="vb" AutoEventWireup="false" Codebehind="VendorSearch.aspx.vb" Inherits="eProcure.VendorSearch" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Vendor Catalogue Search</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		 <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
		 <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher         
            Dim commodity As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Commodity")
        </script>
      
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		 $(document).ready(function(){
        
            $("#txtCommodity").autocomplete("<% Response.write(commodity) %>", {
            width: 200,
            scroll: true,
            selectFirst: false
            });
            $("#txtCommodity").result(function(event, data, formatted) {
            if (data)
            $("#hidCommodity").val(data[1]);
            });
            $("#txtCommodity").blur(function() {
            var hidcommodity = document.getElementById("hidCommodity").value;                        
            if(hidcommodity == "")
            {
                $("#txtCommodity").val("");
            }
            });   
            });
             
		function selectAll()
		{
		    if (document.getElementById("dtgProduct")==null)
		    {
			SelectAllG("dtgProductFTN_ctl02_chkAll","chkSelection");}
			else {
			SelectAllG("dtgProduct_ctl02_chkAll","chkSelection");
			}
		}
				
		function checkChild(id)
		{
		    if (document.getElementById("dtgProduct")==null)
		    {
			checkChildG(id,"dtgProductFTN_ctl02_chkAll","chkSelection");}
			else {
			checkChildG(id,"dtgProduct_ctl02_chkAll","chkSelection");
			}
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
		
			
		function ResetAdv(){
				//Form1.txtProdId.value = "";
				Form1.txtProdModel.value= "";
				Form1.txtProdDesc.value = "";
				if (Form1.txtRole.value!="PM")
					Form1.cboAppVendor1.selectedIndex=0;
				else
					Form1.txtVendor.value= "";				
				//Form1.txtProdCat.value = "";
				//Form1.txtVIC.value= "";
				Form1.txtProdBrand.value = "";
				//Form1.txtBIC.value= "";
				Form1.txtCommodity.value = "";
				Form1.hidCommodity.value = "";
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
					<TD class="header" style="padding:0" colspan="3"><FONT size="1">&nbsp;</FONT><asp:label id="lblTitle" Runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="rowspacing">
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text="Select/fill in the search criteria and click Search button to list the relevant Vendor Catalogue. Click the Raise RFQ button to go to Raise RFQ page."
						></asp:label>

					</TD>
				</TR>
				<tr>
					<TD class="linespacing2" colSpan="4"></TD>
			    </TR>
				<TR id="divAdv" runat="server">
					<TD class="emptycol">
						<TABLE class="alltable" id="Table5" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader" colSpan="5">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
					            <TD class="tablecol" Width="15%" ><strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="Commodity Type :"></asp:Label></strong></TD>
						        <TD class="tablecol" Width="35%" ><asp:textbox id="txtCommodity" width="100%" runat="server" CssClass="txtbox"></asp:textbox><input type="hidden" id="hidCommodity" runat="server" /><%--<asp:dropdownlist id="cboCommodityTypeAdv"  Width="100%" runat="server" CssClass="ddl" ></asp:dropdownlist>--%></TD>
								<TD class="tablecol" width="1%"></TD>
								<TD class="tablecol" width="15%">&nbsp;<STRONG>Item Name</STRONG> :</TD>
								<TD class="tablecol" width="34%" ><asp:textbox id="txtProdDesc"  Width="100%" runat="server" CssClass="txtbox" ></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Item Brand</STRONG> :</TD>
								<TD class="tablecol"><asp:textbox id="txtProdBrand" Width="100%" runat="server" CssClass="txtbox" ></asp:textbox></TD>
								<TD class="tablecol" ></TD>
								<TD class="tablecol">&nbsp;<STRONG>Item Model</STRONG> :</TD>
								<TD class="tablecol"><asp:textbox id="txtProdModel" Width="100%" runat="server" CssClass="txtbox" ></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Vendor</STRONG> :</TD>
								<TD class="tablecol" ><asp:textbox id="txtVendor" Width="100%" runat="server" CssClass="txtbox" >
								</asp:textbox><asp:dropdownlist id="cboAppVendor1" Width="100%" Runat="server" CssClass="ddl" visible="false"></asp:dropdownlist></TD>
								<TD class="tablecol" ></TD>
								<TD class="tablecol" colSpan="2" align="right"><asp:button id="cmdAdvSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
									<INPUT class="Button" id="cmd_Adv_Clear" onclick="ResetAdv();" type="button" value="Clear"
										name="Clear" runat="server">&nbsp;
									</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
							<TR id="divProduct" runat=server>
							<TD class="emptycol">
						<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0">				<TR>
								<TD class="emptycol" colSpan="5"><asp:datagrid id="dtgProduct" runat="server" DataKeyField="PM_PRODUCT_CODE" OnSortCommand="SortCommand_Click">
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
											<asp:BoundColumn DataField="CT_NAME" SortExpression="CT_NAME" HeaderText="Item&lt;BR&gt;Category">
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
							<TR>
						</TABLE>
						</TD>
				</TR>
							<TR id="divProductFTN" runat=server>
							<TD class="emptycol">
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">				
						<TR>
								<TD class="emptycol" colSpan="6"><asp:datagrid id="dtgProductFTN" runat="server" DataKeyField="PM_PRODUCT_CODE" OnSortCommand="SortCommandFTN_Click">
										<Columns>
											<asp:TemplateColumn HeaderText="Delete" >
												<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<HeaderTemplate>
													<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
												</HeaderTemplate>
												<ItemTemplate>
													<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="PM_PRODUCT_CODE" HeaderText="PM_PRODUCT_CODE" visible="false">
											</asp:BoundColumn>
											<asp:TemplateColumn SortExpression="PM_PRODUCT_DESC" HeaderText="Item Name">
												<HeaderStyle Width="25%" VerticalAlign="Bottom"></HeaderStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkProdDescFTN"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor">
												<HeaderStyle Width="25%" VerticalAlign="Bottom"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CT_NAME" SortExpression="CT_NAME" HeaderText="Commodity Type">
												<HeaderStyle Width="25%" VerticalAlign="Bottom"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM" HeaderText="UOM">
												<HeaderStyle width="15%" VerticalAlign="Bottom"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn HeaderText="Currency" DataField="PM_CURRENCY_CODE" SortExpression="PM_CURRENCY_CODE" >
											    <HeaderStyle VerticalAlign="Bottom" ></HeaderStyle></asp:BoundColumn>
											<asp:TemplateColumn SortExpression="PM_UNIT_COST" HeaderText="Price">
												<HeaderStyle Width="8%" VerticalAlign="Bottom" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
												<ItemTemplate>
													<asp:Label Runat="server" ID="lblCost"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="PM_UNIT_COST" SortExpression="PM_UNIT_COST" HeaderText="Item&lt;BR&gt;Price"></asp:BoundColumn>
							</Columns>
									</asp:datagrid></TD>
							</TR>
							<TR>
					<td class="emptycol" colspan="6">
						<asp:button id="cmdRFQ" runat="server"  visible="false" CssClass="Button" Text="Raise RFQ"></asp:button>
						 <INPUT type="hidden" id="txtRole" runat="server">
</td>
							</TR>
						</TABLE>
						</TD>
				</TR>
				
			</TABLE>
		</form>
	</body>
</HTML>
