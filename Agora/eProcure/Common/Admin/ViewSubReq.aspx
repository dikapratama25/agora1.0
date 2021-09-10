<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ViewSubReq.aspx.vb" Inherits="eProcure.AViewSubReq" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ViewSubmittedRequests</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim startcalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Admin", "reqcalendar.aspx", "TextBox=txt_subon") & "','cal','width=180,height=155,left=270,top=180');""><IMG height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' width="16"></A>"
        </script> 
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="WIDTH: 674px; HEIGHT: 25px"><STRONG>View Submitted Requests</STRONG></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="WIDTH: 674px"></TD>
				</TR>
				<TR>
					<TD class="tablecol"><TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" width="100%">
							<TBODY>
								<TR>
									<TD class="tableheader">
										<P>Search Criteria</P>
									</TD>
								</TR>
								<TR>
									<TD class="tablecol">Type Of Request :<asp:dropdownlist id="cbo_typeofreq" runat="server" Width="104px" CssClass="ddl">
											<asp:ListItem Value="">--- SELECT ---</asp:ListItem>
											<asp:ListItem Value="UOM">UOM</asp:ListItem>
											<asp:ListItem Value="CU">CURRENCY</asp:ListItem>
											<asp:ListItem Value="CT">COUNTRY</asp:ListItem>
										</asp:dropdownlist>&nbsp;Status :<asp:dropdownlist id="cbo_status" runat="server" Width="100px" CssClass="ddl">
											<asp:ListItem Value="">--- SELECT ---</asp:ListItem>
											<asp:ListItem Value="P">PENDING</asp:ListItem>
											<asp:ListItem Value="A">ACCEPT</asp:ListItem>
											<asp:ListItem Value="R">REJECT</asp:ListItem>
										</asp:dropdownlist>&nbsp;Submitted On :<asp:textbox id="txt_subon" runat="server" Width="88px" CssClass="txtbox"></asp:textbox>
										<% Response.Write(startcalendar) %>
										<asp:button id="cmdsearch" runat="server" CssClass="Button" Text="Search"></asp:button>&nbsp;<asp:button id="cmd_clear" runat="server" CssClass="Button" Text="Clear"></asp:button></TD>
					</TD>
				</TR>
			</TABLE>
			</TD></TR>
			<TR>
				<TD class="emptycol"></TD>
			</TR>
			<TR>
				<TD class="emptycol">
					<P><asp:datagrid id="MyDataGrid" runat="server" CssClass="Grid" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
							AllowSorting="True" OnPageIndexChanged="MyDataGrid_Page">
							<AlternatingItemStyle BackColor="#f6f9fe"></AlternatingItemStyle>
							<HeaderStyle CssClass="GridHeader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="VR_ENT_DATETIME" SortExpression="VR_ENT_DATETIME" HeaderText="Submitted On">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle Width="20%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="VR_REQ_CODE" SortExpression="VR_REQ_CODE" HeaderText="Code">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle Width="30%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="VR_REQ_DESC" SortExpression="VR_REQ_DESC" HeaderText="Description">
									<HeaderStyle Width="50%"></HeaderStyle>
									<ItemStyle Width="30%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="VR_REMARK" SortExpression="VR_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemStyle Width="20%"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right" CssClass="gridPager"
								Mode="NumericPages"></PagerStyle>
						</asp:datagrid></P>
				</TD>
			</TR>
			<TR>
				<TD class="emptycol" style="WIDTH: 674px"></TD>
			</TR>
			<TR>
				<TD class="emptycol" style="WIDTH: 674px"><A href="#"><STRONG>&lt; Back</STRONG></A></TD>
			</TR>
			</TBODY></TABLE></form>
	</body>
</HTML>
