<%@ Import Namespace="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="bcmSearchAccSetup.aspx.vb" Inherits="eProcure.bcmSearchAccSetup" %>
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
			oform.txtDeptName.value="";
		}
		-->
		</script>
	</HEAD>
	<BODY MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="Header" style="padding:0" colspan="3"><asp:label id ="lblTitle" runat="server" Text="Account Codes Setup"></asp:label></TD>
				</TR>
				<tr><td class="rowspacing"></td></tr>
				<TR>
	                <TD class="EmptyCol" colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Select/fill in the search criteria and click Search button to list the relevant Department Name."></asp:label>
	                </TD>
                </TR>
                <tr><td class="rowspacing"></td></tr>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="TableHeader" colspan="6">Search Criteria</TD>
							</TR>							
							<TR class="tablecol" width="100%">
								<TD class="tablecol" width="80%">&nbsp;<STRONG>Dept.&nbsp;Name</STRONG>&nbsp;:&nbsp;
									<asp:textbox id="txtDeptName" runat="server" Width="300px" CssClass="txtbox"></asp:textbox>&nbsp;
								</td>
								<TD class="TableCol" align="right" width="20%">
									<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
									<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;
								</TD>
							</TR>
							<tr><td class="rowspacing"></td></tr>
						</TABLE>
					</TD>
				</TR>				
				<TR>
					<TD class="emptycol" colspan="6">
					    <asp:datagrid id="dgDept" runat="server">							
							<Columns>
								<asp:TemplateColumn SortExpression="CDM_DEPT_NAME" HeaderText="Department Name">
									<HeaderStyle HorizontalAlign="Left" Width="80%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkDeptName"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="CDM_DEPT_CODE" SortExpression="CDM_DEPT_CODE" HeaderText="Dept. Code"></asp:BoundColumn>
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
