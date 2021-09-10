<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UsSearchUserHub.aspx.vb" Inherits="eAdmin.usSearchUserHub" smartNavigation="False" %>
<%@ Import Namespace="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title></title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="VBScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
        </script>
        <% Response.Write(sCSS)%>
        <% Response.Write(Session("WheelScript"))%>
		
		
		<script language="javascript">
		<!--		
		
		function selectAll()
		{
			SelectAllG("dgUser__ctl2_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dgUser__ctl2_chkAll","chkSelection");
		}
				
		function Reset(){
			var oform = document.forms(0);					
			oform.txtUserID.value="";
			oform.txtUserName.value="";
		}
		-->
		</script>
	</HEAD>
	<BODY topMargin="10" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header"><FONT size="3">User Account Maintenance</FONT></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 19px"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" width="100%">
							<TR>
								<TD class="tableheader" colspan="3" style="height: 19px">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px"></TD>
								<TD class="TableInput" style="HEIGHT: 6px; width: 10px;" colspan="2"></TD>
							</TR>
							<TR>
								<TD class="tablecol" colspan="2" >&nbsp;<STRONG>User&nbsp;ID</STRONG>&nbsp;:
									<asp:textbox id="txtUserID" runat="server" CssClass="txtbox" Width="128px"></asp:textbox>&nbsp;<STRONG>User 
										Name</STRONG>&nbsp;:
									<asp:textbox id="txtUserName" runat="server" CssClass="txtbox" Width="128px"></asp:textbox>&nbsp;&nbsp;
								</TD>
								<td class="tablecol" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;</td>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px"></TD>
								<TD class="TableInput" style="HEIGHT: 6px; width: 10px;" colspan="2"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:label id="lbl_result" runat="server" ForeColor="Red"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 15px">
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD width="137"><asp:label id="lblUL" runat="server" CssClass="label">User License :&nbsp;</asp:label><asp:label id="lblUserLicense" runat="server" CssClass="label"></asp:label></TD>
								<TD><asp:label id="lblAU" runat="server">Active User :&nbsp;</asp:label><asp:label id="lblActiveUser" runat="server" CssClass="label"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dgUser" runat="server">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="UM_USER_ID" HeaderText="User ID">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkUserID"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" HeaderText="User Name">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="UGM_USRGRP_NAME" HeaderText="User Group">
									<HeaderStyle HorizontalAlign="Left" Width="22%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:Repeater ID="sub" Runat="server">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "UGM_USRGRP_NAME")%>
												<br>
											</ItemTemplate>
										</asp:Repeater>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Dept Name">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UM_STATUS" SortExpression="UM_STATUS" HeaderText="Status">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UM_DELETED" SortExpression="UM_DELETED" HeaderText="Account Locked">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify" Enabled="False"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Enabled="False"></asp:button>&nbsp;<asp:button id="cmdUnlock" runat="server" CssClass="button" Width="150" Text="Unlock User Account"
							Enabled="False"></asp:button>&nbsp;<asp:button id="cmdActivate" runat="server" CssClass="button" Text="Activate" Enabled="False"></asp:button>&nbsp;<asp:button id="cmdDeactive" runat="server" CssClass="button" Text="Deactivate" Enabled="False"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" disabled onclick="DeselectAllG('dgUser__ctl2_chkAll','chkSelection')"
							type="button" value="Reset" name="cmdReset" runat="server" style="DISPLAY: none"></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<!--<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" NavigateUrl="javascript: history.back()" ForeColor="blue">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>--></TABLE>
		</form>
	</BODY>
</HTML>
