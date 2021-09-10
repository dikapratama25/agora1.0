<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AppGrpAsgItem.aspx.vb" Inherits="eProcure.AppGrpAsgItem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>AppGrpAsgItem</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--  
		function selectAll()
		{
			SelectAllG("dtgAsgItem_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgAsgItem_ctl02_chkAll","chkSelection");
		}
		
		function PopWindow(myLoc)
		{
			//window.open(myLoc,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight: 255px; dialogWidth: 300px");
			window.open(myLoc,"Wheel","help:No,Height=500,Width=750,resizable=yes,scrollbars=yes");
			return false;
		}
		
		function clear()
		{
			var oform = document.forms(0);
			//alert("ok");
			oform.txtItemCode.value="";
			oform.txtItemName.value="";
		}

		-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ApprWFAsgItem_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
			<tr>
					<td class="linespacing1" colspan="4"></td>
			</tr>
			<tr>
				<td >
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Please select approval group and assign items to selected approval group."></asp:label>

				</td>
			</tr>
				<tr>
					<td class="tablecol">
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0" width="100%">
							<tr>
								<td class="tableheader" colspan="6" width="100%">&nbsp;Search Criteria</td>
							</tr>
							<tr class="tablecol">
							    <td class="tablecol">&nbsp;<strong>Group Type </strong>:</td>
							    <td class="tablecol" colspan="5"><asp:DropDownList id="ddl_Type" runat="server" CssClass="ddl" width="150px" AutoPostBack="true">									
									<asp:ListItem Value="IQC">IQC</asp:ListItem>
							    </asp:DropDownList></td>
							</tr> 
							<tr class="tablecol">
								<td class="tablecol" width="16%" style="height: 22px">&nbsp;<strong>Approval Group </strong>:</td>
								<td class="tablecol" width="28%" style="height: 22px"><asp:DropDownList id="ddl_Approval" runat="server" CssClass="ddl" width="220px" AutoPostBack="true"></asp:DropDownList></td>							
								<td class="tablecol" width="12%" style="height: 22px" >&nbsp;<strong>Department </strong>:</td>
								<td class="tablecol" width="20%" style="height: 22px" ><asp:Label ID="lblDept" runat="server" Text="" Width="100%"></asp:Label></td>
								<td class="tablecol" width="10%" style="height: 22px">&nbsp;<strong>IQC Type </strong>:</td>
								<td class="tablecol" width="14%" style="height: 22px"><asp:Label ID="lblIQCType" runat="server" Text="" Width="100%"></asp:Label></td>
							</tr>
							<tr class="tablecol">
							    <td class="tablecol">&nbsp;<strong>Item Code </strong>:</td>
							    <td class="tablecol"><asp:textbox id="txtItemCode" runat="server" CssClass="txtbox" width="220px"></asp:textbox></td>
							    <td class="tablecol" colspan="4"></td>
							</tr>
							<tr class="tablecol">
							    <td class="tablecol">&nbsp;<strong>Item Name </strong>:</td>
							    <td class="tablecol"><asp:textbox id="txtItemName" runat="server" CssClass="txtbox" width="220px"></asp:textbox></td>
							    <td class="tablecol" colspan="4"></td>
							</tr>
							<tr class="tablecol">
							    <td class="tablecol" colspan="2">&nbsp;
							        <asp:radiobuttonlist ID="rbItemCode" runat="server" BorderStyle="None" CssClass="rbtn" RepeatDirection="Horizontal" AutoPostBack="true"> 
                                        <asp:ListItem Value="1">Available Item Code</asp:ListItem>
							            <asp:ListItem Value="2" Selected="True">Selected Item Code</asp:ListItem>
						            </asp:radiobuttonlist>
							    </td>
							    <td class="tablecol" colspan="4" align="right"><asp:button id="cmd_search" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_clear1" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="3"></td>
				</tr>
			</table>
			<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="emptycol" style="width: 1192px">
						<p><asp:datagrid id="dtgAsgItem" runat="server" OnPageIndexChanged="dtgAsgItem_Page" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False" Width="100%">
								<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="PM_PRODUCT_INDEX" SortExpression="PM_PRODUCT_INDEX" HeaderText="Item Index"></asp:BoundColumn>
								<asp:TemplateColumn SortExpression="PM_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle Width="15%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkItem"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle Width="20%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CT_NAME" SortExpression="CT_NAME" HeaderText="Commodity Type">
									<HeaderStyle Width="20%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM" HeaderText="UOM">
									<HeaderStyle Width="10%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="PM_PRODUCT_CODE" SortExpression="PM_PRODUCT_INDEX" HeaderText="PRODUCT_CODE"></asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS" SortExpression="STATUS" HeaderText="Status">
									<HeaderStyle Width="10%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
							</Columns>
							</asp:datagrid></p>
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 6px; width: 1192px;"></td>
				</tr>
				<tr style="height: 50px">
					<td class="emptycol" style="width: 1192px">&nbsp;<asp:button id="cmd_save" runat="server" CssClass="button" Text="Save" CausesValidation="False"></asp:button><input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server"/>
                        </td>
				</tr>
			</table>
		</form>
	</body>
</html>
