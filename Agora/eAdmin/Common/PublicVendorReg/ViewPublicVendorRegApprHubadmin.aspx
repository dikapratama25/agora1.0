<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ViewPublicVendorRegApprHubadmin.aspx.vb" Inherits="eAdmin.ViewPublicVendorRegApprHubadmin" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ViewPublicVendorRegAppr</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ rel=""stylesheet"" />"
        </script>
        <% Response.Write(sCSS)%>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="AllTable" id="Table1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px"
				cellSpacing="0" cellPadding="0" width="300" border="0">
				<TR>
					<TD class="Header" style="HEIGHT: 17px">&nbsp;View Public Vendor Registration 
						Approval</TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				<TR>
					<TD class="TableHeader">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="TableCol" style="WIDTH: 150px"><strong>&nbsp;Company ID :</strong></TD>
								<TD class="TableInput" style="WIDTH: 200px">&nbsp;
									<asp:textbox id="txtComID" runat="server" Width="114px" CssClass="txtbox"></asp:textbox></TD>
								<TD class="TableCol"><strong>&nbsp;Company Name : &nbsp;</strong></TD>
								<TD class="TableInput">&nbsp;
									<asp:textbox id="txtComName" runat="server" Width="114px" CssClass="txtbox"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="TableCol" style="WIDTH: 150px"><strong>&nbsp;Status :</strong></TD>
								<TD class="TableInput" colSpan="3">&nbsp;
									<asp:checkbox id="chkPending" runat="server" Text="Pending For Approval"></asp:checkbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:CheckBox id="chkApprove" runat="server" Text="Approved"></asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:checkbox id="chkReject" runat="server" Text="Rejected"></asp:checkbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				<TR>
					<TD class="EmptyCol"><asp:button id="cmdSearch" runat="server" Width="57px" CssClass="button" Text="Search"></asp:button>
						<asp:button id="cmdClear" runat="server" Width="74px" CssClass="Button" Text="Clear"></asp:button></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 13px"><asp:datagrid id="dtgVendorRegAppr" runat="server" CssClass="GRID" OnPageIndexChanged="OnPageIndexChanged_Page"
							OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<HeaderStyle CssClass="gridheader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="CM_COY_ID" SortExpression="CM_COY_ID" HeaderText="Company ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Company Name"></asp:BoundColumn>
								<asp:BoundColumn DataField="CM_REG_DATE" SortExpression="CM_REG_DATE" HeaderText="Registration Date"></asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
