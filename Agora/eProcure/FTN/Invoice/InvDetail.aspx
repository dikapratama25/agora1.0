<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="InvDetail.aspx.vb" Inherits="eProcure.InvDetailFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>InvDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
            <tr>
					<TD class="linespacing1"></TD>
			</TR>
				<TR>
					<TD class="header">Invoice Details
					</TD>
				</TR>
            <tr>
					<TD class="linespacing2"></TD>
			</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR class="tableheader">
								<TD width="50%" colSpan="2">&nbsp;Invoice&nbsp;Details&nbsp;
								</TD>
								<TD width="50%" colSpan="2">&nbsp;</TD>
							</TR>
							<TR class="tablecol">
								<TD style="HEIGHT: 28px" width="23%"><strong>&nbsp;Invoice Number </strong>:</TD>
								<TD class="tableinput " style="HEIGHT: 28px" width="27%">&nbsp;
									<asp:label id="lbl_InvNum" runat="server" CssClass="lblinfo"></asp:label></TD>
								<TD style="HEIGHT: 28px" width="23%"><strong>&nbsp;Requisitioner </strong>:
								</TD>
								<TD class="tableinput " style="HEIGHT: 28px" width="27%">&nbsp;
									<asp:label id="lbl_req" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" width="23%"><strong>&nbsp;Date </strong>:</TD>
								<TD class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_date" runat="server" CssClass="lblinfo"></asp:label></TD>
								<TD class="tablecol" width="23%"><strong>&nbsp;Contact Number </strong>:
								</TD>
								<TD class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_contect" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" width="23%"><strong>&nbsp;Tax Reg. No. </strong>:</TD>
								<TD class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_gst" runat="server" CssClass="lblinfo"></asp:label></TD>
								<TD class="tablecol" width="23%"><strong>&nbsp;Currency Code </strong>:
								</TD>
								<TD class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_cur_code" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" width="23%"><strong>&nbsp;Our Ref. </strong>:</TD>
								<TD class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_OurRef" runat="server" CssClass="lblinfo"></asp:label>
								</TD>
								<TD class="tablecol" width="23%"><strong>&nbsp;Attention To </strong>:&nbsp;</TD>
								<TD class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_attention" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 24px" width="23%"><strong>&nbsp;Your Ref.&nbsp;</strong>:</TD>
								<TD class="tableinput " style="HEIGHT: 24px" width="27%">&nbsp;
									<asp:label id="lbl_YourRef" runat="server" CssClass="lblinfo"></asp:label></TD>
								<TD class="tablecol" style="HEIGHT: 24px" width="23%"><strong>&nbsp;Shipment Terms </strong>
									:
								</TD>
								<TD class="tableinput " style="HEIGHT: 24px" width="27%">&nbsp;
									<asp:label id="lbl_st" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" width="23%"><strong>&nbsp;Payment Terms </strong>:</TD>
								<TD class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_Payterm" runat="server" CssClass="lblinfo"></asp:label></TD>
								<TD class="tablecol" width="23%"><strong>&nbsp;Shipment Mode </strong>:</TD>
								<TD class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_sm" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 51px" width="23%"><strong>&nbsp;Payment&nbsp;Method </strong>
									:</TD>
								<TD class="tableinput " style="HEIGHT: 51px" width="27%">&nbsp;
									<asp:label id="lbl_pm" runat="server" CssClass="lblinfo"></asp:label></TD>
								<TD class="tablecol" style="HEIGHT: 51px" vAlign="top" width="23%"><strong>&nbsp;Remarks
									</strong>:
								</TD>
								<TD class="tableinput " style="HEIGHT: 51px" width="27%">&nbsp;
									<asp:textbox id="txt_remark" runat="server" TextMode="MultiLine" Height="44px"  contentEditable="false" 
										Width="186px" CssClass="listtxtbox"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tablecol" style="HEIGHT: 51px" width="23%" valign="top"><strong>&nbsp;Bill 
										To </strong>:</TD>
								<TD class="tableinput " width="27%">&nbsp;<asp:label id="lbl_bill" runat="server" CssClass="lblinfo"></asp:label></TD>
								<TD class="tablecol" style="HEIGHT: 51px" vAlign="middle" width="23%"></TD>
								<TD class="tableinput " style="HEIGHT: 51px" width="27%"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="EMPTYCOL"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_invDetail" runat="server" AutoGenerateColumns="False">
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
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><%--<asp:Button ID="cmd_back" runat="server" Text="Close" cssclass="button" />--%>
					<A id="back_view" href="#" onclick="javascript:history.back();" runat="server"><strong>&lt;Back</strong></A>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
