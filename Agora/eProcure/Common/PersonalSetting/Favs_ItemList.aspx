<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Favs_ItemList.aspx.vb" Inherits="eProcure.Favs_ItemList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Favs_ListMain</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">	
		<% Response.Write(Session("WheelScript"))%>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="header" style="HEIGHT: 25px"><FONT size="1">&nbsp;</FONT><STRONG>Item List</STRONG></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<P><asp:datagrid id="MyDataGrid" runat="server" CssClass="Grid" DataKeyField="FLI_LIST_INDEX" OnPageIndexChanged="MyDataGrid_Page"
								AllowSorting="True" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
								<AlternatingItemStyle BackColor="#f6f9fe"></AlternatingItemStyle>
								<HeaderStyle CssClass="GridHeader"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="PM_VENDOR_ITEM_CODE" SortExpression="PM_VENDOR_ITEM_CODE" HeaderText="Vendor Item Code">
										<HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CBC_B_ITEM_CODE" SortExpression="CBC_B_ITEM_CODE" HeaderText="Buyer Item Code">
										<HeaderStyle Width="12%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor Name">
										<HeaderStyle Width="23%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC" HeaderText="Item Description">
										<HeaderStyle Width="30%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM" HeaderText="UOM">
										<HeaderStyle Width="5%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_CURRENCY_CODE" SortExpression="PM_CURRENCY_CODE" HeaderText="Currency">
										<HeaderStyle Width="5%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PM_UNIT_COST" SortExpression="PM_UNIT_COST" HeaderText="Price">
										<HeaderStyle Width="10%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right" CssClass="gridPager"
									Mode="NumericPages"></PagerStyle>
							</asp:datagrid></P>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 16px"></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;<INPUT type="button" value="Close" class="button" onclick="window.close();">
						<INPUT id="hidMode" style="WIDTH: 42px; HEIGHT: 22px" type="hidden" size="1" runat="server"
							NAME="hidMode"> <INPUT id="hidIndex" style="WIDTH: 49px; HEIGHT: 22px" type="hidden" size="2" runat="server"
							NAME="hidIndex"></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
