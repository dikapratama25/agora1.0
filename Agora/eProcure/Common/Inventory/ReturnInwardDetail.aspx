<%--Copyright © 2011 STRATEQ GLOBAL SERVICES. All rights reserved.--%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReturnInwardDetail.aspx.vb" Inherits="eProcure.ReturnInwardDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Return Inward Detail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
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
		        window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
		        return false;
	        }
				
		    </script>
	</HEAD>	
	
	<body class="body" runat="server">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ReturnInward_tabs"))%>
            <TABLE class="alltable" id="TABLE2" cellSpacing="0" cellPadding="0" border="0" style="width: 100%">
                <tr>
			    <TD class="linespacing1" colSpan="5"></TD>
	            </TR>					  			
			    
                
                <tr>
			    <TD class="linespacing2" colSpan="6"></TD>
	            </TR>
                
			    <TR>
				    <TD class="tableheader" colspan="6">&nbsp;<asp:label id="Label7" runat="server">Return Inward Detail</asp:label></TD>
			    </TR>
    								
                <tr >
				    <td class="tablecol" width="6%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label9" runat="server" >RI No</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" width="10%" style="height: 19px"><asp:label id="lblRINo" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" width="6%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label1" runat="server" >Status</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" width="10%" style="height: 19px"><asp:label id="lblStatus" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" width="10%"></td>
				    <td class="tablecol" width="10%"></td>
			    </tr>
			    <tr >
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label11" runat="server" >RI Date</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblRIDate" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label2" runat="server" >MRS Number</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblMRSNo" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
			    </tr>			    
		    </TABLE>
			
			<table class="AllTable" id="Table1" width="100%" cellSpacing="0" cellPadding="0" border="0" runat="server">
				<TR>
					<TD class="emptycol" colSpan="5" style="width: 703px"></TD>
				</TR>
				<TR>
					<td colspan="5" style="width: 100%;">
					    <asp:datagrid id="dtgRIDtl" runat="server" DataKeyField="IM_ITEM_CODE" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="IM_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRID_INVENTORY_NAME" HeaderText="Item Name">
									<HeaderStyle Width="22%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRID_UOM" HeaderText="UOM">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DOL_LOT_NO" HeaderText="Lot No">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="LM_LOCATION" HeaderText="Loc">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="LM_SUB_LOCATION" HeaderText="Sub-Loc">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRIL_LOT_QTY" HeaderText="Qty">
									<HeaderStyle Width="8%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRIL_REMARK" HeaderText="Remark">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
                    </TD>
				</TR>
			</table> 
	            
		    <table class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">		 				
		        <TR>
			        <TD class="emptycol">&nbsp;&nbsp;</TD>
		        </TR>
		        <TR>
					<TD vAlign="middle">&nbsp;<STRONG>
							<asp:label id="lblRemarkCR" runat="server">Reject Remarks :</asp:label></STRONG></TD>
					<TD><asp:textbox id="txtRemarkCR" Runat="server" Width="450px" TextMode="MultiLine" MaxLength="1000"
							Rows="3" CssClass="listtxtbox" ReadOnly="true"></asp:textbox></TD>
				</TR>
				<TR>
			        <TD class="emptycol">&nbsp;&nbsp;</TD>
		        </TR>
		        <TR>
			        <TD class="emptycol" style="height: 24px">
			        <asp:button id="cmdView" runat="server" CssClass="button" Text="View"></asp:button>&nbsp;
			    </TD>
		        </tr>
		        <TR>
				    <TD class="emptycol" colSpan="4"></TD>
			    </TR>
			</table>
		    <div class="emptycol"><asp:hyperlink id="lnkBack" Runat="server"><STRONG>&lt; Back</STRONG></asp:hyperlink></div>
		</form>
	</body> 
</HTML>
