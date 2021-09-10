<%@ Page Language="vb" AutoEventWireup="false" Codebehind="POApprovalSetup.aspx.vb" Inherits="eProcure.POApprovalSetup" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Approval Setup</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<% Response.Write(Session("WheelScript"))%>
	</HEAD>
	<body onload="" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_AddPOAppr_tabs"))%>
			<TABLE class="alltable" id="Table1" width="100%" cellSpacing="0" cellPadding="0" border="0">
            <tr>
					<TD class="linespacing1" colSpan="2" ></TD>
			</TR>
        		<TR vAlign="top">
					<TD class="header" colspan="2" ><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></TD>
				</TR>
			<tr>
					<TD class="linespacing2" colSpan="2" ></TD>
            </tr>
				<TR>
					<TD align="left" colSpan="2" >
						<asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Please select the approval workflow from the approval list and submit the PO."></asp:label>
                        
					</TD>
				</TR>
            <tr>
					<TD class="linespacing2" colSpan="6" ></TD>
			</TR>
				<TR vAlign="top">
					<TD class="TableHeader" colspan="2">&nbsp;Approval Setup Header</TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol" align="left" width="20%" nowrap>&nbsp;<STRONG>Purchase Order
							No.</STRONG>&nbsp;:</TD>
					<TD class="TableInput" width="80%"><asp:label id="lblPO" runat="server" Width="290px"></asp:label></TD>
				</TR>
                <%--Jules 2018.10.26 - U00010--%> 
                    <TR vAlign="top" id="trPRDetail1" runat="server">
					    <TD class="tablecol" align="left" width="20%" nowrap>&nbsp;<STRONG>Requester ID</STRONG>&nbsp;:</TD>
					    <TD class="TableInput" width="80%"><asp:label id="lblRequesterID" runat="server" Width="290px"></asp:label></TD>
				    </TR>
                    <TR vAlign="top" id="trPRDetail2" runat="server">
					    <TD class="tablecol" align="left" width="20%" nowrap>&nbsp;<STRONG>Requester Name</STRONG>&nbsp;:</TD>
					    <TD class="TableInput" width="80%"><asp:label id="lblRequesterName" runat="server" Width="290px"></asp:label></TD>
				    </TR>
                    <TR vAlign="top" id="trPRDetail3" runat="server">
					    <TD class="tablecol" align="left" width="20%" nowrap>&nbsp;<STRONG>PR Cost Centre</STRONG>&nbsp;:</TD>
					    <TD class="TableInput" width="80%"><asp:label id="lblPRCostCentre" runat="server" Width="290px"></asp:label></TD>
				    </TR>
                    <TR vAlign="top" id="trPRDetail4" runat="server">
					    <TD class="tablecol" align="left" width="20%" nowrap>&nbsp;<STRONG>PR Approval Group Name</STRONG>&nbsp;:</TD>
					    <TD class="TableInput" width="80%"><asp:label id="lblPRGApprovalGrp" runat="server" Width="290px"></asp:label></TD>
				    </TR>                
                <%--End modification--%>


				<TR vAlign="top">
					<TD class="tablecol" nowrap>&nbsp;<STRONG>Approval List</STRONG><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
					<TD class="TableInput"><asp:dropdownlist id="cboApproval" runat="server" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist>
						<asp:RequiredFieldValidator id="rfvApproval" runat="server" ErrorMessage="Approval List is required." ControlToValidate="cboApproval"
							Display="None"></asp:RequiredFieldValidator><asp:label id="lblMsg" runat="server" Width="100%"></asp:label></TD>
				</TR>
				<TR class="emptycol">
					<TD colspan="4"><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2">&nbsp;&nbsp;&nbsp;</TD>
				</TR>
				<tr>
					<td colSpan="2"><asp:datagrid id="dtgAO" runat="server" OnPageIndexChanged="dtgAO_Page" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="AGA_SEQ"  readonly="true"   HeaderText="Level">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="AGA_AO"  readonly="true"   HeaderText="Main AO"></asp:BoundColumn>
								<asp:BoundColumn DataField="AO_NAME"  readonly="true"   HeaderText="Main AO">
									<HeaderStyle Width="40%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="AGA_A_AO"  readonly="true"   HeaderText="Alternative AO"></asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME"  readonly="true"   HeaderText="Alternative AO">
									<HeaderStyle Width="40%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Type*">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label ID="lblType" Runat="server"></asp:Label>
										<asp:Label ID="lblLimit" Runat="server" Visible="False"></asp:Label>
										<asp:Label ID="lblRelief" Runat="server" Visible="False"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<TR>
					<TD class="emptycol" colSpan="2">&nbsp;</TD>
				</TR>
				<TR vAlign="top" runat="server" id="trConsolidator">
					<TD class="tablecol" align="left" colspan="2">&nbsp;<STRONG>Consolidator</STRONG>&nbsp;:&nbsp;<asp:label id="lblConsolidator" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2" style="height: 19px">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"><asp:label id="lblRemark" runat="server" Width="100%"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD colSpan="2"><asp:button id="cmdSubmit" runat="server" CssClass="Button" Text="Submit"></asp:button></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="2"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary>
					</TD>
				</TR>
				<tr>
					<td colspan="2">&nbsp;</td>
				</tr>
				<TR>
					<TD class="emptycol" colSpan="2"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
