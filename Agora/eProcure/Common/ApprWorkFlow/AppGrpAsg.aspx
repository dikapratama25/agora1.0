<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AppGrpAsg.aspx.vb" Inherits="eProcure.AppGrpAsg" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>AppGrpAsg</title>
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
			SelectAllG("MyDataGrid_ctl02_chkAll","chkSelection");
			
			
		}
				
		function checkChild(id)
		{
			checkChildG(id,"MyDataGrid_ctl02_chkAll","chkSelection");
		}
	
		function ShowDialog(filename,height)
			{
				
				var retval="";
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 700px");
				//retval=window.open(filename);
								if (retval == "1" || retval =="" || retval==null)
				{  window.close;
					return false;

				} else {
				    window.close;
					return true;

				}
			}
		-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ApprWFAsg_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
            <tr>
					<td class="linespacing1" colspan="3"></td>
			    </tr>
				<tr>
	                <td colspan="3">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Step 1: Create, delete or modify Approval Group.<br /><b>=></b> Step 2: Assign Approving Officer to the Selected Approval Group.<br>Step 3: Assign User to the Selected Approval Group."></asp:label>

	                </td>
                </tr>
              <tr>
					<td class="linespacing2" colspan="4"></td>
			</tr>
            <tr>
					<td align="center">
						<div align="left"><asp:label id="Label5" runat="server"  CssClass="lblInfo"
						Text="Please select Approval Group Type and the related Approval Group."
						></asp:label>
                        </div>
					</td>
			</tr>
               <tr>
					<td class="linespacing2" colspan="3"></td>
			    </tr>
				<tr>
					<td class="tableheader" colspan="2" width="100%">&nbsp;Search Criteria</td>
				</tr>
				<tr>
					<td>
						<table class="alltable" cellspacing="0" cellpadding="0" border="0">
							<tr class="tablecol" id="trGrpType" runat="server">
								<td style="WIDTH: 16%">&nbsp;<strong>Group Type</strong><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
								<td class="TableCol">&nbsp;<asp:dropdownlist id="cboType" runat="server" CssClass="txtbox" Width="100px" AutoPostBack="True">
										<asp:ListItem Value="INV">INV</asp:ListItem>
										<asp:ListItem Value="PO">PO</asp:ListItem>
										<asp:ListItem Value="PR">PR</asp:ListItem>
										<asp:ListItem Value="E2P">E2P</asp:ListItem>
										<asp:ListItem Value="IQC">IQC</asp:ListItem>
										<asp:ListItem Value="MRS">eMRS</asp:ListItem>
										<asp:ListItem Value="BIL">Billing</asp:ListItem>
										<asp:ListItem Value="SC">Staff Claim</asp:ListItem>
									</asp:dropdownlist></td>
								<td class="TableCol"><strong>&nbsp;<asp:Label ID="lblIQC" text="IQC Type :" runat="server"></asp:Label></strong></td>	
								<td class="TableCol"><asp:Label ID="lblIQCType" runat="server" Text=""></asp:Label></td>
							</tr>
							<tr>
								<td class="TableCol" style="WIDTH: 16%"><strong>&nbsp;Approval Group </strong>
									:</td>
								<td class="TableCol" style="WIDTH: 44%">&nbsp;<asp:dropdownlist id="cboGroup" runat="server" AutoPostBack="True" CssClass="txtbox" Width="90%"></asp:dropdownlist>
								<asp:checkbox id="chkConsol" runat="server" AutoPostBack="True" Text="Consolidation Required"
										Enabled="False"></asp:checkbox></td>
								<td class="TableCol" style="WIDTH: 16%"><strong>&nbsp;<asp:Label ID="lbldept" text="Department :" runat="server"></asp:Label></strong></td>
								<td class="TableCol" style="WIDTH: 24%">
                                    <asp:Label ID="lbldeptname" runat="server" Text=""></asp:Label>
                                </td>
							</tr>
						</table>
					</td>
				</tr>
				</table>
						<div id="Div_AppGrp" style="DISPLAY: none" runat="server">
							<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" border="0" width="100%">
								<tbody>
									<tr>
										<td class="EmptyCol">&nbsp;
											<asp:datagrid id="MyDataGrid" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyData_Page">
												<Columns>
													<asp:TemplateColumn HeaderText="Delete">
														<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
														<ItemStyle HorizontalAlign="Center"></ItemStyle>
														<HeaderTemplate>
															<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
														</HeaderTemplate>
														<ItemTemplate>
															<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="AGA_SEQ" SortExpression="AGA_SEQ"  readonly="true" HeaderText="Level">
														<HeaderStyle Width="10%"></HeaderStyle>
														<ItemStyle HorizontalAlign="Left"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="AO_ID"></asp:BoundColumn>
													<asp:BoundColumn DataField="AO_NAME" SortExpression="AO_NAME"  readonly="true" HeaderText="Approving Officer">
														<HeaderStyle Width="28%"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="UM_MASS_APP" SortExpression="UM_MASS_APP" HeaderText="Mass Approval">
														<HeaderStyle Width="5%"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="AAO_ID"></asp:BoundColumn>
													<asp:BoundColumn DataField="AAO_NAME" SortExpression="AAO_NAME" readonly="true" HeaderText="Alternative Approving Officer">
														<HeaderStyle Width="28%"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="AGA_RELIEF_IND" SortExpression="AGA_RELIEF_IND" readonly="true" HeaderText="Relief Staff Control">
														<HeaderStyle Width="14%"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="AGA_GRP_INDEX"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="AAO_ID2"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="AAO_ID3"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="AAO_ID4"></asp:BoundColumn>
												    <asp:BoundColumn Visible="False" DataField="AGA_OFFICER_TYPE" HeaderText="IQC Officer Type">
												        <HeaderStyle Width="10%"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="AGA_BRANCH_CODE"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="AGA_CC_CODE"></asp:BoundColumn>
												</Columns>
											</asp:datagrid></td>
									</tr>
									<tr>
										<td class="emptycol">
											<asp:Label id="lblRed" runat="server" Visible="False"><font color="red">*</font>&nbsp;deleted or inactive user is displayed in red colour</asp:Label></td>
									</tr>
									<tr>
										<td class="emptycol">&nbsp;</td>
									</tr>
									<tr>
										<td class="emptycol" style="HEIGHT: 7px">
											<asp:button id="cmd_Add" runat="server" CssClass="button" Text="Add"></asp:button>
											<asp:button id="btn_Add3" runat="server" CssClass="button" Text="Add Finance Officer" 
											visible="False"></asp:button>
											<asp:button id="btn_Add2" runat="server" CssClass="button" Text="Add Finance Manager" 
												visible="False"></asp:button>
											<asp:button id="btn_Add4" runat="server" CssClass="button" Text="Add Finance Manager" 
												visible="False"></asp:button>	
                                            <%--Zulham 02082018 - PAMB--%>
                                            <asp:button id="btnAddProcurement" runat="server" CssClass="button" Text="Add Procurement Approval" 
												visible="False"></asp:button>
                                            <%--End--%>
											<asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify"></asp:button>
											<asp:button id="cmd_Delete" runat="server" CssClass="button" Text="Delete"></asp:button>
								                <asp:button id ="btnHidden" runat="server" style="display:none" onclick="btnHidden_Click"></asp:button> &nbsp;
								<input id="hidMode" style="WIDTH: 42px; HEIGHT: 22px" type="hidden" size="1" name="hidMode"
												runat="server"/> <input id="hidIndex" style="WIDTH: 49px; HEIGHT: 22px" type="hidden" size="2" name="hidIndex"
												runat="server"/>&nbsp;
										</td>
	            <tr>
					<td class="linespacing2" colspan="4"></td>
			</tr>
								</tr>
								</tbody>
							</table>
						</div>
			<table class="alltable" cellspacing="0" cellpadding="0">
				<tr id="trhid" runat="server">
					<td align="center" >
						<div align="left"><asp:label id="Label4" runat="server"  CssClass="lblInfo"
						Text="a) Please click the Add button to assign new Approving Officer to the Approval Group.<br>b) To remove/un-assign Approving Officer, tick the checkbox and click Delete button."></asp:label>
                        </div>
					</td>
				</tr>
				<tr>
					<td class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<strong>&lt; Back</strong></asp:hyperlink></td>
				</tr>
			</table>
		</form>
	</body>
</html>
