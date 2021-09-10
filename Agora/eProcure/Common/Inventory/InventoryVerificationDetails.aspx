<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InventoryVerificationDetails.aspx.vb" Inherits="eProcure.InventoryVerificationDetails" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>Inventory Verification Detail</title>
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
        <%  Response.Write(Session("w_InvList_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
			<tr><td class="rowspacing"></td></tr>
			<TR>
				<TD class="TableHeader" colSpan="4">
                    Status Detail</TD>
			</TR>
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label1" runat="server" Text="GRN Number :" CssClass="lbl"></asp:Label></strong></TD>
				<TD class="TableCol"  width="30%"><asp:Label ID="lblGRNNo" runat="server" CssClass="lbl" Font-Bold="false"></asp:Label></TD>

				<TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label2" runat="server" Text="Vendor :" CssClass="lbl"></asp:Label></strong></TD>
				<TD class="TableCol" width="30%"><asp:Label ID="lblVendor" runat="server" CssClass="lbl" Font-Bold="false"></asp:Label></TD>
			</TR>
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label3" runat="server" Text="GRN Date :" CssClass="lbl"></asp:Label></strong></TD>
				<TD class="TableCol" width="30%"><asp:Label ID="lblGRNDate" runat="server" CssClass="lbl" Font-Bold="false"></asp:Label></TD>

				<TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label5" runat="server" Text="Actual Received Date :" CssClass="lbl"></asp:Label></strong></TD>
				<TD class="TableCol" width="30%"><asp:Label ID="lblReceivedDate" runat="server" CssClass="lbl" Font-Bold="false"></asp:Label></TD>
			</TR>
			
				<TR>
					<TD class="EmptyCol" colspan="4">
						    <asp:datagrid id="dtgInv" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
						        <Columns>							       
							        <asp:BoundColumn DataField="IM_ITEM_CODE" SortExpression="IM_ITEM_CODE" HeaderText="Item Code">
								        <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							        </asp:BoundColumn>
							        <asp:BoundColumn DataField="IM_INVENTORY_NAME" SortExpression="IM_INVENTORY_NAME" HeaderText="Item Name">
									    <HeaderStyle HorizontalAlign="Left" Width="18%" ></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="LM_LOCATION" SortExpression="LM_LOCATION" HeaderText="Location">
									    <HeaderStyle HorizontalAlign="Left" Width="10%" ></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
								    </asp:BoundColumn>
							        <asp:BoundColumn DataField="LM_SUB_LOCATION" SortExpression="LM_SUB_LOCATION" HeaderText="Sub Location">
								        <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							        </asp:BoundColumn>
							        <asp:BoundColumn DataField="IV_RECEIVE_QTY" SortExpression="IV_RECEIVE_QTY" HeaderText="Received Qty">
								        <HeaderStyle HorizontalAlign="Right" Width="8%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
							        </asp:BoundColumn>	
							        <asp:BoundColumn HeaderText="Passed Qty">
								        <HeaderStyle HorizontalAlign="Right" Width="7%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
							        </asp:BoundColumn>	
							        <asp:BoundColumn HeaderText="Failed Qty">
								        <HeaderStyle HorizontalAlign="Right" Width="7%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
							        </asp:BoundColumn>	
							        <asp:TemplateColumn HeaderText="Pass Qty">
									    <HeaderStyle HorizontalAlign="Right" Width="9%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									    <ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
									    <ItemTemplate>
										    <asp:textbox ID="txtPassQty" Runat="server" cssclass="numerictxtbox" Width="90%"></asp:textbox>
										    <asp:RegularExpressionValidator id="revPassQty" runat="server" ControlToValidate="txtPassQty" Display="Dynamic" ></asp:RegularExpressionValidator>
									    </ItemTemplate>
								    </asp:TemplateColumn>
								    <asp:TemplateColumn HeaderText="Fail Qty">
									    <HeaderStyle HorizontalAlign="Right" Width="10%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									    <ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
									    <ItemTemplate>
										    <asp:textbox ID="txtFailQty" Runat="server" cssclass="numerictxtbox" Width="85%"></asp:textbox>
										    <asp:RegularExpressionValidator id="revFailQty" runat="server" ControlToValidate="txtFailQty" Display="Dynamic" ></asp:RegularExpressionValidator>
										    <asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" width="1%" ForeColor="Red"
											 contentEditable="false" Visible="false" ></asp:TextBox>
										    <INPUT class="txtbox" id="hidCode" type="hidden" runat="server" NAME="hidCode">
									    </ItemTemplate>									    
								    </asp:TemplateColumn>
							        <asp:BoundColumn Visible="False" DataField="IV_INVENTORY_INDEX"></asp:BoundColumn>	
							        <asp:BoundColumn Visible="False" DataField="IV_LOCATION_INDEX"></asp:BoundColumn>	
							        <asp:BoundColumn Visible="False" DataField="IV_VERIFY_INDEX"></asp:BoundColumn>	
							        
						        </Columns>
					        </asp:datagrid>																	
					</TD>
				</TR>
				<%-- Craven 07-04-2011 below have not been change--%>
				
				<TR>
					<TD class="EmptyCol"><br>
						<asp:button id="cmd_Save" runat="server" CssClass="button" Text="Save"></asp:button>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="4">&nbsp;&nbsp;
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary>
						<asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label>
						<INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary" runat="server">
						<INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl" runat="server">
					</TD>
				</TR>
				<tr><td class="rowspacing"></td></tr>
				<TR>
					<TD class="emptycol" style="height: 19px">
    				    <asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="#"><STRONG>&lt; Back</STRONG></asp:hyperlink>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
