<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HubCatalogueApproval.aspx.vb" Inherits="eAdmin.HubCatalogueApproval" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Catalogue Approval</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
            Dim calStartDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
            Dim calEndDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
       </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		function Reset(){
			var oform = document.forms(0);
			oform.txtCode.value="";
			oform.txtBuyer.value="";
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
			oform.chkHubPending.checked=checked;
			oform.chkReject.checked=checked;
		}
		
		-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" width="100%" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" colspan="6"><FONT size="1"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="6">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="tableheader" colspan="6">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
					<TD class="tablecol" nowrap width="10%">&nbsp;<STRONG>Contract Ref.&nbsp;No.</STRONG>&nbsp;:</TD>
					<TD class="tablecol" nowrap width="20%"><asp:textbox id="txtCode" runat="server" CssClass="txtbox" Width="100px"></asp:textbox></TD>
					<TD class="tablecol" nowrap width="10%">&nbsp;<STRONG>Description</STRONG>&nbsp;:</TD>
					<TD class="tablecol" nowrap width="20%"><asp:textbox id="txtDesc" runat="server" CssClass="txtbox" Width="100px"></asp:textbox></TD>
					<TD class="tablecol" nowrap width="15%">&nbsp;<STRONG>Buyer Company</STRONG>&nbsp;:</TD>
					<TD class="tablecol" nowrap width="25%"><asp:textbox id="txtBuyer" runat="server" CssClass="txtbox" Width="100px"></asp:textbox></TD>
				</TR>
				<tr class="tablecol">
					<TD class="tablecol" nowrap>&nbsp;<STRONG>Start Date</STRONG>&nbsp;:</TD>
					<TD class="tablecol" nowrap><asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(calStartDate)%></TD>
					<TD class="tablecol" nowrap>&nbsp;<STRONG>End Date</STRONG>&nbsp;:</TD>
					<TD class="tablecol" nowrap><asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(calEndDate)%></TD>
					<TD class="tablecol" colspan="2"></TD>
				</tr>
				<tr class="tablecol">
					<td nowrap>&nbsp;<STRONG>Status</STRONG>&nbsp;:</td>
					<TD class="tablecol" nowrap colspan="2"><asp:checkbox id="chkHubPending" Text="Pending Approval From Hub Admin" Runat="server"></asp:checkbox></TD>
					<TD class="tablecol" nowrap colspan="3"><asp:checkbox id="chkReject" Text="Rejected By Hub Admin" Runat="server"></asp:checkbox></TD>
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
					<TD colspan="6"><asp:datagrid id="dtgCatalogue" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
							DataKeyField="CDUM_Upload_Index">
							<Columns>
								<asp:TemplateColumn SortExpression="CDUM_Contract_Code" HeaderText="Contract Ref. No.">
									<HeaderStyle HorizontalAlign="left" Width="17%"></HeaderStyle>
									<ItemStyle HorizontalAlign="left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CDUM_Contract_Desc" SortExpression="CDUM_Contract_Desc" ReadOnly="True"
									HeaderText="Description">
									<HeaderStyle Width="22%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" ReadOnly="True" HeaderText="Buyer Company">
									<HeaderStyle Width="19%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDUM_Start_Date" SortExpression="CDUM_Start_Date" ReadOnly="True" HeaderText="Start Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign=Right></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDUM_End_Date" SortExpression="CDUM_End_Date" ReadOnly="True" HeaderText="End Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign=Right></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDUM_Submit_Date" SortExpression="CDUM_Submit_Date" ReadOnly="True" HeaderText="Submission Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign=Right></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" ReadOnly="True" HeaderText="Status">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDUM_B_Reject_Cnt" SortExpression="CDUM_B_Reject_Cnt" ReadOnly="True"
									HeaderText="CDUM_B_Reject_Cnt" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="CDUM_H_Reject_Cnt" SortExpression="CDUM_H_Reject_Cnt" ReadOnly="True"
									HeaderText="CDUM_H_Reject_Cnt" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="CDUM_Upload_Status" SortExpression="CDUM_Upload_Status" ReadOnly="True"
									HeaderText="CDUM_Upload_Status" Visible="False"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
