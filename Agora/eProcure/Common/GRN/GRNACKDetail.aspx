<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="GRNACKDetail.aspx.vb" Inherits="eProcure.GRNACKDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>GRNACKDetail</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="tblDOHeader" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="header"><FONT size="1">&nbsp;</FONT><asp:label id="lblTitle" runat="server">Receiving History</asp:label></TD>
				</TR>
				<TR>
				<TR>
					<TD class="tablecol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgAckDtl" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:BoundColumn DataField="GL_LEVEL" SortExpression="GL_LEVEL" HeaderText="GRN Level">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ACTION_NAME" SortExpression="ACTION_NAME" HeaderText="Created By">
									<HeaderStyle HorizontalAlign="Left" Width="18%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GL_ACTION_DT" SortExpression="GL_ACTION_DT" HeaderText="Created On">
									<HeaderStyle HorizontalAlign="Left" Width="11%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GL_RECEIVED_QTY" SortExpression="GL_RECEIVED_QTY" HeaderText="Receive Qty">
									<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GL_REJECTED_QTY" SortExpression="GL_REJECTED_QTY" HeaderText="Reject Qty">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GL_REMARKS" SortExpression="GL_REMARKS" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="39%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
