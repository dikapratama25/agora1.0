<%@ Page Language="vb" AutoEventWireup="false" Codebehind="coCompanyDetail.aspx.vb" Inherits="eAdmin.coCompanyDetail1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Com_Profile</title>
		<META content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<META content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<META content="JavaScript" name="vs_defaultClientScript">
		<META content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher as New Dispatcher.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
            Dim startcalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtSubStart") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
            Dim endcalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtSubEnd") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
        </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
	<!--
	
			function checkFileType(fileType1, fle1, fileType2, fle2)
			{
				var formValidate;	
				if (typeof(Page_Validators) == "undefined")
					formValidate = true;
				else	
					formValidate = Page_ClientValidate();	

				if (formValidate == true){
					if (checkDocFile(fileType1, fle1)) {
						if (checkDocFile(fileType2, fle2)) 
							Page_IsValid = true;
						else
							Page_IsValid = false;				
					}
					else
						Page_IsValid = false;
				}
				else {
					Page_IsValid = false;
				}
				
				return Page_IsValid;	
			}

			function checkPackageChange() {
				if (document.activeElement.type =="checkbox") {
					chk = document.activeElement;
					if (chk.checked == false) {
						if (document.getElementById(chk.id.replace("chkLst", "chkLstOri")).checked == true) {
							if (confirm("Are you sure that you want to deselect this package?\nAll the previous configured user's user group for this package will be removed.") == false) {
								chk.checked = true;
							}
						}
					}
				}
			}

			function companyTypeChange(mode) {	
				//Always Postback for add mode	
				if (mode='add') __doPostBack('cboCompType','');
				//edit mode
				if (document.getElementById("cboCompType").value != 'BOTH' && document.getElementById("cboCompType").selectedIndex != 0 && document.getElementById("txtCompTypeOri").value != '') {
					if (document.getElementById("cboCompType").selectedIndex != document.getElementById("txtCompTypeOri").value) {
						if (confirm("Are you sure that you want to change the company type?\nYou may lost some of the previous configured user's user group.") == true) {
							__doPostBack('cboCompType','');
						} else {
							document.activeElement.selectedIndex = document.getElementById("txtCompTypeOri").value;
							return false;
						}
					}
				}
			}

	function PreviewImage(f)       
			{	var temp;
			if (f.cmdCompLogo.value!="")
			{	temp='file:///' + f.cmdCompLogo.value;
			}
			else
			{	if (f.cmdCompLogo.value!="")
					temp=document.images["image1"].src;
				else
					temp=''
			}
						
			//msg=window.open("../PO/POReport.aspx?pageid=18&po_from=admin&img=" + temp,"","Width=700,Height=500,resizable=yes,scrollbars=yes");		 	                                 
			msg=window.open("","","Width=500,Height=400,resizable=yes,scrollbars=yes");
		 	msg.document.clear();
			 /* Note that the word SCRIPT was not
			    kept intact on one line in the
			    write() below.  A bug will parse 
			    it and will not compile the script
			    correctly if you don't break that
			    word when it appears within write()
			    statements. */
			   
			 msg.document.write('<HTML><HEAD><TITLE'
			  +'>Image Preview</TITLE>'
			  +'</HEAD><BODY>'
			  +'<img src="'
        	  + temp + '"></img>'
        	   + '</BODY></H'
			  +'TML><P>');
		}
		
			function validateControl(oSrc, args, id, msg, ctl)
			{
				debugger;
				if (document.getElementById(ctl).value == '') {
					args.IsValid = false;
					Form1.document.getElementById(id).errormessage = msg + ' is required.';
				}
				else {
					if (isNaN(document.getElementById(ctl).value)){
						args.IsValid = false;
						Form1.document.getElementById(id).errormessage = msg + ' is expecting numeric value.';
					}
					else if (parseInt(document.getElementById(ctl).value) == 0) {
						args.IsValid = false;
						Form1.document.getElementById(id).errormessage = msg + ' must > 0.';
					}
					else
						args.IsValid = true;
				}
				return args.IsValid;
			}
			
			function validateTrans(oSrc, args)
			{
				return validateControl(oSrc, args, 'cvTrans', 'No. of Trans', 'txtTrans');
			}
			
			function validateSKU(oSrc, args)
			{
				debugger;
				return validateControl(oSrc, args, 'cvSKU', 'No. of SKU', 'txtSKU');
			}


//-->
		</script>
	</HEAD>
	<BODY >
		<FORM id="Form1" method="post" runat="server">
			<TABLE class="ALLTABLE" id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD><asp:label id="lblTitle" runat="server" CssClass="header">Company Details</asp:label></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD class="tableheader">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tablecol" width="200">&nbsp;<STRONG>Company ID</STRONG><asp:label id="Label16" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<STRONG>&nbsp;</STRONG></TD>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtCoyID" runat="server" CssClass="txtbox" Width="250px" MaxLength="20"></asp:textbox><asp:requiredfieldvalidator id="rfv_txtCoyID" runat="server" ControlToValidate="txtCoyID" ErrorMessage="Company ID is required."
										Display="None"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="rev_txtCoyID" runat="server" ControlToValidate="txtCoyID" ErrorMessage="Invalid Company ID."
										Display="None" ValidationExpression="^[a-zA-Z0-9_]+$"></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 157px">&nbsp;<STRONG>Company Name</STRONG><asp:label id="Label17" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtCoyName" runat="server" CssClass="txtbox" Width="250px" MaxLength="100"></asp:textbox><asp:requiredfieldvalidator id="rfv_txtCoyName" runat="server" ControlToValidate="txtCoyName" ErrorMessage="Company Name is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 6px" colSpan="3"><DIV id="Hubadmin1" runat="server">
										<TABLE class="alltable" id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
											<TR width="100%">
												<TD class="tablecol" width="200">&nbsp;<B>Status :</B></TD>
												<TD class="tablecol" colSpan="2">&nbsp;
													<asp:radiobuttonlist id="rdStatus" CssClass="rbtn" Width="200" RepeatDirection="Horizontal" Runat="server">
														<asp:ListItem Value="A" Selected="True">Active</asp:ListItem>
														<asp:ListItem Value="I">Inactive</asp:ListItem>
													</asp:radiobuttonlist></TD>
											</TR>
										</TABLE>
									</DIV>
								</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 6px" colSpan="3"><HR>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table9" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px" width="200">&nbsp;<STRONG>Company Type<asp:label id="Label19" runat="server" CssClass="errormsg">*</asp:label>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" style="HEIGHT: 24px">&nbsp;<asp:dropdownlist id="cboCompType" CssClass="ddl" Width="120px" Runat="server" AutoPostBack="True">
										<asp:ListItem Value="NONE">---Select---</asp:ListItem>
										<asp:ListItem Value="BUYER">Buyer</asp:ListItem>
										<asp:ListItem Value="VENDOR">Vendor</asp:ListItem>
									</asp:dropdownlist><asp:comparevalidator id="cv_cboCompType" ControlToValidate="cboCompType" ErrorMessage="Company type is required."
										Display="None" Runat="server" Operator="NotEqual" ValueToCompare="NONE"></asp:comparevalidator>
									<asp:textbox id="txtCompTypeOri" style="DISPLAY: none" runat="server"></asp:textbox></TD>
							</TR>
							<TR style="display:none">
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>Parent Company</STRONG>&nbsp;<FONT size="1"><STRONG></STRONG>:</FONT></TD>
								<TD class="tablecol" style="HEIGHT: 24px">&nbsp;<asp:dropdownlist id="cboParentCoy" runat="server" CssClass="ddl" Width="250px"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>Package </STRONG>:</TD>
								<TD class="tablecol" style="HEIGHT: 24px">&nbsp;<asp:dropdownlist id="cboLicensePackage" CssClass="ddl" Width="120px" Runat="server"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 20px" vAlign="middle">&nbsp;<STRONG>Application 
										Package<asp:label id="Label21" runat="server" CssClass="errormsg">*</asp:label>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" style="HEIGHT: 20px" vAlign="middle"><asp:checkboxlist id="chkLst" runat="server" Width="496px" RepeatDirection="Horizontal" Height="6px"
										DataTextField="AP_APP_NAME" DataValueField="AP_APP_ID" RepeatColumns="4"></asp:checkboxlist>
									<asp:textbox id="txtLst" style="DISPLAY: none" runat="server"></asp:textbox><asp:requiredfieldvalidator id="rfvLst" runat="server" ControlToValidate="txtLst" ErrorMessage="Application Package is required."
										Display="None"></asp:requiredfieldvalidator><asp:checkboxlist id="chkLstOri" style="DISPLAY: none" runat="server" Width="496px" RepeatDirection="Horizontal"
										Height="6px" DataTextField="AP_APP_NAME" DataValueField="AP_APP_ID" RepeatColumns="4"></asp:checkboxlist></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 36px">&nbsp;<STRONG>User License<asp:label id="Label18" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
								<TD class="tablecol" style="HEIGHT: 36px">&nbsp;<asp:textbox id="txtUserLicense" CssClass="txtbox" MaxLength="4" width="50" Runat="server">1</asp:textbox><INPUT class="txtbox" id="txtUserLicenseHide" style="DISPLAY: none" type="text" value="1"
										name="Text1" runat="server">
									<asp:requiredfieldvalidator id="rfv_txtUserLicense" runat="server" ControlToValidate="txtUserLicense" ErrorMessage="User License is required."
										Display="None"></asp:requiredfieldvalidator><asp:rangevalidator id="rfv_txtUserLicense2" runat="server" ControlToValidate="txtUserLicense" ErrorMessage="At least one user license"
										Display="None" Type="Integer" MaximumValue="9999" MinimumValue="1"></asp:rangevalidator>&nbsp;<asp:comparevalidator id="rfc_txtUserLicense3" ControlToValidate="txtUserLicense" ErrorMessage="User license limit can not less than existing user."
										Display="None" Runat="server" Operator="GreaterThanEqual" Type="Integer" ControlToCompare="txtUserLicenseHide"></asp:comparevalidator></TD>
							</TR>
							<tr>
								<td class="tablecol">&nbsp;<STRONG>Report User</STRONG>&nbsp;:
								</td>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtReportUser" CssClass="txtbox" MaxLength="4" width="50" Runat="server">1</asp:textbox>
									<asp:comparevalidator id="cvReportUser" ControlToValidate="txtReportUser" ErrorMessage="Report user cannot more than user license."
										Display="None" Runat="server" Operator="LessThanEqual" Type="Integer" ControlToCompare="txtUserLicense"></asp:comparevalidator></TD>
							</tr>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>Subscription Start Date&nbsp;</STRONG>:</TD>
								<TD class="tablecol" style="HEIGHT: 24px">&nbsp;<asp:textbox id="txtSubStart" runat="server" CssClass="txtbox" Width="100px" contentEditable="false" ></asp:textbox>
								<% Response.Write(startcalendar)%>
									<asp:regularexpressionvalidator id="rfv_txtSubStart" runat="server" ControlToValidate="txtSubStart" ErrorMessage="Invalid Subcription Start Date"
										Display="None" ValidationExpression="\d{1,2}/\d{1,2}/\d{4}"></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>Subscription End Date </STRONG>
									:</TD>
								<TD class="tablecol" style="HEIGHT: 24px">&nbsp;<asp:textbox id="txtSubEnd" runat="server" CssClass="txtbox" Width="100px" contentEditable="false" ></asp:textbox>
								<% Response.Write(endcalendar)%>
									<asp:regularexpressionvalidator id="rfv_txtSubEnd" runat="server" ControlToValidate="txtSubEnd" ErrorMessage="Invalid Subcription End Date"
										Display="None" ValidationExpression="\d{1,2}/\d{1,2}/\d{4}"></asp:regularexpressionvalidator><asp:comparevalidator id="cvf_txtDate" ControlToValidate="txtSubEnd" ErrorMessage="Subscription end date must greater than subscription start date. "
										Display="None" Runat="server" Operator="GreaterThan" ControlToCompare="txtSubStart" type="Date"></asp:comparevalidator></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="tablecol">
						<HR>
						&nbsp;</TD>
				</TR>
				<TR>
					<TD class="tablecol">
						<TABLE class="alltable" id="Table10" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>Address<asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:<STRONG>
									</STRONG>
								</TD>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtAddress" runat="server" CssClass="txtbox" Width="250px" MaxLength="50"></asp:textbox><asp:requiredfieldvalidator id="rfv_address" runat="server" ControlToValidate="txtAddress" ErrorMessage="Address is required. "
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px"></TD>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtAddress2" runat="server" CssClass="txtbox" Width="250px" MaxLength="50"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px"></TD>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtAddress3" runat="server" CssClass="txtbox" Width="250px" MaxLength="50"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>City<asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:<STRONG>
									</STRONG>
								</TD>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtCity" runat="server" CssClass="txtbox" Width="250px" MaxLength="30"></asp:textbox><asp:requiredfieldvalidator id="rfv_city" runat="server" ControlToValidate="txtCity" ErrorMessage="City is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>State</STRONG><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<STRONG>
									</STRONG>
								</TD>
								<TD class="tablecol">&nbsp;<asp:dropdownlist id="cbostate" runat="server" CssClass="ddl" Width="250px"></asp:dropdownlist><asp:requiredfieldvalidator id="rfv_state" runat="server" ControlToValidate="cbostate" ErrorMessage="State is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>Post Code</STRONG><asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<STRONG>
									</STRONG>
								</TD>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtPostCode" runat="server" CssClass="txtbox" MaxLength="10"></asp:textbox><asp:regularexpressionvalidator id="rev_postcode" runat="server" ControlToValidate="txtPostCode" ErrorMessage="Invalid Post Code"
										Display="None" ValidationExpression="\d{4,5}" Enabled="False"></asp:regularexpressionvalidator><asp:requiredfieldvalidator id="rfv_postcode" runat="server" ControlToValidate="txtPostCode" ErrorMessage="Post Code is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>Country<asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
								<TD class="tablecol">&nbsp;<asp:dropdownlist id="cbocountry" runat="server" CssClass="ddl" Width="200px" AutoPostBack="True"></asp:dropdownlist><asp:requiredfieldvalidator id="rfv_country" runat="server" ControlToValidate="cbocountry" ErrorMessage="Country is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>Phone<asp:label id="Label8" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtPhone" runat="server" CssClass="txtbox" MaxLength="20"></asp:textbox><asp:requiredfieldvalidator id="rfv_phone" runat="server" ControlToValidate="txtPhone" ErrorMessage="Phone is required."
										Display="None"></asp:requiredfieldvalidator>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>Fax<asp:label id="Label9" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtFax" runat="server" CssClass="txtbox" Width="250px" MaxLength="20"></asp:textbox><asp:requiredfieldvalidator id="rfv_fax" runat="server" ControlToValidate="txtFax" ErrorMessage="Fax is required."
										Display="None"></asp:requiredfieldvalidator>
							</TR>
							<TR>
								<TD style="HEIGHT: 18px">&nbsp;<STRONG>Email<asp:label id="Label10" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
								<TD>&nbsp;<asp:textbox id="txtEmail" runat="server" CssClass="txtbox" Width="250px" MaxLength="50"></asp:textbox><asp:regularexpressionvalidator id="rev_email" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid Email"
										Display="None" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:regularexpressionvalidator><asp:requiredfieldvalidator id="rfv_email" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD style="HEIGHT: 18px" width="200">&nbsp;<STRONG>Business Registration No.</STRONG><asp:label id="Label11" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<STRONG></STRONG>
								</TD>
								<TD>&nbsp;<asp:textbox id="txtBusinessRegNo" runat="server" CssClass="txtbox" Width="250px" MaxLength="30"></asp:textbox><asp:requiredfieldvalidator id="rfv_BRegNo" runat="server" ControlToValidate="txtBusinessRegNo" ErrorMessage="Business Registration No. is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD style="HEIGHT: 18px" width="200">&nbsp;<STRONG>Contact Person</STRONG>&nbsp;:<STRONG></STRONG>
								</TD>
								<TD>&nbsp;<asp:textbox id="txtContact" runat="server" CssClass="txtbox" Width="250px" MaxLength="30"></asp:textbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="tablecol">
						<HR>
					</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table11" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px" width="200">&nbsp;<STRONG>Bank Name </STRONG>:</TD>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtBankName" runat="server" CssClass="txtbox" MaxLength="90"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px" width="200">&nbsp;<STRONG>Bank Account No. </STRONG>:</TD>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtAccountNo" runat="server" CssClass="txtbox" MaxLength="30"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>Bank Code </STRONG>:<STRONG> </STRONG></TD>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtBankCode" runat="server" CssClass="txtbox" Width="250px" MaxLength="30"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 20px">&nbsp;<STRONG>Bank Branch Code </STRONG>:<STRONG>
									</STRONG>
								</TD>
								<TD class="tablecol" style="HEIGHT: 20px">&nbsp;<asp:textbox id="txtBranchCode" runat="server" CssClass="txtbox" Width="250px" MaxLength="30"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Currency<asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:<STRONG>
									</STRONG>
								</TD>
								<TD class="tablecol">&nbsp;<asp:dropdownlist id="cbocurrency" runat="server" CssClass="ddl" Width="250px"></asp:dropdownlist><asp:requiredfieldvalidator id="rfv_currency" runat="server" ControlToValidate="cbocurrency" ErrorMessage="Currency is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Current Company&nbsp;Logo </STRONG>:</TD>
								<TD class="tablecol"><asp:image id="Image1" runat="server" Height="30"></asp:image><asp:label id="lblLogo" runat="server">No Logo</asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Company&nbsp;Logo </STRONG>:<BR>
									&nbsp;
									<asp:label id="lblComLogo" runat="server" CssClass="div" Width="180px">Recommended dimension is 130(W) x 70(H) pixels.</asp:label></TD>
								<TD class="tablecol">&nbsp;<INPUT class="button" id="cmdCompLogo" style="FONT-SIZE: 8pt; WIDTH: 318px; FONT-FAMILY: verdana; HEIGHT: 18px; BACKGROUND-COLOR: white"
										type="file" size="33" name="cmdCompLogo" runat="server">&nbsp;<INPUT class="button" style="WIDTH: 88px; HEIGHT: 19px" onclick="PreviewImage(this.form);"
										type="button" value="Preview Image"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>T&amp;C Document Upload </STRONG>
									&nbsp;:<br>
									&nbsp;<asp:label id="lblAttach" runat="server" CssClass="div" Width="176px">Recommended file size is <% Response.Write(Session("FileSize"))%>KB</asp:label></TD>
								<TD class="tablecol">&nbsp;<INPUT class="button" id="cmdTCBrow" style="FONT-SIZE: 8pt; WIDTH: 318px; FONT-FAMILY: verdana; HEIGHT: 18px; BACKGROUND-COLOR: white"
										type="file" size="33" name="cmdTCBrow" runat="server">
									<asp:hyperlink id="lnkTC" runat="server">HyperLink</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px">&nbsp;<STRONG>Tax Reg. No. </STRONG>:</TD>
								<TD class="tablecol">&nbsp;<asp:textbox id="txtGSTRegNo" runat="server" CssClass="txtbox" Width="250px" MaxLength="30"></asp:textbox></TD>
							</TR>
							<TR id="tdVendor" style="DISPLAY: none" runat="server">
								<TD class="tablecol">&nbsp;<STRONG>Tax Calculate By </STRONG>:</TD>
								<TD class="tablecol"><asp:radiobuttonlist id="rdGSTCalcBy" CssClass="rbtn" Width="250" RepeatDirection="Horizontal" Runat="server"
										Height="0">
										<asp:ListItem Value="0">Item</asp:ListItem>
										<asp:ListItem Value="1" Selected="True">Sub Total</asp:ListItem>
									</asp:radiobuttonlist></TD>
							</TR>
							<tr id="trSKU" style="DISPLAY: none" runat="server">
								<td class="tablecol">&nbsp;<STRONG>No. of SKU </STRONG>
									<asp:label id="Label15" runat="server" CssClass="errormsg">*</asp:label>:</td>
						    	<td class="tablecol">&nbsp;<asp:textbox id="txtSKU" runat="server" CssClass="numerictxtbox" Width="64px" MaxLength="10" ></asp:textbox>
										<asp:regularexpressionvalidator id="revSKU" runat="server" ControlToValidate="txtSKU" ErrorMessage="No of SKU is expecting numeric value."
										Display="None" ValidationExpression="(?!^0*$)^\d{1,5}?$" Enabled="False"></asp:regularexpressionvalidator><asp:requiredfieldvalidator id="rfvSKU" runat="server" ControlToValidate="txtSKU" ErrorMessage="No of SKU is required."
										Display="None" Enabled="False"></asp:requiredfieldvalidator><asp:customvalidator id="cvSKU" runat="server" ErrorMessage="CustomValidator" Display="None" ClientValidationFunction="validateSKU"></asp:customvalidator>   </td> 
							</tr>
							<tr id="trTrans" style="DISPLAY: none" runat="server">
								<td class="tablecol">&nbsp;<STRONG>No. of Transaction </STRONG>
									<asp:label id="Label20" runat="server" CssClass="errormsg">*</asp:label>:</td>
								<td class="tablecol">&nbsp;<asp:textbox id="txtTrans" runat="server" CssClass="numerictxtbox" Width="64px" MaxLength="10"></asp:textbox>
									<asp:regularexpressionvalidator id="revTrans" runat="server" ControlToValidate="txtTrans" ErrorMessage="No of Transaction is expecting numeric value."
										Display="None" ValidationExpression="(?!^0*$)^\d{1,5}?$" Enabled="False"></asp:regularexpressionvalidator><asp:requiredfieldvalidator id="rfvTrans" runat="server" ControlToValidate="txtTrans" ErrorMessage="No of Transaction is  required."
										Display="None" Enabled="False"></asp:requiredfieldvalidator><asp:customvalidator id="cvTrans" runat="server" ErrorMessage="CustomValidator" Display="None" ClientValidationFunction="validateTrans"></asp:customvalidator></td>
							</tr>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD>
						<DIV id="Hubadmin2" runat="server">
							<TABLE class="alltable" id="Table7" cellSpacing="0" cellPadding="0" width="100" border="0">
								<TR width="100%">
									<TD class="tablecol" width="200">&nbsp;<STRONG>BCM Setting </STRONG>:</TD>
									<TD class="tablecol"><asp:radiobuttonlist id="rdBCM" CssClass="rbtn" Width="200" RepeatDirection="Horizontal" Runat="server">
											<asp:ListItem Value="1">On</asp:ListItem>
											<asp:ListItem Value="0" Selected="True">Off</asp:ListItem>
										</asp:radiobuttonlist><asp:textbox id="txtBCMSetting" style="DISPLAY: none" Runat="server"></asp:textbox></TD>
									<TD class="tablecol"></TD>
								</TR>
								<TR>
									<TD class="tablecol" style="WIDTH: 200px">&nbsp;<STRONG>Finance Dept. Mode </STRONG>
										:</TD>
									<TD class="tablecol"><asp:radiobuttonlist id="rdFinMode" CssClass="rbtn" Width="200" RepeatDirection="Horizontal" Runat="server">
											<asp:ListItem Value="Y">On</asp:ListItem>
											<asp:ListItem Value="N" Selected="True">Off</asp:ListItem>
										</asp:radiobuttonlist></TD>
									<TD class="tablecol"></TD>
								</TR>
								<TR id="INV" runat="server">
									<TD class="tablecol" style="WIDTH: 200px">&nbsp;<STRONG>Invoice &amp; Payment Approval </STRONG>
										:</TD>
									<TD class="tablecol"><asp:radiobuttonlist id="rdInvPayAppr" CssClass="rbtn" Width="200" RepeatDirection="Horizontal" Runat="server">
											<asp:ListItem Value="Y">On</asp:ListItem>
											<asp:ListItem Value="N" Selected="True">Off</asp:ListItem>
										</asp:radiobuttonlist></TD>
									<TD class="tablecol"></TD>
								</TR>
								<TR id="Multi" runat="server">
									<TD class="tablecol" style="WIDTH: 200px">&nbsp;<STRONG>PR To Multiple POs </STRONG>
										:</TD>
									<TD class="tablecol"><asp:radiobuttonlist id="rdInvMultiAppr" CssClass="rbtn" Width="200" RepeatDirection="Horizontal" Runat="server">
											<asp:ListItem Value="Y">On</asp:ListItem>
											<asp:ListItem Value="N" Selected="True">Off</asp:ListItem>
										</asp:radiobuttonlist></TD>
									<TD class="tablecol"></TD>
								</TR>
								<TR id="BA" runat="server">
									<TD class="tablecol" style="WIDTH: 200px">&nbsp;<STRONG>Buyer Admin Cancel POs </STRONG>
										:</TD>
									<TD class="tablecol"><asp:radiobuttonlist id="rdBA" CssClass="rbtn" Width="200" RepeatDirection="Horizontal" Runat="server">
											<asp:ListItem Value="Y">On</asp:ListItem>
											<asp:ListItem Value="N" Selected="True">Off</asp:ListItem>
										</asp:radiobuttonlist></TD>
									<TD class="tablecol"></TD>
								</TR>
							</TABLE>
						</DIV>
					</TD>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR id="trPayTerm" style="DISPLAY: none" runat="server" width="100%">
								<TD class="tablecol" style="HEIGHT: 18px" width="200">&nbsp;<STRONG>Payment Terms</STRONG><asp:label id="Label12" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="tablecol" style="HEIGHT: 26px" colSpan="2">&nbsp;<asp:dropdownlist id="cbo_PayTerm" runat="server" CssClass="ddl" Width="180px"></asp:dropdownlist><asp:requiredfieldvalidator id="PayTerm" runat="server" ControlToValidate="cbo_PayTerm" ErrorMessage="Payment Term Value is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR id="trPayMethod" style="DISPLAY: none" runat="server">
								<TD class="tablecol" style="HEIGHT: 18px" width="200">&nbsp;<STRONG>Payment Method</STRONG><asp:label id="Label13" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="tablecol" colSpan="2">&nbsp;<asp:dropdownlist id="cbo_PayMeth" runat="server" CssClass="ddl" Width="180px"></asp:dropdownlist><asp:requiredfieldvalidator id="PayMethod" runat="server" ControlToValidate="cbo_PayMeth" ErrorMessage="Payment Method Value is required."
										Display="None"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 18px" width="200">&nbsp;<STRONG>Password Duration</STRONG><asp:label id="Label14" runat="server" CssClass="errormsg" Width="8px" Height="5px">*</asp:label>&nbsp;:</TD>
								<TD class="tablecol" colSpan="2">&nbsp;<asp:textbox id="txt_PwdDur" runat="server" CssClass="txtbox" Width="80px" MaxLength="4"></asp:textbox>Days
									<asp:requiredfieldvalidator id="PwdDur" runat="server" ControlToValidate="txt_PwdDur" ErrorMessage="Password Duration is required."
										Display="None"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="revPwdDur" runat="server" ControlToValidate="txt_PwdDur" ErrorMessage="Password Duration is expecting numeric value."
										Display="None" ValidationExpression="^[0-9]{0,4}$"></asp:regularexpressionvalidator></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD>
						<DIV id="Hubadmin3" style="DISPLAY: none" runat="server">
							<TABLE class="alltable" id="Table8" cellSpacing="0" cellPadding="0" width="100" border="0">
								<TR width="100%">
									<TD class="tablecol" width="200">&nbsp;<STRONG>Private Labeling </STRONG>:</TD>
									<TD class="tablecol"><asp:radiobuttonlist id="rdPrivateLbl" CssClass="rbtn" Width="200" RepeatDirection="Horizontal" Runat="server">
											<asp:ListItem Value="Y">On</asp:ListItem>
											<asp:ListItem Value="N" Selected="True">Off</asp:ListItem>
										</asp:radiobuttonlist></TD>
									<TD class="tablecol"></TD>
								</TR>
								<TR width="100%">
									<TD class="tablecol">&nbsp;<STRONG>Skin </STRONG>:</TD>
									<TD class="tablecol"><asp:radiobuttonlist id="rdSkin" CssClass="rbtn" Width="295" RepeatDirection="Horizontal" Runat="server"
											RepeatColumns="3">
											<asp:ListItem Value="1" Selected="True">Skin 1</asp:ListItem>
											<asp:ListItem Value="2">Skin 2</asp:ListItem>
											<asp:ListItem Value="3">Skin 3</asp:ListItem>
											<asp:ListItem Value="4">Skin 4</asp:ListItem>
											<asp:ListItem Value="5">Skin 5</asp:ListItem>
											<asp:ListItem Value="6">Skin 6</asp:ListItem>
										</asp:radiobuttonlist></TD>
									<TD class="tablecol"></TD>
								</TR>
								<TR>
									<TD class="tablecol">&nbsp;<STRONG>For Training/ Demo Purpose </STRONG>:</TD>
									<TD class="tablecol">&nbsp;<asp:checkbox id="chktraining" Runat="server"></asp:checkbox></TD>
									<TD class="tablecol"></TD>
								</TR>
								<TR>
									<TD class="emptycol"><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
										required field</TD>
								</TR>
							</TABLE>
						</DIV>
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" style="HEIGHT: 32px"><asp:button id="cmdsave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" onclick="ValidatorReset();" type="button" value="Reset"
							name="Reset1" runat="server">&nbsp;<asp:button id="cmdApprove" runat="server" CssClass="button" Text="Approve"></asp:button>&nbsp;<asp:button id="cmdReject" runat="server" CssClass="button" Text="Reject"></asp:button></TD>
				</TR>
				<TR>
					<TD class="EmptyCol"></TD>
				</TR>
				<TR id="trBack" runat="server">
					<TD class="emptycol" colSpan="2"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
				<TR>
					<TD><asp:validationsummary id="vldsumm" runat="server" CssClass="errormsg" Width="696px"></asp:validationsummary></TD>
				</TR>
			</TABLE>
			</FORM>
		<script language="javascript">
	<!--
		function Page_ClientValidate() {
			var i;

			document.getElementById("txtLst").value = ""
			i = 0;
			while (true) {
				chk = document.getElementById("chkLst_" + i++);
				if (chk == null) break;
				
				try {
					if (chk.checked == true) {
						document.getElementById("txtLst").value = "true"
						break;
					}
				}
				catch(e) {
					break;
				}
			}

			for (i = 0; i < Page_Validators.length; i++) {
				ValidatorValidate(Page_Validators[i]);
				
				if (document.activeElement.id == "cmdReject") {
					if (Page_Validators[i].id == "rfv_txtCoyID" || Page_Validators[i].id == "rev_txtCoyID" || Page_Validators[i].id == "rfv_txtCoyName") {
//						alert(Page_Validators[i].isvalid);
					} else {
						Page_Validators[i].isvalid = true;
					}
				}
			}
			
				ValidatorUpdateIsValid();    
				ValidationSummaryOnSubmit();
				Page_BlockSubmit = !Page_IsValid;
				return Page_IsValid;
		}

	//-->
		</script>
	</BODY>
</HTML>
