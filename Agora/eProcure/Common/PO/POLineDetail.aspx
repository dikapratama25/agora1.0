<%@ Page Language="vb" AutoEventWireup="false" Codebehind="POLineDetail.aspx.vb" Inherits="eProcure.POLineDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>POLineDetail</title>
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
		<script>
			function winopen(link)
			{
				window.location.href = link;
			}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="ALLTABLE" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header"><asp:label id="Label1" runat="server">PO Line Details</asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tablecol"><STRONG>&nbsp;PO Number</STRONG> :<STRONG> </STRONG>
						<asp:label id="lbl_Po_No" runat="server"></asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<STRONG>Order Date</STRONG> :<STRONG> </STRONG>
						<asp:label id="lbl_date" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_POList" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_POList_Page"
							AutoGenerateColumns="False" CssClass="Grid">
							<HeaderStyle HorizontalAlign="Left" CssClass="GridHeader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn HeaderText="Line" SortExpression="POD_PO_Line">
									<HeaderStyle HorizontalAlign="right" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_VENDOR_ITEM_CODE" SortExpression="POD_VENDOR_ITEM_CODE" HeaderText="Vendor Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PRODUCT_DESC" SortExpression="POD_PRODUCT_DESC" HeaderText="Description">
									<HeaderStyle HorizontalAlign="Left" Width="24%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" SortExpression="POD_ETD" HeaderText="EDD"></asp:BoundColumn>
								<asp:BoundColumn DataField="POD_MIN_PACK_QTY" SortExpression="POD_MIN_PACK_QTY" HeaderText="MPQ">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_WARRANTY_TERMS" SortExpression="POD_WARRANTY_TERMS" HeaderText="Warranty Terms">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" SortExpression="POD_ORDERED_QTY" HeaderText="Ordered">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="POD_PR_LINE" HeaderText="Outstanding">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_RECEIVED_QTY" SortExpression="POD_RECEIVED_QTY" HeaderText="Receive Qty">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_REJECTED_QTY" SortExpression="POD_REJECTED_QTY" HeaderText="Reject Qty">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_REMARK" SortExpression="POD_REMARK" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="21%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="header"><asp:label id="lbl_do_grn" runat="server">DO And GRN Summary</asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_doc" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_POList_Page"
							AutoGenerateColumns="False" AllowSorting="True">
							<Columns>
								<asp:BoundColumn SortExpression="DOM_DO_NO" HeaderText="DO NO.">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="CREATIONDATE" HeaderText="DO Creation Date">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="SUBMITIONDATE" HeaderText="DO Submission Date">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DO_CREATED_BY" SortExpression="DO_CREATED_BY" HeaderText="DO Created By">
									<HeaderStyle HorizontalAlign="Left" Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="GM_GRN_NO" HeaderText="GRN No.">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="GM_CREATED_DATE" HeaderText="GRN Date">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="GM_DATE_RECEIVED" HeaderText="GRN Received Date">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GM1_CREATED_BY" SortExpression="GM1_CREATED_BY" HeaderText="GRN Created by">
									<HeaderStyle HorizontalAlign="Left" Width="17%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="header"><asp:label id="lbl_cancel" runat="server">Cancellation Request Summary</asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_cr" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_POList_Page"
							AutoGenerateColumns="False" AllowSorting="True">
							<Columns>
								<asp:BoundColumn SortExpression="PCM_CR_NO" HeaderText="CR NO.">
									<HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="PCM_REQ_DATE" HeaderText="CR Creation Date">
									<HeaderStyle HorizontalAlign="Left" Width="16%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PCM_REQ_BY" SortExpression="PCM_REQ_BY" HeaderText="CR Create By">
									<HeaderStyle HorizontalAlign="Left" Width="54%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="PCM_CR_NO">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4">&nbsp;<A id="back" href="#" runat="server"><strong>&lt; 
								Back</strong></A></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
