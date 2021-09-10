<%--Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.--%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReturnOutwardDetail.aspx.vb" Inherits="eProcure.ReturnOutwardDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Return Outward Detail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim bubblepopupcss As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "jquery.bubblepopup.v2.3.1.css") & """ rel='stylesheet' type='text/css'>"
            Dim bubblepopupjquery As String = "<script type=""text/javascript"" src="""& dDispatcher.direct("Plugins/include","jquery.bubblepopup.v2.3.1.min.js") &""">"            
        </script>
		    <% Response.Write(Session("WheelScript"))%>
		    <% Response.Write(bubblepopupcss)%>
		    <% Response.Write(Session("JQuery")) %>
		    <% Response.Write(bubblepopupjquery) %>
            <% Response.Write(Session("AutoComplete")) %>
		    <script type="text/javascript">
		    $(document).ready(function(){
                <%  Response.Write(Session("jqPopup")) %>
            
            });
       
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
	</head>	
	
	<body class="body" runat="server">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ReturnInward_tabs"))%>
            <table class="alltable" id="TABLE2" cellspacing="0" cellpadding="0" border="0" style="width: 100%">
                <tr>
			        <td class="linespacing1" colspan="5"></td>
	            </tr>
	            <tr>
                    <td class="EmptyCol" colspan="6">
	                    <asp:label id="Label8" runat="server"  CssClass="lblInfo"
	                            Text="Fill in the required field(s) and click the Submit button to submit the Return Outward."
	                    ></asp:label>

                    </td>
                </tr>
                <tr>
			        <td class="linespacing2" colspan="6"></td>
	            </tr>    
			    <tr>
				    <td class="tableheader" colspan="6">&nbsp;<asp:label id="Label7" runat="server">Return Outward Generation</asp:label></td>
			    </tr>
			    
			    <tr>
				    <td class="tablecol" width="14%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label10" runat="server" >RO No</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" width="16%" style="height: 19px"><asp:label id="lblRONo" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" width="10%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label13" runat="server" >RO Date</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" width="10%" style="height: 19px"><asp:label id="lblRODate" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" width="9%"></td>
				    <td class="tablecol" width="8%"></td>
			    </tr>
			        
			    <tr>
				    <td class="tablecol" width="12%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label9" runat="server" >GRN No</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" width="15%" style="height: 19px"><asp:label id="lblGRNNo" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" width="10%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label3" runat="server" >GRN Date</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" width="10%" style="height: 19px"><asp:label id="lblGRNDate" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" width="10%"></td>
				    <td class="tablecol" width="10%"></td>
			    </tr>
			    <tr >
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label11" runat="server" >Vendor</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblVendor" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
			    </tr>
			    <tr >
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label1" runat="server" >PO Number</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblPONumber" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label2" runat="server" >PO Creation Date</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblPOCreationDate" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
			    </tr>
			    <tr >
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label4" runat="server" >DO Number</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblDONo" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label6" runat="server" >DO Date</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblDODate" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
			    </tr>
			    <tr >
				    <td class="tablecol" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label5" runat="server" >Actual Goods Received Date</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" style="height: 19px"><asp:label id="lblActualGoodRecDate" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
				    <td class="tablecol"></td>
			    </tr>
		    </table>
			
			<table class="AllTable" id="Table1" width="100%" cellspacing="0" cellpadding="0" border="0" runat="server">
				<tr>
					<td class="emptycol" colspan="5" style="width: 703px"></td>
				</tr>
				<tr>
					<td colspan="5" style="width: 100%;">
					    <asp:datagrid id="dtgRODtl" runat="server" DataKeyField="POD_VENDOR_ITEM_CODE" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>
							    <asp:BoundColumn HeaderText="Line">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="RIGHT" Width="5%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="8%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="30%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" HeaderText="UOM">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								
								<asp:BoundColumn DataField="POD_MIN_PACK_QTY" HeaderText="MPQ">
									<HeaderStyle Width="8%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" HeaderText="Ordered Qty">
									<HeaderStyle Width="8%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GD_RECEIVED_QTY" HeaderText="Received Qty">
									<HeaderStyle Width="8%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GD_REJECTED_QTY" HeaderText="Rejected Qty">
									<HeaderStyle Width="8%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IROD_QTY" HeaderText="Returned Qty">
									<HeaderStyle Width="14%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IROD_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="18%" HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
                    </td>
				</tr>
			</table> 
	            
		    <table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0">		 				
		    <tr>
			    <td class="emptycol" style="height: 24px">&nbsp;&nbsp;</td>
		    </tr>
		    <tr>
		        <td class="emptycol">
		            <asp:Button ID="cmdView" runat="server" Text="View" Width="56px" CssClass="button" />
		        </td>
		    </tr>
		    <%--<tr>
			    <td class="emptycol">
			    <asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;
			    <asp:button id="cmdReset" runat="server" CssClass="BUTTON" Text="Clear" CausesValidation="False"></asp:button>
		    </td>
		    </tr>--%>
		    <tr>
				<td class="emptycol" colspan="4"></td>
			</tr>
			<tr>
			    <td><asp:validationsummary id="Validationsummary1" runat="server" CssClass="errormsg"></asp:validationsummary></td>
			</tr>
			<tr>
				<td class="emptycol" colspan="4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; <asp:label id="lbl_check" runat="server" CssClass="errormsg" Width="400px" ForeColor="Red"></asp:label></td>
			</tr>
		    </table>
		    <div class="emptycol"><asp:hyperlink id="lnkBack" Runat="server"><strong>&lt; Back</strong></asp:hyperlink></div>
		</form>
	</body> 
</html>
