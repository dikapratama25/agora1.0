<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="PO_CRReport.aspx.vb" Inherits="eProcure.PO_CRReport" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Preview PO Cancellation Request</title>
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
								<TD width="50%"><asp:image id="Image1" runat="server" ImageUrl="../Images/logo_tx123_2.jpg" ImageAlign="Left"
										BorderStyle="None" Width="140px"></asp:image></TD>
								<TD class="header" width="50%"><FONT size="5">PO Cancellation Request</FONT></TD>
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
										<TR>
											<td colSpan="3"><asp:label id="lblComName" runat="server" Font-Bold="True"></asp:label><br>
												<asp:label id="lblAddr" runat="server"></asp:label></td>
										</TR>
										<tr>
											<td vAlign="top" noWrap width="36%">Business Reg. No.</td>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<td vAlign="top" width="60%"><asp:label id="lblBregNo" runat="server" Width="145px"></asp:label></td>
										</tr>
										<tr>
											<td vAlign="top">CR Request By</td>
											<TD vAlign="top" align="center">:</TD>
											<td vAlign="top"><asp:label id="lblReqBuyer" runat="server" Width="145px"></asp:label></td>
										</tr>
										<tr>
											<td vAlign="top">Tel
											</td>
											<TD vAlign="top" align="center">:</TD>
											<td vAlign="top"><asp:label id="lblTel" runat="server" Width="145px"></asp:label></td>
										</tr>
										<tr>
											<td>Email</td>
											<td vAlign="top" align="center">:</td>
											<td><asp:label id="lblBuyerEmail" runat="server"></asp:label></td>
										</tr>
									</table>
								</TD>
								<TD class="emptycol" width="50%">
									<TABLE class="alltable" id="Table12" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top"><STRONG>CR No.</STRONG></TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblCRNo" runat="server" Width="145px" Font-Bold="True"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap><STRONG>CR Date</STRONG></TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblReqDate" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<td vAlign="top" noWrap width="36%"><STRONG>PO No.</STRONG></td>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<td vAlign="top" width="60%"><asp:label id="lblPONo" runat="server" Width="145px"></asp:label></td>
										</TR>
										<TR>
											<td vAlign="top" noWrap width="36%"><STRONG>PO Date</STRONG></td>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<td vAlign="top" width="60%"><asp:label id="lblPODate" runat="server" Width="145px"></asp:label></td>
										</TR>
										<TR>
											<TD vAlign="top" noWrap><STRONG>Payment Terms </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblPayTerm" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap><STRONG>Payment Method </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblPayMethod" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap><STRONG>Shipment Terms </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblShipTerm" runat="server" Width="145px"></asp:label></TD>
										<TR>
											<TD vAlign="top" noWrap><STRONG>Shipment Mode </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblShipMode" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" style="display:none;"><STRONG>Freight Terms</STRONG></TD>
											<TD vAlign="top" align="center" style="display:none;">:</TD>
											<TD vAlign="top" style="display:none;"><asp:label id="lblFreighTerm" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top"><STRONG>Ship Via </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblShipVia" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" align="left"><B>Ship To</B></TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblShipTo" runat="server"></asp:label></TD>
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
										<TBODY>
											<tr>
												<TD colSpan="3"><B>To&nbsp; </B>:<br>
													<asp:label id="lblVendorName" runat="server" Font-Bold="True"></asp:label><br>
													<asp:label id="lblVAddr" runat="server"></asp:label><br>
													<asp:label id="lblCountry" runat="server"></asp:label></TD>
											</tr>
											<TR>
												<TD vAlign="top" noWrap width="36%">Business Reg. No.</TD>
												<TD vAlign="top" align="center" width="4%">:</TD>
												<TD class="emptycol" width="60%"><asp:label id="lblVregNo" runat="server" Width="145px"></asp:label></TD>
											</TR>
											<TR>
												<TD vAlign="top">Tel</TD>
												<TD vAlign="top" align="center">:</TD>
												<TD vAlign="top"><asp:label id="lblVendorTel" runat="server"></asp:label></TD>
											</TR>
											<TR>
												<TD vAlign="top">Email</TD>
												<TD vAlign="top" align="center">:</TD>
												<TD vAlign="top"><asp:label id="lblVendorEmail" runat="server"></asp:label></TD>
											</TR>
										</TBODY>
									</table>
								</td>
								<td vAlign="top" width="50%"><STRONG>Bill To </STRONG>:<BR>
									<asp:label id="lblBuyerComp" runat="server" Font-Bold="True"></asp:label><BR>
									<asp:label id="lblBillTo" runat="server" Width="145px"></asp:label></td>
							</tr>
							<tr>
								<td colSpan="2">&nbsp;</td>
							</tr>
							<TR>
								<td colSpan="2">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" width="18%"><STRONG>CR Remarks</STRONG>
											</TD>
											<TD vAlign="top" align="center" width="2%">:</TD>
											<TD vAlign="top" width="80%"><asp:label id="lblRemark" runat="server"></asp:label></TD>
										</TR>
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
					<TD><asp:datagrid id="dtg_POList" runat="server" Width="100%" AutoGenerateColumns="False" BorderWidth="0px"
							HeaderStyle-VerticalAlign="Bottom">
							<HeaderStyle VerticalAlign="Bottom"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="POD_PO_LINE" SortExpression="POD_PO_LINE" HeaderText="Line" Visible="false">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Line" SortExpression="PO Line">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PRODUCT_DESC" SortExpression="POD_PRODUCT_DESC" HeaderText="Description">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="34%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_MIN_PACK_QTY" SortExpression="POD_MIN_PACK_QTY" HeaderText="MPQ">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="6%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_WARRANTY_TERMS" SortExpression="POD_WARRANTY_TERMS" HeaderText="W.T.&lt;br&gt;(Mths)">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" SortExpression="POD_ORDERED_QTY" HeaderText="Ordered">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_RECEIVED_QTY" SortExpression="POD_RECEIVED_QTY" HeaderText="Receive&lt;br&gt;Qty">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_REJECTED_QTY" SortExpression="POD_REJECTED_QTY" HeaderText="Reject&lt;br&gt;Qty">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="7%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Outstanding">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PCD_CANCELLED_QTY" SortExpression="PCD_CANCELLED_QTY" HeaderText="Qty To&lt;br&gt;Cancel">
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
					<TD class="emptycol">W.T. = Warranty Terms
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<HR>
					</TD>
				</TR>
				<TR>
					<TD><font style="FONT-SIZE: 7pt">This is a computer generated document. No signature is 
							required.</font>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
