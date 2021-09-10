<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WHTReceipt.aspx.vb" Inherits="eProcure.WHTReceipt" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
<head>
    <title>WHTReceipt</title>
    <script runat="server">
        Dim dDispatcher As New AgoraLegacy.dispatcher
        dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
        Dim sCal As String = "<IMG src=" & dDispatcher.direct("Plugins/Images", "i_Calendar2.gif") & " border=""0"">"
        Dim typeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=IPPCOY&coytype=V")
        Dim CSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "AutoComplete.css") & """ rel='stylesheet'>"
    </script>
    
    <% Response.Write(CSS)%>
    <%response.write(Session("WheelScript"))%>
	<%response.write(Session("JQuery"))%>
	<% Response.Write(Session("AutoComplete")) %>
	
    <script type="text/javascript">
    
    $(document).ready(function(){
            $("#txtVendor").autocomplete("<% Response.write(typeahead) %>", {
            width: 342,
            scroll: true,
            selectFirst: false
            });
    });
    
    function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=800,height=600,location=no,toolbar=no,menubar=no,scrollbars=yes,resizable=no");
			return false;
		}
	function popCalendar(val)
		{
			txtVal= document.getElementById(val);
			//window.open('../popCalendar.aspx?textbox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
			window.open('<% response.write(PopCalendar) %>' ,'cal','status=no,resizable=yes,width=180,height=155,left=270,top=180');
			
		}
	function selectAll()
		{
			SelectAllG("dtgWHTList_ctl02_chkAll","chkSelection");
		}
		
	function checkChild(id)
    {
	    checkChildG(id,"dtgWHTList_ctl02_chkAll","chkSelection");
	}

	function isNumberKey(evt)
    {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

            return true;
    }    
//	function checkChild(id)
//		{
//			checkChildG(id,"dtgInvoice_ctl02_chkAll","chkSelection");
//		}
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <%--<% Response.write(Session("w_IPP_tabs")) %>--%>
    
        <table id="table1" cellpadding="0" cellspacing="0" class="AllTable">        
            <tr>
				<td class="Header"><asp:label id="lblHeader" runat="server" Text="WHT Receipt"></asp:label></td>
			</tr>
			<tr>
                <td class="EmptyCol"></td>
            </tr>
            <tr>
                <td class="EmptyCol">
                    <asp:Label ID="Label1" runat="server" CssClass="lblInfo" Text="Fill in the search criteria and click on Search button to list the  relevant document. Click the Save button to save record."></asp:Label></td>
            </tr>
         </table>
         <table id="Table2" class="AllTable" cellpadding="0" cellspacing="0">
            <tr>
                <td class="TableHeader" colspan="5">
                    <asp:Label ID="Label2" runat="server" Text=" Search Criteria"></asp:Label></td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 12%">
                    <strong>Document No</strong> :</td>
                <td class="TableCol" style="width: 28%">
                    <asp:TextBox ID="txtDocNo" CssClass="txtbox" MaxLength="50" runat="server" style="width:180px;"></asp:TextBox></td>                
                <td class="TableCol" style="width: 10%">
                    <strong>Receipt No</strong> :</td>
                <td class="TableCol" style="width: 30%">
                    <asp:TextBox ID="txtReceiptNo" CssClass="txtbox" MaxLength="50" runat="server" style="width:180px;"></asp:TextBox></td>
                <td class="TableCol" align="right" style="width: 20%">
                </td>                                  
            </tr>    
            <tr>
                <td class="TableCol" style="width: 12%">
                    <strong>Vendor</strong> :</td>
                <td class="TableCol" style="width: 28%">
                    <asp:TextBox ID="txtVendor" CssClass="txtbox" MaxLength="50" runat="server" style="width:180px;"></asp:TextBox></td>                
                <td class="TableCol" style="width: 10%"></td>
                <td class="TableCol" style="width: 30%"></td>
                <td class="TableCol" align="right" style="width: 20%">
                    <asp:Button ID="btnSearch" CssClass="button" runat="server" Text="Search" />                    
                    <asp:Button ID="cmdClear" CssClass="button" runat="server" Text="Clear" />  
                </td> 
            </tr>
            <tr>
                <td colspan="5" class="EmptyCol">
                </td>
            </tr>
            </table>
           <table border="0" cellpadding="0" cellspacing="0" class="AllTable"> 
           <tr>
                <td class="emptycol">               					    	
						<asp:datagrid id="dtgWHTList" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:BoundColumn SortExpression="IM_S_COY_NAME" DataField="IM_S_COY_NAME" HeaderText="Vendor">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="IM_INVOICE_NO" DataField="IM_INVOICE_NO" HeaderText="Document No">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="IM_DOC_DATE" DataField="IM_DOC_DATE" HeaderText="Document Date">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="IM_CURRENCY_CODE" DataField="IM_CURRENCY_CODE" HeaderText="Currency">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="WHT_AMT" DataField="WHT_AMT" HeaderText="WHT Tax Amount (MYR)">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Section">
									<HeaderStyle HorizontalAlign="Left" Width="16%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList ID="ddlSection" runat="server" Width="100%" CssClass="ddl">
										    <asp:ListItem value="" Selected="True">---Select---</asp:ListItem>
										    <asp:ListItem value="S107A">S107A</asp:ListItem>
										    <asp:ListItem value="S109">S109</asp:ListItem>
										    <asp:ListItem value="S109B">S109B</asp:ListItem>
										    <asp:ListItem value="S109F">S109F</asp:ListItem>
										</asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Receipt No">
									<HeaderStyle HorizontalAlign="Left" Width="19%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtReceipt" CssClass="txtbox" MaxLength="50" Width="100%" Runat="server"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Receipt Date">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtDate" CssClass="txtbox" Width="90px" contentEditable="false" Runat="server"></asp:TextBox>
										<asp:label id="lblDate" Runat="server"></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="IM_INVOICE_INDEX" HeaderText="IM_INVOICE_INDEX"></asp:BoundColumn>
						    </Columns>
					</asp:datagrid>	
				</td> 
			</tr>    	       				  
        </table>
       <table>
            <tr>
			    <td>
			        <asp:Button ID="btnSave" CssClass="button" runat="server" Text="Save" /> 
			    </td>
			</tr>	
			<tr>
				<td>
				    <asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label>
				</td>
		    </tr> 
			<tr>
				<td class="emptycol">
				    <input class="txtbox" id="hid6" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid5" runat="server" />			
				</td>
			</tr>
        </table>
    </form>
</body>
</html>
