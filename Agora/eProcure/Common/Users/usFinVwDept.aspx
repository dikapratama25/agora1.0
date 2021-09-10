<%@ Page Language="vb" AutoEventWireup="false" Codebehind="usFinVwDept.aspx.vb" Inherits="eProcure.usFinVwDept" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title></title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="VBScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		function selectAll()
		{
			SelectAllG("dgDept_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dgDept_ctl02_chkAll","chkSelection");
		}
		
		-->
		</script>
	</HEAD>
	<BODY topMargin="10" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_UserAddr_tabs"))%>
			<TABLE class="alltable" id="Table1" width="100%" cellSpacing="0" cellPadding="0" border="0">
				<tr>
					<TD class="linespacing1" colSpan="6"></TD>
			    </TR>
				<TR>
	                <TD colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text=""></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="6"></TD>
			    </TR>
				<TR>
				<TD class="TableHeader" colSpan="6">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
			        <TD class="TableCol" width="15%" ><STRONG><asp:label id="lbl1" runat="server" Text="User Name " CssClass="lbl"></asp:label></STRONG>: </TD>
			        <td class="TableCol" colspan="4"><asp:dropdownlist id="cboUser" runat="server" AutoPostBack="True" CssClass="txtbox" Width="350px" ></asp:dropdownlist></TD>
			        <td class="TableCol"></td>
			    </TR>
			<tr><td class="rowspacing"></td></tr>
			</TABLE>
			<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD><asp:datagrid id="dgDept" runat="server">
							<ItemStyle Height="23px"></ItemStyle>
							<HeaderStyle Height="23px"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All" Checked="true"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="CDM_SELECTED" SortExpression="CDM_SELECTED" HeaderText="Code"></asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_DEPT_CODE" SortExpression="CDM_DEPT_CODE" HeaderText="Department Code">
									<HeaderStyle Width="35%"></HeaderStyle>
									<ItemStyle></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Department Name">
									<HeaderStyle Width="60%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol" ><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button></TD>
				</TR>				
			</TABLE>
		</form>
	</BODY>
</HTML>
