<%@ Page Language="vb" AutoEventWireup="false" Codebehind="usSearchUser.aspx.vb" Inherits="eProcure.usSearchUser" smartNavigation="False" %>
<%@ Import Namespace="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title></title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="VBScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		function selectAll()
		{
			SelectAllG("dgUser_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dgUser_ctl02_chkAll","chkSelection");
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
        <%  Response.Write(Session("w_SearchUser_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
            <tr>
					<TD class="linespacing1" colSpan="4" ></TD>
			</TR>
				<TR>
					<TD>
						<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text="Fill in the search criteria and click Search button to list the relevant users. Click the Add button to add new user."></asp:label>
					</TD>
				</TR>
            <tr>
					<TD class="linespacing2" colSpan="4" ></TD>
			</TR>
				<TR>
					<TD class="emptycol" colSpan="2">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" width="100%">
							<TR>
								<TD class="tableheader" colspan="2">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92" colspan="2"></TD>
							</TR>
							<TR>
								<TD class="tablecol" width="80%">&nbsp;<STRONG>User&nbsp;ID</STRONG>&nbsp;:
									<asp:textbox id="txtUserID" runat="server" Width="200px" CssClass="txtbox"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<STRONG>User 
										Name</STRONG>&nbsp;:
									<asp:textbox id="txtUserName" runat="server" Width="200px" CssClass="txtbox"></asp:textbox>&nbsp;&nbsp;</TD>
								<TD align="right" class="tablecol" width="20%">
									<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;</TD>
							</TR>
							<TR>
								<TD class="tablecol" colspan="2" style="WIDTH: 92px; HEIGHT: 6px" width="92"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"><asp:label id="lblLegend" runat="server"></asp:label><asp:label id="lbl_result" runat="server" ForeColor="Red"></asp:label></TD>
				</TR>
				<TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 15px">
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD width="137"><asp:label id="lblUL" runat="server" CssClass="label">User License :&nbsp;</asp:label><asp:label id="lblUserLicense" runat="server" CssClass="label"></asp:label></TD>
								<TD><asp:label id="lblAU" runat="server" CssClass="label">Active User :&nbsp;</asp:label><asp:label id="lblActiveUser" runat="server" CssClass="label"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD colSpan="2"><asp:datagrid id="dgUser" runat="server">
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
									<HeaderStyle Width="27%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="UU_USERGROUP_ID" HeaderText="User Group" Visible="True">
									<HeaderStyle Width="30%"></HeaderStyle>
									<ItemStyle VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:Repeater ID="sub" Runat="server">
											<ItemTemplate >
												<%# DataBinder.Eval(Container.DataItem, "color1")%>
												<%# DataBinder.Eval(Container.DataItem, "UGM_USRGRP_NAME")%>
												<%# DataBinder.Eval(Container.DataItem, "color2")%>
												<br>
											</ItemTemplate>
										</asp:Repeater>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Dept Name">
									<HeaderStyle Width="13%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UM_STATUS" SortExpression="UM_STATUS"  ItemStyle-HorizontalAlign="Center" HeaderText="Status">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UM_DELETED" SortExpression="UM_DELETED" ItemStyle-HorizontalAlign="Center" HeaderText="Account Locked">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify" Enabled="False"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Enabled="False"></asp:button>&nbsp;<asp:button id="cmdUnlock" runat="server" Width="150" CssClass="button" Text="Unlock User Account"
							Enabled="False"></asp:button>&nbsp;<asp:button id="cmdActivate" runat="server" CssClass="button" Text="Activate" Enabled="False"></asp:button>&nbsp;<asp:button id="cmdDeactive" runat="server" CssClass="button" Text="Deactivate" Enabled="False"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" disabled onclick="DeselectAllG('dgUser_ctl02_chkAll','chkSelection')"
							type="button" value="Reset" name="cmdReset" runat="server" visible="false"></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<!--<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" NavigateUrl="javascript: history.back()" ForeColor="blue">
							<STRONG>&lt; Back</STRONG></asp:hyperlink>--> 
					</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
