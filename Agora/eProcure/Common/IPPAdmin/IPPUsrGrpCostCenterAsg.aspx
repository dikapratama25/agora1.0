<%@ Page Language="vb" AutoEventWireup="false" Codebehind="IPPUsrGrpCostCenterAsg.aspx.vb" Inherits="eProcure.IPPUsrGrpCostCenterAsg" smartNavigation="False" %>
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
		 <% Response.write(Session("typeaheadIPPCCAsg")) %>
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
			oform.txtUserGroup.value="";
			oform.txtCC.value="";
			oform.hid1.value="";
			oform.hid2.value="";
		}
		
		function ShowDialog(filename,height)
		    {
    			
			    var retval="";
			    retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 1024px");
			    //retval=window.open(filename);
			    if (retval == "1" || retval =="" || retval==null)
			    {  
			        window.close;
				    return false;

			    } else {
			        window.close;
				    return true;

			    }
		    }
		-->
		</script>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_SearchUser_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
					<td class="linespacing1" colspan="4" ></td>
			</tr>
				<tr>
				<td class="EmptyCol" colspan="4" >
					<asp:label id="Label1" runat="server"  CssClass="lblInfo"
					Text="Step 1: Create, delete or modify User Group.<br/> Step 2: Assign Branch to the User Group.<br/><b>=></b>Step 3: Assign Cost Center to the User Group.<br/>Step 4: Assign User to the User Group."></asp:label>

				</td>
			</tr>
            <tr>
					<td class="linespacing2" colspan="4" ></td>
			</tr>
				<%--<tr>
					<td class="emptycol" colspan="2">
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0" width="100%">--%>
							<tr>
								<td class="tableheader" colspan="4">&nbsp;Search Criteria</td>
							</tr>
							<tr>
								<td class="tablecol"  colspan="4"></td>
							</tr>
							<tr>
								<td class="tablecol" >&nbsp;<strong>User&nbsp;Group</strong> :</td>
								<td class="tablecol" colspan="2">	<asp:textbox id="txtUserGroup" runat="server"  CssClass="txtbox"></asp:textbox>
							</td>			
								
							</tr>
							<tr>
								<td class="tablecol">&nbsp;<strong>Cost&nbsp;Center</strong> :</td>
								<td class="tablecol">	<asp:textbox id="txtCC" runat="server"  CssClass="txtbox"></asp:textbox>	</td>							
								<td align="right" class="tablecol">
									<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
									<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;</td>
							</tr>
							<tr>
								<td class="tablecol" colspan="4" ></td>
							</tr>
						</table>
				<%--	</td>
				</tr>--%>

			<br/>
				<tr>
					<td class="emptycol" colspan="2"><asp:datagrid id="dgUser" runat="server">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete" ItemStyle-VerticalAlign="Top">
									<HeaderStyle HorizontalAlign="Center" Width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="IUC_CC_CODE" SortExpression="IUC_CC_CODE" HeaderText="Cost Center" ItemStyle-VerticalAlign="Top">
									<HeaderStyle Width="13%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="cc_cc_desc" SortExpression="cc_cc_desc" HeaderText="Description" ItemStyle-VerticalAlign="Top">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
								<%--<asp:TemplateColumn HeaderText="Description">
										<HeaderStyle Width="25%"></HeaderStyle>
										<ItemTemplate>
											<asp:Label Runat="server" ID="lblCCDesc"></asp:Label>
										</ItemTemplate>
							  </asp:TemplateColumn>--%>
								<asp:TemplateColumn HeaderText="User Group" ItemStyle-VerticalAlign="Top">
										<HeaderStyle Width="25%"></HeaderStyle>
										<ItemTemplate>
											<asp:Label Runat="server" ID="lblUserGroup"></asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
								<%--<asp:TemplateColumn SortExpression="IUM_GROUP_NAME" HeaderText="User Group">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>--%>
							<%--		<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkUserID"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>--%>
	                          <asp:BoundColumn Visible="False" DataField="CC_CC_CODE"></asp:BoundColumn>
							  
							</Columns>
						</asp:datagrid></td>
				</tr>
			
				<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
			
				<tr>
					<td class="EmptyCol"><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Enabled="False"></asp:button>
					<asp:button id="btnhidden" runat="server" CssClass="Button"  Text="btnhidden" style=" display :none"></asp:button>
					<input class="txtbox" id="hid1" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid1" runat="server" />
					<input class="txtbox" id="hid2" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid2" runat="server" /></td>
				</tr>
				</table>
				<br/>
				<tr runat="server" style="height: 25px">
					<td align="center" >
						<div align="left"><asp:label id="Label4" runat="server"  CssClass="lblInfo"
						Text="# For Deletion: If no Cost Center/User Group is specified, the selected Cost Center will be removed from the all User Group."></asp:label>
                        </div>
					</td>
				</tr>
					<!--<td class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" NavigateUrl="javascript: history.back()" ForeColor="blue">
							<strong>&lt; Back</strong></asp:hyperlink>--> 
					
			<%--</table>--%>
		</form>
	</BODY>
</HTML>
