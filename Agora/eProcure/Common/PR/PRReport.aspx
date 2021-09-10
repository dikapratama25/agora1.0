<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="PRReport.aspx.vb" Inherits="eProcure.PRReport" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>Preview PR Report</title>
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
								<TD width="50%"><asp:image id="Image1" runat="server" ImageAlign="Left" ImageUrl="../Images/logo_tx123_2.jpg"
										BorderStyle="None" Width="140px"></asp:image></TD>
								<TD class="header" width="50%"><FONT size="5">Purchase Requisition</FONT></TD>
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
									<TABLE class="alltable" cellSpacing="0" cellPadding="0" border="0">
										<TR>
											<td colSpan="3"><asp:label id="lblComName" runat="server" Font-Bold="True"></asp:label><BR>
												<asp:label id="lblAddr" runat="server"></asp:label></td>
										</TR>
										<TR>
											<TD vAlign="top" width="46%">Business Reg. No.</TD>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<TD class="emptycol" vAlign="top" width="50%"><asp:label id="lblTaxNo" runat="server" Width="104px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top">Requestor</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblRequestor" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top">Tel</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblReqTel" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap>Email</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblReqEmail" runat="server" Width="104px"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
								<TD class="emptycol" width="50%">
									<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" width="40%"><STRONG>PR No. </STRONG>
											</TD>
											<TD vAlign="top" align="center" width="5%">:</TD>
											<TD class="emptycol" vAlign="top" width="60%"><asp:label id="lblPONo" runat="server" Width="145px" Font-Bold="True"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top"><STRONG>Date&nbsp; </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblDate" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top"><STRONG>Status</STRONG></TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblPrStatus" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap><STRONG>Payment Terms</STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblPayTerm" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap><STRONG>Payment Method </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblPayMethod" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap><STRONG>Shipment Terms </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblShipTerm" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap><STRONG>Shipment Mode </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblShipMode" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top"><STRONG>Ship Via </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblShipVia" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap><STRONG>Freight Terms </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblFreighTerm" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top"><STRONG>Currency</STRONG></TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblCurrency" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top"><STRONG>Exchange Rate</STRONG></TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblRate" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<tr>
					<td colSpan="2">&nbsp;</td>
				</tr>
				<TR>
					<TD width="50%">
						<table class="alltable" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD vAlign="top" width="50%">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" width="46%"><STRONG>Attention To </STRONG>
											</TD>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<TD class="emptycol" vAlign="top" width="50%"><asp:label id="lblAttn" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" nowrap><STRONG>(Order Recipient Name)</STRONG>
											</TD>
											<TD vAlign="top" align="center" width="4%">&nbsp;</TD>
											<TD class="emptycol" vAlign="top" width="60%">&nbsp;</TD>
										</TR>
										<TR>
											<TD vAlign="top"><STRONG>Approved by</STRONG></TD>
											<TD vAlign="top" align="center">:</TD>
											<TD class="emptycol" vAlign="top"><asp:label id="lblAppBy" runat="server"></asp:label></TD>
										</TR>
									</table>
								</TD>
								<td width="50%">&nbsp;</td>
							</TR>
							<TR>
					<TD class="emptycol" colspan=2>&nbsp;</TD>
				</TR>
				<TR>
								<TD vAlign="top" width="50%">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD colSpan="3">
												<p><STRONG>To :</STRONG><br>
													<asp:label id="lblVendorName" runat="server" Font-Bold="True"></asp:label><BR>
													<asp:label id="lblVAddr" runat="server"></asp:label></p>
											</TD>
										</TR>
										<tr>
											<TD vAlign="top" width="46%">Business Reg. No.</TD>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<TD vAlign="top" width="50%"><asp:label id="lblVendorReg" runat="server"></asp:label></TD>
										</tr>
										<TR>
											<TD vAlign="top">Tel</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblVendorTel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap>Email</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblVendorEmail" runat="server"></asp:label></TD>
										</TR>
									</table>
								</TD>
								<td vAlign="top" width="50%"><STRONG>Bill To</STRONG> :<br>
									<STRONG>
										<asp:label id="lblBillCompName" runat="server"></asp:label></STRONG><br>
									<asp:label id="lblBillTo" runat="server"></asp:label></td>
							</TR>
						</table>
					</TD>
				</TR>
				<tr>
					<td colSpan="2">&nbsp;</td>
				</tr>
				<TR>
					<TD width="50%">
						<TABLE class="alltable" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD vAlign="top" width=23%><STRONG>Remarks</STRONG></TD>
								<TD vAlign="top" align="center" width=2%>:</TD>
								<TD class="emptycol" vAlign="top" width=75%><asp:label id="lblHeaderRemarks" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgShopping" runat="server" Width="100%" AutoGenerateColumns="False" BorderWidth="0px"
							HeaderStyle-Font-Bold="True" HeaderStyle-Font-Size="10pt" HeaderStyle-VerticalAlign="Bottom">
							<HeaderStyle Font-Size="10pt" Font-Bold="True" VerticalAlign="Bottom"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="PRD_PR_LINE" HeaderText="Line">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_PRODUCT_DESC" HeaderText="Description">
									<HeaderStyle Width="34%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_MIN_ORDER_QTY" HeaderText="MOQ">
									<HeaderStyle HorizontalAlign="Right" Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_MIN_PACK_QTY" HeaderText="MPQ">
									<HeaderStyle HorizontalAlign="Right" Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_ORDERED_QTY" HeaderText="Qty">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_UOM">
									<HeaderStyle Width="1%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_UNIT_COST" HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_UNIT_COST" HeaderText="Amt">
									<HeaderStyle HorizontalAlign="Right" Width="11%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Tax">
									<HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_ETD" HeaderText="EDD(Days)">
									<HeaderStyle HorizontalAlign="Right" Width="6%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_WARRANTY_TERMS" HeaderText="W.T.(Mths)">
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
					<TD class="emptycol">EDD = Estimated Delivery Date (Days)&nbsp;
						<BR>
						W.T. = Warranty Terms
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<HR>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"><font style="FONT-SIZE: 7pt">This is a computer generated 
							document. No signature is required.</font></TD>
				</TR>
			</TABLE>
			&nbsp;
		</form></FORM>
	</body>
</HTML>
