<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ExceedBCM.aspx.vb" Inherits="eProcure.ExceedBCMSEH" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Approval Setup</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<% Response.Write(Session("WheelScript"))%>
	</HEAD>
	<body onload="" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" colSpan="2"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2" style="color:Black "><asp:Label id="lblRemarkHeader" runat="server"></asp:Label></TD>
				</TR>
				<tr>
					<TD class="linespacing1" colSpan="7"></TD>
			    </TR>
				<TR>
					<TD class="TableHeader" colSpan="2">&nbsp;Request Budget Top-up</TD>
				</TR>
				<TR vAlign="top" runat="server" id="trApp">
					<TD class="tablecol" noWrap width="20%">&nbsp;<STRONG>To Approving Officer</STRONG>&nbsp;:</TD>
					<TD class="TableInput" width="80%"><asp:dropdownlist id="cboApproval" runat="server" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol">&nbsp;<STRONG>Department Name</STRONG>&nbsp;:</TD>
					<TD class="TableInput"><asp:label id="lblDeptName" runat="server" Width="290px"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol">&nbsp;<STRONG>Buyer Name</STRONG>&nbsp;:</TD>
					<TD class="TableInput"><asp:label id="lblBuyerName" runat="server" Width="290px"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol">&nbsp;<STRONG>Buyer ID</STRONG>&nbsp;:</TD>
					<TD class="TableInput"><asp:label id="lblBuyerID" runat="server" Width="290px"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol">&nbsp;<STRONG>Buyer Email</STRONG>&nbsp;:</TD>
					<TD class="TableInput"><asp:label id="lblBuyerEmail" runat="server" Width="290px"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol" noWrap>&nbsp;<STRONG>Current Account(s) Exceed</STRONG>&nbsp;:</TD>
					<TD class="TableInput"><asp:label id="lblAccount" runat="server" Width="100%"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol">&nbsp;<STRONG>Remarks</STRONG>&nbsp;:</TD>
					<TD class="TableInput"><asp:textbox id="txtRemark" runat="server" CssClass="listtxtbox" Width="300px" TextMode="MultiLine"
							MaxLength="1000"></asp:textbox>
						<asp:Label id="lblRemark" runat="server"></asp:Label><asp:button id="cmdSubmit" runat="server" CssClass="Button" Text="Submit"></asp:button></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
