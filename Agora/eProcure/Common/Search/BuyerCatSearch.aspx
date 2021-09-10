<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BuyerCatSearch.aspx.vb" Inherits="eProcure.BuyerCatSearch" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Item Listing</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
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
				form1.hidCommodity.value="";
				form1.cboCommodityType.selectedIndex=0;
				if( document.getElementById('chkSpot') ){
				form1.chkSpot.checked=false;}
				if( document.getElementById('chkStock') ){
				form1.chkStock.checked=false;}
				if( document.getElementById('chkMRO') ){
				form1.chkMRO.checked=false;}
			}
			
		function SelectAllCheckBox(){
			var oform = document.forms(0);			
			if( document.getElementById('chkSpot') ){
			form1.chkSpot.checked=true;}
			if( document.getElementById('chkStock') ){
			form1.chkStock.checked=true;}
			if( document.getElementById('chkMRO') ){
			form1.chkMRO.checked=true;}
		}
			
		function PopWindow(myLoc)
		{
			//window.open(myLoc,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight: 255px; dialogWidth: 300px");
			window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
			return false;
		}

		-->
		</script>
	</HEAD>
<body class="body" MS_POSITIONING="GridLayout">
    <form id="form1" method="post" runat="server" defaultbutton="cmdSearch">
			<TABLE class="alltable" id="Tab1" cellSpacing="0" cellPadding="0" width="100%">
 				<TR>
					<TD class="Header" style="padding:0" colspan="3"><asp:label id ="lblTitle" runat="server" Text="Buyer Catalogue Search"></asp:label></TD>
				</TR>
				<tr><td class="rowspacing"></td></tr>
				<TR>
	                <TD class="EmptyCol" colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Select/fill in the search criteria and click Search button to list the relevant Buyer Catalogue. Click the Raise PR button to go to Raise PR page."></asp:label>
	                </TD>
                </TR>
                <tr><td class="rowspacing"></td></tr>
				<TR>
					<TD class="TableHeader" colspan="6">Search Criteria</TD>
				</TR>
				<TR class="tablecol" width="100%">
					<TD class="tablecol" width="8%" >
                       <strong><asp:Label ID="Label4" runat="server" Text="Item Code " CssClass="lbl"></asp:Label></strong>:</TD>
                    <TD class="TableCol" width="32%"><asp:textbox id="txtItemCode" width="80%" runat="server" CssClass="txtbox"></asp:textbox></TD>
					<TD class="tablecol" width="15%" >
                       <strong><asp:Label ID="Label1" runat="server" Text="Item Name " CssClass="lbl"></asp:Label></strong>:</TD>
                    <TD class="TableCol" width="44%"><asp:textbox id="txtItemName" width="80%" runat="server" CssClass="txtbox"></asp:textbox></TD>
				</TR>
				<tr class="tablecol" width ="100%">
					<TD class="TableCol" width="8%"><STRONG><asp:Label ID="Label2" runat="server" Text="Buyer Catalogue " CssClass="lbl"></asp:Label></STRONG>:</TD>
					<TD class="TableCol" width="32%"><asp:dropdownlist id="cboCatalogueBuyer" runat="server" CssClass="ddl"  width="95%"  AutoPostBack="True"></asp:dropdownlist></TD>
					<TD class="tablecol" width="15%"><strong><asp:Label ID="Label3" runat="server" Text="Commodity Type " CssClass="lbl"></asp:Label></strong>:</TD>
                    <TD class="TableCol" width="44%" ><asp:textbox id="txtCommodity" width="80%" runat="server" CssClass="txtbox"></asp:textbox><input type="hidden" id="hidCommodity" runat="server" /><%--<asp:dropdownlist id="cboCommodityType" width="99%" runat="server" CssClass="ddl" ></asp:dropdownlist>--%></TD>
				</tr>
                <tr class="tablecol" width ="100%">
                	<TD class="TableCol" width="20%"><STRONG><asp:Label ID="Label5" runat="server" Text="Item Type " CssClass="lbl"></asp:Label></STRONG>:</TD>
                    <TD class="tablecol">
					    <asp:checkbox id="chkSpot" Text="Spot" Runat="server" Checked="false"></asp:checkbox>&nbsp;
					    <asp:checkbox id="chkStock" Text="Stock" Runat="server" Checked="false"></asp:checkbox>&nbsp;
					    <asp:checkbox id="chkMRO" Text="MRO" Runat="server" Checked="false"></asp:checkbox>
					</TD>
                    <TD class="TableCol" align="right" colspan="2" width="48%">
			            <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
			            <INPUT class="button" id="cmdSelectAll" onclick="SelectAllCheckBox();" type="button" value="Select All" name="cmdSelectAll" runat="server">
						<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear" runat="server">
					</TD>
                </tr>
				<tr><td class="rowspacing"></td></tr>
				<tr>
					<td class="emptycol" colspan="6">
					    <asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">
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
							    <asp:BoundColumn Visible="False" DataField="pm_last_txn_s_coy_id1" SortExpression="pm_last_txn_s_coy_id1" HeaderText="Last Txn. Vendor 1">
								    <HeaderStyle Width="18%" HorizontalAlign="Left" ></HeaderStyle>
								    <ItemStyle HorizontalAlign="Left"></ItemStyle>
							    </asp:BoundColumn>							    
                            </Columns>
						</asp:datagrid>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="6">
						<asp:button id="cmdPR" runat="server" CssClass="Button" Text="Raise PR" Visible="false"></asp:button>
                    </td>
				</tr>
			</TABLE>

    </form>
</body>
</html>
