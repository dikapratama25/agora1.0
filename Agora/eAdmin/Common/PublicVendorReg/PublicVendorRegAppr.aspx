<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PublicVendorRegAppr.aspx.vb" Inherits="eAdmin.PublicVendorRegAppr" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PublicVendorRegAppr</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" />"
        </script>
        <% Response.Write(sCSS)%>
		
		
	</HEAD>
	<BODY MS_POSITIONING="GridLayout">
		<FORM id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px"
				cellSpacing="0" cellPadding="0" width="300" border="0">
				<TR>
					<TD class="header">&nbsp;Public Vendor Registration Approval</TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table5" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="tableheader" colSpan="3">&nbsp;Company Details</TD>
							</TR>
							<TR>
								<TD class="tablecol" colSpan="3"></TD>
							</TR>
							<TR class="tablecol">
								<TD class="tablecol" style="WIDTH: 190px; HEIGHT: 25px" width="190">&nbsp;<STRONG>Company 
										ID</STRONG>
									<asp:label id="Label16" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<STRONG>&nbsp;</STRONG></TD>
								<TD class="tableinput" style="HEIGHT: 25px">&nbsp;
									<asp:textbox id="txtComID" runat="server" CssClass="txtbox" Width="264px"></asp:textbox></TD>
								<TD class="tableinput" style="HEIGHT: 25px"><asp:requiredfieldvalidator id="rfv_txtComID" runat="server" Display="None" ErrorMessage="Company ID is required."
										ControlToValidate="txtComID"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="rev_txtComID" runat="server" Display="None" ErrorMessage="Invalid Company ID."
										ControlToValidate="txtComID" ValidationExpression="^[a-zA-Z0-9_]+$"></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px"><STRONG>&nbsp;Company Name</STRONG>
									<asp:label id="Label17" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtComName" runat="server" CssClass="txtbox" Width="264px" MaxLength="100"></asp:textbox></TD>
								<TD class="tableinput"><asp:requiredfieldvalidator id="rfv_txtComName" runat="server" Display="None" ErrorMessage="Company Name is required."
										ControlToValidate="txtComName"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px"><STRONG>&nbsp;Status :</STRONG></TD>
								<TD class="tableinput">&nbsp;
									<asp:radiobuttonlist id="optStatus" Width="176px" RepeatDirection="Horizontal" Runat="server">
										<asp:ListItem Value="A" Selected="True">Active</asp:ListItem>
										<asp:ListItem Value="I">Inactive</asp:ListItem>
									</asp:radiobuttonlist></TD>
								<TD class="tableinput"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 6px" colSpan="3"><hr>
								</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px"><STRONG>&nbsp;Address</STRONG><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<STRONG>
									</STRONG>
								</TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtAddress" runat="server" CssClass="txtbox" Width="264px" MaxLength="50"></asp:textbox>&nbsp;</TD>
								<TD class="tableinput"><asp:requiredfieldvalidator id="rfv_address" runat="server" Display="None" ErrorMessage="Address is required. "
										ControlToValidate="txtAddress"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px">&nbsp;</TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtAddress2" runat="server" CssClass="txtbox" Width="264px" MaxLength="50"></asp:textbox></TD>
								<TD class="tableinput"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px">&nbsp;<B> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<FONT size="1">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										</FONT></B>
								</TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtAddress3" runat="server" CssClass="txtbox" Width="264px" MaxLength="50"></asp:textbox></TD>
								<TD class="tableinput"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px">&nbsp;<STRONG>City</STRONG><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<STRONG>
									</STRONG>
								</TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtCity" runat="server" CssClass="txtbox" Width="264px" MaxLength="30"></asp:textbox>&nbsp;</TD>
								<TD class="tableinput"><asp:requiredfieldvalidator id="rfv_city" runat="server" Display="None" ErrorMessage="City is required." ControlToValidate="txtCity"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px; HEIGHT: 3px">&nbsp;<STRONG>State</STRONG><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<STRONG>
									</STRONG>
								</TD>
								<TD class="tableinput" style="HEIGHT: 3px">&nbsp;
									<asp:dropdownlist id="cboState" runat="server" CssClass="ddl" Width="264px"></asp:dropdownlist>&nbsp;</TD>
								<TD class="tableinput" style="HEIGHT: 3px"><asp:requiredfieldvalidator id="rfv_state" runat="server" Display="None" ErrorMessage="State is required." ControlToValidate="cbostate"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px">&nbsp;<STRONG>Post Code</STRONG>
									<asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<STRONG>
									</STRONG>
								</TD>
								<TD class="tableinput" align="left">&nbsp;
									<asp:textbox id="txtPostCode" runat="server" CssClass="txtbox" MaxLength="10"></asp:textbox>&nbsp;</TD>
								<TD class="tableinput" align="left"><asp:regularexpressionvalidator id="rev_postcode" runat="server" Display="None" ErrorMessage="Invalid Post Code"
										ControlToValidate="txtPostCode" ValidationExpression="\d{4,5}"></asp:regularexpressionvalidator><asp:requiredfieldvalidator id="rfv_postcode" runat="server" Display="None" ErrorMessage="Post Code is required."
										ControlToValidate="txtPostCode"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px">&nbsp;<STRONG>Country</STRONG><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="tableinput">&nbsp;
									<asp:dropdownlist id="cboCountry" runat="server" CssClass="ddl" Width="200px" AutoPostBack="True"></asp:dropdownlist>&nbsp;</TD>
								<TD class="tableinput"><asp:requiredfieldvalidator id="rfv_country" runat="server" Display="None" ErrorMessage="Country is required."
										ControlToValidate="cbocountry"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px">&nbsp;<STRONG>Phone</STRONG><asp:label id="Label8" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="tableinput" align="left">&nbsp;
									<asp:textbox id="txtPhone" runat="server" CssClass="txtbox" MaxLength="50"></asp:textbox>&nbsp;</TD>
								<TD class="tableinput" align="left"><br>
									<asp:requiredfieldvalidator id="rfv_phone" runat="server" Display="None" ErrorMessage="Phone is required." ControlToValidate="txtPhone"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px">&nbsp;<STRONG>Fax</STRONG><asp:label id="Label9" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtFax" runat="server" CssClass="txtbox" Width="264px" MaxLength="50"></asp:textbox>&nbsp;</TD>
								<TD class="tableinput"><br>
									<asp:requiredfieldvalidator id="rfv_fax" runat="server" Display="None" ErrorMessage="Fax is required." ControlToValidate="txtFax"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px">&nbsp;<STRONG>Email</STRONG><asp:label id="Label10" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<STRONG>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</STRONG></TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtEmail" runat="server" CssClass="txtbox" Width="264px" MaxLength="30"></asp:textbox>&nbsp;</TD>
								<TD class="tableinput"><asp:regularexpressionvalidator id="rev_email" runat="server" Display="None" ErrorMessage="Invalid Email" ControlToValidate="txtEmail"
										ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:regularexpressionvalidator><asp:requiredfieldvalidator id="rfv_email" runat="server" Display="None" ErrorMessage="Email is required." ControlToValidate="txtEmail"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px; HEIGHT: 24px">&nbsp;<STRONG>Business 
										Registration No.</STRONG>
									<asp:label id="Label11" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<STRONG>&nbsp;</STRONG></TD>
								<TD class="tableinput">&nbsp;&nbsp;<asp:textbox id="txtBusinessRegNo" runat="server" CssClass="txtbox" Width="264px" MaxLength="30"></asp:textbox>&nbsp;</TD>
								<TD class="tableinput" style="HEIGHT: 24px"><asp:requiredfieldvalidator id="rfv_BRegNo" runat="server" Display="None" ErrorMessage="Business Registration No. is required."
										ControlToValidate="txtBusinessRegNo"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 6px" colSpan="3"><hr>
								</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px">&nbsp;<STRONG>Bank Account No. </STRONG>:</TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtAccountNo" runat="server" CssClass="txtbox" MaxLength="30"></asp:textbox></TD>
								<TD class="tableinput">&nbsp;</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px"><STRONG>&nbsp;Bank Code </STRONG>:<STRONG> </STRONG>
								</TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtBankCode" runat="server" CssClass="txtbox" Width="264px" MaxLength="30"></asp:textbox></TD>
								<TD class="tableinput">&nbsp;</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px">&nbsp;<STRONG>Bank Branch Code </STRONG>:<STRONG>
									</STRONG>
								</TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtBranchCode" runat="server" CssClass="txtbox" Width="264px" MaxLength="30"></asp:textbox></TD>
								<TD class="tableinput">&nbsp;</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px"><STRONG>&nbsp;Currency</STRONG><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<STRONG>
									</STRONG>
								</TD>
								<TD class="tableinput">&nbsp;
									<asp:dropdownlist id="cboCurrency" runat="server" CssClass="ddl" Width="264px"></asp:dropdownlist>&nbsp;</TD>
								<TD class="tableinput"><asp:requiredfieldvalidator id="rfv_currency" runat="server" Display="None" ErrorMessage="Currency is required."
										ControlToValidate="cbocurrency"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" colSpan="3"></TD>
							</TR>
							<!--<TR>
								<TD class="tablecol">&nbsp;<B>Company&nbsp;Logo :</B></TD>
								<TD class="tablecol">&nbsp;
									<asp:textbox id="txtCompanyLogo" runat="server" CssClass="txtbox" Width="264px"></asp:textbox></TD>
								<TD class="tablecol"><asp:button id="btnBrowse" runat="server" CssClass="button" Text="Browse"></asp:button>&nbsp;<asp:button id="btnUpload" runat="server" CssClass="button" Text="Upload"></asp:button>&nbsp;<asp:button id="btnPrevPO" runat="server" CssClass="button" Text="Preview"></asp:button></TD>
							</TR>
							<TR>
								<TD class="tablecol">
									<P>&nbsp;<B>T&amp;C Document Upload :</B></P>
								</TD>
								<TD class="tablecol">&nbsp;
									<asp:textbox id="txtTC" runat="server" CssClass="txtbox" Width="264px"></asp:textbox></TD>
								<td class="tablecol"><asp:button id="btnBrowse1" runat="server" CssClass="button" Text="Browse"></asp:button>&nbsp;<asp:button id="btnUpload1" runat="server" CssClass="button" Text="Upload"></asp:button>&nbsp;<asp:button id="btnPrevPO1" runat="server" CssClass="button" Text="Preview"></asp:button></td>
							</TR>-->
							<TR>
								<TD class="tablecol" style="WIDTH: 190px">&nbsp;<STRONG>Current Company&nbsp;Logo </STRONG>
									:</TD>
								<TD class="tableinput">&nbsp;
									<asp:image id="Image1" runat="server" Height="30"></asp:image><asp:label id="lblLogo" runat="server">No Logo</asp:label></TD>
								<TD class="tableinput"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px; HEIGHT: 50px" height="47">&nbsp;<STRONG>Company&nbsp;Logo
									</STRONG>:<br>
									&nbsp;<asp:label id="lblComLogo" runat="server" CssClass="div">Recommended dimension is 130(W) x 70(H) pixels.</asp:label></TD>
								<TD class="tableinput" style="HEIGHT: 47px" height="47">&nbsp;&nbsp;<input class="button" id="cmdCompLogo" style="FONT-SIZE: 8pt; WIDTH: 340px; FONT-FAMILY: verdana; BACKGROUND-COLOR: white"
										type="file" name="cmdCompLogo" runat="server">&nbsp;</TD>
								<TD class="tableinput" style="HEIGHT: 47px" height="47"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px" vAlign="top">&nbsp;<STRONG>T&amp;C Document 
										Upload</STRONG>&nbsp;:<br>
									&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="div" Width="176px">Recommended file size is 300KB</asp:Label>
								</TD>
								<TD class="tableinput">&nbsp;&nbsp;<input class="button" id="cmdTCBrow" style="FONT-SIZE: 8pt; WIDTH: 340px; FONT-FAMILY: verdana; BACKGROUND-COLOR: white"
										type="file" name="cmdTCBrow" runat="server">&nbsp;&nbsp;&nbsp;
									<asp:hyperlink id="lnkTC" runat="server">HyperLink</asp:hyperlink>&nbsp;&nbsp;</TD>
								<td class="tableinput"></td>
							</TR>
							<TR>
								<TD class="tablecol" colSpan="3"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px">&nbsp;<STRONG>Tax Reg. No. </STRONG>:</TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtGSTRegNo" runat="server" CssClass="txtbox" Width="264px" MaxLength="30"></asp:textbox></TD>
								<TD class="tableinput"></TD>
							</TR>
							<TR id="tdVendor" style="DISPLAY: none" runat="server">
								<TD class="tablecol" style="WIDTH: 190px; HEIGHT: 38px">&nbsp;<STRONG>Tax Calculate By </STRONG>
									:</TD>
								<TD class="tableinput" style="HEIGHT: 38px" height="38">&nbsp;
									<asp:radiobuttonlist id="optGSTCalcBy" Width="250" Runat="server" RepeatDirection="Horizontal" Height="0">
										<asp:ListItem Value="0">Item</asp:ListItem>
										<asp:ListItem Value="1" Selected="True">Sub Total</asp:ListItem>
									</asp:radiobuttonlist></TD>
								<TD class="tableinput" style="HEIGHT: 38px"></TD>
							</TR>
							<TR style="DISPLAY: none">
								<TD class="tablecol" style="WIDTH: 190px; HEIGHT: 38px"><STRONG>&nbsp;BCM Setting </STRONG>
									:</TD>
								<TD class="tableinput" style="HEIGHT: 38px" height="38">&nbsp;
									<asp:radiobuttonlist id="optBCM" Width="200" Runat="server" RepeatDirection="Horizontal">
										<asp:ListItem Value="1">On</asp:ListItem>
										<asp:ListItem Value="0" Selected="True">Off</asp:ListItem>
									</asp:radiobuttonlist><asp:TextBox id="txtBCMSetting" style="DISPLAY:none" Runat="server"></asp:TextBox></TD>
								<TD class="tableinput" style="HEIGHT: 38px"></TD>
							</TR>
							<TR style="DISPLAY: none">
								<TD class="tablecol" style="WIDTH: 190px; HEIGHT: 38px"><STRONG>&nbsp;Finance Dept. 
										Mode </STRONG>:</TD>
								<TD class="tableinput" style="HEIGHT: 38px" height="38">&nbsp;<asp:radiobuttonlist id="optFinMode" Width="200" Runat="server" RepeatDirection="Horizontal">
										<asp:ListItem Value="Y">On</asp:ListItem>
										<asp:ListItem Value="N" Selected="True">Off</asp:ListItem>
									</asp:radiobuttonlist></TD>
								<TD class="tableinput" style="HEIGHT: 38px"></TD>
							</TR>
							<TR style="DISPLAY: none">
								<TD class="tablecol" style="WIDTH: 190px; HEIGHT: 38px"><STRONG>&nbsp;Invoice &amp; 
										Payment Approval </STRONG>:</TD>
								<TD class="tableinput" style="HEIGHT: 38px" height="38">&nbsp;<asp:radiobuttonlist id="optInvAppr" Width="200" Runat="server" RepeatDirection="Horizontal">
										<asp:ListItem Value="Y">On</asp:ListItem>
										<asp:ListItem Value="N" Selected="True">Off</asp:ListItem>
									</asp:radiobuttonlist></TD>
								<TD class="tableinput" style="HEIGHT: 38px"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px">&nbsp;<STRONG>Password Duration</STRONG>
									<asp:label id="Label14" runat="server" CssClass="errormsg" Width="8px" Height="5px">*</asp:label>&nbsp;:</STRONG></TD>
								<TD class="tableinput">&nbsp;
									<asp:textbox id="txtPwdDur" runat="server" CssClass="txtbox" Width="80px" MaxLength="4"></asp:textbox>&nbsp;Days
									<asp:requiredfieldvalidator id="PwdDur" runat="server" Display="None" ErrorMessage="Password Duration is required."
										ControlToValidate="txtPwdDur"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="revPwdDur" runat="server" Display="None" ErrorMessage="Password Duration is expecting numeric value."
										ControlToValidate="txtPwdDur" ValidationExpression="^[0-9]{0,4}$"></asp:regularexpressionvalidator></TD>
								<TD class="tableinput"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px"><STRONG>&nbsp;Private Labeling </STRONG>:</TD>
								<TD class="tableinput">&nbsp;
									<asp:radiobuttonlist id="optPrivateLbl" Width="200" Runat="server" RepeatDirection="Horizontal">
										<asp:ListItem Value="Y">On</asp:ListItem>
										<asp:ListItem Value="N" Selected="True">Off</asp:ListItem>
									</asp:radiobuttonlist></TD>
								<TD class="tableinput"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px"><STRONG>&nbsp;Skin </STRONG>:</TD>
								<TD class="tableinput">&nbsp;<asp:radiobuttonlist id="optSkin" Width="295" Runat="server" RepeatDirection="Horizontal" RepeatColumns="3">
										<asp:ListItem Value="1" Selected="True">Skin 1</asp:ListItem>
										<asp:ListItem Value="2">Skin 2</asp:ListItem>
										<asp:ListItem Value="3">Skin 3</asp:ListItem>
										<asp:ListItem Value="4">Skin 4</asp:ListItem>
										<asp:ListItem Value="5">Skin 5</asp:ListItem>
										<asp:ListItem Value="6">Skin 6</asp:ListItem>
									</asp:radiobuttonlist></TD>
								<TD class="tableinput"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 190px"><STRONG>&nbsp;For Training/ Demo<br>
										&nbsp;Purpose </STRONG>:</TD>
								<TD class="tableinput"><asp:checkbox id="chktraining" Runat="server"></asp:checkbox></TD>
								<TD class="tableinput"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" colSpan="3"></TD>
							</TR>
						</TABLE>
						<asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>
						<asp:button id="cmdApprove" runat="server" CssClass="button" Text="Approve"></asp:button>
						<asp:button id="cmdReject" runat="server" CssClass="button" Text="Reject"></asp:button></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR id="trBack" runat="server">
					<TD class="emptycol" colSpan="2"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 34px"><asp:validationsummary id="vldsumm" runat="server" CssClass="errormsg" Width="696px"></asp:validationsummary></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
			</TABLE>
		</FORM>
	</BODY>
</HTML>
