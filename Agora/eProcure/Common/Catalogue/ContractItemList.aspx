<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ContractItemList.aspx.vb" Inherits="eProcure.ContractItemList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
	<HEAD>
		<title>Contract Item List</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher           
       </script>
        <% Response.Write(Session("WheelScript"))%>	
        <script type="text/javascript">
        function closepop()
        {
            window.close();
        }
        </script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
		<asp:datagrid id="dtgCatalogue" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
			<Columns>
			    <asp:BoundColumn DataField="cdi_vendor_item_code" SortExpression="cdi_vendor_item_code" HeaderText="Item Code">
				</asp:BoundColumn>																							
				<asp:BoundColumn DataField="cdi_product_desc" SortExpression="cdi_product_desc" HeaderText="Item Name">
				</asp:BoundColumn>	
				<asp:BoundColumn DataField="cdi_currency_code" SortExpression="cdi_currency_code" HeaderText="Currency">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="cdi_unit_cost" SortExpression="cdi_unit_cost" HeaderText="Price">
				</asp:BoundColumn>		
				<asp:BoundColumn DataField="cdi_gst_rate" SortExpression="cdi_gst_rate" HeaderText="SST Rate">					
				</asp:BoundColumn>		
				<asp:BoundColumn DataField="cdi_gst_tax_code" SortExpression="cdi_gst_tax_code" HeaderText="SST Tax Code">					
				</asp:BoundColumn>
				<asp:BoundColumn DataField="cdi_uom" SortExpression="cdi_uom" HeaderText="UOM">					
				</asp:BoundColumn>
				<asp:BoundColumn DataField="cdi_remark" SortExpression="cdi_remark" HeaderText="Remarks">				
				</asp:BoundColumn>			
			</Columns>
		</asp:datagrid>
		<input type="button" id="cmdClose" class="Button" onclick="closepop();" value="Close" />				
		</form>
	</body>
</HTML>
