<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Add_SalesTurnOver.aspx.vb" Inherits="eProcure.Add_SalesTurnOver" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
    <title>Add Sales TurnOver</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		function Close()
			{
	            window.close();
	         }
	         -->
		</script>
</head>
<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="tableheader" colspan="3" >&nbsp;Add Sales TurnOver :</TD>
				</TR>
				<tr class="tablecol" width="100%">
				<td class="tablecol" width="15%">
                    <strong>&nbsp;Year :<asp:Label ID="Label1" runat="server" Text="*" CssClass="errormsg"></asp:Label></strong></TD>
				<td class="TableInput" width="20%">
                    <asp:DropDownList ID="ddlYear" CssClass="ddl" runat="server">
                    </asp:DropDownList></td>
                    <td  width="60%">
                        <asp:RequiredFieldValidator ID="reqYear" runat="server" ControlToValidate="ddlYear"
                            Display="None" EnableClientScript="False" ErrorMessage="Required year."></asp:RequiredFieldValidator></td>
				</tr>
				<tr class="tablecol" >
				<td class="tablecol" >
                     <strong>&nbsp;<asp:Label ID="Label13" runat="server" Text="Currency :"></asp:Label></strong></TD>
				<td class="tablecol">
                    <asp:DropDownList ID="ddlCurrency" CssClass="ddl" width="100%" runat="server">
                    </asp:DropDownList></td>
                    <td ></td>
				</tr>
				<tr class="tablecol" >
				<td class="tablecol" >
                     <strong>&nbsp;Amount :<asp:Label ID="Label3" runat="server" Text="*" CssClass="errormsg"></asp:Label></strong></TD>
				<td class="TableInput" >
                    <asp:TextBox ID="txtAmount" runat="server" width="100%" CssClass="txtbox" MaxLength="30"></asp:TextBox></TD> 
                             <td>eg. 12500.50
                        <asp:RegularExpressionValidator ID="revAmount" runat="server" ControlToValidate="txtAmount"
                            Display="None" EnableClientScript="False" ErrorMessage="Invalid Amount" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="reqAmount" runat="server" ControlToValidate="txtAmount"
                            Display="None" EnableClientScript="False" ErrorMessage="Required Amount."></asp:RequiredFieldValidator></td>
				</tr>
										</TABLE>

			<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD style="HEIGHT: 19px"><asp:button id="cmd_Save" runat="server" CssClass="button" Text="Save"></asp:button>
                        <asp:Button ID="btnclose" runat="server" OnClientClick="Close()" Text="Close" Style="display: none" />&nbsp;<br />
                        <asp:ValidationSummary ID="vldSum" runat="server" CssClass="errormsg" />
                    </TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
