<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="RFQDetail.aspx.vb" Inherits="eProcure.RFQDetail" %>
<!DOCTYPE html PUBLIC '-//W3C//Dtd XHTML 1.0 transitional//EN' 'http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd'>
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
	<head>
		<title>RFQDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<meta http-equiv="Pragma" content="no-cache" />
        <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
        <meta http-equiv="Expires" content="-1" />
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
        <meta http-equiv="Content-Style-Type" content="text/css" />
		<% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
	<!--
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
	//-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_RFQ_tabs"))%>
			<table class="alltable" id="Table1" width="100%" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td class="header" style="height: 19px"><asp:label id="lblHead" Text="RFQ Info" runat="server"></asp:label>
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol" style="HEIGHT: 15px"><strong>RFQ Number</strong> :
						<asp:label id="lbl_Num" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol"><strong>RFQ Description</strong> :
						<asp:label id="lbl_Name" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol">
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr class="tableheader">
								<td width="50%" colspan="2">&nbsp;Purchaser Details
								</td>
								<td width="50%" colspan="2">&nbsp;Vendor Details</td>
							</tr>
							<tr class="tablecol">
								<td style="HEIGHT: 19px" width="23%">&nbsp;<strong>RFQ Expiry Date</strong>:</td>
								<td class="tableinput " style="HEIGHT: 19px" width="27%">
									<asp:label id="lbl_exp" runat="server"></asp:label></td>
								<td style="HEIGHT: 19px" width="23%">&nbsp;<strong>Quotation
										&nbsp;Validity Date</strong> :</td>
								<td class="tableinput " style="HEIGHT: 19px" width="27%">
									<asp:label id="lbl_req_qout" runat="server"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%" style="height: 19px">&nbsp;<strong>Currency</strong> :
								</td>
								<td class="tableinput " width="27%" style="height: 19px">
									<asp:label id="lbl_cur" runat="server"></asp:label></td>
								<td class="tablecol" width="23%" style="height: 19px">&nbsp;<strong>Payment Terms</strong> :
								</td>
								<td class="tableinput " width="27%" style="height: 19px">
									<asp:label id="lbl_pt" runat="server"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%" style="height: 19px">&nbsp;<strong>Contact Person</strong>:
								</td>
								<td class="tableinput " width="27%" style="height: 19px">
									<asp:label id="lbl_Con_person" runat="server"></asp:label></td>
								<td class="tablecol" width="23%" style="height: 19px">&nbsp;<strong>Payment Method</strong> :
								</td>
								<td class="tableinput " width="27%" style="height: 19px">
									<asp:label id="lbl_pm" runat="server"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%">&nbsp;<strong>Contact Number</strong>:
								</td>
								<td class="tableinput " width="27%">
									<asp:label id="lbl_con_num" runat="server"></asp:label></td>
								<td class="tablecol" width="23%">&nbsp;<strong>Shipment Mode</strong> :
								</td>
								<td class="tableinput " width="27%">
									<asp:label id="lbl_sm" runat="server"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%">&nbsp;<strong>Email</strong>:
								</td>
								<td class="tableinput " width="27%">
									<asp:label id="lbl_email" runat="server"></asp:label></td>
								<td class="tablecol" width="23%">&nbsp;<strong>Shipment Terms</strong> :
								</td>
								<td class="tableinput " width="27%">
									<asp:label id="lbl_st" runat="server"></asp:label></td>
							</tr>
							<tr id="tr_delterm" runat="server">
								<td class="tablecol" width="23%">&nbsp;<strong>Delivery Term</strong>:
								</td>
								<td class="tableinput " width="27%">
									<asp:label id="lbl_dt" runat="server"></asp:label></td>
								<td class="tablecol" width="23%">
								</td>
								<td class="tableinput " width="27%"></td>
							</tr>
							<tr>
								<td class="tablecol" style="HEIGHT: 51px" width="23%">&nbsp;<strong><asp:label id="lblExtFile" runat="server" Text="External File(s) Attached "></asp:label></strong>
									:
								</td>
								<td class="tableinput " style="HEIGHT: 51px" width="27%">
									<asp:panel id="pnlAttach2" runat="server"></asp:panel></td>
								<td class="tablecol" style="HEIGHT: 51px" valign="middle" width="23%">&nbsp;<strong><asp:label id="lblRemark" runat="server" Text="External Remarks"></asp:label></strong>
									:
								</td>
								<td class="tableinput " style="HEIGHT: 51px" width="27%">
									<asp:textbox id="txt_remark" runat="server"  contentEditable="false"  Height="44px" TextMode="MultiLine"
										Width="186px"></asp:textbox></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 19px"></td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtg_rfqdetail" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_rfqdetail_Page"
							AutoGenerateColumns="False" AllowSorting="True">
							<Columns>
								<asp:BoundColumn HeaderText="No." Visible="False">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_Product_Desc"  readonly="True"  HeaderText="Item Name">
									<HeaderStyle HorizontalAlign="Left" Width="47%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_UOM"  readonly="True"   HeaderText="UOM ">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_Quantity"  readonly="True"  HeaderText="Quantity">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_Delivery_Lead_Time"  readonly="True"   HeaderText="Delivery Lead Time (Days)">
									<HeaderStyle HorizontalAlign="Right" Width="14%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_Warranty_Terms" HeaderText="Warranty &lt;BR&gt;Terms (mths) ">
									<HeaderStyle HorizontalAlign="Right" Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_ITEM_TYPE" HeaderText="Item Type">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol"><input class="button" id="cmdPreview" style="WIDTH: 75px" type="button" value="View RFQ"
							name="cmdPreview" runat="server"/></td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>							
					<td class="emptycol" colspan="5"><a id="A1" runat="server" onclick="history.back();" href="#"><strong>&lt; Back</strong></a></td>
				</tr>
			</table>
		</form>
	</body>
</html>
