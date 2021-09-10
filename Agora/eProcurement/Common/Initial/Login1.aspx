<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Login1.aspx.vb" Inherits="eProcurement.Login" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<HEAD>
		<title>Login eProcurement</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../Styles.css" rel="stylesheet">
		<script language="javascript">
  
  <!--
function MM_swapImgRestore() { //v3.0
  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}

function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_swapImage() { //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}
//-->
		</script>
	</HEAD>
	<body text="#000000" bgColor="white" leftMargin="0" topMargin="0">
		<form id="form2" name="form2" action="StartPage.aspx" runat="server">
			<table height="90%" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
				<tr>
					<td>
						<table style="WIDTH: 780px; HEIGHT: 466px" height="460" cellspacing="0" cellpadding="0" width="780" align="center" border="0">
							<tr>
								<td align="center" bgcolor="#ffffff" width="200"><IMG src="Images/logo_tx123_2.jpg"></td>
								<td id="oFilterDIV" style="FILTER: progid: DXImageTransform.Microsoft.Gradient(GradientType=1, StartColorStr='#ffffff', EndColorStr='#F19402'); HEIGHT: 49px"
									vAlign="middle" width="490"></td>
								<td bgcolor="#f19402" width="100"><!--- <IMG src="Images/eprocurementlogo.jpg"> ---></td>
							</tr>
							<tr borderColor="bisque" bgColor="bisque" valign="top">
								<td vAlign="middle" background="images/wheelbody-785x376.jpg" bgColor="bisque" colSpan="3"
									height="320"><table cellpadding="0" cellspacing="0" border="1" bordercolor="red">
										<tr>
											<td>&nbsp;<font size="1" face="Verdana, Arial, Helvetica, sans-serif">&nbsp;<font color="red"><b>! 
															Important: Update IE to ver 6.0.&nbsp;</b></font>&nbsp;<asp:hyperlink id="lnkBrowser" runat="server" CssClass="hpl">Click Here</asp:hyperlink></font>&nbsp;</td>
										</tr>
									</table>
									<p></p>
									<p></p>
									<table style="WIDTH: 780px" cellspacing="0" cellpadding="0" width="780" align="center"
										border="0">
										<TBODY>
											<tr valign="top">
												<td colspan="4">&nbsp;</td>
											</tr>
											<tr>
												<td style="HEIGHT: 13px"></td>
												<td style="WIDTH: 748px; HEIGHT: 13px" align="right" width="748"></td>
												<td style="HEIGHT: 13px" align="center" colSpan="2"><FONT face="Verdana" size="5"><b><EM><FONT size="4"></FONT></EM></b></FONT></td>
											</tr>
											<tr>
												<td style="HEIGHT: 4px"></td>
												<td style="WIDTH: 748px; HEIGHT: 191px" vAlign="bottom" align="right" width="748" rowSpan="7"><IMG src="images/intro3.gif">&nbsp;</td>
												<td style="WIDTH: 99px; HEIGHT: 4px" vAlign="top" align="center" colSpan="2" rowSpan="7"><FONT color="#696969" size="2"><FONT color="#000000" size="3">&nbsp;</FONT></FONT>
													<table style="WIDTH: 163px; HEIGHT: 205px" cellspacing="0" cellpadding="0" width="163"
														border="0">
														<!--- <tr>
															<td style="WIDTH: 303px; HEIGHT: 11px">&nbsp;</td>
															<td style="WIDTH: 303px; HEIGHT: 11px">&nbsp;</td>
															<td style="WIDTH: 303px; HEIGHT: 11px">&nbsp;</td>
														</tr> --->
														<tr>
															<td style="WIDTH: 303px; HEIGHT: 11px"><FONT size="1"><FONT face="Verdana"><b><FONT color="#696969">Company&nbsp;ID 
																				:</FONT></b></FONT></FONT></td>
															<td style="WIDTH: 123px; HEIGHT: 11px">&nbsp;</td>
															<td style="WIDTH: 149px; HEIGHT: 11px">&nbsp;<asp:textbox id="txtCompID" runat="server" MaxLength="20" Width="121px"></asp:textbox></td>
														</tr>
														<tr>
															<td style="WIDTH: 303px; HEIGHT: 9px"><b><FONT face="Verdana" color="#696969" size="1">User 
																		ID&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</FONT></b></td>
															<td style="WIDTH: 123px; HEIGHT: 9px">&nbsp;</td>
															<td style="WIDTH: 149px; HEIGHT: 9px">&nbsp;<asp:textbox id="txtUserID" runat="server" MaxLength="20" Width="122px"></asp:textbox></td>
														</tr>
														<tr>
															<td style="WIDTH: 303px; HEIGHT: 17px"><b><FONT color="#696969" size="2"><FONT face="Verdana" size="1">Password&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</FONT>
																	</FONT></b>
															</td>
															<td style="WIDTH: 123px; HEIGHT: 17px">&nbsp;</td>
															<td style="WIDTH: 149px; HEIGHT: 17px">&nbsp;<asp:textbox id="txtPassword" runat="server" MaxLength="10" Width="123px" TextMode="Password">qqqqqq</asp:textbox></td>
														</tr>
														<tr>
															<td style="WIDTH: 303px; HEIGHT: 30px" align="right"></td>
															<td style="WIDTH: 123px; HEIGHT: 30px"></td>
															<td style="WIDTH: 149px; HEIGHT: 30px">&nbsp;
																<asp:checkbox id="chkCookies" Text="Remember my ID" Runat="server" Font-Size="Smaller"></asp:checkbox></td>
														</tr>
														<tr>
															<td style="WIDTH: 303px; HEIGHT: 33px" align="left"></td>
															<td style="WIDTH: 123px; HEIGHT: 33px"></td>
															<td style="WIDTH: 149px; HEIGHT: 33px">&nbsp;<asp:imagebutton id="cmdOk" onmouseover="MM_swapImage('cmdOk','','images/loginDown.jpg',1)" onmouseout="MM_swapImage('cmdOk','','images/loginUp.jpg',1)"
																	runat="server" ImageUrl="images/loginUp.jpg"></asp:imagebutton>
																<asp:imagebutton id="cmdClear" onmouseover="MM_swapImage('cmdClear','','images/ResetDown.jpg',1)"
																	onmouseout="MM_swapImage('cmdClear','','images/ResetUp.jpg',1)" runat="server" ImageUrl="images/ResetUp.jpg"></asp:imagebutton>
																<!--<a href="Index.aspx" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image1','','images/loginDown.jpg',1)"><img name="Image1" border="0" src="images/loginUp.jpg"></a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="#" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('Image2','','images/ResetDown.jpg',1)"><img name="Image2" border="0" src="images/ResetUp.jpg"></a>--></td>
														</tr>
														<tr>
															<td style="WIDTH: 303px; HEIGHT: 8px"></td>
															<td style="WIDTH: 123px; HEIGHT: 8px"></td>
															<td style="WIDTH: 149px; HEIGHT: 8px"><asp:hyperlink id="lnkForgotPwd" runat="server" Width="134px" Font-Size="Smaller" NavigateUrl="forgotPwd.aspx"
																	Height="8px"><font style="FONT-SIZE: 7pt; FONT-FAMILY: Verdana; TEXT-DECORATION: underline">Forgot 
																		your password?</font></asp:hyperlink></td>
														</tr>
														<tr>
															<td style="WIDTH: 303px; HEIGHT: 8px"></td>
															<td style="WIDTH: 123px; HEIGHT: 8px"></td>
															<td style="WIDTH: 149px; HEIGHT: 8px"><font style="FONT-SIZE: 7pt; FONT-FAMILY: Verdana; TEXT-DECORATION: underline"><a target="_self" href="forgotPwd.aspx">Unlock 
																	your account</a></font></td>
														</tr>
														<tr>
															<td style="WIDTH: 303px; HEIGHT: 8px"></td>
															<td style="WIDTH: 123px; HEIGHT: 8px"></td>
															<td style="WIDTH: 149px; HEIGHT: 8px"><asp:hyperlink id="lnkpubNotice" runat="server" Width="134px" Font-Size="Smaller" NavigateUrl="/eRFP/PublicRFP/NonRegPublicRFPNoticeBoard.aspx"
																	Height="8px"><font style="FONT-SIZE: 7pt; FONT-FAMILY: Verdana; TEXT-DECORATION: underline">Public 
																		RFP Notice Board</font></asp:hyperlink></td>
														</tr>
														<tr>
															<td style="WIDTH: 303px; HEIGHT: 8px"></td>
															<td style="WIDTH: 123px; HEIGHT: 8px"></td>
															<td style="WIDTH: 149px; HEIGHT: 8px"><a target="_blank" href="/eRFP/PublicVendorReg/PublicVendorRegistration.aspx?mode=add"><font style="FONT-SIZE: 7pt; FONT-FAMILY: Verdana; TEXT-DECORATION: underline">Vendor 
																		Registration</font></a></td>
														</tr>
														<tr>
															<td style="WIDTH: 303px; HEIGHT: 51px"></td>
															<td style="WIDTH: 123px; HEIGHT: 51px"></td>
															<td style="WIDTH: 149px; HEIGHT: 51px"><asp:label id="lblMsg" Runat="server" Font-Size="10px" Font-Name="Verdana" Font-Bold="True"
																	ForeColor="red"></asp:label></td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td style="WIDTH: 320px; HEIGHT: 5px">&nbsp;</td>
											</tr>
											<tr>
												<td style="WIDTH: 320px; HEIGHT: 2px" width="320" height="2">&nbsp;</td>
											</tr>
											<tr>
												<td style="WIDTH: 320px; HEIGHT: 2px" height="2">&nbsp;</td>
											</tr>
											<tr>
												<td style="WIDTH: 320px; HEIGHT: 5px" width="320" height="5">&nbsp;</td>
											</tr>
											<tr>
												<td style="WIDTH: 320px; HEIGHT: 4px" width="320" height="4"></td>
											</tr>
											<tr>
												<td style="WIDTH: 320px; HEIGHT: 2px" width="320" height="4"></td>
											</tr>
											<tr valign="top">
												<td style="WIDTH: 320px; HEIGHT: 43px" colspan="2">&nbsp;
												</td>
												<td style="HEIGHT: 43px" align="right">
												    <table width="135" border="0" cellpadding="0" cellspacing="0">
														<tr>
															<td align="center" valign="top" style="height: 27px"><script src="https://seal.verisign.com/getseal?host_name=www.tx123.com.my&amp;size=S&amp;use_flash=NO&amp;use_transparent=NO&amp;lang=en"></script><br>
																<a href="http://www.verisign.com/ssl/ssl-information-center/" target="_blank" style="PADDING-RIGHT:0px; PADDING-LEFT:0px; PADDING-BOTTOM:0px; MARGIN:0px; FONT:bold 7px verdana,sans-serif; COLOR:#000000; PADDING-TOP:0px; TEXT-ALIGN:center; TEXT-DECORATION:none">
																	About SSL Certificates</a></td>
														</tr>
													</table>
												</td>
											</tr>
										</TBODY>
									</table>
									<MARQUEE style="WIDTH: 777px; HEIGHT: 3px" runat="server" id="marq" class="div">
										<DIV align="center"><FONT face="Verdana, Arial, Helvetica, sans-serif" size="1">Kompakar 
												eBiz Sdn Bhd © 2004 All Rights Reserved (.NET Version) . Best View with 800 x 
												600 Resolution</FONT></DIV>
									</MARQUEE>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
