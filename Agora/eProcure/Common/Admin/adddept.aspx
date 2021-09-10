<%@ Page Language="vb" AutoEventWireup="false" Codebehind="adddept.aspx.vb" Inherits="eProcure.adddept" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Add/Modify Department</title>
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
	function Reset(){
			var oform = document.forms(0);
			oform.txt_deptName.value="";
			oform.txt_deptCode.value="";
				}
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_AddDept_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
           <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			</TR>
			
				<TR>
	                <TD colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the relevant info and click the Save button to save the department."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="6"></TD>
			    </TR>
				<TR>
					<TD vAlign="middle" align="left">
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tableheader" colSpan="2">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 7px"></TD>
								<td class="TableInput" style="HEIGHT: 7px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 140px; HEIGHT: 19px" width="140">&nbsp;<STRONG>Department 
										Code</STRONG><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="TableInput" style="HEIGHT: 19px">&nbsp;
									<asp:textbox id="txt_deptCode" runat="server" CssClass="txtbox" MaxLength="10" Width="112px"></asp:textbox><asp:requiredfieldvalidator id="vldDeptCode" runat="server" Display="None" ControlToValidate="txt_deptCode"
										ErrorMessage="Department Code Required"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 140px; HEIGHT: 17px" width="140">&nbsp;<STRONG>Department 
										Name</STRONG><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
								<TD class="TableInput" style="HEIGHT: 17px">&nbsp;
									<asp:textbox id="txt_deptName" runat="server" CssClass="txtbox" MaxLength="50" Width="361px"></asp:textbox>&nbsp;
									<asp:requiredfieldvalidator id="vldDeptName" runat="server" Display="None" ControlToValidate="txt_deptName"
										ErrorMessage="Department Name Required"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol" noWrap>&nbsp;<STRONG>Approval List For</STRONG>&nbsp;:</TD>
								<TD class="TableInput">&nbsp;
									<asp:dropdownlist id="cboApprovalType" runat="server" CssClass="ddl" AutoPostBack="True">
                                        <asp:ListItem>Invoice</asp:ListItem>
                                        <%--<asp:ListItem>IPP</asp:ListItem>--%>
                                    </asp:dropdownlist><asp:label id="Label5" runat="server" Width="100%"></asp:label></TD>
							</TR>							
							<TR id="hidrow" runat="server">
								<TD class="tablecol" noWrap>&nbsp;<STRONG>Invoice Approval List</STRONG>&nbsp;:</TD>
								<TD class="TableInput">&nbsp;
									<asp:dropdownlist id="cboApproval" runat="server" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist><asp:label id="lblMsg" runat="server" Width="100%"></asp:label></TD>
							</TR>
							<TR id="TRIPP" runat="server" style="display:none ">
								<TD class="tablecol" noWrap>&nbsp;<STRONG>IPP Approval List</STRONG>&nbsp;:</TD>
								<TD class="TableInput">&nbsp;
									<asp:dropdownlist id="cboIPPApproval" runat="server" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist><asp:label id="lblMsg1" runat="server" Width="100%"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 7px"></TD>
								<td class="TableInput" style="HEIGHT: 7px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
							</TR>
							<TR class="emptycol">
								<TD style="HEIGHT: 7px"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
									required field</TD>
								<td style="HEIGHT: 7px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="2">&nbsp;&nbsp;&nbsp;</TD>
							</TR>
                           <%--Zulham 16072018 - PAMB--%>
                            <tr id="hidApprFlow" style="width:100%; cursor:pointer;" class="tableHeader" onclick="showHide1('ApprFlow')">
						                <td valign="top" class="tableHeader" colspan="2">	
					                    Resident Approval Flow
					                    </td>
					                    </tr>
				                <%--<div id="ApprFlow" style="display:inline"  />--%>
							<tr>
                            <%--'End--%>
                            <tr>
								<td colSpan="2">
                                    <%--<asp:datagrid id="dtgAO" runat="server" AutoGenerateColumns="False" OnPageIndexChanged="dtgAO_Page">
										<Columns>
											<asp:BoundColumn DataField="AGA_SEQ" readonly="true"    HeaderText="Level">
												<HeaderStyle Width="5%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="AGA_AO"  readonly="true"    HeaderText="Main AO"></asp:BoundColumn>
											<asp:BoundColumn DataField="AO_NAME"  readonly="true"    HeaderText="Main AO">
												<HeaderStyle Width="20%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="AGA_A_AO"  readonly="true"    HeaderText="Alternative AO"></asp:BoundColumn>
											<asp:BoundColumn DataField="AAO_NAME"  readonly="true"    HeaderText="Alternative AO">
												<HeaderStyle Width="20%"></HeaderStyle>
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
									</asp:datagrid>--%>
                                    <%--Zulham 11102018 - PAMB SST--%>
                                    <asp:datagrid id="dtgAO" runat="server" AutoGenerateColumns="False" OnPageIndexChanged="dtgAO_Page">
										<Columns>
											<asp:BoundColumn DataField="agm_grp_name" readonly="true"  HeaderText="Approval Group">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn HeaderText="FO">
                                                <HeaderStyle Width="15%"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFO" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
											 <asp:TemplateColumn HeaderText="FM" ItemStyle-VerticalAlign="Top" >
                                                <HeaderStyle Width="15%"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFM" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Type*">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemTemplate>
													<asp:Label ID="lblType" Runat="server"></asp:Label>
													<asp:Label ID="lblLimit" Runat="server" Visible="False"></asp:Label>
													<asp:Label ID="lblRelief" Runat="server" Visible="False"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
                                            <asp:BoundColumn HeaderText="GRP Index" DataField="AGM_GRP_INDEX"></asp:BoundColumn>
										</Columns>
									</asp:datagrid>
								</td>
							</tr>
                            <%--Zulham 16072018 - PAMB--%>
                            <TR>
								<TD class="emptycol" colSpan="2">&nbsp;&nbsp;&nbsp;</TD>
							</TR>
                            <tr id="hidApprFlow_NR" style="width:100%; cursor:pointer;" class="tableHeader" onclick="showHide1('ApprFlow')">
						                <td valign="top" class="tableHeader" colspan="2">	
					                    Non-Resident Approval Flow
					                    </td>
					                    </tr>
				                <%--<div id="ApprFlow" style="display:inline"  />--%>
							<tr>
                            <tr>
                               <%--<td colSpan="2"><asp:datagrid id="dtgAO_NR" runat="server" AutoGenerateColumns="False" OnPageIndexChanged="dtgAO_Page">
										<Columns>
											<asp:BoundColumn DataField="AGA_SEQ" readonly="true"    HeaderText="Level">
												<HeaderStyle Width="5%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="AGA_AO"  readonly="true"    HeaderText="Main AO"></asp:BoundColumn>
											<asp:BoundColumn DataField="AO_NAME"  readonly="true"    HeaderText="Main AO">
												<HeaderStyle Width="40%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="AGA_A_AO"  readonly="true"    HeaderText="Alternative AO"></asp:BoundColumn>
											<asp:BoundColumn DataField="AAO_NAME"  readonly="true"    HeaderText="Alternative AO">
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
									</asp:datagrid>--%>
                                    <asp:datagrid id="dtgAO_NR" runat="server" AutoGenerateColumns="False" OnPageIndexChanged="dtgAO_Page">
										<Columns>
											<asp:BoundColumn DataField="agm_grp_name" readonly="true"  HeaderText="Approval Group">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn HeaderText="FO">
                                                <HeaderStyle Width="15%"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFO" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
											 <asp:TemplateColumn HeaderText="FM" ItemStyle-VerticalAlign="Top" >
                                                <HeaderStyle Width="15%"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFM" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Type*">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemTemplate>
													<asp:Label ID="lblType" Runat="server"></asp:Label>
													<asp:Label ID="lblLimit" Runat="server" Visible="False"></asp:Label>
													<asp:Label ID="lblRelief" Runat="server" Visible="False"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
                                            <asp:BoundColumn HeaderText="GRP Index" DataField="AGM_GRP_INDEX"></asp:BoundColumn>
										</Columns>
									</asp:datagrid>
								</td>
							</tr>
                            <%--End--%>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"><asp:label id="lblRemark" runat="server" Width="100%"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD align="left"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Visible="False"></asp:button>&nbsp;
					<INPUT class="button" id="cmdAdd" type="button" value="Add" name="cmdAdd" runat="server">&nbsp;
                    <INPUT class="button" id="cmdClear" onclick="ValidatorReset()" type="button" value="Reset"
							name="cmdClear" runat="server"> 
						<!--	
					<INPUT class="button" id="cmdReset1" type="reset" value="Clear" runat="server">&nbsp;
					
						<INPUT class="button" id="cmdReset" type="button" value="Clear" runat="server" onclick="Form1.reset();">&nbsp;
					--></TD>
				</TR>
				<TR>
					<TD><br>
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
