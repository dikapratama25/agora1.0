<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UsUserHub2.aspx.vb" Inherits="eAdmin.usUserHub2" smartNavigation="false" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>User</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
        </script>
        <% Response.Write(sCSS)%>
		
		<script language="javascript">
		<!--
			function Reset(){
			window.location.reload();
			}
		//-->
		</script>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" colSpan="3"><FONT size="3">User Account Maintenance</FONT></TD>
				</TR>
				<tr>
					<td colSpan="3">&nbsp;</td>
				</tr>
				<TR>
					<TD class="tableheader" colSpan="3">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92" colSpan="3"></TD>
				</TR>
				<tr class="tablecol">
					<TD width="160"><STRONG>&nbsp;User ID</STRONG><span class="errorMsg">*</span>&nbsp;:</TD>
					<TD width="200"><asp:textbox id="txtUser" runat="server" Enabled="False" Width="160px" MaxLength="20" CssClass="txtbox"></asp:textbox></TD>
					<TD class="tablecol"><asp:requiredfieldvalidator id="rfv_txtUser" runat="server" ControlToValidate="txtUser" ErrorMessage="User Id Required"
							Display="None"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="revUserId" runat="server" ControlToValidate="txtUser" ErrorMessage="Invalid User Id."
							Display="None" ValidationExpression="^[a-zA-Z0-9_]+$"></asp:regularexpressionvalidator></TD>
				</tr>
				<tr class="tablecol">
					<td><STRONG>&nbsp;User Name</STRONG><span class="errorMsg">&nbsp;*</span>&nbsp;:</td>
					<TD><asp:textbox id="txtName" runat="server" Width="160px" MaxLength="50" CssClass="txtbox"></asp:textbox></TD>
					<TD class="tablecol"><asp:requiredfieldvalidator id="rfv_txtName" runat="server" ControlToValidate="txtName" ErrorMessage="User Name Required"
							Display="None"></asp:requiredfieldvalidator></TD>
				</tr>
				<tr class="tablecol">
					<td><STRONG>&nbsp;Email</STRONG><span class="errorMsg">*</span>&nbsp;:</SPAN></td>
					<TD><asp:textbox id="txtEmail" runat="server" Width="260px" MaxLength="50" CssClass="txtbox"></asp:textbox>&nbsp;</TD>
					<TD class="tablecol"><asp:requiredfieldvalidator id="rfv_txtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required"
							Display="None"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="rev_email" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid Email"
							Display="None" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:regularexpressionvalidator></TD>
				</tr>
				<tr class="tablecol">
					<td colSpan="3">&nbsp;</td>
				</tr>
			</TABLE>
			<!--Buyer admin and Vendor admin
			<table class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
				<tr class="tablecol">
					<td width="160"><STRONG>&nbsp;Phone No</STRONG>:</td>
					<TD><asp:textbox id="txtPhone" runat="server" CssClass="txtbox" MaxLength="50" Width="160px"></asp:textbox></TD>
				</tr>
				<tr class="tablecol">
					<td><STRONG>&nbsp;Fax No</STRONG>:</td>
					<TD><asp:textbox id="txtFax" runat="server" CssClass="txtbox" MaxLength="50" Width="160px"></asp:textbox></TD>
				</tr>
				<tr class="tablecol">
					<td><STRONG>&nbsp;Designation</STRONG>:</td>
					<TD><asp:textbox id="Textbox1" runat="server" CssClass="txtbox" MaxLength="50" Width="160px"></asp:textbox></TD>
				</tr>
				<tr class="tablecol">
					<td><STRONG>&nbsp;Approval Limit</STRONG>:</td>
					<TD><asp:textbox id="Textbox2" runat="server" CssClass="txtbox" MaxLength="50" Width="160px"></asp:textbox></TD>
				</tr>
				<tr class="tablecol">
					<td><STRONG>&nbsp;Password Expiration</STRONG>:</td>
					<TD><asp:Label id="lblPwdExp" Runat="server" CssClass="lbl" Width="160px"></asp:Label>
					</TD>
				</tr>
			</table>
			End Buyer admin and Vendor admin-->
			<table class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
				<tr class="tablecol">
					<td width="160"><STRONG>&nbsp;Status</STRONG>&nbsp;:</td>
					<TD colSpan="2"><asp:radiobutton id="rdAct" Width="80" CssClass="Rbtn" GroupName="grpStatus" Checked="True" Text="Active"
							Runat="server"></asp:radiobutton><asp:radiobutton id="rdDeAct" Width="80" CssClass="Rbtn" GroupName="grpStatus" Text="Inactive" Runat="server"></asp:radiobutton></TD>
				</tr>
				<tr class="tablecol">
					<td><STRONG>&nbsp;Account Locked</STRONG>&nbsp;:</td>
					<TD colSpan="2"><asp:checkbox id="chkAccLock" Runat="server"></asp:checkbox></TD>
				</tr>
				<TR class="tablecol">
					<TD style="WIDTH: 92px; HEIGHT: 6px" colSpan="3"></TD>
				</TR>
				<TR class="emptycol">
					<TD style="HEIGHT: 7px"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</TD>
					<td style="HEIGHT: 7px" colSpan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
				</TR>
				<TR class="emptycol">
					<TD colSpan="3">&nbsp;</TD>
				</TR>
			</table>
			<table class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD><asp:datagrid id="dgUserGroup" runat="server" Width="100%" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="AP_APP_NAME" HeaderText="Package">
									<HeaderStyle HorizontalAlign="Left" Width="45%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="AP_APP_ID" HeaderText="Package Id"></asp:BoundColumn>
								<asp:BoundColumn DataField="USER_TYPE" HeaderText="User Type">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="User Group">
									<HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboUserGroup" runat="server" CssClass="ddl" Width="90%"></asp:DropDownList>
										<asp:RequiredFieldValidator id="rfv_UserGroup" runat="server" Display="Dynamic" ErrorMessage="User group is required."
											ControlToValidate="cboUserGroup">*</asp:RequiredFieldValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD colSpan="2"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdGeneratePwd" runat="server" Width="150" CssClass="button" Text="Generate Password"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>&nbsp;<asp:button id="cmdReset" runat="server" CssClass="button" Text="Reset" CausesValidation="False"></asp:button></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:validationsummary id="vldsumm" runat="server" Width="352px" CssClass="errormsg"></asp:validationsummary></TD>
				</TR>
				<TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="#">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</table>
		</form>
	</BODY>
</HTML>
