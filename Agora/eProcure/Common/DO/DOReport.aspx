<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="DOReport.aspx.vb" Inherits="eProcure.DDOReport" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Preview DO Report</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="tblDOHeader" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td vAlign="top" align="left" width="50%"><asp:image id="Image1" runat="server" ImageUrl="../Images/logo_tx123_2.jpg" Width="140px"></asp:image></td>
					<td class="header" noWrap width="50%"><font size="5">Delivery Order</font>
					</td>
				</tr>
				<TR>
					<TD class="emptycol" colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD vAlign="top" width="50%">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td colSpan="3" style="HEIGHT: 38px"><asp:label id="lblSuppComp" runat="server" Font-Bold="True"></asp:label><br>
												<asp:label id="lblSuppAdd1" runat="server"></asp:label></td>
										</tr>
										<TR>
											<TD vAlign="top" noWrap width="36%">Business Reg. No.</TD>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<td vAlign="top" width="60%"><asp:label id="lblGstRegNo" runat="server" Width="145px"></asp:label></td>
										</TR>
										<TR>
											<TD vAlign="top" noWrap width="36%">Tel</TD>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<td vAlign="top" width="60%"><asp:label id="lblTel" runat="server" Width="145px"></asp:label></td>
										</TR>
										<TR>
											<TD vAlign="top" noWrap width="36%">Email</TD>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<td vAlign="top" width="60%"><asp:label id="lblEmail" runat="server" Width="145px"></asp:label></td>
										</TR>
									</table>
								</TD>
								<TD vAlign="top" width="50%">
									<TABLE class="alltable" id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD noWrap width="35%"><STRONG>DO No. </STRONG>
											</TD>
											<TD align="center" width="5%">:</TD>
											<TD width="60%"><asp:label id="lblDONo" runat="server" Font-Bold="True"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>PO No. </STRONG>
											</TD>
											<TD align="center">:</TD>
											<TD><asp:label id="lblPONo" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Delivery Date </STRONG>
											</TD>
											<TD align="center">:</TD>
											<TD><asp:label id="lblDevlDate" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Payment Terms</STRONG></TD>
											<TD align="center">:</TD>
											<TD><asp:label id="lblPayTerm" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Payment Method</STRONG></TD>
											<TD align="center">:</TD>
											<TD><asp:label id="lblPayMthd" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Shipment Terms </STRONG>
											</TD>
											<TD align="center">:
											</TD>
											<TD><asp:label id="lblShipTerm" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Shipment Mode </STRONG>
											</TD>
											<TD align="center">:
											</TD>
											<TD><asp:label id="lblShipMthd" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Our Ref. No. </STRONG>
											</TD>
											<TD align="center">:
											</TD>
											<TD><asp:label id="lblOurRefNo" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Our Ref. Date </STRONG>
											</TD>
											<TD align="center">:
											</TD>
											<TD><asp:label id="lblRefDate" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><b>Air Way Bill No. </b>
											</TD>
											<TD align="center">:
											</TD>
											<TD><asp:label id="lblAirWayBillNo" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Freight Carrier </STRONG>
											</TD>
											<TD align="center">:
											</TD>
											<TD><asp:label id="lblFreightCarrier" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD noWrap><STRONG>Freight Amount </STRONG>
											</TD>
											<TD align="center">:
											</TD>
											<TD><asp:label id="lblFrightAmt" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2">
						<TABLE class="alltable" id="Table7" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD colSpan="2">
									<TABLE class="alltable" id="Table8" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD noWrap width="18%"><STRONG>Attention To </STRONG>
											</TD>
											<TD align="center" width="2%">:</TD>
											<TD width="80%"><asp:label id="lblAttnTo" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD><STRONG>Requestor </STRONG>
											</TD>
											<TD align="center">:</TD>
											<TD><asp:label id="lblRequestor" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="2"></TD>
							</TR>
							<TR>
								<TD noWrap width="50%"><STRONG>Ship To</STRONG> :
									<br>
									<asp:label id="lblBuyerCompName" runat="server" Font-Bold="True"></asp:label><BR>
									<asp:label id="lblShipAdd1" runat="server"></asp:label></TD>
								<TD noWrap width="50%"><STRONG>Bill To</STRONG> :
									<br>
									<asp:label id="lblBuyerCompName2" runat="server" Font-Bold="True"></asp:label><br>
									<asp:label id="lblBillAdd1" runat="server"></asp:label></TD>
							</TR>
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
			</TABLE>
			<TABLE class="alltable" id="tblDOHeader2" cellSpacing="0" cellPadding="0" width="100%"
				border="0">
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgDODtl" runat="server" Width="100%" HeaderStyle-VerticalAlign="Bottom" OnSortCommand="SortCommand_Click"
							AutoGenerateColumns="False" BorderWidth="0px">
							<HeaderStyle VerticalAlign="Bottom"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="DOD_DO_LINE" SortExpression="DOD_DO_LINE" HeaderText="Line">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Vendor_Item_Code" SortExpression="POD_Vendor_Item_Code" HeaderText="Vendor&lt;br&gt;Item Code">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Product_Desc" SortExpression="POD_Product_Desc" HeaderText="Description">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="31%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="6%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ETD" SortExpression="POD_ETD" HeaderText="EDD">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Warranty_Terms" SortExpression="POD_Warranty_Terms" HeaderText="W.T.<br>(Mths)">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="6%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Min_Pack_Qty" SortExpression="POD_Min_Pack_Qty" HeaderText="MPQ">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Min_Order_Qty" SortExpression="POD_Min_Order_Qty" HeaderText="MOQ">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Ordered_Qty" SortExpression="POD_Ordered_Qty" HeaderText="Ordered">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="6%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOD_SHIPPED_QTY" SortExpression="DOD_SHIPPED_QTY" HeaderText="Shipped">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="6%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">EDD = Estimated Delivery Date<br>
						W.T. = Warranty Terms
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<HR>
					</TD>
				</TR>
				<TR>
					<TD><font style="FONT-SIZE: 7pt"> This is a computer generated document. No signature 
							is required.</font>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
