<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="CR_DETAIL.aspx.vb" Inherits="eProcure.CR_DETAILFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CR_DETAIL</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_CRDetail_tabs"))%>
			<TABLE class="alltable" id="Table10" cellSpacing="0" cellPadding="0" border="0">
            <tr>
					<TD class="header" colSpan="4" style="height: 7px"></TD>
			</TR>
			</Table>
			<TABLE class="ALLTABLE" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header"><asp:label id="Label1" runat="server">Cancellation Request Line Detail</asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"><STRONG>CR No.</STRONG> :<asp:label id="lblCRNo" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 14px"><STRONG>CR Remarks</STRONG> :
						<asp:label id="lblCRRemarks" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_POList" runat="server" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="POD_PO_LINE" SortExpression="POD_PO_LINE" HeaderText="Line" Visible="false">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Line" SortExpression="LineNo"></asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PRODUCT_DESC" SortExpression="POD_PRODUCT_DESC" HeaderText="Description">
									<HeaderStyle Width="35%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_MIN_PACK_QTY" SortExpression="POD_MIN_PACK_QTY" HeaderText="MPQ">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_WARRANTY_TERMS" SortExpression="POD_WARRANTY_TERMS" HeaderText="Warranty Terms">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" SortExpression="POD_ORDERED_QTY" HeaderText="Ordered">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_RECEIVED_QTY" SortExpression="POD_RECEIVED_QTY" HeaderText="Receive Qty">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_REJECTED_QTY" SortExpression="POD_REJECTED_QTY" HeaderText="Reject Qty">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Outstanding">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PCD_CANCELLED_QTY" SortExpression="PCD_CANCELLED_QTY" HeaderText="Qty To Cancel">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PCD_REMARKS" SortExpression="PCD_REMARKS" HeaderText="Remarks">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><A id="back" href="#" runat="server"><strong>&lt; Back</strong></A>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
