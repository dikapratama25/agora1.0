<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PRConsolidation.aspx.vb" Inherits="eProcure.PRConsolidation" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PR Approval</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--	
		
		function submitOnClick(chk, cnt1, cnt2)
		{
			if (CheckAtLeastOne(chk)){
				if (resetSummary(cnt1,cnt2))
					return true;
				else
					return false;	
			}
			else
				return false;
		}	
			
		function resetOnClick()
		{	
			ValidatorReset();		
			//DeselectAllG('dtgPRList__ctl1_chkAll','chkSelection');
			
		}
		
		//For check in Datagrid
		function selectAll()
		{
			SelectAllG("dtgPRList_ctl01_chkAll","chkSelection");
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtgPRList_ctl01_chkAll","chkSelection");
		}
		
		function limitText (textObj, maxCharacters){
			//alert(event.keyCode);
            if (textObj.innerText.length >= maxCharacters){
            //alert(textObj.innerText);
            if ((event.keyCode >= 32 && event.keyCode <= 126) || (event.keyCode >= 128 && event.keyCode <= 254))
            {event.returnValue = false;}
         }}
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header"><FONT size="1">&nbsp;</FONT>Purchase Requisition Consolidation</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD>
						<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR id="tdConso" style="VISIBILITY: visible" runat="server">
								<TD class="emptycol"><STRONG>&nbsp;Consolidated By</STRONG>&nbsp;:&nbsp;
									<asp:dropdownlist id="cboConso" AutoPostBack="True" CssClass="ddl" Runat="server" Width="200px"></asp:dropdownlist></TD>
							</TR>
							<TR id="tblSearchResult" runat="server">
								<TD><asp:datagrid id="dtgPRList" runat="server" DataKeyField="PRM_PR_INDEX" OnSortCommand="SortCommand_Click">
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
											<asp:TemplateColumn HeaderText="PR Number" SortExpression="PRM_PR_NO">
												<HeaderStyle Width="20%"></HeaderStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="PRM_PR_Date" HeaderText="Creation Date" SortExpression="PRM_PR_Date">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRM_REQ_NAME" HeaderText="Buyer" SortExpression="PRM_REQ_NAME">
												<HeaderStyle Width="15%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRM_EXTERNAL_REMARK" HeaderText="Remarks" SortExpression="PRM_EXTERNAL_REMARK">
												<HeaderStyle Width="25%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRM_S_COY_NAME" HeaderText="Vendor Name" SortExpression="PRM_S_COY_NAME">
												<HeaderStyle Width="20%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRM_CURRENCY_CODE" HeaderText="Currency" SortExpression="PRM_CURRENCY_CODE">
												<HeaderStyle Width="5%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRM_PR_COST" HeaderText="Amount" SortExpression="PRM_PR_COST">
												<HeaderStyle Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PRM_FREIGHT_TERMS"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PRM_PAYMENT_TYPE"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PRM_SHIPMENT_TERM"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PRM_SHIPMENT_MODE"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PRM_PAYMENT_TERM"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PRM_SHIP_VIA"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PRM_B_ADDR_CODE"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PRM_PR_NO"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PRM_S_COY_ID"></asp:BoundColumn>
										</Columns>
									</asp:datagrid><br>
									<STRONG>Please key in remarks for PO</STRONG> :
									<br>
									<asp:textbox id="txtRemark" CssClass="listtxtbox" Runat="server" Width="600px" TextMode="MultiLine"
										MaxLength="1000" Rows="3"></asp:textbox></TD>
							</TR>
							<TR id="trMessage" runat="server">
								<TD><asp:label id="lblMsg" CssClass="ErrorMsg" Runat="server">&nbsp;No outstanding Purchase Requisitions for consolidation.</asp:label></TD>
							</TR>
							<TR>
								<TD class="emptycol">&nbsp;&nbsp;</TD>
							</TR>
							<TR>
								<TD><asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;
									<asp:button id="cmdPreview" runat="server" CssClass="button" Text="View PO"></asp:button>&nbsp;
									<INPUT class="button" id="cmdReset" onclick="ValidatorReset();" type="button" value="Reset"
										name="cmdReset" runat="server">
								</TD>
							</TR>
							<TR>
								<TD class="emptycol">&nbsp;&nbsp;
									<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
										runat="server"><INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
										runat="server"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
