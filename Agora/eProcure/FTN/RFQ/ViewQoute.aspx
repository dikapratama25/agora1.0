<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="ViewQouteFTN.aspx.vb" Inherits="eProcure.ViewQouteFTN1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>ViewQoute</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
	<!--
		function PopWindow(myLoc)
		{
			window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
	//-->
		</script>
  </HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_RFQ_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header">Quotation
					</TD>
					<td></td>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;Quotation Number :
						<asp:label id="lbl_quoteNum" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD class="tableheader" colSpan="4">&nbsp;Quotation&nbsp; Details</TD>
							</TR>
							<TR>
								<TD class="tablecol" width="150"><STRONG>&nbsp;Date Created</STRONG> </STRONG>:</TD>
								<TD class="tableinput ">&nbsp;<asp:label id="lbl_CreateDate" runat="server" CssClass="lblinfo"></asp:label></TD>
								<TD class="tablecol"><STRONG>&nbsp;Contact Person</STRONG>&nbsp;</STRONG>:</TD>
								<TD class="tableinput ">&nbsp;<asp:label id="lbl_ContactPer" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol"><STRONG>&nbsp;Quotation Validity</STRONG></STRONG> :</TD>
								<TD class="tableinput ">&nbsp;<asp:label id="lbl_QuoteVal" runat="server" CssClass="lblinfo"></asp:label></TD>
								<TD class="tablecol"><STRONG>&nbsp;Contact Number</STRONG> </STRONG>:</TD>
								<TD class="tableinput ">&nbsp;<asp:label id="lbl_ContNum" runat="server" CssClass="lblinfo"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol"><STRONG>&nbsp;From</STRONG> </STRONG>:
								</TD>
								<TD class="tableinput ">&nbsp;<asp:label id="lbl_From" runat="server" CssClass="lblinfo"></asp:label></TD>
								<TD class="tablecol"><STRONG>&nbsp;Email Address </STRONG>:</TD>
								<TD class="tableinput ">&nbsp;<asp:label id="lbl_Email" runat="server" CssClass="lblinfo"></asp:label></TD>

							</TR>
							<TR>
								<TD class="tablecol"><STRONG>&nbsp;Physical Address</STRONG> </STRONG>:</TD>
								<TD class="tableinput ">&nbsp;<asp:label id="lbl_PhyAdds" runat="server" CssClass="lblinfo"></asp:label></TD>
								<TD class="tablecol">&nbsp;<STRONG>Remarks </STRONG>:</TD>
								<TD class="tableinput ">&nbsp;<asp:textbox id="txt_remark" runat="server" Width="190px" TextMode="MultiLine" Height="44px"
										 contentEditable="false"  CssClass="lblinfo"></asp:textbox></TD>

							</TR>							
							<TR>
								<TD class="tablecol"><STRONG>&nbsp;File attachment(s) </STRONG>:</TD>
								<TD class="tableinput ">&nbsp;<asp:panel id="pnlAttach2" runat="server"></asp:panel></TD>
								<td class="tablecol" colSpan="2"></td>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dg_viewitem" runat="server" CssClass="grid" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="RRD_Vendor_Item_Code" HeaderText="Vendor Item Code " Visible="False">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RRD_Product_Desc"  readonly="true"  HeaderText="Item Name">
									<HeaderStyle Width="29%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RRD_UOM" HeaderText="UOM">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RRD_Quantity" HeaderText="QTY">
									<HeaderStyle  HorizontalAlign="Right" Width="5%" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RRD_Tolerance" HeaderText="Qty Tolerance" Visible="False">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Unit Price">
									<HeaderStyle Width="7%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_unit_price" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Amount">
									<HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_price" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<%--Jules GST Enhancement--%>
								<asp:TemplateColumn HeaderText="GST Rate">
									<HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_GSTRate" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="GST Amount">
									<HeaderStyle Width="3%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_tax" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>								
								<%--Jules GST Enhancement end--%>
								<asp:TemplateColumn HeaderText="Pack Qty">
									<HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_mpq" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="MOQ">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_moq" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>								
								<asp:TemplateColumn HeaderText="Delivery Lead Time(days)">
									<HeaderStyle  HorizontalAlign="left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="left"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_Delivery" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Warranty Terms (mths) ">
									<HeaderStyle  HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_warranty" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="RRD_Remarks" HeaderText="Remarks">
									<HeaderStyle Width="18%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD>
						<INPUT type="button" value="View Quotation" id="cmdView" runat="server" Class="button" style="width: 83px">
					</TD>
				</TR>
			
				<tr>
					<td>&nbsp;</td>
				</tr>
			    <TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
