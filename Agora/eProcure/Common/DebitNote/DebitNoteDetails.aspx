<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DebitNoteDetails.aspx.vb" Inherits="eProcure.DebitNoteDetails" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>DebitNoteDetails</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<% Response.Write(Session("JQuery")) %>
		<script type="text/javascript">
		<!--  
		
		$(document).ready(function(){
        $('#cmdSave').click(function() {
        if(document.getElementById("cmdSave"))
        { document.getElementById("cmdSave").style.display= "none"; }
        if(document.getElementById("cmdMark"))
        { document.getElementById("cmdMark").style.display= "none"; }
        if(document.getElementById("cmdAppInv"))
        { document.getElementById("cmdAppInv").style.display= "none"; }
        if(document.getElementById("cmdRejectInv"))
        { document.getElementById("cmdRejectInv").style.display= "none"; }
        if(document.getElementById("cmdVerify"))
        { document.getElementById("cmdVerify").style.display= "none"; }
        });
        $('#cmdRejectInv').click(function() {
        if(document.getElementById("cmdSave"))
        { document.getElementById("cmdSave").style.display= "none"; }
        if(document.getElementById("cmdMark"))
        { document.getElementById("cmdMark").style.display= "none"; }
        if(document.getElementById("cmdAppInv"))
        { document.getElementById("cmdAppInv").style.display= "none"; }
        if(document.getElementById("cmdRejectInv"))
        { document.getElementById("cmdRejectInv").style.display= "none"; }
        if(document.getElementById("cmdVerify"))
        { document.getElementById("cmdVerify").style.display= "none"; }
        });
        $('#cmdVerify').click(function() {
        if(document.getElementById("cmdSave"))
        { document.getElementById("cmdSave").style.display= "none"; }
        if(document.getElementById("cmdMark"))
        { document.getElementById("cmdMark").style.display= "none"; }
        if(document.getElementById("cmdAppInv"))
        { document.getElementById("cmdAppInv").style.display= "none"; }
        if(document.getElementById("cmdRejectInv"))
        { document.getElementById("cmdRejectInv").style.display= "none"; }
        if(document.getElementById("cmdVerify"))
        { document.getElementById("cmdVerify").style.display= "none"; }
        });
        $('#cmdAppInv').click(function() {
        if(document.getElementById("cmdSave"))
        { document.getElementById("cmdSave").style.display= "none"; }
        if(document.getElementById("cmdMark"))
        { document.getElementById("cmdMark").style.display= "none"; }
        if(document.getElementById("cmdAppInv"))
        { document.getElementById("cmdAppInv").style.display= "none"; }
        if(document.getElementById("cmdRejectInv"))
        { document.getElementById("cmdRejectInv").style.display= "none"; }
        if(document.getElementById("cmdVerify"))
        { document.getElementById("cmdVerify").style.display= "none"; }
        });
        $('#cmdMark').click(function() {
        if(document.getElementById("cmdSave"))
        { document.getElementById("cmdSave").style.display= "none"; }
        if(document.getElementById("cmdMark"))
        { document.getElementById("cmdMark").style.display= "none"; }
        if(document.getElementById("cmdAppInv"))
        { document.getElementById("cmdAppInv").style.display= "none"; }
        if(document.getElementById("cmdRejectInv"))
        { document.getElementById("cmdRejectInv").style.display= "none"; }
        if(document.getElementById("cmdVerify"))
        { document.getElementById("cmdVerify").style.display= "none"; }
        });
        });


		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}	

		-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_DnTracking_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">			
				<tr>
					<td class="emptycol" align="left" colspan="4"></td>
				</tr>
				<tr>
					<td>
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" width="400" border="0">
							<tr>
								<td class="tableheader" colspan="2">&nbsp;Debit Note Header</td>
							</tr>
							<tr class="tableinput" style="width: 100%;">
								<td valign="top" style="width: 50%; height: 209px;">
									<table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr class="tablecol">
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Debit Note Number</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblDnNo" runat="server"></asp:label></td>
										</tr>
										<tr class="tablecol">
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Date</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblDnDate" runat="server" Width="145px"></asp:label></td>
										</tr>
										<tr class="tablecol">
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Bill To</td>
											<td class="tablecol " valign="top" align="center" width="4%">:</td>
											<td class="tableinput " valign="top" width="60%"><asp:label id="lblBillTo" runat="server"></asp:label></td>
										</tr>
										<tr class="tablecol">
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">File Attached</td>
											<td class="tablecol " valign="top" align="center" width="4%">:</td>
											<td class="tableinput " valign="top" width="60%"><asp:panel id="pnlAttach" runat="server"></asp:panel></td>
										</tr>
										<tr>
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Vendor 
												Remarks</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblVenRemarks" runat="server"></asp:label></td>
										</tr>
									</table>
								</td>
								<td valign="top" style="width: 50%; height: 209px;">
									<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr>
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Invoice No.</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblInvNo" runat="server" Font-Bold="True"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top" noWrap style="height: 19px"><strong>Date</strong>
											</td>
											<td class="tablecol" valign="top" align="center" style="height: 19px">:</td>
											<td class="tableinput" valign="top" style="height: 19px"><asp:label id="lblDate" runat="server"></asp:label></td>
										</tr>
										<tr id="tr_curr" runat="server">
											<td class="tablecol" valign="top"><strong>Currency</strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblCurr" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top"><strong>Invoice Amount </strong>
											</td>
											<td class="tablecol" valign="top" align="center" width="5%">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblInvAmt" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top" noWrap><strong>Related Debit Note </strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblRelatedDN" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top" noWrap><strong>Total Related Debit Note Amount </strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblTotalRelatedDNAmt" runat="server"></asp:label></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td class="emptycol"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="EMPTYCOL"></td>
				</tr>
				<tr>
					<td class="tableheader">Approval Workflow</td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtgAppFlow" runat="server">
							<Columns>
								<asp:BoundColumn DataField="DNA_SEQ" HeaderText="Level">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
									<HeaderStyle Width="29%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DNA_APPROVAL_TYPE" HeaderText="Approval Type">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DNA_ACTION_DATE" HeaderText="Approved Date">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DNA_AO_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="EMPTYCOL"></td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtgDnDetail" runat="server" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="DND_DN_LINE" HeaderText="Line">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_PRODUCT_DESC" HeaderText="Item Description">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DND_QTY" HeaderText="Qty">
									<HeaderStyle HorizontalAlign="Right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DND_UNIT_COST" HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Total">
									<HeaderStyle HorizontalAlign="Right" Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GST_RATE" SortExpression="GST_RATE" HeaderText="SST Rate">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="SST Amount">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="SST Tax Code (Purchase)">
								    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:DropDownList id="ddlTaxCode" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList>
									    <asp:Label id="lblTaxCode" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="DND_REMARKS" SortExpression="DND_REMARKS" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DND_GST_RATE" visible="false">
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td>
						<table class="AllTable" id="tblApproval" cellspacing="0" cellpadding="0" border="0" runat="server">
							<tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<tr>
								<td valign="middle" style="width: 197px">&nbsp;<strong>Remarks</strong>&nbsp;:</td>
								<td>
									<asp:textbox id="txtRemark" Runat="server" CssClass="listtxtbox" Rows="3" MaxLength="1000" TextMode="MultiLine"
										Width="600px"></asp:textbox></td>
							</tr>
							<tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<tr id="Tr1" runat="server">
								<td colspan="2">
									<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save" ></asp:button>
									<asp:button id="cmdMark" runat="server" CssClass="button" Text="Mark as Paid" Width="100px"></asp:button>
									<asp:button id="cmdAppDn" runat="server" CssClass="button" Width="100px" Text="Approve Debit Note"></asp:button>
									</td>
							</tr>
							<tr>
								<td class="emptycol" style="width: 197px"></td>
							</tr>
							<tr id="trMessage" runat="server">
								<td colspan="2">
									<asp:label id="lblMsg" Runat="server" CssClass="ErrorMsg"></asp:label></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr id="trPreview" runat="server">
					<td colspan="2">
						<asp:button id="cmdPreviewDN" runat="server" CssClass="button" Width="100px" Text="View Debit Note" CausesValidation="False" UseSubmitBehavior="False"></asp:button>
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4"><a id="back" href="#" runat="server"><strong>&lt; Back</strong></a></td>
				</tr>
			</table>
		</form>
	</body>
</html>
