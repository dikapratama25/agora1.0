<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HubListPriceCatalogue.aspx.vb" Inherits="eAdmin.HubListPriceCatalogue"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>List Price Catalogue Approval</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
       <script runat="server">
           Dim dDispatcher As New AgoraLegacy.dispatcher
           Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
       </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		function PopWindow(myLoc)
		{
			//window.open(myLoc,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight: 255px; dialogWidth: 300px");
			window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
			return false;
		}
		
			function selectAll()
			{
				SelectAllG("dtgCatalogue__ctl2_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCatalogue__ctl2_chkAll","chkSelection");
			}
		
			function Reset(){
				var oform = document.forms(0);
				oform.txtItemId.value="";
				oform.txtDesc.value="";
				oform.txtVendorItemCode.value="";
				checkStatus(false);
			}
			
			function SelectAll_1()
			{
				checkStatus(true);
			}
		
			function checkStatus(checked)
			{
				var oform = document.forms(0);
				if (Form1.hidType.value == "A"){
					oform.chkReject.checked=checked;
					oform.chkHubPending.checked=checked;
					oform.chkHubApprove.checked=checked;	
				}
				//oform.chkHubApprove.checked=checked;	
			}
		
			/*function CheckDelete(pChkSelName){
				var oform = document.forms[0];
				var itemCnt, itemCheckCnt;
				var result, result2;
				itemCheckCnt = 0;
				
				re = new RegExp(':' + pChkSelName + '$');  //generated control name starts with a colon	
				for (var i=0;i<oform.elements.length;i++){
					var e = oform.elements[i];
					if (e.type=="checkbox"){						
						if (re.test(e.name)){
							if (e.checked==true)
								itemCheckCnt ++;
						}
					}
				}
				
				if (itemCheckCnt == 0) {
					alert ('Please make at least one selection!');
					return false;
				}
				else{
					return confirm('Are you sure that you want to permanently delete this item(s) ? \n This item(s) will be deleted from Contract Catalogue and Discount Group also (if any).');
				}				
			}*/
		-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" colspan="6"><FONT size="1"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="6">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="tableheader" colspan="6">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
				    <TD class="tablecol" width="15%" nowrap>&nbsp;<STRONG>Vendor Item Code</STRONG>&nbsp;:</TD>
					<TD class="tablecol" width="25%" nowrap><asp:textbox id="txtVendorItemCode" runat="server" CssClass="txtbox" Width="150px"></asp:textbox></TD>
					<TD class="tablecol" width="10%" nowrap>&nbsp;<STRONG>Item Name</STRONG>&nbsp;:</TD>
					<TD class="tablecol" width="22%" nowrap><asp:textbox id="txtDesc" runat="server" CssClass="txtbox" Width="150px"></asp:textbox></TD>
					<TD class="tablecol" width="10%" nowrap></TD>
					<TD class="tablecol" width="18%" nowrap><input id="txtItemId" runat="server" type="hidden" /></TD>
					
					
				</TR>
				<tr class="tablecol" runat="server" id="trStatus">
					<td>&nbsp;<STRONG>Status</STRONG> :</td>
					<td colspan="3" nowrap><asp:checkbox id="chkHubPending" Text="Pending For Approval" Runat="server"></asp:checkbox>
					<asp:checkbox id="chkReject" Text="Rejected By Hub Admin" Runat="server"></asp:checkbox>
					<asp:checkbox id="chkHubApprove" Text="Approved By Hub Admin" Runat="server"></asp:checkbox>
					</td>
			        <TD colspan="3" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
						<INPUT class="button" id="cmdSelectAll" onclick="SelectAll_1();" type="button" value="Select All"
							name="cmdSelectAll" runat="server"> <INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"><INPUT class="txtbox" id="hidType" style="WIDTH: 43px; HEIGHT: 18px" type="hidden" size="1"
							name="hidType" runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="6">&nbsp;</TD>
				</TR>
				<tr>
					<td class="emptycol" colspan="6"><asp:datagrid id="dtgCatalogue" runat="server" OnSortCommand="SortCommand_Click" AutoGenerateColumns="false">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="PM_PRODUCT_CODE" HeaderText="Item Code">
									<HeaderStyle Width="21%"></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
										<asp:Label Runat="server" ID="lblCode" Visible="False"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PM_PRODUCT_INDEX" SortExpression="PM_PRODUCT_INDEX" HeaderText="Index" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_VENDOR_ITEM_CODE" SortExpression="PM_VENDOR_ITEM_CODE" HeaderText="Vendor Item Code" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="PM_CURRENCY_CODE" SortExpression="PM_CURRENCY_CODE" HeaderText="Currency">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_UNIT_COST" SortExpression="PM_UNIT_COST" HeaderText="Price">
									<HeaderStyle Width="7%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM" HeaderText="UOM">
									<HeaderStyle Width="6%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_REMARK" SortExpression="PM_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="12%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_ENT_DT" SortExpression="PM_ENT_DT" HeaderText="Submission Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_ACTION" SortExpression="PM_ACTION" HeaderText="Action">
									<HeaderStyle Width="7%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status">
									<HeaderStyle Width="12%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_STATUS" SortExpression="PM_STATUS" HeaderText="Status" Visible="False"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<TR>
					<TD class="emptycol" colspan="6"></TD>
				</TR>
				<tr>
					<td colspan="6" runat="server" id="trAdd">
						<asp:button id="cmdAdd" runat="server" CssClass="Button" Text="Add"></asp:button>
						<asp:button id="cmdModify" runat="server" CssClass="Button" Text="Modify"></asp:button>
						<asp:button id="cmdDelete" runat="server" CssClass="Button" Text="Delete"></asp:button><INPUT class="txtbox" id="hidIndex" style="WIDTH: 43px; HEIGHT: 18px" type="hidden" size="1"
							name="hidIndex" runat="server"></td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
