<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MenuAddNode.aspx.vb" Inherits="eAdmin.MenuAddNode" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Add New Node</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>        
        <% Response.Write(css)%>
        <% Response.Write(Session("WheelScript"))%>
        
	</HEAD>
	<BODY MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%Response.Write(Session("MenuAdd_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0">
				<TR>
					<TD class="header">Menu Node&nbsp;Maintenance</TD>
				</TR>
				<TR>
					<TD align="center">
						<div align="left"><asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text="Step 1: Add a new module.<br><b>=></b>Step 2: Add a new node under a module.<br>Step 3: Assign menu access right to a fixed role."
						></asp:label>
                        </div>
					</TD>
				</TR>
				<table>
				<br>
				
				<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="tableheader" colSpan="3">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92" colSpan="3"></TD>
				</TR>
				
				<tr class="tablecol">
					<TD width="160"><STRONG>&nbsp;Menu&nbsp;ID</STRONG><span class="errorMsg">*</span>&nbsp; 
						:</TD>
					<TD width="160"><asp:textbox id="txtMenuId" runat="server" CssClass="txtbox" MaxLength="30"
							Width="160px" ></asp:textbox></TD>
					<TD class="tablecol"><asp:requiredfieldvalidator id="rfv_txtMenuId" runat="server" ControlToValidate="txtMenuId" ErrorMessage="Menu Id is required"
							Display="None"></asp:requiredfieldvalidator></TD>
				</tr>
				<tr class="tablecol">
					<td style="HEIGHT: 2px"><STRONG>&nbsp;Menu Name</STRONG><span class="errorMsg">*</span>&nbsp;:</td>
					<TD style="HEIGHT: 2px"><asp:textbox id="txtMenuName" runat="server" CssClass="txtbox" MaxLength="100" Width="160px"></asp:textbox><asp:textbox id="txOriUsrGrpName" style="DISPLAY: none" runat="server" Width="0"></asp:textbox></TD>
					<TD class="tablecol" style="HEIGHT: 2px"><asp:requiredfieldvalidator id="rfv_txtMenuName" runat="server" ControlToValidate="txtMenuName" ErrorMessage="Menu name is required"
							Display="None"></asp:requiredfieldvalidator></TD>
				</tr>
				<tr class="tablecol">
					<td style="HEIGHT: 2px"><STRONG>&nbsp;Menu Image</STRONG><span class="errorMsg"></span>&nbsp;:</td>
					<TD style="HEIGHT: 2px"><asp:textbox id="txtMenuImg" runat="server" CssClass="txtbox" MaxLength="100" Width="160px" ></asp:textbox>
					</TD>
					<TD></TD>
				</tr>
		        <%-->tr class="tablecol">
					<td width="160"><STRONG>&nbsp;Menu Parent</STRONG><span class="errorMsg" Width="160px" >*</span>&nbsp;:</td>
					<TD><asp:textbox id="txtMenuParent" CssClass="txtbox" Width="160" Runat="server">
						</asp:textbox></TD>
					<td><asp:requiredfieldvalidator id="rfv_txtMenuParent" runat="server" ControlToValidate="txtMenuParent" ErrorMessage="Menu Parent is Required"
							Display="None"></asp:requiredfieldvalidator></td>
				</tr--%>
				<tr class="tablecol">
						<td width="160"><STRONG>&nbsp;Menu Parent</STRONG><span class="errorMsg" Width="160px" >*</span>&nbsp;:</td>
						<td><asp:DropDownList id="ddlMenuParent" runat="server" CssClass = "ddl" Width="160px" autopostback=true></asp:DropDownList>&nbsp;</td>
		                <td><asp:requiredfieldvalidator id="rfv_ddlMenuParent" runat="server" ControlToValidate="ddlMenuParent" ErrorMessage="Menu Parent is Required"
							Display="None"></asp:requiredfieldvalidator></td>
								
				</TR>
				<TR class="tablecol">
					<TD width="160">&nbsp;<STRONG>Menu Level</STRONG><span class="errorMsg">*</span>&nbsp;:</TD>
					<TD><asp:textbox id="txtMenuLevel" runat="server" CssClass="txtbox" Width="160px" ></asp:textbox></TD>
					<td></td>
				</TR>
		
				<tr class="tablecol">
					<td height="23"><STRONG>&nbsp;Menu URL</STRONG><span class="errorMsg">*</span>&nbsp;:</td>
					<td><asp:textbox id="txtMenuURL" runat="server" CssClass="txtbox" MaxLength="100" Width="160px"></asp:textbox></td>
					<TD style="HEIGHT: 2px"><asp:requiredfieldvalidator id="rfv_txtMenuURL" runat="server" ControlToValidate="txtMenuURL" ErrorMessage="URL is required"
							Display="None"></asp:requiredfieldvalidator></TD>
					
				</tr>
				<TR class="tablecol">
					<TD style="WIDTH: 92px; HEIGHT: 6px" width="92" colSpan="3"></TD>
				</TR>
				<TR class="emptycol">
					<TD style="HEIGHT: 7px"><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</TD>
					<td style="HEIGHT: 7px" colSpan="3">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
				</TR>
			</TABLE>
			<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
			</TABLE>
			<div id="tblButton" runat="server">
				<table class="alltable" id="tbltable4" cellSpacing="0" cellPadding="0" border="0">
					<TR>
						<TD colSpan="2"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdGrant" runat="server" CssClass="button" Width="120" Text="Grant Access Rights"
								Visible="False"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" CausesValidation="False"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" type="button" value="Clear" name="cmdReset" runat="server"></TD>
					</TR>
				</table>
			</div>
			<table class="alltable" id="Table5" cellSpacing="0" cellPadding="0" border="0">
				
				<tr>
				</tr>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
			    <TR>
					<TD class="emptycol"><asp:validationsummary id="vldsumm" runat="server" CssClass="errormsg" Width="696px"></asp:validationsummary></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="#">
							<STRONG>&lt; Back</STRONG></asp:hyperlink>
						</TD>
				</TR>
			</table>
		</form>
	</BODY>
</HTML>
