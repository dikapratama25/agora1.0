<%@ Page Language="vb" AutoEventWireup="false" Codebehind="bcmViewBudget.aspx.vb" Inherits="eProcure.bcmViewBudget" %>
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
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script> 
        <% Response.Write(css)%> 
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
			oform.txtFind.value="";
		}
		-->
		</script>
	</HEAD>
	<BODY MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="Header" style="padding:0" colspan="3"><asp:label id ="lblTitle" runat="server" Text="View Budget"></asp:label></TD>
				</TR>
				<tr><td class="rowspacing"></td></tr>
				<TR>
	                <TD class="EmptyCol" colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Select/fill in the search criteria and click Search button to list the relevant Budget Account."></asp:label>
	                </TD>
                </TR>
                <tr><td class="rowspacing"></td></tr>
				<TR>
					<TD class="TableHeader" colspan="6">Search Criteria</TD>
				</TR>				
				<TR class="tablecol" width="100%">
					<TD class="tablecol" width="80%">&nbsp;<STRONG>View by</STRONG>&nbsp;:&nbsp;
						<asp:dropdownlist id="cboLevel" Runat="server" Width="200px" CssClass="ddl">
							<asp:ListItem Selected="True" Value="0">Department</asp:ListItem>
							<asp:ListItem Value="1">Account Code</asp:ListItem>
							<asp:ListItem Value="2">Sub Account Code</asp:ListItem>
							<asp:ListItem Value="3">Project Code</asp:ListItem>
						</asp:dropdownlist>&nbsp; <STRONG>Find</STRONG>&nbsp;:&nbsp;
						<asp:textbox id="txtFind" runat="server" Width="94px" CssClass="txtbox"></asp:textbox>&nbsp;
			        </td>
			        <TD class="TableCol" align="right" width="20%">
						<asp:button id="cmdSearchUser" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
						<INPUT class="button" id="cmdClearUser" onclick="Reset();" type="button" value="Clear"
							name="cmdClear">&nbsp;
					</TD>
				</TR>
				<tr><td class="rowspacing"></td></tr>				
				<TR>
					<TD class="emptycol" colspan="6">
					    <asp:datagrid id="dgView" runat="server" DataKeyField="CDM_DEPT_INDEX">							
							<Columns>
								<asp:BoundColumn Visible="False" DataField="CDM_DEPT_INDEX" SortExpression="CDM_DEPT_INDEX" HeaderText="Department&lt;BR&gt;Index"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="AM_ACCT_INDEX" SortExpression="AM_ACCT_INDEX" HeaderText="Code&lt;BR&gt;Index"></asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Department&lt;BR&gt;Name">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_ACCT_CODE" SortExpression="AM_ACCT_CODE" HeaderText="Acc. Code">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_SUB_ACCT_CODE" SortExpression="AM_SUB_ACCT_CODE" HeaderText="Sub&lt;BR&gt;Acc. Code">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_PROJ_CODE" SortExpression="AM_PROJ_CODE" HeaderText="Project&lt;BR&gt;Code">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<%--<ItemStyle Font-Size="8pt"></ItemStyle>--%>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AM_ACCT_DESC" SortExpression="AM_ACCT_DESC" HeaderText="Description">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<%--<ItemStyle Font-Size="8pt"></ItemStyle>--%>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Carry Forward&lt;BR&gt;Budget">
									<HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
									<%--<ItemStyle Font-Size="6.5pt" HorizontalAlign="Right"></ItemStyle>--%>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="lblBCF" Runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="AM_INIT_BUDGET" SortExpression="AM_INIT_BUDGET" HeaderText="Initial&lt;BR&gt;Budget"
									DataFormatString="{0:n}">
									<HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<%--<ItemStyle Font-Size="6.5pt" HorizontalAlign="Right"></ItemStyle>--%>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Operating&lt;BR&gt;Budget">
									<HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
									<%--<ItemStyle Font-Size="6.5pt" HorizontalAlign="Right"></ItemStyle>--%>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="lblOpBudget" Runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Reserved&lt;BR&gt;Budget">
									<HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="lblResBudget" Runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Committed&lt;BR&gt;Budget">
									<HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="lblComBudget" Runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Utilized&lt;BR&gt;Budget">
									<HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="lblUtiBudget" Runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
			</TABLE>
		</form>
	</BODY>
</HTML>
