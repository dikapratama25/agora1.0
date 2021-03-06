<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DashboardSearch.aspx.vb" Inherits="eAdmin.DashboardSearch" %>
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
        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>	
        <% Response.Write(Session("AutoComplete")) %>
        <script language="javascript">
		<!--
            
            
            $(document).ready(function(){
            $("#txtDashboardName").autocomplete("DashboardTypeAhead.aspx", {
            width: 260,
            selectFirst: false
            });
            });
            -->
            </script>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" runat="server">
		<%Response.Write(Session("Dashboard_tabs"))%>
			<TABLE class="AllTable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="Header" colspan="2"><asp:label id="lblTitle" runat="server">Dashboard Listing</asp:label></TD>
				</TR>
				<tr>
	            <td colspan="2" class="EmptyCol" style="padding-bottom:4px;"><asp:label id="lblAction" runat="server" CssClass="lblInfo" Text="Fill in the search criteria and click Search button to list the relevant dashboard."></asp:label></td>
				</tr>
				<TR>
					<TD class="TableHeader" colspan="2"><asp:label id="lblHeader" runat="server" Text="Dashboard Search"></asp:label></TD>
				</TR>
				<TR>
				<TD class="TableCol" width="30%"><STRONG>User Role</STRONG>:
                    <asp:DropDownList ID="ddlUserRole" cssclass="ddl" width="150px" runat="server">
                    </asp:DropDownList></TD> 
                <TD class="TableCol"><STRONG>Dashboard Panel Name</STRONG>:
                    <asp:TextBox ID="txtDashboardName" cssclass="txtbox" width="150px" runat="server"></asp:TextBox>
                    <asp:Button ID="cmdSearch" cssclass="button" runat="server" Text="Search"/>
                </TD>
				</TR>
				<tr><td class="EmptyCol" style="height:15px;"></td></tr>
				</TABLE>
				
			<table class="AllTable" id="Table2" cellSpacing="0" cellPadding="0"  border="0" width="100%">
				<TR>
					<TD class="EmptyCol">
					<asp:datagrid id="dtgDashboardList" runat="server" OnSortCommand="SortCommand_Click" CssClass="Grid">
							<Columns>
                                <asp:TemplateColumn HeaderText="User Role" SortExpression="DM_FIXED_ROLE_ID">
                                <HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" Height="20px"></ItemStyle>
                                <ItemTemplate>
                                <asp:HyperLink Runat="server" ID="lnkUserRole"></asp:HyperLink>
                                </ItemTemplate>
                                </asp:TemplateColumn>
								<asp:BoundColumn DataField="DM_PANEL_NAME" HeaderText="Dashboard Panel Name" SortExpression="DM_PANEL_NAME">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Height="20px"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</TD>
				</TR>
				<tr>
					<td>
                        <asp:Button ID="btnhidden" runat="server" Text="Button" style="display:none;"></asp:Button></td>
				</tr>
			</table>
		</form>
	</BODY>
</HTML>
