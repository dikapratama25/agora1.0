<%@ Page Language="vb" AutoEventWireup="false" Codebehind="WebForm1.aspx.vb" Inherits="eProcure.WebForm1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>WebForm1</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio.NET 7.0">
		<meta name="CODE_LANGUAGE" content="Visual Basic 7.0">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            
        </script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:DataGrid id="DataGrid1"  DataMember="album" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Horizontal" Width="503px" AutoGenerateColumns="False">
				<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
				<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
				<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
				<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
				<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
				<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
				<Columns>
					<asp:TemplateColumn ItemStyle-Width="9">
						<ItemTemplate>
							<asp:ImageButton ImageUrl="#" CommandName="Expand" ID="btnExpand" Runat="server"></asp:ImageButton>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="AM_ACCT_INDEX" HeaderText="AM_ACCT_INDEX" />
					<asp:BoundColumn DataField="AM_ACCT_INDEX" HeaderText="AM_ACCT_INDEX" />
					<asp:BoundColumn DataField="AM_ACCT_INDEX" HeaderText="AM_ACCT_INDEX" />
					<asp:TemplateColumn>
						<ItemStyle Width="1" />
						<ItemTemplate>
							<asp:PlaceHolder ID="ExpandedContent" Visible="False" Runat="server">
								</td></tr>
								<tr>
									<td width="9">&nbsp;</td>
									<td colspan="3">
										<asp:DataGrid id="Datagrid2" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Horizontal" Width="503px" AutoGenerateColumns="False">
											<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
											<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
											<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
											<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
											<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
											<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
											<Columns>
												<asp:BoundColumn DataField="AM_ACCT_INDEX" HeaderText="AM_ACCT_INDEX" />
												<asp:BoundColumn DataField="AM_ACCT_INDEX" HeaderText="AM_ACCT_INDEX" />
											</Columns>
										</asp:DataGrid>
							</asp:PlaceHolder>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid>
		</form>
	</body>
</HTML>
