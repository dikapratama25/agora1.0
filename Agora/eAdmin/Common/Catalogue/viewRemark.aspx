<%@ Page Language="vb" AutoEventWireup="false" Codebehind="viewRemark.aspx.vb" Inherits="eAdmin.viewRemark"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>View Remark</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
       <script runat="server">
           Dim dDispatcher As New AgoraLegacy.dispatcher
           Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
       </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		
			
		-->
		</script>
	</HEAD>
	<body onload="" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header" colSpan="4"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<TR>
					<TD class="TableHeader" colSpan="4">&nbsp;Contract&nbsp;Group Item</TD>
				</TR>
				<TR id="trCodeRead" vAlign="top" runat="server">
					<TD class="tablecol" noWrap width="15%">&nbsp;<STRONG>Contract Ref. No.</STRONG>&nbsp;:</TD>
					<TD class="TableInput" width="35%" noWrap><asp:label id="lblCode" runat="server"></asp:label></TD>
					<TD class="tablecol" width="15%" noWrap>&nbsp;<STRONG>Item Code</STRONG>&nbsp;:</TD>
					<TD class="TableInput" width="35%" noWrap><asp:label id="lblItem" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<tr>
					<td colSpan="4"><asp:datagrid id="dtgCatalogue" runat="server" AutoGenerateColumns="false" Width="100%" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:BoundColumn DataField="CDUR_Created_DateTime" SortExpression="CDUR_Created_DateTime" ReadOnly="True"
									HeaderText="Date">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" ReadOnly="True" HeaderText="User Name">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDUR_User_Role" SortExpression="CDUR_User_Role" ReadOnly="True" HeaderText="Role">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDUR_Remark" SortExpression="CDUR_Remark" ReadOnly="True" HeaderText="Remarks">
									<HeaderStyle Width="45%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<TR>
					<TD colSpan="4">&nbsp;</TD>
				</TR>
				<TR>
					<TD colSpan="4"><INPUT class="button" id="cmdClose" onclick="window.close()" type="button" value="Close"
							name="cmdClose"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
