<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ApprovalWorkFlow.aspx.vb" Inherits="eProcure.ApprovalWorkFlow" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>ApprovalWorkFlow</title>
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
			SelectAllG("dtgApp_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgApp_ctl02_chkAll","chkSelection");
		}

		function ValidateConsol(source, arguments)
		{
			var selected;
			selected = document.getElementById("cboType").selectedIndex;
			
			if (selected == 0){ // PR
				if (arguments.Value=="")
					arguments.IsValid=false;
				else
					arguments.IsValid=true;
			}
			else { // INV
				arguments.IsValid=true;
			}
		}
		
		function DisplayConsolidator()
		{
			var selected;
			selected = document.getElementById("cboType").selectedIndex;
			
			if (selected == 0){ // PR
				document.getElementById("chkConsol").style.display = "inline";
				document.getElementById("lblConsol").style.display = "inline";
				//document.getElementById("val_consol").disabled = false;
				document.getElementById("divConsolidator").style.display = "inline";
			}
			else { // INV
				document.getElementById("chkConsol").style.display = "none";
				document.getElementById("lblConsol").style.display = "none";
				//document.getElementById("val_consol").disabled = true;
				document.getElementById("divConsolidator").style.display = "none";
			}
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
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ApprWF_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
            <tr>
					<td class="linespacing1" colspan="4"></td>
			</tr>
			<tr>
				<td >
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="<b>=></b> Step 1: Create, delete or modify Approval Group.<br />Step 2: Assign Approving Officer to the Selected Approval Group.<br>Step 3: Assign User to the Selected Approval Group."></asp:label>

				</td>
			</tr>
            <tr>
					<td class="linespacing2" colspan="4"></td>
			</tr>
				<tr>
					<td class="tablecol">
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0" width="100%">
				            <tr>
					            <td class="header" colspan="5" width="100%"></td>
				            </tr>
							<tr>
								<td class="tableheader" colspan="5" width="100%">&nbsp;Search Criteria</td>
							</tr>
							<tr class="tablecol" id="trGrpType" runat="server">
								<td style="height: 22px">&nbsp;<strong>Group Type</strong><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
								<td style="height: 22px"><asp:dropdownlist id="cboType" runat="server" CssClass="txtbox" AutoPostBack="True" Width="100px"></asp:dropdownlist></td>
								<td style="height: 22px">&nbsp;<strong><asp:label id="Label5" runat="server" Text="IQC Type :"></asp:label></strong></td>
								<td style="height: 22px"><asp:dropdownlist id="cboIQCType" runat="server" CssClass="txtbox" Width="100px"></asp:dropdownlist></td>
								<td style="height: 22px"></td> 
							</tr>
							<tr>
								<td class="tablecol" width="15%">&nbsp;<strong>Approval Group </strong>:</td>
								<td class="tablecol" width="35%"><asp:textbox id="txtGrpSearch" runat="server" MaxLength="50" CssClass="txtbox" width="80%" ></asp:textbox></td>
								<td class="tablecol" width="15%">&nbsp;<strong><asp:label id="Label6" runat="server" Text="Department :"></asp:label></strong></td>
								<td class="tablecol" width="15%"><asp:dropdownlist id="cboDpt" runat="server" CssClass="txtbox" Width="100px"></asp:dropdownlist></td>
								<td class="tablecol" width="20%" align="right" ><asp:button id="cmd_search" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_clear1" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
								</td>
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
						<td colspan="7" class="tableheader" style="height: 19px" >&nbsp;Please
							<asp:label id="lbl_add_mod" Runat="server"></asp:label>&nbsp;the following 
							value.
						</td>
					</tr>
					<tr class="tablecol">
						<td width="15%" style="height: 24px">&nbsp;<strong>Approval Group</strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
						<td width="20%" style="height: 24px"><asp:textbox id="txt_add_mod" runat="server" MaxLength="100" width="100%" CssClass="txtbox"></asp:textbox></td>
						<td>&nbsp;<strong><asp:label ID="lbl1" Text="Department :" runat="server"></asp:label></strong></td>
						<td>
                            <asp:DropDownList ID="ddlDept" CssClass="ddl" runat="server">
                            </asp:DropDownList>
                        </td>
						<td width="10%" style="height: 24px"><asp:checkbox id="chkConsol" runat="server" AutoPostBack="True" Font-Bold="True"></asp:checkbox><asp:label id="lblConsol" runat="server">Consolidator Required</asp:label></td>
                        <%--Zulham 26062018 - PAMB--%>
						<td style="height: 24px"><asp:checkbox id="chkResident" runat="server" Visible ="false"  Font-Bold="True" Text="Resident"></asp:checkbox></td>
                        <%--End--%>
						<td align="right">	
						    <asp:button id="cmd_save" runat="server" CssClass="button" Text="Save"></asp:button>
							<asp:button id="cmd_clear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
							<asp:button id="Button1" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"></asp:button>
						</td>
					</tr>
					<tr class="tablecol" id="trIQCType" runat="server" >
						<td width="15%" style="height: 24px">&nbsp;<strong>IQC Type</strong><asp:label id="Label9" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
						<td width="20%" style="height: 24px"><asp:dropdownlist id="cboIQCType2" runat="server" CssClass="txtbox" Width="100%"></asp:dropdownlist></td>
						<td  colspan="5"></td>
					</tr>
					<tr class="tablecol" id="trConsol" runat="server">
						<td colspan="7" noWrap><span id="divConsolidator" runat="server">&nbsp;<asp:label id="lblconsolidator" runat="server" Font-Bold="True" >Consolidator</asp:label>
								<asp:label id="lblSymbol" runat="server" CssClass="errormsg" >*</asp:label>&nbsp;:&nbsp;&nbsp;&nbsp;
								<asp:dropdownlist id="cboConsol" runat="server" CssClass="txtbox" Width="43%"></asp:dropdownlist>&nbsp;</span>
						</td>
					</tr>
					<tr class="tablecol" id="trUrgentIR" runat="server">
					    <td width="20%" style="height: 24px">&nbsp;<strong>Email recipient (Urgent IR)</strong>&nbsp;:</td>
					    <td width="20%" style="height: 24px"><asp:TextBox ID="txtUrgentIR" runat="server" CssClass="txtbox" Width="100%"></asp:TextBox></td>
					    <td colspan="5"></td>
					</tr>
					<tr class="tablecol" id="trRejectMRS" runat="server">
					    <td width="20%" style="height: 24px">&nbsp;<strong>Email recipient (Reject MRS)</strong>&nbsp;:</td>
					    <td width="20%" style="height: 24px"><asp:TextBox ID="txtRejectMRS" runat="server" CssClass="txtbox" Width="100%"></asp:TextBox></td>
					    <td colspan="5" align="right">	
						    <asp:button id="cmd_save1" runat="server" CssClass="button" Text="Save"></asp:button>
							<asp:button id="cmd_clear2" runat="server" CssClass="button" Text="Reset" CausesValidation="False"></asp:button>
							<asp:button id="button3" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"></asp:button>
						</td>
					</tr>
					<tr>
						<td colspan="4" class="emptycol"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
							required field<asp:requiredfieldvalidator id="validate_grp_name" runat="server" ControlToValidate="txt_add_mod" ErrorMessage="Approval Group is required."
								Display="None"></asp:requiredfieldvalidator>
							<asp:requiredfieldvalidator id="val_consol" runat="server" ControlToValidate="cboConsol" ErrorMessage="Consolidator is required."
								Display="None" Enabled="False"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="rfvType" runat="server" ControlToValidate="cboType" ErrorMessage="Group Type is required."
								Display="None"></asp:requiredfieldvalidator><asp:customvalidator id="cvConsol" runat="server" ControlToValidate="cboConsol" ErrorMessage="Consolidator is required."
								Display="None" ClientValidationFunction="ValidateConsol"></asp:customvalidator></td>
					</tr>
					<tr>
						<td colspan="3">
							<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary></td>
							
					</tr>
					<tr>
						<td colspan="4" style="height: 19px">
                            <asp:Label ID="lblmsg" runat="server" CssClass="errormsg"></asp:Label></td>
							
					</tr>
					<tr>
						<td class="emptycol"></td>
					</tr>
				</table>
			</div>
			<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="emptycol">
						<P><asp:datagrid id="dtgApp" runat="server" OnPageIndexChanged="MyDataGrid_Page" AllowSorting="True"
								OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
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
									<asp:BoundColumn DataField="AGM_GRP_NAME" SortExpression="AGM_GRP_NAME"  readonly="true"  HeaderText="Approval Group">
										<HeaderStyle Width="25%"></HeaderStyle>
										<ItemStyle></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME"  readonly="true"  HeaderText="Consolidator" Visible="false">
										<HeaderStyle Width="20%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Approving Officer /&lt;br&gt; Alternative Approving Officer">
										<HeaderStyle Width="30%"></HeaderStyle>
										<ItemTemplate>
											<asp:Label Runat="server" ID="lblApproval"></asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Buyer">
										<HeaderStyle Width="20%"></HeaderStyle>
										<ItemTemplate>
											<asp:Label Runat="server" ID="lblBuyer"></asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn Visible="False" DataField="AGM_GRP_INDEX"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="UM_USER_ID"></asp:BoundColumn>
									<asp:TemplateColumn HeaderText="Approving Finance Officer  / Alternative Approving Finance Officer" Visible="false">
									<HeaderStyle Width="20%"></HeaderStyle>
										<ItemTemplate>
											<asp:Label Runat="server" ID="lblfo"></asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="Approving Finance Manager  / Alternative Approving Finance Manager" Visible="false">
									<HeaderStyle Width="15%"></HeaderStyle>
										<ItemTemplate>
											<asp:Label Runat="server" ID="lblfm"></asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="AGM_IQC_TYPE" HeaderText="IQC Type" SortExpression="AGM_IQC_TYPE">
										<HeaderStyle Width="10%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CDM_DEPT_NAME" HeaderText="Department" SortExpression="CDM_DEPT_NAME">
										<HeaderStyle Width="5%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn  DataField="CDM_DEPT_CODE" HeaderText="Department" SortExpression="CDM_DEPT_CODE" Visible ="false">
										<HeaderStyle Width="5%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="AGM_MRS_EMAIL1" HeaderText="Email Recipient (Urgent IR)" SortExpression="AGM_MRS_EMAIL1">
									    <HeaderStyle Width="10%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="AGM_MRS_EMAIL2" HeaderText="Email Recipient (Reject MRS)" SortExpression="AGM_MRS_EMAIL2">
									    <HeaderStyle Width="10%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="AGM_TYPE" HeaderText="Type" SortExpression="AGM_TYPE">
										<HeaderStyle Width="5%"></HeaderStyle>
									</asp:BoundColumn>	
                                    <%--Zulham 09072018 - PAMB
                                    Added column for residence--%>
                                    <asp:BoundColumn DataField="AGM_RESIDENT" HeaderText="Residence" SortExpression="AGM_RESIDENT">
										<HeaderStyle  Width="5%" HorizontalAlign="Center" ></HeaderStyle>
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
									</asp:BoundColumn>
                                    <%--End--%>
								</Columns>
							</asp:datagrid></P>
					</td>
				</tr>
				<tr>
					<td class="emptycol"><asp:label id="lblRed" runat="server" Visible="False"><font color="red">*</font>&nbsp;deleted or inactive user is displayed in red colour</asp:label></td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 6px"></td>
				</tr>
				<tr style="height: 50px">
					<td class="emptycol"><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" Width="59px" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_delete" runat="server" CssClass="button" Text="Delete" CausesValidation="False"></asp:button>&nbsp;<input class="button" id="cmd_Reset" style="DISPLAY: none" onclick="DeselectAllG('dtgApp_ctl02_chkAll','chkSelection')"
							type="button" value="Reset" name="cmd_Reset" runat="server"> <input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"/><input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server"/>&nbsp;
                        </td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 6px"></td>
				</tr>
				<tr id="trhid" runat="server" style="height: 25px">
					<td align="center" >
						<div align="left"><asp:label id="Label4" runat="server"  CssClass="lblInfo"
						Text="a) The system comes with two pre-defined Approval Group i.e. Purchasing Manager to Approving Officer Approval and Purchasing Officer to Approving Officer Approval.<br>b) Click Add button to add new user defined Approval Group.<br>c) Click Modify button to modify the system pre-defined Approval Group and user defined Approval Group.<br>d) Click Delete button to delete the system pre-defined Approval Group and user defined Approval Group.<br>e) Click Approval Officer Assignment to add Approving Officer to Approval Group.<br>f) Click 'User Assignment' to assign User to Approval Group."></asp:label>
                        </div>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
