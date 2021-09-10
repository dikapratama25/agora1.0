<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="GRNDetails.aspx.vb" Inherits="eProcure.GRNDetails" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>GRNDetails</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
	<!--
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
	//-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchGRN_tabs"))%>
			<TABLE class="alltable" id="tblDOHeader" cellSpacing="0" cellPadding="0" border="0">				
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD class="tableheader" colSpan="4">&nbsp;<asp:label id="lblHeader" runat="server">Goods Receipt Note Details</asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" width="20%"><strong>&nbsp;PO Number</strong> :</TD>
					<TD class="tableinput" width="20%"><asp:label id="lblPONo" runat="server"></asp:label></TD>
					<TD class="tablecol" width="30%">&nbsp;<strong>DO Number </strong>:</TD>
					<TD class="tableinput" width="30%"><asp:label id="lblDONo" runat="server"></asp:label></TD>
				</TR>				
				<TR>
					<TD class="tablecol" width="20%"><strong>&nbsp;GRN Number </strong>:</TD>
					<td class="tableinput" width="20%"><asp:label id="lblGrnNo" runat="server"></asp:label></td>					
					<TD class="tablecol" width="30%"><strong>&nbsp;Created By </strong>:</TD>
					<td class="tableinput" width="30%"><asp:label id="lblCreatedBy" runat="server"></asp:label></td>
				</TR>
				<TR>				    
					<TD class="tablecol" width="20%"><strong>&nbsp;GRN Date </strong>:</TD>
					<td class="tableinput" width="20%"><asp:label id="lblGRNDate" runat="server"></asp:label></td>
					<TD class="tablecol" width="30%"><strong>&nbsp;Actual Goods Received Date </strong>:</TD>
					<td class="tableinput" width="30%"><asp:label id="lblDtREceived" runat="server"></asp:label></td>
				</TR>		
				<TR>
					<TD class="emptycol" colspan="4">&nbsp;&nbsp;</TD>
				</TR>
			</TABLE>
			<TABLE class="alltable" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD><asp:datagrid id="dtgGrnDtl" runat="server" OnSortCommand="SortCommand_Click" AllowSorting="True">
							<Columns>
								<asp:BoundColumn DataField="GD_PO_LINE" SortExpression="GD_PO_LINE" HeaderText="Line" Visible="false">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Line" SortExpression="LineNo"></asp:BoundColumn>
								<asp:BoundColumn DataField="POD_VENDOR_ITEM_CODE" SortExpression="POD_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PRODUCT_DESC" SortExpression="POD_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_MIN_PACK_QTY" SortExpression="POD_MIN_PACK_QTY" HeaderText="MPQ">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" SortExpression="POD_ORDERED_QTY" HeaderText="PO Qty">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOD_SHIPPED_QTY" SortExpression="DOD_SHIPPED_QTY" HeaderText="Shipped Qty">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GD_RECEIVED_QTY" SortExpression="GD_RECEIVED_QTY" HeaderText="GRN Qty">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GD_REJECTED_QTY" SortExpression="GD_REJECTED_QTY" HeaderText="Rejected Qty">
									<HeaderStyle HorizontalAlign="Left" Width="7%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GD_REMARKS" SortExpression="GD_REMARKS" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkDetails"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD style="height: 24px">
						<INPUT type="button" value="View GRN" id="cmdPreviewGRN" runat="server" Class="button" style="width: 96px">
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
