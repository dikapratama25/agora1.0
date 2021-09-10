<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Menu.aspx.vb" Inherits="eProcure.Menu" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Menu</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sCSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"">"
		    
		    Dim sHide As String = dDispatcher.direct("Plugins/Images", "m_hide.jpg")
		    Dim sShow As String = dDispatcher.direct("Plugins/Images", "m_show.jpg")
 		    Dim sStrateg_Login = "<img src=""" & dDispatcher.direct("Plugins/Images", "Powered by Strateq (after).jpg") & """ />"
       </script>

		
		<% Response.Write(sCSS)%> 
		<script language="javascript">
		<!--
			
			function Display()
			{
				var tree = document.getElementById("tree");
				var menu_box = document.getElementById("menu_box");
				var c = document.getElementById("Comp");
				var pnl = document.getElementById("pnlIcon");
				var image = document.getElementById("imgMenu");
				var lblIcon = document.getElementById("lblIcon");
				var trhid = document.getElementById("trhid");
				
				if(menu_box.style.display == "none"){
					menu_box.style.display ="";
					c.style.display ="";
					//pnl.style.display ="";
					parent.second.cols = "200,*";
					image.lowsrc = '<% Response.Write(sHide) %>';
					image.src = '<% Response.Write(sHide) %>';			
					lblIcon.style.display ="";	
					trhid.style.display="";
				}
				else{		
					parent.menu.scrolling="no";
					menu_box.style.display ="none";
					c.style.display ="none";
					//pnl.style.display ="none";
					parent.second.cols = "56,*";
					image.lowsrc = '<% Response.Write(sShow) %>';
					image.src = '<% Response.Write(sShow) %>';
					lblIcon.style.display ="none";	
					trhid.style.display="none";
				}
			}
						
			function showHideMenu(lnkmenu, lnkdesc)
		    {
			    if (document.getElementById(lnkdesc).style.display == 'none')
			    {
				    document.getElementById(lnkdesc).style.display = '';
				    document.getElementById(lnkmenu).className = 'main_menu';
			    } 
			    else 
			    {
				    document.getElementById(lnkdesc).style.display = 'none';
				    document.getElementById(lnkmenu).className = 'main_menu_drop';
			    }
		    }
			//-->
		</script>
	</HEAD>
	<BODY style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px"
		bgColor="#f7fafe" leftMargin="0" topMargin="0" ms_positioning="GridLayout">
		<FORM id="Form1" style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px"
			method="post" runat="server">
	<INPUT id="hidControl" style="WIDTH: 48px; HEIGHT: 22px" type="hidden" size="2" name="hidControl"
				runat="server">
			<TABLE id="Table1" style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px"
				height="100%" cellSpacing="0" cellPadding="0" width="100%" bgColor="#f7fafe" runat="server">
				<tr height="1">
					<td vAlign="top" align="left">
						<table border="0" cellpadding="0" cellspacing="0" width="100%" runat="server" id="tblIcon">
							<tr>
								<td style="height: 26px"><IMG class="menu_icon" id="imgMenu" style="CURSOR: hand; width:24px; height:24px; " alt="Click here to expand the menu" src="#"
										lowsrc="#"  border="0" runat="server"><asp:label id="lblIcon" Font-Size="11px" Runat="server" Font-Bold="True" Font-Name="verdana"></asp:label></td>
							</tr>
						</table>
					</td>
				</tr>
			    <!--<tr height="5">
					<td colSpan="1">
						<DIV id="Comp" style="DISPLAY: none">
							<TABLE width="100%">
								<TR>
									<TD colSpan="2"><asp:label id="Label1" text="Company :" CssClass="lblname" Runat="server" visible=false></asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="2"><asp:dropdownlist id="cboComp" runat="server" CssClass="ddl" AutoPostBack="True" Width="90%" visible=false></asp:dropdownlist></TD>
								</TR>
							</TABLE>
						</DIV>
					</td>
				</tr>-->
				<TR style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px">
					<TD style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px"
						vAlign="top"><%  Response.Write(Session("w_menu"))%>
						<asp:panel id="Panel1" runat="server" Height="100%">
							<DIV id="tree" runat="server" style="display:none;">
								<IEWC:TREEVIEW id="TreeView1" runat="server" height="100%" backcolor="#FFF8DD" selectexpands="True"
									selectedstyle="color:#FFF8DD;background-color:black;" hoverstyle="color:Black;background-color:#cccccc;"
									indent="0" defaultstyle="display:none; font-family:Verdana;fore-color:black;font-size:11px;" bordercolor="Black"
									width="100%"></IEWC:TREEVIEW></DIV>
						</asp:panel></TD>				
				</TR>				
	<tr>
        <td style="height:20px;">&nbsp;&nbsp;&nbsp;<asp:linkbutton id="lbRN"  Font-Italic="true" Runat="server"></asp:linkbutton></td>
	</tr>			
	<TR id="trhid" style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 10px">
					<td style="height: 30px"><%  Response.Write(sStrateg_Login)%>
					</TD>
				
				</TR>
			</TABLE>
		</FORM>
	</BODY>
</HTML>
