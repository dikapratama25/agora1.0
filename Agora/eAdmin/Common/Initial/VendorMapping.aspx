<%@ Page Language="vb" AutoEventWireup="false" Codebehind="VendorMapping.aspx.vb" Inherits="eAdmin.VendorMapping"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>VendorMapping</title>
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
		
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header"><FONT size="1">&nbsp;</FONT>Vendor Mapping</TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:label id="lbl_result" runat="server" ForeColor="Red"></asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_VendorMap" runat="server" DataKeyField="CM_COY_ID" AllowSorting="True" OnSortCommand="SortCommand_Click"
							AutoGenerateColumns="False" OnPageIndexChanged="dtg_VendorMap_PageIndexChanged">
							<Columns>
								<asp:BoundColumn DataField="CM_COY_ID" SortExpression="CM_COY_ID" ReadOnly="True" HeaderText="Company ID">
									<HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Company Name">
									<HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="VM_VENDOR_MAPPING" HeaderText="External Vendor Code">
									<HeaderStyle HorizontalAlign="Left" Width="50%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_code" runat="server" CssClass="txtbox" Width="150px"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<tr>
					<TD><asp:button id="Button1" runat="server" Text="Save" CssClass="button"></asp:button>&nbsp;
					<INPUT class="button" id="Button2" type="button" runat="server" value="Reset" onclick="ValidatorReset();">
					</TD>
</tr>
			</TABLE>
		</form>
	</body>
</HTML>
