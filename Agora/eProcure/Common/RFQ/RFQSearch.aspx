<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQSearch.aspx.vb" Inherits="eProcure.RFQSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
<HTML>
<HEAD>
		<title>VendorRFQList</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		
			function cmdAddClick()
			{
				var result = confirm("Are you sure that you want to pamanently detele this item(s)?", "Yes", "No");
				if(result == true)
					Form1.hidAddItem.value = "1";
				else 
					Form1.hidAddItem.value = "0";
			}
			function selectAll()
			{
				SelectAllG("dtg_VendorList_ctl02_chkAll","chkSelection");
			}
			
			function checkChild(id)
			{
				checkChildG(id,"dtg_VendorList_ctl02_chkAll","chkSelection");
			}
				
			function selectAll2()
			{
				SelectAllG("dtg_quote_ctl02_chkAll2","chkSelection2");
			}
			
			function checkChild2(id)
			{
				checkChildG(id,"dtg_quote_ctl02_chkAll2","chkSelection2");
			}
				
			function selectAll3()
			{
				SelectAllG("dtg_trash_ctl02_chkAll3","chkSelection3");
			}
			
			function checkChild3(id)
			{
				checkChildG(id,"dtg_trash_ct0l2_chkAll3","chkSelection3");
			}
			
			-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_RFQ_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">				
				 <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			</TR>
			<TR>
				<TD >
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Fill in the search criteria and click Search button to list the relevant RFQ or Quotation. To delete the quotation, check the box and click the Delete button. Click the Reply or Resubmit link to send the quotation to selected buyer."></asp:label>
				</TD>
			</TR>
            <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
				
				<TR>
					<TD colSpan="2">
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="TableHeader" colSpan="2">&nbsp;Search Criteria</TD>
							</TR>
							<TR class="tablecol">
								<TD width="100%">&nbsp;<STRONG>RFQ / Quotation Number&nbsp;</STRONG>:
									<asp:textbox id="txt_Num" runat="server" CssClass="txtbox" Width="104px"></asp:textbox>&nbsp;&nbsp;
									<B>Purchaser Company </B> :
									<asp:textbox id="txt_com_name" runat="server" CssClass="txtbox"></asp:textbox>&nbsp;
								<TD align="right"><asp:button id="cmd_Search" runat="server" CssClass="button" Width="56px" Text="Search"></asp:button>&nbsp;
									<asp:button id="cmd_clear" runat="server" CssClass="button" Width="40px" Text="Clear"></asp:button></TD></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>				
				<TR>
					<TD vAlign="top" colSpan="2"></TD>
				</TR>
				<TR>
					<TD colSpan="2"><asp:datagrid id="dtg_quote" runat="server" CssClass="grid" OnPageIndexChanged="dtg_Page"
							OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll2" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection2" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RRM_Actual_Quot_Num" HeaderText="Quotation Number">
									<HeaderStyle Width="18%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_quo" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RM_RFQ_No" HeaderText="RFQ Number">
									<HeaderStyle Width="13%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_RFQ_Num" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="RM_RFQ_Name" SortExpression="RM_RFQ_Name" HeaderText="RFQ Description">
									<HeaderStyle Width="19%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RM_Created_On" HeaderText="Creation Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RRM_Offer_Till" HeaderText="Quotation Validity ">
									<HeaderStyle Width="9%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Purchaser Company">
									<HeaderStyle Width="22%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Status">
									<HeaderStyle Width="6%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_status1" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="RM_RFQ_ID"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EMPTYCOL" colSpan="2"></TD>
				</TR>
				<TR>
					<TD vAlign="top" colSpan="2">
						<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD align="left" width="70%"><asp:button id="cmdDelete" runat="server" CssClass="button" Width="70px" Text="Delete"></asp:button>
										<asp:label id="lblCurrentIndex" runat="server" CssClass="lbl"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
