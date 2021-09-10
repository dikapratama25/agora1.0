<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PolicySetup.aspx.vb" Inherits="eAdmin.PolicySetup" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PolicySetup</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
				
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
        </script>
        <% Response.Write(sCSS)%>
        <% Response.Write(Session("WheelScript"))%>
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" width="100%">
				<TR>
					<TD class="header" colSpan="2"><FONT size="3">Security Policy Maintenance</FONT></TD>
				</TR>
				<tr class="emptycol">
					<td colSpan="2">&nbsp;</td>
				</tr>
				<tr class="tablecol">
					<td>
						<asp:datagrid id="dgPolicy" Runat="server">
							<HeaderStyle Height="22px"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="LP_AUTO_NO" SortExpression="LP_AUTO_NO" ReadOnly="True" HeaderText=" No ">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="LP_PARAM" SortExpression="LP_PARAM" ReadOnly="True" HeaderText="Policy">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="LP_PARAM_DESC" SortExpression="LP_PARAM_DESC" ReadOnly="True" HeaderText="Description">
									<HeaderStyle Width="45%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="LP_PARAM_IND" SortExpression="LP_PARAM_IND" ReadOnly="True"
									HeaderText="Ind"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="LP_VALUE" SortExpression="LP_VALUE" ReadOnly="True" HeaderText="Value"></asp:BoundColumn>
								<asp:TemplateColumn SortExpression="LP_PARAM_IND" HeaderText="Ind.">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemTemplate>
										<asp:DropDownList Runat="server" ID="ddl" CssClass="ddl" Width="50px">
											<asp:ListItem>YES</asp:ListItem>
											<asp:ListItem>NO</asp:ListItem>
										</asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="LP_VALUE" HeaderText="Value">
									<HeaderStyle Width="12%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox runat="server" ID="txtValue" CssClass="txtbox" Width="120px"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid>
					</td>
				<TR>
					<TD class="emptycol">&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" type="reset" value="Reset" name="cmdReset" runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:label id="lbl_result" runat="server" ForeColor="Red"></asp:label></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
