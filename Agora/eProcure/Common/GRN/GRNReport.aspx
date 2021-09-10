<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="GRNReport.aspx.vb" Inherits="eProcure.GGRNReport" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Preview GRN Report</title>
		<meta content="False" name="vs_snapToGrid">
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD width="50%"><asp:image id="Image1" runat="server" ImageAlign="Left" ImageUrl="../Images/logo_tx123_2.jpg"
							BorderStyle="None" Width="140px"></asp:image></TD>
					<TD width="50%"><font size="5"><STRONG>Goods Receipt Note</STRONG></font></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD vAlign="top" width="50%"><asp:label id="lblComName" runat="server" Font-Bold="True"></asp:label><br>
						<asp:label id="lblComAddr" runat="server"></asp:label>
						<TABLE id="tb_user" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
							<TR>
								<TD vAlign="top" width="36%">Business Reg. No.
								</TD>
								<TD vAlign="top" align="center" width="4%">:</TD>
								<TD vAlign="top" width="60%"><asp:label id="lblBusReg" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">Received By</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblRecipiet" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">Tel</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblTelReceived" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">Email</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblEmailReceived" runat="server"></asp:label></TD>
							</TR>
							<TR id="lblVerified" runat="server">
								<TD vAlign="top" nowrap>Verified By</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblVerifiedBy" runat="server"></asp:label></TD>
							</TR>
							<TR id="lblTel" runat="server">
								<TD vAlign="top">Tel</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblTelVerified" runat="server"></asp:label></TD>
							</TR>
							<TR id="lblEmail" runat="server">
								<TD vAlign="top">Email</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblEmailVerified" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
					<TD vAlign="top" width="50%">
						<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD vAlign="top" width="36%"><STRONG>GRN No.</STRONG>
								</TD>
								<TD vAlign="top" align="center" width="4%">:</TD>
								<TD vAlign="top" width="60%"><asp:label id="lblGRNNo" runat="server" Width="145px" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top"><STRONG> Date</STRONG></TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblCreateOn" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" nowrap><STRONG>Goods Received Date</STRONG></TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblGRecOn" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top"><STRONG>Requestor</STRONG></TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblRequestor" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top"><STRONG>Attention To</STRONG></TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblAttention" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top"><STRONG>PO No.</STRONG></TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblPONo" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top"><STRONG>DO No.</STRONG></TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblDONo" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD vAlign="top" colSpan="2"><STRONG>Ship To</STRONG> :<br>
						<asp:label id="lblOrganiz" runat="server" Font-Bold="True"></asp:label><br>
						<asp:label id="lblDelAddr" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD colSpan="2"><asp:datagrid id="dtgPOReport" runat="server" HeaderStyle-VerticalAlign="Bottom" HeaderStyle-Font-Size="10pt"
							HeaderStyle-Font-Bold="True" AutoGenerateColumns="False" GridLines="None">
							<HeaderStyle Font-Size="10pt" Font-Bold="True" VerticalAlign="Bottom"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="GD_PO_LINE" SortExpression="GD_PO_LINE" HeaderText="Line Item">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GD_PO_LINE" SortExpression="GD_PO_LINE">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_VENDOR_ITEM_CODE" SortExpression="POD_VENDOR_ITEM_CODE" HeaderText="Vendor Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Product_Desc" SortExpression="POD_Product_Desc" HeaderText="Description">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="33%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Width="6%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_MIN_PACK_QTY" SortExpression="POD_MIN_PACK_QTY" HeaderText="MPQ">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" SortExpression="POD_ORDERED_QTY" HeaderText="Ordered Qty">
									<HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_DELIVERED_QTY" SortExpression="POD_DELIVERED_QTY" HeaderText="Delivered Qty">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GD_RECEIVED_QTY" SortExpression="GD_RECEIVED_QTY" HeaderText="Received Qty">
									<HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GD_REJECTED_QTY" SortExpression="GD_REJECTED_QTY" HeaderText="Rejected Qty">
									<HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<HR>
					</TD>
				</TR>
				<TR>
					<TD colSpan="2"><font style="FONT-SIZE: 7pt">This is computer generated document. No 
							signature is required.</font>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
