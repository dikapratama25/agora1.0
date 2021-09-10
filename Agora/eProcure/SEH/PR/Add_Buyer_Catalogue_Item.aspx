<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Add_Buyer_Catalogue_Item.aspx.vb" Inherits="eProcure.Add_Buyer_Catalogue_ItemSEH" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Add_Catalogue_Item_</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		 <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
         <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher            
            Dim commodity As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Commodity")
        </script>
		<% Response.Write(Session("WheelScript"))%>
		<% Response.write(Session("typeahead")) %>
		
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
				SelectAllG("dtg_Cat_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtg_Cat_ctl02_chkAll","chkSelection");
			}
			

			function check(){
			var change = document.getElementById("onchange");
			change.value ="1";
			}
						
			function PopWindow(myLoc)
		    {
			    window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
			    return false;
		    }
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header">Add Item</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="height: 3px" ></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader" colspan="6">&nbsp;Search Criteria :</TD>
							</TR>
							<tr class="tablecol" width="100%">
					        <td class="TableCol" colspan="6" style="height: 10px;" rowspan="">
					            <asp:radiobuttonlist ID="rd1" CssClass="rbtn"  RepeatDirection="Horizontal" runat="server" AutoPostBack="true"> 
                                    <asp:ListItem Value="CAT" Selected="True">Buyer Catalogue Item</asp:ListItem>
										<asp:ListItem Value="FREE">Free Form</asp:ListItem>
									</asp:radiobuttonlist></td>
							</tr>
							<TR class="tablecol" width="100%">
					            <TD class="tablecol" width="18%">
                                 <strong><asp:Label ID="Label3" runat="server" Text="Item Code :"></asp:Label></strong></TD>
                                <TD class="tablecol" width="30%" >
                                    <asp:TextBox ID="txtItemCode" runat="server" CssClass="txtbox" Height="20px" Width="100%"></asp:TextBox></TD>
			                    <td class="tablecol" width="2%"></td>
					            <TD class="tablecol" width="15%" >
                                    <strong><asp:Label ID="Label1" runat="server" Text="Item Name :" CssClass="lbname"></asp:Label></strong></TD>
                                <TD class="tablecol" width="34%"><asp:textbox id="txt_item_desc" width="100%" runat="server" CssClass="txtbox" Height="20px" ></asp:textbox></TD>
			                    <td  class="tablecol" style="width: 29px"></td>
				            </TR>
							<TR class="tablecol" width="100%">
								<TD class="TableCol" nowrap width="20%"><STRONG><asp:Label ID="Label4" runat="server" Text="Buyer Catalogue :"></asp:Label></STRONG></TD>
								<TD class="TableCol" width="30%"><asp:dropdownlist id="cboCatalogueBuyer" runat="server" CssClass="ddl"  width="99%"  AutoPostBack="True"></asp:dropdownlist></TD>
			                    <td  class="tablecol"></td>
								<%--<td colspan="2" align="right">	
								    <asp:Button ID="cmd_freeformClear" runat="server" CssClass="button" Text="Clear" Visible="False"
                                        Width="59px" /><!--<Input Type= "Hidden" Name= "hidVendors" Value= "">-->
                                        <asp:button id="cmd_search" runat="server" CssClass="button" Width="63px" Text="Search"></asp:button>
									<asp:button id="cmd_clear" runat="server" CssClass="button" Width="59px" Text="Clear"></asp:button>
                                    </TD>--%>
                                <TD class="tablecol" width="18%"><strong><asp:Label ID="Label2" runat="server" Text="Commodity Type :"></asp:Label></strong></TD>
                                <TD class="tablecol" >
                                   <asp:textbox id="txtCommodity" width="100%" runat="server" CssClass="txtbox"></asp:textbox><input type="hidden" id="hidCommodity" runat="server" /></td>
			                    <td  class="tablecol" style="width: 29px"></td>
							</TR>
							<TR class="tablecol" width="100%">
								<TD class="TableCol" nowrap width="20%"><STRONG><asp:Label ID="Label5" runat="server" Text="Item Type :"></asp:Label></STRONG></TD>
								<TD class="tablecol">
					                <asp:checkbox id="chkSpot" Text="Spot" Runat="server" Checked="false"></asp:checkbox>&nbsp;
					                <asp:checkbox id="chkStock" Text="Stock" Runat="server" Checked="false"></asp:checkbox>&nbsp;
					                <asp:checkbox id="chkMRO" Text="MRO" Runat="server" Checked="false"></asp:checkbox>
					            </TD>
			                    <td class="tablecol" class="tablecol" style="height: 24px"></td>
								<td class="tablecol" colspan="2" align="right" style="height: 24px">	
								    <asp:Button ID="cmd_freeformClear" runat="server" style="display:none" CssClass="button" Text="Clear" Visible="False"
                                        Width="59px" /><!--<Input Type= "Hidden" Name= "hidVendors" Value= "">-->
                                        <asp:button id="cmd_search" runat="server" CssClass="button" Width="63px" Text="Search"></asp:button>
									<asp:button id="cmd_clear" runat="server" CssClass="button" Width="59px" Text="Clear"></asp:button>
                                    </TD>
			                    <td  class="tablecol" style="width: 29px; height: 24px;"></td>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				
				<tr id="hiddtg_cat" runat="server">
					<TD class="emptycol">
					    <asp:datagrid id="dtg_Cat" runat="server" CssClass="grid" DataKeyField="PM_VENDOR_ITEM_CODE" AutoGenerateColumns="False"
							OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_cat_Page">
							<Columns>
							    <asp:TemplateColumn HeaderText="Delete">
								    <HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Center"></ItemStyle>
								    <HeaderTemplate>
									    <asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
								    </HeaderTemplate>
								    <ItemTemplate>
									    <asp:checkbox id="chkSelection" Width="5%" Runat="server"></asp:checkbox>
								    </ItemTemplate>
							    </asp:TemplateColumn>
							    <asp:TemplateColumn SortExpression="PM_VENDOR_ITEM_CODE" HeaderText="Item Code" >
								    <HeaderStyle Width="13%" HorizontalAlign="Left"></HeaderStyle>
								    <ItemTemplate>
									    <asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
								    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
							    </asp:TemplateColumn>
                                <asp:BoundColumn DataField="PM_PRODUCT_CODE" Visible="False" HeaderText="Product Code">
                                    <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC" HeaderText="Item Name">
                                    <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
							    <asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM" HeaderText="UOM">
								    <HeaderStyle Width="8%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
							    </asp:BoundColumn>
							    <asp:BoundColumn DataField="PM_LAST_TXN_PRICE_CURR" SortExpression="PM_LAST_TXN_PRICE_CURR" HeaderText="Currency">
								    <HeaderStyle Width="9%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
							    </asp:BoundColumn>
							    <asp:BoundColumn DataField="PM_LAST_TXN_PRICE" SortExpression="PM_LAST_TXN_PRICE" HeaderText="Last Txn. Price ">
								    <HeaderStyle Width="11%" HorizontalAlign="Right"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Right"></ItemStyle>
							    </asp:BoundColumn>
							    <asp:BoundColumn DataField="PM_LAST_TXN_TAX" SortExpression="PM_LAST_TXN_TAX" HeaderText="Last Txn. Tax(%)">
								    <HeaderStyle Width="10%" HorizontalAlign="Right" ></HeaderStyle>
								    <ItemStyle HorizontalAlign="Right"></ItemStyle>
							    </asp:BoundColumn>
								<asp:BoundColumn DataField="pm_last_txn_s_coy_id" SortExpression="pm_last_txn_s_coy_id" HeaderText="Last Txn. Vendor ">
								    <HeaderStyle Width="18%" HorizontalAlign="Left" ></HeaderStyle>
								    <ItemStyle HorizontalAlign="Left"></ItemStyle>
							    </asp:BoundColumn>
							    <asp:TemplateColumn SortExpression="PM_ITEM_TYPE" HeaderText="Item Type" >
								    <HeaderStyle Width="18%" HorizontalAlign="Left"></HeaderStyle>
								    <ItemTemplate>
									    <asp:Label id="lblItemType" runat="server"></asp:Label>
								    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
							    </asp:TemplateColumn>
                            </Columns>
						</asp:datagrid>
					</td>
				</tr>
				
				<%--<TR id=hiddtg_cat>
					<TD><asp:datagrid id="dtg_Cat" runat="server" CssClass="grid" DataKeyField="PM_VENDOR_ITEM_CODE" AutoGenerateColumns="False"
							OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_cat_Page">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelecffffftAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC"  readonly="true"    HeaderText="Item Name">
									<HeaderStyle HorizontalAlign="Left" Width="28%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_UNIT_COST" SortExpression="PM_UNIT_COST" HeaderText="List Price">
									<HeaderStyle HorizontalAlign="Right" Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor">
									<HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="QTY">
									<HeaderStyle HorizontalAlign="Right" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_qty" CssClass="txtbox"  Width="55px" MaxLength="5" Rows="2" Runat="server"></asp:TextBox>
										<asp:RegularExpressionValidator id="rvl_qty" runat="server"></asp:RegularExpressionValidator>
										<asp:Label id="lbl_alert" runat="server" ForeColor="Red" Visible="False"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Delivery Lead Time (days)+">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_delivery" runat="server" CssClass="numerictxtbox" Width="60px" MaxLength="3"></asp:TextBox>
										<asp:RegularExpressionValidator id="rvl_delivery_time" runat="server" Display="Dynamic"></asp:RegularExpressionValidator>
									</ItemTemplate>
								    </asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="PM_PRODUCT_CODE"  readonly="true"    HeaderText="Product_Id ">
									<HeaderStyle HorizontalAlign="Left" Width="7px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CM_COY_ID"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="PM_VENDOR_ITEM_CODE"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>--%>
				<TR id="hiddtg_freeform" runat="server" class="alltable" style="display:none">
					<TD class="emptycol"><asp:datagrid id="dtg_freeform" runat="server" CssClass="grid" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Item Name *">
									<HeaderStyle Width="50%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_desc" runat="server" CssClass="txtbox" Width="350px" TextMode="MultiLine"
											Height="40px" MaxLength="250" Rows="3"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="reqDesc" runat="server" ControlToValidate="txt_desc" Display="None" ></asp:RequiredFieldValidator>--%>
										<asp:Label id="lbl_limit" runat="server"></asp:Label>
										<asp:Label id="lbl_desc" runat="server"></asp:Label>
									</ItemTemplate>
								    </asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="UOM *">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_uom" style="width:100px" runat="server" CssClass="ddl"></asp:DropDownList>
										<asp:Label id="lbl_uom" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Commodity Type">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemTemplate>
										<%--<asp:DropDownList id="ddl_comm" style="width:200px" runat="server" CssClass="ddl"></asp:DropDownList>--%>
										<asp:textbox id="txtCommodityFree" style="width:150px" runat="server" CssClass="txtbox"></asp:textbox><input type="hidden" id="hidCommodityFree" runat="server" /></td>
										<asp:Label id="lbl_comm" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Item Type">
								    <HeaderStyle Width="9%"></HeaderStyle>
								    <ItemTemplate>
								        <asp:DropDownList id="ddl_stock" runat="server" CssClass="ddl" Width="90px">
								            <asp:ListItem Selected="True" Value="">--- Select ---</asp:ListItem>
								            <asp:ListItem Value="SP">Spot</asp:ListItem>
                                            <asp:ListItem Value="ST">Stock</asp:ListItem>
                                            <asp:ListItem Value="MI">MRO</asp:ListItem>
								        </asp:DropDownList>
								    </ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
						
				</TR>
				<%--<TR>
					<TD><STRONG>+0 denotes Ex-Stock.</STRONG></TD>
				</TR>--%>
				<TR>
					<TD class="emptycol" style="HEIGHT: 19px"><asp:button id="cmd_Save" runat="server" CssClass="button" Text="Save" ></asp:button>&nbsp;
                        <asp:Button ID="cmd_back" runat="server" CssClass="button" Text="Close" />
					</TD>
				</TR>
				<tr>
					<td class="emptycol"><asp:validationsummary id="ValidationSummary1" runat="server" CssClass="errormsg" Height="24px"></asp:validationsummary><asp:label id="lbl_check" runat="server" ForeColor="Red" CssClass="errormsg"></asp:label></td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
