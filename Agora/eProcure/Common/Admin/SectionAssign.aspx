<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SectionAssign.aspx.vb" Inherits="eProcure.SectionAssign" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Section</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--  
		function selectAll()
		{
			SelectAllG("dtgSection_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgSection_ctl02_chkAll","chkSelection");
		}

		-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
			<tr>
				<td class="linespacing1" colspan="4"></td>
			</tr>
			<tr>
				<td >
					<asp:label id="Label4" runat="server"  CssClass="lblInfo"
					Text="Please select related setup from drop-down list. (e.g. Delivery Term...)"></asp:label>

				</td>
			</tr>
            <tr>
			    <td class="linespacing2" colspan="4"></td>
			</tr>
			<tr>
			    <td class="tablecol">
					    <strong>Setting </strong>:&nbsp;
					    <asp:dropdownlist id="ddl_Select" runat="server" Width="170px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="true" >
					        <asp:ListItem Value="">---Select---</asp:ListItem>
							<asp:ListItem Value="1">Delivery Term</asp:ListItem>
							<asp:ListItem Value="2">Packing Type</asp:ListItem>
							<asp:ListItem Value="3">Section</asp:ListItem>
							<asp:ListItem Value="4" Selected="True">User Assignment (Section)</asp:ListItem>
					    </asp:dropdownlist>
				</td>
			</tr>
            <tr>
					<td class="linespacing1" colspan="4"></td>
			</tr>
			<tr>
				<td >
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Fill in the search criteria and click Search button to list the relevant section. Click the Add button to add new section."></asp:label>

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
								<td class="tableheader" colspan="6" width="100%">&nbsp;Select Section</td>
							</tr>
							<tr class="tablecol" id="trGrpType" runat="server">
								<td class="tablecol" width="12%">&nbsp;<strong>Section </strong>:</td>
								<td class="tablecol" width="88%" colspan="5"><asp:DropDownList id="ddl_Section" runat="server" CssClass="ddl" width="270px" AutoPostBack="true"></asp:DropDownList></td>							
							</tr>
							<tr>
					            <td class="header" colspan="6" width="100%"></td>
				            </tr>
							<tr>
								<td class="tableheader" colspan="6" width="100%">&nbsp;Search Criteria</td>
							</tr>
							<tr class="tablecol">
								<td class="tablecol" width="12%">&nbsp;<strong>User ID </strong>:</td>
								<td class="tablecol" width="20%"><asp:textbox id="txtUserId" runat="server" MaxLength="10" CssClass="txtbox" width="150px"></asp:textbox></td>							
								<td class="tablecol" width="12%" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>User Name </strong>:</td>
								<td class="tablecol" width="20%" ><asp:textbox id="txtUserName" runat="server" MaxLength="50" CssClass="txtbox" width="150px"></asp:textbox></td>
								<td class="tablecol" width="20%"><asp:checkbox id="chkUserAssigned" Text="User Assigned" Runat="server" ></asp:checkbox></td>
								<td class="tablecol" width="36%" align="right" ><asp:button id="cmd_search" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_clear1" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="3"></td>
				</tr>
			</table>
			<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="emptycol" style="width: 1192px">
						<p><asp:datagrid id="dtgSection" runat="server" OnPageIndexChanged="dtgSection_Page" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False" Width="100%">
								<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="UM_USER_ID" SortExpression="UM_USER_ID" HeaderText="User ID">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" HeaderText="User Name">
									<HeaderStyle Width="27%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Dept Name">
									<HeaderStyle Width="13%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UM_STATUS" SortExpression="UM_STATUS"  ItemStyle-HorizontalAlign="Center" HeaderText="Status">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UM_DELETED" SortExpression="UM_DELETED" ItemStyle-HorizontalAlign="Center" HeaderText="Account Locked">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UM_DELETED" SortExpression="UM_DELETED" ItemStyle-HorizontalAlign="Center" HeaderText="Account Locked" Visible="false">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
							</asp:datagrid></p>
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 6px; width: 1192px;"></td>
				</tr>
				<tr style="height: 50px">
					<td class="emptycol" style="width: 1192px">&nbsp;<asp:button id="cmd_save" runat="server" CssClass="button" Text="Save" CausesValidation="False"></asp:button><input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server"/>
                        </td>
				</tr>
			</table>
		</form>
	</body>
</html>
