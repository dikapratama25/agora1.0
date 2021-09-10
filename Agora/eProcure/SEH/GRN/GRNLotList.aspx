<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GRNLotList.aspx.vb" Inherits="eProcure.GRNLotList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>GRNLotList</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		
		<% Response.Write(Session("WheelScript"))%>
		<% Response.Write(Session("JQuery")) %>
		
    	<script type="text/javascript">
		<!--
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}	
		-->
		</script>	
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="header"><font size="1">&nbsp;</font><asp:label id="lblTitle" runat="server">Item Lot No</asp:label></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
	                <td >
	                    <asp:label id="lbl_item" runat="server"  CssClass="lblInfo"></asp:label>
	                </td>
                </tr>
                <tr>
                    <td >
	                    <asp:label id="lbl_grn" runat="server"  CssClass="lblInfo"></asp:label>
	                </td>
                </tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtg_lotlist" runat="server" OnSortCommand="SortCommand_Click" CssClass="grid"
							AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn SortExpression="DOL_LOT_QTY" DataField="DOL_LOT_QTY" HeaderText="Lot Qty">
									<HeaderStyle Width="7%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>	
								<asp:BoundColumn SortExpression="DOL_LOT_NO" DataField="DOL_LOT_NO" HeaderText="Lot No">
									<HeaderStyle Width="13%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Continue Lot">
								        <HeaderStyle HorizontalAlign="Left" Width="10%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        <ItemTemplate>
								            <asp:label id="lblCLot" runat="server"></asp:label>
								        </ItemTemplate>
								    </asp:TemplateColumn>	
								<asp:BoundColumn SortExpression="DOL_DO_MANU_DT" DataField="DOL_DO_MANU_DT" HeaderText="Mfg Date">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>	
								<asp:BoundColumn SortExpression="DOL_DO_EXP_DT" DataField="DOL_DO_EXP_DT" HeaderText="Expiry Date">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>	
								<asp:BoundColumn SortExpression="DOL_DO_MANUFACTURER" DataField="DOL_DO_MANUFACTURER" HeaderText="Mfg Name">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Attachment">
									<HeaderStyle HorizontalAlign="Left" Width="30%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
									<ItemTemplate>
									    <asp:label id="lblFileAttach" runat="server"></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>	
								<asp:BoundColumn SortExpression="DOL_ITEM_LINE" DataField="DOL_ITEM_LINE" HeaderText="ItemLine" Visible="false">
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="DOL_LOT_INDEX" DataField="DOL_LOT_INDEX" HeaderText="Index" Visible="false">
								</asp:BoundColumn>
								<%--<asp:BoundColumn SortExpression="PRD_PR_NO" DataField="PRD_PR_NO" HeaderText="Attachment">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>--%>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol">
					    <asp:button id="cmdClose" runat="server" CssClass="button" Text="Close" CausesValidation="false" ></asp:button>&nbsp;
					</td>
				</tr>
				
			</table>
		</form>
	</body>
</html>
