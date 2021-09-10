<%@ Page Language="vb" AutoEventWireup="false" Codebehind="POList.aspx.vb" Inherits="eProcure.POList" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>AddDO</title>
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

              <%  Response.Write(Session("w_POList_tabs"))%>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
 <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			</TR>			
            <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
            <tr>
					<TD class="header" colSpan="4" style="height: 20px"><FONT size="1">&nbsp;</FONT><asp:label id="lblTitle" runat="server" Height="20px">Outstanding PO</asp:label></TD>
			</TR>
            <TR>
				<TD >
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Click the PO Number to go to DO Details page."
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
								<asp:BoundColumn DataField="Due_Date" SortExpression="Due_Date" HeaderText="Due Date">
									<HeaderStyle HorizontalAlign="Left" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Buyer Company">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_D_ADDR_CODE" SortExpression="POD_D_ADDR_CODE" HeaderText="Delivery Address Code">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Tot" SortExpression="Tot" HeaderText="Ordered Qty">
									<HeaderStyle HorizontalAlign="Right" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Outs" SortExpression="Outs" HeaderText="Outstanding Qty">
									<HeaderStyle HorizontalAlign="Right" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>

		
            
		</form>
	</body>
</HTML>
