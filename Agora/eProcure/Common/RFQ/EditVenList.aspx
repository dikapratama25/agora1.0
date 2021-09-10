<%@ Page Language="vb" AutoEventWireup="false" Codebehind="EditVenList.aspx.vb" Inherits="eProcure.EditVenList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>EditVenList</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--

			function selectAll()
			{
				SelectAllG("dtg_vendor_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtg_vendor_ctl02_chkAll","chkSelection");
			}
			
			function check(){
			var change = document.getElementById("onchange");
			change.value ="1";
			}
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD>
						<TABLE id="Table2" cellSpacing="0" cellPadding="2" width="100%" border="0">
							<TR>
								<TD class="header"><A></A>Vendor Companies
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="tableheader">&nbsp;RFQ Info</TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<asp:label id="lbl_List_Name" runat="server"></asp:label>:<asp:label id="lbl_name" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 14px"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_vendor" runat="server" AutoGenerateColumns="False" CssClass="grid" OnSortCommand="SortCommand_Click"
							DataKeyField="CM_COY_ID">
							<Columns>
								<asp:TemplateColumn HeaderText="Add">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" ToolTip="Select/Deselect All" AutoPostBack="false"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor Company Name">
									<HeaderStyle Width="40%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Contact Details ">
									<HeaderStyle Width="55%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_adds" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="CM_COY_ID" SortExpression="CM_COY_ID" HeaderText="com_ID"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD><asp:button id="cmd_Remove" runat="server" CssClass="Button" Text="Remove"></asp:button>&nbsp;<asp:button id="cmd_exit" runat="server" CssClass="button" Text="Exit"></asp:button></TD>
				</TR>
			</TABLE>
			<asp:label id="Label1" style="Z-INDEX: 102; LEFT: 8px; POSITION: absolute; TOP: 352px" runat="server">Label</asp:label></form>
	</body>
</HTML>
