<%@ Page Language="vb" AutoEventWireup="false" Codebehind="bcmSearchBuyerAss.aspx.vb" Inherits="eProcure.bcmSearchBuyerAss" %>
<%@ Import Namespace="Microsoft.Web.UI.WebControls" %>
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
			oform.txtDeptName.value="";
			oform.txtUserName.value="";
			oform.txtUserID.value="";
			oform.txtAccCode.value="";
		}
		-->
		</script>
	</HEAD>
	<BODY MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="header" style="padding:0" colspan="3"><asp:label id ="lblTitle" runat="server" Text="BCM Assignment"></asp:label></TD>
				</TR>
				<tr><td class="rowspacing"></td></tr>
				<TR>
	                <TD class="EmptyCol" colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Select/fill in the search criteria and click Search button to list the relevant BCM Assignment information."></asp:label>
	                </TD>
                </TR>
                <tr><td class="rowspacing"></td></tr>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader" colspan="10">&nbsp;Search Criteria</TD>
							</TR>							
							<TR>
								<TD class="tablecol" colspan="10">&nbsp;&nbsp;<STRONG>Assign&nbsp;By</STRONG>&nbsp;:&nbsp;&nbsp;
									<asp:radiobutton id="rdUser" GroupName="Ass" Width="80" Checked="true" CssClass="Rbtn" Text="User"
										Runat="server" AutoPostBack="true"></asp:radiobutton><asp:radiobutton id="rdAccountCode" GroupName="Ass" CssClass="Rbtn" Text="Account Code" Runat="server"
										AutoPostBack="true"></asp:radiobutton></TD>
							</TR>
							<TR class="tablecol">
								<TD class="tablecol" style="HEIGHT: 7px" width="100%"></TD>
							</TR>
						</TABLE>
						<div id="tbUser" runat="server">
							<table class="alltable" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<TD class="tablecol">&nbsp; <STRONG>Dept.&nbsp;Name</STRONG>&nbsp;:&nbsp;
										<asp:textbox id="txtDeptName" runat="server" Width="94px" CssClass="txtbox"></asp:textbox>&nbsp;
										<STRONG>User&nbsp;Name</STRONG>&nbsp;:&nbsp;
										<asp:textbox id="txtUserName" runat="server" Width="94px" CssClass="txtbox"></asp:textbox>&nbsp;
										<STRONG>User&nbsp;ID</STRONG>&nbsp;:&nbsp;
										<asp:textbox id="txtUserID" runat="server" Width="94px" CssClass="txtbox"></asp:textbox>&nbsp;
									</TD>
									<td class="tablecol" align="right">
										<asp:button id="cmdSearchUser" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
										<INPUT class="button" id="cmdClearUser" onclick="Reset();" type="button" value="Clear"
											name="cmdClear">&nbsp;
									</TD>
								</TR>
							<TR class="tablecol">
								<TD class="tablecol" style="HEIGHT: 7px" colspan="10"></TD>
							</TR>
							</table>
						</div>
						<div id="tbAccCode" style="DISPLAY: none" runat="server">
							<table class="alltable" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<TD class="tablecol">&nbsp; <STRONG>Acc.&nbsp;Code</STRONG>&nbsp;:&nbsp;&nbsp;&nbsp;
										<asp:textbox id="txtAccCode" runat="server" Width="180px" CssClass="txtbox"></asp:textbox>&nbsp;
									</td>
									<td class="tablecol" align="right">
										<asp:button id="cmdSearchAccCode" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
										<INPUT class="button" id="cmdClearAcc" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;
								    </TD>
								</TR>
							<TR class="tablecol">
								<TD class="tablecol" style="HEIGHT: 7px" colspan="10"></TD>
							</TR>
							</table>
						</div>
					</TD>
				</TR>				
				<TR>
					<TD class="emptycol"><asp:label id="lbl_result" runat="server" ForeColor="Red"></asp:label></TD>
				</TR>
				<TR id="Acc" runat="server">
					<TD class="emptycol"><asp:datagrid id="dgAccCode" runat="server" OnPageIndexChanged="PageIndexChanged" OnItemCreated="ItemCreated">
							<Columns>
								<asp:BoundColumn DataField="Acct_List" HeaderText="Account Code">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemStyle Width="450px"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Acct_Index" HeaderText="AM_ACCT_INDEX">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn Visible="False" HeaderText="Assigned A/C">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:Repeater ID="rptDeptIndex" Runat="server">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "CDM_DEPT_INDEX")%>
												<br>
											</ItemTemplate>
										</asp:Repeater>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Dept. Name">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemStyle Width="120px"></ItemStyle>
									<ItemTemplate>
										<asp:Repeater ID="rptDept" Runat="server">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "CDM_DEPT_NAME")%>
												<br>
											</ItemTemplate>
										</asp:Repeater>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="User Name">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemStyle Wrap="False" Width="180px"></ItemStyle>
									<ItemTemplate>
										<asp:Repeater ID="rptUserName" Runat="server">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "UM_USER_NAME")%>
												<br>
											</ItemTemplate>
										</asp:Repeater>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="User ID">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle Width="150px"></ItemStyle>
									<ItemTemplate>
										<asp:Repeater ID="rptUserID" Runat="server">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "UM_USER_ID")%>
												<br>
											</ItemTemplate>
										</asp:Repeater>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:ButtonColumn Text="&lt;IMG src='../Plugins/images/i_edit.gif' width =17px height =17px border=0 alt='modify'&gt;"
									CommandName="Modify" ItemStyle-HorizontalAlign="center">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle Width="5px" HorizontalAlign="center"></ItemStyle>
								</asp:ButtonColumn>
								<asp:ButtonColumn Text="&lt;IMG src='../Plugins/images/i_delete2.gif' width =17px height =17px border=0 alt='Remove'&gt;"
									CommandName="Delete" ItemStyle-HorizontalAlign="center">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle Width="5px" HorizontalAlign="center"></ItemStyle>
								</asp:ButtonColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:datagrid id="dgDept" runat="server" OnPageIndexChanged="PageIndexChanged" OnItemCreated="ItemCreated">
							<Columns>
								<asp:BoundColumn DataField="CDM_DEPT_NAME" HeaderText="Department Name">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemStyle Width="150px"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CDM_DEPT_CODE" HeaderText="Department CODE">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UM_USER_NAME" HeaderText="User Name">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemStyle Width="100px"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UM_USER_ID" HeaderText="User ID">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle Width="100px"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Assigned A/C">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemStyle VerticalAlign="Top"></ItemStyle>
									<ItemTemplate>
										<asp:Repeater ID="sub" Runat="server">
											<ItemTemplate>
												<%# DataBinder.Eval(Container.DataItem, "Acct_List")%>
												<br>
											</ItemTemplate>
										</asp:Repeater>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:ButtonColumn Text="&lt;IMG src='../Plugins/images/i_edit.gif' width =17px height =17px border=0 alt='modify'&gt;"
									CommandName="Modify" ItemStyle-HorizontalAlign="center">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle Width="5px" HorizontalAlign="center"></ItemStyle>
								</asp:ButtonColumn>
								<asp:ButtonColumn Text="&lt;IMG src='../Plugins/images/i_delete2.gif' width =17px height =17px border=0 alt='Remove'&gt;"
									CommandName="Delete" ItemStyle-HorizontalAlign="center">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle Width="5px" HorizontalAlign="center"></ItemStyle>
								</asp:ButtonColumn>
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
