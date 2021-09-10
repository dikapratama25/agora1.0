<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConfigDefValue.aspx.vb" Inherits="eProcure.ConfigDefValue" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ConfigRefValue</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
         <% Response.Write(css)%>   
		<script language="javascript">
		
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ConfigDef_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
			<tr><td class="rowspacing"></td></tr>
			<TR>
				<TD class="EmptyCol" colspan="2">
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="To configure default setting, choose an option and click Save button."
					></asp:label>

				</TD>
			</TR>
			<tr><td class="rowspacing"></td></tr>
			<TR>
				<TD class="TableHeader" colSpan="2">Search Criteria</TD>
			</TR>
			<TR class="tablecol">
				<TD class="TableCol" style="width:20%;">
                    <asp:Label ID="Label1" runat="server" Text="Configure for:" CssClass="lbl"></asp:Label>
                    </td>
                    <td class="TableCol">
					<asp:dropdownlist id="ddl_Select" runat="server" Width="200px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="True">
					</asp:dropdownlist></TD>
<%--									<asp:ListItem Value="C">Custom Field</asp:ListItem> 
					</asp:dropdownlist></TD> --%>
			</TR>
			</table>
			<div id="divCustom" runat="server" style="display:none;">
			<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="100%">
			<TR class="tablecol">
			
									<TD class="tablecol" style="width:20%;">
                                        <asp:Label ID="Label2" runat="server" Text="Module:" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td class="TableCol">
										<asp:dropdownlist id="ddl_Module" runat="server" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="True" Width="200px"></asp:dropdownlist></TD>
								</TR>
								<tr>
								<td class="tablecol">
                                    <asp:Label ID="Label3" runat="server" Text="Custom Field Name:" CssClass="lbl"></asp:Label></td>
								<td class="tablecol">
                                    <asp:DropDownList ID="ddl_Custom" CssClass="ddl" style="margin-bottom:1px;" runat="server" AutoPostBack="True" Width="200px">
                                    </asp:DropDownList>
								</td>
								
								</tr>
								</TABLE>
				</div>
			<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="100%">
				<TR>
					<TD class="EmptyCol" colspan="2">
						<div id="addr" style="DISPLAY: none" runat="server">
						<%--This is how to make rowspacing inside TD tag--%>
						<div class="rowspacing"></div>
							<asp:datagrid id="dtg_Address" runat="server" AutoGenerateColumns="False" OnPageIndexChanged="dtg_Address_Page"
								OnSortCommand="SortCommand_Click">
								<Columns>
									<asp:TemplateColumn>
										<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<ItemTemplate>
											<asp:Label id="lblAddr" Runat="server"></asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="AM_Addr_Code" SortExpression="AM_Addr_Code"  readonly="true"    HeaderText="Code">
										<HeaderStyle Width="10%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Address" SortExpression="Address"  readonly="true"    HeaderText="Address">
										<HeaderStyle Width="30%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="AM_CITY" SortExpression="AM_CITY"  readonly="true"    HeaderText="City">
										<HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="STATE" SortExpression="STATE"  readonly="true"    HeaderText="State">
										<HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="AM_POSTCODE" SortExpression="AM_POSTCODE"  readonly="true"    HeaderText="Post Code">
										<HeaderStyle Width="12%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="COUNTRY" SortExpression="COUNTRY"  readonly="true"    HeaderText="Country">
										<HeaderStyle Width="13%"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
							</asp:datagrid>												
						</div>
						<div id="loc" style="DISPLAY: none" runat="server">
						<%--This is how to make rowspacing inside TD tag--%>
						    <div class="rowspacing"></div>	
							<asp:datagrid id="dtgLocation" runat="server" OnSortCommand="SortCommandLocation_Click" DataKeyField="LM_LOCATION_INDEX">
							    <Columns>
								    <asp:TemplateColumn>
										<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<ItemTemplate>
											<asp:Label id="lblLoc" Runat="server"></asp:Label>
										</ItemTemplate>
									</asp:TemplateColumn>
								    <asp:BoundColumn Visible="False" DataField="LM_LOCATION_INDEX" SortExpression="LM_LOCATION_INDEX" HeaderText="LM_LOCATION_INDEX">
									    <HeaderStyle Width="15%"></HeaderStyle>
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="LM_LOCATION" SortExpression="LM_LOCATION" HeaderText="Location">
									    <HeaderStyle Width="40%"></HeaderStyle>
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="LM_SUB_LOCATION" SortExpression="LM_SUB_LOCATION" HeaderText="Sub Location">
									    <HeaderStyle Width="40%"></HeaderStyle>
								    </asp:BoundColumn>
							    </Columns>
						    </asp:datagrid>							
						</div>
						<div id="customfield" style="DISPLAY: none" runat="server">
						<div class="rowspacing"></div>	
						<asp:datagrid id="dtg_Custom" runat="server" AutoGenerateColumns="False" OnPageIndexChanged="dtg_Custom_Page"
									OnSortCommand="SortCommandCustom_Click">
									<Columns>
										<asp:TemplateColumn>
											<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
											<ItemStyle HorizontalAlign="Center"></ItemStyle>
											<ItemTemplate>
												<asp:Label id="lblCustom" Runat="server"></asp:Label>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="CF_FIELD_VALUE" SortExpression="CF_FIELD_VALUE"  readonly="true"    HeaderText="Custom Field Values">
											<HeaderStyle Width="95%"></HeaderStyle>
										</asp:BoundColumn>
									</Columns>
								</asp:datagrid>
								</div> 
					</TD>
				</TR>
				<TR>
					<TD class="EmptyCol"><br>
						<asp:button id="cmd_Save" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<INPUT class="button" id="cmd_Reset2" type="reset" value="Reset" name="Reset1" runat="server"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
