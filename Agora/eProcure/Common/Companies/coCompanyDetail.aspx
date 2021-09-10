<%@ Page Language="vb" AutoEventWireup="false" Codebehind="coCompanyDetail.aspx.vb" Inherits="eProcure.coCompanyDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Com_Profile</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		 <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim collapse_up as string = dDispatcher.direct("Plugins/images","collapse_up.gif")
            dim collapse_down as string = dDispatcher.direct("Plugins/images","collapse_down.gif")
            dim PreviewPOVendor as string = dDispatcher.direct("Invoice","InvoiceReport.aspx","pageid='+document.forms(0).hdn_pageid.value.toString(10)+'&com_id='+comname+'&inv_from=admin&img='+temp+'")
            dim PreviewPO as string = dDispatcher.direct("PO","POReport.aspx","pageid='+document.forms(0).hdn_pageid.value.toString(10)+'&com_id='+comname+'&po_from=admin&img='+temp+'") 
            Dim commodity As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Commodity")
        </script> 
		<%response.write(Session("WheelScript"))%>
		<script type="text/javascript">
	<!--
	 $(document).ready(function(){
        
            $("#txtCommodity").autocomplete("<% Response.write(commodity) %>", {
            width: 200,
            scroll: true,
            selectFirst: false
            });
            $("#txtCommodity").result(function(event, data, formatted) {
            if (data)
            $("#hidCommodity").val(data[1]);
            });
            $("#txtCommodity").blur(function() {
            var txtCommodity = document.getElementById("txtCommodity").value;
            if(txtCommodity == "")
            {
                $("#hidCommodity").val("");
            }  
            var hidcommodity = document.getElementById("hidCommodity").value;                        
            if(hidcommodity == "")
            {
                $("#txtCommodity").val("");
            }
            
            });                       
            });
            
		    function showHide(lnkdesc)
            {
                if (document.getElementById(lnkdesc).style.display == 'none')
                {						
	                document.getElementById(lnkdesc).style.display = '';
	                document.getElementById("Image2").src = '<%response.write(collapse_up) %>';
                } 
                else 
                {
	                document.getElementById(lnkdesc).style.display = 'none';
	                document.getElementById("Image2").src = '<%response.write(collapse_down) %>';
                }
            }


            function Change()
            {
            alert(Form1.ddlOwnership.selectedIndex.value);
                if (Form1.ddlOwnership.selectedItem.Text = "Others")
                {
                    Form1.txtOwnership.text = "";
                    Form1.txtOwnership.Enabled = "False";
                }
                else
                {
                    Form1.txtOwnership.Enabled = "True";
               }
            }
            
            function formReset()
			{
				Page_IsValid = true;
				return Page_IsValid;	
			}
	
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
			
			function PreviewPOTemplate(f,UsrType)       
			{	var temp;
				if (f.cmdCompLogo.value!=""){	
					temp='file:///' + f.cmdCompLogo.value;
				}	
				else
				{	
					if (f.cmdCompLogo.value!="")
						temp=document.images["image1"].src;
					else
						temp=''
				}
				var comname = escape(document.forms(0).txtCoyName.value);
				var UsrType;
				
//				alert(UsrType)
				
				if (UsrType == 'VENDOR'){
//					alert(UsrType)
						//document.forms(0).PreTemp.value = "Preview Invoice Template";
						//convert 'hdn_pageid' to string using decimal base
						msg=window.open('<% Response.Write(PreviewPOVendor) %>','','width=700,Height=500,resizable=yes,scrollbars=yes');
				}
				else {
						
						//document.forms(0).PreTemp.value = "Preview PO Template";
						msg=window.open('<% Response.Write(PreviewPO) %>','','Width=700,Height=500,resizable=yes,scrollbars=yes');		 	                                 
				}
							
					
				
				
				//msg.document.clear();
				/* Note that the word SCRIPT was not
					kept intact on one line in the
					write() below.  A bug will parse 
					it and will not compile the script
					correctly if you don't break that
					word when it appears within write()
					statements. */
				/*
				msg.document.write('<HTML><HEAD><TITLE'
				+'>Image Preview</TITLE>'
				+'</HEAD><BODY>'
				+'<img src="'
        		+ temp + '"></img>'
        		+ '</BODY></H'
				+'TML><P>');*/
			}

//-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_CompanyDet_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
				
            <tr>
					<td class="linespacing1" colspan="4"></td>
			</tr>
			
				<tr>
	                <td class = "emptycol" colspan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the relevant field(s) and click the Save button to save the changes."
		                ></asp:label>

	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colspan="6"></td>
			    </tr>
				<tr>
					<td class = "emptycol">
						<table class="alltable" id="Table5" cellspacing="0" cellpadding="0" width="300" border="0">
							<tr>
								<td class="tableheader" colspan="7">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" colspan="7"></td>
							</tr>
							<tr class="tablecol">
								<td class="tablecol" width="17%">&nbsp;<strong>Company ID</strong><asp:label id="Label17" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
								<td class="tablecol" width="25%">
									<asp:textbox id="txtCoyID" runat="server" CssClass="txtbox" ></asp:textbox></td>
								<td class="tablecol" width="1%"><asp:requiredfieldvalidator id="rfv_txtCoyID" runat="server" Display="None" ErrorMessage="Company ID is required."
										ControlToValidate="txtCoyID" EnableClientScript="False"></asp:requiredfieldvalidator></td>
								<td></td>
								<td class="tablecol"  width="23%"><strong>&nbsp;Company Name</strong><asp:label id="Label16" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" width="33%">&nbsp;
									<asp:textbox id="txtCoyName" runat="server" CssClass="txtbox" MaxLength="100"  width="98%" Enabled="False"></asp:textbox></td>
								<td class="tablecol" width="1%"><asp:requiredfieldvalidator id="rfv_txtCoyName" runat="server" Display="None" ErrorMessage="Company Name is required."
										ControlToValidate="txtCoyName" EnableClientScript="False"></asp:requiredfieldvalidator></td>
							</tr>
							<tr>
								<td class="tablecol" ><strong>&nbsp;Parent Company</strong>&nbsp;:</td>
								<td class="tablecol" ><asp:dropdownlist id="cboParentCoy" runat="server" CssClass="ddl" ></asp:dropdownlist></td>
								<td class="tablecol" ></td>
								<td class="tablecol" ></td>
								<td class="tablecol" >&nbsp;<strong>Company Long Name</strong><asp:label id="Label117" runat="server" ></asp:label>&nbsp;:</td>
								<td class="tablecol" >&nbsp;
									<asp:textbox id="txtCoyLongName" runat="server" CssClass="txtbox" MaxLength="255" width="98%"></asp:textbox></td>
							<td class="tablecol"></td></tr>
							<tr class="tablecol" style="DISPLAY: none" >
								<td colspan="7">
									<div id="Hubadmin1" style="DISPLAY: none" runat="server">
										<table class="alltable" id="Table6" cellspacing="0" cellpadding="0" width="300" border="0">
											<tr width="100%">
												<td class="tablecol" ><strong>&nbsp;Status </strong>:</td>
												<td class="tablecol">&nbsp;
													<asp:radiobuttonlist id="rdStatus" CssClass="rbtn" Runat="server" RepeatDirection="Horizontal">
														<asp:ListItem Value="A" Selected="True">Active</asp:ListItem>
														<asp:ListItem Value="I">Inactive</asp:ListItem>
													</asp:radiobuttonlist></td>
												<td class="tablecol"></td>
											</tr>
										</table>
									</div>
								</td>
							</tr>
							<tr>
								<td class="tablecol" style="HEIGHT: 6px" colspan="7"><hr>
								</td>
							</tr>
							<tr class="tablecol">
								<td class="tablecol"><strong>&nbsp;Company Type</strong><asp:label id="Label15" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol">
								<asp:dropdownlist id="cboCompType" CssClass="ddl" width="50%" Runat="server" Enabled="False">
										<asp:ListItem Value="BUYER">Buyer</asp:ListItem>
										<asp:ListItem Value="VENDOR">Vendor</asp:ListItem>
										<asp:ListItem Value="BOTH">Both</asp:ListItem>
									</asp:dropdownlist></td>
								<td class="tablecol"></td>
								<td class="tablecol"></td>
								<td class="tablecol">&nbsp;<strong>Business Registration No.</strong><asp:label id="Label11" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol">&nbsp;&nbsp;<asp:textbox id="txtBusinessRegNo" runat="server" CssClass="txtbox" Enabled="False"></asp:textbox>&nbsp;</td>
								<td class="tablecol"><asp:requiredfieldvalidator id="rfv_BRegNo" runat="server" Display="None" ErrorMessage="Business Registration No. is required."
										ControlToValidate="txtBusinessRegNo" EnableClientScript="False"></asp:requiredfieldvalidator></td>
							</tr>
							<tr class="tablecol" >
								<td class="tablecol"><strong>&nbsp;Package </strong>:</td>
								<td class="tablecol"><asp:dropdownlist id="cboLicensePackage" CssClass="ddl" Runat="server" Enabled="False" width="50%" ></asp:dropdownlist></td>
								<td class="tablecol"></td>
								<td class="tablecol"></td>
											<td class="tablecol">&nbsp;<strong>SST Registration No. </strong>:</td>
								<td class="tablecol" colspan="2">
									&nbsp;&nbsp;<asp:textbox id="txtGSTRegNo" runat="server" CssClass="txtbox" MaxLength="30" Enabled="False" Width="120px"></asp:textbox>
                                    <asp:Label ID="lblDayOfLastStatus" runat="server"></asp:Label></td>
							</tr>
							<tr class="tablecol">
								<td class="tablecol"><strong>&nbsp;User License </strong>:</td>
		                        <td class="tablecol"><asp:textbox id="txtUserLicense" CssClass="txtbox" width="20%" MaxLength="5" Runat="server" Enabled="False"></asp:textbox></td>
								<td class="tablecol"></td>
								<td class="tablecol"></td>
					        <td class="tablecol"><strong>&nbsp;Subscription Start Date </strong>:</td>
								<td class="tablecol">&nbsp;&nbsp;<asp:textbox id="txtSubStart" runat="server" CssClass="txtbox" Enabled="False"></asp:textbox></td>
								<td class="tablecol"><asp:regularexpressionvalidator id="rfv_txtSubStart" runat="server" Display="None" ErrorMessage="Invalid Subcription Start Date"
										ControlToValidate="txtSubStart" ValidationExpression="\d{1,2}/\d{1,2}/\d{4}" EnableClientScript="False"></asp:regularexpressionvalidator></td>
							</tr>
							<tr class="tablecol">
								<td class="tablecol"><strong>&nbsp;Report User </strong>:</td>
								<td class="tablecol"><asp:textbox id="txtReportUser" CssClass="txtbox" width="20%" MaxLength="5" Runat="server" Enabled="False"></asp:textbox></td>
								<td class="tablecol"></td>
								<td class="tablecol"></td>
								<td class="tablecol"><strong>&nbsp;Subscription End Date </strong>:</td>
								<td class="tablecol">&nbsp;&nbsp;<asp:textbox id="txtSubEnd" runat="server" CssClass="txtbox" Enabled="False"></asp:textbox></td>
								<td class="tablecol"><asp:regularexpressionvalidator id="rfv_txtSubEnd" runat="server" Display="None" ErrorMessage="Invalid Subcription End Date"
										ControlToValidate="txtSubEnd" ValidationExpression="\d{1,2}/\d{1,2}/\d{4}" EnableClientScript="False"></asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td class="tablecol" style="HEIGHT: 6px" colspan="7"><hr>
								</td>
							</tr>
							<tr>
								<td class="tablecol"><strong>&nbsp;Address</strong><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" colspan="4">
									<asp:textbox id="txtAddress" runat="server" CssClass="txtbox" width="100%" MaxLength="255"></asp:textbox></td>
								<td class="tablecol" colspan="2"><asp:requiredfieldvalidator id="rfv_address" runat="server" Display="None" ErrorMessage="Address is required ."
										ControlToValidate="txtAddress" EnableClientScript="False"></asp:requiredfieldvalidator></td>
							</tr>
							<tr>
								<td class="tablecol"></td>
								<td class="tablecol"  colspan="4">
									<asp:textbox id="txtAddress2" runat="server" CssClass="txtbox" width="100%" MaxLength="255"></asp:textbox></td>
								<td class="tablecol"  colspan="2"></td>
							</tr>
							<tr>
								<td class="tablecol" style="height: 12px"></td>
								<td class="tablecol"  colspan="4" style="height: 12px">
									<asp:textbox id="txtAddress3" runat="server" CssClass="txtbox" width="100%" MaxLength="255"></asp:textbox></td>
								<td class="tablecol"  colspan="2" style="height: 12px"></td>
							</tr>
							<tr>
								<td class="tablecol">&nbsp;<strong>City</strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" >
									<asp:textbox id="txtCity" runat="server" CssClass="txtbox" MaxLength="50" Width="100%"></asp:textbox></td>
								<td class="tablecol"><asp:requiredfieldvalidator id="rfv_city" runat="server" Display="None" ErrorMessage="City is required." ControlToValidate="txtCity" EnableClientScript="False"></asp:requiredfieldvalidator></td>
								<td class="tablecol"></td>
								<td class="tablecol">&nbsp;<strong>State</strong><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol">&nbsp;
									<asp:dropdownlist id="cbostate" runat="server" CssClass="ddl"  Width="98%"></asp:dropdownlist></td>
								<td class="tablecol" ><asp:requiredfieldvalidator id="rfv_state" runat="server" Display="None" ErrorMessage="State is required." ControlToValidate="cbostate" EnableClientScript="False"></asp:requiredfieldvalidator></td>
							</tr>
							<tr>
								<td class="tablecol" >&nbsp;<strong>Post Code</strong><asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" align="left" >
									<asp:textbox id="txtPostCode" runat="server" CssClass="txtbox" MaxLength="10" Width="100%"></asp:textbox></td>
								<td class="tablecol" align="left" >
								    <asp:regularexpressionvalidator id="rev_postcode" runat="server" Display="None" ErrorMessage="Invalid Post Code"
										ControlToValidate="txtPostCode" ValidationExpression="\d{4,5}" Enabled="False" EnableClientScript="False"></asp:regularexpressionvalidator></td>
								<td class="tablecol"><asp:requiredfieldvalidator id="rfv_postcode" runat="server" Display="None" ErrorMessage="Post Code is required."
										ControlToValidate="txtPostCode" EnableClientScript="False"></asp:requiredfieldvalidator></td>
								<td class="tablecol" >&nbsp;<strong>Country</strong><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" >&nbsp;
									<asp:dropdownlist id="cbocountry" runat="server" CssClass="ddl" AutoPostBack="True"  Width="98%"></asp:dropdownlist></td>
								<td class="tablecol" ><asp:requiredfieldvalidator id="rfv_country" runat="server" Display="None" ErrorMessage="Country is required."
										ControlToValidate="cbocountry" EnableClientScript="False"></asp:requiredfieldvalidator></td>
							</tr>
							<tr>
								<td class="tablecol">&nbsp;<strong>Web Sites</strong><asp:label id="Label18" runat="server" ></asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol">
									<asp:textbox id="txtWebSites" runat="server"  Width="100%" CssClass="txtbox" MaxLength="50"></asp:textbox></td>
								<td class="tablecol"></td>
								<td class="tablecol"></td>
								<td class="tablecol">&nbsp;<strong>Email</strong><asp:label id="Label10" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" >&nbsp;
									<asp:textbox id="txtEmail" runat="server"  Width="98%" CssClass="txtbox" MaxLength="50"></asp:textbox></td>
								<td class="tablecol"><asp:regularexpressionvalidator id="rev_email" runat="server" Display="None" ErrorMessage="Invalid Email" ControlToValidate="txtEmail"
										ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" EnableClientScript="False"></asp:regularexpressionvalidator><asp:requiredfieldvalidator id="rfv_email" runat="server" Display="None" ErrorMessage="Email is required." ControlToValidate="txtEmail" EnableClientScript="False"></asp:requiredfieldvalidator></td>
							</tr>
							<tr>
								<td class="tablecol" style="height: 38px">&nbsp;<strong>Phone</strong><asp:label id="Label8" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" align="left" style="height: 38px">
									<asp:textbox id="txtPhone" runat="server" CssClass="txtbox"  Width="100%" MaxLength="50"></asp:textbox></td>
								<td class="tablecol" align="left" style="height: 38px">
									<asp:requiredfieldvalidator id="rfv_phone" runat="server" Display="None" ErrorMessage="Phone is required." ControlToValidate="txtPhone" EnableClientScript="False"></asp:requiredfieldvalidator>
								<td class="tablecol" style="height: 38px"></td>
								<td class="tablecol" style="height: 38px">&nbsp;<strong>Fax</strong><asp:label id="Label9" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" style="height: 38px" >&nbsp;
									<asp:textbox id="txtFax" runat="server" CssClass="txtbox"  Width="98%" MaxLength="50"></asp:textbox></td>
								<td class="tablecol" style="height: 38px">
									<asp:requiredfieldvalidator id="rfv_fax" runat="server" Display="None" ErrorMessage="Fax is required." ControlToValidate="txtFax" EnableClientScript="False"></asp:requiredfieldvalidator>
							</tr>
							<tr>
								<td class="tablecol" style="HEIGHT: 6px" colspan="7"><hr>
								</td>
							</tr>
							<tr>
								<td class="tablecol" style="height: 31px"><strong>&nbsp;Currency</strong><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" style="height: 31px">
									<asp:dropdownlist id="cbocurrency" runat="server" CssClass="ddl" Width="100%" ></asp:dropdownlist>
								    <asp:requiredfieldvalidator id="rfv_currency" runat="server" Display="None" ErrorMessage="Currency is required."
										ControlToValidate="cbocurrency" EnableClientScript="False"></asp:requiredfieldvalidator></td>
								<td class="tablecol" colspan="2" style="height: 31px"></td>
								<td class="tablecol" style="height: 31px">&nbsp;<strong>Password Duration</strong><asp:label id="Label14" runat="server" CssClass="errormsg" >*</asp:label>&nbsp;:</td>
								<td class="tablecol" style="height: 31px">&nbsp;
									<asp:textbox id="txt_PwdDur" runat="server" CssClass="txtbox" Width="80px" MaxLength="4"></asp:textbox>&nbsp;Days
								<td class="tablecol" style="height: 31px">
									<asp:requiredfieldvalidator id="PwdDur" runat="server" Display="None" ErrorMessage="Password Duration is required."
										ControlToValidate="txt_PwdDur" EnableClientScript="False"></asp:requiredfieldvalidator>
										<asp:regularexpressionvalidator id="revPwdDur" runat="server" Display="None" ErrorMessage="Password Duration is expecting numeric value."
										ControlToValidate="txt_PwdDur" ValidationExpression="^[0-9]{0,4}$" EnableClientScript="False"></asp:regularexpressionvalidator></td>
							</tr>
							<tr class="tablecol" id="TrPay" style="DISPLAY: none" runat="server">
								<td class="tablecol"><strong>&nbsp;Payment Terms</strong><asp:label id="Label12" runat="server" CssClass="errormsg">*</asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol">
									<asp:dropdownlist id="cbo_PayTerm" runat="server" CssClass="ddl" Width="100%"></asp:dropdownlist>
									<asp:requiredfieldvalidator id="PayTerm" runat="server" Display="None" ErrorMessage="Payment Term is required."
										ControlToValidate="cbo_PayTerm" EnableClientScript="False"></asp:requiredfieldvalidator></td>
								<td class="tablecol" colspan="2"></td>
								<td class="tablecol">&nbsp;<strong>Payment Method</strong><asp:label id="Label13" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
								<td class="tablecol">&nbsp;
									<asp:dropdownlist id="cbo_PayMeth" runat="server" CssClass="ddl" Width="98%"></asp:dropdownlist><asp:requiredfieldvalidator id="PayMethod" runat="server" Display="None" ErrorMessage="Payment Method Value is required."
										ControlToValidate="cbo_PayMeth" EnableClientScript="False"></asp:requiredfieldvalidator></td>
								<td class="tablecol"></td>
						</tr>
							<tr>
								<td class="tablecol" style="HEIGHT: 6px" colspan="7"><hr>
								</td>
							</tr>
							</table>
						<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="300" border="0">
							<tr>
								<td class="tablecol">&nbsp;<strong>Current Company&nbsp;Logo </strong>:</td>
								<td class="tablecol">&nbsp;
									<asp:image id="Image1" runat="server"  Height="30"></asp:image><asp:label id="lblLogo" runat="server">No Logo</asp:label></td>
								<td class="tablecol"></td>
							</tr>
							<tr>
								<td class="tablecol" >&nbsp;<strong>Company&nbsp;Logo </strong>
									:<br/>
									&nbsp;<asp:label id="lblComLogo" runat="server" CssClass="small_remarks"></asp:label></td>
								<td class="tablecol" >&nbsp;&nbsp;<input class="button" id="cmdCompLogo" style="FONT-SIZE: 8pt; WIDTH: 340px; FONT-FAMILY: verdana; BACKGROUND-COLOR: white"
										type="file" runat="server"/>&nbsp;<input class="button" id="PreTemp" style="WIDTH: 136px; HEIGHT: 18px" onclick="PreviewPOTemplate(this.form, cboCompType.value);"
										type="button" value="Preview PO Template" runat="server"/></td>
								<td class="tablecol" ></td>
							</tr>
							<tr>
								<td class="tablecol" >&nbsp;<strong>T&amp;C Document Upload </strong>:<br>
									&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="small_remarks" Width="176px">Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label>
								</td>
								<td class="tablecol" >&nbsp;&nbsp;<input class="button" id="cmdTCBrow" style="FONT-SIZE: 8pt; WIDTH: 340px; FONT-FAMILY: verdana; BACKGROUND-COLOR: white"
										type="file" runat="server"/> &nbsp;&nbsp;&nbsp;&nbsp;<asp:hyperlink id="lnkTC" runat="server">HyperLink</asp:hyperlink></td>
								<td class="tablecol" ></td>
							</tr>
							<tr>
								<td class="tablecol" style="HEIGHT: 6px" colspan="3"><hr>
								</td>
							</tr>
							</table>
						<table class="alltable" id="Table2" style="DISPLAY: none" cellspacing="0" cellpadding="0" width="300" border="0">
							<tr id="tdVendor" style="DISPLAY: none" runat="server">
								<td class="tablecol" style="HEIGHT: 38px">&nbsp;<strong>Tax Calculate By </strong>:</td>
								<td class="tablecol" style="HEIGHT: 38px" height="38">&nbsp;
									<asp:radiobuttonlist id="rdGSTCalcBy" CssClass="rbtn" Width="210" Runat="server" RepeatDirection="Horizontal"
										Height="0">
										<asp:ListItem Value="0">Item</asp:ListItem>
										<asp:ListItem Value="1" Selected="True">Sub Total</asp:ListItem>
									</asp:radiobuttonlist></td>
								<td class="tablecol" style="HEIGHT: 38px"></td>
							</tr>
							<tr class="tablecol">
								<td colspan="3">
									<div id="Hubadmin2" style="DISPLAY: none" runat="server">
										<table class="alltable" id="Table7" cellspacing="0" cellpadding="0" width="300" border="0">
											<tr width="100%">
												<td class="tablecol" width="200"><strong>&nbsp;BCM Setting </strong>:</td>
												<td class="tablecol"><asp:radiobuttonlist id="rdBCM" CssClass="rbtn" Width="200" Runat="server" RepeatDirection="Horizontal">
														<asp:ListItem Value="1">On</asp:ListItem>
														<asp:ListItem Value="0" Selected="True">Off</asp:ListItem>
													</asp:radiobuttonlist><asp:TextBox id="txtBCMSetting" style="DISPLAY:none" Runat="server"></asp:TextBox></td>
												<td class="tablecol"></td>
											</tr>
											<tr>
												<td class="tablecol"><strong>&nbsp;Finance Dept. Mode </strong>:</td>
												<td class="tablecol"><asp:radiobuttonlist id="rdFinMode" CssClass="rbtn" Width="200" Runat="server" RepeatDirection="Horizontal">
														<asp:ListItem Value="Y">On</asp:ListItem>
														<asp:ListItem Value="N" Selected="True">Off</asp:ListItem>
													</asp:radiobuttonlist></td>
												<td class="tablecol"></td>
											</tr>
											<tr>
												<td class="tablecol"><strong>&nbsp;Invoice &amp; Payment Approval </strong>:</td>
												<td class="tablecol"><asp:radiobuttonlist id="rdInvAppr" CssClass="rbtn" Width="200" Runat="server" RepeatDirection="Horizontal">
														<asp:ListItem Value="Y">On</asp:ListItem>
														<asp:ListItem Value="N" Selected="True">Off</asp:ListItem>
													</asp:radiobuttonlist></td>
												<td class="tablecol"></td>
											</tr>
											<tr>
												<td class="tablecol"><strong>&nbsp;PR To Multiple POs </strong>:</td>
												<td class="tablecol"><asp:radiobuttonlist id="rdInvMultiAppr" CssClass="rbtn" Width="200" Runat="server" RepeatDirection="Horizontal">
														<asp:ListItem Value="Y">On</asp:ListItem>
														<asp:ListItem Value="N" Selected="True">Off</asp:ListItem>
													</asp:radiobuttonlist></td>
												<td class="tablecol"></td>
											</tr>
											<tr>
												<td class="tablecol">&nbsp;<strong>Buyer Admin Cancel POs </strong>:</td>
												<td class="tablecol"><asp:radiobuttonlist id="rdBA" CssClass="rbtn" Width="200" Runat="server" RepeatDirection="Horizontal">
														<asp:ListItem Value="Y">On</asp:ListItem>
														<asp:ListItem Value="N" Selected="True">Off</asp:ListItem>
													</asp:radiobuttonlist></td>
												<td class="tablecol"></td>
											</tr>
										</table>
									</div>
								</td>
							</tr>
							<tr>
							</tr>
							<tr class="tablecol">
								<td colspan="3">
									<DIV id="Hubadmin3" style="DISPLAY: none" runat="server">
										<table class="alltable" id="Table8" cellspacing="0" cellpadding="0" width="200" border="0">
											<tr width="100%">
												<td class="tablecol" width="200"><strong>&nbsp;Private Labeling </strong>:</td>
												<td class="tablecol"><asp:radiobuttonlist id="rdPrivateLbl" CssClass="rbtn" Width="200" Runat="server" RepeatDirection="Horizontal">
														<asp:ListItem Value="Y">On</asp:ListItem>
														<asp:ListItem Value="N" Selected="True">Off</asp:ListItem>
													</asp:radiobuttonlist></td>
												<td class="tablecol"></td>
											</tr>
											<tr width="100%">
												<td class="tablecol" width="200"><strong>&nbsp;Skin </strong>:</td>
												<td class="tablecol"><asp:radiobuttonlist id="rdSkin" CssClass="rbtn" Width="295" Runat="server" RepeatDirection="Horizontal"
														RepeatColumns="3">
														<asp:ListItem Value="1" Selected="True">Skin 1</asp:ListItem>
														<asp:ListItem Value="2">Skin 2</asp:ListItem>
														<asp:ListItem Value="3">Skin 3</asp:ListItem>
														<asp:ListItem Value="4">Skin 4</asp:ListItem>
														<asp:ListItem Value="5">Skin 5</asp:ListItem>
														<asp:ListItem Value="6">Skin 6</asp:ListItem>
													</asp:radiobuttonlist></td>
												<td class="tablecol"></td>
											</tr>
											<tr>
												<td class="tablecol"><strong>&nbsp;For Training/ Demo Purpose </strong>:</td>
												<td class="tablecol">&nbsp;
													<asp:checkbox id="chktraining" Runat="server"></asp:checkbox></td>
												<td class="tablecol"></td>
											</tr>
										</table>
									</DIV>
								</td>
							</tr>
							<tr>
								<td class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" colspan="3"></td>
							</tr>
						</table>
							    <div style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide('ItemSpec')">
					    <asp:label id="Label30" runat="server">Company Registration</asp:label>
                                    <asp:Image ID="Image2" runat="server" ImageUrl="#" /></div>
					    <div id="ItemSpec" style="display:inline"  >
				        <table class="alltable" id="Table4" border="0" width="100%" cellspacing="0" cellpadding="0" >
							<tr>
								<td class="tablecol" width="17%">&nbsp;<asp:label id="Label19" runat="server" CssClass="lbl" Text="Year of Registration"></asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" width="23%">
									<asp:dropdownlist id="ddlYear" runat="server" CssClass="ddl" Width="55%" ></asp:dropdownlist></td>
								<td class="tablecol" width="1%"></td>
								<td class="tablecol" width="26%">&nbsp;<asp:label id="Label20" runat="server" CssClass="lbl" text="Paid-up Capital" ></asp:label>&nbsp;:</td>
								<td class="tablecol" width="20%">
									<asp:dropdownlist id="ddlCurrency" runat="server" CssClass="ddl" Width="100%" ></asp:dropdownlist></td>
								<td class="tablecol" width="14%">
									<asp:textbox id="txtPaid" runat="server" CssClass="numerictxtbox" MaxLength="10" Width="100%"></asp:textbox>
								    <asp:regularexpressionvalidator id="Regularexpressionvalidator1" runat="server" Display="None" ErrorMessage="Invalid Paid-up Capital"
										ControlToValidate="txtPaid" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,2})?$" EnableClientScript="False"></asp:regularexpressionvalidator></td>
								<td class="tablecol"   width="1%"></td>
							</tr>
							<tr>
								<td class="tablecol" >&nbsp;<asp:label id="Label21" runat="server" CssClass="lbl" Text="Company Ownership"></asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" >
									<asp:dropdownlist id="ddlOwnership" runat="server" CssClass="ddl" Width="100%" ></asp:dropdownlist></td>
								<td class="tablecol" ></td>
								<td class="tablecol" >&nbsp;<asp:label id="Label22" runat="server" CssClass="lbl" text="Others, Please specify" ></asp:label>&nbsp;:</td>
								<td class="tablecol" colspan="2">
									<asp:textbox id="txtOwnership" runat="server" CssClass="txtbox" MaxLength="50" Width="100%"></asp:textbox></td>
								<td class="tablecol" ></td>
							</tr>
							<tr>
								<td class="tablecol" >&nbsp;<asp:label id="Label23" runat="server" CssClass="lbl" Text="Business Nature"></asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" >
									<asp:dropdownlist id="ddlBusiness" runat="server" CssClass="ddl" Width="100%" ></asp:dropdownlist></td>
								<td class="tablecol" ></td>
								<td class="tablecol" >&nbsp;<asp:label id="Label24" runat="server" CssClass="lbl" Text="Commodity Type"></asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" >
									<%--<asp:dropdownlist id="ddlCommodity" runat="server" CssClass="ddl" Width="100%" ></asp:dropdownlist>--%>
									<asp:textbox id="txtCommodity" width="100%" runat="server" CssClass="txtbox"></asp:textbox><input type="hidden" id="hidCommodity" runat="server" /></td>
								<td class="tablecol" colspan="2"></td>
							</tr>
							<tr>
								<td class="tablecol" >&nbsp;<asp:label id="Label25" runat="server" CssClass="lbl" Text="Organization Code"></asp:label><strong>&nbsp;</strong>:</td>
								<td class="tablecol" >
									<asp:textbox id="txtOrgCode" runat="server" CssClass="txtbox" MaxLength="50" Width="100%"></asp:textbox></td>
								<td class="tablecol" colspan="5"></td>
							</tr>
							<tr>
								<td class="tablecol">&nbsp;<strong>Bank Name </strong>:</td>
								<td class="tablecol">
                                    <asp:TextBox ID="txtBankName" runat="server" CssClass="txtbox" MaxLength="90" Width="100%"></asp:TextBox></td>
								<td class="tablecol" ></td>
								<td class="tablecol"><strong>&nbsp;Bank Account No. </strong>:<strong> </strong>
								</td>
								<td class="tablecol">
									<asp:textbox id="txtAccountNo" runat="server" CssClass="txtbox" width="100%" MaxLength="30"  ></asp:textbox></td>
								<td class="tablecol" colspan="3"></td>
							</tr>
							<tr>
								<td class="tablecol">&nbsp;<strong>Bank Branch Code </strong>:<strong> </strong>
								</td>
								<td class="tablecol">
									<asp:textbox id="txtBranchCode" runat="server" CssClass="txtbox" width="100%" MaxLength="30"
										></asp:textbox></td>
								<td class="tablecol" ></td>
								<td class="tablecol"><strong>&nbsp;Bank Code </strong>:<strong> </strong>
								</td>
								<td class="tablecol">
									<asp:textbox id="txtBankCode" runat="server" CssClass="txtbox" MaxLength="30" width="100%"  ></asp:textbox><asp:textbox id="txtTrans" runat="server" CssClass="txtbox" MaxLength="30" width="100%" Visible="False"  ></asp:textbox></td>
								<td class="tablecol" ></td>
								<td class="tablecol" ></td>
								
							</tr>
				        </table>
	                    </div>			              

				<asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol"><asp:button id="cmdsave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<input class="button" id="cmdReset" onclick="ValidatorReset();" type="button" value="Reset"
							name="Reset1" runat="server"/></td>
				</tr>
				<tr>
					<td class="emptycol"><input id="hdn_pageid" style="WIDTH: 64px; HEIGHT: 22px" type="hidden" size="5" name="hdn_pageid"
							runat="server"/></td>
				</tr>
				<tr>
					<td class="emptycol" style="HEIGHT: 10px"><asp:validationsummary id="vldsumm" runat="server" CssClass="errormsg" Width="696px"></asp:validationsummary></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
			</table>
		</form>
	</body>
</html>
