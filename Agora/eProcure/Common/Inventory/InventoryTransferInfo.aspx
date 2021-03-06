<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InventoryTransferInfo.aspx.vb" Inherits="eProcure.InventoryTransferInfo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Inventory Transfer Info</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("JQuery")) %>	
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
        <% Response.Write(css)%>
        <% Response.Write(Session("WheelScript"))%>
        
		<script language="javascript">
		    function PopWindow(myLoc)
	        {
		        window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
		        return false;
	        }
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_InventoryTran_tabs"))%>
			<TABLE  class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
			<tr><td colspan="5" class="rowspacing"></td></tr>
			<TR>
			<TD class="TableHeader" colSpan="5">Inventory Transfer Detail</TD>
		    </TR>
						
			<tr >
				<td class="tablecol" align="left" width="18%" style="height: 19px">&nbsp;<strong><asp:label id="Label3" runat="server" >IT Number</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" width="50%" style="height: 19px"><asp:label id="lblITNo" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
			</tr>
			<tr >
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label4" runat="server" >Transfer Date</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:label id="lblTransferDate" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
			</tr>
			<tr >
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label1" runat="server" >Reference No</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:label id="lblRefNo" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
			</tr>
			<tr >
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label2" runat="server" >Remark</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:label id="lblRemark" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
			</tr>
			
			<TR>
				<TD class="emptycol" colSpan="6" ></TD>
			</TR>
			</table>
			
            <TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="100%">
			<TR>
            
            <%--<asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">--%>
            <td class="EmptyCol" colspan="6">
            <asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">
            <Columns>
                        
            <asp:BoundColumn DataField="IM_ITEM_CODE" HeaderText="Item Code"></asp:BoundColumn>
            <asp:BoundColumn DataField="IT_INVENTORY_NAME" HeaderText="Item Name"></asp:BoundColumn>
            <asp:BoundColumn DataField="IT_TRANS_QTY" HeaderText="Qty">
                <HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundColumn>	    
            <asp:BoundColumn DataField="LM_LOCATION" HeaderText="From Location"></asp:BoundColumn>
            <asp:BoundColumn DataField="LM_SUB_LOCATION" HeaderText="From Sub Location"></asp:BoundColumn>
            <asp:BoundColumn DataField="LM_LOCATION2" HeaderText="To Location"></asp:BoundColumn>
            <asp:BoundColumn DataField="LM_SUB_LOCATION2" HeaderText="To Sub Location"></asp:BoundColumn>            
            </Columns>
            </asp:datagrid></td>
            </tr>
			</table>
			
			<tr>
				<td class="EmptyCol"><br>
					<INPUT type="button" value="View" id="cmdPrint" runat="server" Class="button" style="width: 50px">
					
			    </TD>
			</tr>
			<div class="emptycol"></div>
			<div class="emptycol"><asp:hyperlink id="lnkBack" Runat="server"><STRONG>&lt; Back</STRONG></asp:hyperlink></div>
			
		</form>
	</body>
</HTML>
