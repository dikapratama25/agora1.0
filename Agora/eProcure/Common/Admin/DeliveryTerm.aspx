<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DeliveryTerm.aspx.vb" Inherits="eProcure.DeliveryTerm" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>DeliveryTerm</title>
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
			SelectAllG("dtgDel_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgDel_ctl02_chkAll","chkSelection");
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
							<asp:ListItem Value="1" Selected="True">Delivery Term</asp:ListItem>
							<asp:ListItem Value="2">Packing Type</asp:ListItem>
							<asp:ListItem Value="3">Section</asp:ListItem>
							<asp:ListItem Value="4">User Assignment (Section)</asp:ListItem>
					    </asp:dropdownlist>
				</td>
			</tr>
            <tr>
					<td class="linespacing1" colspan="4"></td>
			</tr>
			<tr>
				<td >
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Fill in the search criteria and click Search button to list the relevant delivery term. Click the Add button to add new delivery term."></asp:label>

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
								<td class="tableheader" colspan="5" width="100%">&nbsp;Search Criteria</td>
							</tr>
							<tr class="tablecol" id="trGrpType" runat="server">
								<td class="tablecol" width="16%">&nbsp;<strong>Delivery Code </strong>:</td>
								<td class="tablecol" width="18%"><asp:textbox id="textDelCode" runat="server" MaxLength="10" CssClass="txtbox" width="150px" ></asp:textbox></td>							
								<td class="tablecol" width="16%" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Delivery Name </strong>:</td>
								<td class="tablecol" width="18%" ><asp:textbox id="textDelName" runat="server" MaxLength="50" CssClass="txtbox" width="150px" ></asp:textbox></td>
								<td class="tablecol" width="32%" align="right" ><asp:button id="cmd_search" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_clear1" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button></td>
								
								</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="3"></td>
				</tr>
			</table>
			<div id="Hide_Add2" style="DISPLAY: none" runat="server">
				<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
					<tr>
						<td colspan="6" class="tableheader" style="height: 19px" >&nbsp;Please
							<asp:label id="lbl_add_mod" Runat="server"></asp:label>&nbsp;the following 
							value.
						</td>
					</tr>
					<tr class="tablecol">
						<td width="16%" style="height: 24px">&nbsp;<strong>Delivery Code</strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
						<td width="18%" style="height: 24px"><asp:textbox id="txt_add_DelCode" runat="server" MaxLength="10" width="150px" CssClass="txtbox"></asp:textbox></td>
						<td width="16%" style="height: 24px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Delivery Name</strong><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
						<td width="18%" style="height: 24px"><asp:textbox id="txt_add_DelName" runat="server" MaxLength="50" width="150px" CssClass="txtbox"></asp:textbox></td>
						<td align="right">	
						    <asp:button id="cmd_save" runat="server" CssClass="button" Text="Save"></asp:button>
							<asp:button id="cmd_clear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
							<asp:button id="cmd_cancel" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"></asp:button>
						</td>
					</tr>
					<tr class="tablecol" style="width:100%">
					    <td width="16%" style="height: 24px">&nbsp;<strong>GRN Factor</strong><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					    <td width="18%" style="height: 24px"><asp:textbox id="txt_add_Factor" runat="server" MaxLength="20" width="150px" CssClass="txtbox"></asp:textbox></td>
					    <td width="16%" style="height: 24px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Oversea</strong>&nbsp;:</td>
					    <td width="18%" style="height: 24px" colspan="2"><asp:radiobuttonlist ID="rbOversea" runat="server" BorderStyle="None" CssClass="rbtn" RepeatDirection="Horizontal"> 
                                        <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
							            <asp:ListItem Value="Y">Yes</asp:ListItem>
						            </asp:radiobuttonlist></td>
					</tr>
					<tr>
						<td colspan="3" class="emptycol" style="height: 19px"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
							required field<asp:requiredfieldvalidator id="validate_del_code" runat="server" ControlToValidate="txt_add_DelCode" ErrorMessage="Delivery Code is required."
								Display="None"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="validate_del_name" runat="server" ControlToValidate="txt_add_DelName" ErrorMessage="Delivery Name is required."
								Display="None"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="validate_grn_factor" runat="server" ControlToValidate="txt_add_Factor" ErrorMessage="GRN Factor is required."
								Display="None"></asp:requiredfieldvalidator><asp:RegularExpressionValidator ID="rev_grn_factor" runat="server" ControlToValidate="txt_add_Factor"
                                Display="None" ErrorMessage="Invalid GRN Factor. Range should be from 0 to 999.999" ValidationExpression="^\d{1,3}(?:\.(?:\d{0,3})?\d)?$"></asp:RegularExpressionValidator>
							</td>
					</tr>
					<tr>
						<td colspan="3" >
							<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
							</td>
					</tr>
					<tr>
						<td class="emptycol"></td>
					</tr>
				</table>
			</div>
			<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="emptycol" style="width: 1192px">
						<p><asp:datagrid id="dtgDel" runat="server" OnPageIndexChanged="dtgDel_Page" AllowSorting="True"
								OnSortCommand="SortCommand_Click" AutoGenerateColumns="False" Width="100%">
								<Columns>
									<asp:TemplateColumn HeaderText="Delete">
										<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<HeaderTemplate>
											<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
											<!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
										</HeaderTemplate>
										<ItemTemplate>
											<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="CDT_DEL_CODE" SortExpression="CDT_DEL_CODE"  readonly="True"  HeaderText="Delivery Code">
										<HeaderStyle Width="25%"></HeaderStyle>
									</asp:BoundColumn>	
									<asp:BoundColumn Visible="False" DataField="CDT_DEL_INDEX"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="CDT_COY_ID"></asp:BoundColumn>
									<asp:BoundColumn DataField="CDT_DEL_NAME" HeaderText="Delivery Name" SortExpression="CDT_DEL_NAME">
										<HeaderStyle Width="40%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn  DataField="CDT_DEL_GRNFACTOR" HeaderText="GRN Factor %" SortExpression="CDT_DEL_GRNFACTOR" >
										<HeaderStyle HorizontalAlign="Right"  Width="20%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right" />
									</asp:BoundColumn>
									<asp:BoundColumn  DataField="CDT_DEL_OVERSEA" HeaderText="Oversea" SortExpression="CDT_DEL_OVERSEA" >
									    <HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>							
								</Columns>
							</asp:datagrid></p>
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 6px; width: 1192px;"></td>
				</tr>
				<tr style="height: 50px">
					<td class="emptycol" style="width: 1192px"><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" Width="59px" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button><input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"/><input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server"/>&nbsp;
                        </td>
				</tr>
			</table>
		</form>
	</body>
</html>
