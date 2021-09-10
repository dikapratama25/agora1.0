<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PRTemplate.aspx.vb" Inherits="eProcure.PRTemplate" %>
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
		
		function selectAll()
		{
			SelectAllG("dtgProduct_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgProduct_ctl02_chkAll","chkSelection");
		}
		
		function Test(f)
		{
			alert(f.name);
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
			<asp:Repeater Runat="server" ID="pr">
				<ItemTemplate>
					<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
						<TR>
							<TD class="header"><FONT size="1">&nbsp;</FONT>Purchase Requisition Approval</TD>
						</TR>
						<TR>
							<TD class="emptycol"></TD>
						</TR>
						<TR>
							<TD>
								<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
									<TR>
										<TD class="tableheader" colSpan="4">&nbsp;Purchase Requisition Header</TD>
									</TR>
									<TR>
										<TD class="tablecol" width="25%">&nbsp;<STRONG>Purchase Requisition Number</STRONG>&nbsp;:</TD>
										<TD class="TableInput" width="25%" colSpan="3">
											<asp:label id="lblPR" Runat="server">PR001</asp:label></TD>
									</TR>
									<TR>
										<TD class="tablecol" width="25%">&nbsp;<STRONG>Requestor Name</STRONG>&nbsp;:</TD>
										<TD class="TableInput" width="25%" colSpan="3">
											<asp:label id="lblReqName" Runat="server">MOO</asp:label></TD>
									<TR>
										<TD class="tablecol" vAlign="top" width="25%">&nbsp;<STRONG>Billing Address</STRONG>&nbsp;:</TD>
										<TD class="TableInput" width="25%" colSpan="3">
											<asp:label id="lblBillAddr" Runat="server">
								12, Jalan 13/4(Bersatu)<br>
								46200 Petaling Jaya<br>
								Selangor Malaysia<br>
								</asp:label></TD>
									</TR>
									<TR>
										<TD class="tablecol" vAlign="top" width="25%">&nbsp;<STRONG>For Internal Use</STRONG>&nbsp;:</TD>
										<TD class="TableInput" width="25%" colSpan="3">
											<asp:label id="lblInternalRemark" Runat="server">
								TEST TEST TEST TEST TEST<br>
								TEST TEST TEST TEST TEST<br>
								</asp:label></TD>
									</TR>
									<TR>
										<TD class="tablecol" width="25%">&nbsp;<STRONG>Vendor</STRONG>&nbsp;:</TD>
										<TD class="TableInput" width="25%" colSpan="3">
											<asp:label id="lblVendor" Runat="server">Kompakar Group of companies</asp:label></TD>
									</TR>
									<TR>
										<TD class="tablecol" vAlign="top" width="25%">&nbsp;<STRONG>Remarks</STRONG>&nbsp;:</TD>
										<TD class="TableInput" width="25%" colSpan="3">
											<asp:label id="lblPRRemark" Runat="server">
								TEST TEST TEST TEST TEST<br>
								TEST TEST TEST TEST TEST<br>
								</asp:label></TD>
									</TR>
									<TR>
										<TD class="tablecol" width="25%">&nbsp;<STRONG>Currency</STRONG>&nbsp;:</TD>
										<TD class="TableInput" width="25%">
											<asp:label id="lblCurrency" Runat="server">MOO</asp:label></TD>
										<TD class="tablecol" width="25%">&nbsp;<STRONG>Exchange Rate</STRONG>&nbsp;:</TD>
										<TD class="TableInput">
											<asp:label id="lblExRate" Runat="server">MOO</asp:label></TD>
									</TR>
									<TR>
										<TD class="tablecol" width="25%">&nbsp;<STRONG>Payment Terms</STRONG>&nbsp;:</TD>
										<TD class="TableInput" width="25%">
											<asp:label id="lblPT" Runat="server">MOO</asp:label></TD>
										<TD class="tablecol" width="25%">&nbsp;<STRONG>Payment Type</STRONG>&nbsp;:</TD>
										<TD class="TableInput">
											<asp:label id="lblPM" Runat="server">MOO</asp:label></TD>
									</TR>
									<TR>
										<TD class="tablecol" width="25%">&nbsp;<STRONG>Shipment Terms</STRONG>&nbsp;:</TD>
										<TD class="TableInput" width="25%">
											<asp:label id="lblST" Runat="server">MOO</asp:label></TD>
										<TD class="tablecol" width="25%">&nbsp;<STRONG>Shipment Mode</STRONG>&nbsp;:</TD>
										<TD class="TableInput">
											<asp:label id="lblSM" Runat="server">MOO</asp:label></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
						<TR>
							<TD class="emptycol"></TD>
						</TR>
						<TR>
							<TD>
								<TABLE class="AllTable" id="tblSearchResult" cellSpacing="0" cellPadding="0" border="0" runat="server">
									<TR>
										<TD class="emptycol" colSpan="2"></TD>
									</TR>
									<TR>
										<TD colSpan="2">
											<asp:datagrid id="dtgPRList" runat="server">
												<Columns>
													<asp:BoundColumn DataField="PRD_PR_LINE" HeaderText="Line"></asp:BoundColumn>
													<asp:BoundColumn DataField="PRD_VENDOR_ITEM_CODE" HeaderText="Vendor Item Code"></asp:BoundColumn>
													<asp:BoundColumn DataField="PRD_PRODUCT_DESC" HeaderText="Product Description"></asp:BoundColumn>
													<asp:TemplateColumn HeaderText="MOQ">
														<ItemTemplate>
															<asp:Label runat="server" Text='1' ID="Label1" NAME="Label1"></asp:Label>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="MPQ">
														<ItemTemplate>
															<asp:Label runat="server" Text='1' ID="Label2" NAME="Label2"></asp:Label>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="PRD_ORDERED_QTY" HeaderText="Qty."></asp:BoundColumn>
													<asp:BoundColumn DataField="PRD_UOM" HeaderText="UOM"></asp:BoundColumn>
													<asp:BoundColumn DataField="PRD_UNIT_COST" HeaderText="Unit Cost"></asp:BoundColumn>
													<asp:BoundColumn DataField="PRD_UNIT_COST" HeaderText="Sub Total"></asp:BoundColumn>
													<asp:BoundColumn DataField="PRD_PR_LINE" HeaderText="Budget Account"></asp:BoundColumn>
													<asp:BoundColumn DataField="PRD_PR_LINE" HeaderText="Delivery Address"></asp:BoundColumn>
													<asp:BoundColumn DataField="PRD_PR_LINE" HeaderText="Est. Date of Delivery (days"></asp:BoundColumn>
													<asp:BoundColumn DataField="PRD_PR_LINE" HeaderText="Warranty Terms (mths)"></asp:BoundColumn>
													<asp:BoundColumn DataField="PRD_PR_LINE" HeaderText="Remark"></asp:BoundColumn>
												</Columns>
											</asp:datagrid></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
					<br>
					<br>
					<br>
				</ItemTemplate>
			</asp:Repeater>
		</form>
	</body>
</HTML>
