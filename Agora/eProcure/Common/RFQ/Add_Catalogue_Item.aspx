<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Add_Catalogue_Item.aspx.vb" Inherits="eProcure.Add_Catalogue_Item" ValidateRequest="false" %>
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
			
			function UncheckFreeForm(){
			
				var oform = document.forms(0);
				oform.chkFreeForm.checked = false;
			}
			
			function ShowFreeForm(){
				var oform = document.forms(0);
				var div_cat = document.getElementById("hiddtg_cat");
				var div_freeform = document.getElementById("hiddtg_freeform");
				if (!oform.chkFreeForm.checked){
				div_cat.style.display ="";
				div_freeform.style.display ="none";
				oform.chkVCI.checked = true;
				}
				else
				{
				div_cat.style.display ="none";
				div_freeform.style.display ="";
				oform.chkVCI.checked = false;
				oform.chkPCI.checked = false;
				}
			}

			function check(){
			var change = document.getElementById("onchange");
			change.value ="1";
			}
			
			function ClearAll(page)
			{
			    ClearAllG("txt_qty");
			    if (page == "freeform")
			    {
			    
			    var vendor = document.getElementById("txt_vendor_com");
			    var item = document.getElementById("txt_item_desc");
			    var ddl = document.getElementById("txtCommodity");
			    var hid = document.getElementById("hidCommodity");
			    vendor.value = "";
			    item.value = "";
			    ddl.value = "";
			    hid.value = "";
			    
			    }
			    
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
					<TD class="tablecol">
						<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader" colspan="6">&nbsp;Search Criteria :</TD>
							</TR>
							<tr class="tablecol" width="100%">
					        <td class="tablecol" colspan="6" nowrap style="height: 20px;">
					            &nbsp;<asp:checkbox id="chkVCI" Text="Vendor Catalogue Item" Runat="server" Checked="True" autopostback="true"></asp:checkbox>
					            &nbsp;&nbsp;&nbsp;&nbsp;<asp:checkbox id="chkPCI" Text="Purchaser Catalogue Item" Runat="server" Checked="false"  autopostback="true"></asp:checkbox>
					            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:checkbox id="chkFreeForm" Text="Free Form" Runat="server" Checked="False" autopostback="true"></asp:checkbox>
					        </td>
							</tr>
							<TR class="tablecol" width="100%">
					            <TD class="tablecol" width="18%">
                                 <strong>&nbsp;<asp:Label ID="Label3" runat="server" Text="Commodity Type :"></asp:Label></strong></TD>
                                <TD class="tablecol" width="30%" ><asp:textbox id="txtCommodity" width="100%" runat="server" CssClass="txtbox"></asp:textbox><input type="hidden" id="hidCommodity" runat="server" /></TD>
			                    <td class="tablecol" width="2%"></td>
					            <TD class="tablecol" width="15%" >
                                    <strong>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Item Name :" CssClass="lbname"></asp:Label></strong></TD>
                                <TD class="tablecol" width="34%"><asp:textbox id="txt_item_desc" width="100%" runat="server" CssClass="txtbox" Height="20px" ></asp:textbox></TD>
			                    <td  class="tablecol" style="width: 29px"></td>
				            </TR>
							<TR class="tablecol" width="100%">
								<TD class="tablecol">&nbsp;<STRONG>Vendor Company</STRONG>:</td>
								<TD class="tablecol" ><asp:textbox id="txt_vendor_com" runat="server"  Width="100%" CssClass="txtbox"></asp:textbox><asp:dropdownlist id="ddl_vendor_com" runat="server" CssClass="ddl"></asp:dropdownlist></td>
			                    <td  class="tablecol"></td>
								<td class="tablecol" colspan="2" align="right">	
								    <asp:Button ID="cmd_freeformClear" runat="server" CssClass="button" Text="Clear" Visible="False"
                                        Width="59px" /><!--<Input Type= "Hidden" Name= "hidVendors" Value= "">-->
                                        <asp:button id="cmd_search" runat="server" CssClass="button" Width="63px" Text="Search"></asp:button>
									<asp:button id="cmd_clear" runat="server" CssClass="button" Width="59px" Text="Clear"></asp:button>
                                    </TD>
			                    <td  class="tablecol" style="width: 29px"></td>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR id=hiddtg_cat>
					<TD class="emptycol"><asp:datagrid id="dtg_Cat" runat="server" CssClass="grid" DataKeyField="PM_VENDOR_ITEM_CODE" AutoGenerateColumns="False"
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
										<asp:TextBox id="txt_qty" CssClass="txtbox"  Width="55px" Rows="2" Runat="server"></asp:TextBox>
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
				</TR>
				<TR id=hiddtg_freeform class="alltable" style="DISPLAY: none" >
					<TD class="emptycol"><asp:datagrid id="dtg_freeform" runat="server" CssClass="grid" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Item Description *">
									<HeaderStyle Width="50%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_desc" runat="server" CssClass="txtbox" Width="367px" TextMode="MultiLine"
											Height="40px" MaxLength="250" Rows="3"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqDesc" runat="server" ControlToValidate="txt_desc" Display="None" ></asp:RequiredFieldValidator>
										<asp:Label id="lbl_limit" runat="server"></asp:Label>
										<asp:Label id="lbl_desc" runat="server"></asp:Label>
									</ItemTemplate>
								    </asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="UOM ">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_uom" runat="server" CssClass="ddl"></asp:DropDownList>
										<asp:Label id="lbl_uom" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="QTY">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_qty" CssClass="numerictxtbox" Width="55px" Runat="server" Rows="2" ></asp:TextBox>
										<asp:RegularExpressionValidator id="val_qty" Runat="server" ControlToValidate="txt_qty" Display="Dynamic" ValidationExpression="\d{1,5}">></asp:RegularExpressionValidator>
						</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Delivery Lead Time (days)+ ">
									<HeaderStyle Width="19%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_delivery" runat="server" CssClass="numerictxtbox" Width="55px" MaxLength="3"></asp:TextBox>
										<asp:RegularExpressionValidator id="val_delivery" Display="none" Runat="server"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
						
				</TR>
				<TR>
					<TD class="emptycol"><STRONG>+0 denotes Ex-Stock.</STRONG></TD>
				</TR>
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
