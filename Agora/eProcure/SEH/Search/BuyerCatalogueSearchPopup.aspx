<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BuyerCatalogueSearchPopup.aspx.vb" Inherits="eProcure.BuyerCatalogueSearchPopupSEH" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Add Item</title>
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
				oform.txtItemName.value="";
				form1.cboCatalogueBuyer.selectedIndex=0;
				form1.txtCommodity.value="";
				form1.hidCommodity.value="";
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
<body class="body" ms_positioning="GridLayout">
    <form id="form1" method="post" runat="server" defaultbutton="cmdSearch">
            <input id="hidVendor" type="hidden" name="hidVendor" runat="server" />
            <input id="hidVendorCode" type="hidden" name="hidVendorCode" runat="server" />
            <input id="hidSingleVendor" type="hidden" name="hidSingleVendor" runat="server" />
            <input id="hidItemType" type="hidden" name="hidItemType" runat="server" />
            <input id="hidOversea" type="hidden" name="hidOversea" runat="server" />
			<table class="alltable" id="Tab1" cellspacing="0" cellpadding="0" width="100%">
 				<tr>
					<td class="header" style="padding:0" colspan="3">
					    <asp:label id ="lblHeader" runat="server" Font-Bold="True"  Text="Purchaser Catalogue Search"></asp:label>
                </td>
				</tr>
				<tr>
					<td class="emptycol" colspan="3">
					</td>
				</tr>
				<tr>
					<td class="tableheader" colspan="6">&nbsp;Search Criteria</td>
				</tr>
				<tr class="tablecol" width="100%">
					<td class="tablecol" width="20%">
                        <strong>&nbsp;<asp:Label ID="Label3" runat="server" Text="Commodity Type :"></asp:Label></strong></td>
                    <td class="Tablecol" width="30%" ><asp:textbox id="txtCommodity" width="100%" runat="server" CssClass="txtbox"></asp:textbox><input type="hidden" id="hidCommodity" runat="server" /></td>
			        <td class="tablecol" width="1%"></td>
					<td class="tablecol" width="15%" >
                       <strong>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Item Name :" CssClass="lbname"></asp:Label></strong></td>
                    <td class="Tablecol" width="34%"><asp:textbox id="txtItemName" width="100%" runat="server" CssClass="txtbox" Height="20px" ></asp:textbox></td>
			        <td  class="tablecol" width="1%"></td>
				</tr>
				<tr>
					<td class="TableCol" nowrap><strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="Purchaser Catalogue :"></asp:Label></strong></td>
					<td class="TableCol" ><asp:dropdownlist id="cboCatalogueBuyer" runat="server" CssClass="txtbox"  width="99%"  AutoPostBack="True"></asp:dropdownlist></td>
			        <td class="TableCol" ></td>
			        <td class="TableCol" align="right" colspan="2"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
						<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/></td>
			        <td  class="tablecol" ></td>
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
							</Columns>
						</asp:datagrid></td>
				<tr>
					<td class="emptycol" colspan="6">
						<asp:button id="cmdSave" runat="server" CssClass="Button" Text="Save" Visible="false"></asp:button>
					    <input type="button" name="cmd_back" value="Close" onclick="window.close(); " id="cmd_back" class="Button" />
                    </td>
				</tr>				
			</table>

    </form>
</body>
</html>
