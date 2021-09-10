<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InventoryAdjustList.aspx.vb" Inherits="eProcure.InventoryAdjustList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Adjustment Listing</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim StartCalendar As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_startdate") & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim EndCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_enddate") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            
            Dim sOpen As String = dDispatcher.direct("Initial", "popCalendar.aspx", "textbox="" + val + ""&seldate="" + txtVal.value")

            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
         <% Response.Write(css)%>   
         <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		    function Reset(){
			     var oform = document.forms(0);			    
			    oform.txtItemCode.value = ""
			    oform.txtItemName.value=""	
			    oform.txt_startdate.value=""
			    oform.txt_enddate.value=""
			    oform.ddl_Loc.selectedIndex=0
			    oform.ddl_SubLoc.selectedIndex=0;     
		    }	    
		  
		-->    
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_Adj_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
			<tr><td class="rowspacing"></td></tr>
			<TR>
				<TD class="EmptyCol" colSpan="4">
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Fill in the search criteria and click Search button to list the relevant inventory adjustment."></asp:label>
				</TD>
			</TR>
			<tr><td class="rowspacing"></td></tr>
			<TR>
				<TD class="TableHeader" colSpan="4">Search Criteria</TD>
			</TR>
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label2" runat="server" Text="Item Code :" CssClass="lbl"></asp:Label></strong></TD>
				<TD class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txtItemCode" runat="server" CssClass="txtbox" Width="104px"></asp:textbox></TD>
				<TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="Label3" runat="server" Text="Item Name :" CssClass="lbl"></asp:Label></strong></TD>
                <TD class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txtItemName" runat="server" CssClass="txtbox" Width="160px"></asp:textbox></TD>				
			</TR>
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px" width="20%"><STRONG><asp:Label ID="lblStartDt" runat="server" Text="Start Date :" CssClass="lbl"></asp:Label></STRONG></td>
				<td class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txt_startdate" runat="server" CssClass="txtbox" contentEditable="false" Width="104px"></asp:textbox><% Response.Write(StartCalendar)%></td>
				<td class="TableCol" style="height: 18px" width="20%"><STRONG><asp:Label ID="lblEndDt" runat="server" Text="End Date :" CssClass="lbl"></asp:Label></STRONG></td>
				<td class="TableCol" style="height: 18px" width="30%"><asp:textbox id="txt_enddate" runat="server" CssClass="txtbox" contentEditable="false" Width="104px"></asp:textbox><% Response.Write(EndCalendar)%></TD>
			</TR>
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="lblLoc" runat="server" Text="Location" CssClass="lbl"></asp:Label><asp:Label ID="Label1" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></TD>
				<TD class="TableCol" style="height: 18px" width="30%"><asp:dropdownlist id="ddl_Loc" runat="server" Width="200px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist></TD>
				<TD class="TableCol" style="height: 18px" width="20%"><strong><asp:Label ID="lblSubLoc" runat="server" Text="Sub Location" CssClass="lbl"></asp:Label><asp:Label ID="Label4" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></TD>
                <TD class="TableCol" style="height: 18px" width="30%"><asp:dropdownlist id="ddl_SubLoc" runat="server" Width="200px" style="margin-bottom:1px;" CssClass="ddl" AutoPostBack="False"></asp:dropdownlist></TD>				
			</TR>	
			<TR class="tablecol">
			    <TD class="TableCol" style="height: 18px"></TD>
			    <TD class="TableCol" style="height: 18px"></TD>
			    <TD class="TableCol" style="height: 18px"></TD>				
				<TD class="TableCol" style="height: 18px; text-align:right;">
				    <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" ></asp:button>&nbsp;
                    <INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear" runat="server">
				</TD>
			</TR>	
			<tr><td class="rowspacing"></td></tr>
				<TR>
					<TD class="EmptyCol" colspan="4">
						    <asp:datagrid id="dtgInv" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
						        <Columns>
						            <asp:BoundColumn DataField="IM_ITEM_CODE" SortExpression="IM_ITEM_CODE" HeaderText="Item Code">
								        <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							        </asp:BoundColumn>
							        <asp:BoundColumn DataField="IM_INVENTORY_NAME" SortExpression="IM_INVENTORY_NAME" HeaderText="Item Name">
									    <HeaderStyle HorizontalAlign="Left" Width="16%" ></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
								    </asp:BoundColumn>
								    <asp:BoundColumn DataField="LM_LOCATION" SortExpression="LM_LOCATION" HeaderText="Location">
									    <HeaderStyle HorizontalAlign="Left" Width="12%" ></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" />
								    </asp:BoundColumn>
							        <asp:BoundColumn DataField="LM_SUB_LOCATION" SortExpression="LM_SUB_LOCATION" HeaderText="Sub Location">
								        <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							        </asp:BoundColumn>
							        <asp:BoundColumn DataField="IT_ADDITION_INFO" SortExpression="IT_ADDITION_INFO" HeaderText="Original </br> Qty">
								        <HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
							        </asp:BoundColumn>	
							        <asp:BoundColumn DataField="IT_TRANS_QTY" SortExpression="IT_TRANS_QTY" HeaderText="Physical </br> Qty">
								        <HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
							        </asp:BoundColumn>	
							        <asp:BoundColumn DataField="VarianceQty" SortExpression="VarianceQty" HeaderText="Variance </br> Qty">
								        <HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
							        </asp:BoundColumn>	
							        <asp:BoundColumn DataField="IT_TRANS_DATE" SortExpression="IT_TRANS_DATE" HeaderText="Date </br> Adjusted">
								        <HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
							        </asp:BoundColumn>	
							        <asp:BoundColumn DataField="IT_REMARK" HeaderText="Remarks">
								        <HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
								        <ItemStyle HorizontalAlign="Right"></ItemStyle>
							        </asp:BoundColumn>	
							        <asp:BoundColumn Visible="False" DataField="IM_INVENTORY_INDEX"></asp:BoundColumn>	
							        <asp:BoundColumn Visible="False" DataField="IT_TO_LOCATION_INDEX"></asp:BoundColumn>	
							        
						        </Columns>
					        </asp:datagrid>																	
					</TD>
				</TR>				
			</TABLE>
		</form>
	</body>
</HTML>
