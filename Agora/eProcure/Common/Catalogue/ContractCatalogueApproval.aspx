<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ContractCatalogueApproval.aspx.vb" Inherits="eProcure.ContractCatalogueApproval" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Contract Catalogue Approval</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		 <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
           Dim calStartDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
           Dim calEndDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
      </script>
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		function Reset(){
			var oform = document.forms(0);
			oform.txtCode.value="";
			oform.txtVendor.value="";
			oform.txtDesc.value="";
			oform.txtDateFr.value="";
			oform.txtDateTo.value="";
			checkStatus(false);
		}
		
		function SelectAll_1()
		{
			checkStatus(true);
		}
		
		function checkStatus(checked)
		{
			var oform = document.forms(0);
			oform.chkBuyerPending.checked=checked;
			oform.chkBuyerReject.checked=checked;
			oform.chkHubPending.checked=checked;			
			oform.chkHubReject.checked=checked;
			oform.chkHubApprove.checked=checked;
		}
		
		-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="header" colspan="6"><FONT size="1"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="6">&nbsp;</TD>
				</TR>
				<tr>
					<TD class="tableheader" colSpan="6">&nbsp;Search Criteria</TD>
				</tr>
				<TR>
					<TD class="tablecol" width="10%" nowrap>&nbsp;<STRONG>Contract Ref. No.</STRONG>&nbsp;:&nbsp;</TD>
					<TD class="tablecol" width="20%" nowrap><asp:textbox id="txtCode" runat="server" CssClass="txtbox" Width="100px"></asp:textbox></TD>
					<TD class="tablecol" width="10%" nowrap>&nbsp;<STRONG>Description</STRONG>&nbsp;:&nbsp;</TD>
					<TD class="tablecol" width="25%"><asp:textbox id="txtDesc" runat="server" CssClass="txtbox" Width="100px"></asp:textbox></TD>
					<TD class="tablecol" width="10%" nowrap>&nbsp;<STRONG>Vendor&nbsp;Name</STRONG>&nbsp;:&nbsp;</TD>
					<TD class="tablecol" width="25%" nowrap><asp:textbox id="txtVendor" runat="server" CssClass="txtbox" Width="100px"></asp:textbox></TD>
				</TR>
				<TR class="tablecol">
					<TD nowrap>&nbsp;<STRONG>Start Date</STRONG> :</TD>
					<TD class="tablecol" nowrap><asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox"  contentEditable="false" ></asp:textbox><% Response.Write(calStartDate)%></TD>
					<TD class="tablecol" nowrap>&nbsp;<STRONG>End Date</STRONG> :</TD>
					<TD class="tablecol" nowrap><asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox"  contentEditable="false" ></asp:textbox><% Response.Write(calStartDate)%></TD>
					<TD class="tablecol" colspan="2"></TD>
				</TR>
				<tr class="tablecol">
					<td nowrap>&nbsp;<STRONG>Status</STRONG> :</td>
					<td colspan="2" nowrap><asp:checkbox id="chkBuyerPending" Text="Pending Approval From Buyer" Runat="server"></asp:checkbox></td>
					<td nowrap><asp:checkbox id="chkBuyerReject" Text="Rejected By Buyer" Runat="server"></asp:checkbox></td>
					<td nowrap colspan="2"><asp:checkbox id="chkHubPending" Text="Pending Approval From Hub Admin" Runat="server"></asp:checkbox></td>
				</tr>
				<tr class="tablecol">
					<td nowrap>&nbsp;</td>
					<td colspan="2" nowrap><asp:checkbox id="chkHubReject" Text="Rejected By Hub Admin" Runat="server"></asp:checkbox></td>
					<td nowrap><asp:checkbox id="chkHubApprove" Text="Approved" Runat="server"></asp:checkbox></td>
					<td nowrap colspan="2">&nbsp;</td>
				</tr>
				<tr class="tablecol">
					<td colspan="6">&nbsp;</td>
				</tr>
				<tr class="tablecol">
					<td colspan="6"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
						<INPUT class="button" id="cmdSelectAll" onclick="SelectAll_1();" type="button" value="Select All"
							name="cmdSelectAll"> <INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">
						<asp:comparevalidator id="cvDate" runat="server" Type="Date" Operator="GreaterThanEqual" ControlToCompare="txtDateFr"
							Display="None" ErrorMessage="Start Date should be <= End Date." ControlToValidate="txtDateTo"></asp:comparevalidator></td>
				</tr>
				<TR>
					<TD class="emptycol" colspan="6"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
							ShowSummary="False"></asp:validationsummary></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="6">&nbsp;</TD>
				</TR>
				<TR>
					<TD colspan="6"><asp:datagrid id="dtgCatalogue" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn SortExpression="CDUM_Contract_Code" HeaderText="Contract Ref. No.">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CDUM_Contract_Desc" SortExpression="CDUM_Contract_Desc"  readonly="true"  
									HeaderText="Description">
									<HeaderStyle Width="22%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME"  readonly="true"    HeaderText="Vendor Name">
									<HeaderStyle Width="18%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDUM_Start_Date" SortExpression="CDUM_Start_Date"  readonly="true"    HeaderText="Start Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDUM_End_Date" SortExpression="CDUM_End_Date"  readonly="true"   HeaderText="End Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDUM_Submit_Date" SortExpression="CDUM_Submit_Date" readonly="true"  HeaderText="Submission Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC"  readonly="true"    HeaderText="Status">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CDUM_B_Reject_Cnt" SortExpression="CDUM_B_Reject_Cnt"
									 readonly="true"    HeaderText="CDUM_B_Reject_Cnt"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CDUM_H_Reject_Cnt" SortExpression="CDUM_H_Reject_Cnt"
									 readonly="true"    HeaderText="CDUM_H_Reject_Cnt"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CDUM_Upload_Status" SortExpression="CDUM_Upload_Status"
									 readonly="true"    HeaderText="CDUM_Upload_Status"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
