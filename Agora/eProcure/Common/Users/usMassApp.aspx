<%@ Page Language="vb" AutoEventWireup="false" Codebehind="usMassApp.aspx.vb" Inherits="eProcure.usMassApp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>usMassApp</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../css/Styles.css" type="text/css" rel="stylesheet">
  </HEAD>
	<BODY topMargin="10">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" align="center" border="0">
				<TR>
					<TD class="header" colSpan="2"><FONT size="3">Mass Approval</FONT></TD>
				</TR>
				<tr>
					<td colSpan="2">&nbsp;</td>
				</tr>
				<TR>
					<TD class="emptycol" colspan="2">
						<TABLE class=alltable id=Table6 cellSpacing=0 cellPadding=0 border=0 >
                            <tbody>
                            </tbody>
                        </TABLE>
						<TR>
							<TD class="tableheader" colSpan=2>&nbsp;User Info
							</TD>
						</TR>
						<TR class="tablecol">
							<TD style="HEIGHT: 6px" colSpan="2"></TD>
						</TR>
						<TR class="tablecol">
							<TD width="15%">&nbsp;<STRONG>User&nbsp;ID</STRONG>&nbsp;:</TD>
							<td width="85%"><asp:label id="txtUserID" runat="server" CssClass="lblInfo"></asp:label></td>
						</TR>
						<TR class="tablecol">
							<TD>&nbsp;<STRONG>User&nbsp;Name</STRONG>&nbsp;:</TD>
							<td><asp:label id="txtUserName" runat="server" CssClass="lblInfo"></asp:label></td>
						</TR>
						<TR class="tablecol">
							<TD style="HEIGHT: 6px" colSpan="2"></TD>
						</TR>
						
						</TABLE>
<TABLE class=alltable id=Table2 cellSpacing=0 cellPadding=0 border=0 >
					
				<TR class="emptycol">
							<TD colSpan="2"></TD>
				</TR>
				<TR>
					<TD class="tableheader" colSpan="2">&nbsp;Mass Approval Rights</TD>
				</TR>
				<TR>
					<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92"></TD>
					<TD class="TableInput" style="HEIGHT: 6px"></TD>
				</TR>
				<tr class="tablecol">
					<TD width="160"><STRONG>&nbsp;Description:</STRONG>&nbsp; :</TD>
					<TD><asp:Label id=lblDescription runat="server">Mass Approval authorise approving officer to approve more than one PO or Invoice at a time.</asp:Label>
					</TD>
				</tr>
				<tr class="tablecol">
					<td><STRONG>&nbsp;Mass Approval</STRONG><span class="errorMsg">&nbsp;*</span>&nbsp;:</td>
					<TD><asp:checkbox id="chkMassApp" Text="PO Mass Approval" Runat="server"></asp:checkbox>
					&nbsp;<asp:checkbox id="chkInvMassApp" Text="Invoice Mass Approval" Runat="server"></asp:checkbox>
					</TD>
				</tr>
				<tr class="tablecol">
					<td colSpan="2">&nbsp;</td>
				</tr>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
						</TABLE>
			<table class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD colSpan="2"><asp:button id="cmdSave" runat="server" Text="Save" CssClass="button"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" type="reset" value="Reset" name="cmdReset" runat="server">
					<asp:Button ID="lnkBack" runat="server" Text="Close" CssClass="button" onclick="window.close();" /></TD>
				</TR>
				<TR>
					<TD><br>
						<asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
			</table>
		</form>
	</BODY>
</HTML>
