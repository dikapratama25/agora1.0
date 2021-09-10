<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="POReport.aspx.vb" Inherits="eProcure.POReport" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Preview PO Report</title>
		<meta content="False" name="vs_snapToGrid">
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD>
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD width="50%"><asp:image id="Image1" runat="server" Width="140px" BorderStyle="None" ImageUrl=""
										ImageAlign="Left"></asp:image></TD>
								<td></td>
								<TD class="header" width="50%" align="left"><font size="5" >Purchase Order</font></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD vAlign="top" width="50%">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3"><asp:label id="lblComName" runat="server" Font-Bold="True"></asp:label><br>
												<asp:label id="lblAddr" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td vAlign="top" noWrap width="36%">Business Reg. No.</td>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<td vAlign="top" width="60%"><asp:label id="lblBusiness" runat="server" Width="145px"></asp:label></td>
										</tr>
										<tr>
											<td vAlign="top">Buyer Name</td>
											<TD vAlign="top" align="center">:</TD>
											<td vAlign="top"><asp:label id="lblBuyer" runat="server" Width="145px"></asp:label></td>
										</tr>
										<tr>
											<td vAlign="top" style="height: 19px">Tel
											</td>
											<TD vAlign="top" align="center" style="height: 19px">:</TD>
											<td vAlign="top" style="height: 19px"><asp:label id="lblTel" runat="server" Width="145px"></asp:label></td>
										</tr>
										<tr>
											<td>Email</td>
											<td vAlign="top" align="center">:</td>
											<td><asp:label id="lblBuyerEmail" runat="server"></asp:label></td>
										</tr>
									</table>
								</TD>
								<TD class="emptycol" width="50%">
									<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD width="36%"><STRONG>PO No. </STRONG>
											</TD>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<TD class="emptycol" vAlign="top" width="60%"><asp:label id="lblPONo" runat="server" Width="145px" Font-Bold="True"></asp:label></TD>
										</TR>
										<TR>
											<TD><STRONG>Date </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblDate" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Payment Terms</STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblPayTerm" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Payment Method </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblPayMethod" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Shipment Terms </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblShipTerm" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Shipment Mode </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblShipMode" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap style="display:none;"><STRONG>Freight Terms </STRONG>
											</TD>
											<TD vAlign="top" align="center" style="display:none;">:</TD>
											<TD class="emptycol" vAlign="top" style="display:none;"><asp:label id="lblFreighTerm" runat="server" Width="145px" Visible="false"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Ship Via </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblShipVia" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD><STRONG>Ship To</STRONG></TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" style="HEIGHT: 2px" vAlign="top"><asp:label id="lblShipTo" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<tr>
								<td colSpan="2">&nbsp;</td>
							</tr>
							<tr>
								<td width="50%">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<TD colSpan="3">
												<P><STRONG>To </STRONG>:<BR>
													<asp:label id="lblVendorName" runat="server" Font-Bold="True"></asp:label><BR>
													<asp:label id="lblVAddr" runat="server"></asp:label><asp:label id="lblCountry" runat="server"></asp:label></P>
											</TD>
										</tr>
										<TR>
											<TD vAlign="top" noWrap width="36%">Business Reg. No.</TD>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<TD class="emptycol" width="60%"><asp:label id="lblTaxNo" runat="server"></asp:label></TD>
										</TR>
										<tr>
											<td>Tel</td>
											<TD vAlign="top" align="center">:</TD>
											<TD><asp:label id="lblVendorTel" runat="server"></asp:label></TD>
										</tr>
										<TR>
											<TD>Email</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD><asp:label id="lblVendorEmail" runat="server"></asp:label></TD>
										</TR>
									</table>
								</td>
								<td vAlign="top" width="50%"><STRONG>Bill To </STRONG>:<BR>
									<asp:label id="lblVendorName2" runat="server" Font-Bold="True"></asp:label><br>
									<asp:label id="lblBillTo" runat="server"></asp:label></td>
							</tr>
							<tr>
								<td colSpan="2">&nbsp;</td>
							</tr>
							<TR>
								<td colSpan="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<TD width="18%"><STRONG>Remarks </STRONG>
											</TD>
											<TD vAlign="top" align="center" width="2%">:</TD>
											<TD class="emptycol" width="80%"><asp:label id="lblRemark" runat="server"></asp:label></TD>
										</tr>
									</table>
								</td>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgPOReport" runat="server" HeaderStyle-Font-Bold="True" HeaderStyle-Font-Size="10pt"
							HeaderStyle-VerticalAlign="Bottom" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False"
							BorderWidth="0px">
							<HeaderStyle Font-Size="10pt" Font-Bold="True" VerticalAlign="Bottom"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="POD_PO_LINE" SortExpression="POD_PO_LINE" HeaderText="Line">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Product_Desc" SortExpression="POD_Product_Desc" HeaderText="Item Description">
									<HeaderStyle HorizontalAlign="Left" Width="19%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" SortExpression="POD_ORDERED_QTY" HeaderText="Qty">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM">
									<HeaderStyle HorizontalAlign="Left" Width="1%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Min_Pack_Qty" SortExpression="POD_Min_Pack_Qty" HeaderText="Pack Qty">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Min_Order_Qty" SortExpression="POD_Min_Order_Qty" HeaderText="MOQ">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UNIT_COST" SortExpression="POD_UNIT_COST" HeaderText="Unit Price (MYR)">
									<HeaderStyle HorizontalAlign="Right" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Amt (MYR)">
									<HeaderStyle HorizontalAlign="Right" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="POD_GST" HeaderText="Tax (MYR)">
									<HeaderStyle HorizontalAlign="Right" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="EDD">
									<HeaderStyle HorizontalAlign="Left" Width="8%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_WARRANTY_TERMS" SortExpression="POD_WARRANTY_TERMS" HeaderText="W.T. (Mths)" Visible="False">
									<HeaderStyle HorizontalAlign="Right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="height: 38px">EDD = Estimated Delivery Date						
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<HR>
					</TD>
				</TR>
				<TR>
					<TD><font style="FONT-SIZE: 7pt">All shipments, shipping papers, invoices and 
							correspondence must be identified with our Purchase Order Number. Overshipments 
							will not be accepted.<br>
							This is a computer generated document. No signature is required.</font>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
