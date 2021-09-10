<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RFQPOList.aspx.vb" Inherits="eProcure.RFQPOList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Comlist</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		 <script language="javascript">
		<!--
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}	
		-->
		</script>	
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header"><FONT size="1">&nbsp;</FONT><asp:label id="lblTitle" runat="server">Purchase Order List</asp:label></TD>
				</TR>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<TR>
	                <TD >
	                    <asp:label id="lbl_rfq" runat="server"  CssClass="lblInfo"></asp:label>
	                </TD>
                </TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtg_polist" runat="server" OnSortCommand="SortCommand_Click" CssClass="grid"
							AutoGenerateColumns="False">
							<Columns>
								<%--<asp:BoundColumn DataField="PRD_CONVERT_TO_DOC" HeaderText="RFQ No">
									<HeaderStyle Width="50%"></HeaderStyle>
								</asp:BoundColumn>--%>	
								<asp:BoundColumn SortExpression="POM_PO_NO" DataField="POM_PO_NO" HeaderText="PO No">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>	
								<asp:BoundColumn SortExpression="POM_PO_DATE" DataField="POM_PO_DATE" HeaderText="Order Date">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>	
								<asp:BoundColumn SortExpression="POM_S_COY_NAME" DataField="POM_S_COY_NAME" HeaderText="Vendor Name">
									<HeaderStyle Width="28%"></HeaderStyle>
								</asp:BoundColumn>	
								<asp:BoundColumn SortExpression="POM_CURRENCY_CODE" DataField="POM_CURRENCY_CODE" HeaderText="Currency">
									<HeaderStyle Width="18%"></HeaderStyle>
								</asp:BoundColumn>	
								<asp:BoundColumn SortExpression="POM_PO_STATUS" DataField="POM_PO_STATUS" HeaderText="Status">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>	
								<asp:BoundColumn SortExpression="PRD_PR_NO" DataField="PRD_PR_NO" HeaderText="PR No">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">
					    <asp:button id="cmdClose" runat="server" CssClass="button" Text="Close" CausesValidation="false" ></asp:button>&nbsp;
					</TD>
				</TR>
				
			</TABLE>
		</form>
	</body>
</HTML>
