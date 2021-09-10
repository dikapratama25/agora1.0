<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InventoryVerificationListing.aspx.vb" Inherits="eProcure.InventoryVerificationListing" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Inventory Verification Listing</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim StartCalendar As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_startdate") & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim EndCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_enddate") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"

            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
         <% Response.Write(css)%>   
		<script language="javascript">
		    function Reset(){
			    var oform = document.forms(0);			    
			    oform.txtNo.value = ""
			    oform.txtVendor.value="";	
			    oform.txt_startdate.value=""
			    oform.txt_enddate.value=""		    
		    }
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_InvList_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
			<tr><td class="rowspacing"></td></tr>
			<TR>
				<TD class="EmptyCol" colSpan="4" style="height: 19px">
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Fill in the search criteria and/or click Search button to list the relevant verified inventory."></asp:label>
				</TD>
			</TR>
			<tr><td class="rowspacing"></td></tr>
			<TR>
				<TD class="TableHeader" colSpan="4">Search Criteria</TD>
			</TR>
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label1" runat="server" Text="GRN Number :" CssClass="lbl"></asp:Label></strong></td>
				<td class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txtNo" runat="server" CssClass="txtbox" Width="150px"></asp:textbox></td>
				<td class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label4" runat="server" Text="Vendor :" CssClass="lbl"></asp:Label></strong></td>
                <td class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txtVendor" runat="server" CssClass="txtbox" Width="150px"></asp:textbox></td>
            </TR>      
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px" width="20%"><STRONG><asp:Label ID="lblStartDt" runat="server" Text="Start Date :" CssClass="lbl"></asp:Label></STRONG></td>
				<td class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txt_startdate" runat="server" CssClass="txtbox" contentEditable="false" Width="104px"></asp:textbox><% Response.Write(StartCalendar)%></td>
				<td class="TableCol" style="height: 18px" width="20%"><STRONG><asp:Label ID="lblEndDt" runat="server" Text="End Date :" CssClass="lbl"></asp:Label></STRONG></td>
				<td class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txt_enddate" runat="server" CssClass="txtbox" contentEditable="false" Width="104px"></asp:textbox><% Response.Write(EndCalendar)%></TD>
			</TR>
			<TR class="tablecol">
			    <TD class="TableCol" style="height: 18px"></TD>
			    <TD class="TableCol" style="height: 18px"></TD>
			    <TD class="TableCol" style="height: 18px"></TD>				
				<TD class="TableCol" style="height: 18px; text-align:right;">			
				    <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" ></asp:button>&nbsp;
				    <asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear" ></asp:button>
				</TD>
			</TR>	
			<tr><td class="rowspacing"></td></tr>
				<TR>
					<TD class="EmptyCol" colspan="4">
						    <asp:datagrid id="dtgInv" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
						        <Columns>
						            <asp:BoundColumn DataField="IV_GRN_NO" SortExpression="IV_GRN_NO" HeaderText="GRN Number">
								        <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							        </asp:BoundColumn>
							        <asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor">
								        <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							        </asp:BoundColumn>						             					       
							        <asp:BoundColumn DataField="IM_ITEM_CODE" SortExpression="IM_ITEM_CODE" HeaderText="Item Code">
								        <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							        </asp:BoundColumn>
							        <asp:BoundColumn DataField="IM_INVENTORY_NAME" SortExpression="IM_INVENTORY_NAME" HeaderText="Item Name">
									    <HeaderStyle HorizontalAlign="Left" Width="15%" ></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="LM_LOCATION" SortExpression="LM_LOCATION" HeaderText="Location">
									    <HeaderStyle HorizontalAlign="Left" Width="10%" ></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
								    </asp:BoundColumn>
							        <asp:BoundColumn DataField="LM_SUB_LOCATION" SortExpression="LM_SUB_LOCATION" HeaderText="Sub Location">
								        <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							        </asp:BoundColumn>
							        <asp:BoundColumn DataField="IV_RECEIVE_QTY" SortExpression="IV_RECEIVE_QTY" HeaderText="Received </br> Qty">
								        <HeaderStyle HorizontalAlign="Right" Width="6%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
							        </asp:BoundColumn>	
							        <asp:BoundColumn HeaderText="Passed </br> Qty">
								        <HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
							        </asp:BoundColumn>	
							        <asp:BoundColumn HeaderText="Failed </br> Qty">
								        <HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
							        </asp:BoundColumn>
							        <asp:BoundColumn Visible="False" DataField="IV_INVENTORY_INDEX"></asp:BoundColumn>	
							        <asp:BoundColumn Visible="False" DataField="IV_LOCATION_INDEX"></asp:BoundColumn>	
							        <asp:BoundColumn Visible="False" DataField="IV_VERIFY_INDEX"></asp:BoundColumn>	
							        
						        </Columns>
					        </asp:datagrid>																	
					</TD>
				</TR>				
			</TABLE>
		</form>
	</body>
</HTML>
