<%--Copyright © 2011 STRATEQ GLOBAL SERVICES. All rights reserved.--%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="WriteOffDetail.aspx.vb" Inherits="eProcure.WriteOffDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Write Off Detail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script src="http://code.jquery.com/jquery-1.4.min.js" type="text/javascript"></script>
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
        </script>
		    <% Response.Write(Session("WheelScript"))%>
		    <% Response.Write(Session("JQuery")) %>
            <% Response.Write(Session("AutoComplete")) %>
		    <script type="text/javascript">
		    $(document).ready(function(){
            $('#cmd_Submit').click(function() {
            document.getElementById("cmd_Submit").style.display= "none";
            });
            });
				
			function PopWindow(myLoc)
			{
				window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
				return false;
			}
		</script>
	</HEAD>	
	
	<body class="body" runat="server">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_WriteOffDetail_tabs"))%>
            <TABLE class="alltable" id="TABLE2" cellSpacing="0" cellPadding="0" border="0" style="width: 100%">
                <tr>
			    <TD class="linespacing1" colSpan="5"></TD>
	            </TR>					  			
			    
                
                <tr>
			    <TD class="linespacing2" colSpan="6"></TD>
	            </TR>
                
			    <TR>
				    <TD class="tableheader" colspan="6">&nbsp;<asp:label id="Label7" runat="server">Write Off Detail</asp:label></TD>
			    </TR>
    								
                <tr >
				    <td class="tablecol" width="6%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label9" runat="server" >WO No</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" width="10%" style="height: 19px"><asp:label id="lblWONo" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" width="6%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label1" runat="server" >Status</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" width="10%" style="height: 19px"><asp:label id="lblStatus" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" width="10%"></td>
				    <td class="tablecol" width="10%"></td>
			    </tr>
			    <tr >
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label11" runat="server" >WO Date</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblWODate" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
			    </tr>
			    <tr >
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label2" runat="server" >Remark</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px" colspan="4"><asp:label id="lblRemark" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol"></td>
			    </tr>
			    <tr >
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label3" runat="server" >File(s) Attached</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px" colspan="4"><asp:panel id="pnlAttach2" runat="server"></asp:panel></td>
				    <td class="tablecol"></td>
			    </tr>			    
		    </TABLE>
			
			<table class="AllTable" id="Table1" width="100%" cellSpacing="0" cellPadding="0" border="0" runat="server">
				<TR>
					<TD class="emptycol" colSpan="5" style="width: 703px"></TD>
				</TR>
				<TR>
					<td colspan="5" style="width: 100%;">
					    <asp:datagrid id="dtgWODtl" runat="server" DataKeyField="IM_ITEM_CODE" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="IM_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IWOD_INVENTORY_NAME" HeaderText="Item Name">
									<HeaderStyle Width="32%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IWOD_UOM" HeaderText="UOM">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>									
								<asp:BoundColumn DataField="QTY" HeaderText="Write Off Qty">
									<HeaderStyle Width="8%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IWOD_WO_LOT_NO" HeaderText="Lot No">
									<HeaderStyle Width="14%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="LM_LOCATION" HeaderText="Loc">
									<HeaderStyle Width="14%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="LM_SUB_LOCATION" HeaderText="Sub-Loc">
									<HeaderStyle Width="14%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
                    </TD>
				</TR>
			</table> 
			
			<table class="alltable" id="table4" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td style="height: 24px">
					    <asp:button id="cmdCancel" runat="server" CssClass="Button" Text="Cancel"></asp:button>&nbsp;
					    <asp:button id="cmdView" runat="server" CssClass="Button" Text="View"></asp:button>&nbsp;
					</td>
				</tr>
				<tr>
					<td style="height: 27px">&nbsp;</td>
				</tr>				
			</table>
		    <div class="emptycol"><asp:hyperlink id="lnkBack" Runat="server"><STRONG>&lt; Back</STRONG></asp:hyperlink></div>
		</form>
	</body> 
</HTML>
