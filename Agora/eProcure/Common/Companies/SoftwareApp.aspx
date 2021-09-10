<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SoftwareApp.aspx.vb" Inherits="eProcure.SoftwareApp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ApprovalWorkFlow</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
		<%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--  
		function selectAll()
		{
			SelectAllG("dtgApp_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgApp_ctl02_chkAll","chkSelection");
		}

		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_SoftWare_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
            <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			</TR>
			<TR>
				<TD >
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Please update the Software application(s) that you use in your company.</br>Fill in the search criteria and click Search button to list the relevant software applications. Click the Add button to add new software application."
					></asp:label>

				</TD>
			</TR>
            <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
				<TR>
					<TD class="tablecol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="header" colSpan="3" width="100%"></TD>
				</TR>
							<TR>
								<TD class="tableheader" colspan="3" width="100%">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol" width="20%" >&nbsp;<STRONG>Software Application </STRONG>:</TD>
								<td class="tablecol" width="60%" ><asp:textbox id="txtSWSearch" runat="server" MaxLength="255" CssClass="txtbox" Width="357px" ></asp:textbox>&nbsp;&nbsp;
								</td>
								<td class="tablecol" width="20%" align="right">
									<asp:button id="cmd_search" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_clear1" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
								</td>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
			</TABLE>
			<DIV id="Hide_Add2" style="DISPLAY: none" runat="server">
				<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
					<TR>
						<TD class="tableheader" colspan="2" >&nbsp;Please
							<asp:label id="lbl_add_mod" Runat="server"></asp:label>&nbsp;the following 
							value.
						</TD>
					</TR>
					<TR class="tablecol">
						<TD width="75%">&nbsp;<STRONG>Software Application</STRONG><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:textbox id="txt_add_mod" runat="server" MaxLength="255" Width="356px" CssClass="txtbox"></asp:textbox>&nbsp;</td>
						<td align="right" width="25%">
							<asp:button id="cmd_save" runat="server" CssClass="button" Text="Save"></asp:button>
							<asp:button id="cmd_clear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
							<asp:button id="Button1" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"></asp:button></TD>
					</TR>
					<TR>
						<TD class="emptycol" colspan="2"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
							required field
							<asp:requiredfieldvalidator id="validate_grp_name" runat="server" ControlToValidate="txt_add_mod" ErrorMessage="Software Application is required."
								Display="None"></asp:requiredfieldvalidator></TD>
					</TR>
					<TR>
						<TD colspan="2"><br>
							<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary></TD>
					</TR>
					<TR>
						<TD class="emptycol" colspan="2"></TD>
					</TR>
				</TABLE>
			</DIV>
			<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="emptycol" colspan="">
						<P><asp:datagrid id="dtgApp" runat="server" OnPageIndexChanged="MyDataGrid_Page" AllowSorting="True"
								OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
								<Columns>
									<asp:TemplateColumn HeaderText="Delete">
										<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<HeaderTemplate>
											<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
											<!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
										</HeaderTemplate>
										<ItemTemplate>
											<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="CSW_DESCRIPTION" SortExpression="CSW_DESCRIPTION"  readonly="true"  HeaderText="Software Application">
										<HeaderStyle Width="100%"></HeaderStyle>
										<ItemStyle></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="CSW_INDEX"></asp:BoundColumn>
								</Columns>
							</asp:datagrid></P>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" Width="59px" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_delete" runat="server" CssClass="button" Text="Delete" CausesValidation="False"></asp:button>&nbsp;<INPUT class="button" id="cmd_Reset" style="DISPLAY: none" onclick="DeselectAllG('dtgApp_ctl02_chkAll','chkSelection')"
							type="button" value="Reset" name="cmd_Reset" runat="server"> <INPUT id="hidMode" type="hidden" size="1" name="hidMode" runat="server"><INPUT id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server">&nbsp;
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
