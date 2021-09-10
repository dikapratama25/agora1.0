<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DashboardMatrix.aspx.vb" Inherits="eAdmin.DashboardMaint" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Dashboard</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>  
        <% Response.Write(css)%>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" runat="server">
		<%Response.Write(Session("Dashboard_tabs"))%>
			<TABLE class="AllTable" id="Table1" cellSpacing="0" cellPadding="0">
				<TR>
					<TD class="Header"><asp:label id="lblTitle" runat="server">Dashboard Matrix</asp:label></TD>
				</TR>
				<tr>
	            <td class="EmptyCol" style="padding-bottom:4px;"><asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Choose a User role to change the dashboard matrix."></asp:label></td>
				</tr>
				<TR>
					<TD class="TableHeader"><asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
				<TR>
				<TD class="TableCol"><STRONG>User Role</STRONG>:
                    <asp:DropDownList ID="ddlUserRole" cssclass="ddl" runat="server">
                    </asp:DropDownList></td>
				</TR>
				<tr>
				<td height="10px" class="TableCol"></td>
				</tr>
				</TABLE>
			<table class="AllTable" id="Table5" cellSpacing="0" cellPadding="0">
				<TR>
					<TD class="EmptyCol"><asp:datagrid id="dtgDashboardMaint" OnSortCommand="SortCommand_Click" runat="server">
							<Columns>
                                <asp:BoundColumn DataField="DM_PANEL_ID" HeaderText="Dashboard ID" Visible="False" >
                                <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                </asp:BoundColumn>
								<asp:BoundColumn DataField="DM_PANEL_NAME" HeaderText="Dashboard Panel Name" SortExpression="DM_PANEL_NAME">
									<HeaderStyle HorizontalAlign="Left" Width="60%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Allow View">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:checkbox runat="server" ID="ChkView">
										</asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<tr>
					<td class="EmptyCol"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save" Visible="False"></asp:button>
                        <asp:Button ID="btnhidden" runat="server" Text="Button" style="display:none;"/></td>
				</tr>
			</table>
		</form>
	</BODY>
</HTML>
