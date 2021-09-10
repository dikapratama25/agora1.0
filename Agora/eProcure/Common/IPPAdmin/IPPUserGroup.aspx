<%@ Page Language="vb" AutoEventWireup="false" Codebehind="IPPUserGroup.aspx.vb" Inherits="eProcure.IPPUserGroup" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>E2PUserGroup</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--  
		function selectAll()
		{
			SelectAllG("dtgApp_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgApp_ctl02_chkAll","chkSelection");
		}


	
		function Display(num)
			{
				var check = num;
				var div_add = document.getElementById("Hide_Add1");
				var div_add = document.getElementById("Hide_Add2");
				var div_add = document.getElementById("Hide_Add3");
				
				var cmd_delete = document.getElementById("cmd_delete");
				var hidMode = document.getElementById("hidMode");
				var add_mod = document.getElementById("add_mod");
				div_add.style.display ="";
				
				if (check==1)
				{
					cmd_delete.style.display = "none";
					Form1.hidMode.value = 'm';
					add_mod.value='add';
				}
				else if (check==0)
				{
					cmd_delete.style.display = "none";
				}
			}
	
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ApprWF_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
            <tr>
					<td class="linespacing1" colspan="4"></td>
			</tr>
			<tr>
				<td >
                    <%--Zulham 10072018 - PAMB--%>
                    <%--Removed branch--%>
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="<b>=></b> Step 1: Create, delete or modify User Group.<br/>Step 2: Assign Cost Center to the User Group.<br>Step 3: Assign User to the User Group."></asp:label>

				</td>
			</tr>
            <tr>
					<td class="linespacing2" colspan="4"></td>
			</tr>
				<tr>
					<td class="tablecol">
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0" width="100%">
				<tr>
					<td class="header" colspan="3" width="100%"></td>
				</tr>
							<tr>
								<td class="tableheader" colspan="3" width="100%">&nbsp;Search Criteria</td>
							</tr>
							<tr>
								<td class="tablecol" width="15%" >&nbsp;<strong>User Group </strong>:</td>
								<td class="tablecol" width="43%" ><asp:textbox id="txtUserGroup" runat="server" MaxLength="50" CssClass="txtbox" width="100%" ></asp:textbox></td>
								<td class="tablecol" width="44%" align="right" ><asp:button id="cmd_search" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_clear1" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
								</td>
								</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="3"></td>
				</tr>
			</table>
			<DIV id="Hide_Add2" style="DISPLAY: none" runat="server">
				<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
					<tr>
						<td colspan="6" class="tableheader" style="height: 19px" >&nbsp;Please
							<asp:label id="lbl_add_mod" Runat="server"></asp:label>&nbsp;the following 
							value.
						</td>
					</tr>
					<tr class="tablecol">
						<td width="15%" style="height: 24px">&nbsp;<strong>User Group</strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
						<td width="20%" style="height: 24px"><asp:textbox id="txt_add_mod" runat="server" MaxLength="100" width="100%" CssClass="txtbox"></asp:textbox></td>
						
						<td align="right">	
						    <asp:button id="cmd_save" runat="server" CssClass="button" Text="Save"></asp:button>
							<asp:button id="cmd_clear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
							<asp:button id="Button1" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"></asp:button>
						</td>
					</tr>										
					<tr>
						<td class="emptycol"></td>
					</tr>
				</table>
			</DIV>
			<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="emptycol">
						<P><asp:datagrid id="dtgApp" runat="server" OnPageIndexChanged="MyDataGrid_Page" AllowSorting="True"
								OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
								<Columns>
									<asp:TemplateColumn HeaderText="Delete"  ItemStyle-VerticalAlign="Top">
										<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<HeaderTemplate>
											<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
										</HeaderTemplate>
										<ItemTemplate>
											<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="IUM_GRP_NAME" SortExpression="IUM_GRP_NAME"  readonly="true"  HeaderText="User Group" ItemStyle-VerticalAlign="Top">
										<HeaderStyle Width="15%" VerticalAlign="Top"></HeaderStyle>
										<ItemStyle></ItemStyle>
									</asp:BoundColumn>
								    <asp:TemplateColumn HeaderText="User Name" ItemStyle-VerticalAlign="Top">
										<HeaderStyle Width="20%"></HeaderStyle>
										<ItemTemplate>
											<asp:Label Runat="server" ID="lblUserName"></asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
									<%--Zulham 10072018 - PAMB--%>
									<asp:TemplateColumn HeaderText="Branch Code" ItemStyle-VerticalAlign="Top" Visible="false" >
										<HeaderStyle Width="12%"></HeaderStyle>
										<ItemTemplate>
											<asp:Label Runat="server" ID="lblBranchCode"></asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText = "Description" ItemStyle-VerticalAlign="Top" Visible="false" >
                                        <HeaderStyle Width="13%"></HeaderStyle>
                                        <ItemTemplate>
											<asp:Label Runat="server" ID="lblBranchCodeDesc"></asp:Label>
										</ItemTemplate>
                                    </asp:TemplateColumn>
                                    <%--End--%>
                                    <asp:TemplateColumn HeaderText = "Company ID" ItemStyle-VerticalAlign="Top">
                                        <HeaderStyle Width="13%"></HeaderStyle>
                                        <ItemTemplate>
											<asp:Label Runat="server" ID="lblBRCoyID"></asp:Label>
										</ItemTemplate>
                                    </asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Cost Center" ItemStyle-VerticalAlign="Top">
										<HeaderStyle Width="10%"></HeaderStyle>
										<ItemTemplate>
											<asp:Label Runat="server" ID="lblCostCenter"></asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Description" ItemStyle-VerticalAlign="Top">
									    <ItemTemplate>
											<asp:Label Runat="server" ID="lblCostCenterDesc"></asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn Visible="False" DataField="IUM_GRP_INDEX"></asp:BoundColumn>														
								</Columns>
							</asp:datagrid></P>
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 6px"></td>
				</tr>
				<tr style="height: 50px">
					<td class="emptycol"><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" Width="59px" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_delete" runat="server" CssClass="button" Text="Delete" CausesValidation="False"></asp:button>&nbsp;
					<input class="button" id="cmd_Reset" style="DISPLAY: none" onclick="DeselectAllG('dtgApp_ctl02_chkAll','chkSelection')" type="button" value="Reset" name="cmd_Reset" runat="server"> 
							<input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"><input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server">&nbsp;
                        </td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 6px"></td>
				</tr>
				<%--<tr id="trhid" runat="server" style="height: 25px">
					<td align="center" >
						<div align="left"><asp:label id="Label4" runat="server"  CssClass="lblInfo"
						Text="a) The system comes with two pre-defined Approval Group i.e. Purchasing Manager to Approving Officer Approval and Purchasing Officer to Approving Officer Approval.<br>b) Click Add button to add new user defined Approval Group.<br>c) Click Modify button to modify the system pre-defined Approval Group and user defined Approval Group.<br>d) Click Delete button to delete the system pre-defined Approval Group and user defined Approval Group.<br>e) Click Approval Officer Assignment to add Approving Officer to Approval Group.<br>f) Click 'User Assignment' to assign User to Approval Group."></asp:label>
                        </div>
					</td>
				</tr>--%>
			</table>
		</form>
	</body>
</HTML>
