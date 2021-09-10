<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DisplayAddedItem.aspx.vb" Inherits="eProcure.DisplayAddedItem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Add/Modify Department</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
	</HEAD>
		<script language=javascript>
<!--
<!--
		<!-- Begin
		function right(e) {
		if (navigator.appName == 'Netscape' && (e.which == 3 || e.which == 2))
			return false;
		else if (navigator.appName == 'Microsoft Internet Explorer' && (event.button == 2 || event.button == 3)) {
			alert("Sorry, you do not have permission to right click.");
			return false;
		}
			return true;
		}
			document.onmousedown=right;
			if (document.layers) window.captureEvents(Event.MOUSEDOWN);
			window.onmousedown=right;	
//-->
</script>

	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="712" border="0">
				<TR>
					<TD class="header"><asp:label id="lblHeader" runat="server">Add Items to Shopping Cart</asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tableheader">&nbsp;<asp:label id="lblOKHeader" runat="server">Items successfully added to shopping cart</asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol"><asp:Label ID="lblOK" Runat="server">Test</asp:Label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tableheader">&nbsp;<asp:label id="lblDupHeader" runat="server">Items already exist in shopping cart</asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol"><asp:Label ID="lblDup" Runat="server">Test</asp:Label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tableheader">&nbsp;<asp:label id="lblNoPriceHdr" runat="server">Items without price are not allowed </asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol"><asp:Label ID="lblNoPrice" Runat="server">Test</asp:Label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<asp:HyperLink Runat="server" ID="lnkHere" NavigateUrl="#">
						Click here
						</asp:HyperLink><asp:label id="lblHere" runat="server">to view your Shopping Cart.</asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<asp:HyperLink Runat="server" ID="lnkBack" NavigateUrl="#">
							<STRONG>&lt; Back</STRONG></asp:HyperLink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
