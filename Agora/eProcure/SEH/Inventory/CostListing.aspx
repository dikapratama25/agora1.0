<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CostListing.aspx.vb" Inherits="eProcure.CostListingSEH" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>Inventory Listing</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
            dim CalPicture as string = "<IMG src="& dDispatcher.direct("Plugins/images","i_Calendar2.gif") &" border=""0""></A>"
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            Dim typeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=code")
            Dim typeahead1 As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=name")

        </script>
         <% Response.Write(css)%>   
        <% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
        <% Response.Write(Session("BgiFrame")) %>
         

		<script language="javascript">
		<!--
		    function PopWindow(myLoc)
		    {
			    window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			    return false;
		    }
		    
		    function popCalendar(val)
		{
			txtVal= document.getElementById(val);
			//window.open('../popCalendar.aspx?textbox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
			window.open('<%Response.Write(PopCalendar) %>','cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
		}
		function checkDateTo(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txt_startdate.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		function checkDateFr(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txt_enddate.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		    function Reset(){
			    var oform = document.forms(0);			    
			    oform.txtItemCode.value = ""
			    oform.txtItemName.value=""	
			    oform.ddl_InvType.selectedIndex=0
			    oform.ddl_SubLoc.selectedIndex=0;
//			    oform.rd2.SelectedValue = "N";
//			    oform.rd2.Items.FindByValue("N").Selected = True;   
		    }
		    
		    $(document).ready(function(){
            $("#txtItemCode").autocomplete("<% Response.write(typeahead) %>", {
            width: 200,
            scroll: true,
            selectFirst: false
            });    
            $("#txtItemName").autocomplete("<% Response.write(typeahead1) %>", {
            width: 200,
            scroll: true,
            selectFirst: false
            });        
            });
            
            function ShowDialog(filename,height)
			{
				
				var retval="";
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable:Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 680px");
				//retval=window.open(filename);
				if (retval == "1" || retval =="" || retval==null)
				{  window.close;
					return false;

				} else {
				    window.close;
					return true;

				}
			}
		
		-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchInvListing_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
			<TR>
			    <td class="linespacing1" colspan="4"></td>
		    </TR>
			<TR>
				<TD class="EmptyCol" colSpan="6">
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Fill in the search criteria and click Search button to list the relevant inventory." BorderColor="Transparent"></asp:label>
				</TD>
			</TR>
			<tr><td class="rowspacing"  colSpan="6" style="height: 19px"></td></tr>
			<TR>
				<TD class="TableHeader" colSpan="6" style="height: 19px">Search Criteria</TD>
			</TR>
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px"><strong><asp:Label ID="Label1" runat="server" Text="Item Code :" CssClass="lbl"></asp:Label></strong></TD>
				<TD class="TableCol" style="height: 18px; width: 236px;"><asp:textbox id="txtItemCode" runat="server" CssClass="txtbox" Width="180px"></asp:textbox></TD>
				<TD class="TableCol" style="height: 18px; width: 45px;"></TD>
				<TD class="TableCol" style="height: 18px"><strong><asp:Label ID="Label4" runat="server" Text="Item Name :" CssClass="lbl"></asp:Label></strong></TD>
               <TD class="TableCol" style="height: 18px; width: 174px;"><asp:textbox id="txtItemName" runat="server" CssClass="txtbox" Width="180px"></asp:textbox></TD>				
 			   <TD class="TableCol" style="height: 18px"></TD>
			</TR>
			<TR class="tablecol">
				<TD class="TableCol" style="height: 18px;"><strong><asp:Label ID="lblLoc" runat="server" Text="Start Date" CssClass="lbl"></asp:Label><asp:Label ID="Label3" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></TD>
				<TD class="TableCol" style="height: 18px; width: 236px;">
                    <asp:TextBox ID="txt_startdate" runat="server" CssClass="txtbox" Width="144px" contentEditable="false"></asp:TextBox><a onclick="popCalendar('txt_startdate');" href="javascript:;"><%Response.Write(CalPicture) %></a>
                    <input id="hidDateS" runat="server" name="hidDateS" size="1" style="width: 32px;
                        height: 22px" type="hidden" /></TD>
				<TD class="TableCol" style="height: 18px; width: 45px;" ></TD>
				<TD class="TableCol" style="height: 18px"><strong><asp:Label ID="lblSubLoc" runat="server" Text="End Date" CssClass="lbl"></asp:Label><asp:Label ID="Label5" runat="server" Text=" :" CssClass="lbl"></asp:Label></strong></TD>
                <TD class="TableCol" style="height: 18px; width: 174px;">
                    <asp:TextBox ID="txt_enddate" runat="server" CssClass="txtbox" Width="136px" contentEditable="false"></asp:TextBox><a onclick="popCalendar('txt_enddate');" href="javascript:;"><%Response.Write(CalPicture) %></a>
                    <input id="hidDateE" runat="server" name="hidDateE" size="1" style="width: 32px; height: 22px" type="hidden" /></TD>				
				<TD class="TableCol" style="height: 18px">
                    </TD>
			</TR>	
			<TR class="tablecol">
				<TD class="TableCol" style="height: 24px" width="18%"><strong>
                    <asp:Label ID="Label2" runat="server" CssClass="lbl" Text="Inventory Type"></asp:Label><asp:Label
                        ID="Label6" runat="server" CssClass="lbl" Text=" :"></asp:Label></strong></TD>
				<TD class="TableCol" style="height: 24px; width: 236px;"><asp:DropDownList ID="ddl_InvType" runat="server" AutoPostBack="True" CssClass="ddl"
                        Style="margin-bottom: 1px" Width="180px">
                    <asp:ListItem Value="">---Select---</asp:ListItem>
                    <asp:ListItem Value="GRN">Good Received Note</asp:ListItem>
                    <asp:ListItem Value="II">Inventory Issue</asp:ListItem>
                    <asp:ListItem Value="RO">Return Outward</asp:ListItem>
                    <asp:ListItem Value="RI">Return Inward</asp:ListItem>
                    <asp:ListItem Value="WO">Write Off</asp:ListItem>
                    <asp:ListItem Value="IIC">Inventory Issue Cancel</asp:ListItem>
                    </asp:DropDownList></TD>
				<TD class="TableCol" style="height: 24px; width: 45px;"></TD>
			    <TD class="TableCol" style="height: 24px" width="23%"><strong></strong></TD>
			    <TD class="TableCol" style="height: 24px; width: 174px;">
                    &nbsp;</TD>
				    <TD class="TableCol" style="height: 24px">
				    <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" Width="64px"></asp:button>&nbsp;
                    <asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear"></asp:button>
				</TD>
			</TR>				
			</TABLE>
			<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" width="100%">
							<TR>
					<TD class="EmptyCol" colspan="6" style="height: 152px">
                        <asp:datagrid ID="dtgCos" runat="server" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundColumn DataField="IM_ITEM_CODE" HeaderText="Item Code" SortExpression="IM_ITEM_CODE">
                                    <HeaderStyle Width="8%" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IM_INVENTORY_NAME" HeaderText="Item Name" SortExpression="IM_INVENTORY_NAME">
                                    <HeaderStyle HorizontalAlign="Left" Width="11%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IC_COST_DATE" HeaderText="Date" SortExpression="IC_COST_DATE" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IC_COST_OPEN_QTY" HeaderText="Opening Qty" SortExpression="IC_COST_OPEN_QTY">
                                    <HeaderStyle HorizontalAlign="Right" Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IC_COST_OPEN_UPRICE" HeaderText="Opening U/p" SortExpression="IC_COST_OPEN_UPRICE">
                                    <HeaderStyle HorizontalAlign="Right" Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IC_COST_OPEN_COST" HeaderText="Opening T.Value" SortExpression="IC_COST_OPEN_COST">
                                    <HeaderStyle HorizontalAlign="Right" Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="RECEIVED_QTY" HeaderText="Receiving Qty" SortExpression="RECEIVED_QTY">
                                    <HeaderStyle HorizontalAlign="Right" Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Receiving U/p" DataField="RECEIVED_UPRICE" SortExpression="RECEIVED_UPRICE">
                                    <HeaderStyle HorizontalAlign="Right" Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Receiving T.Value" DataField="RECEIVED_COST" SortExpression="RECEIVED_COST">
                                    <HeaderStyle HorizontalAlign="Right" Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Issuing Qty" DataField="ISSUED_QTY" SortExpression="ISSUED_QTY">
                                    <HeaderStyle HorizontalAlign="Right" Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Issuing U/p" DataField="ISSUED_UPRICE" SortExpression="ISSUED_UPRICE">
                                    <HeaderStyle HorizontalAlign="Right" Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Issuing T.Value" DataField="ISSUED_COST" SortExpression="ISSUED_COST">
                                    <HeaderStyle HorizontalAlign="Right" Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Closing Qty" DataField="IC_COST_CLOSE_QTY" SortExpression="IC_COST_CLOSE_QTY">
                                    <HeaderStyle HorizontalAlign="Right" Width="6%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Closing U/p" DataField="IC_COST_CLOSE_UPRICE" SortExpression="IC_COST_CLOSE_UPRICE">
                                    <HeaderStyle HorizontalAlign="Right" Width="6%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Closing T.Value" DataField="IC_COST_CLOSE_COST" SortExpression="IC_COST_CLOSE_COST">
                                    <HeaderStyle HorizontalAlign="Right" Width="6%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Transaction Type" DataField="IC_INVENTORY_TYPE" SortExpression="IC_INVENTORY_TYPE">
                                    <HeaderStyle HorizontalAlign="Left" Width="23%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                            </Columns>
                        </asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EmptyCol" style="height: 24px; width: 119px;">
                        &nbsp;&nbsp;
                        <br />
                        <input id="cmdPrint" runat="server" class="button" style="width: 128px" type="button"
                            value="Print Count List" />
                        <TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" style="width: 334%">
                            <tr>
                                <TD class="TableCol" style="height: 18px; width: 301px;">
                                    <asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List"
                                        ShowMessageBox="True" ShowSummary="False" />
                                    &nbsp; &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <TD class="TableCol" style="height: 18px; width: 301px;">
                                    &nbsp;<asp:CompareValidator ID="vldDateFtDateTo" runat="server" ControlToCompare="txt_startdate"
                            ControlToValidate="txt_enddate" Display="None" ErrorMessage="End Date must greater than or equal to Start Date"
                            Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>
                        <asp:CustomValidator ID="vldDateFr" runat="server" ClientValidationFunction="checkDateTo"
                            ControlToValidate="txt_startdate" Display="None" Enabled="False" ErrorMessage="Date From cannot be empty"></asp:CustomValidator>
                                    <asp:CustomValidator ID="vldDateTo" runat="server" ClientValidationFunction="checkDateFr"
                            ControlToValidate="txt_enddate" Display="None" Enabled="False" ErrorMessage="Date From cannot be empty"></asp:CustomValidator></td>
                            </tr>
                        </table>
                    </TD>
				</TR>					
			</TABLE>
		</form>
	</body>
</HTML>
