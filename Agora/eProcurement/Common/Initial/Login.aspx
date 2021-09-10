<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="eProcurement.LWebForm1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Login eProcurement</title>
    <script runat="server">
        Dim dDispatcher As New AgoraLegacy.dispatcher
        Dim sCSS As String = "<LINK href = """ & dDispatcher.direct("Plugins/CSS", "Styles.css") & """  type=""text/css"" rel=""stylesheet"">"
        Dim sPage As String = dDispatcher.direct("Initial", "default.aspx")
        Dim sNew As String = "<img class=""imgnew"" src=""" & dDispatcher.direct("Plugins/Images", "New.gif") & """ />"
        'Dim sVendorReg As String = "<a target=""_blank"" href=""/eRFP/PublicVendorReg/PublicVendorRegistration.aspx?mode=add"">Vendor Registration</a>"
        Dim sShakeHand As String = "<img src=""" & dDispatcher.direct("Plugins/Images", "FTN login shakehand.JPG") & """ />"
        Dim sGetAdobe As String = "<img src=""" & dDispatcher.direct("Plugins/Images", "get_adobe_reader.png") & """ alt=""Get Adobe Reader"" />"
        Dim sGetIE8 As String = "<img src=""" & dDispatcher.direct("Plugins/Images", "IE8-DLnow.png") & """ alt=""Get Internet Explorer 8"" />"
        Dim sTrustSeal As String = "<img src=""" & dDispatcher.direct("Plugins/Images", "compare-trust-seal.gif") & """ alt=""Validate Us"" />"
        'Dim sMyFairTradeNet_Login As String = "<img src=""" & dDispatcher.direct("Plugins/Images", "MyFairTradeNet_Login.gif") & """ alt="""" />"
        Dim sStrateq_Login As String = "<img src=""" & dDispatcher.direct("Plugins/Images", "Powered by Strateq (before).jpg") & """ alt="""" />"
        'Dim sStrateq_Login As String = "<ImageUrl=""" & dDispatcher.direct("Plugins/Images", "Powered by Strateq (before).jpg") & """ />"        
    </script>
    <% Response.Write(sCSS)%> 
    <script language="javascript">
    function SubmitFormEnter(evt, frmjs, sLang) {
        var charCode = (evt.charCode) ? evt.charCode :
        ((evt.which) ? evt.which : evt.keyCode);
        if (charCode == 13) {
            return SubmitForm(frmjs, sLang);
        }
    }
    
    function changeColour(elementId) {
    var interval = 500;
    var colour1 = "#ff0000"
    var colour2 = "#424dce";
        if (document.getElementById) {
            var element = document.getElementById(elementId);
            element.style.color = (element.style.color == colour1) ? colour2 : colour1;
            setTimeout("changeColour('" + elementId + "')", interval);
        }
    }

    function  ShowMsg(Curr, Amt, Day, Status)
    {    
     var msg=""
     if (Status = "Active" && Amt > 0)   
        {
            msg="Dear customer, you have outstanding amount of " + Curr + Amt + "\r" + "and your account will be turned to limited access in " + Day + " day/days." + "\r" + "Please top up your prepaid balance to clear the outstanding" + "\r" + "amount within the grace period to prevent service interruption.";
        }
     else  
        {
            msg="Dear customer, your account had been turned to limited access" + "\r" + "due to there is outstanding invoice pending in Billing System." + "\r" + "Please top up your prepaid balance to clear the outstanding" + "\r" + "amount to resume the service and continue using AGORA.";
        }
     alert (msg);
     window.location =  "<% Response.Write(sPage)%>";
     }

    function SubmitForm(frmLogin)
    {
        frmLogin.submit(); return true;
    }
    
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
    </script>
    <style type="text/css">
    * html  {	height: 1%;}

        img { border:0; }
        a:link, a:visited, a:focus {color:#333399; text-decoration:none; -moz-outline-style: none; outline: none;} 
        a:active, a:hover {color:#424dce; text-decoration:underline; -moz-outline-style: none; outline: none; border-bottom: none; }
        td { height: 30px; }
        .btn { background-color: #fafafa; border-top: 1px solid #999999; border-right: 1px solid #666666; border-bottom: 1px solid #666666; border-left: 1px solid #999999; color: #333333; cursor:pointer; font-family: Arial, Helvetica; font-size: 11px; font-weight:bold; margin: 0px 4px; outline: none;  -moz-border-radius: 4px; -webkit-border-radius: 4px; width: 50px; padding:3px 8px; float:left; text-align:center; }
        .btn:link, .btn:visited {color: #333333;}
        .btn:active, .btn:hover {background: #efefef; border-top: 1px solid #505050; border-right: 1px solid #303030; border-bottom: 1px solid #303030; border-left: 1px solid #505050; color: #000000; } 
        
        .download_ext {text-align:center;margin-top:10px;cursor:pointer; }
        .download_ext2 {text-align:center;margin-top:10px; }
  
        
    </style>
</head>
<body style="background: #fff; color:#666; font-family:Arial; " onload="changeColour('lblMsg2');">
    <form id="frmLogin" runat="server">
    <div style="margin:0px auto; width:880px; border: 2px solid #26b0eb; padding:20px;">
        <div style="padding:2px 20px; border-bottom: 3px solid #aabdd0; height:80px; clear:both;">
            <div><% Response.Write(Session("topLogo"))%></div>
            <div style="float:right; font-size: 20px; margin-top:30px; color:#1C2172; ">
                <center><asp:label id="lblTitle" runat="server" Text=""></asp:label></center> 
                <center><asp:label id="lblTitle2" runat="server" Text="Software-as-a-Service"></asp:label></center>  
            </div>
            <div style="clear:both; "></div> 
        </div>
        <div style="clear:both; "></div> 
        <div style="margin-top:20px;">
            <div style="float:left; font-weight:bold; font-size:11px; margin-left:10px; width:280px;">
                <table cellspacing="0" cellpadding="0" border="0">                    
				    <tr>
					    <td style="width:100px; ">Company ID:</td>
					    <td>&nbsp;<asp:textbox id="txtCompID" runat="server" MaxLength="20" Width="120px" onkeypress="return SubmitFormEnter(event, frmLogin);"></asp:textbox></td>
				    </tr>
				    <tr>
					    <td>User ID:</td>
					    <td>&nbsp;<asp:textbox id="txtUserID" runat="server" MaxLength="20" Width="120px" onkeypress="return SubmitFormEnter(event, frmLogin);"></asp:textbox></td>
				    </tr>
				    <tr>
					    <td>Password:</td>
					    <td>&nbsp;<asp:textbox id="txtPassword" runat="server" MaxLength="10" Width="120px" TextMode="Password" onkeypress="return SubmitFormEnter(event, frmLogin);"></asp:textbox></td>
				    </tr>
				    <tr>
					    <td></td>
					    <td><asp:checkbox id="chkCookies" Text="Remember my ID" Runat="server"></asp:checkbox></td>
				    </tr>
				    <tr>
					    <td></td>
					    <td>
						    <div class="btn" onclick="SubmitForm(frmLogin); return false; " id="DIV1">Log In</div>
						    <div class="btn" onclick="document.frmLogin.reset(); return false; ">Reset</div>
						</td>
				    </tr>
				    <tr><td style="height:10px;"></td></tr>
				    <tr>
					    <td style="height:20px;"></td>
					    <td style="height:20px; "><asp:hyperlink id="lnkForgotPwd" runat="server"   NavigateUrl="forgotPwd.aspx" >Forgot your password?</asp:hyperlink></td>
				    </tr>
                    <%--Zulham 15112018--%>
				    <tr style="display:none">
					    <td style="height:20px;"></td>
					    <td style="height:20px;"><a target="_self" href="forgotPwd.aspx">Unlock your account</a></td>
				    </tr>
                    <%--Zulham 18102018 - PAMB--%>
				    <tr style="display:none">
                        <td style="height:20px;" align="right"><div> <% Response.Write(sNew)%></div> </td>
					    <td style="height:20px;"><asp:hyperlink Visible="true" id="Hyperlink5" runat="server" NavigateUrl="https://docs.google.com/spreadsheet/viewform?formkey=dHdKZGJjaXdyR0lDd014cm9Qa0hWZHc6MA">Sign Up Vendor SaaS</asp:hyperlink></td>
				    </tr>
				    <tr style="display:none">
                        <td style="height:20px;" align="right"><div> <% Response.Write(sNew)%></div> </td>
					    <td style="height:20px;"><asp:hyperlink Visible="true" id="Hyperlink6" runat="server" NavigateUrl="https://docs.google.com/spreadsheet/viewform?formkey=dFJ1ek0tODAwOFZPRHFxRDFjX0dWbFE6MQ">Sign Up Buyer SMB SaaS</asp:hyperlink></td>
				    </tr>
				    <tr>
					    <td style="height:10px;"></td>
					    <td style="height:10px;"><asp:hyperlink Visible="false" id="lnkpubNotice" runat="server" NavigateUrl="#">Public RFP Notice Board</asp:hyperlink></td>
				    </tr>
				    <tr style="display:none">
					    <td style="height:20px;"></td>
					    <td style="height:20px;"><%--<a target="_blank" visible="false" href="#">Vendor Registration</a>--%></td>
				    </tr>
				    <tr>
					    <td></td>
					    <td><asp:label id="lblMsg" Runat="server" Font-Size="10px" Font-Bold="True" ForeColor="red"></asp:label></td>
					  <input class="button" id="hidBtnContinue" type="button" value="hidBtnContinue" name="hidBtnContinue" runat="server" style=" display :none" />
					  <asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none" onclick="btnHidden1_Click"></asp:button> 
				    </tr>
			    </table>
            </div> 
            <div style="float:left; margin: 0px 20px;">
                <div><% Response.Write(sShakeHand)%></div>
            </div> 
            <div style="float:left; font-size:30px; text-align:center; margin-right:10px; width:330px;">
                <div style="margin-top:20px; font-weight:bold; color:#333399; ">Sourcing-Procurement System</div>
                <div style="margin-top:30px; color:#424dce; font-size:22px; ">Buying and Selling</div>
                <div style="color:#424dce; font-size:22px; ">Begins At A Single Point</div>
                <div style="margin-top:20px;"><asp:label id="lblMsg2" Runat="server" style="color:#424dce;" Font-Size="14px" Font-Bold="False" ForeColor="red"></asp:label></div> 
            </div> 
            <div style="clear:both; "></div> 
        </div>
            <div style="float:left; font-weight:bold; font-size:6px; margin-left:10px; width:700px;">
                <table cellspacing="0" cellpadding="0" border="0">
                    <tr>
					    <td style="width:100px; height: 10px;"></td>
					    <td style="height: 10px"><font color="red"><u>IMPORTANT NOTES:</u></font></td>
				    </tr>
				    <tr>
					    <td style="height: 10px"></td>
					    <td style="height: 10px"><font color="#111">a) This website used Microsoft Internet Explorer version 8 and above.</font></td>
				    </tr>
				    <tr>
					    <td  style="height: 10px"></td>
                        <%--Zulham 27112018--%>
					    <td style="height: 10px"><font color="#111">b) Set the Internet Browser's Compatibility Settings for this website (pru2pay.com.my).</font></td>
				    </tr>
				    <tr>
					    <td style="height: 10px"></td>
					    <td style="height: 10px"><font color="#111">c) Ensure that Active-X is enabled in the Internet Explorer Security settings.</font></td>
				    </tr>
				    <tr>
					    <td style="height: 12px"></td>
					    <td style="height: 12px"><font color="#111">d) Ensure that Pop-up Blocker is turn off.</font></td>
				    </tr>
				    <tr>
					    <td style="height: 10px;"></td>
					    <td style="height: 10px"></td>
				    </tr>             
                </table>
            </div>
        <div style="padding:0px 10px; clear:both; font-weight:bold; font-size:13px; ">
            <table style="width:100%; border: 1px solid #aabdd0; text-align:center;">
            <% Response.Write(Session("partnerLogo"))%>
                <!--<tr>
                    <td style="width:33%; height: 100px; padding: 0px 40px;">
                    <div class="download_ext" onclick="window.open('http://www.strateqgrp.com','FTN','menubar=yes,toolbar=yes,resizeable=yes,resizable=1, scrollbars=1, width=1000, height=500')"><% Response.Write(sStrateq_Login)%></div> 
                  </td>
                   <td style="width:33%; height: 100px; padding: 0px 40px;">
                        <div>Best view with Microsoft IE 8.0</div>
                        <div class="download_ext" onclick="window.open('http://www.microsoft.com/windows/internet-explorer/worldwide-sites.aspx','FTN','menubar=yes,toolbar=yes,resizeable=yes,resizable=1, scrollbars=1, width=1000, height=500')"><% Response.Write(sGetIE8)%></div>
                    </td>
                    <td style="width:33%; height: 100px; padding: 0px 40px;">
                        <div>To view our reports, get Adobe Reader</div>
                        <div class="download_ext" onclick="window.open('http://get.adobe.com/uk/reader/','FTN','menubar=yes,toolbar=yes,resizeable=yes,resizable=1, scrollbars=1, width=1000, height=500')"><% Response.Write(sGetAdobe)%></div>
                    </td>
                    <td style="width:33%; height: 100px; padding: 0px 40px;">
                        <div>About SSL Certificate</div>
                        <div class="download_ext"><% Response.Write(sTrustSeal)%></div>
                    </td>  
                </tr>-->
            </table>
        </div>
        <div style="clear:both; "></div> 
        <%--Zulham 18102018 - PAMB--%>
        <div style="padding:2px 20px; clear:both; font-weight:bold; font-size:12px; text-align:center; color:#111;display:none">
            <asp:hyperlink id="Hyperlink1" runat="server"></asp:hyperlink>
            <span>|</span>
            <asp:hyperlink id="Hyperlink2" runat="server">Contact Us</asp:hyperlink>
            <span>|</span>
            <asp:hyperlink id="Hyperlink3" navigateurl="http://www.microsoft.com/windows/internet-explorer/worldwide-sites.aspx" runat="server">Download IE8</asp:hyperlink>
            <asp:hyperlink id="Hyperlink7" runat="server">Enterprise SaaS</asp:hyperlink>
            <span>|</span>
            <asp:hyperlink id="Hyperlink4" navigateurl="http://get.adobe.com/uk/reader/" runat="server">Download Adobe Reader</asp:hyperlink>
            <asp:hyperlink id="Hyperlink8" runat="server">SMB SaaS</asp:hyperlink>
            <span id="span1" runat="server">|</span>
            <asp:hyperlink id="Hyperlink9" runat="server">Billing System</asp:hyperlink>
        </div>
        <div style="padding:4px 20px; clear:both; font-weight:bold; font-size:12px; text-align:center; font-weight:bold; color:#111;">
            <span><% Response.Write(Session("bottomText"))%></span>
        </div>
    </div>
    </form>
</body>
</html>