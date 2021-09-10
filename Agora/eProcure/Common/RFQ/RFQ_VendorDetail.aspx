<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="RFQ_VendorDetail.aspx.vb" Inherits="eProcure.RFQ_VendorDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RFQ_VendorDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sCollapseUp As String = dDispatcher.direct("Plugins/images", "collapse_up.gif")
		    Dim sCollapseDown As String = dDispatcher.direct("Plugins/Images", "collapse_down.gif")		    
		    Dim sOpen As String = dDispatcher.direct("ExtraFunc", "GeneratePDF.aspx", "pageid=" + strPageId + "&type=INV")		    
        </script>      
        <%response.write(Session("WheelScript"))%>        
		<script language="javascript">
	<!--
		    function showHide(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = ''; 
	                    document.getElementById("Image2").src = '<% Response.Write(sCollapseUp)%>';
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
	                    document.getElementById("Image2").src = '<% Response.Write(sCollapseDown)%>';
                    }
            }
 
 		    function showHide1(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';
	                    document.getElementById("Image3").src = '<% Response.Write(sCollapseUp)%>';
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
	                    document.getElementById("Image3").src = '<% Response.Write(sCollapseDown)%>';
                    }
            }
		    function showHide2(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';
	                    document.getElementById("Image6").src = '<% Response.Write(sCollapseUp)%>';
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
	                    document.getElementById("Image6").src = '<% Response.Write(sCollapseDown)%>';
                    }
            }
		    function showHide3(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';
	                    document.getElementById("Image4").src = '<% Response.Write(sCollapseUp)%>';
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
	                    document.getElementById("Image4").src = '<% Response.Write(sCollapseDown)%>';
                    }
            }
		    function showHide4(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';
	                    document.getElementById("Image5").src = '<% Response.Write(sCollapseUp)%>';
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
	                    document.getElementById("Image5").src = '<% Response.Write(sCollapseDown)%>';
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
				
				//alert(UsrType)
				
				if (UsrType == 'VENDOR'){
						//alert(UsrType)
						//document.forms(0).PreTemp.value = "Preview Invoice Template";
						//convert 'hdn_pageid' to string using decimal base
						msg=window.open("../Invoice/PreviewInvoice.aspx?pageid=" + document.forms(0).hdn_pageid.value.toString(10) + "&com_id=" + comname + "&inv_from=admin&img=" + temp,"","width=700,Height=500,resizable=yes,scrollbars=yes");
				}
				else {
						
						//document.forms(0).PreTemp.value = "Preview PO Template";
						msg=window.open("../PO/POReport.aspx?pageid=" + document.forms(0).hdn_pageid.value.toString(10) + "&com_id=" + comname + "&po_from=admin&img=" + temp,"","Width=700,Height=500,resizable=yes,scrollbars=yes");		 	                                 
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
	</HEAD>
	<BODY MS_POSITIONING="GridLayout">
		<FORM id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute;"
				cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD style="width: 100%">
						<TABLE class="alltable" id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tableheader" colSpan="7">&nbsp;<asp:label id="lblHeader" runat="server">Vendor Detail</asp:label></TD>
							</TR>
							
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" colSpan="7"></TD>
							</TR>
							<TR class="tablecol" vAlign="top">
								<TD class="tablecol" width="16%" style="height: 19px">&nbsp;<STRONG>Company Name</STRONG>&nbsp;:</TD>
								<TD class="tablecol" width="25%" style="height: 19px">
                                    <asp:Label ID="lblCoyName" runat="server" CssClass="lbl"></asp:Label></TD>
                                    <td class="tablecol" width="10%" style="height: 19px"><strong>Company Logo:</strong> </td>
                                    <TD width="20%" rowSpan="4" >&nbsp;
									<asp:image id="Image1" runat="server" width="150px" Height="48px"></asp:image><asp:label id="lblLogo" runat="server">No Logo</asp:label></TD>
								
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" ><STRONG>&nbsp;Company Long Name</STRONG>&nbsp;:</TD>
								<TD class="tablecol" >
                                    <asp:Label ID="lblCoyLongName" runat="server" CssClass="lbl"></asp:Label>
                                    </TD>
                                    <td class="tablecol"></td>
                                    <tr vAlign="top">
                                    <TD class="tablecol" ><STRONG>&nbsp;Parent Company </STRONG>:</TD>
                                    <td class="tablecol" >
                                        <asp:Label ID="lblParentCoy" runat="server" CssClass="lbl"></asp:Label>
                                        <asp:DropDownList ID="cboParentCoy" runat="server" style="display:none;" CssClass="ddl">
                                    </asp:DropDownList></td>
                                    <td class="tablecol"></td>
                                    </tr>
                                    <tr vAlign="top">
                                    <TD class="tablecol" ><STRONG>&nbsp;Business Registration No. </STRONG>:</TD>
                                    <td class="tablecol" >
                                    <asp:Label ID="lblBusinessRegNo" runat="server" CssClass="lbl"></asp:Label></td>
                                    <td class="tablecol"></td>
                                    </tr>
                            <TR>
								<TD class="tablecol" style="HEIGHT: 6px" colSpan="7"><hr>
								</TD>
							</TR>
						

							
							</TABLE>
							<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<TR>
								<TD class="tablecol" width="23%"><STRONG>&nbsp;Address</STRONG><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" width="50%" colspan="4">
                                    <asp:Label ID="lblAddress" runat="server" CssClass="lbl"></asp:Label></TD>
								<TD class="tablecol" colspan="2"></TD>
							</TR>
							<TR>
								<TD class="tablecol"></TD>
								<TD class="tablecol"  colspan="4">
                                    <asp:Label ID="lblAddress2" runat="server" CssClass="lbl"></asp:Label></TD>
								<TD class="tablecol"  colspan="2"></TD>
							</TR>
							<TR>
								<TD class="tablecol"></TD>
								<TD class="tablecol"  colspan="4">
                                    <asp:Label ID="lblAddress3" runat="server" CssClass="lbl"></asp:Label></TD>
								<TD class="tablecol"  colspan="2"></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>City</STRONG><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" width="25%">
                                    <asp:Label ID="lblCity" runat="server" CssClass="lbl"></asp:Label></TD>
								<TD class="tablecol"></TD>
								
								<TD class="tablecol" width="20%">&nbsp;<STRONG>State</STRONG><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" width="15%">
                                    <asp:Label ID="lblstate" runat="server" CssClass="lbl"></asp:Label>
                                    <asp:DropDownList ID="cboState" runat="server" style="display:none;" CssClass="ddl">
                                    </asp:DropDownList></TD>
								<TD class="tablecol" style="width: 44660px" ></TD>
							</TR>
							<TR>
								<TD class="tablecol" >&nbsp;<STRONG>Post Code</STRONG><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" align="left" >
                                    <asp:Label ID="lblPostCode" runat="server" CssClass="lbl"></asp:Label></TD>
								<TD class="tablecol" align="left" >
								    </TD>
								
								<TD class="tablecol" >&nbsp;<STRONG>Country</STRONG><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" >
                                    <asp:Label ID="lblCountry" runat="server" CssClass="lbl"></asp:Label>
                                    <asp:DropDownList ID="cbocountry" runat="server" style="display:none;" CssClass="ddl">
                                    </asp:DropDownList></TD>
								<TD class="tablecol" style="width: 44660px" ></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Web Sites</STRONG><asp:label id="Label18" runat="server" ></asp:label><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol">
                                    <asp:HyperLink ID="lnkWebsite" runat="server">[lnkWebsite]</asp:HyperLink></TD>
								<TD class="tablecol"></TD>
								
								<TD class="tablecol">&nbsp;<STRONG>Email</STRONG><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" >
                                    <asp:HyperLink ID="lnkEmail" runat="server">[lnkEmail]</asp:HyperLink></TD>
								<TD class="tablecol" style="width: 44660px"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="height: 24px">&nbsp;<STRONG>Phone</STRONG><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" align="left" style="height: 24px">
                                    <asp:Label ID="lblPhone" runat="server" CssClass="lbl"></asp:Label></TD>
								<TD class="tablecol" align="left" style="height: 24px;">
									</TD>
								
								<TD class="tablecol" style="height: 24px">&nbsp;<STRONG>Fax</STRONG><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" style="height: 24px" >
                                    <asp:Label ID="lblFax" runat="server" CssClass="lbl"></asp:Label></TD>
								<TD class="tablecol" style="width: 44660px; height: 24px">
									</TD>
							</TR>
							</TABLE>
					
							
							    <div style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide('ItemSpec')">
					    <asp:label id="Label30" runat="server">Company Registration</asp:label>
                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Common/Plugins/images/collapse_up.gif" /></div>
					    <div id="ItemSpec" style="display:inline"  >
				        <table class="alltable" id="Table4" border="0" width="100%" cellSpacing="0" cellPadding="0" >
							<TR>
								<TD class="tablecol" width="23%">&nbsp;<asp:label id="Label19" runat="server" CssClass="lblname" Text="Year of Registration"></asp:label><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" width="24%">
                                    <asp:Label ID="lblYear" runat="server" CssClass="lbl"></asp:Label></TD>
								<TD class="tablecol" width="1%"></TD>
								<TD class="tablecol" width="20%">&nbsp;<asp:label id="Label20" runat="server" CssClass="lblname" text="Paid-up Capital" Height="5px"></asp:label>&nbsp;:</TD>
								<TD class="tablecol" width="20%">
                                    <asp:Label ID="lblPaidCurrency" runat="server" CssClass="lbl"></asp:Label>
                                    <asp:DropDownList ID="ddlCurrency" runat="server" style="display:none;" CssClass="ddl">
                                </asp:DropDownList></TD>
								<TD class="tablecol" width="14%">
                                    &nbsp;<asp:Label ID="lblPaid" runat="server" CssClass="lbl"></asp:Label></TD>
								<TD class="tablecol"   width="1%"></TD>
							</TR>
							<TR>
								<TD class="tablecol" >&nbsp;<asp:label id="Label21" runat="server" CssClass="lblname" Text="Company Ownership"></asp:label><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" >
                                    <asp:Label ID="lblCoyOwnership" runat="server" CssClass="lbl"></asp:Label>
                                    <asp:DropDownList ID="ddlOwnerShip" runat="server" style="display:none;" CssClass="ddl">
                                </asp:DropDownList></TD>
								<TD class="tablecol" ></TD>
								<TD class="tablecol" >&nbsp;<asp:label id="Label22" runat="server" CssClass="lblname" text="Others, Please specify" ></asp:label>&nbsp;:</TD>
								<TD class="tablecol" colspan="2">
                                    <asp:Label ID="lblOwnership" runat="server" CssClass="lbl"></asp:Label></TD>
								<TD class="tablecol" ></TD>
							</TR>
							<TR>
								<TD class="tablecol" >&nbsp;<asp:label id="Label23" runat="server" CssClass="lblname" Text="Business Nature"></asp:label><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" >
                                    <asp:Label ID="lblBusiness" runat="server" CssClass="lbl"></asp:Label>
                                    <asp:DropDownList ID="ddlBusiness" runat="server" style="display:none;" CssClass="ddl">
                                    </asp:DropDownList></TD>
								<TD class="tablecol" ></TD>
								<TD class="tablecol" >&nbsp;<asp:label id="Label24" runat="server" CssClass="lblname" Text="Commodity Type"></asp:label><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" >
                                    <asp:Label ID="lblCommodity" runat="server" CssClass="lbl"></asp:Label>
                                    <asp:DropDownList ID="ddlCommodity" runat="server" style="display:none;" >
                                    </asp:DropDownList></TD>
								<TD class="tablecol" colspan="2"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="height: 19px">&nbsp;<STRONG>SST Registration No. </STRONG>:</TD>
								<TD class="tablecol" style="height: 19px">
                                    <asp:Label ID="lblGSTRegNo" runat="server" CssClass="lbl"></asp:Label></TD>
								<TD class="tablecol" style="height: 19px"></TD>
								<TD class="tablecol" style="height: 19px" >&nbsp;<asp:label id="Label25" runat="server" CssClass="lblname" Text="Organization Code"></asp:label><STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tablecol" style="height: 19px" >
                                    <asp:Label ID="lblOrgCode" runat="server" CssClass="lbl"></asp:Label></TD>
								<TD class="tablecol" colspan="2" style="height: 19px"></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="height: 19px">&nbsp;<STRONG>Bank Name </STRONG>:</TD>
								<TD class="tablecol" style="height: 19px">
                                    <asp:Label ID="lblBankName" runat="server"></asp:Label></TD>
								<TD class="tablecol" style="height: 19px" ></TD>
								<TD class="tablecol" style="height: 19px"><STRONG>&nbsp;Bank Account No. </STRONG>:<STRONG>&nbsp;</STRONG></TD>
								<TD class="tablecol" style="height: 19px">
                                    <asp:Label ID="lblAccountNo" runat="server" CssClass="lbl"></asp:Label></TD>
								<TD class="tablecol" colspan="2" style="height: 19px"></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Bank Branch Code </STRONG>:<STRONG> </STRONG>
								</TD>
								<TD class="tablecol">
                                    <asp:Label ID="lblBranchCode" runat="server" CssClass="lbl"></asp:Label></TD>
                                    <TD class="tablecol" style="height: 19px" ></TD>
                                    <TD class="tablecol">&nbsp;<STRONG>Bank Code </STRONG>:<STRONG> </STRONG>
								</TD>
								<TD class="tablecol" colspan="5">
                                    <asp:Label ID="lblBankCode" runat="server" CssClass="lbl"></asp:Label></TD>
								
							</TR>
				        </table>
	                    </div>			              
					</TD>
				</TR>
								
				<TR>
					<TD class="emptycol" style="width: 100%">
                        <div style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide1('Div1')">
					    <asp:label id="Label1" runat="server">Previous Year Sales Area</asp:label>
                            <asp:Image ID="Image3" runat="server" ImageUrl="~/images/collapse_up.gif" /></div>
					    <div id="Div1" style="display:inline"  >
					    <TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
								<TD class="tablecol" style="width: 30%; height: 24px;" >&nbsp;<STRONG>Local Domestic
                                    Sales (%)&nbsp; :</STRONG></TD>
								<td class="tablecol" width="80%" style="height: 24px" >
                                    <asp:Label ID="lblLocalSales" runat="server" CssClass="lbl"></asp:Label></td>
							</TR>
							
							<TR>
								<TD class="tablecol" style="width: 20%; height: 19px;" >&nbsp;<STRONG>Export Sales (%)&nbsp; :</STRONG></TD>
								<td class="tablecol" width="80%" style="height: 19px" >
                                    <asp:Label ID="lblExportSales" runat="server" CssClass="lbl"></asp:Label></td>
							</TR>
				</TABLE> 
					    
					    </div> 
					    </TD>
				</TR>
				<TR>
					<TD class="emptycol" style="width: 100%">
                        <div style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide2('Div2')">
					    <asp:label id="Label12" runat="server">Sales TurnOver</asp:label>
                            <asp:Image ID="Image6" runat="server" ImageUrl="~/images/collapse_up.gif" /></div>
					    <div id="Div2" style="display:inline"  >
			<TABLE class="alltable" id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="emptycol" style="height: 19px">
						<P>
                            <asp:DataGrid ID="dtgSalesTurnover" runat="server">
                                <Columns>
                                    <asp:BoundColumn DataField="CS_YEAR" HeaderText="Year" ReadOnly="True">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="30%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="CS_CURRENCY_CODE" HeaderText="Currency" ReadOnly="True">
                                        <HeaderStyle Width="40%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="CS_AMOUNT" HeaderText="Amount" ReadOnly="True">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" Width="40%" />
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>&nbsp;</P>
					</TD>
				</TR>
				</TABLE>
				</div>
				
                        
                        </TD>
				</TR>
				<TR>
					<TD class="emptycol" style="width: 100%">
                        <div style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide3('Div3')">
					    <asp:label id="Label13" runat="server">Software Application</asp:label>
                            <asp:Image ID="Image4" runat="server" ImageUrl="~/images/collapse_up.gif" /></div>
					    <div id="Div3" style="display:inline"  >
			<TABLE class="alltable" id="Table7" cellSpacing="0" cellPadding="0" width="100%" border="0">
			<tr>
					<TD class="emptycol" style="height: 19px">
						<P><asp:datagrid id="dtgApp" runat="server" OnPageIndexChanged="dtgApp_Page">
								<Columns>
									<asp:BoundColumn DataField="CSW_DESCRIPTION"  readonly="True"  HeaderText="Software Application">
										<HeaderStyle Width="100%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="CSW_INDEX"></asp:BoundColumn>
								</Columns>
							</asp:datagrid></P>
					</TD>
						
			</tr>
			</TABLE></div></TD>
				</TR>
				<tr>
				<TD class="emptycol" style="width: 100%">
                        <div style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide4('Div4')">
					    <asp:label id="Label26" runat="server">Quality Standard Attachments</asp:label>
                            <asp:Image ID="Image5" runat="server" ImageUrl="~/images/collapse_up.gif" /></div>
					    <div id="Div4" style="display:inline"  >
			<TABLE class="alltable" id="Table8" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
												<TD class="tablecol"><STRONG>&nbsp;File(s) Attached </STRONG>:</TD>
												<TD class="tableinput"><asp:panel id="pnlAttach" runat="server"></asp:panel></TD>
											</TR>
											<TR>
												<TD class="tablecol" height="*"></TD>
												<TD class="tableinput"></TD>
											</TR>
			</TABLE></div></TD>
			</tr>
				<tr>
					<td class="emptycol" colSpan="5"></td>
				</tr>
				<TR>
					<TD class="emptycol" colSpan="5">
					    <A id="lnkBack" onclick="history.back();" href="#" runat="server"><STRONG>&lt;Back</STRONG></A>
					    <asp:button id="cmdClose" runat="server" CssClass="button" Text="Close" CausesValidation="false" ></asp:button>&nbsp;
					</TD>
				</TR>
			</TABLE>
			
		</form>
	</body>
</HTML>
