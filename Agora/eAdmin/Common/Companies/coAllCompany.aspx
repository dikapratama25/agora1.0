<%@ Import Namespace="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="coAllCompany.aspx.vb" Inherits="eAdmin.coAllCompany" smartNavigation="False" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title></title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="VBScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
        </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		function selectAll()
		{
			SelectAllG("dgComp__ctl2_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dgComp__ctl2_chkAll","chkSelection");
		}
				
		function Reset(){
			var oform = document.forms(0);					
			oform.txtCompID.value="";
			oform.txtCompName.value="";
		}
		-->
		</script>
	</HEAD>
	<BODY topMargin="10" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header"><FONT size="3">All Companies</FONT></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" width="100%">
							<TR>
								<TD class="tableheader" colspan="3" >&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92"></TD>
								<TD class="TableInput" style="HEIGHT: 6px" colspan="2"></TD>
							</TR>
							<TR>
								<TD class="tablecol" colspan="2" >&nbsp;<STRONG>Company&nbsp;ID</STRONG>&nbsp;:
									<asp:textbox id="txtCompID" runat="server" Width="94px" CssClass="txtbox" MaxLength="20"></asp:textbox>&nbsp;<STRONG>Company 
										Name</STRONG>&nbsp;:
									<asp:textbox id="txtCompName" runat="server" Width="128px" CssClass="txtbox" MaxLength="100"></asp:textbox>&nbsp;&nbsp;</TD>
								<TD class="tablecol" align="right">
									<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92"></TD>
								<TD class="TableInput" style="HEIGHT: 6px; width: 11px;" colspan="2"></TD>
							</TR>
							<TR>
								<td style="HEIGHT: 7px" colspan="3">&nbsp;</td>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:label id="lbl_result" runat="server" ForeColor="Red"></asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dgComp" runat="server">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CM_COY_ID" SortExpression="CM_COY_ID" HeaderText="Comp. ID">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Company Name">
									<HeaderStyle Width="24%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_TYPE" SortExpression="CM_COY_TYPE" HeaderText="Comp. Type">
									<HeaderStyle Width="12%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_LICENCE_PACKAGE" SortExpression="CM_LICENCE_PACKAGE" HeaderText="License Package">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_LICENSE_USERS" SortExpression="CM_LICENSE_USERS" HeaderText="No.of License Users">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_SUB_START_DT" SortExpression="CM_SUB_START_DT" HeaderText="Subscription Start Date"
									DataFormatString="{0:dd/MM/yyyy}">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_SUB_END_DT" SortExpression="CM_SUB_END_DT" HeaderText="Subscription End Date"
									DataFormatString="{0:dd/MM/yyyy}">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_STATUS" SortExpression="CM_STATUS" HeaderText="Status">
									<HeaderStyle Width="6%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Enabled="False"></asp:button>&nbsp;<asp:button id="cmdActivate" runat="server" CssClass="button" Text="Activate" Enabled="False"></asp:button>&nbsp;<asp:button id="cmdDeactivate" runat="server" CssClass="button" Text="Deactivate" Enabled="False"></asp:button>
						&nbsp;<INPUT class="button" id="cmdReset" onclick="DeselectAllG('dgComp__ctl2_chkAll','chkSelection')"
							type="button" value="Reset" name="cmdReset" runat="server" disabled style="DISPLAY:none"></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<!--<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server" NavigateUrl="javascript: history.back()" ForeColor="blue">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>-->
			</TABLE>
		</form>
	</BODY>
</HTML>
