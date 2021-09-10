<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="GRNMsg.aspx.vb" Inherits="eProcure.GRNMsg" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>GRNMsg</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div runat="server" id="Pass" style="DISPLAY:none">
				<TABLE class="alltable" id="tblDOHeader" cellSpacing="0" cellPadding="0" border="0">
					<TR>
						<TD class="header"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></TD>
					</TR>
					<TR>
						<TD class="emptycol"></TD>
					</TR>
					<tr>
						<td class="tableheader">&nbsp;<asp:Label id="lblHeader" runat="server"></asp:Label>
						</td>
					</tr>
					<Tr>
						<td class="tablecol" align="left" width="20%" style="HEIGHT: 18px">&nbsp;GRN Number
							<asp:Label id="lblGRNNum" runat="server"></asp:Label>&nbsp;has been 
							successfully
							<asp:Label id="lblLevel" runat="server" CssClass="label"></asp:Label>&nbsp;for 
							DO Number
							<asp:Label id="lblDONum" runat="server"></asp:Label>.
						</td>
					</Tr>
					<Tr>
						<td class="tablecol">
						</td>
					</Tr>
				</TABLE>
			</div>
			<div runat="server" id="Fail" style="DISPLAY:none">
				<TABLE class="alltable" id="tblDOHeader1" cellSpacing="0" cellPadding="0" border="0">
					<tr>
						<td class="tableheader">&nbsp;
							<asp:Label id="lblHD" runat="server">
								<strong></strong>
							</asp:Label>
						</td>
					</tr>
					<Tr>
						<td>
						</td>
					</Tr>
					<Tr>
						<td>
						</td>
					</Tr>
					<Tr>
						<td class="tablecol" align="left" width="20%">&nbsp;GRN already created for this 
							DO.</td>
					</Tr>
					<Tr>
						<td class="tablecol">
						</td>
					</Tr>
				</TABLE>
			</div>
			<table>
				<Tr>
					<td>
					</td>
				</Tr>
				<tr>
					<td><asp:HyperLink Runat="server" ID="lnkBack">
							<STRONG>&lt; Back</STRONG></asp:HyperLink>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
