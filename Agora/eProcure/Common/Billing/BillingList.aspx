<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BillingList.aspx.vb" Inherits="eProcure.BillingList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IPP Listing</title>
    <% Response.Write(Session("JQuery")) %> 
    <% Response.Write(Session("AutoComplete")) %>
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
                    <asp:Label ID="Label1" runat="server" CssClass="lblInfo" Text="Fill in the search criteria and click on Search button to list the  relevant IPP document."></asp:Label></td>
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
                        <asp:ListItem Value="NON">Non-Invoice</asp:ListItem>
                        <%--Zulham 27/07/2017 - IPP Stage 3--%>
                        <asp:ListItem Value="CN">Credit Note</asp:ListItem>
                        <asp:ListItem Value="DN">Debit Note</asp:ListItem>
                        <asp:ListItem Value="CA">Credit Advice</asp:ListItem>
                        <asp:ListItem Value="DA">Debit Advice</asp:ListItem>
                        <asp:ListItem Value="CNN">Credit Note(Non-Invoice)</asp:ListItem>
                        <asp:ListItem Value="DNN">Debit Note(Non-Invoice)</asp:ListItem>
                        <%--'''--%>
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
                <td class="TableCol" style="height: 24px">
                    <asp:Label ID="Label8" CssClass="lbl" runat="server" Text=" Vendor :"></asp:Label></td>
                <td class="TableCol" style="height: 24px">
                    <input id="txtVendor" runat="server" type="text" class="txtbox" style="width:120px;"/></td>                    
                <td class="TableCol" style="height: 24px"></td>                    
                <td class="TableCol" style="height: 24px"></td>                    
                <td class="TableCol" style="height: 24px"></td>
            </tr>
            <tr>
                <td class="TableCol">
                    <asp:Label ID="Label7" CssClass="lbl" runat="server" Text="Status :"></asp:Label>
                </td>
                <td class="TableCol" colspan="3">
                    <asp:CheckBoxList ID="chkdocstatus" CssClass="chklist" runat="server" RepeatDirection="Horizontal" RepeatColumns="3">
                        <asp:ListItem Value="1">Draft</asp:ListItem>
                        <asp:ListItem Value="2">Submitted</asp:ListItem>
                        <asp:ListItem Value="3">Approved</asp:ListItem>
                        <asp:ListItem Value="4">Rejected</asp:ListItem>
                        <asp:ListItem Value="5">Void</asp:ListItem>
                        <asp:ListItem Value="6">Billed</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
                <td class="TableCol" align="right">
                    <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="Search" />                    
                    <asp:Button ID="cmdSelectAll" CssClass="button" runat="server" Text="Select All" />                    
                    <asp:Button ID="cmdClear" CssClass="button" runat="server" Text="Clear" />                    
                </td>
            </tr>
            </table>
           <table border="0" cellpadding="0" cellspacing="0" class="AllTable">                					    
					<div class="rowspacing"></div>
						<asp:datagrid id="dtgIPPList" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>								
								<asp:BoundColumn SortExpression="BM_INVOICE_NO" DataField="BM_INVOICE_NO" HeaderText="Document No.">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="BM_INVOICE_TYPE" SortExpression="BM_INVOICE_TYPE" HeaderText="Document Type">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="bM_created_on" SortExpression="bM_created_on" HeaderText="Document Date">
									    <HeaderStyle Width="10%"></HeaderStyle>
								    </asp:BoundColumn>
							    <asp:BoundColumn DataField="bM_S_COY_NAME" SortExpression="bM_S_COY_NAME" HeaderText="Vendor">
                                        <HeaderStyle Width="10%" />
                                </asp:BoundColumn>		
                           <asp:BoundColumn DataField="bM_BANK_CODE" SortExpression="bM_BANK_CODE" HeaderText="Bank Code">
                            <HeaderStyle Width="10%" />
                        </asp:BoundColumn>			
                        <asp:BoundColumn DataField="bM_BANK_ACCT" SortExpression="bM_BANK_ACCT" HeaderText="Bank Account">
                            <HeaderStyle Width="10%" />
                        </asp:BoundColumn>			
                                <asp:BoundColumn DataField="bM_CURRENCY_CODE" SortExpression="bM_CURRENCY_CODE" HeaderText="Currency">
                                        <HeaderStyle Width="10%" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="bM_INVOICE_TOTAL" SortExpression="bM_INVOICE_TOTAL" HeaderText="Payment Amount" ItemStyle-HorizontalAlign = "Right">
                                        <HeaderStyle Width="10%" HorizontalAlign = "Right"  />
                                </asp:BoundColumn>		 
                                <asp:BoundColumn DataField="bM_INVOICE_STATUS" SortExpression="bM_INVOICE_STATUS" HeaderText="Status">
                                        <HeaderStyle Width="10%" />
                                </asp:BoundColumn>   						   
							    </Columns>
						    </asp:datagrid>		        	       				  
        </table>
    
    </div>
    				<table>
				<tr>
				<td class="emptycol">
				
				<input class="txtbox" id="hid6" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid5" runat="server" />			
				
				</td>
			   </tr>
</table>
    </form>
</body>
</html>
