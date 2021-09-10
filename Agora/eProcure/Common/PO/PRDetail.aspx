<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PRDetail.aspx.vb" Inherits="eProcure.ApprovePR" %>
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
function confirmReject()
{	
	result=resetSummary(1,0);	
	ans=confirm("Are you sure that you want to reject this PR ?");
	//alert(ans);
	//return false;	
	if (ans){			
		return result;
		}
	else
		return false;
}
function confirmApprove(action)
{	
	result=resetSummary(1,0);	
	ans=confirm("Are you sure that you want to " + action + " this PR ?");
	//alert(ans);
	//return false;	
	if (ans){			
		return result;
		}
	else
		return false;
}
//-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header"><FONT size="1">&nbsp;</FONT><asp:label id="lblTitle" runat="server">Purchase Requisition Approval</asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD style="height: 358px">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tableheader" colSpan="4">&nbsp;Purchase Requisition Header</TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" width="25%">&nbsp;<STRONG>Purchase Requisition Number</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="25%" colSpan="3"><asp:label id="lblPR" Runat="server">PR001</asp:label></TD>
							</TR>
							<TR id="trAdmin" vAlign="top" runat="server">
								<TD class="tablecol" width="25%">&nbsp;<STRONG>Requestor Name</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="25%" colSpan="3"><asp:label id="lblReqName" Runat="server">MOO</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" width="25%">&nbsp;<STRONG>Submission Date</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="25%" colSpan="3"><asp:label id="lblPRDate" Runat="server">MOO</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" width="25%">&nbsp;<STRONG>Status</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="25%" colSpan="3"><asp:label id="lblStatus" Runat="server"></asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" vAlign="top" width="25%">&nbsp;<STRONG>Billing Address</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="25%" colSpan="5"><asp:label id="lblBillAddr" Runat="server">
								12, Jalan 13/4(Bersatu)<br>
								46200 Petaling Jaya<br>
								Selangor Malaysia<br>
								</asp:label></TD>
							<TR vAlign="top">
								<TD class="tablecol" vAlign="top" width="25%">&nbsp;<STRONG>For Internal Use</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="25%" colSpan="3"><asp:label id="lblInternalRemark" Runat="server">
								TEST TEST TEST TEST TEST<br>
								TEST TEST TEST TEST TEST
								</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" width="25%">&nbsp;<STRONG>Vendor</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="25%" colSpan="3"><asp:label id="lblVendor" Runat="server">Kompakar Group of companies</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" vAlign="top" width="25%">&nbsp;<STRONG>Remarks</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="25%" colSpan="3"><asp:label id="lblPRRemark" Runat="server">
								TEST TEST TEST TEST TEST<br>
								TEST TEST TEST TEST TEST<br>
								</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" vAlign="top" width="25%">&nbsp;<STRONG>File(s) Attached</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="25%" colSpan="3"><asp:label id="lblFile" Runat="server"></asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" width="25%">&nbsp;<STRONG>Currency</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="25%"><asp:label id="lblCurrency" Runat="server">MOO</asp:label></TD>
								<TD class="tablecol" width="25%">&nbsp;<STRONG>Exchange Rate</STRONG>&nbsp;:</TD>
								<TD class="TableInput"><asp:label id="lblExRate" Runat="server">MOO</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" width="25%">&nbsp;<STRONG>Payment Terms</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="25%"><asp:label id="lblPT" Runat="server">MOO</asp:label></TD>
								<TD class="tablecol" width="25%">&nbsp;<STRONG>Payment&nbsp;Method</STRONG>&nbsp;:</TD>
								<TD class="TableInput"><asp:label id="lblPM" Runat="server">MOO</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD class="tablecol" width="25%">&nbsp;<STRONG>Shipment Terms</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="25%"><asp:label id="lblST" Runat="server">MOO</asp:label></TD>
								<TD class="tablecol" width="25%">&nbsp;<STRONG>Shipment Mode</STRONG>&nbsp;:</TD>
								<TD class="TableInput"><asp:label id="lblSM" Runat="server">MOO</asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tableheader">Approval Workflow</TD>
				</TR>
				<TR>
					<TD width="100%"><asp:datagrid id="dtgAppFlow" runat="server" AutoGenerateColumns="False" Width="100%">
							<Columns>
								<asp:BoundColumn DataField="PRA_SEQ" HeaderText="Level">
									<HeaderStyle Width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRA_APPROVAL_TYPE" HeaderText="Approval Type">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRA_ACTION_DATE" HeaderText="Action Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRA_AO_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Attachment">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemTemplate>
										<asp:DataList id=DataList1 runat="server" DataSource='<%# CreateFileLinks( DataBinder.Eval( Container.DataItem, "PRA_AO") , DataBinder.Eval( Container.DataItem, "PRA_A_AO") , DataBinder.Eval( Container.DataItem, "PRA_SEQ" ) ) %>' ShowFooter="False" Width="100%" BorderColor=#0000ff ShowHeader="False">
											<ItemTemplate>
												<%# DataBinder.Eval( Container.DataItem ,"Hyperlink") %>
											</ItemTemplate>
										</asp:DataList>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<tr id="trConsolidator" runat="server">
					<td>
						<table class="alltable" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td class="emptycol">&nbsp;</td>
							</tr>
							<TR vAlign="top">
								<TD class="tablecol">&nbsp;<STRONG>Consolidator</STRONG>&nbsp;:&nbsp;<asp:label id="lblConsolidator" Runat="server"></asp:label></TD>
							</TR>
						</table>
					</td>
				</tr>
			</TABLE>
			<TABLE class="AllTable" id="tblSearchResult" cellSpacing="0" cellPadding="0" border="0"
				runat="server">
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgPRList" runat="server" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="PRD_PR_LINE" HeaderText="Line">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_B_ITEM_CODE" HeaderText="Buyer Item Code">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_VENDOR_ITEM_CODE" HeaderText="Vendor Item Code">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn>
									<HeaderTemplate>
										(GL Code) GL Description
									</HeaderTemplate>
									<ItemTemplate>
										<%# GenerateGLString( DataBinder.Eval( Container.DataItem , "PRD_B_GL_CODE" ) , DataBinder.Eval( Container.DataItem , "CBG_B_GL_DESC" )  ) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PRD_B_CATEGORY_CODE" HeaderText="Category Code"></asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_B_TAX_CODE" HeaderText="Tax Code"></asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_PRODUCT_DESC" HeaderText="Item Description">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								
								<asp:BoundColumn DataField="ASSET_CODE" HeaderText="Asset Code">
									<HeaderStyle Width="16%"></HeaderStyle>
								</asp:BoundColumn>
								
								<asp:TemplateColumn HeaderText="MOQ">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label runat="server" Text='1' ID="Label1"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="MPQ">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label runat="server" Text='1' ID="Label2"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PRD_RFQ_QTY" HeaderText="RFQ Qty">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_QTY_TOLERANCE" HeaderText="Quotation Qty Tolerance">
									<HeaderStyle Width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_ORDERED_QTY" HeaderText="Qty">
									<HeaderStyle Width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_UOM" HeaderText="UOM">
									<HeaderStyle Width="3%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_UNIT_COST" HeaderText="Unit Price">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_UNIT_COST" HeaderText="Sub Total">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Tax">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Budget Account">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Delivery Address">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_ETD" HeaderText="Est. Date of Delivery (days)">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_WARRANTY_TERMS" HeaderText="Warranty Terms (mths)">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="12px"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="tblApproval" cellSpacing="0" cellPadding="0" border="0" runat="server">
							<TR>
								<TD class="emptycol" colSpan="2"></TD>
							</TR>
							<TR>
								<TD vAlign="middle">&nbsp;<STRONG>Remarks</STRONG>&nbsp;:</TD>
								<TD><asp:textbox id="txtRemark" Runat="server" Width="600px" TextMode="MultiLine" MaxLength="1000"
										Rows="3" CssClass="listtxtbox"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="2"></TD>
							</TR>
							<TR vAlign="top">
								<TD style="HEIGHT: 17px">&nbsp;<STRONG>File Attachment </STRONG>&nbsp;:</TD>
								<TD style="HEIGHT: 22px" colSpan="3" rowSpan="2"><INPUT class="button" id="File1" style="WIDTH: 222px; HEIGHT: 17px" type="file" size="17"
										name="uploadedFile3" runat="server">&nbsp;<asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button><asp:label id="lblFileAO" Runat="server" Visible="False"></asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD>&nbsp;<asp:label id="lblAttach" runat="server" Width="176px" CssClass="div">Recommended file size is 300KB</asp:label></TD>
							</TR>
							<TR vAlign="top">
								<TD style="HEIGHT: 19px">&nbsp;<STRONG>File Attached </STRONG>:</TD>
								<TD style="HEIGHT: 19px" colSpan="3"><asp:panel id="pnlAttach" runat="server"></asp:panel></TD>
							</TR>
							<TR vAlign="top">
								<td colSpan="1"><br>
								</td>
								<td></td>
							</TR>
							<TR id="trButton" runat="server">
								<TD colSpan="2"><asp:button id="cmdAppPR" runat="server" Width="100px" CssClass="button" Text="Approve PR"></asp:button><asp:button id="cmdRejectPR" runat="server" Width="100px" CssClass="button" Text="Reject PR"></asp:button><asp:button id="cmdHoldPR" runat="server" Width="100px" CssClass="button" Text="Hold PR"></asp:button><INPUT class="button" id="cmdClear" onclick="document.forms(0).txtRemark.value=''" type="button"
										value="Clear" name="cmdClear" runat="server"></TD>
							</TR>
							<TR id="trMessage" runat="server">
								<TD colSpan="2"><asp:label id="lblMsg" Runat="server" CssClass="ErrorMsg">The amount has exceeded your approval limit.</asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD>
						<TABLE id="tblBuyer" cellSpacing="0" cellPadding="0" border="0" runat="server">
							<TR>
								<TD class="emptycol" colSpan="2"></TD>
							</TR>
							<TR>
								<TD vAlign="middle">&nbsp;<STRONG>
										<asp:label id="lblRemarkCR" runat="server" Visible="False">Cancel Remarks :</asp:label></STRONG></TD>
								<TD><asp:textbox id="txtRemarkCR" Runat="server" Width="600px" TextMode="MultiLine" MaxLength="1000"
										Rows="3" CssClass="listtxtbox" Visible="False"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="2"></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="2"><asp:button id="cmdDup" runat="server" Width="100px" CssClass="button" Text="Duplicate PR"></asp:button><asp:button id="cmdCancel" runat="server" Width="100px" CssClass="button" Text="Cancel PR" Visible="False"></asp:button>&nbsp;&nbsp;&nbsp;
								</TD>
							</TR>
						</TABLE>
						<P><asp:textbox id="TextBox1" runat="server" Visible="False"></asp:textbox></P>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"><INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<P><asp:hyperlink id="lnkBack" Runat="server">
								<STRONG>&lt; Back</STRONG></asp:hyperlink></P>
					</TD>
				</TR>
			</TABLE>
			<asp:button id="cmdPrReport" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 1200px"
				runat="server" Text="View PR Report" Visible="False"></asp:button></form>
	</body>
</HTML>
