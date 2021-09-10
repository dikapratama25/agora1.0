<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="InvDetail.aspx.vb" Inherits="eProcure.InvDetail1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>InvDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
					<td class="linespacing1"></td>
			</tr>
				<tr>
					<td class="header">Invoice Details
					</td>
				</tr>
            <tr>
					<td class="linespacing2"></td>
			</tr>
				<tr>
					<td>
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" width="300" border="0">
							<tr class="tableheader">
								<td width="50%" colspan="2">&nbsp;Invoice&nbsp;Details&nbsp;
								</td>
								<td width="50%" colspan="2">&nbsp;</td>
							</tr>
							<tr class="tablecol">
								<td style="HEIGHT: 28px" width="23%"><strong>&nbsp;Invoice Number </strong>:</td>
								<td class="tableinput " style="HEIGHT: 28px" width="27%">&nbsp;
									<asp:label id="lbl_InvNum" runat="server" CssClass="lblinfo"></asp:label></td>
								<td style="HEIGHT: 28px" width="23%"><strong>&nbsp;Requisitioner </strong>:
								</td>
								<td class="tableinput " style="HEIGHT: 28px" width="27%">&nbsp;
									<asp:label id="lbl_req" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%"><strong>&nbsp;Date </strong>:</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_date" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol" width="23%"><strong>&nbsp;Contact Number </strong>:
								</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_contect" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%"><strong>&nbsp;Tax Reg. No. </strong>:</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_gst" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol" width="23%"><strong>&nbsp;Currency Code </strong>:
								</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_cur_code" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%"><strong>&nbsp;Our Ref. </strong>:</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_OurRef" runat="server" CssClass="lblinfo"></asp:label>
								</td>
								<td class="tablecol" width="23%"><strong>&nbsp;Attention To </strong>:&nbsp;</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_attention" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" style="HEIGHT: 24px" width="23%"><strong>&nbsp;Your Ref.&nbsp;</strong>:</td>
								<td class="tableinput " style="HEIGHT: 24px" width="27%">&nbsp;
									<asp:label id="lbl_YourRef" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol" style="HEIGHT: 24px" width="23%"><strong>&nbsp;Shipment Terms </strong>
									:
								</td>
								<td class="tableinput " style="HEIGHT: 24px" width="27%">&nbsp;
									<asp:label id="lbl_st" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%"><strong>&nbsp;Payment Terms </strong>:</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_Payterm" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol" width="23%"><strong>&nbsp;Shipment Mode </strong>:</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_sm" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" style="HEIGHT: 51px" width="23%"><strong>&nbsp;Payment&nbsp;Method </strong>
									:</td>
								<td class="tableinput " style="HEIGHT: 51px" width="27%">&nbsp;
									<asp:label id="lbl_pm" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol" style="HEIGHT: 51px" valign="top" width="23%"><strong>&nbsp;Remarks
									</strong>:
								</td>
								<td class="tableinput " style="HEIGHT: 51px" width="27%">&nbsp;
									<asp:textbox id="txt_remark" runat="server" TextMode="MultiLine" Height="44px"  contentEditable="false" 
										Width="186px" CssClass="listtxtbox"></asp:textbox></td>
							</tr>
							<tr id="tr_dt" runat="server">
								<td class="tablecol" width="23%"><strong>&nbsp;Delivery Term </strong>:</td>
								<td class="tableinput" width="27%">&nbsp;<asp:label id="lbl_dt" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol" width="23%"></td>
								<td class="tableinput" width="27%"></td>
							</tr>
							<tr>
								<td class="tablecol" style="HEIGHT: 51px" width="23%" valign="top"><strong>&nbsp;Bill 
										To </strong>:</td>
								<td class="tableinput " width="27%">&nbsp;<asp:label id="lbl_bill" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol" style="HEIGHT: 51px" valign="middle" width="23%"></td>
								<td class="tableinput " style="HEIGHT: 51px" width="27%"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="EMPTYCOL"></td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtg_invDetail" runat="server" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn HeaderText="Line">
									<HeaderStyle HorizontalAlign="Right" Width="8%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Product_Desc" HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="QTY" HeaderText="Qty">
									<HeaderStyle HorizontalAlign="Right" Width="8%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UNIT_COST" HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="Right" Width="15%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Sub Total">
									<HeaderStyle HorizontalAlign="Right" Width="14%" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Tax">
									<HeaderStyle HorizontalAlign="Right" Width="10%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_WARRANTY_TERMS" HeaderText="Warranty Terms">
									<HeaderStyle HorizontalAlign="Right" Width="15%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td><%--<asp:Button ID="cmd_back" runat="server" Text="Close" cssclass="button" />--%>
					<a id="back_view" href="#" onclick="javascript:history.back();" runat="server"><strong>&lt;Back</strong></a>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
