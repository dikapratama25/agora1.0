<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ContractCatalogueItem.aspx.vb" Inherits="eProcure.ContractCatalogueItem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Contract Catalogue Item</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher            
            Dim calStartDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
            Dim calEndDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
        </script>
        <% Response.Write(Session("WheelScript"))%>
        <script type="text/javascript" language="javascript">	
            function PopWindow(myLoc)
			{
				window.open(myLoc,"Wheel","width=800,height=600,location=no,toolbar=no,menubar=no,scrollbars=yes,resizable=no");
				return false;
			}
        </script>
	</head>
	<body class="body">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" width="100%" cellspacing="0" cellpadding="0">
			    <tr>
				    <td class="linespacing1"></td>
			    </tr>
			    <tr>
				    <td class="header" colspan="3"><asp:label id="lblTitle" runat="server" Text="Contract Group Items" CssClass="header"></asp:label></td>
			    </tr>
			    <tr>
				    <td class="linespacing1"></td>
		        </tr>			    
				<tr>
					<td class="tableheader" colspan="4">
                        <asp:Label ID="Label1" runat="server" Text="Contract Group Header"></asp:Label></td>
				</tr>
				<tr>
				    <td class="tablecol" style="width:15%">
                        <strong><asp:Label ID="Label2" runat="server" Text="Contract Ref. No. :"></asp:Label></strong></td>
                    <td class="tablecol">
                        <asp:Label ID="lblContractRef" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="tablecol" style="width:15%">
                        <strong><asp:Label ID="Label3" runat="server" Text="Description :"></asp:Label></strong></td>
                    <td class="tablecol">
                        <asp:Label ID="lblDescription" runat="server" Text=""></asp:Label>
                    </td>
				</tr>
				<tr>
				    <td class="tablecol">
                        <strong><asp:Label ID="Label4" runat="server" Text="Start Date :"></asp:Label></strong></td>
                    <td class="tablecol">
                        <asp:Label ID="lblStartDate" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="tablecol">
                        <strong><asp:Label ID="Label6" runat="server" Text="End Date :"></asp:Label></strong></td>
                    <td class="tablecol">
                        <asp:Label ID="lblEndDate" runat="server" Text=""></asp:Label>
                    </td>
				</tr>
				<tr>
				    <td class="tablecol">
                        <strong><asp:Label ID="Label5" runat="server" Text="Buyer Company :"></asp:Label></strong></td>
                    <td class="tablecol"  colspan="3">
                        <asp:Label ID="lblBuyerCompany" runat="server" Text=""></asp:Label>
                    </td>
				</tr>				
				<tr>
					<td class="EmptyCol" colspan="4">
					    <asp:datagrid id="dtgItem" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
							Width="100%" DataKeyField="cdm_group_index">
                            <Columns>
                                <asp:TemplateColumn HeaderText="Item Code" SortExpression="cdi_vendor_item_code">
                                    <ItemTemplate>
									    <asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
								    </ItemTemplate>
                                </asp:TemplateColumn>                                                                
                                <asp:BoundColumn HeaderText="Item Description" DataField="cdi_product_desc" SortExpression="cdi_product_desc"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Currency" DataField="cdi_currency_code" SortExpression="cdi_currency_code"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Price" DataField="cdi_unit_cost" SortExpression="cdi_unit_cost"><ItemStyle HorizontalAlign="Right" />
                                    <HeaderStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="SST Rate" DataField="cdi_gst_rate" SortExpression="cdi_gst_rate"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="UOM" DataField="cdi_uom" SortExpression="cdi_uom"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Remarks" DataField="cdi_remark" SortExpression="cdi_remark"></asp:BoundColumn>                                                                
                            </Columns>
						</asp:datagrid>
					</td>
				</tr>							
				<tr>
				    <td class="EmptyCol" colspan="4"><asp:HyperLink ID="lnkBack" runat="server"><STRONG>&lt; Back</STRONG></asp:HyperLink></td>
				</tr>
			</table>
			
		</form>
	</body>
</html>