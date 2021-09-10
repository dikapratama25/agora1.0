<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchBItem.aspx.vb" Inherits="eProcure.SearchBItem" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Item Listing</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
             Dim commodity As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Commodity")
        </script> 
	<%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
				 $(document).ready(function(){
        
            $("#txtCommodity").autocomplete("<% Response.write(commodity) %>", {
            width: 262,
            scroll: true,
            selectFirst: false
            });
            $("#txtCommodity").result(function(event, data, formatted) {
            if (data)
            $("#hidCommodity").val(data[1]);
            });
            $("#txtCommodity").blur(function() {
            var hidcommodity = document.getElementById("hidCommodity").value;                        
            if(hidcommodity == "")
            {
                $("#txtCommodity").val("");
            }
            });                       
            });
            
		function selectAll()
			{
				SelectAllG("dtgCatalogue_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCatalogue_ctl02_chkAll","chkSelection");
			}
		
			function Reset(){
				var oform = document.forms(0);
				oform.txtVendorItemCode.value="";
				oform.txtDesc.value="";
				Form1.txtCommodity.value="";
				Form1.hidCommodity.value="";
				if( document.getElementById('chkActive') ){
				Form1.chkActive.checked=true;}
				if( document.getElementById('chkInActive') ){
				Form1.chkInActive.checked=false;}
				oform.chkSpot.checked=false;
				oform.chkStock.checked=false;
				oform.chkMRO.checked=false;
			}

			function Close()
			{
	            window.close();
	         }
		-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_BIM_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
			<TR>
					<TD class="header" style="height: 3px; width: 838px;"></TD>
				</TR>
				<TR>
					<TD class="header" ><FONT size="1"><asp:label id="lblTitle" runat="server" CssClass="header">Item Listing</asp:label></FONT></TD>
				</TR>
            </table>
			<DIV id="hidAction" style="DISPLAY: inline" runat="server">
			<table>
			<tr>
					<TD class="header" colSpan="4" style="height: 7px"></TD>
            </tr>
				<TR>
					<TD class="emptycol" align="center" colSpan="6" >
						<div align="left"><asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text="Fill in the search criteria and click Search button to list the relevant items. Click the Add button to add new item."></asp:label>
                        </div>
					</TD>
				</TR>
            <tr>
					<TD class="header" colSpan="6" style="height: 7px"></TD>
			</TR>
            </table>
			</DIV>
			<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="tableheader" colspan="6">&nbsp;Search Criteria</TD>
				</TR>
				<TR class="tablecol" colspan="5" width="100%">
					<TD class="tablecol" width="13%" >
                       <strong>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Item Code :" CssClass="lbname"></asp:Label></strong></TD>
                    <TD class="Tablecol" width="30%"><asp:textbox id="txtVendorItemCode" width="100%" runat="server" CssClass="txtbox" Height="20px" ></asp:textbox></TD>
			        <td class="tablecol" width="1%"></td>
					<TD class="tablecol" width="18%">
                        <strong>&nbsp;<asp:Label ID="Label3" runat="server" Text="Commodity Type :"></asp:Label></strong></TD>
                    <TD class="Tablecol" width="38%" colspan="1"><asp:textbox id="txtCommodity" width="80%" runat="server" CssClass="txtbox"></asp:textbox><input type="hidden" id="hidCommodity" runat="server" /></TD>
				</TR>
				<TR class="tablecol">
 					<TD class="tablecol"  nowrap>
                        <strong>&nbsp;<asp:Label ID="Label2" runat="server" Text=" Item Name : "></asp:Label></strong></TD>
					<TD class="tablecol" nowrap><asp:textbox id="txtDesc" width="100%" runat="server" CssClass="txtbox" ></asp:textbox></TD>
					<td class="tablecol" colspan="2" nowrap style="height: 20px;">
					    &nbsp;&nbsp;&nbsp;<asp:checkbox id="chkActive" Text="Active" Runat="server" Checked="True"></asp:checkbox>
					    &nbsp;&nbsp;&nbsp;<asp:checkbox id="chkInActive" Text="Inactive" Runat="server" Checked="False"></asp:checkbox>
					</td>
					<TD class="tablecol" align="right"></TD>
				</TR>
                <tr class="tablecol">
                    <td class="tablecol" nowrap="nowrap">
                        <strong>&nbsp;<asp:Label ID="Label4" runat="server" Text=" Item Type : "></asp:Label></strong></td>
                         <TD class="tablecol">
                             &nbsp;&nbsp;<asp:CheckBox ID="chkSpot" runat="server" Text="Spot" />
                             &nbsp;<asp:CheckBox ID="chkStock" runat="server" Text="Stock" />
                             <asp:CheckBox ID="chkMRO" runat="server" Text="MRO" /></TD>
                    <td class="tablecol" nowrap="nowrap">
                    </td>           
                    <td class="tablecol"></td>        
                    <td class="tablecol" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
						<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">
                    </td>
                </tr>
				<TR class="tablecol">
				</TR>
				<TR>
					<TD class="emptycol" colspan="6">&nbsp;</TD>
				</TR>
				<tr>
					<td class="emptycol" colspan="7"><asp:datagrid id="dtgCatalogue" runat="server" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="PM_PRODUCT_INDEX" SortExpression="PM_PRODUCT_INDEX" HeaderText="Item Index"></asp:BoundColumn>
								<asp:TemplateColumn SortExpression="PM_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle Width="9%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkItem"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle Width="17%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CT_NAME" SortExpression="CT_NAME" HeaderText="Commodity Type">
									<HeaderStyle Width="11%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PM_UOM" SortExpression="PM_UOM" HeaderText="UOM">
									<HeaderStyle Width="7%"  Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="PM_PRODUCT_CODE" SortExpression="PM_PRODUCT_INDEX" HeaderText="PRODUCT_CODE"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Status" SortExpression="PM_DELETED" >
										<HeaderStyle Width="4%"></HeaderStyle>
										<ItemTemplate>
											<asp:Label Runat="server" ID="lblStatus"></asp:Label>
										</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="6">
						<asp:button id="cmdAdd" runat="server" CssClass="Button" Text="Add"></asp:button>
						<asp:button id="cmdModify" runat="server" CssClass="Button" Text="Modify"></asp:button>
						<asp:button id="cmdActivate" runat="server" CssClass="Button" Text="Activate"></asp:button>
						<asp:button id="cmdDeActivate" runat="server" CssClass="Button" Text="Deactivate"></asp:button>
						<asp:button id="cmdSave" runat="server" CssClass="Button" Text="Save"></asp:button>
                        <INPUT class="button" id="cmdClose" onclick="Close();" type="button" value="Close" >
                        <INPUT class="txtbox" id="hidIndex" style="WIDTH: 43px; HEIGHT: 18px" type="hidden" size="1"
							name="hidIndex" runat="server">
                        </td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
