<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ChgPassword.aspx.vb" Inherits="eAdmin.ChgPassword" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Change Password</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
        </script>
        <% Response.Write(sCSS)%>
        
	</HEAD>
	<body topMargin="10" MS_POSITIONING="GridLayout">
		<form id="form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" colspan="2"><FONT size="1">&nbsp;</FONT>Change Password
						<asp:label id="lblMsg" Font-Names="arial" Runat="server" Font-Size="X-Small"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="2"></TD>
				</TR>
				<TR>
					<TD class="tableheader" colSpan="2">&nbsp;User Identification</TD>
				</TR>
				<tr>
					<TD class="tablecol" width="20%"><STRONG>&nbsp;User ID</STRONG><span class="errormsg">*</span>&nbsp;:</TD>
					<TD class="tableinput">&nbsp;
						<asp:textbox id="txtUserID" runat="server" CssClass="txtbox" Width="160px" Enabled="False"></asp:textbox></TD>
				</tr>
				<tr>
					<TD class="tablecol"><STRONG>&nbsp;Old Password</STRONG><span class="errormsg">*</span>&nbsp;:</TD>
					<TD class="tableinput">&nbsp;
						<asp:textbox id="txtOldPW" runat="server" CssClass="txtbox" Width="160px" TextMode="Password"
							MaxLength="20"></asp:textbox>&nbsp;</TD>
				</tr>
				<tr>
					<TD class="tablecol"><STRONG>&nbsp;New Password</STRONG><span class="errormsg">*</span>&nbsp;:</TD>
					<TD class="tableinput">&nbsp;
						<asp:textbox id="txtNewPW" runat="server" CssClass="txtbox" Width="160px" TextMode="Password"
							MaxLength="20"></asp:textbox>&nbsp;</TD>
				</tr>
				<tr>
					<TD class="tablecol"><STRONG>&nbsp;Confirm New Password</STRONG><span class="errormsg">*</span>&nbsp;:</TD>
					<TD class="tableinput">&nbsp;
						<asp:textbox id="txtConfNPW" runat="server" CssClass="txtbox" Width="160px" TextMode="Password"
							MaxLength="20"></asp:textbox>&nbsp;</TD>
				</tr>
			</TABLE>
			<div id="tblQA" style="DISPLAY: none" runat="server"><br>
				<TABLE class="alltable" cellSpacing="0" cellPadding="0" width="100%" border="0">
					<TR>
						<TD class="tableheader" colSpan="2">&nbsp;Authentication Q&amp;A</TD>
					</TR>
					<tr class="tablecol">
						<TD class="tablecol" width="20%"><STRONG>&nbsp;Challenge Phrase</STRONG><span class="errormsg">*</span>&nbsp;:</TD>
						<TD class="tableinput">&nbsp;
							<asp:dropdownlist id="cboQuestion" runat="server" CssClass="ddl" Width="250px"></asp:dropdownlist></TD>
					</tr>
					<tr class="tablecol">
						<TD class="tablecol"><STRONG>&nbsp;Answer</STRONG><span class="errormsg">*</span>&nbsp;:</TD>
						<TD class="tableinput">&nbsp;
							<asp:textbox id="txtAns" runat="server" CssClass="txtbox" Width="250px" MaxLength="150"></asp:textbox></TD>
					</tr>
				</TABLE>
			</div>
			<TABLE class="alltable" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR class="emptycol">
					<TD style="HEIGHT: 7px"><span class="errormsg">*</span>&nbsp;indicates required 
						field</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD align="left"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save" CausesValidation="false"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" type="reset" value="Clear" name="cmdReset" runat="server">&nbsp;
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:label id="lblMsg2" Runat="server" CssClass="errormsg"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:label id="lblLogin" Runat="server" CssClass="lblInfo"></asp:label></TD>
				</TR>
			</TABLE>
			<br>
		</form>
	</body>
</HTML>
