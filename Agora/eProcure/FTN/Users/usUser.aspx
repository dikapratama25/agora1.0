<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="usUser.aspx.vb" Inherits="eProcure.usUserFTN" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>User</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
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
</HEAD>
	<BODY topMargin="10">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_User_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
             <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			</TR>
			
				<TR>
	                <TD colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the relevant info and click the Save button to save the user."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="6"></TD>
			    </TR>
				<TR>
					<TD class="tableheader" colSpan="2">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
				<TR class="tablecol">
					<TD style="WIDTH: 92px; HEIGHT: 6px" width="92" colSpan="2"></TD>
				</TR>
				<tr class="tablecol">
					<TD width="160"><STRONG>&nbsp;User ID</STRONG><span class="errorMsg">*</span>&nbsp;:</TD>
					<TD>&nbsp;<asp:textbox id="txtUser" runat="server" Width="160px" MaxLength="20" CssClass="txtbox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="reqUser" runat="server" ErrorMessage="Required User ID" ControlToValidate="txtUser" Display="None"></asp:RequiredFieldValidator></TD>
				</tr>
				<tr class="tablecol">
					<td style="HEIGHT: 21px"><STRONG>&nbsp;Name</STRONG><span class="errorMsg">*</span>&nbsp;:</td>
					<TD style="HEIGHT: 21px">&nbsp;<asp:textbox id="txtName" runat="server" Width="160px" MaxLength="50" CssClass="txtbox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="reqName" runat="server" ErrorMessage="Required Name" ControlToValidate="txtName" Display="None"></asp:RequiredFieldValidator></TD>
				</tr>
			</table>
			<DIV id="hidlistbox" style="DISPLAY: none" runat="server">
			<TABLE class="alltable" id="Table6" cellSpacing="0" cellPadding="0" border="0">
				<tr class="tablecol" >
					<td style="HEIGHT: 4px"><STRONG>&nbsp;User&nbsp;Group</STRONG><span class="errorMsg">*</span>&nbsp;:</td>
					<TD style="HEIGHT: 0px">
						<table>
							<tr>
								<td><asp:listbox id="lstUGAvail" Width="220" CssClass="listbox" AutoPostBack="false" SelectionMode="Multiple"
										Runat="server" Height="120"></asp:listbox></td>
								<td align="center" width="70"><asp:button id="cmdRight" CssClass="button" Runat="server" CausesValidation="False" text=">"
										width="30"></asp:button><br>
									<br>
									<asp:button id="cmdLeft" CssClass="button" Runat="server" CausesValidation="False" text="<"
										width="30"></asp:button></td>
								<td><asp:listbox id="lstUGSelected" Width="220" CssClass="listbox" AutoPostBack="false" SelectionMode="Multiple"
										Runat="server" Height="120"></asp:listbox></td>
							</tr>
						</table>
                        </TD>
						</tr>
						</TABLE>
						</div>
				<DIV id="hidrb" style="DISPLAY: none" runat="server">
					<TABLE class="alltable" id="Table7" cellSpacing="0" cellPadding="0" border="0" >	
					<tr> 
					<TD class="TableInput" style="width: 160px"><STRONG>&nbsp;User Role</STRONG>&nbsp;:<br />
					<A id="link_term" href="../Users/FTN Functional Matrix.jpg" target="_blank" runat="server">Click Here to view the matrix</A> 
                    </TD>
					<TD class="TableInput">
                    <asp:RadioButtonList id="rbRole"  CssClass="txtbox" runat="server" Width="520px" AutoPostBack = "true" OnSelectedIndexChanged="rbRole_SelectedIndexChanged">
                        <asp:ListItem selected="True" Value="BUYER">Buyer</asp:ListItem>
                        <asp:ListItem Value="PO" Text="Purchasing Officer"></asp:ListItem>
                        <asp:ListItem Value="PM">Purchasing Manager</asp:ListItem>
                        <asp:ListItem Value="AO">Approval Officer</asp:ListItem>
                        <asp:ListItem Value="SK">Storekeeper</asp:ListItem>                        
                    </asp:RadioButtonList>
</TD>
                    </tr>
                    </TABLE>
				</DIV>
				<TABLE class="alltable" id="Table5" cellSpacing="0" cellPadding="0" border="0">
				<tr id="trDept" class="tablecol"  runat="server" >
					<td style="width: 160px"><STRONG>&nbsp;Department Name</STRONG>&nbsp;:</td>
					<TD style="HEIGHT: 12px">&nbsp;<asp:dropdownlist id="cboDeptName" Width="260px" CssClass="ddl" Runat="server"></asp:dropdownlist></TD>
				</tr>
				<tr class="tablecol">
					<td style="width: 160px"><STRONG>&nbsp;Email</STRONG><span class="errorMsg">*</span>&nbsp;:</td>
					<TD>&nbsp;<asp:textbox id="txtEmail" runat="server" Width="260px" MaxLength="50" CssClass="txtbox"></asp:textbox>
                        <asp:RegularExpressionValidator ID="rev_email" runat="server" ControlToValidate="txtEmail"
                            Display="None" EnableClientScript="False" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="reqEmail" runat="server" ErrorMessage="Required Email" ControlToValidate="txtEmail" Display="None"></asp:RequiredFieldValidator>&nbsp;
					</TD>
				</tr>
			</TABLE>
			<table class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0" runat="server">
				<tr class="tablecol">
					<td width="160"><STRONG>&nbsp;Phone No.</STRONG>&nbsp;:</td>
					<TD>&nbsp;<asp:textbox id="txtPhone" runat="server" Width="160px" MaxLength="50" CssClass="txtbox"></asp:textbox>
</TD>
				</tr>
				<tr class="tablecol">
					<td><STRONG>&nbsp;Fax No.</STRONG>&nbsp;:</td>
					<TD>&nbsp;<asp:textbox id="txtFax" runat="server" Width="160px" MaxLength="50" CssClass="txtbox"></asp:textbox>
</TD>
				</tr>
				<tr class="tablecol">
					<td><STRONG>&nbsp;Designation</STRONG><span class="errorMsg">*</span>&nbsp;:</td>
					<TD>&nbsp;<asp:textbox id="txtDesignation" runat="server" Width="160px" MaxLength="50" CssClass="txtbox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="reqDesignation" runat="server" ErrorMessage="Required Designation" ControlToValidate="txtDesignation" Display="None"></asp:RequiredFieldValidator>
					</TD>
				</tr>
				<tr class="tablecol" id="trApprovalLimit" runat="server" visible="false">
					<td><STRONG>&nbsp;PR Approval Limit</STRONG><span class="errorMsg">*</span>&nbsp;:</td>
					<TD>&nbsp;<asp:textbox id="txtAppLimit" runat="server" Width="160px" MaxLength="50" CssClass="txtbox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="reqPRAppLimit" runat="server" ControlToValidate="txtAppLimit"
                            Display="None" ErrorMessage="Required PR Approval Limit"></asp:RequiredFieldValidator>
					</TD>
				</tr>
				<tr class="tablecol" id="trPOApprovalLimit" runat="server" visible="false">
					<td><STRONG>&nbsp;PO Approval Limit</STRONG><span class="errorMsg">*</span>&nbsp;:</td>
					<TD>&nbsp;<asp:textbox id="txtPOAppLimit" runat="server" Width="160px" MaxLength="50" CssClass="txtbox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPOAppLimit"
                            Display="None" ErrorMessage="Required PO Approval Limit"></asp:RequiredFieldValidator>
					</TD>
				</tr>
				<tr class="tablecol" id="trInvoiceApprovalLimit" runat="server" visible="false">
					<td><STRONG>&nbsp;Invoice Approval Limit</STRONG><span class="errorMsg">*</span>&nbsp;:</td>
					<TD>&nbsp;<asp:textbox id="txtInvAppLimit" runat="server" Width="160px" MaxLength="50" CssClass="txtbox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="reqInvAppLimit" runat="server" ControlToValidate="txtInvAppLimit"
                            Display="None" ErrorMessage="Required Invoice Approval Limit"></asp:RequiredFieldValidator></TD>
				</tr>
				<tr class="tablecol">
					<td><STRONG>&nbsp;Password Expiration</STRONG>&nbsp;:</td>
					<TD>&nbsp;<asp:label id="lblPwdExp" Width="160px" CssClass="lblInfo" Runat="server"></asp:label></TD>
				</tr>
			</table>
				<DIV id="hidlnk" style="DISPLAY: none" runat="server">
			<table class="alltable" id="Table9" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92" colSpan="2"></TD>
				</TR>
				<tr class="tablecol">
					<td colSpan="2"><asp:linkbutton id="lnkSetDelAddBuyer" CssClass="HpLnk" Runat="server" Visible="False">[ Set Delivery address - Buyer ]</asp:linkbutton><asp:linkbutton id="lnkSetBillAddBuyer" CssClass="HpLnk" Runat="server" Visible="False">[ Set Billing address - Buyer ]</asp:linkbutton><asp:linkbutton id="lnkSetDelAdd" CssClass="HpLnk" Runat="server" Visible="False">[ Set Delivery address - Store Keeper/2nd Level Receiving ]</asp:linkbutton><asp:linkbutton id="lnkMassApp" CssClass="HpLnk" Runat="server" Visible="False">[ Mass Approval ]</asp:linkbutton><asp:linkbutton id="lnkFinVwDept" CssClass="HpLnk" Runat="server" Visible="False">[ Set Finance Viewing Department ]</asp:linkbutton></td>
				</tr>
				<TR>
					<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92" colSpan="2"></TD>
				</TR>
			</table>
			</DIV>
			<table class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
				<tr class="tablecol" id="trMassAppr" runat="server"  visible="false">
					<td><STRONG>&nbsp;Mass Approval</STRONG>&nbsp;:</td>
					<TD><asp:checkbox id="chkMassAppr" Runat="server"></asp:checkbox>
					<asp:Label id="lbl1" runat="server" Text="Mass Approval authorise approving officer to approve more than one PO at a time"></asp:label></td>
					<td></td>
				</tr>
				<tr class="tablecol">
					<td width="160"><STRONG>&nbsp;Status</STRONG>&nbsp;:</td>
					<TD><asp:radiobutton id="rdAct" Width="80" CssClass="Rbtn" Runat="server" GroupName="grpStatus" Checked="True"
							Text="Active"></asp:radiobutton><asp:radiobutton id="rdDeAct" Width="80" CssClass="Rbtn" Runat="server" GroupName="grpStatus" Text="Inactive"></asp:radiobutton></TD>
					<td></td>
				</tr>
				<tr class="tablecol">
					<td><STRONG>&nbsp;Account Locked</STRONG>&nbsp;:</td>
					<TD><asp:checkbox id="chkAccLock" Runat="server"></asp:checkbox></TD>
					<td></td>
				</tr>
				<TR>
					<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92" colSpan="3"></TD>
				</TR>
				<TR class="emptycol">
					<TD style="HEIGHT: 7px" colspan="2"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</TD>
					<td style="HEIGHT: 7px"></td>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;<INPUT id="hidPageCount" type="hidden" name="hidPageCount" runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
			</table>
			<table class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD colSpan="2"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdGeneratePwd" runat="server" Width="150" CssClass="button" Text="Generate Password"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>&nbsp;
					<INPUT class="button" id="cmdAdd" type="button" value="Add" name="cmdAdd" runat="server">&nbsp;
					<INPUT class="button" id="cmdReset" type="button" value="Reset" name="cmdReset" runat="server"
							onclick="ValidatorReset();"></TD>
				</TR>
				<TR>
					<TD>
                        <asp:ValidationSummary ID="vldsummary" runat="server" CssClass="errormsg" />
                        <br>
						<asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</table>
		</form>
	</BODY>
</HTML>
