<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportMatrix.aspx.vb" Inherits="eAdmin.ReportMatrix" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Report</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
		</script>
		<% Response.Write(sCSS) %>
		<%Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
			function selectAll()
			{
				SelectAllG("dtgReport__ctl1_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgReport__ctl1_chkAll","chkSelection");
			}	
					
			function resetForm()
			{
				Form1.reset();
			}	
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="HEIGHT: 16px" colSpan="2"><FONT size="1">&nbsp;</FONT><asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"></TD>
				</TR>
				<TR>
					<TD class="tableheader" colSpan="2">&nbsp;Report Matrix</TD>
				</TR>
				<TR>
					<TD noWrap></TD>
				</TR>
				</TR>
				<TR>
					<TD colSpan="2"><asp:datagrid id="dtgReport" runat="server">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										<asp:Label id="lblSelection" Runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="RM_REPORT_NAME" HeaderText="Report Name" ItemStyle-Width="95%"></asp:BoundColumn>
								<asp:BoundColumn DataField="RM_REPORT_INDEX" Visible="False"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<tr>
					<td class="emptycol" colSpan="2"></td>
				</tr>
				<tr>
					<td colSpan="2"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="ValidatorReset();" type="button" value="Clear"
							name="cmdClear" runat="server"></td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
