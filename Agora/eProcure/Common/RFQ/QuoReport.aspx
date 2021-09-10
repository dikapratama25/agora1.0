<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="QuoReport.aspx.vb" Inherits="eProcure.QuoReport" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Preview Quotation Report</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD width="50%"><asp:image id="Image1" runat="server" ImageUrl="../Images/logo_tx123_2.jpg" Width="140px"></asp:image></TD>
					<TD class="header" width="50%" vAlign="middle"><FONT size="5">Quotation </FONT>
					</TD>
				</TR>
				<TR>
					<TD colSpan="2" class="emptycol">&nbsp;</TD>
				</TR>
				<TR>
					<TD vAlign="top" width="50%"><asp:label id="lblComName" runat="server" Font-Bold="True"></asp:label><br>
						<asp:label id="lblAddr" runat="server"></asp:label>
						<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD vAlign="top" width="36%">Business Reg. No.</TD>
								<TD vAlign="top" align="center" width="4%">:</TD>
								<TD vAlign="top" width="60%"><asp:label id="lblBusRegNo" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">Tel</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lbl_Contact" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">Email</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblEmail" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
						<br>
					</TD>
					<TD vAlign="top" width="50%">
						<TABLE class="alltable" id="Table5" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD vAlign="top" width="36%"><STRONG>Quotation No.</STRONG></TD>
								<TD vAlign="top" align="center" width="4%">:</TD>
								<TD vAlign="top" width="80%"><asp:label id="lblQuoNo" runat="server" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top"><B>Date</B>
								</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblDate" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top"><B>Quotation Validity</B></TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblvalidtill" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top"><B>Payment Terms</B>
								</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblPayTerm" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top"><B>Payment Method </B>
								</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lblPayMethod" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top"><STRONG>Shipment Terms</STRONG></TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lbl_st" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top"><STRONG>Shipment Mode</STRONG></TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lbl_sm" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"></TD>
				</TR>
				<TR>
					<TD vAlign="top" width="50%"><STRONG>To </STRONG>:<br>
						<asp:label id="lblBCoyName" runat="server" Font-Bold="True"></asp:label><br>
						<asp:label id="lblBillTo" runat="server" Width="150px"></asp:label><br>
						<TABLE class="alltable" id="Table6" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD vAlign="top" noWrap width="36%">Business Reg. No.</TD>
								<TD vAlign="top" align="center" width="4%">:</TD>
								<TD vAlign="top" noWrap width="60%"><asp:label id="lbl_bRegNo" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">Contact Person</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top" noWrap><asp:label id="lbl_bcontact" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" noWrap>Tel</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top"><asp:label id="lbl_bconnum" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top">Email</TD>
								<TD vAlign="top" align="center">:</TD>
								<TD vAlign="top" noWrap><asp:label id="lbl_bemail" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
					<TD width="50%"></TD>
				</TR>
				<TR>
					<TD colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<td width="100%" colspan="2">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<TD vAlign="top" noWrap width="18%"><STRONG>Vendor Remarks</STRONG>
								<TD vAlign="top" align="center" width="2%">:</TD>
								<TD vAlign="top" width="80%"><asp:label id="lbl_Vremark" runat="server"></asp:label></TD>
							</tr>
						</table>
					</td>
				</TR>
				<TR>
					<TD colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD colSpan="2"><asp:datagrid HeaderStyle-Font-Bold="True" HeaderStyle-Font-Size="10pt" id="dg_viewitem" runat="server"
							BorderWidth="0px" GridLines="None" AutoGenerateColumns="False">
							<HeaderStyle Font-Bold="True" VerticalAlign="Bottom"></HeaderStyle>
							<Columns>
								<asp:BoundColumn HeaderText="No.">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RRD_Vendor_Item_Code" HeaderText="Vendor Item Code ">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RRD_Product_Desc"  readonly="true"  HeaderText="Description">
									<HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RRD_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RRD_Quantity" HeaderText="Qty">
									<HeaderStyle HorizontalAlign="Right" Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RRD_Tolerance" HeaderText="Qty Tolerance">
									<HeaderStyle HorizontalAlign="Right" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="Right" Width="17%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_unit_price" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Amt">
									<HeaderStyle HorizontalAlign="Right" Width="7%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_price" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Tax">
									<HeaderStyle HorizontalAlign="Right" Width="7%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_tax" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="MOQ">
									<HeaderStyle HorizontalAlign="Right" Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_moq" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="MPQ">
									<HeaderStyle HorizontalAlign="Right" Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_mpq" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Delivery Lead Time(Days)">
									<HeaderStyle HorizontalAlign="Right" Width="8%" VerticalAlign="Top"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_Delivery" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="W.T. (Mths) ">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_warranty" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2">
						W.T.&nbsp;= Warranty Terms&nbsp;
						<HR>
						<div class="div">This is a computer generated document. No signature is required.</div>
						&nbsp;</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
