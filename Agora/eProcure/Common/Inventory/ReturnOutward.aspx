<%--Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.--%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReturnOutward.aspx.vb" Inherits="eProcure.ReturnOutward" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Return Outward</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
        </script>
		    <% Response.Write(Session("WheelScript"))%>
		    <% Response.Write(Session("JQuery")) %>
            <% Response.Write(Session("AutoComplete")) %>
		    <script type="text/javascript">
		    $(document).ready(function(){
            $('#cmdSubmit').click(function() {
            document.getElementById("cmdSubmit").style.display= "none";
            });
            });
            
            function ShowDialog(filename,height)
		    {
				
			    var retval="";
			    retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 850px");
			    //retval=window.open(filename);
			    if (retval == "1" || retval =="" || retval==null)
			    {  window.close;
				    return false;

			    } else {
			        window.close;
				    return true;

			    }
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
				    <td class="tablecol" width="14%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label9" runat="server" >GRN No</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" width="16%" style="height: 19px"><asp:label id="lblGRNNo" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" width="10%" align="left" style="height: 19px">&nbsp;<strong><asp:label id="Label3" runat="server" >GRN Date</asp:label></strong>&nbsp;:</td>
				    <td class="tablecol" width="10%" style="height: 19px"><asp:label id="lblGRNDate" runat="server" Width="100%"></asp:label></td>
				    <td class="tablecol" width="9%"></td>
				    <td class="tablecol" width="8%"></td>
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
					    <asp:datagrid id="dtgGRNDtl" runat="server" DataKeyField="POD_VENDOR_ITEM_CODE" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
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
									<HeaderStyle Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="30%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" HeaderText="UOM">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_MIN_PACK_QTY" HeaderText="MPQ">
									<HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" HeaderText="Ordered Qty">
									<HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GD_RECEIVED_QTY" HeaderText="Received Qty">
									<HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GD_REJECTED_QTY" HeaderText="Rejected Qty">
									<HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Returned Qty">
									<HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Remaining Qty">
									<HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="IQC Approved Qty">
								    <HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="IQC Rejected Qty">
								    <HeaderStyle Width="5%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" ></ItemStyle>
								</asp:TemplateColumn> 
								<asp:TemplateColumn HeaderText="Return Qty">
									<HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:label id="lblQty" runat="server" style="text-align:left;"></asp:label>
										<asp:Button ID="btn_lot" runat="server" CssClass="Button" Text="Set" width="35px" style="padding-right:auto;"/>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:textbox ID="txtDtlRemarks" Runat="server" cssclass="txtbox" TextMode="MultiLine" Rows="2" Height="30px" MaxLength="400"></asp:textbox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red" ReadOnly="true" Visible="false"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="GD_PO_LINE" Visible="false">
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="QC" Visible="false">
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Prev Remaining Qty" Visible="false">
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
                    </td>
				</tr>
			</table> 
	            
		    <table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0">		 				
		    <tr>
			    <td class="emptycol">&nbsp;&nbsp;</td>
		    </tr>
		    <tr>
			    <td class="emptycol">
			    <asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;
			    <asp:button id="cmdReset" runat="server" CssClass="BUTTON" Text="Clear" CausesValidation="False"></asp:button>
			    <asp:button id="btnhidden2" runat="server" CssClass="Button"  Text="btnhidden2" style=" display :none"></asp:button>
		    </td>
		    </tr>
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
