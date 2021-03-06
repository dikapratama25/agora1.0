<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UsSearchUsrGrp.aspx.vb" Inherits="eAdmin.usSearchUsrGrp" %>
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
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
        </script>
        <% Response.Write(sCSS)%>
        <% Response.Write(Session("WheelScript"))%>
                
		<script language="javascript">
				
		
		function selectAll()
		{
			SelectAllG("dgUsrGrp__ctl2_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dgUsrGrp__ctl2_chkAll","chkSelection");
		}
				
		function Reset(){
			var oform = document.forms(0);					
			oform.txtUsrGrpID.value="";
			oform.txtUsrGrpName.value="";
		}
		
		</script>
	</HEAD>
	<BODY topMargin="10" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header"><FONT size="3">User Group Maintenance</FONT></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92"></TD>
								<TD class="TableInput" style="HEIGHT: 6px"></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>User&nbsp;Group ID</STRONG>&nbsp;:
									<asp:textbox id="txtUsrGrpID" runat="server" CssClass="txtbox" Width="94px" MaxLength="30"></asp:textbox>&nbsp;<STRONG>User 
										Group Name</STRONG>&nbsp;&nbsp;:
									<asp:textbox id="txtUsrGrpName" runat="server" CssClass="txtbox" Width="128px" MaxLength="100"></asp:textbox>&nbsp;&nbsp;
									<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92"></TD>
								<TD class="TableInput" style="HEIGHT: 6px"></TD>
							</TR>
							<TR>
								<td style="HEIGHT: 7px">&nbsp;</td>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:label id="lbl_result" runat="server" ForeColor="Red"></asp:label></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dgUsrGrp" runat="server">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="UGM_USRGRP_ID" HeaderText="User Group ID">
									<HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkUsrGrpID"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="UGM_USRGRP_NAME" SortExpression="UGM_USRGRP_NAME" HeaderText="User Group Name">
									<HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UGM_APP_PKG" SortExpression="UGM_APP_PKG" HeaderText="Application Package">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UGM_FIXED_ROLE" SortExpression="UGM_FIXED_ROLE" HeaderText="Role">
									<HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UGM_TYPE" SortExpression="UGM_TYPE" HeaderText="Type">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn Visible="False">
									<HeaderStyle HorizontalAlign="Left" Width="0%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label Runat="server" ID="lblAppPackage" Visible="False"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" CssClass="button" Text="Modify" Enabled="False"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Enabled="False"></asp:button>&nbsp;<INPUT class="button" id="cmdReset" disabled onclick="DeselectAllG('dgUsrGrp__ctl2_chkAll','chkSelection')"
							type="button" value="Reset" name="cmdReset" runat="server" style="DISPLAY: none"></TD>
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
