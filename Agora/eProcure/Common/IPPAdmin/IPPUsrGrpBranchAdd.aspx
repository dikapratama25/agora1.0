<%@ Page Language="vb" AutoEventWireup="false" Codebehind="IPPUsrGrpBranchAdd.aspx.vb" Inherits="eProcure.IPPUsrGrpBranchAdd" smartNavigation="False" %>
<%@ Import Namespace="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title></title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="VBScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
			<% Response.Write(Session("JQuery")) %> 
            <% Response.Write(Session("AutoComplete")) %>

		<% Response.Write(Session("WheelScript"))%>
		 <% Response.write(Session("typeaheadIPPBranchAdd")) %>
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
			oform.txtBranchCode.value="";
			oform.txtBranchName.value="";
		}
		
		
		-->
		</script>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">         
	            <tr>
					<td class="header"><strong>
                        <asp:Label ID="lbltitle" runat="server" Text="Assign Branch Code"></asp:Label></strong></td>
				</tr>	       
				<%--<tr>
					<td class="emptycol" colspan="2">
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0" width="100%">--%>
							<tr>
								<td class="tableheader" colspan="3">&nbsp;Select Branch Code</td>
							</tr>
							<tr>
								<td class="tablecol" colspan="3"></td>
							</tr>
	
							<tr>
								<td class="tablecol">&nbsp;<strong>Branch&nbsp;Code</strong>&nbsp;:&nbsp;
									<asp:textbox id="txtBranchCode" runat="server"  CssClass="txtbox"></asp:textbox></td>								
									<td class="tablecol" >&nbsp;<strong>Branch&nbsp;Name</strong>&nbsp;&nbsp;&nbsp;:&nbsp;
									<asp:textbox id="txtBranchName" runat="server"  CssClass="txtbox"></asp:textbox></td>
								<td align="right" class="tablecol">
									<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
									<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;
									</td>
							</tr>
					     </table>
			<%--			</table>
					</td>
				</tr>
--%>
				<br/>
				<tr>
					<td class="emptycol" colspan="2">
					<asp:datagrid id="dgUser" runat="server">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CBM_BRANCH_CODE" SortExpression="CBM_BRANCH_CODE" HeaderText="Branch Code">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CBM_BRANCH_NAME" SortExpression="CBM_BRANCH_NAME" HeaderText="Branch Name">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CBM_COY_ID" SortExpression="CBM_COY_ID" HeaderText="Company ID">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									</asp:BoundColumn>
							<%--		<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkUserID"></asp:HyperLink>
									</ItemTemplate>--%>
								<%--</asp:TemplateColumn>
	                          <asp:BoundColumn Visible="False" DataField="CDM_DEPT_INDEX"></asp:BoundColumn>
							--%>
		
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
				<tr>
					<td class="EmptyCol"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;
					<input class="button" id="cmd_Reset" onclick="DeselectAllG('dgUser_ctl02_chkAll','chkSelection')" type="button" value="Reset" name="cmd_Reset" runat="server"> 
					<%--<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Enabled="False"></asp:button>--%>
					<%--<asp:button id="btnhidden" runat="server" CssClass="Button"  Text="btnhidden" style=" display :none"></asp:button>--%>
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
		
			
		</form>
	</BODY>
</HTML>
