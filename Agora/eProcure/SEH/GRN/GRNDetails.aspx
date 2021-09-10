<%@ Page Language="vb" EnableSessionState="ReadOnly" EnableViewState="false" AutoEventWireup="false" Codebehind="GRNDetails.aspx.vb" Inherits="eProcure.GRNDetailsSEH" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>GRNDetails</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
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
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
	//-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchGRN_tabs"))%>
			<table class="alltable" id="tblDOHeader" cellspacing="0" cellpadding="0" border="0">				
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
				<tr>
					<td class="tableheader" colspan="6">&nbsp;<asp:label id="lblHeader" runat="server">Goods Receipt Note Details</asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" width="18%"><strong>&nbsp;PO Number</strong> :</td>
					<td class="tableinput" width="18%"><asp:label id="lblPONo" runat="server"></asp:label></td>
					<td class="tablecol" width="17%">&nbsp;<strong>PO Date </strong>:</td>
					<td class="tableinput" width="17%"><asp:label id="lblPODate" runat="server"></asp:label></td>
					<td class="tablecol" width="15%">&nbsp;<strong><asp:Label ID="lblGrn" runat="server" Text="GRN Value"></asp:Label></strong> :</td>
					<td class="tableinput" width="15%"><asp:label id="lblGrnValue" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" width="18%"><strong>&nbsp;DO Number</strong> :</td>
					<td class="tableinput" width="18%"><asp:label id="lblDONo" runat="server"></asp:label></td>
					<td class="tablecol" width="17%"><strong>&nbsp;Created By </strong>:</td>
					<td class="tableinput" width="17%" colspan="3"><asp:label id="lblCreatedBy" runat="server"></asp:label></td>
				</tr>				
				<tr>
					<td class="tablecol" width="18%"><strong>&nbsp;GRN Number </strong>:</td>
					<td class="tableinput" width="18%"><asp:label id="lblGrnNo" runat="server"></asp:label></td>					
					<td class="tablecol" width="17%"></td>
					<td class="tableinput" width="17%" colspan="3"></td>
				</tr>
				<tr>				    
					<td class="tablecol" width="18%"><strong>&nbsp;GRN Date </strong>:</td>
					<td class="tableinput" width="18%"><asp:label id="lblGRNDate" runat="server"></asp:label></td>
					<td class="tablecol" width="17%"><strong>&nbsp;Actual Goods Received Date </strong>:</td>
					<td class="tableinput" width="17%" colspan="3"><asp:label id="lblDtREceived" runat="server"></asp:label></td>
				</tr>
				<tr id="tr_SEH1" style="display:none" runat="server">				    
					<td class="tablecol" width="18%"><strong>&nbsp;<asp:Label ID="lblDL" runat="server" Text="Label"></asp:Label></strong> :</td>
					<td class="tableinput" width="18%"><asp:label id="lblDefaultLocation" runat="server"></asp:label></td>
					<td class="tablecol" width="17%"><strong>&nbsp;<asp:Label ID="lblSDL" runat="server" Text="Label"></asp:Label></strong> :</td>
					<td class="tableinput" width="17%" colspan="3"><asp:label id="lblDefaultSubLocation" runat="server"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tablecol"><strong>&nbsp;<asp:label id="lblExtAttach" text="DO File(s) Attached" runat="server"></asp:label></strong> :</td>
					<td class="tableinput" valign="top" colspan="3">&nbsp;<asp:label id="lblFileAttach" runat="server"></asp:label></td>
					<td class="tableinput"></td>
					<td class="tableinput" colspan="3"></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="6">&nbsp;&nbsp;</td>
				</tr>
			</table>
			<table class="alltable" cellspacing="0" cellpadding="0" border="0" width="100%">
				<tr>
					<td><asp:datagrid id="dtgGrnDtlStk" runat="server" OnSortCommand="SortCommand_Click" AllowSorting="True">
							<Columns>
								<asp:BoundColumn DataField="GD_PO_LINE" SortExpression="GD_PO_LINE" HeaderText="Line" Visible="false">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Line" SortExpression="LineNo"></asp:BoundColumn>
								<asp:BoundColumn DataField="POD_VENDOR_ITEM_CODE" SortExpression="POD_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PRODUCT_DESC" SortExpression="POD_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_B_GL_CODE" SortExpression="POD_B_GL_CODE" HeaderText="(GL Code) <br>GL Description">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POM_CURRENCY_CODE" SortExpression="POM_CURRENCY_CODE" HeaderText="Currency">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UNIT_COST" SortExpression="POD_UNIT_COST" HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="Right" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Disc Amt">
								    <HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
								        <asp:label id="lblDiscAmt" runat="server"></asp:label>
								    </ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="GD_OTH_CHARGE" SortExpression="GD_OTH_CHARGE" HeaderText="Oth Charges (F)">
									<HeaderStyle HorizontalAlign="Right" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Unit Cost(M)">
								    <HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
								        <asp:label id="lblUnitCost" Width="50px" runat="server"></asp:label>
								    </ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Amount(F)" >
								    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
									    <asp:label id="lblAmountF" Width="50px" Runat="server"></asp:label>
								    </ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Amount(M)" >
								    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
									    <asp:label id="lblAmountM" Width="50px" Runat="server"></asp:label>
								    </ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="GRN Factor" >
								    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
									    <asp:label id="lblGRNFac" Width="50px" Runat="server"></asp:label>
								    </ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="GD_INLAND_CHARGE" SortExpression="GD_INLAND_CHARGE" HeaderText="Inland Charges(M)">
									<HeaderStyle HorizontalAlign="Right" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="CIF Values" >
								    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
									    <asp:label id="lblCIF" Width="50px" Runat="server"></asp:label>
								    </ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="GD_DUTIES" SortExpression="GD_DUTIES" HeaderText="Duties (M)">
									<HeaderStyle HorizontalAlign="Right" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Landed Cost" >
								    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
									    <asp:label id="lblLandCost" Width="50px" Runat="server"></asp:label>
								    </ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="POD_SPEC1" SortExpression="POD_SPEC1" HeaderText="Spec 1">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_SPEC2" SortExpression="POD_SPEC2" HeaderText="Spec 2">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_SPEC3" SortExpression="POD_SPEC3" HeaderText="Spec 3">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Lot No">
								    <HeaderStyle HorizontalAlign="Right" Width="5%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
								    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								    <ItemTemplate>
								        <asp:label id="lblLotNo" runat="server"></asp:label>
								    </ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="POD_MIN_PACK_QTY" SortExpression="POD_MIN_PACK_QTY" HeaderText="MPQ">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" SortExpression="POD_ORDERED_QTY" HeaderText="PO Qty">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOD_SHIPPED_QTY" SortExpression="DOD_SHIPPED_QTY" HeaderText="Shipped Qty">
									<HeaderStyle HorizontalAlign="Right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GD_RECEIVED_QTY" SortExpression="GD_RECEIVED_QTY" HeaderText="GRN Qty">
									<HeaderStyle HorizontalAlign="Right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GD_REJECTED_QTY" SortExpression="GD_REJECTED_QTY" HeaderText="Rejected Qty">
									<HeaderStyle HorizontalAlign="Right" Width="7%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GD_REMARKS" SortExpression="GD_REMARKS" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkDetails"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td style="height: 24px">
						<input type="button" value="View GRN" id="cmdPreviewGRN" runat="server" class="button" style="width: 96px"/>
					</td>
				</tr>
				<TR>
			        <TD class="emptycol">&nbsp;&nbsp;</TD>
		        </TR>
				<tr>
					<td class="emptycol"><asp:hyperlink id="lnkBack" Runat="server"><strong>&lt; Back</strong></asp:hyperlink></td>
				</tr>
			</table>
		</form>
	</body>
</html>
