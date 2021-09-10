<%@ Page Language="vb" AutoEventWireup="false" Codebehind="usUser.aspx.vb" Inherits="eProcure.usUser" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
		<title>User</title>
		<meta http-equiv="Content-Type" content="text/html; charset=windows-1252"/>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
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
			function goback ()
			{
			    document.getelementById("lnkBack").click();
			    
			}
		</script>
</head>
	<body topmargin="10">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_User_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
             <tr>
					<td class="linespacing1" colspan="4"></td>
			</tr>
			
				<tr>
	                <td colspan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the relevant info and click the Save button to save the user."
		                ></asp:label>

	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colspan="6"></td>
			    </tr>
				<tr>
					<td class="tableheader" colspan="2">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></td>
				</tr>
				<tr class="tablecol">
					<td style="WIDTH: 92px; HEIGHT: 6px" width="92" colspan="2"></td>
				</tr>
				<tr class="tablecol">
					<td width="20%"><strong>&nbsp;User ID</strong><span class="errorMsg">*</span>&nbsp;:</td>
					<td width="80%">&nbsp;<asp:textbox id="txtUser" runat="server" Width="160px" MaxLength="20" CssClass="txtbox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="reqUser" runat="server" ErrorMessage="Required User ID" ControlToValidate="txtUser" Display="None"></asp:RequiredFieldValidator></td>
				</tr>
				<tr class="tablecol">
					<td style="HEIGHT: 21px" width="20%"><strong>&nbsp;Name</strong><span class="errorMsg">*</span>&nbsp;:</td>
					<td style="HEIGHT: 21px" width="80%">&nbsp;<asp:textbox id="txtName" runat="server" Width="160px" MaxLength="50" CssClass="txtbox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="reqName" runat="server" ErrorMessage="Required Name" ControlToValidate="txtName" Display="None"></asp:RequiredFieldValidator></td>
				</tr>
			</table>
			<div id="hidlistbox" style="DISPLAY: none" runat="server">
			<table class="alltable" id="Table6" cellspacing="0" cellpadding="0" border="0">
				<tr class="tablecol" >
					<td style="HEIGHT: 4px" width="20%"><strong>&nbsp;User&nbsp;Group</strong><span class="errorMsg">*</span>&nbsp;:</td>
					<td style="HEIGHT: 0px">
						<table>
							<tr>
								<td><asp:listbox id="lstUGAvail" Width="220" CssClass="listbox" AutoPostBack="false" SelectionMode="Multiple"
										Runat="server" Height="120"></asp:listbox></td>
								<td align="center" width="70"><asp:button id="cmdRight" CssClass="button" Runat="server" CausesValidation="False" text=">"
										width="30"></asp:button><br/>
									<br/>
									<asp:button id="cmdLeft" CssClass="button" Runat="server" CausesValidation="False" text="<"
										width="30"></asp:button></td>
								<td><asp:listbox id="lstUGSelected" Width="220" CssClass="listbox" AutoPostBack="false" SelectionMode="Multiple"
										Runat="server" Height="120"></asp:listbox></td>
							</tr>
						</table>
                        </td>
						</tr>
						</table>
						</div>
				<div id="hidrb" style="DISPLAY: none" runat="server">
					<table class="alltable" id="Table7" cellspacing="0" cellpadding="0" border="0" >	
					<tr> 
					<td class="Tableinput" style="width: 160px"><strong>&nbsp;User Role</strong>&nbsp;:<br />
					<a id="link_term" href="../Users/FTN Functional Matrix.jpg" target="_blank" runat="server">Click Here to view the matrix</a> 
                    </td>
					<td class="Tableinput">
                    <asp:RadioButtonList id="rbRole"  CssClass="txtbox" runat="server" Width="520px" AutoPostBack = "true" OnSelectedIndexChanged="rbRole_SelectedIndexChanged">
                        <asp:ListItem selected="True" Value="BUYER">Buyer</asp:ListItem>
                        <asp:ListItem Value="PO" Text="Purchasing Officer"></asp:ListItem>
                        <asp:ListItem Value="PM">Purchasing Manager</asp:ListItem>
                        <asp:ListItem Value="AO">Approval Officer</asp:ListItem>
                        <asp:ListItem Value="SK">Storekeeper</asp:ListItem>                        
                    </asp:RadioButtonList>
</td>
                    </tr>
                    </table>
				</div>
				<table class="alltable" id="Table5" cellspacing="0" cellpadding="0" border="0">
				<tr id="trDept" class="tablecol"  runat="server" >
					<td style="width: 160px"><strong>&nbsp;Department Name</strong>&nbsp;:</td>
					<td style="HEIGHT: 12px">&nbsp;<asp:dropdownlist id="cboDeptName" Width="260px" CssClass="ddl" Runat="server"></asp:dropdownlist></td>
				</tr>
				<tr class="tablecol">
					<td style="width: 160px"><strong>&nbsp;Email</strong><span class="errorMsg">*</span>&nbsp;:</td>
					<td>&nbsp;<asp:textbox id="txtEmail" runat="server" Width="260px" MaxLength="50" CssClass="txtbox"></asp:textbox>
                        <asp:RegularExpressionValidator ID="rev_email" runat="server" ControlToValidate="txtEmail"
                            Display="None" EnableClientScript="False" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="reqEmail" runat="server" ErrorMessage="Required Email" ControlToValidate="txtEmail" Display="None"></asp:RequiredFieldValidator>&nbsp;
					</td>
				</tr>
			</table>
			<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0" runat="server">
				<tr class="tablecol">
					<td width="160"><strong>&nbsp;Phone No.</strong>&nbsp;:</td>
					<td>&nbsp;<asp:textbox id="txtPhone" runat="server" Width="160px" MaxLength="50" CssClass="txtbox"></asp:textbox>
</td>
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;Fax No.</strong>&nbsp;:</td>
					<td>&nbsp;<asp:textbox id="txtFax" runat="server" Width="160px" MaxLength="50" CssClass="txtbox"></asp:textbox>
</td>
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;Designation</strong><span class="errorMsg">*</span>&nbsp;:</td>
					<td>&nbsp;<asp:textbox id="txtDesignation" runat="server" Width="160px" MaxLength="50" CssClass="txtbox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="reqDesignation" runat="server" ErrorMessage="Required Designation" ControlToValidate="txtDesignation" Display="None"></asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr class="tablecol" id="trApprovalLimit" runat="server" visible="false">
					<td><strong>&nbsp;PR Approval Limit</strong><span class="errorMsg">*</span>&nbsp;:</td>
					<td>&nbsp;<asp:textbox id="txtAppLimit" runat="server" Width="160px" MaxLength="20" CssClass="txtbox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="reqPRAppLimit" runat="server" ControlToValidate="txtAppLimit"
                            Display="None" ErrorMessage="Required PR Approval Limit"></asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr class="tablecol" id="trPOApprovalLimit" runat="server" visible="false">
					<td><strong>&nbsp;PO Approval Limit</strong><span class="errorMsg">*</span>&nbsp;:</td>
					<td>&nbsp;<asp:textbox id="txtPOAppLimit" runat="server" Width="160px" MaxLength="20" CssClass="txtbox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="reqPOAppLimit" runat="server" ControlToValidate="txtPOAppLimit"
                            Display="None" ErrorMessage="Required PO Approval Limit"></asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr class="tablecol" id="trInvoiceApprovalLimit" runat="server" visible="false">
					<td><strong>&nbsp;Invoice Approval Limit</strong><span class="errorMsg">*</span>&nbsp;:</td>
					<td>&nbsp;<asp:textbox id="txtInvAppLimit" runat="server" Width="160px" MaxLength="20" CssClass="txtbox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="reqInvAppLimit" runat="server" ControlToValidate="txtInvAppLimit"
                            Display="None" ErrorMessage="Required Invoice Approval Limit"></asp:RequiredFieldValidator></td>
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;Password Expiration</strong>&nbsp;:</td>
					<td>&nbsp;<asp:label id="lblPwdExp" Width="160px" CssClass="lblInfo" Runat="server"></asp:label></td>
				</tr>
			</table>
				<div id="hidlnk" style="DISPLAY: none" runat="server">
			<table class="alltable" id="Table9" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92" colspan="2"></td>
				</tr>
				<tr class="tablecol">
					<td colspan="2"><asp:linkbutton id="lnkSetDelAddBuyer" CssClass="HpLnk" Runat="server" Visible="False">[ Set Delivery address - Buyer ]</asp:linkbutton><asp:linkbutton id="lnkSetBillAddBuyer" CssClass="HpLnk" Runat="server" Visible="False">[ Set Billing address - Buyer ]</asp:linkbutton><asp:linkbutton id="lnkSetDelAdd" CssClass="HpLnk" Runat="server" Visible="False">[ Set Delivery address - Store Keeper/2nd Level Receiving ]</asp:linkbutton><asp:linkbutton id="lnkMassApp" CssClass="HpLnk" Runat="server" Visible="False">[ Mass Approval ]</asp:linkbutton><asp:linkbutton id="lnkFinVwDept" CssClass="HpLnk" Runat="server" Visible="False">[ Set Finance Viewing Department ]</asp:linkbutton></td>
				</tr>
				<tr>
					<td class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92" colspan="2"></td>
				</tr>
			</table>
			</div>
			<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" border="0">
				<tr class="tablecol" id="trMassAppr" runat="server" visible="false">
					<td><strong>&nbsp;Mass Approval</strong>&nbsp;:</td>
					<td>
					    <div id="divchk1" runat="server"><asp:checkbox id="chkMassAppr" Runat="server"></asp:checkbox>
					    <asp:Label id="lbl1" runat="server" Text="Mass Approval authorise approving officer to approve more than one PR/PO at a time"></asp:Label></div>
					    <div id="divchk2" runat="server"><asp:checkbox id="chkInvMassAppr" Runat="server"></asp:checkbox>
					    <asp:Label id="lbl2" runat="server" Text="Mass Approval authorise approving officer to approve more than one Invoice at a time"></asp:Label><br/></div> 
					    <div id="divchk3" runat="server"><asp:checkbox id="chkMrsMassAppr" Runat="server"></asp:checkbox>
					    <asp:Label id="lbl3" runat="server" Text="Mass Approval authorise approving officer to approve more than one MRS at a time"></asp:Label></div> 
					</td>
					<td></td>
				</tr>
				<tr class="tablecol">
					<td width="160"><strong>&nbsp;Status</strong>&nbsp;:</td>
					<td><asp:radiobutton id="rdAct" Width="80" CssClass="Rbtn" Runat="server" GroupName="grpStatus" Checked="True"
							Text="Active"></asp:radiobutton><asp:radiobutton id="rdDeAct" Width="80" CssClass="Rbtn" Runat="server" GroupName="grpStatus" Text="Inactive"></asp:radiobutton></td>
					<td></td>
				</tr>
				<tr class="tablecol" id="trStockType" runat="server" visible="false">
					<td><strong>&nbsp;Item Type</strong>&nbsp;:</td>
					<td>
					    <asp:checkbox id="chkSpot" Runat="server" Checked="True"></asp:checkbox>
					    <asp:Label id="lblSpot" runat="server" Text="Spot"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					    <asp:checkbox id="chkStock" Runat="server" Checked="True"></asp:checkbox>
					    <asp:Label id="lblStock" runat="server" Text="Stock"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					    <asp:checkbox id="chkMro" Runat="server" Checked="True"></asp:checkbox>
					    <asp:Label id="lblMro" runat="server" Text="MRO, M&E and IT"></asp:Label>
					</td>
					<td></td>
				</tr>
				<tr class="tablecol">
					<td><strong>&nbsp;Account Locked</strong>&nbsp;:</td>
					<td><asp:checkbox id="chkAccLock" Runat="server"></asp:checkbox></td>
					<td></td>
				</tr>
				<tr>
					<td class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92" colspan="3"></td>
				</tr>
				<tr class="emptycol">
					<td style="HEIGHT: 7px" colspan="2"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</td>
					<td style="HEIGHT: 7px"></td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;<input id="hidPageCount" type="hidden" name="hidPageCount" runat="server"/></td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
			</table>
			<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td colspan="2"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdGeneratePwd" runat="server" Width="150" CssClass="button" Text="Generate Password"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>&nbsp;
					<input class="button" id="cmdAdd" type="button" value="Add" name="cmdAdd" runat="server"/>&nbsp;
					<input class="button" id="cmdReset" type="button" value="Reset" name="cmdReset" runat="server"
							onclick="ValidatorReset();"/></td>
				</tr>
				<tr>
					<td>
                        <asp:ValidationSummary ID="vldsummary" runat="server" CssClass="errormsg" />
                        <br/>
						<asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue">
							<strong>&lt; Back</strong></asp:hyperlink></td>
				</tr>
			</table>
		</form>
	</body>
</html>
