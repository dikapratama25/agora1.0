<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BranchCostCentreMaint.aspx.vb" Inherits="eProcure.BranchCostCentreMaint" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>BranchCostCentreMaint</title>
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
			SelectAllG("dtgBCC_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgBCC_ctl02_chkAll","chkSelection");
		}
		
		function ShowDialog(filename,height,width)
		{
				
			var retval="";
			retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth:" + width);
			//retval=window.open(filename);
			if (retval == "1" || retval =="" || retval==null)
			{  window.close;
				return false;

			} else {
			    window.close;
				return true;

			}
		}
		
		function Reset(){
			var oform = document.forms(0);
		
			oform.cboCompany.selectedIndex = 0;
			oform.txtBranchCode.value="";
		}
	
		-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
			<tr>
				<td class="header" colspan="4" style="height: 3px" width="100"></td>
			</tr>
			<tr>
				<td class="header" colspan="4"><asp:label id="lbl_title" runat="server">Branch - Cost Centre Control</asp:label></td>
			</tr>
			<tr>
				<td class="linespacing1" colspan="4"></td>
			</tr>
			<tr>
				<td>
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Fill in the search criteria and click Search button to list the relevant Branch information."></asp:label>
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
								<td class="tablecol" width="15%">&nbsp;<strong>Company </strong>:</td>
								<td class="tablecol" width="20%">
                                    <asp:DropDownList ID="cboCompany" runat="server" cssclass="ddl" width="100px">
                                        <asp:ListItem Value="" Selected="True">---Select---</asp:ListItem>
                                        <asp:ListItem Value="HLB">HLB</asp:ListItem>
                                        <asp:ListItem Value="HLISB">HLISB</asp:ListItem>
                                    </asp:DropDownList></td>
								<td class="tablecol" width="15%">&nbsp;<strong>Branch Code </strong>:</td>
								<td class="tablecol" width="20%"><asp:textbox id="txtBranchCode" runat="server" MaxLength="50" CssClass="txtbox" width="200px"></asp:textbox></td>
								<td class="tablecol" width="30%" align="right" ><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/></td>
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
						<p><asp:datagrid id="dtgBCC" runat="server" OnPageIndexChanged="dtgBCC_Page" AllowSorting="True" AutoGenerateColumns="False" Width="100%" OnSortCommand="SortCommand_Click" Visible="False">
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
									<asp:BoundColumn DataField="BCC_COY_ID" SortExpression="BCC_COY_ID" HeaderText="Company ID">
										<HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="BCC_BRANCH_CODE" SortExpression="BCC_BRANCH_CODE" HeaderText="Branch Code">
										<HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>	
									<asp:BoundColumn DataField="CBM_BRANCH_NAME" SortExpression="CBM_BRANCH_NAME" HeaderText="Description">
										<HeaderStyle Width="20%"></HeaderStyle>
									</asp:BoundColumn>	
									<asp:BoundColumn HeaderText="Cost Centre">
										<HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>	
									<asp:BoundColumn HeaderText="Description">
										<HeaderStyle Width="30%"></HeaderStyle>
									</asp:BoundColumn>								
								</Columns>
							</asp:datagrid></p>
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 6px; width: 1192px;"></td>
				</tr>
				<tr style="height: 50px">
					<td class="emptycol" style="width: 1192px"><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;
					<asp:button id="cmdModify" runat="server" Width="59px" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>&nbsp;
					<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>
					<asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none" onclick="btnHidden1_Click"></asp:button>
					<input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"/>
					<input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server"/>&nbsp;
                        </td>
				</tr>
			</table>
		</form>
	</body>
</html>
