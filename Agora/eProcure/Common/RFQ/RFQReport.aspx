<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="RFQReport.aspx.vb" Inherits="eProcure.RFQReport1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Preview RFQ Report</title>
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
								<TD width="50%"><asp:image id="Image2" runat="server" ImageAlign="Left" ImageUrl="../Images/logo_tx123_2.jpg"
										BorderStyle="None" Width="140px"></asp:image></TD>
								<TD class="header" width="50%"><font size="5"><FONT size="5">Request For Quotation</FONT></font></TD>
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
								<TD vAlign="top" width="50%"><asp:label id="lblBComName" runat="server" Font-Bold="True"></asp:label><br>
									<asp:label id="lblBAddr" runat="server" Width="40%"></asp:label>
									<TABLE class="alltable" id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD width="36%" valign="top">Business Reg. No.</TD>
											<TD width="4%" vAlign="top" align="center">:</TD>
											<TD width="60%"><asp:label id="lblBusRegNo" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD valign="top">Contact Person</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD valign="top"><asp:label id="lblContact" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD valign="top">Tel
											</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD valign="top"><asp:label id="lblConNo" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD valign="top">Email</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD valign="top"><asp:label id="lblEmail" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
								<TD class="emptycol" width="50%">
									<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD vAlign="top" width="55%"><STRONG><STRONG>RFQ&nbsp;No.</STRONG> </STRONG>
											</TD>
											<TD vAlign="top" align="center" width="5%">:</TD>
											<TD class="emptycol" vAlign="top" width="60%"><asp:label id="lblRFQNo" runat="server" Font-Bold="True"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap width="55%"><STRONG>Date</STRONG>
											</TD>
											<TD vAlign="top" align="center" width="5%">:</TD>
											<TD class="emptycol" vAlign="top" width="60%"><asp:label id="lblDate" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="55%"><STRONG>RFQ Expiry Date </STRONG>
											</TD>
											<TD vAlign="top" align="center" width="5%">:</TD>
											<TD class="emptycol" vAlign="top" width="60%"><asp:label id="lblExpDate" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="55%"><STRONG>Requested Quotation Validity </STRONG>
											</TD>
											<TD vAlign="top" align="center" width="5%">:</TD>
											<TD class="emptycol" vAlign="top" width="60%"><asp:label id="lblReqQuoDate" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="55%"><STRONG>Payment Terms&nbsp; </STRONG>
											</TD>
											<TD vAlign="top" align="center" width="5%">:</TD>
											<TD class="emptycol" vAlign="top" width="60%"><asp:label id="lblPayTerm" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap width="55%"><STRONG>Payment Method </STRONG>
											</TD>
											<TD vAlign="top" align="center" width="5%">:</TD>
											<TD class="emptycol" vAlign="top" width="60%"><asp:label id="lblPayMethod" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" noWrap width="55%"><STRONG>Shipment&nbsp;Terms&nbsp; </STRONG>
											</TD>
											<TD vAlign="top" align="center" width="5%">:</TD>
											<TD class="emptycol" vAlign="top" width="60%"><asp:label id="lblShipTerm" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD vAlign="top" width="55%"><STRONG>Shipment Mode</STRONG></TD>
											<TD vAlign="top" align="center" width="5%">:</TD>
											<TD class="emptycol" width="60%"><asp:label id="lblShipMode1" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="2">
						<TABLE class="alltable" id="Table8" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD width="50%" class="emptycol"></TD>
								<TD width="50%" class="emptycol"></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="50%" colSpan="2"><STRONG>To</STRONG> :<BR>
									<asp:label id="lblSCoyName" runat="server" Font-Bold="True"></asp:label><BR>
									<asp:label id="lblSAddrTo" runat="server" Width="40%"></asp:label>
									<TABLE id="Table9" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
										<TR>
											<TD vAlign="top" width="18%">Business Reg. No.</TD>
											<TD vAlign="top" align="center" width="2%">:</TD>
											<TD vAlign="top" width="80%"><asp:label id="lblVBusRegNo" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD class="emptycol">Tel</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD><asp:label id="lblVTel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD class="emptycol">Email</TD>
											<TD vAlign="top" align="center">:</TD>
											<TD><asp:label id="lblVEmail" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
								<TD width="50%"></TD>
							</TR>
						</TABLE>
					</TD>
				<TR>
					<TD class="emptycol" colspan="2"></TD>
				</TR>
				<tr>
					<td class="emptycol" colspan="2">
						<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD vAlign="top" width="18%"><STRONG>Remarks</STRONG></TD>
								<TD vAlign="top" width="2%" align="center">:</TD>
								<TD vAlign="top" width="80%"><asp:label id="lblRemarks" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
				<TR>
					<TD class="emptycol" colspan="2"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_RFQReport" runat="server" Width="100%" BorderWidth="0px" AutoGenerateColumns="False"
							HeaderStyle-VerticalAlign="Bottom">
							<Columns>
								<asp:BoundColumn HeaderText="No.">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_Product_Desc" HeaderText="Description">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="45%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_UOM" HeaderText="UOM">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_Quantity" HeaderText="Qty">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_Delivery_Lead_Time" HeaderText="Delivery Lead Time (Days)">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_Warranty_Terms" HeaderText="W.T.<br>(Mths)">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
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
		</TD></TR></TBODY></TABLE>
	</body>
</HTML>
