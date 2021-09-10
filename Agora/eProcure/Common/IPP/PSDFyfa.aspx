<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PSDFyfa.aspx.vb" Inherits="eProcure.PSDFyfa" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IPP Listing</title>
    <% Response.Write(Session("JQuery")) %> 
    <% Response.Write(Session("AutoComplete")) %>
    <% Response.Write(Session("WheelScript")) %>
    <script runat="server">
        Dim dDispatcher As New AgoraLegacy.dispatcher
        dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
        Dim sCal As String = "<IMG src=" & dDispatcher.direct("Plugins/Images", "i_Calendar2.gif") & " border=""0"">"
    </script>
    <% Response.write(Session("typeahead2")) %>
    <script language="javascript">
    function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=800,height=600,location=no,toolbar=no,menubar=no,scrollbars=yes,resizable=no");
			return false;
		}
	function popCalendar(val)
		{
			txtVal= document.getElementById(val);
			//window.open('../popCalendar.aspx?textbox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
			window.open('<% response.write(PopCalendar) %>' ,'cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
		}
	function selectAll()
		{
			SelectAllG("dtgIPPList_ctl02_chkAll","chkSelection");
		}
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <% Response.write(Session("w_IPP_tabs")) %>
    <div>
        <table id="table1" cellpadding="0" cellspacing="0" class="AllTable">        
            <tr>
                <td class="linespacing1" colspan="6">
                </td>
            </tr>
            <tr>
                <td class="EmptyCol">
                    <asp:Label ID="Label1" runat="server" CssClass="lblInfo" Text="Fill in the search criteria and click on Search button to list the  relevant E2P document."></asp:Label></td>
            </tr>
            <tr>
                <td class="EmptyCol">
                    <asp:Label ID="Label9" runat="server" CssClass="lblInfo" Text="Select the PSD Sent Date and the E2P documents. Click Save to save the record."></asp:Label>                    
                </td>
            </tr>
         </table>
         <table id="Table2" class="AllTable" cellpadding="0" cellspacing="0">
            <tr>
                <td class="TableHeader" colspan="5">
                    <asp:Label ID="Label2" runat="server" Text=" Search Criteria"></asp:Label></td>
            </tr>
            <tr>
                <td class="TableCol">
                    <asp:Label ID="Label3" CssClass="lbl" runat="server" Text=" Document No. :"></asp:Label></td>
                <td class="TableCol">
                    <asp:TextBox ID="txtDocNo" CssClass="txtbox" runat="server" style="width:120px;"></asp:TextBox></td>                
                <td class="TableCol">
                    <asp:Label ID="Label4" CssClass="lbl" runat="server" Text="Document Type : "></asp:Label></td>
                <td class="TableCol">
                    <asp:DropDownList ID="ddlDocType" runat="server" CssClass="ddl" Width="120px">
                        <asp:ListItem Value="">---Select---</asp:ListItem>
                        <asp:ListItem Value="INV">Invoice</asp:ListItem>
                        <asp:ListItem Value="BILL">Bill</asp:ListItem>
                        <asp:ListItem Value="CN">Credit Note</asp:ListItem>
                        <asp:ListItem Value="DN">Debit Note</asp:ListItem>                        
                        <asp:ListItem Value="LETTER">Letter</asp:ListItem>
                    </asp:DropDownList></td>
                <td class="TableCol"></td>                                  
            </tr>
            <tr>
                <td class="TableCol">
                    <asp:Label ID="Label5" CssClass="lbl" runat="server" Text=" Document Start Date :"></asp:Label></td>
                <td class="TableCol">
                    <input id="txtDocStartDate" runat="server" type="text" class="txtbox" readonly="readonly" style="width:120px;"/><A onclick="popCalendar('txtDocStartDate');" href="javascript:;"><% Response.Write(sCal)%></A></td>
                <td class="TableCol">
                    <asp:Label ID="Label6" CssClass="lbl" runat="server" Text="Document End Date :"></asp:Label></td>
                <td class="TableCol">
                    <input id="txtDocEndDate" runat="server" type="text" class="txtbox" readonly="readonly" style="width:120px;"/><A onclick="popCalendar('txtDocEndDate');" href="javascript:;"><% Response.Write(sCal)%></A></td>
                <td class="TableCol"></td>
            </tr>
            <tr>
                <td class="TableCol">
                    <asp:Label ID="Label8" CssClass="lbl" runat="server" Text=" Vendor :"></asp:Label></td>
                <td class="TableCol">
                    <input id="txtVendor" runat="server" type="text" class="txtbox" style="width:120px;"/></td>                    
                <td class="TableCol"></td>                    
                <td class="TableCol"></td>                    
                <td class="TableCol"></td>
            </tr>
            <tr>
                <td class="TableCol" align="right" colspan="5">
                    <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="Search" />                    
                   <%-- <asp:Button ID="cmdSelectAll" CssClass="button" runat="server" Text="Select All" />                    --%>
                    <asp:Button ID="cmdClear" CssClass="button" runat="server" Text="Clear" />                    
                </td>
            </tr>
            </table>
           <table border="0" cellpadding="0" cellspacing="0" class="AllTable">                					    
					<div class="rowspacing"></div>
						<asp:datagrid id="dtgIPPList" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
							    <asp:TemplateColumn >
									<HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								<%--	<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>--%>
								</asp:TemplateColumn>
								<asp:BoundColumn SortExpression="IM_INVOICE_INDEX" DataField="IM_INVOICE_INDEX" HeaderText="" Visible="false">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="IM_INVOICE_NO" DataField="IM_INVOICE_NO" HeaderText="Document No.">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IM_INVOICE_TYPE" SortExpression="IM_INVOICE_TYPE" HeaderText="Document Type">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IM_DOC_DATE" SortExpression="IM_DOC_DATE" HeaderText="Document Date">
									    <HeaderStyle Width="10%"></HeaderStyle>
								    </asp:BoundColumn>
								<asp:BoundColumn DataField="im_prcs_sent" SortExpression="im_prcs_sent" HeaderText="PSD Sent Date" DataFormatString="{0:dd/MM/yyyy}">
									    <HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>    
							    <asp:BoundColumn DataField="IM_S_COY_NAME" SortExpression="IM_S_COY_NAME" HeaderText="Vendor">
                                        <HeaderStyle Width="10%" />
                                </asp:BoundColumn>		
                                <asp:BoundColumn DataField="IM_BANK_CODE" SortExpression="IM_BANK_CODE" HeaderText="Bank Code">
                            <HeaderStyle Width="10%" />
                        </asp:BoundColumn>			
                                <asp:BoundColumn DataField="IM_BANK_ACCT" SortExpression="IM_BANK_ACCT" HeaderText="Bank Account">
                            <HeaderStyle Width="10%" />
                        </asp:BoundColumn>			
                                <asp:BoundColumn DataField="IM_CURRENCY_CODE" SortExpression="IM_CURRENCY_CODE" HeaderText="Currency">
                                        <HeaderStyle Width="10%" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IM_INVOICE_TOTAL" SortExpression="IM_INVOICE_TOTAL" HeaderText="Payment Amount" ItemStyle-HorizontalAlign = "Right">
                                        <HeaderStyle Width="10%" HorizontalAlign = "Right"  />
                                </asp:BoundColumn>	
                                <asp:BoundColumn DataField="IM_PAYMENT_TERM" SortExpression="IM_PAYMENT_TERM" HeaderText="Payment Mode">
                                        <HeaderStyle Width="10%" />                                        
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IM_INVOICE_STATUS" SortExpression="IM_INVOICE_STATUS" HeaderText="Status" Visible="false">
                                        <HeaderStyle Width="10%" />
                                </asp:BoundColumn> 
                        <asp:BoundColumn DataField="IM_PAYMENT_DATE" SortExpression="IM_PAYMENT_DATE" HeaderText="Payment Date" Visible="false">
                            <HeaderStyle Width="10%" />                                        
                        </asp:BoundColumn>					   
							    </Columns>
						    </asp:datagrid>		        	       				  
        </table>
    </div>
    </form>
</body>
</html>
