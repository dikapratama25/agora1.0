<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DOMsgAdd.aspx.vb" Inherits="eProcure.DOMsgAdd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>DOMsgAdd</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
	<!--
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
	//-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div runat="server" id="Pass" style="DISPLAY:none">
				<TABLE class="alltable" id="tblDOHeader" cellSpacing="0" cellPadding="0" border="0">
					<TR>
						<TD class="header" ><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></TD>
					</TR>
					<TR>
						<TD class="emptycol" ></TD>
					</TR>
					<tr>
						<td class="tableheader">&nbsp;<asp:Label id="lblHeader" runat="server"></asp:Label>
						</td>
					</tr>
					<Tr>
						<td>
						</td>
					</Tr>
					<Tr>
						<td class="tablecol" align="left" width="20%" style="height: 19px">&nbsp;Delivery Order Number
							<asp:Label id="lblDONum" runat="server"></asp:Label>
							has been
							<asp:Label id="lblSts" runat="server"></asp:Label>.
						</td>
					</Tr>
					<Tr>
						<td class="emptycol">
						</td>
					</Tr>
					<TR>
						<TD class="emptycol"><asp:button id="cmdPreviewDO" runat="server" Text="View DO" CssClass="button"></asp:button>&nbsp;
						</TD>
					</TR>
				</TABLE>
			</div>
			<div runat="server" id="Fail" style="DISPLAY:none">
				<TABLE class="alltable" id="tblDOHeader1" cellSpacing="0" cellPadding="0" border="0">
					<tr>
						<td class="tableheader" style="height: 19px">&nbsp;
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
						<td class="tablecol" align="left" width="20%">&nbsp;
							<asp:Label ID="lblMsg" Runat="server">No outstanding items. Nothing has been modified.</asp:Label>
						</td>
					</Tr>
					<Tr>
						<td class="tablecol">
						</td>
					</Tr>
				</TABLE>
			</div>
			<table>
				<Tr>
					<td class="emptycol">
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
