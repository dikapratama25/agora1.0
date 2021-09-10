<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AddAdds.aspx.vb" Inherits="eProcure.AddAdds" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>AddAdds</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">		
		<script runat="server">
		Dim dDispatcher As New AgoraLegacy.dispatcher
		
		</script>
		<%response.write(Session("WheelScript"))%>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_AddAddress_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
            <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			</TR>
			
				<TR>
	                <TD colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the relevant info and click the Save button to save the address."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="6"></TD>
			    </TR>
				<TR>
					<TD vAlign="middle" align="left">
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tableheader" colSpan="2">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 16px" width="92"></TD>
								<TD class="TableInput" style="HEIGHT: 16px"></TD>
							</TR>
							<TR>
								<TD class="tablecol" width="92">&nbsp;<strong>Code</strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="TableInput" style="HEIGHT: 19px"><asp:textbox id="txt_AddCode" runat="server" CssClass="txtbox" Width="150px" MaxLength="20"></asp:textbox><asp:requiredfieldvalidator id="vldAddCode" runat="server" ErrorMessage="Code is required." ControlToValidate="txt_AddCode"
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 17px" width="92">&nbsp;<STRONG>Address</STRONG><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="TableInput" style="HEIGHT: 17px"><asp:textbox id="txt_Add1" runat="server" CssClass="txtbox" Width="361px" MaxLength="50"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="vldAdd" runat="server" ErrorMessage="Address is required." ControlToValidate="txt_Add1"
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 21px" width="92"></TD>
								<TD class="TableInput" style="HEIGHT: 19px"><asp:textbox id="txt_Add2" runat="server" CssClass="txtbox" Width="361px" MaxLength="50"></asp:textbox>&nbsp;
								</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 17px" width="92"></TD>
								<TD class="TableInput" style="HEIGHT: 17px"><asp:textbox id="txt_Add3" runat="server" CssClass="txtbox" Width="361px" MaxLength="50"></asp:textbox>&nbsp;
								</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 17px" width="92">&nbsp;<STRONG>City<asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
								<TD class="TableInput" style="HEIGHT: 17px"><asp:textbox id="txt_City" runat="server" CssClass="txtbox" Width="150px" MaxLength="20"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="vldCity" runat="server" ErrorMessage="City is required." ControlToValidate="txt_City"
										Display="None"></asp:requiredfieldvalidator>
								</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 17px" width="92">&nbsp;<STRONG>State<asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
								<TD class="TableInput" style="HEIGHT: 17px"><asp:dropdownlist id="cbo_State" runat="server" CssClass="txtbox" Width="150px"></asp:dropdownlist>&nbsp;
									<asp:requiredfieldvalidator id="vldState" runat="server" ErrorMessage="State is required." ControlToValidate="cbo_State"
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 17px" width="92">&nbsp;<STRONG>Post 
										Code<asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
								<TD class="TableInput" style="HEIGHT: 17px"><asp:textbox id="txt_PostCode" runat="server" CssClass="txtbox" Width="99px" MaxLength="10"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="vldPostCode" runat="server" ErrorMessage="Post Code is required." ControlToValidate="txt_PostCode"
										Display="None"></asp:requiredfieldvalidator>
									<asp:regularexpressionvalidator id="revPostcode" runat="server" Display="None" ControlToValidate="txt_PostCode"
										ErrorMessage="Invalid Post Code " ValidationExpression="\d{5}" Enabled="False"></asp:regularexpressionvalidator>
								</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 17px" width="92">&nbsp;<STRONG>Country</STRONG><asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="TableInput" style="HEIGHT: 17px"><asp:dropdownlist id="cbo_Country" runat="server" CssClass="txtbox" Width="150px" AutoPostBack="True"></asp:dropdownlist>&nbsp;
									<asp:requiredfieldvalidator id="vldCountry" runat="server" ErrorMessage="Country is required." ControlToValidate="cbo_Country"
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 7px"></TD>
								<td class="TableInput" style="HEIGHT: 7px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
							</TR>
							<TR class="emptycol">
								<TD style="HEIGHT: 7px" colspan="2"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
									required field</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD align="left"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;
						<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Visible="False"></asp:button>&nbsp;
						<INPUT class="button" id="cmdAdd" type="button" value="Add" name="cmdAdd" runat="server">&nbsp;
						<INPUT class="button" id="cmdReset" type="button" value="Clear" name="cmdReset" runat="server"
							onclick="ValidatorReset();">&nbsp;
					</TD>
				</TR>
				<TR>
					<TD><br>
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
