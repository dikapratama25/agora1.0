<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InvoiceDetails.aspx.vb" Inherits="eProcure.InvoiceDetailsFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>InvDetail</title>
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
        if(document.getElementById("cmdHoldInv"))
        { document.getElementById("cmdHoldInv").style.display= "none"; }
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
        if(document.getElementById("cmdHoldInv"))
        { document.getElementById("cmdHoldInv").style.display= "none"; }
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
        if(document.getElementById("cmdHoldInv"))
        { document.getElementById("cmdHoldInv").style.display= "none"; }
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
        if(document.getElementById("cmdHoldInv"))
        { document.getElementById("cmdHoldInv").style.display= "none"; }
        if(document.getElementById("cmdRejectInv"))
        { document.getElementById("cmdRejectInv").style.display= "none"; }
        if(document.getElementById("cmdVerify"))
        { document.getElementById("cmdVerify").style.display= "none"; }
        });
        $('#cmdHoldInv').click(function() {
        if(document.getElementById("cmdSave"))
        { document.getElementById("cmdSave").style.display= "none"; }
        if(document.getElementById("cmdMark"))
        { document.getElementById("cmdMark").style.display= "none"; }
        if(document.getElementById("cmdAppInv"))
        { document.getElementById("cmdAppInv").style.display= "none"; }
        if(document.getElementById("cmdHoldInv"))
        { document.getElementById("cmdHoldInv").style.display= "none"; }
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
        if(document.getElementById("cmdHoldInv"))
        { document.getElementById("cmdHoldInv").style.display= "none"; }
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
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_InvTracking_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">			
				<tr>
					<td class="emptycol" align="left" colspan="4"></td>
				</tr>
				<tr>
					<td>
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" width="300" border="0">
							<tr>
								<td class="tableheader" colspan="4">&nbsp;Invoice Header</td>
							</tr>
							<tr class="tableinput">
								<td valign="top" width="50%">
									<table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr class="tablecol">
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Vendor</td>
											<td class="tablecol " valign="top" align="center" width="4%">:</td>
											<td class="tableinput " valign="top" width="60%"><asp:label id="lblComName" runat="server"></asp:label><br>
												<asp:label id="lblAddr" runat="server"></asp:label></td>
										</tr>
										<tr class="tablecol">
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Business 
												Reg. No.</td>
											<td class="tablecol " valign="top" align="center" width="4%">:</td>
											<td class="tableinput " valign="top" width="60%"><asp:label id="lblBusRegNo" runat="server" Width="145px"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top">Tel</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblTel" runat="server" Width="145px"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top">Email</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblEmail" runat="server"></asp:label></td>
										</tr>
										<tr class="tablecol">
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Bill To</td>
											<td class="tablecol " valign="top" align="center" width="4%">:</td>
											<td class="tableinput " valign="top" width="60%"><asp:label id="lblBCoyName" runat="server"></asp:label><br>
												<asp:label id="lblBillTo" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Vendor 
												Remarks</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblVenRemarks" runat="server"></asp:label></td>
										</tr>
									</table>
								</td>
								<td class="emptycol" style="WIDTH: 206px" width="206">
									<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr>
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Invoice No.</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblInvNo" runat="server" Font-Bold="True"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top" noWrap><strong>Date</strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblDate" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top"><strong>Our Ref.</strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblOurRef" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top"><strong>Your Ref.</strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblYourRef" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top"><strong>Payment Terms</strong>
											</td>
											<td class="tablecol" valign="top" align="center" width="5%">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblPayTerm" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top" noWrap><strong>Payment Method </strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblPayMethod" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top" noWrap><strong>Shipment&nbsp;Terms&nbsp; </strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblShipType" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top"><strong>Shipment Mode</strong></td>
											<td class="tablecol" valign="top" align="center" width="5%">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblShipMode" runat="server"></asp:label></td>
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
								<asp:BoundColumn DataField="FA_SEQ" HeaderText="Level">
									<HeaderStyle Width="1%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
									<HeaderStyle Width="29%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FA_APPROVAL_TYPE" HeaderText="Approval Type">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FA_ACTION_DATE" HeaderText="Approved Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FA_AO_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="EMPTYCOL"></td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtg_invDetail" runat="server" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn HeaderText="Line">
									<HeaderStyle HorizontalAlign="Right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_PRODUCT_DESC" HeaderText="Item Description">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_RECEIVED_QTY" HeaderText="Qty">
									<HeaderStyle HorizontalAlign="Right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_UNIT_COST" HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="Right" Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Total">
									<HeaderStyle HorizontalAlign="Right" Width="16%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_GST" SortExpression="ID_GST" HeaderText="Tax">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GST_RATE" SortExpression="GST_RATE" HeaderText="GST Rate">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_GST" SortExpression="ID_GST" HeaderText="GST Amoount">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="GST Tax Code (Purchase)">
								    <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:DropDownList id="ddlTaxCode" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList>
									    <asp:Label id="lblTaxCode" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="ID_WARRANTY_TERMS" HeaderText="Warranty Terms">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IM_SHIP_AMT" visible="false">
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_GST_RATE" visible="false">
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
									<asp:button id="cmdAppInv" runat="server" CssClass="button" Width="100px" Text="Approve Invoice"></asp:button>
									<asp:button id="cmdHoldInv" runat="server" CssClass="button" Width="100px" Text="Hold Invoice" Visible="False"></asp:button>
									<asp:button id="cmdRejectInv" runat="server" CssClass="button" Width="100px" Text="Reject Invoice"
										Visible="False"></asp:button>
                                    <asp:Button ID="cmdVerify" runat="server" CssClass="button" Text="Reject Invoice"
                                        Visible="False" Width="100px" /></td>
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
						<asp:button id="cmdPreviewInvoice" runat="server" CssClass="button" Width="100px" Text="View Invoice"></asp:button>
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
</HTML>
