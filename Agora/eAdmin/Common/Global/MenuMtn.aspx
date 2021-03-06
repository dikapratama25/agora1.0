<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MenuMtn.aspx.vb" Inherits="eAdmin.MenuMtn" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Menu Maintenance</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
        <% Response.Write(css)%>
        <% Response.Write(Session("WheelScript"))%>
                
		<script language="javascript">
				
		
		function selectAll()
		{
			SelectAllG("dgMenuMtn__ctl2_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dgMenuMtn__ctl2_chkAll","chkSelection");
		}
				
		function Reset(){
			var oform = document.forms(0);					
			oform.txtMenuID.value="";
			oform.txtMenuName.value="";
		}
		
		</script>
	</HEAD>
	<BODY MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%Response.Write(Session("Menu_tabs"))%>		
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0">
				<TR>
					<TD class="Header"><asp:label id="lblTitle" runat="server">Menu Maintenance</asp:label></TD>
				</TR>
				<tr>
	            <td><asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Choose a module to change the module and node."></asp:label></td>
				</tr>
				
				</TABLE>
				<br>
			<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader" >&nbsp;Search Criteria</TD>
							</TR>
							
							<TR>
								<TD class="tablecol"><STRONG>Menu Name</STRONG>&nbsp;:
								<asp:DropDownList id="ddlMenuName" cssclass = "ddl" runat="server" ></asp:DropDownList>&nbsp;
								 <asp:Button ID="cmdSearch" cssclass="button" runat="server" Text="Search"/>
									
							</TR>
							<!--TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92"></TD>
								<TD class="TableInput" style="HEIGHT: 6px"></TD>
							</TR>
							<TR>
								<td style="HEIGHT: 7px">&nbsp;</td>
							</TR-->
						</TABLE>
				
				<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="emptycol"><asp:label id="lbl_result" runat="server" ForeColor="Red"></asp:label></TD>
				</TR>
				
				<br>
				<TR>
					<TD><asp:datagrid id="dgMenuMtn" runat="server">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="MM_MENU_ID" HeaderText="Menu ID">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkMenuID"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								
									<asp:BoundColumn DataField="MM_MENU_IDX" SortExpression="MM_MENU_IDX" HeaderText="Seq. ID">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								
								<asp:BoundColumn DataField="MM_MENU_NAME" SortExpression="MM_MENU_NAME" HeaderText="Module Name">
									<HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="MM_MENU_LEVEL" SortExpression="MM_MENU_LEVEL" HeaderText="Level">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="MM_MENU_PARENT" SortExpression="MM_MENU_PARENT" HeaderText="Parent">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="MM_MENU_URL" SortExpression="MM_MENU_URL" HeaderText="URL">
									<HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn Visible="False">
									<HeaderStyle HorizontalAlign="Left" Width="0%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label Runat="server" ID="lblMenuLevel" Visible="False"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<tr>
					<td >
					<asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add"></asp:button>&nbsp;
					<asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify" Enabled="False"></asp:button>&nbsp;
					<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Enabled="False"></asp:button>&nbsp;
					<%--INPUT class="button" id="cmdReset" disabled onclick="DeselectAllG('dgMenuMtn__ctl2_chkAll','chkSelection')"
							type="button" value="Reset" name="cmdReset" runat="server" style="DISPLAY: none"> <asp:Button ID="btnhidden" runat="server" Text="Button" style="display:none;" /></TD--%>
					</td>
				</tr>
				
				
		</table>
		</form>
	</BODY>
</HTML>
