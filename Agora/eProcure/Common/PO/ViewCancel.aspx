<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ViewCancel.aspx.vb" Inherits="eProcure.ViewCancel" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ViewCancel</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		//For check in Datagrid
		function selectAll()
		{
			SelectAllG("dtg_CancelList_ctl02_chkAll","chkSelection");
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtg_CancelList_ctl02_chkAll","chkSelection");
		}
		
		
		function Reset()
		{
			var oform = document.forms(0);					
			oform.txt_po_no.value="";
			oform.txt_CRNO.value="";
			oform.chk_New.checked=false;
			oform.chk_ack.checked=false;
		}
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","help:No;resizable: No;Status:No;dialogHeight: 255px; dialogWidth: 300px");
			return false;
		}
			
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header"><FONT size="1"></FONT><asp:label id="lblTitle" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" runat="server">
							<TR>
								<TD class="tableheader">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tableCOL" style="HEIGHT: 20px">&nbsp;<STRONG>CR No. </STRONG>:<STRONG> </STRONG>
									<asp:textbox id="txt_CRNO" runat="server" Width="105px" CssClass="TXTBOX"></asp:textbox><STRONG>&nbsp;&nbsp;&nbsp;PO 
										No.&nbsp;</STRONG>:<STRONG> </STRONG>
									<asp:textbox id="txt_po_no" runat="server" Width="105px" CssClass="TXTBOX"></asp:textbox><STRONG>&nbsp;</STRONG><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button><STRONG>&nbsp;</STRONG><INPUT class="Button" id="cmdClear" runat="server" type="button" value="Clear" name="cmdClear"
										onclick="Reset();"><STRONG> </STRONG>
								</TD>
							</TR>
							<TR>
								<TD class="tableCOL" style="HEIGHT: 20px">&nbsp;<STRONG>Status </STRONG>:<STRONG> </STRONG>
									<asp:checkbox id="chk_New" runat="server" Text="New "></asp:checkbox>&nbsp;
									<asp:checkbox id="chk_ack" runat="server" Text="Acknowledged"></asp:checkbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<tr>
					<TD colSpan="3"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" ShowSummary="False" ShowMessageBox="True"
							DisplayMode="List"></asp:validationsummary></TD>
				</tr>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="tblSearchResult" width="100%" cellSpacing="0" cellPadding="0"
							border="0" runat="server">
							<TR>
								<TD class="emptycol" colSpan="5"></TD>
							</TR>
							<TR>
								<TD colSpan="5"><asp:datagrid id="dtg_CancelList" runat="server" DataKeyField="PCM_CR_NO" OnSortCommand="SortCommand_Click"
										AutoGenerateColumns="False">
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
											<asp:BoundColumn SortExpression="PCM_CR_NO" HeaderText="CR No.">
												<HeaderStyle Width="20%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn SortExpression="POM_PO_NO" HeaderText="PO No.">
												<HeaderStyle Width="20%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PCM_REQ_DATE" SortExpression="PCM_REQ_DATE">
												<HeaderStyle Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" HeaderText="Request By ">
												<HeaderStyle Width="12%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status">
												<HeaderStyle Width="12%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Buyer Company">
												<HeaderStyle Width="21%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="POM_S_COY_NAME" SortExpression="POM_S_COY_NAME" HeaderText="Vendor Company"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PCM_CR_NO"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="POM_PO_INDEX"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PCM_B_COY_ID"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="POM_PO_NO"></asp:BoundColumn>
										</Columns>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="5"></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="5"><asp:button id="cmd_ack" runat="server" Width="128px" CssClass="button" Text="Mass Acknowledge"
										Height="22px" Visible="False"></asp:button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD>
						<div id="hidlink" runat="server"><A id="back" href="#" runat="server"><strong>&lt; Back</strong></A></div>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
