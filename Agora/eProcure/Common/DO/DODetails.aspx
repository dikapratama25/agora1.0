<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="DODetails.aspx.vb" Inherits="eProcure.DODetails" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>DODetails</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script type="text/javascript">
	<!--
		function PopWindow(myLoc)
		{
			window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
	//-->
		</script>
		<% Response.Write(Session("WheelScript"))%>
		
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_DO_tabs"))%>
			<table class="alltable" id="tblDOHeader" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td class="header" colspan="4" style="height: 3px"></td>
				</tr>
				<tr>
					<td class="tableheader" align="left" colspan="4" style="height: 19px">&nbsp;<asp:label id="lblHeader" runat="server"><b>Delivery 
								Order Details</b></asp:label></td>
				</tr>
				<tr>
					<td></td>
				</tr>
				<tr>
					<td class="tablecol" align="left" width="20%">&nbsp;<strong>DO Number</strong> :</td>
					<td class="TableInput"><asp:label id="lblDONo" runat="server"></asp:label></td>
					<td class="tablecol" width="20%">&nbsp;<strong>Status </strong>:</td>
					<td class="TableInput" width="30%"><asp:label id="lblStatus" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>PO Number</strong> :</td>
					<td class="TableInput"><asp:label id="lblPONo" runat="server"></asp:label></td>
					<td class="tablecol" width="25%">&nbsp;<strong>Customer Name</strong> :</td>
					<td class="TableInput" width="25%"><asp:label id="lblCustName" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" valign="top">&nbsp;<strong>Bill To</strong> :</td>
					<td class="TableInput" width="199"><asp:label id="lblBillTo" runat="server"></asp:label></td>
					<td class="tablecol" valign="top">&nbsp;<strong>Ship To</strong> :</td>
					<td class="TableInput"><asp:label id="lblShipTo" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Delivery Date</strong> :</td>
					<td class="tableinput"><asp:label id="lblDevlDate" runat="server"></asp:label></td>
					<td class="tablecol">&nbsp;<strong>Our Ref No.</strong> :</td>
					<td class="tableinput"><asp:label id="lblOurRefNo" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Payment Terms</strong> :</td>
					<td class="tableinput"><asp:label id="lblPayTerm" runat="server"></asp:label></td>
					<td class="tablecol">&nbsp;<strong>Our Ref Date</strong> :</td>
					<td class="tableinput"><asp:label id="lblOurRefDate" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Shipment Terms</strong> :</td>
					<td class="tableinput"><asp:label id="lblShipTerm" runat="server"></asp:label></td>
					<td class="tablecol">&nbsp;<strong>Payment Method</strong> :</td>
					<td class="tableinput"><asp:label id="lblPayMthd" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Air Way Bill No.</strong> :</td>
					<td class="tableinput"><asp:label id="lblAirWayBillNo" runat="server"></asp:label></td>
					<td class="tablecol">&nbsp;<strong>Shipment Mode</strong> :</td>
					<td class="tableinput"><asp:label id="lblShipMthd" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" style="HEIGHT: 19px">&nbsp;<strong>Freight Carrier</strong> :</td>
					<td class="tableinput" style="HEIGHT: 19px"><asp:label id="lblFrieghtCarrier" runat="server"></asp:label></td>
					<td class="tablecol" style="HEIGHT: 19px">&nbsp;<strong>Freight Amount</strong> :</td>
					<td class="tableinput" style="HEIGHT: 19px"><asp:label id="lblFreightAmt" runat="server"></asp:label></td>
				</tr>
				<tr id="tr_delTerm" runat="server">
					<td class="tablecol">&nbsp;<strong>Delivery Term</strong> :</td>
					<td class="tableinput" colspan="6"><asp:label id="lblDelTerm" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Remarks</strong> :</td>
					<td class="tableinput" colspan="6"><asp:label id="lblRemarks" runat="server"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tablecol"><strong>&nbsp;<asp:label id="lblExtAttach" text="File(s) Attached" runat="server"></asp:label></strong> :</td>
					<td class="tableinput" valign="top" colspan="3">&nbsp;<asp:label id="lblFileAttach" runat="server"></asp:label></td>
					<td class="tableinput"></td>
				</tr>
			</table>
			<table width="100%">
				<tr>
					<td class="emptycol1"></td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtgDODtl" runat="server" OnSortCommand="SortCommand_Click">
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
								<asp:TemplateColumn HeaderText="Total Lot No">
								    <HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
								        <asp:label id="lblLotNo" runat="server"></asp:label>
								    </ItemTemplate>
								</asp:TemplateColumn>
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
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
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
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
				<tr>
					<td>
						<input type="button" value="View DO" id="cmdPreviewDO1" runat="server" class="button" style="width: 75px"/>
					</td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol">
					    <asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="#"><strong>&lt; Back</strong></asp:hyperlink>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
