<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ListPriceCatalogue.aspx.vb" Inherits="eProcure.ListPriceCatalogue" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Item Listing</title>
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
				oform.txtCommodity.value="";
				oform.hidCommodity.value="";
				checkStatus(false);
			}
			
			function SelectAll_1()
			{
				checkStatus(true);
			}
		
			function checkStatus(checked)
			{
				var oform = document.forms(0);
				oform.chkReject.checked=checked;
				oform.chkHubPending.checked=checked;	
				oform.chkHubApprove.checked=checked;	
			}
		
		-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_VIM_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="header" colspan="6" style="height: 3px;"></TD>
				</TR>
				<TR>
					<TD class="header" colspan="6"><FONT size="1"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></FONT></TD>
				</TR>
				<tr>
					<TD class="linespacing1" colSpan="6"></TD>
			    </TR>
			    <TR>
				    <TD class="emptycol" colSpan="6">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Fill in the search criteria and click Search button to list the relevant item. Click the Add button to add new item. Select the item and click the Modify button to modify the item."
					    ></asp:label>

				    </TD>
			    </TR>
                <tr>
					    <TD class="linespacing2" colSpan="6"></TD>
			    </TR>
				<TR>
					<TD class="tableheader" colspan="6">&nbsp;Search Criteria</TD>
				</TR>
				<TR class="tablecol">
					<TD class="tablecol" width="10%" nowrap>
                       <strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="Item Code :" CssClass="lbl"></asp:Label></strong>
                    <TD class="tablecol" width="15%">&nbsp;<asp:textbox id="txtVendorItemCode" runat="server" CssClass="txtbox" Width="153px" ></asp:textbox>&nbsp;</TD>
					<TD class="tablecol" width="13%" nowrap>
                        <strong>&nbsp;<asp:Label ID="Label2" runat="server" Text=" Item Name : "></asp:Label></strong></TD>
					<TD class="tablecol" width="30%" nowrap><asp:textbox id="txtDesc" runat="server" CssClass="txtbox" Width="190px" ></asp:textbox></TD>
				
				</TR>
				<TR class="tablecol">
					<TD class="tablecol" nowrap>
                        <strong>&nbsp;<asp:Label ID="Label3" runat="server" Text="Commodity Type :"></asp:Label></strong>
                    <TD class="tablecol" >&nbsp;<asp:textbox id="txtCommodity" width="100%" runat="server" CssClass="txtbox"></asp:textbox><input type="hidden" id="hidCommodity" runat="server" /></TD>
                    <td class="tablecol"></td>
                    <td class="tablecol"></td>
				</TR>
				<tr class="tablecol">
					<td class="tablecol" style="height: 20px">&nbsp;<STRONG>Status</STRONG> :</td>
					<td class="tablecol" nowrap style="height: 20px;"><asp:checkbox id="chkHubPending" Text="Pending Approval" Runat="server"></asp:checkbox></td>
					<td class="tablecol" nowrap style="height: 20px"><asp:checkbox id="chkReject" Text="Rejected By Hub Admin" Runat="server"></asp:checkbox></td>
					<td class="tablecol" nowrap style="height: 20px"><asp:checkbox id="chkHubApprove" Text="Approved By Hub Admin" Runat="server"></asp:checkbox></td>
				</tr>
				<TR class="tablecol">
					<TD class="tablecol" colspan="6" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
						<INPUT class="button" id="cmdSelectAll" onclick="SelectAll_1();" type="button" value="Select All"
							name="cmdSelectAll" runat="server"> 
						<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="6">&nbsp;</TD>
				</TR>
				<tr>
					<td class="emptycol" colspan="6"><asp:datagrid id="dtgCatalogue" runat="server" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="PM_PRODUCT_INDEX" SortExpression="PM_PRODUCT_INDEX" HeaderText="Item Index"></asp:BoundColumn>
								    <asp:TemplateColumn SortExpression="PM_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle Width="10%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
										<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkItemCode"></asp:HyperLink>
										<asp:Label Runat="server" ID="lblItemCode"></asp:Label>
									</ItemTemplate>
							</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="PM_PRODUCT_CODE" HeaderText="Item ID" Visible="False">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
										<asp:Label Runat="server" ID="lblCode"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle Width="20%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_CATEGORY_NAME" SortExpression="PM_CATEGORY_NAME" HeaderText="Commodity Type">
									<HeaderStyle Width="11%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_UNIT_COST" SortExpression="PM_UNIT_COST" HeaderText="Price">
									<HeaderStyle Width="7%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM" HeaderText="UOM">
									<HeaderStyle Width="10%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status">
									<HeaderStyle Width="12%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="PM_STATUS" SortExpression="PM_STATUS" HeaderText="Status"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="6">
						<asp:button id="cmdAdd" runat="server" CssClass="Button" Text="Add"></asp:button>
						<asp:button id="cmdModify" runat="server" CssClass="Button" Text="Modify"></asp:button>
						<asp:button id="cmdDelete" runat="server" CssClass="Button" Text="Delete"></asp:button><INPUT class="txtbox" id="hidIndex" style="WIDTH: 43px; HEIGHT: 18px" type="hidden" size="1"
							name="hidIndex" runat="server"></td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
