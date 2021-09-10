<%@ Page Language="vb" AutoEventWireup="false" Codebehind="POListS.aspx.vb" Inherits="eProcure.POListS" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Purchase Order List</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<%Response.Write(Session("WheelScript"))%>
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
	<body >
		<form id="Form1" method="post" runat="server">
            <TD class="emptycol">
                <TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
                <TR>
					    <TD class="header" colSpan="4" style="height: 20px"><FONT size="1">&nbsp;</FONT><asp:label id="lblTitle" runat="server" Height="20px">Purchase Order List</asp:label></TD>
			    </TR>
                <%--<TR>
				    <TD >
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Click the PO Number to go to PO Details page."
					    ></asp:label>

				    </TD>
			    </TR>--%>
                </TABLE>
            </TD>
            <TR>
	            <TD class="emptycol" style="width: 751px; height: 24px;" colSpan="6" ></TD>
            </TR>
          
			<TD><asp:datagrid id="dtgPO" runat="server" OnSortCommand="SortCommand_Click">
				<Columns>
					<asp:TemplateColumn SortExpression="POM_PO_No" HeaderText="PO Number">
						<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink Runat="server" ID="lnkPONo"></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="POM_PO_Date" SortExpression="POM_PO_Date" HeaderText="PO Date">
						<HeaderStyle HorizontalAlign="Left" Width="9%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="POM_S_COY_NAME" SortExpression="POM_S_COY_NAME" HeaderText="Vendor Company">
						<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="POM_PO_STATUS" SortExpression="POM_PO_STATUS" HeaderText="Status">
						<HeaderStyle HorizontalAlign="Right" Width="9%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="POD_ORDERED_QTY" SortExpression="POD_ORDERED_QTY" HeaderText="Quantity">
						<HeaderStyle HorizontalAlign="Right" Width="9%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundColumn>
				</Columns>
			</asp:datagrid></TD>
			<tr valign="top" >
				<td align="right"><asp:label Style="text-align: right" id="lblTotal"  runat="server" text="Total Quantity:" Width="750px" Font-Bold="True" CssClass="lblInfo" Font-Size="9"></asp:label></td>
		    </tr>
			<tr valign="top" >
				<td align="right"><asp:label Style="text-align: right" id="lblTotalV"  runat="server" text="0.00" Width="105px" Font-Bold="True" CssClass="lblInfo" Font-Size="9"></asp:label></td>
			</tr>
			<%--<TR>
			    <TD align="right">
			        <asp:Label CssClass="header" id="lblTotal" Width="500px" Runat="server" Text="Total Quantity:" Font-Bold="True"></asp:Label>
	                <asp:Label CssClass="header" id="lblTotalV" Width="150px" Runat="server" Text="0.00" Font-Bold="True"></asp:Label>
	            </TD>	            
            </TR>--%><br/>            
            <TR>
	            <TD class="emptycol" style="width: 751px; height: 24px;" colSpan="6" ></TD>
            </TR><br /><br />
            <TR>
			    <td class="emptycol" colspan="6">				
			        <input type="button" name="cmd_back" value="Close" onclick="window.close(); " id="cmd_back" class="Button" />
                </td>
            </TR>
		</form>
	</body>
</HTML>
