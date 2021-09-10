<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PreviewDODetails.aspx.vb" Inherits="eProcure.PreviewDODetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
	<HEAD>
		<title>DODetails</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript">
	<!--
		function PopWindow(myLoc)
		{
			window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
	//-->
		</script>
		<% Response.Write(Session("WheelScript"))%>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_DO_tabs"))%>
			<TABLE class="alltable" id="tblDOHeader" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" colSpan="4" style="height: 3px"></TD>
				</TR>
				<TR>
					<TD class="tableheader" align="left" colSpan="4" style="height: 19px">&nbsp;<asp:label id="lblHeader" runat="server"><b>Delivery 
								Order Details</b></asp:label></TD>
				</TR>
				<tr>
					<td></td>
				</tr>
				<TR>
					<TD class="tablecol" align="left" width="20%">&nbsp;<STRONG>DO Number</STRONG> :</TD>
					<TD class="TableInput"><asp:label id="lblDONo" runat="server"></asp:label></TD>
					<TD class="tablecol" width="20%">&nbsp;<STRONG>Status </STRONG>:</TD>
					<TD class="TableInput" width="30%"><asp:label id="lblStatus" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol">&nbsp;<STRONG>PO Number</STRONG> :</TD>
					<TD class="TableInput"><asp:label id="lblPONo" runat="server"></asp:label></TD>
					<TD class="tablecol" width="25%">&nbsp;<STRONG>Customer Name</STRONG> :</TD>
					<TD class="TableInput" width="25%"><asp:label id="lblCustName" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" valign="top">&nbsp;<STRONG>Bill To</STRONG> :</TD>
					<TD class="TableInput" width="199"><asp:label id="lblBillTo" runat="server"></asp:label></TD>
					<TD class="tablecol" valign="top">&nbsp;<STRONG>Ship To</STRONG> :</TD>
					<TD class="TableInput"><asp:label id="lblShipTo" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" style="height: 19px">&nbsp;<STRONG>Delivery Date</STRONG> :</TD>
					<TD class="tableinput" style="height: 19px"><asp:label id="lblDevlDate" runat="server"></asp:label></TD>
					<TD class="tablecol" style="height: 19px">&nbsp;<STRONG>Our Ref No.</STRONG> :</TD>
					<TD class="tableinput" style="height: 19px"><asp:label id="lblOurRefNo" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" style="height: 19px">&nbsp;<STRONG>Payment Terms</STRONG> :</TD>
					<TD class="tableinput" style="height: 19px"><asp:label id="lblPayTerm" runat="server"></asp:label></TD>
					<TD class="tablecol" style="height: 19px">&nbsp;<STRONG>Our Ref Date</STRONG> :</TD>
					<TD class="tableinput" style="height: 19px"><asp:label id="lblOurRefDate" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol">&nbsp;<STRONG>Shipment Terms</STRONG> :</TD>
					<TD class="tableinput"><asp:label id="lblShipTerm" runat="server"></asp:label></TD>
					<TD class="tablecol">&nbsp;<STRONG>Payment Method</STRONG> :</TD>
					<TD class="tableinput"><asp:label id="lblPayMthd" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol">&nbsp;<STRONG>Air Way Bill No.</STRONG> :</TD>
					<TD class="tableinput"><asp:label id="lblAirWayBillNo" runat="server"></asp:label></TD>
					<TD class="tablecol">&nbsp;<STRONG>Shipment Mode</STRONG> :</TD>
					<TD class="tableinput"><asp:label id="lblShipMthd" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" style="HEIGHT: 19px">&nbsp;<STRONG>Freight Carrier</STRONG> :</TD>
					<TD class="tableinput" style="HEIGHT: 19px"><asp:label id="lblFrieghtCarrier" runat="server"></asp:label></TD>
					<TD class="tablecol" style="HEIGHT: 19px">&nbsp;<STRONG>Freight Amount</STRONG> :</TD>
					<TD class="tableinput" style="HEIGHT: 19px"><asp:label id="lblFreightAmt" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol">&nbsp;<STRONG>Remarks</STRONG> :</TD>
					<TD class="tableinput" colSpan="6"><asp:label id="lblRemarks" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;<asp:label id="lblExtAttach" text="File(s) Attached" runat="server"></asp:label></STRONG> :</TD>
					<TD class="tableinput" vAlign="top" colSpan="2">&nbsp;<asp:label id="lblFileAttach" runat="server"></asp:label></TD>
					<TD class="tableinput"></TD>
				</TR>
			</TABLE>
			<table width="100%">
				<TR>
					<TD class="emptycol1"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgDODtl" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:BoundColumn DataField="DOD_DO_LINE" SortExpression="DOD_DO_LINE" HeaderText="Line">
									<HeaderStyle HorizontalAlign="Right" Width="6%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Vendor_Item_Code" SortExpression="POD_Vendor_Item_Code" HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="10%" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Product_Desc" SortExpression="POD_Product_Desc" HeaderText="Item Name">
									<HeaderStyle HorizontalAlign="Left" Width="23%" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="5%" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ETD" SortExpression="POD_ETD" HeaderText="EDD (Date)">
									<HeaderStyle HorizontalAlign="Left" Width="9%" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Warranty_Terms" SortExpression="POD_Warranty_Terms" HeaderText="Warranty Terms(Mths)">
									<HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Min_Pack_Qty" SortExpression="POD_Min_Pack_Qty" HeaderText="MPQ">
									<HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Min_Order_Qty" SortExpression="POD_Min_Order_Qty" HeaderText="MOQ">
									<HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Ordered_Qty" SortExpression="POD_Ordered_Qty" HeaderText="PO Qty">
									<HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOD_SHIPPED_QTY" SortExpression="DOD_SHIPPED_QTY" HeaderText="Shipped Qty">
									<HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOD_DO_QTY" SortExpression="DOD_DO_QTY" HeaderText="GRN Qty">
									<HeaderStyle HorizontalAlign="Right" Width="4%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOD_REMARKS" SortExpression="DOD_REMARKS" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="20%" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="POM_PO_Date" SortExpression="POM_PO_Date" HeaderText="Line">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
			</table>
			<table width="100%" runat="server" id="tblDOSumm">
				<tr>
					<td><b>Delivery Order Summary For Purchase Order</b> :
						<asp:label id="lblSummPONum" runat="server">Label</asp:label></td>
				</tr>
				<tr>
					<td><asp:datagrid id="DtgDoSumm" runat="server">
							<Columns>
								<asp:BoundColumn DataField="date_created" HeaderText="DO Date">
									<HeaderStyle HorizontalAlign="Left" Width="13%" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="DO Number">
									<HeaderStyle HorizontalAlign="Left" Width="35%" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkDONum"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="UM_USER_NAME" HeaderText="Created By">
									<HeaderStyle HorizontalAlign="Left" Width="52%" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>											
			</table>
			<TR>
				<TD style="HEIGHT: 17px"><asp:Button ID="cmdClose" runat="server" Text="Close" CssClass="button" /></TD>
			</TR>
		</form>
	</body>
</HTML>
