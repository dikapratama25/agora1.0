<%@ Import Namespace="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="mytab" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="bcmAccSetup.aspx.vb" Inherits="eProcure.bcmAccSetup" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Account Setup</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="VBScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
           Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script> 
        <%Response.Write(css)%> 
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
				
		function Reset(level){
			var oform = document.forms(0);					
		if (level==1)
			oform.txtAccCode.value="";
		else 
			if (level==2)
				oform.txtSubAccCode.value="";
			else(level==3)
				oform.txtProjCode.value="";
		}
		-->
		</script>
	</HEAD>
	<BODY MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TBODY>
					<TR>
						<TD class="Header" style="padding:0" colspan="3"><asp:label id ="lblTitle" runat="server" Text="Account Codes Setup"></asp:label></TD>
					</TR>
					<tr><td class="rowspacing"></td></tr>
				    <TR>
	                    <TD class="EmptyCol" colSpan="6">
		                    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                            Text="Select/fill in the search criteria and click Search button to list the relevant Accounts."></asp:label>
	                    </TD>
                    </TR>
                    <tr><td class="rowspacing"></td></tr>
					<TR>
						<TD class="emptycol">
							<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
								<TR>
									<TD class="tableheader" colSpan="6">&nbsp;Account Code Maintenance</TD>
								</TR>								
								<TR class="tablecol">
									<TD class="tablecol" width="15%">&nbsp;<STRONG>Dept.&nbsp;Name</STRONG>&nbsp;:</TD>
									<td class="tablecol" width="35%"><asp:label id="lblDeptName" CssClass="lblInfo" Runat="server"></asp:label></td>
									<TD class="tablecol" width="15%">&nbsp;<STRONG>Acc. &nbsp;Code</STRONG>&nbsp;:</TD>
									<td class="tablecol" width="35%"><asp:label id="lblAccCode" CssClass="lblInfo" Runat="server">--</asp:label></td>
								</TR>
								<TR class="tablecol">
									<TD class="tablecol" width="15%">&nbsp;<STRONG>Initial Budget</STRONG>&nbsp;:</TD>
									<td class="tablecol" width="35%"><asp:label id="lblInit" CssClass="lblInfo" Runat="server">-</asp:label></td>
									<TD class="tablecol" width="15%">&nbsp;<STRONG>Initial Budget</STRONG>&nbsp;:</TD>
									<td class="tablecol" width="35%"><asp:label id="lblIBAcc" CssClass="lblInfo" Runat="server">--</asp:label></td>
								</TR>
								<TR class="tablecol">
									<TD class="tablecol" width="15%">&nbsp;</TD>
									<td class="tablecol" width="35%">&nbsp;</td>
									<TD class="tablecol" width="15%">&nbsp;<STRONG>Sub Acc. Code</STRONG>&nbsp;:</TD>
									<td class="tablecol" width="35%"><asp:label id="lblSubAcc" CssClass="lblInfo" Runat="server">--</asp:label></td>
								</TR>
								<TR class="tablecol">
									<TD class="tablecol" width="15%">&nbsp;</TD>
									<td class="tablecol" width="35%">&nbsp;</td>
									<TD class="tablecol" width="15%">&nbsp;<STRONG><STRONG>Initial Budget</STRONG> &nbsp;</STRONG>&nbsp;:</TD>
									<td class="tablecol" width="35%"><asp:label id="lblIBProj" CssClass="lblInfo" Runat="server">--</asp:label></td>
								</TR>								
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD class="emptycol" colSpan="2"></TD>
					</TR>
					
					<TR>
						<td class="emptycol"><mytab:tabstrip id="tsHoriz" style="FONT-WEIGHT: bold" runat="server" TabDefaultStyle="color: #27537a;border:solid 1px gray;BORDER-BOTTOM:gray 0px;background:transparent;padding-left:10px;padding-right:10px;height:20px"
								TabHoverStyle="color:black;" TabSelectedStyle="color: black;border:solid 1px gray;border-bottom:none;background:transparent;padding-left:10px;padding-right:10px;"
								SepDefaultStyle="border-bottom:solid 1px #000000;" TargetID="mpHoriz">
								<MYTAB:TAB id="tsAccCode" Text="Account Code"></MYTAB:TAB>
								<MYTAB:TABSEPARATOR DefaultStyle="width:1;BORDER-BOTTOM:#000000 0px solid"></MYTAB:TABSEPARATOR>
								<MYTAB:TAB id="Tab3" Text="Sub Account Code" enabled="false"></MYTAB:TAB>
								<MYTAB:TABSEPARATOR DefaultStyle="width:1;BORDER-BOTTOM:#000000 0px solid"></MYTAB:TABSEPARATOR>
								<MYTAB:TAB id="Tab4" Text="Project Code" enabled="false"></MYTAB:TAB>
							</mytab:tabstrip></td>
					</TR>
					<tr>
						<td class="emptycol">
						    <mytab:multipage id="mpHoriz" style="BORDER-RIGHT: gray 1px solid; PADDING-RIGHT: 0px; BORDER-TOP: gray 1px solid; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; BORDER-LEFT: gray 1px solid; PADDING-TOP: 0px; BORDER-BOTTOM: gray 1px solid"
								runat="server" Height="380">
								<mytab:PageView>
									<table width="100%">
										<TR>
											<TD class="tablecol">&nbsp;<STRONG>Acc.&nbsp;Code</STRONG>&nbsp;:&nbsp;
												<asp:textbox id="txtAccCode" runat="server" Width="160px" CssClass="txtbox"></asp:textbox>&nbsp;
												<asp:button id="cmdSearchAccName" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdClear1" onclick="Reset(1);" type="button" value="Clear" name="cmdClear">&nbsp;</TD>
										</TR>
										<TR>
											<TD class="emptycol">
												<asp:datagrid id="dgAccCode" runat="server" OnDeleteCommand="DeleteCommand" OnEditCommand="EditCommand"
													OnPageIndexChanged="PageIndex" OnUpdateCommand="UpdateCommand" OnItemCreated="ItemCreated"
													OnItemCommand="ItemCommand" OnSortCommand="SortCommand" OnItemDataBound="ItemDataBound" CellPadding="0"
													CellSpacing="0">
													<HeaderStyle Height="22px"></HeaderStyle>
													<ItemStyle Height="22px"></ItemStyle>
													<Columns>
														<asp:ButtonColumn ButtonType="LinkButton" DataTextField="AM_ACCT_CODE" SortExpression="AM_ACCT_CODE"
															HeaderText="Account Code" CommandName="Go" ItemStyle-Width="20%"></asp:ButtonColumn>
														<asp:BoundColumn DataField="AM_ACCT_DESC" SortExpression="AM_ACCT_DESC" HeaderText="Description"
															ItemStyle-Width="50%"></asp:BoundColumn>
														<asp:BoundColumn DataField="AM_INIT_BUDGET" SortExpression="AM_INIT_BUDGET" HeaderText="Initial Budget"
															DataFormatString="{0:n}" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right"></asp:BoundColumn>
														<asp:EditCommandColumn runat="server" ButtonType="LinkButton" ItemStyle-HorizontalAlign="Center" editText="&lt;IMG src='../Plugins/images/i_edit.gif' width =17px height =17px border=0 alt='Modify'&gt;"
															UpdateText="&lt;IMG src='../Plugins/images/i_save.gif' width =17px height =17px border=0 alt='Save'&gt;"></asp:EditCommandColumn>
														<asp:ButtonColumn ButtonType="LinkButton" ItemStyle-HorizontalAlign="Center" Text="<IMG src='../Plugins/images/i_delete2.gif' width =17px height =17px border=0 alt='Delete'>"
															CommandName="Delete"></asp:ButtonColumn>
													</Columns>
												</asp:datagrid>
											</TD>
										</TR>
										<tr>
											<td>
												<asp:Label Runat="server" ID="Parent0" Visible="false"></asp:Label>
											</td>
										</tr>
									</table>
								</mytab:PageView>
								<mytab:PageView>
									<table width="100%">
										<TR>
											<TD class="tablecol">&nbsp;<STRONG>Sub Acc. Code</STRONG>&nbsp;:&nbsp;
												<asp:textbox id="txtSubAccCode" runat="server" Width="160px" CssClass="txtbox"></asp:textbox>&nbsp;
												<asp:button id="cmdsearchSub" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdClear2" onclick="Reset(2);" type="button" value="Clear" name="cmdClear">&nbsp;</TD>
										</TR>
										<TR>
											<TD class="emptycol">
												<asp:datagrid id="dgSubAccCode" runat="server" OnDeleteCommand="DeleteCommand" OnEditCommand="EditCommand"
													OnPageIndexChanged="PageIndex" OnUpdateCommand="UpdateCommand" OnItemCreated="ItemCreated"
													OnItemCommand="ItemCommand" OnItemDataBound="ItemDataBound" OnSortCommand="SortCommand">
													<HeaderStyle Height="22px"></HeaderStyle>
													<ItemStyle Height="22px"></ItemStyle>
													<Columns>
														<asp:ButtonColumn ButtonType="LinkButton" DataTextField="AM_ACCT_CODE" SortExpression="AM_ACCT_CODE"
															HeaderText="Sub Account Code" CommandName="Go" ItemStyle-Width="20%"></asp:ButtonColumn>
														<asp:BoundColumn DataField="AM_ACCT_DESC" SortExpression="AM_ACCT_DESC" HeaderText="Description"
															ItemStyle-Width="50%"></asp:BoundColumn>
														<asp:BoundColumn DataField="AM_INIT_BUDGET" SortExpression="AM_INIT_BUDGET" HeaderText="Initial Budget"
															DataFormatString="{0:n}" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right"></asp:BoundColumn>
														<asp:EditCommandColumn HeaderText="Modify" ItemStyle-HorizontalAlign="Center" runat="server" ButtonType="LinkButton"
															editText="&lt;IMG src='../Plugins/images/i_edit.gif' width =17px height =17px border=0 alt='Modify'&gt;" UpdateText="&lt;IMG src='../Plugins/images/i_save.gif' width =17px height =17px border=0 alt='Save'&gt;"></asp:EditCommandColumn>
														<asp:ButtonColumn ButtonType="LinkButton" ItemStyle-HorizontalAlign="Center" Text="&lt;IMG src='../Plugins/images/i_delete2.gif' width =17px height =17px border=0 alt='Delete'&gt;"
															CommandName="Delete" HeaderText="Delete"></asp:ButtonColumn>
													</Columns>
												</asp:datagrid>
											</TD>
										</TR>
										<tr>
											<td>
												<asp:Label Runat="server" ID="Parent1" Visible="false"></asp:Label>
											</td>
										</tr>
									</table>
								</mytab:PageView>
								<mytab:PageView>
									<table width="100%">
										<TR>
											<TD class="tablecol">&nbsp;<STRONG>Project Code</STRONG>&nbsp;:&nbsp;
												<asp:textbox id="txtProjCode" runat="server" Width="160px" CssClass="txtbox"></asp:textbox>&nbsp;
												<asp:button id="cmdProjCode" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdClear3" onclick="Reset(3);" type="button" value="Clear" name="cmdClear">&nbsp;</TD>
										</TR>
										<tr>
											<TD class="emptycol">
												<asp:datagrid id="dgProjCode" runat="server" OnDeleteCommand="DeleteCommand" OnEditCommand="EditCommand"
													OnPageIndexChanged="PageIndex" OnUpdateCommand="UpdateCommand" OnItemCreated="ItemCreated"
													OnSortCommand="SortCommand" OnItemCommand="ItemCommand" OnItemDataBound="ItemDataBound">
													<HeaderStyle Height="22px"></HeaderStyle>
													<ItemStyle Height="22px"></ItemStyle>
													<Columns>
														
														<asp:BoundColumn DataField="AM_ACCT_CODE" SortExpression="AM_ACCT_CODE" HeaderText="Project Code"
															 ReadOnly="True"  ItemStyle-Width="20%"></asp:BoundColumn>
														<asp:BoundColumn DataField="AM_ACCT_DESC" SortExpression="AM_ACCT_DESC" HeaderText="Description"
															ItemStyle-Width="50%"></asp:BoundColumn>
														<asp:BoundColumn DataField="AM_INIT_BUDGET" SortExpression="AM_INIT_BUDGET" HeaderText="Initial Budget"
															DataFormatString="{0:n}" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right"></asp:BoundColumn>
														<asp:EditCommandColumn HeaderText="Modify" ItemStyle-HorizontalAlign="Center"  runat="server" ButtonType="LinkButton"
															editText="&lt;IMG src='../Plugins/images/i_edit.gif' width =17px height =17px border=0 alt='Modify'&gt;" UpdateText="&lt;IMG src='../Plugins/images/i_save.gif' width =17px height =17px border=0 alt='Save'&gt;"></asp:EditCommandColumn>
														<asp:ButtonColumn ButtonType="LinkButton" ItemStyle-HorizontalAlign="Center" Text="&lt;IMG src='../Plugins/images/i_delete2.gif' width =17px height =17px border=0 alt='Delete'&gt;"
															CommandName="Delete" HeaderText="Delete"></asp:ButtonColumn>
													</Columns>
												</asp:datagrid>
											</TD>
										</tr>
										<tr>
											<td>
												<asp:Label Runat="server" ID="Parent2" Visible="false"></asp:Label>
											</td>
										</tr>
									</table>
								</mytab:PageView>
							</mytab:multipage>
						</td>
					</tr>					
					<TR>
						<TD class="emptycol"></TD>
					</TR>
					<TR>
						<TD class="emptycol">&nbsp;&nbsp;</TD>
					</TR>
					<TR>
						<TD class="emptycol">
						    <asp:hyperlink id="lnkBack" Runat="server" NavigateUrl="" ForeColor="blue"><STRONG>&lt; Back</STRONG></asp:hyperlink>
						</TD>
					</TR>
				</TBODY>
			</TABLE>
		</form>
	</BODY>
</HTML>
