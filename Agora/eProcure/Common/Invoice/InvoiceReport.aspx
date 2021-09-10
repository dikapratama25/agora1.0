<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="InvoiceReport.aspx.vb" Inherits="eProcure.InvoiceReport" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Preview Invoice Report</title>
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
								<TD width="50%"><asp:image id="Image1" runat="server" ImageAlign="Left" ImageUrl="#" BorderStyle="None" Width="140px"></asp:image></TD>
								<TD class="header" width="50%"><font size="5">Invoice</font></TD>
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
								<TD vAlign="top" width="50%"><asp:label id="lblComName" runat="server" Font-Bold="True"></asp:label><br>
									<asp:label id="lblAddr" runat="server"></asp:label>
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" width="36%">Business Reg. No.</TD>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<TD vAlign="top" width="60%"><asp:label id="lblBusRegNo" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top">Tel
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblTel" runat="server" Width="145px"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top">Email</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblEmail" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
								<TD class="emptycol" width="50%">
									<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" width="36%"><STRONG><STRONG>Invoice No.</STRONG> </STRONG>
											</TD>
											<TD vAlign="top" align="center" width="4%">:</TD>
											<TD vAlign="top" width="60%"><asp:label id="lblInvNo" runat="server" Font-Bold="True"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap><STRONG>Date</STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblDate" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top"><STRONG>Our Ref.</STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblOurRef" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top"><STRONG>Your Ref.</STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblYourRef" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top"><STRONG>Payment Terms</STRONG>
											</TD>
											<TD vAlign="top" align="center" width="5%">:</TD>
											<TD vAlign="top"><asp:label id="lblPayTerm" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap><STRONG>Payment Method </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblPayMethod" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap><STRONG>Shipment&nbsp;Terms&nbsp; </STRONG>
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD vAlign="top"><asp:label id="lblShipType" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top"><STRONG>Shipment Mode</STRONG></TD>
											<TD vAlign="top" align="center" width="5%">:</TD>
											<TD vAlign="top"><asp:label id="lblShipMode" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<tr>
					<td><STRONG>Bill To</STRONG> :<br>
						<asp:label id="lblBCoyName" runat="server" Font-Bold="True"></asp:label><br>
						<asp:label id="lblBillTo" runat="server"></asp:label></td>
				</tr>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<tr>
					<td class="emptycol">
						<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD vAlign="top" width="18%"><STRONG>Vendor Remarks</STRONG></TD>
								<TD vAlign="top" align="center" width="2%">:</TD>
								<TD vAlign="top" width="80%"><asp:label id="lblVenRemarks" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_PrevInvoice" runat="server" Width="100%" BorderWidth="0px" AutoGenerateColumns="False"
							HeaderStyle-VerticalAlign="Bottom">
							<HeaderStyle VerticalAlign="Bottom"></HeaderStyle>
							<Columns>
								<asp:BoundColumn HeaderText="No.&#160;">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="6%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_PRODUCT_DESC" SortExpression="ID_PRODUCT_DESC" HeaderText="Description">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="32%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_UOM" SortExpression="ID_UOM" HeaderText="UOM">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_RECEIVED_QTY" SortExpression="ID_RECEIVED_QTY" HeaderText="Qty">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="7%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="ID_UNIT_COST" HeaderText="Unit Price">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Amt">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_GST" SortExpression="ID_GST" HeaderText="Tax">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_WARRANTY_TERMS" SortExpression="ID_WARRANTY_TERMS" HeaderText="W.T.<br>(Mths)">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">W.T. = Warranty Terms</TD>
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
		</form>
	</body>
</HTML>
