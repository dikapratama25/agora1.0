<%@ Page Language="vb" AutoEventWireup="false" Codebehind="usDeliveryAddr.aspx.vb" Inherits="eProcure.usDeliveryAddr" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title></title>
		<style>
.ButtonCSS { FONT-SIZE: 8pt; CURSOR: hand; HEIGHT: 18px; BACKGROUND-COLOR: lightgrey }
		</style>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="VBScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
				
				
				function selectAll()
				{
					SelectAllG("dgAddr_ctl02_chkAll","chkSelection");
				}
						
				function checkChild(id)
				{
					checkChildG(id,"dgAddr_ctl02_chkAll","chkSelection");
				}
						
				function ResetSearch(){
					//debugger;
					Form1.txt_Code.value = "";
					Form1.txt_City.value = "";
					Form1.cbo_State.selectedIndex = 0;
					Form1.cboUser.selectedIndex = 0;
					Form1.cbo_Country.selectedIndex = 0;
				}
				/*function CheckAtLeastOneNotAll(pChkSelName){
					if (Form1.hidAll.value == '0'){
						var oform = document.forms[0];
						re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
						for (var i=0;i<oform.elements.length;i++)
						{
							var e = oform.elements[i];
							if (e.type=="checkbox"){
								if (re.test(e.name) && e.checked==true)
									return true;
							}
						}
						//alert('Please make at least one selection!');
						//return false;
					}
					else
						return true;
				}*/

		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_UserAddr_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0"  cellpadding="0" width="100%" border="0">
            <tr>
					<td class="linespacing1" colspan="6"></td>
			    </tr>
				<tr>
	                <td colspan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant address. Click the Save button to save the changes."
		                ></asp:label>

	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colspan="6"></td>
			    </tr>
				<tr>
				<td class="TableHeader" colspan="6">&nbsp;<asp:Label ID="Label1" runat="server" Text="Search Criteria"></asp:Label></td>
				</tr>
				<tr>
				<td class="TableCol" width="20%" ><strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="User Name"></asp:Label></strong>: </td>
				<td class="TableCol" colspan="4"><asp:dropdownlist id="cboUser" runat="server" AutoPostBack="True" CssClass="txtbox" Width="100%" ></asp:dropdownlist></td>
				<td class="TableCol"></td>
				</tr>
				<tr>
				    <td class="tablecol" width="20%" >&nbsp;<strong><asp:Label ID="Label3" runat="server"
                        Text="Code"></asp:Label> </strong>: </td>
				    <td class="tablecol" width="5%" ><asp:textbox id="txt_Code" runat="server" CssClass="txtbox" MaxLength="20"></asp:textbox></td>
				    <td class="tablecol" width="1%"></td>
				    <td class="tablecol" width="10%" >&nbsp;<strong><asp:Label ID="Label4" runat="server"
                        Text="City"></asp:Label> </strong>:</td>
				    <td class="tablecol" width="10%" ><asp:textbox id="txt_City" runat="server" CssClass="txtbox" MaxLength="20"></asp:textbox>&nbsp;<strong></td>
				    <td class="tablecol" width="65%"></td>
				</tr>
				<tr>
				    <td class="tablecol">&nbsp;<strong><asp:Label ID="Label5" runat="server" Text="State"></asp:Label> </strong>:</td>
				    <td class="tablecol" colspan="3"><asp:dropdownlist id="cbo_State" runat="server" CssClass="txtbox" Width="128px"></asp:dropdownlist></td>
				    <td class="tablecol" width="1%" colspan="2"></td>
				</tr>
				<tr>
				    <td class="tablecol">&nbsp;<strong><asp:Label ID="Label6" runat="server" Text="Country"></asp:Label> </strong>:</td>
				    <td class="tablecol" colspan="3"><asp:dropdownlist id="cbo_Country" AutoPostBack="True" runat="server" CssClass="txtbox" Width="128px"></asp:dropdownlist></td>
				    <td class="tablecol" width="1%" colspan="2"></td>
				</tr>
				<tr>
				    <td class="tablecol" width="25%" runat="server">&nbsp;<strong><asp:Label Text="Delivery Address for " ID="lblAddFor" runat="server"></asp:Label></strong><asp:Label ID="lblAddFor1" runat="server">:</asp:Label></td>
				    <td class="tablecol" colspan="4" width="20%">
                        <asp:radiobuttonlist ID="rbFixedRole" runat="server" BorderStyle="None" CssClass="rbtn" RepeatDirection="Horizontal" AutoPostBack="true"> 
                            <asp:ListItem Value="Buyer">Buyer Role</asp:ListItem>
							<asp:ListItem Value="SK">Store Keeper Role</asp:ListItem>
						</asp:radiobuttonlist>
						<!-- All address Radiobtn -->
                        <asp:RadioButtonList ID="rbtnType" AutoPostBack="true" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="allAddr" Text="All Address"></asp:ListItem>
						<asp:ListItem Value="selectedAddr" Selected="true" Text=""></asp:ListItem>
                        </asp:RadioButtonList>							
					</td>
					<td class="tablecol" align="right">
                        <asp:button id="cmd_Search" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
                        <asp:button id="cmd_Clear" runat="server" CssClass="button" Text="Clear"></asp:button>
			        </td>
				</tr>
	</table>
						
	<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" border="0" width="100%">
				<tr>
					<td><asp:datagrid id="dgAddr" runat="server" DataKeyField="AM_ADDR_CODE">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All" Checked="True"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="AM_ADDR_CODE" SortExpression="AM_ADDR_CODE" HeaderText="Code">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_ADDR_LINE1" SortExpression="AM_ADDR_LINE1" HeaderText="Address">
									<HeaderStyle Width="35%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_CITY" SortExpression="AM_CITY" HeaderText="City">
									<HeaderStyle Width="17%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STATE" SortExpression="STATE" HeaderText="State">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_POSTCODE" SortExpression="AM_POSTCODE" HeaderText="Post Code">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="COUNTRY" SortExpression="COUNTRY" HeaderText="Country">
									<HeaderStyle Width="12%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_SELECTED" SortExpression="AM_SELECTED" HeaderText="Selected" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
				<tr>
					<td>
					    <asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>
                        <asp:Button ID="cmdReset" runat="server" CssClass="button" Text="Reset" />
					    <asp:button id="cmdRemoveAll" runat="server" CssClass="button" Width="180px" Text="Remove All"></asp:button>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
