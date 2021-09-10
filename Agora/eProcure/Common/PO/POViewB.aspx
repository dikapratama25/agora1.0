<%@ Page Language="vb" AutoEventWireup="false" Codebehind="POViewB.aspx.vb" Inherits="eProcure.POViewB" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>POViewB</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">		
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		
		-->
		</script>
	</HEAD>
	<body >
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_VendorPOAck_tabs"))%>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
                     <tr>
					    <TD class="linespacing1" colSpan="4"></TD>
			        </TR>			       
                    <tr>
					        <TD class="linespacing2" colSpan="4"></TD>
			        </TR>
                    <tr>
					    <TD class="header" colSpan="4" style="height: 20px"><FONT size="1">&nbsp;</FONT><asp:label id="lblTitle" runat="server" Height="20px">Pending Acceptance / Acknowledgement PO</asp:label></TD>
					</TR>
                    <TR>
				        <TD >
					        <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					        Text="Click the PO Number to see the PO details."
					        ></asp:label>

				        </TD>
			        </TR>
                       </TABLE>
                    </TD>
  				<TR>
					<TD class="emptycol" style="width: 751px; height: 24px;" colSpan="6" ></TD>
				</TR>
       					<TD><asp:datagrid id="dtgPO" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn SortExpression="POM_PO_No" HeaderText="PO Number">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lbl_po_no"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn visible="false" DataField="POM_PO_INDEX" SortExpression="POM_PO_INDEX"  HeaderText="POM_PO_INDEX">
									<HeaderStyle HorizontalAlign="Right" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POM_PO_Date" SortExpression="POM_PO_Date" HeaderText="PO Date">
									<HeaderStyle HorizontalAlign="Left" Width="15%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Buyer Company">
									<HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POM_BUYER_NAME" SortExpression="POM_BUYER_NAME" HeaderText="Purchaser">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="PO Status">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CR_NO" visible="false" SortExpression="CR_NO" HeaderText="CR_NO">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
   
            
		</form>
	</body>
</HTML>
