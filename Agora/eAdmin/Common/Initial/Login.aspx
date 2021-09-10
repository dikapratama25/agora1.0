<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="eAdmin.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Login eProcurement</title>
    
    
    <script runat="server">
        Dim dDispatcher As New AgoraLegacy.dispatcher
        Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
        'Zulham 24102018
        Dim sMyFairTradeNet_Login = "<img src=""" & dDispatcher.direct("Plugins/Images", "4255.jpg") & """ />"
        Dim sFTN_login = "<img src=""" & dDispatcher.direct("Plugins/Images", "FTN login shakehand.JPG") & """ />"
        Dim sStrateq_login = "<img src=""" & dDispatcher.direct("Plugins/Images", "Powered by Strateq (before).jpg") & """ />"
        Dim sIE8 = "<img src=""" & dDispatcher.direct("Plugins/Images", "IE8-DLnow.png") & """ alt=""Get Internet Explorer 8"" /></div>"
        Dim sadobe = "<img src=""" & dDispatcher.direct("Plugins/Images", "get_adobe_reader.png") & """ alt=""Get Adobe Reader"" /></div>"
        Dim strust = "<img src=""" & dDispatcher.direct("Plugins/Images", "compare-trust-seal.gif") & """ alt=""Validate Us"" /></div>"
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
    function SubmitForm(frmLogin)
    {
        frmLogin.submit(); return true;
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
  
        
    </style>
</head>
<body style="background: #fff; color:#666; font-family:Arial; ">
    <form id="frmLogin" runat="server">
    <div style="margin:0px auto; width:880px; border: 2px solid #26b0eb; padding:20px;">
        <div style="padding:2px 20px; border-bottom: 3px solid #aabdd0; height:80px; clear:both;">
            <div style="float:left; "><% Response.Write(sMyFairTradeNet_Login)%></div>
            <div style="float:right; font-size: 20px; margin-top:60px; color:#333399; ">
                Software-as-a-Service
            </div>
            <div style="clear:both; "></div> 
        </div>
        <div style="clear:both; "></div> 
        <div style="margin-top:20px;">
            <div style="float:left; font-weight:bold; font-size:11px; margin-left:10px; width:280px;">
                <table cellspacing="0" cellpadding="0" border="0">
				    <tr>
					    <td style="width:100px;">Company ID:</td>
					    <td style="width: 161px">&nbsp;<asp:textbox id="txtCompID" runat="server" MaxLength="20" Width="120px" onkeypress="return SubmitFormEnter(event, frmLogin);" Enabled="False">hub</asp:textbox></td>
				    </tr>
				    <tr>
					    <td style="width: 100px">User ID:</td>
					    <td style="width: 161px">&nbsp;<asp:textbox id="txtUserID" runat="server" MaxLength="20" Width="120px" onkeypress="return SubmitFormEnter(event, frmLogin);"></asp:textbox></td>
				    </tr>
				    <tr>
					    <td style="width: 100px">Password:</td>
					    <td style="width: 161px">&nbsp;<asp:textbox id="txtPassword" runat="server" MaxLength="10" Width="120px" TextMode="Password" onkeypress="return SubmitFormEnter(event, frmLogin);"></asp:textbox></td>
				    </tr>
				    <tr>
					    <td style="width: 100px"></td>
					    <td style="width: 161px"><asp:checkbox id="chkCookies" Text="Remember my ID" Runat="server"></asp:checkbox></td>
				    </tr>
				    <tr>
					    <td style="width: 100px"></td>
					    <td style="width: 161px">
						    <div class="btn" onclick="SubmitForm(frmLogin); return false; " id="DIV1">Log In</div>
						    <div class="btn" onclick="document.frmLogin.reset(); return false; ">Reset</div>
						</td>
				    </tr>
				    <tr><td style="height:20px; width: 100px;"></td></tr>
				    <tr>
					    <td style="height:20px; width: 100px;"></td>
					    <td style="height:20px; width: 161px;"><asp:hyperlink id="lnkForgotPwd" runat="server" NavigateUrl="forgotPwd.aspx" >Forgot your password?</asp:hyperlink></td>
				    </tr>
				    <tr>
					    <td style="height:20px; width: 100px;"></td>
					    <td style="height:20px; width: 161px;"><a target="_self" href="forgotPwd.aspx">Unlock your account</a></td>
				    </tr>
				    <tr>
					    <td style="height:20px; width: 100px;"></td>
					    <td style="height:20px; width: 161px;"><asp:hyperlink id="lnkpubNotice" runat="server" NavigateUrl="/eRFP/PublicRFP/NonRegPublicRFPNoticeBoard.aspx" Visible="False">Public RFP Notice Board</asp:hyperlink></td>
				    </tr>
				    <tr>
					    <td style="height:20px; width: 100px;"></td>
					    <td style="height:20px; width: 161px;"><%--<a target="_blank" href="/eRFP/PublicVendorReg/PublicVendorRegistration.aspx?mode=add">Vendor Registration</a>--%></td>
				    </tr>
				    <tr>
					    <td style="width: 100px"></td>
					    <td style="width: 161px"><asp:label id="lblMsg" Runat="server" Font-Size="10px" Font-Bold="True" ForeColor="red"></asp:label></td>
				    </tr>
			    </table>
            </div> 
            <div style="float:left; margin: 0px 20px;">
                <div><% Response.Write(sFTN_login)%></div>
            </div> 
            <div style="float:left; font-size:30px; text-align:center; margin-right:10px; width:330px;">
                <div style="margin-top:20px; font-weight:bold; color:#333399; ">Sourcing-Procurement System</div>
                <div style="margin-top:30px; color:#424dce; font-size:22px; ">Buying and Selling</div>
                <div style="color:#424dce; font-size:22px; ">Begins At A Single Point</div>
            </div> 
            <div style="clear:both; "></div> 
        </div>
        <div style="clear:both; "></div> 
        <div style="padding:0px 10px; clear:both; font-weight:bold; font-size:13px; ">
            <table style="width:100%; border: 1px solid #aabdd0; text-align:center;">
                <tr>
                    <td style="width:33%; height: 100px; padding: 0px 40px;">    
                    <div class="download_ext" onclick="window.open('http://www.strateqgrp.com','FTN','menubar=yes,toolbar=yes,resizeable=yes,resizable=1, scrollbars=1, width=1000, height=500')"><% Response.Write(sStrateq_Login)%></div> 
                    </td>
                    <td style="width:33%; height: 100px; padding: 0px 40px;">
                        <div>Best view with Microsoft IE 8.0</div>
                        <div class="download_ext" onclick="window.open('http://www.microsoft.com/windows/internet-explorer/worldwide-sites.aspx','FTN','menubar=yes,toolbar=yes,resizeable=yes,resizable=1, scrollbars=1, width=1000, height=500')"><% Response.Write(sIE8)%></div>
                    </td>
                    <td style="width:33%; height: 100px; padding: 0px 40px;">
                        <div>To view our reports, get Adobe Reader</div>
                        <div class="download_ext" onclick="window.open('http://get.adobe.com/uk/reader/','FTN','menubar=yes,toolbar=yes,resizeable=yes,resizable=1, scrollbars=1, width=1000, height=500')"><% Response.Write(sadobe)%></div>
                    </td>
       <!--             <td style="width:33%; height: 100px; padding: 0px 40px;">
                        <div>About SSL Certificate</div>
                        <div class="download_ext"><% Response.Write(strust)%></div>
                    </td> -->
                </tr>
            </table>
        </div>
        <div style="clear:both; "></div> 
        <%--'Zulham 24102018--%>
        <div style="padding:2px 20px; clear:both; font-weight:bold; font-size:12px; text-align:center; color:#111;display:none">
            <asp:hyperlink id="Hyperlink1" runat="server"   NavigateUrl="http://www.myfairtradenet.com" >myFairTradeNet</asp:hyperlink>
            <span>|</span>
            <asp:hyperlink id="Hyperlink2" runat="server"   NavigateUrl="http://www.myfairtradenet.com/web/contactus.html" >Contact Us</asp:hyperlink>
        </div>
        <div style="padding:4px 20px; clear:both; font-weight:bold; font-size:12px; text-align:center; font-weight:bold; color:#111;">
            <span>Strateq BusinessHub Sdn Bhd All Rights Reserved</span>
        </div>
                </div>

    </form>
</body>
</html>