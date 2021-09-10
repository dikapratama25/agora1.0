<%@ Page Language="vb" AutoEventWireup="false" Codebehind="invoiceView.aspx.vb" Inherits="eProcure.invoiceView1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>invoice</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<%Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
				
		function Reset()
		{
			var oform = document.forms(0);					
			oform.txt_po_no.value="";
			oform.txt_CRNO.value="";
			oform.chk_New.checked=false;
			oform.chk_Pending.checked=false;
			oform.chk_Approved.checked=false;
			oform.chk_paid.checked=false;
		}
		
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}		
		
	
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchGInv_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				 <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			    </TR>
			    <TR>
				    <TD colSpan="4" >
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Fill in the search criteria and click Search button to list the relevant Invoice."
					    ></asp:label>

				    </TD>
			    </TR>
                <tr>
					    <TD class="linespacing2" colSpan="4"></TD>
			    </TR>
				<%--<TR>--%>
					<%--<TD>
						<TABLE class="alltable" id="Table4" width="100%" cellSpacing="0" cellPadding="0" border="0" runat="server" style="width: 100%">--%>
							<TR>
								<TD class="tableheader" style="height: 19px;" colspan="6">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tableCOL" align="left" width="10%">&nbsp;<STRONG><asp:Label ID="Label1" runat="server" Text ="Invoice No. :"></asp:Label></STRONG></td>
								<td class="TableInput" width="20%"><asp:textbox id="txt_po_no" runat="server" CssClass="TXTBOX" ></asp:textbox></td>
								
								<td class="tableCOL"  align="left" width="10%"><STRONG><asp:Label ID="Label2" runat="server" Text = "Buyer Company :" ></asp:Label></STRONG></td>
								<td class="TableInput"  width="30%"><asp:textbox id="txt_CRNO" runat="server" CssClass="TXTBOX" ></asp:textbox><STRONG>&nbsp;</STRONG></TD>
								
							</TR>
							<TR>
					<td class="tablecol" colSpan="6" >
							<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" border="0" runat="server" width="100%">
							<tr class="tablecol">
								<TD class="tableCOL" align="left" width="13%"><STRONG><asp:Label ID="Label11" runat="server" Text="Status :"></asp:Label></STRONG></td>
							
									<td><asp:checkbox id="chk_New" runat="server" Text="New "></asp:checkbox></td>
									<td><asp:checkbox id="chk_Pending" runat="server" Text="Pending Approval"></asp:checkbox></td>
									<td><asp:checkbox id="chk_Approved" runat="server" Text="Approved"></asp:checkbox></td>
									<td><asp:checkbox id="chk_paid" runat="server" Text="Paid"></asp:checkbox></td>
									
								<td class="tableCOL" align="right" ><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button><STRONG>&nbsp;</STRONG><INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"></td>
								
								
							</TR>
							<tr>
										</tr>
					</TABLE>
					</TD>
				</TR>
				<tr>
					<TD colSpan="3"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
							ShowSummary="False"></asp:validationsummary></TD>
				</tr>
				<%--<TR>
					<TD class="emptycol" style="width: 793px"></TD>
				</TR>--%>
				<TR>
					<TD>
						<%--<TABLE class="AllTable" id="tblSearchResult" cellSpacing="0" cellPadding="0" border="0"
							runat="server" width="100%">--%>
						<TR>
								<TD class="emptycol" colSpan="5" ></TD>
							</TR>
							<TR>
								<TD colSpan="5" ><asp:datagrid id="dtg_InvList" runat="server" OnPageIndexChanged="dtg_InvList_Page" AllowSorting="True"
										AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
										<Columns>
											<asp:BoundColumn SortExpression="IM_INVOICE_NO" HeaderText="Invoice Number">
												<HeaderStyle HorizontalAlign="Left" Width="18%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn SortExpression="IM_CREATED_ON" HeaderText="Creation Date">
												<HeaderStyle HorizontalAlign="Left" Width="11%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn SortExpression="POM_PO_NO" HeaderText="PO Number">
												<HeaderStyle HorizontalAlign="Left" Width="18%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Buyer Company">
												<HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="POM_CURRENCY_CODE" SortExpression="POM_CURRENCY_CODE" HeaderText="Currency">
												<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn SortExpression="IM_INVOICE_TOTAL" HeaderText="Amount">
												<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status">
												<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
										</Columns>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="5" style="width: 793px"></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="5" style="width: 793px"><asp:button id="cmd_createInv" runat="server" CssClass="button" Text="Create Invoice" Width="96px" Visible="False"></asp:button></TD>
							</TR>
						<%--</TABLE>--%>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
