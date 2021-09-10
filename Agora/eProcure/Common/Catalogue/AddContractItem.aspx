<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AddContractItem.aspx.vb" Inherits="eProcure.AddContractItem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Add Contract Item</title>
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
            
			function cmdAddClick()
			{
				var result = confirm("Save PR?", "Yes", "No");
				if(result == true)
					Form1.hidAddItem.value = "1";
				else 
					Form1.hidAddItem.value = "0";
			}
			
			function selectAll()
			{
				SelectAllG("dtgCatalogue_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCatalogue_ctl02_chkAll","chkSelection");
			}
			
			function Reset(){
				var oform = document.forms(0);
				oform.txtVendorItemCode.value="";
				oform.txtDesc.value="";
				Form1.txtCommodity.value="";
				Form1.hidCommodity.value="";
				if( document.getElementById('chkSpot') ){
				Form1.chkSpot.checked=false;}
				if( document.getElementById('chkStock') ){
				Form1.chkStock.checked=false;}
				if( document.getElementById('chkMRO') ){
				Form1.chkMRO.checked=false;}
			}

		-->
		</script>
	</head>
	<body class="body" ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="header" colspan="5"><asp:label id="lblTitle" runat="server" Text="Assign Items"></asp:label></td>
				</tr>	
				<tr><td class="rowspacing" colspan="5"></td></tr>			
				<tr id="trSearch" runat="server">
					<td class="TableHeader" colspan="5"><asp:Label id="lblHeader" runat="server" Text="Search Criteria"></asp:Label></td>
				</tr>
				<tr id="trSearchCriteria1" runat="server" class="tablecol" colspan="4" width="100%">
					<td class="tablecol" width="13%" >
                       <strong><asp:Label ID="Label1" runat="server" Text="Item Code " CssClass="lbname"></asp:Label></strong>:</td>
                    <td class="tablecol" width="30%"><asp:textbox id="txtVendorItemCode" width="100%" runat="server" CssClass="txtbox" Height="20px" ></asp:textbox></td>
					<td class="tablecol" width="18%">
                        <strong><asp:Label ID="Label3" runat="server" Text="Commodity Type "></asp:Label></strong>:</td>
                    <td class="tablecol" width="38%" colspan="1">&nbsp;<asp:textbox id="txtCommodity" width="80%" runat="server" CssClass="txtbox"></asp:textbox><input type="hidden" id="hidCommodity" runat="server" /></td>
				</tr>
				<tr id="trSearchCriteria2" runat="server" class="tablecol">
 					<td class="tablecol">
                        <strong><asp:Label ID="Label2" runat="server" Text=" Item Name "></asp:Label></strong>:</td>
					<td class="tablecol"><asp:textbox id="txtDesc" width="100%" runat="server" CssClass="txtbox" ></asp:textbox></td>
					<td class="tablecol" width="18%">
                        <strong><asp:Label ID="Label4" runat="server" Text="Item Type "></asp:Label></strong>:</td>
<%--					<td colspan="2" style="height: 20px;">
					    &nbsp;&nbsp;&nbsp;<asp:checkbox id="chkActive" Text="Active" Runat="server" Checked="True"></asp:checkbox>
					    &nbsp;&nbsp;&nbsp;<asp:checkbox id="chkInActive" Text="Inactive" Runat="server" Checked="False"></asp:checkbox>
					</td>--%>
					<td class="tablecol">
					    <asp:checkbox id="chkSpot" Text="Spot" Runat="server" Checked="false"></asp:checkbox>
					    <asp:checkbox id="chkStock" Text="Stock" Runat="server" Checked="false"></asp:checkbox>
					    <asp:checkbox id="chkMRO" Text="MRO" Runat="server" Checked="false"></asp:checkbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					    <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" CausesValidation="false"></asp:button>&nbsp;
						<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/>
					</td>
				</tr>
				
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr id="trAddItem" runat="server">
					<td class="emptycol" colspan="5">
					    <asp:datagrid id="dtgCatalogue" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>								
								<asp:BoundColumn DataField="PM_Vendor_Item_Code" SortExpression="PM_Vendor_Item_Code" HeaderText="Item Code">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_Product_Desc" SortExpression="PM_Product_Desc" HeaderText="Item Name">
									<HeaderStyle Width="17%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Item Type" SortExpression="PM_ITEM_TYPE">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM"  HeaderText="UOM">
									<HeaderStyle Width="6%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Currency">
									<HeaderStyle HorizontalAlign="Left" Width="13%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_curr" runat="server" CssClass="ddl" Width="90%"></asp:DropDownList>								
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Contract Price *">
									<HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_price" runat="server" CssClass="numerictxtbox" Width="90%"></asp:TextBox>
										<asp:RegularExpressionValidator id="rev_price" runat="server" ControlToValidate="txt_price" ValidationExpression="(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Tax (%)">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_tax" runat="server" CssClass="ddl" Width="85%"></asp:DropDownList>								
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="SST Rate *">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_GSTRate" runat="server" CssClass="ddl" Width="90%" AutoPostBack="true"></asp:DropDownList>								
										<asp:Label runat="server" ID="lbl_GSTRate" Visible="false"></asp:Label>
										<asp:TextBox id="txtGSTQ" runat="server" CssClass="lblnumerictxtbox" ForeColor="Red" Width="2px"
											 contentEditable="false" Visible="false"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="SST Tax Code">
									<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_GSTCode" runat="server" CssClass="ddl" Width="90%">
										</asp:DropDownList>								
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Remarks">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle Wrap="False"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_remark" runat="server" CssClass="Remarks" Width="90%" Rows="2" TextMode="MultiLine"
											MaxLength="400"></asp:TextBox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" ForeColor="Red" Width="2px"
											 contentEditable="false" Visible="false" ></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PM_PRODUCT_CODE" ReadOnly="True" HeaderText="Product Code" Visible="false">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</td>
				</tr>
				<tr id="trModItem" runat="server">
					<td class="emptycol" colspan="5">
					    <asp:datagrid id="dtgCatalogueMod" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommandMode_Click">
							<Columns>															
								<asp:BoundColumn DataField="CDI_VENDOR_ITEM_CODE" SortExpression="CDI_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDI_PRODUCT_DESC" SortExpression="CDI_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle Width="17%"></HeaderStyle>
								</asp:BoundColumn>								
								<asp:BoundColumn DataField="CDI_UOM" SortExpression="CDI_UOM"  HeaderText="UOM">
									<HeaderStyle Width="6%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Currency">
									<HeaderStyle HorizontalAlign="Left" Width="11%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_curr1" runat="server" CssClass="ddl" Width="90%"></asp:DropDownList>								
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Contract Price *">
									<HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_price1" runat="server" CssClass="numerictxtbox" Width="90%"></asp:TextBox>
										<asp:RegularExpressionValidator id="rev_price1" runat="server" ControlToValidate="txt_price1" ValidationExpression="(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Tax (%)">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_tax1" runat="server" CssClass="ddl" Width="90%"></asp:DropDownList>								
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="SST Rate *">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_GSTRate1" runat="server" CssClass="ddl" Width="90%" AutoPostBack="true"></asp:DropDownList>								
										<asp:Label runat="server" ID="lbl_GSTRate1" Visible="false"></asp:Label>
										<asp:TextBox id="txtGSTQ1" runat="server" CssClass="lblnumerictxtbox" ForeColor="Red" Width="2px"
											 contentEditable="false" Visible="false"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="SST Tax Code">
									<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_GSTCode1" runat="server" CssClass="ddl" Width="90%">
										    <asp:ListItem Value ="">---Select---</asp:ListItem>
										</asp:DropDownList>								
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText=" Remarks">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle Wrap="False"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_remark1" runat="server" CssClass="Remarks" Width="90%" Rows="2" TextMode="MultiLine"
											MaxLength="400"></asp:TextBox>
										<asp:TextBox id="txtQ1" runat="server" CssClass="lblnumerictxtbox" ForeColor="Red" Width="2px"
											 contentEditable="false" Visible="false" ></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CDI_PRODUCT_CODE" ReadOnly="True" HeaderText="Product Code" Visible="false">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</td>
				</tr>
				<tr>
					<td class="EmptyCol" colspan="2">
					    <asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;					    
					    <asp:button id="cmdClose" runat="server" CssClass="button" Text="Close" CausesValidation="false" ></asp:button>&nbsp;
					</td>
				</tr>	
				<tr><td class="rowspacing" colspan="2"></td></tr> 
				<tr>
					<td class="emptycol" colspan="2">
					    <asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary>
					    <asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
