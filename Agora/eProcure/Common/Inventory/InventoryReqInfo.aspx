<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InventoryReqInfo.aspx.vb" Inherits="eProcure.InventoryReqInfo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Inventory Requisition Info</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("WheelScript"))%>
        
		<script type="text/javascript">
		    function PopWindow(myLoc)
	        {
		        window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
		        return false;
	        }
		</script>
	</head>
	<body class="body">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_InventoryReq_tabs"))%>
			<table  class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%">
			<tr><td colspan="6" class="rowspacing"></td></tr>
			<tr>
			<td class="TableHeader" colspan="6">Inventory Requisition Detail</td>
		    </tr>			
			<tr>
				<td class="tablecol" align="left" width="8%" style="height: 19px">&nbsp;<strong><asp:label id="Label3" runat="server" >IR Number</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" width="13%" style="height: 19px"><asp:label id="lblIRNo" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol" width="8%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label5" runat="server" >Status</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" width="10%" style="height: 19px"><asp:label id="lblStatus" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol" width="10%"></td>
				<td class="tablecol" width="10%"></td>
			</tr>
			<tr>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label4" runat="server" >Issue Date</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:label id="lblIssueDate" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label7" runat="server" >Approved Date</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:label id="lblApprovedDate" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
				<td class="tablecol"></td>
			</tr>
			<tr>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label8" runat="server" >Requestor Name</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:label id="lblRequestor" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label11" runat="server" >Section</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px" colspan="2"><asp:label id="lblSection" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
			</tr>
			<tr>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="lblIssue" runat="server" >Issue To</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px"><asp:label id="lblIssueTo" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="lblDept" runat="server" >Department</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" style="height: 19px" colspan="2"><asp:label id="lblDepartment" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
			</tr>
			<tr>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label1" runat="server">Reference No</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" colspan="3" style="height: 19px"><asp:label id="lblRefNo" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
				<td class="tablecol"></td>
			</tr>
			<tr>
				<td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label2" runat="server" >Remark</asp:label></strong>&nbsp;:</td>
				<td class="tablecol" colspan="3" style="height: 19px"><asp:label id="lblRemark" runat="server" Width="100%"></asp:label></td>
				<td class="tablecol"></td>
				<td class="tablecol"></td>
			</tr>
			
			<tr>
				<td class="emptycol" colspan="6" ></td>
			</tr>
			</table>
			
            <table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%">
            <tr>
				<td class="tableheader">Approval Workflow</td>
			</tr>
			<tr>
				<td class="EmptyCol" colspan="6"><asp:datagrid id="dtgAppFlow" runat="server" AutoGenerateColumns="False" Width="100%">
					<Columns>
						<asp:BoundColumn DataField="IRA_SEQ" HeaderText="Level">
							<HeaderStyle Width="2%"></HeaderStyle>
							<ItemStyle HorizontalAlign="Right"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
							<HeaderStyle Width="20%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
							<HeaderStyle Width="20%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="IRA_APPROVAL_TYPE" HeaderText="Approval Type">
							<HeaderStyle Width="10%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="IRA_ACTION_DATE" HeaderText="Action Date">
							<HeaderStyle Width="15%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="IRA_AO_REMARK" HeaderText="Remarks">
							<HeaderStyle Width="15%"></HeaderStyle>
						</asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Attachment">
							<HeaderStyle Width="20%"></HeaderStyle>
							<ItemTemplate>
						        <asp:DataList id="DataList1" runat="server" DataSource='<%# CreateFileLinks( DataBinder.Eval( Container.DataItem, "IRA_AO") , DataBinder.Eval( Container.DataItem, "IRA_A_AO") , DataBinder.Eval( Container.DataItem, "IRA_SEQ" ), "IR" ) %>' ShowFooter="False" Width="100%" BorderColor="#0000ff" ShowHeader="False">
								    <ItemTemplate>
									    <%# DataBinder.Eval( Container.DataItem ,"Hyperlink") %>
							        </ItemTemplate>
								</asp:DataList>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:datagrid></td>
			</tr>
			<tr>
				<td class="emptycol" style="height: 19px"></td>
			</tr>
			<tr>
				<td class="tableheader">Inventory Requisition Line Detail</td>
			</tr>
			<tr>
            
            <%--<asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">--%>
                <td class="EmptyCol" colspan="6">
                <asp:datagrid id="dtgItem" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyDataGrid_Page" AutoGenerateColumns="False">
                    <Columns>            
                        <asp:BoundColumn DataField="IM_ITEM_CODE" HeaderText="Item Code"></asp:BoundColumn>
                        <asp:BoundColumn DataField="IRD_INVENTORY_NAME" HeaderText="Item Name"></asp:BoundColumn>
                        <asp:BoundColumn DataField="IRD_UOM" HeaderText="UOM">
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IRD_IR_MTHISSUE" HeaderText="Monthly Stock Issued Accumulative">
                            <HeaderStyle HorizontalAlign="Right" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IRD_IR_LAST3MTH" HeaderText="Last 3 Mths Ave">
                            <HeaderStyle HorizontalAlign="Right" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundColumn>	    
                        <asp:BoundColumn DataField="IRD_QTY" HeaderText="Qty">
                            <HeaderStyle HorizontalAlign="Right" Width="15%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="LM_LOCATION" HeaderText="Location"></asp:BoundColumn>
                        <asp:BoundColumn DataField="LM_SUB_LOCATION" HeaderText="Sub Location"></asp:BoundColumn>            
                    </Columns>
                </asp:datagrid>
                </td>
            </tr>
			</table>
			
			<tr>
				<td class="EmptyCol"><br/>
					<input type="button" value="View" id="cmdPrint" runat="server" class="button" style="width: 50px"/>
					
			    </td>
			</tr>
     		<div class="emptycol"></div>
			<div class="emptycol"><asp:hyperlink id="lnkBack" Runat="server"><strong>&lt; Back</strong></asp:hyperlink></div>
			
			
		</form>
	</body>
</html>
