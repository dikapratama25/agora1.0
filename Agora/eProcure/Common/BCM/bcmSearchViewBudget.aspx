<%@ Page Language="vb" AutoEventWireup="false" Codebehind="bcmSearchViewBudget.aspx.vb" Inherits="eProcure.bcmSearchViewBudget" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>search Account Setup</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="VBScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		/*function selectAll()
		{
			SelectAllG("dgDept__ctl2_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dgDept__ctl2_chkAll","chkSelection");
		}*/
				
		function Reset(){
			var oform = document.forms(0);					
			//oform.txtUserID.value="";
			oform.txtDeptName.value="";
		}
		-->
		</script>
	</HEAD>
	<BODY topMargin="10" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header"><FONT size="3"> View Budget</FONT></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" width="100%">
							<TR>
								<TD class="tableheader">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92"></TD>
								<TD class="TableInput" style="HEIGHT: 6px"></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>Dept.&nbsp;Name</STRONG>&nbsp;:&nbsp;
									<asp:textbox id="txtDeptName" runat="server" Width="160px" CssClass="txtbox"></asp:textbox>&nbsp;
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
					<TD><asp:datagrid id="dgDept" runat="server">
							<ItemStyle Height="22px"></ItemStyle>
							<HeaderStyle Height="22px"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn SortExpression="CDM_DEPT_NAME" HeaderText="Department Name">
									<HeaderStyle HorizontalAlign="Left" Width="60%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkDeptName"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CDM_DEPT_CODE" SortExpression="CDM_DEPT_CODE" HeaderText="Dept. Code">
									<HeaderStyle Width="40%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
