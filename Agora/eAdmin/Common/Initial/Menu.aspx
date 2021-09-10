<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Menu.aspx.vb" Inherits="eAdmin.Menu" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
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
            Dim CSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "AutoComplete.css") & """ rel='stylesheet'>"
            Dim sBack As String = dDispatcher.direct("Plugins/Images", "m_back.jpg")
            Dim sNext As String = dDispatcher.direct("Plugins/Images", "m_next.jpg")
            Dim typeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx")
            Dim sStrateg_Login = "<img src=""" & dDispatcher.direct("Plugins/Images", "Powered by Strateq (after).jpg") & """ />"
            'm_back
            '<LINK href="css/Styles.css" type="text/css" rel="stylesheet">
        </script>
        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>	
		<% Response.Write(sCSS)%> 
		<% Response.Write(CSS)%> 
		<% Response.Write(Session("AutoComplete")) %>

		<script language="javascript">
		<!--
		$(document).ready(function(){
            $("#txtCompany").autocomplete("<% response.write(typeahead) %>");
            
            
            }).result(function(event, item) {
  			    $("#btnSearch").trigger('click');
			});
            
            
            
			
//			function Display()
//			{
//				var tree = document.getElementById("tree");
//				var t = document.getElementById("label");
//				var c = document.getElementById("Comp");
//				
//				var image = document.getElementById("imgMenu");
//				//var arrow = document.getElementById("arrow1");
//				if(tree.style.display == "none"){
//					tree.style.display ="";
//					t.style.display ="";
//					c.style.display ="";
//					parent.second.cols = "265,*";
//					image.lowsrc = 'images/i_collapse.gif';
//					image.src = 'images/i_collapse.gif';
//					
//						
//				}
//				else{		
//					parent.menu.scrolling="no";
//					tree.style.display ="none";
//					t.style.display ="none";
//					c.style.display ="none";
//					parent.second.cols = "50,*";
//					image.lowsrc = 'images/i_expand.gif';
//					image.src = 'images/i_expand.gif';
//					//lbl1.visible = "true";	
//				}
//				//image.style.display = "";
//				//arrow.style.display = "none";
//			}
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
					image.lowsrc = '<% Response.Write(sBack) %>' ;
					image.src = '<% Response.Write(sBack) %>' ;			
					lblIcon.style.display ="";	
					trhid.style.display="";
				}
				else{		
					parent.menu.scrolling="no";
					menu_box.style.display ="none";
					c.style.display ="none";
					//pnl.style.display ="none";
					parent.second.cols = "56,*";
					image.lowsrc = '<% Response.Write(sNext) %>';
					image.src = '<% Response.Write(sNext) %>';
					lblIcon.style.display ="none";	
					trhid.style.display="none";
				}
			}
			
			function showHide(lnkmenu, lnkdesc)
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
			<TABLE id="Table1" style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px"
				height="100%" cellSpacing="0" cellPadding="0" width="100%" bgColor="#f7fafe" runat="server">
				<tr height="1">
					<td vAlign="top" align="left"><%--<IMG id="imgMenu" style="CURSOR: hand" alt="Click here to expand the menu" src="images/i_collapse.gif"
							lowsrc="images/i_collapse.gif" border="0" runat="server">&nbsp;--%>
							
						<table border="0" cellpadding="0" cellspacing="0" width="100%" runat="server" id="tblIcon">
							<tr>
								<td><IMG class="menu_icon" id="imgMenu" style="CURSOR: hand; width:24px; height:24px; float:left;" alt="Click here to expand the menu" src="#"
										lowsrc="#"  border="0" runat="server"><asp:label id="lblIcon" Font-Size="11px" Runat="server" Font-Bold="True" Font-Name="verdana"></asp:label></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr height="5">
					<td colSpan="1">
						<div id="Comp">
							<TABLE width="100%">
								<TR>
									<td colSpan="2"><asp:label id="Label1" text="Company :" CssClass="lblname" Runat="server"></asp:label></td>
								</tr>
								<TR>
									<td colSpan="2">
									<%--<asp:dropdownlist id="cboComp" runat="server" CssClass="ddl" AutoPostBack="True" Width="90%"></asp:dropdownlist>--%>
									<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
									<asp:TextBox ID="txtCompany" runat="server"></asp:TextBox>
									<%--<input type="text" id="txtCompany1" value="" />--%>
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" style="display:none;"/>
                                        &nbsp;
                                    <%--<cc1:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtCompany" ServiceMethod="GetCompany" ServicePath="../AutoComplete/AutoComplete.asmx" MinimumPrefixLength="0" CompletionSetCount="10"></cc1:AutoCompleteExtender>--%></td>
									</TR>
									<tr>
									<td colSpan="2"><asp:button id="cmd_New" runat="server" Text="New Company" Width="100px" Height="25px" ></asp:button></td>
								</tr>
							</TABLE>
						</div>
					</td>
				</tr>
				<tr style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px">
					<td style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px"
						vAlign="top"><%  Response.Write(Session("w_menu"))%>
						<%--<div id="label" runat="server">
							<asp:label id="lbl1" Font-Name="verdana" Font-Bold="True" Runat="server" Font-Size="11px" ForeColor="white"></asp:label></div>
						--%><asp:panel id="Panel1" runat="server" Height="100%">
							<div id="tree" runat="server" style="display:none;">
								<IEWC:TREEVIEW id="TreeView1" runat="server" backcolor="#FFF8DD" height="100%" selectexpands="True"
									selectedstyle="color:#FFF8DD;background-color:black;" hoverstyle="color:Black;background-color:#cccccc;"
									indent="0" defaultstyle="display:none; font-family:Verdana;fore-color:black;font-size:11px;" bordercolor="Black"
									width="100%">
								</IEWC:TREEVIEW></div>
						</asp:panel></td>
				</tr>
				<TR id="trhid" style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 10px">
					<td style="height: 30px"><%  Response.Write(sStrateg_Login)%>
					</TD>
				
				</TR>
			</TABLE>
		</FORM>
	</BODY>
</HTML>
