<%@ Page Language="vb" AutoEventWireup="false" Codebehind="POViewB3.aspx.vb" Inherits="eProcure.POViewB3" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>POViewB3</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="tableCOL">&nbsp;<STRONG>PR Number</STRONG> :&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:label id="LblPrNo" Width="88px" Runat="server"></asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					</TD>
				</TR>
			</TABLE>
			<TABLE class="AllTable" id="tblSearchResult" cellSpacing="0" cellPadding="0" width="100%"
				border="0" runat="server">
				<TR>
					<TD class="emptycol" colSpan="5"></TD>
				</TR>
				<TR>
					<TD colSpan="5"><asp:datagrid id="dtg_POList" runat="server" AutoGenerateColumns="False" DataKeyField="POM_PO_NO"
							OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_POList_Page">
							<Columns>
								<asp:TemplateColumn SortExpression="POM_PO_No" HeaderText="PO NO.">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle VerticalAlign="Middle"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkPRNo" NavigateUrl="#"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="POM_PO_DATE" SortExpression="POM_PO_DATE" HeaderText="Creation Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POM_S_COY_NAME" SortExpression="POM_S_COY_NAME" HeaderText="Vendor Name">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POM_ACCEPTED_DATE" SortExpression="POM_ACCEPTED_DATE" HeaderText="PO Accepted Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="PO Status">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Remark1" SortExpression="Remark1" HeaderText="Fulfilment Status">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"><A id="back" href="#" runat="server"><strong>&lt; Back</strong></A></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
