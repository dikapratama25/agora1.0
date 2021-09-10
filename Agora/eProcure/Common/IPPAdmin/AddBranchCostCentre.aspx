<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AddBranchCostCentre.aspx.vb" Inherits="eProcure.AddBranchCostCentre" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>AddBranchCostCentre</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>	
        <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		
        function selectAll()
		{
			SelectAllG("dtgCC_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgCC_ctl02_chkAll","chkSelection");
		}  
        
        function Close()
        {
            window.close();					
        }
        
        function Reset(){
            var mode = document.getElementById("hidMode").value
			var oform = document.forms(0);
			
			if (mode == "add") {
			    oform.txtBranchCode.value="";
			    oform.txtBranchCode.disabled = false ;
			    oform.txtCostCentre.value="";
			    oform.txtDesc.value="";
			}
			else
			{
			    oform.txtCostCentre.value="";
			    oform.txtDesc.value="";
			}
		}
        
		</script>
	</head>
	<body class="body" ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
			<tr>
				<td class="header" colspan="4" style="height: 3px" width="100"></td>
			</tr>
			<tr>
				<td class="header" colspan="4"><asp:label id="lblTitle" runat="server"></asp:label></td>
			</tr>
			<tr>
				<td class="linespacing1" colspan="4"></td>
			</tr>
			<tr>
				<td class="emptycol">
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"></asp:label>
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
								<td class="tableheader" colspan="5" width="100%">&nbsp;Select Cost Centre</td>
							</tr>
							<tr class="tablecol">
							    <td class="tablecol" width="15%">&nbsp;<strong>Branch Code</strong><asp:label id="Label19" runat="server" ForeColor="Red">*</asp:label> :</td>
								<td class="tablecol" width="25%"><asp:textbox id="txtBranchCode" runat="server" MaxLength="50" CssClass="txtbox" width="150px"></asp:textbox></td>    
								<td class="tablecol" colspan="3"></td> 
							</tr>
							<tr class="tablecol" id="trGrpType" runat="server">
								<td class="tablecol" width="15%">&nbsp;<strong>Cost Centre </strong>:</td>
								<td class="tablecol" width="25%"><asp:textbox id="txtCostCentre" runat="server" MaxLength="50" CssClass="txtbox" width="150px"></asp:textbox></td>
								<td class="tablecol" width="15%">&nbsp;<strong>Description </strong>:</td>
								<td class="tablecol" width="25%"><asp:textbox id="txtDesc" runat="server" MaxLength="50" CssClass="txtbox" width="150px"></asp:textbox></td>
								<td class="tablecol" width="20%" align="right" ><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button></td>
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
						<p><asp:datagrid id="dtgCC" runat="server" OnPageIndexChanged="dtgCC_Page" AllowSorting="True" AutoGenerateColumns="False" Width="100%" OnSortCommand="SortCommand_Click">
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
									<asp:BoundColumn DataField="CC_CC_CODE" SortExpression="CC_CC_CODE" HeaderText="Cost Centre">
										<HeaderStyle Width="35%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CC_CC_DESC" SortExpression="CC_CC_DESC" HeaderText="Description">
										<HeaderStyle Width="60%"></HeaderStyle>
									</asp:BoundColumn>		
									<asp:BoundColumn Visible="False" HeaderText="INDEX"></asp:BoundColumn>							
								</Columns>
							</asp:datagrid></p>
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 6px; width: 1192px;"></td>
				</tr>
				<tr style="height: 50px">
					<td class="emptycol" style="width: 1192px"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save" CausesValidation="False"></asp:button>
					<asp:button id="cmdDelete" runat="server" Width="59px" CssClass="button" Text="Delete" CausesValidation="False"></asp:button>&nbsp;
					<input class="button" id="cmdClose" onclick="window.close();" type="button" value="Close" name="cmdClose" runat="server">
					<input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"/>
                        </td>
				</tr>
			</table>
		</form>
	</body>
</html>
