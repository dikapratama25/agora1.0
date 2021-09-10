<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BuyerCatalogueSearch.aspx.vb" Inherits="eProcure.BuyerCatalogueSearchSEH" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Item Listing</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		 <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
		 <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher            
            Dim commodity As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Commodity")
        </script>
       
		<% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
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
				SelectAllG("dtgItem_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgItem_ctl02_chkAll","chkSelection");
			}
		
			function Reset(){
				var oform = document.forms(0);
				oform.txtItemCode.value="";
				oform.txtItemName.value="";
				form1.cboCatalogueBuyer.selectedIndex=0;
				form1.txtCommodity.value="";
				form1.txtVendorName.value="";
				form1.hidCommodity.value="";
				form1.chkSpot.checked=false;
				form1.chkStock.checked=false;
				form1.chkMRO.checked=false;
			}
			
		function PopWindow(myLoc)
		{
			//window.open(myLoc,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight: 255px; dialogWidth: 300px");
			window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
			return false;
		}

		-->
		</script>
		</head>
<body class="body">
    <form id="form1" method="post" runat="server" defaultbutton="cmdSearch">
			<table class="alltable" id="Tab1" cellspacing="0" cellpadding="0" width="100%">
 				<tr>
					<td class="header" colspan="3" style="padding:0">
					    <asp:label id ="lblHeader" runat="server" Font-Bold="True"  Text="Purchaser Catalogue Search"></asp:label>
</td>
				</tr>
				<tr>
					<td class="rowspacing">
					</td>
				</tr>
				<tr>
	                <td class="emptycol" colspan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Select/fill in the search criteria and click Search button to list the relevant Purchaser Catalogue. Click the Raise PO button to go to Raise PO page. Click the Raise RFQ button to go to Raise RFQ page."
		                ></asp:label>

	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colspan="4"></td>
			    </tr>
				<tr>
					<td class="tableheader" colspan="6">&nbsp;Search Criteria</td>
				</tr>
				<tr class="TableCol" width="100%">
					<td class="TableCol" width="20%">
                        <strong><asp:Label ID="Label4" runat="server" Text="Item Code :" CssClass="lbl"></asp:Label></strong></td>
                    <td class="TableCol" width="30%" >
                        <asp:TextBox ID="txtItemCode" Width="80%" runat="server" CssClass="txtbox"></asp:TextBox></td>
			       <td width="1%"></td>
					<td class="TableCol" width="15%" >
                       <strong><asp:Label ID="Label1" runat="server" Text="Item Name :" CssClass="lbl"></asp:Label></strong></td>
                    <td class="TableCol"><asp:textbox id="txtItemName" width="80%" runat="server" CssClass="txtbox"></asp:textbox></td>
			        <td class="tablecol" width="1%"></td>
				</tr>
				<tr width ="100%">
					<td class="TableCol" width="20%"><strong><asp:Label ID="Label2" runat="server" Text="Purchaser Catalogue :"></asp:Label></strong></td>
					<td class="TableCol" width="30%"><asp:dropdownlist id="cboCatalogueBuyer" runat="server" CssClass="ddl"  width="100%"  AutoPostBack="True"></asp:dropdownlist></td>
			        <td class="TableCol" width="1%"></td>
			        <td class="TableCol" width="15%"><strong><asp:Label ID="Label3" runat="server" Text="Commodity Type :"></asp:Label></strong></td>
			        <td class="TableCol" width="44%" ><asp:textbox id="txtCommodity" width="80%" runat="server" CssClass="txtbox"></asp:textbox><input type="hidden" id="hidCommodity" runat="server" /></td>
			        
			        <%--<td class="TableCol" colspan="2" width="34%"><asp:dropdownlist id="cboCommodityType" width="99%" runat="server" CssClass="ddl" ></asp:dropdownlist>
                    </td>--%>
			        <td  class="tablecol" width="1%"></td>
				</tr>
				<tr >
				    <td class="TableCol" width="20%"><strong><asp:Label ID="Label6" runat="server" Text="Vendor Name :"></asp:Label></strong></td>
					<td class="TableCol" width="30%"><asp:textbox id="txtVendorName" width="80%" runat="server" CssClass="txtbox"></asp:textbox></td>
			        <td class="TableCol" width="1%" colspan="4"></td>
				</tr>
                <tr width="100%">
                    <td class="TableCol" nowrap="nowrap" width="20%">
                        <strong><asp:Label ID="Label5" runat="server" Text=" Item Type :"></asp:Label></strong></td>
                    <td class="TableCol" width="30%">
                        <asp:CheckBox ID="chkSpot" runat="server" Text="Spot" />
                        <asp:CheckBox ID="chkStock" runat="server" Text="Stock" />
                        <asp:CheckBox ID="chkMRO" runat="server" Text="MRO" /></td>
                    <td class="TableCol" width="1%">
                    </td>
                    <td align="right" class="TableCol" colspan="2" width="48%">
                        <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button><input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/></td>
                    <td class="tablecol" width="1%">
                    </td>
                </tr>

				<tr>
					<td class="emptycol" colspan="6" ></td>
				</tr>
								<tr>
									<td class="emptycol" colspan="6"><asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Width="5%" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="PM_VENDOR_ITEM_CODE" HeaderText="Item Code" >
									<HeaderStyle ></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:BoundColumn DataField="PM_PRODUCT_CODE" Visible="false" HeaderText="Product Code"></asp:BoundColumn>
                                <asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC" HeaderText="Item Name"></asp:BoundColumn>
								<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM" HeaderText="UOM">
									<HeaderStyle Width="9%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_LAST_TXN_PRICE_CURR" SortExpression="PM_LAST_TXN_PRICE_CURR" HeaderText="Currency">
									<HeaderStyle Width="8%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_LAST_TXN_PRICE" SortExpression="PM_LAST_TXN_PRICE" HeaderText="Last Txn. Price ">
									<HeaderStyle Width="13%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Quantity">
									<HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtQty" CssClass="numerictxtbox" Width="60px" MaxLength="9" Runat="server"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
 								<asp:TemplateColumn HeaderText="VENDOR">
									<HeaderStyle Width="30%"></HeaderStyle>
								    <ItemTemplate>
										<asp:Label Runat="server" ID="lblVendor"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:BoundColumn DataField="PREFER" SortExpression="_" HeaderText="Prefer Vendor" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="1ST" SortExpression="_" HeaderText="1ST Vendor" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="2ND" SortExpression="_" HeaderText="2ND Vendor" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="3RD" SortExpression="_" HeaderText="3RD Vendor" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="PM_PREFER_S_COY_ID" SortExpression="_" HeaderText="Prefer Vendor" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="PM_1ST_S_COY_ID" SortExpression="_" HeaderText="1ST Vendor" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="PM_2ND_S_COY_ID" SortExpression="_" HeaderText="2ND Vendor" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="PM_3RD_S_COY_ID" SortExpression="_" HeaderText="3RD Vendor" Visible="False"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Specification">
									<HeaderStyle Width="15%"></HeaderStyle>
								    <ItemTemplate>
										<asp:Label Runat="server" ID="lblSpec"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PM_SPEC1" SortExpression="_" HeaderText="1ST Spec" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="PM_SPEC2" SortExpression="_" HeaderText="2ND Spec" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="PM_SPEC3" SortExpression="_" HeaderText="3RD Spec" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="PM_OVERSEA" SortExpression="_" HeaderText="Oversea" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="PM_ITEM_TYPE" SortExpression="_" HeaderText="Item Type" Visible="False"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
						</tr> 
						<tr>
					        <td class="emptycol" colspan="6"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary></td>
				        </tr>
				        <tr>
					<td class="emptycol" colspan="6"></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="6">
						<asp:button id="cmdPO" runat="server" CssClass="Button" Text="Raise PO" Visible="false"></asp:button>
						<asp:button id="cmdRFQ" runat="server" CssClass="Button" Text="Raise RFQ" Visible="false"></asp:button>
</td>
				</tr>
			</table>

    </form>
</body>
</html>
