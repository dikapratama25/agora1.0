<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CreditNoteDetails.aspx.vb" Inherits="eProcure.CreditNoteDetails" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>CreditNoteDetails</title>
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
        if(document.getElementById("cmdAck"))
        { document.getElementById("cmdAck").style.display= "none"; }
        });
        $('#cmdAck').click(function() {
        if(document.getElementById("cmdSave"))
        { document.getElementById("cmdSave").style.display= "none"; }
        if(document.getElementById("cmdAck"))
        { document.getElementById("cmdAck").style.display= "none"; }
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
		<%  Response.Write(Session("w_CnTrackingList_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">			
				<tr>
					<td class="emptycol" align="left" colspan="4"></td>
				</tr>
				<tr>
					<td>
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" width="400" border="0">
							<tr>
								<td class="tableheader" colspan="2">&nbsp;Credit Note Header</td>
							</tr>
							<tr class="tableinput" style="width: 100%;">
								<td valign="top" style="width: 50%; height: 209px;">
									<table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr class="tablecol">
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Credit Note Number</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblCnNo" runat="server"></asp:label></td>
										</tr>
										<tr class="tablecol">
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Date</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblCnDate" runat="server" Width="145px"></asp:label></td>
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
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Remarks</td>
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
											<td class="tablecol" valign="top" noWrap><strong>Related Credit Note </strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblRelatedCN" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top" noWrap><strong>Total Related Credit Note Amount </strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblTotalRelatedCNAmt" runat="server"></asp:label></td>
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
					<td><asp:datagrid id="dtgCnDetail" runat="server" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="CND_CN_LINE" HeaderText="Line">
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
								<asp:BoundColumn DataField="CND_QTY" HeaderText="Qty">
									<HeaderStyle HorizontalAlign="Right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CND_UNIT_COST" HeaderText="Unit Price">
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
								<asp:BoundColumn DataField="CND_REMARKS" SortExpression="CND_REMARKS" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CND_GST_RATE" visible="false">
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
									<asp:button id="cmdAck" runat="server" CssClass="button" Text="Acknowledge Credit Note" Width="150px"></asp:button>
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
						<asp:button id="cmdPreviewCN" runat="server" CssClass="button" Width="100px" Text="View Credit Note" CausesValidation="False" UseSubmitBehavior="False"></asp:button>
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
