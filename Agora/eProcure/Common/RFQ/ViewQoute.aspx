<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="ViewQoute.aspx.vb" Inherits="eProcure.ViewQoute" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
		<title>ViewQoute</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim bubblepopupcss As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "jquery.bubblepopup.v2.3.1.css") & """ rel='stylesheet' type='text/css'>"
            Dim bubblepopupjquery As String = "<script type=""text/javascript"" src="""& dDispatcher.direct("Plugins/include","jquery.bubblepopup.v2.3.1.min.js") &""">"            
        </script> 
		<% Response.Write(Session("WheelScript"))%>
		<% Response.Write(bubblepopupcss)%>
		<% Response.Write(Session("JQuery")) %>
		<% Response.Write(bubblepopupjquery) %>
		<% Response.Write("</script>") %>
		<script type="text/javascript">
		
		$(document).ready(function(){
        <%  Response.Write(Session("jqPopup")) %>
            
       });
       
	<!--
		function PopWindow(myLoc)
		{
			window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
	//-->
		</script>
  </head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_RFQ_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="header">Quotation
					</td>
					<td></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;Quotation Number :
						<asp:label id="lbl_quoteNum" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol">
						<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" width="300" border="0">
							<tr>
								<td class="tableheader" colspan="4">&nbsp;Quotation&nbsp; Details</td>
							</tr>
							<tr id="tr_curr" runat="server">
								<td class="tablecol" width="150"><strong>&nbsp;Currency</strong> :</td>
								<td class="tableinput ">&nbsp;<asp:label id="lbl_Currency" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol"></td>
								<td class="tableinput "></td>
							</tr>
							<tr>
								<td class="tablecol" width="150"><strong>&nbsp;Date Created</strong> :</td>
								<td class="tableinput ">&nbsp;<asp:label id="lbl_CreateDate" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol"><strong>&nbsp;Contact Person</strong>&nbsp;:</td>
								<td class="tableinput ">&nbsp;<asp:label id="lbl_ContactPer" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol"><strong>&nbsp;Quotation Validity</strong> :</td>
								<td class="tableinput ">&nbsp;<asp:label id="lbl_QuoteVal" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol"><strong>&nbsp;Contact Number</strong> :</td>
								<td class="tableinput ">&nbsp;<asp:label id="lbl_ContNum" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol"><strong>&nbsp;From</strong> :
								</td>
								<td class="tableinput ">&nbsp;<asp:label id="lbl_From" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol"><strong>&nbsp;Email Address </strong>:</td>
								<td class="tableinput ">&nbsp;<asp:label id="lbl_Email" runat="server" CssClass="lblinfo"></asp:label></td>

							</tr>
							<tr>
								<td class="tablecol"><strong>&nbsp;Physical Address</strong> :</td>
								<td class="tableinput ">&nbsp;<asp:label id="lbl_PhyAdds" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol">&nbsp;<strong>Remarks </strong>:</td>
								<td class="tableinput ">&nbsp;<asp:textbox id="txt_remark" runat="server" Width="190px" TextMode="MultiLine" Height="44px"
										 contentEditable="false"  CssClass="lblinfo"></asp:textbox></td>

							</tr>							
							<tr>
								<td class="tablecol"><strong>&nbsp;File attachment(s) </strong>:</td>
								<td class="tableinput ">&nbsp;<asp:panel id="pnlAttach2" runat="server"></asp:panel></td>
								<td class="tablecol" colspan="2"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td><asp:datagrid id="dg_viewitem" runat="server" CssClass="grid" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="RRD_Vendor_Item_Code" HeaderText="Vendor Item Code " Visible="False">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RRD_Product_Desc"  readonly="true"  HeaderText="Item Name">
									<HeaderStyle Width="25%"></HeaderStyle>
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
									<HeaderStyle Width="6%" HorizontalAlign="Right"></HeaderStyle>
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
								<asp:TemplateColumn HeaderText="SST Rate">
									<HeaderStyle Width="3%" HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_gst_rate" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="SST Amount">
									<HeaderStyle Width="3%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_gst_amt" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Tax Amount">
									<HeaderStyle Width="3%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_tax" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="RRD_DEL_CODE" HeaderText="Delivery </br>Term">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
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
									<HeaderStyle Width="13%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td>
						<input type="button" value="View Quotation" id="cmdView" runat="server" class="button" style="width: 83px"/>
					</td>
				</tr>
			
				<tr>
					<td>&nbsp;</td>
				</tr>
			    <tr>
					<td class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<strong>&lt; Back</strong></asp:hyperlink></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
			</table>
		</form>
	</body>
</html>
