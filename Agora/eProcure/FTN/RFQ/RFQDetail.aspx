<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="RFQDetail.aspx.vb" Inherits="eProcure.RFQDetailFTN" %>
<!DOCTYPE html PUBLIC '-//W3C//Dtd XHTML 1.0 transitional//EN' 'http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd'>
<HTML xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
	<HEAD>
		<title>RFQDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Pragma" content="no-cache" />
        <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
        <meta http-equiv="Expires" content="-1" />
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
        <meta http-equiv="Content-Style-Type" content="text/css" />
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
	<!--
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
	//-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_RFQ_tabs"))%>
			<TABLE class="alltable" id="Table1" width="100%" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="height: 19px"><asp:label id="lblHead" Text="RFQ Info" runat="server"></asp:label>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 15px"><STRONG>RFQ Number</STRONG> :
						<asp:label id="lbl_Num" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"><STRONG>RFQ Description</STRONG> :
						<asp:label id="lbl_Name" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR class="tableheader">
								<TD width="50%" colSpan="2">&nbsp;Purchaser Details
								</TD>
								<td width="50%" colSpan="2">&nbsp;Vendor Details</td>
							</TR>
							<TR class="tablecol">
								<TD style="HEIGHT: 19px" width="23%">&nbsp;<STRONG>RFQ Expiry Date</STRONG>:</TD>
								<td class="tableinput " style="HEIGHT: 19px" width="27%">
									<asp:label id="lbl_exp" runat="server"></asp:label></td>
								<TD style="HEIGHT: 19px" width="23%">&nbsp;<STRONG>Quotation
										&nbsp;Validity Date</STRONG> :</TD>
								<td class="tableinput " style="HEIGHT: 19px" width="27%">
									<asp:label id="lbl_req_qout" runat="server"></asp:label></td>
							</TR>
							<TR>
								<TD class="tablecol" width="23%" style="height: 19px">&nbsp;<STRONG>Currency</STRONG> :
								</TD>
								<TD class="tableinput " width="27%" style="height: 19px">
									<asp:label id="lbl_cur" runat="server"></asp:label></TD>
								<TD class="tablecol" width="23%" style="height: 19px">&nbsp;<STRONG>Payment Terms</STRONG> :
								</TD>
								<TD class="tableinput " width="27%" style="height: 19px">
									<asp:label id="lbl_pt" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" width="23%" style="height: 19px">&nbsp;<STRONG>Contact Person</STRONG>:
								</TD>
								<TD class="tableinput " width="27%" style="height: 19px">
									<asp:label id="lbl_Con_person" runat="server"></asp:label></TD>
								<TD class="tablecol" width="23%" style="height: 19px">&nbsp;<STRONG>Payment Method</STRONG> :
								</TD>
								<TD class="tableinput " width="27%" style="height: 19px">
									<asp:label id="lbl_pm" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" width="23%">&nbsp;<STRONG>Contact Number</STRONG>:
								</TD>
								<TD class="tableinput " width="27%">
									<asp:label id="lbl_con_num" runat="server"></asp:label></TD>
								<TD class="tablecol" width="23%">&nbsp;<STRONG>Shipment Mode</STRONG> :
								</TD>
								<TD class="tableinput " width="27%">
									<asp:label id="lbl_sm" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" width="23%">&nbsp;<STRONG>Email</STRONG>:
								</TD>
								<TD class="tableinput " width="27%">
									<asp:label id="lbl_email" runat="server"></asp:label></TD>
								<TD class="tablecol" width="23%">&nbsp;<STRONG>Shipment Terms</STRONG> :
								</TD>
								<TD class="tableinput " width="27%">
									<asp:label id="lbl_st" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 51px" width="23%">&nbsp;<STRONG><asp:label id="lblExtFile" runat="server" Text="External File(s) Attached "></asp:label></STRONG>
									:
								</TD>
								<TD class="tableinput " style="HEIGHT: 51px" width="27%">
									<asp:panel id="pnlAttach2" runat="server"></asp:panel></TD>
								<TD class="tablecol" style="HEIGHT: 51px" vAlign="middle" width="23%">&nbsp;<STRONG><asp:label id="lblRemark" runat="server" Text="External Remarks"></asp:label></STRONG>
									:
								</TD>
								<TD class="tableinput " style="HEIGHT: 51px" width="27%">
									<asp:textbox id="txt_remark" runat="server"  contentEditable="false"  Height="44px" TextMode="MultiLine"
										Width="186px"></asp:textbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="height: 19px"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_rfqdetail" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_rfqdetail_Page"
							AutoGenerateColumns="False" AllowSorting="True">
							<Columns>
								<asp:BoundColumn HeaderText="No." Visible="False">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RD_Product_Desc"  readonly="True"  HeaderText="Item Name">
									<HeaderStyle HorizontalAlign="Left" Width="55%"></HeaderStyle>
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
								<asp:BoundColumn DataField="RD_Warranty_Terms" HeaderText="W. T. (Mths) ">
									<HeaderStyle HorizontalAlign="Right" Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<TR>
					<TD class="emptycol"><INPUT class="button" id="cmdPreview" style="WIDTH: 75px" type="button" value="View RFQ"
							name="cmdPreview" runat="server"></TD>
				</TR>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<TR>							
					<TD class="emptycol" colSpan="5"><A id="A1" runat="server" onclick="history.back();" href="#"><STRONG>&lt; Back</STRONG></A></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
