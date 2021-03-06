<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DashboardMaster.aspx.vb" Inherits="eAdmin.DashboardMaster" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Dashboard Maintenance</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<META content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
        <% Response.Write(css)%>   
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%Response.Write(Session("Dashboard_tabs"))%>
			<TABLE class="AllTable" id="Table1" cellSpacing="0" cellPadding="0">
				<TR>
					<TD class="Header"><asp:label id="lblTitle" runat="server">Dashboard Master</asp:label></TD>
				</TR>
				<tr>
	            <td class="EmptyCol" style="padding-bottom:4px;"><asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Fill in the dashboard panel name and click Save button to modify the dashboard panel name."></asp:label></td>
				</tr>
				</table>
				<table id="Table2" class="AllTable" cellpadding="0" cellspacing="0">
				<TR>
					<TD class="EmptyCol"><asp:datagrid id="dtgDashboard" runat="server">
							<Columns>
                                <asp:BoundColumn DataField="DM_PANEL_ID" HeaderText="Panel ID">
                                <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Panel Name">
                                <ItemTemplate>
                                    <asp:TextBox id="txtPanelName" cssclass="txtbox" MaxLength="50" runat="server" Width="70%" >
                                    </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EmptyCol"><asp:button id="cmdSave" runat="server" cssclass="button" Text="Save"></asp:button></TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
