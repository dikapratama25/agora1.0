<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Testing.aspx.vb" Inherits="eProcure.Testing" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CustomFields</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>	
        <% Response.Write(Session("AutoComplete")) %>
        <script runat="server">
            Dim dDispatcher as New dispatcher
        </script>
        <% Response.write(Session("typeahead")) %>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
					<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header">Add Item</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="height: 3px" ></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader" colspan="6">&nbsp;Search Criteria :</TD>
							</TR>
							<tr class="tablecol" width="100%">
					        <td colspan="6" nowrap style="height: 20px;">
					        </td>
							</tr>
							<TR class="tablecol" width="100%">
					            <TD class="tablecol" width="18%">
                                 <strong>&nbsp;<asp:Label ID="Label3" runat="server" Text="Commodity Type :"></asp:Label></strong></TD>
                                <TD class="TableInput" width="30%" ><asp:dropdownlist id="cboCommodityType" width="100%" runat="server" CssClass="ddl" ></asp:dropdownlist></TD>
			                    <td width="2%"></td>
					            <TD class="tablecol" width="15%" >
                                    <strong>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Item Name :" CssClass="lbname"></asp:Label></strong></TD>
                                <TD class="TableInput" width="34%"><asp:textbox id="txt_item_desc" width="100%" runat="server" CssClass="txtbox" Height="20px" ></asp:textbox></TD>
			                    <td  class="tablecol" width="1%"></td>
				            </TR>
							<TR class="tablecol">
								<TD class="tablecol">&nbsp;<STRONG>Vendor Company</STRONG>:</td>
								<TD class="TableInput" ><asp:textbox id="txt_vendor_com" runat="server"  Width="100%" CssClass="txtbox"></asp:textbox><asp:dropdownlist id="ddl_vendor_com" runat="server" CssClass="ddl"></asp:dropdownlist></td>
			                    <td  class="tablecol"></td>
								<td colspan="2">	<asp:button id="cmd_search" runat="server" CssClass="button" Width="63px" Text="Search"></asp:button>
									<asp:button id="cmd_clear" runat="server" CssClass="button" Width="59px" Text="Clear"></asp:button>
                                    <asp:Button ID="cmd_freeformClear" runat="server" CssClass="button" Text="Clear" Visible="False"
                                        Width="59px" /><!--<Input Type= "Hidden" Name= "hidVendors" Value= "">--></TD>
			                    <td  class="tablecol"></td>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgTest" runat="server" CssClass="grid">
							<Columns>
                                <asp:TemplateColumn HeaderText="Item Code">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
										<asp:TextBox id="txtItemCode" CssClass="txtbox"  Width="55px" MaxLength="5" Runat="server"></asp:TextBox>
								</ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Item Name">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
										<asp:TextBox id="txtItemName" CssClass="txtbox"  Width="55px" MaxLength="5" Runat="server"></asp:TextBox>
								</ItemTemplate>
                                </asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="QTY">
									<HeaderStyle HorizontalAlign="Right" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_qty" CssClass="txtbox"  Width="55px" MaxLength="5" Rows="2" Runat="server"></asp:TextBox>
										<asp:Label id="lbl_alert" runat="server" ForeColor="Red" Visible="False"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Company">
                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlCompany" runat="server">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                </asp:TemplateColumn>
                                
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<tr>
				<td>
				<% Response.Write(Session("ConstructTable")) %>
				</td>
				</tr>
				<TR>
					<TD style="HEIGHT: 19px"><asp:button id="cmd_Save" runat="server" CssClass="button" Text="Save" ></asp:button>&nbsp;
					<asp:Button ID="btnAddRow" runat="server" Text="Add Row" /></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>

