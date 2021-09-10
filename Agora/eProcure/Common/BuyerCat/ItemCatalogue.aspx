<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ItemCatalogue.aspx.vb" Inherits="eProcure.ItemCatalogue" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ItemCatalogue</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
	<%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		function selectAll()
		{
			SelectAllG("dtgItem_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgItem_ctl02_chkAll","chkSelection");
		}

		
		function ShowDialog(filename,height)
			{
				
				var retval="";
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 700px");
				//retval=window.open(filename);
								if (retval == "1" || retval =="" || retval==null)
				{  window.close;
					return false;

				} else {
				    window.close;
					return true;

				}
			}
		
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ItemCatalogue_tabs"))%>
			<TABLE class="alltable" id="Table11" cellSpacing="0" cellPadding="0" width="100%" border="0">
            <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			</TR>
				<TR>
					<TD align="center">
						<div align="left"><asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text="Step 1: Create user defined Purchase Catalogue.<br><b>=></b> Step 2: Assign item master to Purchaser Catalogue.<br>Step 3: Assign purchaser to Purchaser Catalogue."
						></asp:label>
                        </div>
					</TD>
				</TR>
             <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
            <tr>
					<TD align="center">
						<div align="left"><asp:label id="Label5" runat="server"  CssClass="lblInfo"
						Text="The system comes with the Default Purchaser Catalogue which can be immediately assigned to the Purchaser.</br>Note: Default Purchaser Catalogue consists of all items in the Item Master that have been created in the system."
						></asp:label>
                        </div>
					</TD>
			</TR>
            <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
				<TR>
					<TD class="tablecol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" colSpan="5"style="height: 3px"></TD>
				</TR>
						<TR>
								<TD class="tableheader">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
					<TD class="tablecol" nowrap style="height: 25px">&nbsp;<STRONG>Purchaser&nbsp;Catalogue</STRONG> :&nbsp;<asp:DropDownList ID="cboCatalogueBuyer" runat="server" CssClass="txtbox" Width="322px" AutoPostBack="True">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                   </TD>
                                   </TR>	
             </table>

				<Div id="legend" style="DISPLAY: none" runat="server"><br>
				<table class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
					<tr><TD class="EmptyCol" colSpan="5">&nbsp;Legend:</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colSpan="5">&nbsp;LP:&nbsp;List Price 
						Item&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; CP: Contract Price 
						Item&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; DP: Discount Price Item</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colSpan="5">&nbsp;
						<asp:label id="Label1" runat="server" Width="24px" BackColor="#ff99cc"></asp:label>&nbsp;Contract 
						Price Item has Expired/Removed from Product List</TD>
				</TR>
				<TR>
					<TD class="EmptyCol" colSpan="5">&nbsp;
						<asp:label id="Label2" runat="server" Width="24px" BackColor="PaleTurquoise"></asp:label>&nbsp;Invalid 
						Vendor Status</TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				</table>
				</Div>
        </TABLE>
			<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="emptycol" colSpan="6"width="100%" ><asp:datagrid id="dtgItem" runat="server" CssClass="Grid" OnPageIndexChanged="MyDataGrid_Page"
							AllowSorting="True" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
										<!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PM_VENDOR_ITEM_CODE" SortExpression="PM_VENDOR_ITEM_CODE"  readonly="True"  
									HeaderText="Item Code">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC"  readonly="True"   HeaderText="Item Name">
									<HeaderStyle Width="27%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CT_NAME" SortExpression="CT_NAME" HeaderText="Commodity Type">
									<HeaderStyle Width="20%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
                        		<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM"  readonly="True"    HeaderText="UOM">
									<HeaderStyle Width="12%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="BCI_ITEM_INDEX" readonly="True"   ></asp:BoundColumn>
								<asp:BoundColumn Visible="False"></asp:BoundColumn>
</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="height: 6px"></TD>
				</TR>
				<tr>
					<td colSpan="6" style="height: 25px"><asp:button id="cmd_Add" runat="server" Text="Add" CssClass="button" Enabled="False" Visible="False"></asp:button>
					                <asp:button id="cmd_Delete" runat="server" Text="Remove" CssClass="button" Enabled="False" Visible="False"></asp:button>&nbsp;
					                <asp:button id ="btnHidden" runat="server" style="display:none" onclick="btnHidden_Click"></asp:button> &nbsp;
					                <asp:button id="cmdAddContract" runat="server" Text="Add Contract" CssClass="button" Width="90px" Enabled="False" Visible="False"></asp:button>
					                &nbsp;<asp:button id="cmdBuyerAsg" runat="server" Text="Buyer Assignment" CssClass="button" Width="120px" Enabled="False" Visible="False"></asp:button>&nbsp;
					                <asp:button id="cmdDeleteContract" runat="server" Text="Delete Contract" CssClass="button" Width="90px" Enabled="False" Visible="False"></asp:button>&nbsp;
					                <INPUT class="button" id="cmd_Reset" onclick="DeselectAllG('dtgItem_ctl02_chkAll','chkSelection')"
							type="button" value="Reset" name="cmd_Reset" runat="server" visible="false">
					</td>
				</tr>
				<TR id="trhid" runat="server">
					<TD align="center" >
						<div align="left"><asp:label id="Label4" runat="server"  CssClass="lblInfo"
						Text="a) Click Add button to add new item master to the selected user defined Purchaser Catalogue (Not applicable to Default Purchaser Catalogue).<br>b) Click Remove button to delete item master from the selected user defined Purchaser Catalogue (Not applicable to Default Purchaser Catalogue)."></asp:label>
                        </div>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="5"><asp:hyperlink id="lnkBack" Runat="server" Enabled="False" Visible="False">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
