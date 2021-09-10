<%@ Page Language="vb" AutoEventWireup="false" Codebehind="VendorReq.aspx.vb" Inherits="eProcure.VendorReq" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>NewCountry</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="712" border="0">
				<TR>
					<TD class="header"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="left">
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="550" border="0">
							<TR>
								<TD class="tableheader" COLSPAN="2">&nbsp;
									<asp:label id="lblHeader" runat="server"></asp:label>
								</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 125px; HEIGHT: 16px" width="125"></TD>
								<TD class="TableInput" style="HEIGHT: 16px"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 250px; HEIGHT: 19px" width="125">
									<asp:label id="lblCode" runat="server" Font-Bold="True"></asp:label><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>
									&nbsp;:&nbsp;
								</TD>
								<TD class="TableInput" style="HEIGHT: 19px"><asp:textbox id="txt_CountryCode" runat="server" CssClass="txtbox" Width="150px" MaxLength="10"></asp:textbox><asp:requiredfieldvalidator id="vldCountryCode" runat="server" ErrorMessage="<%=lblCode.Text%> Required" ControlToValidate="txt_CountryCode" Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 250px; HEIGHT: 17px" width="125">
									<asp:label id="lblDesc" runat="server" Font-Bold="True"></asp:label><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>
									&nbsp;:&nbsp;
								</TD>
								<TD class="TableInput" style="HEIGHT: 17px"><asp:textbox id="txt_CountryDesc" runat="server" CssClass="txtbox" Width="361px" MaxLength="50"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="vldCountryDesc" runat="server" ErrorMessage="Mandatory" ControlToValidate="txt_CountryDesc"
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR class="tablecol">
								<TD style="HEIGHT: 7px"></TD>
								<td style="HEIGHT: 7px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
							</TR>
							<TR class="emptycol">
								<TD style="HEIGHT: 7px"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
									required field</TD>
								<td style="HEIGHT: 7px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 24px" align="left"><asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" type="reset" value="Clear" name="cmdReset" runat="server">&nbsp;
					</TD>
				</TR>
				<TR>
					<TD><br>
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary><br>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
