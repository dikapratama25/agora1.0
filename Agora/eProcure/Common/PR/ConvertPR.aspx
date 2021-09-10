<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConvertPR.aspx.vb" Inherits="eProcure.ConvertPR" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Convert PR</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<% Response.Write(Session("JQuery")) %>	
        <% Response.Write(Session("AutoComplete")) %>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            ' Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
            dim CalPicture as string = "<IMG src="& dDispatcher.direct("Plugins/images","i_Calendar2.gif") &" border=""0""></A>"
            Dim commodity As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=Commodity")
        </script> 
		
		<% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--		
		
		    $(document).ready(function(){
		    $("#txtCommodity").autocomplete("<% Response.write(commodity) %>", {
            width: 250,
            scroll: true,
            selectFirst: false
            });
            $("#txtCommodity").result(function(event, data, formatted) {
            if (data)
            $("#hidCommodity").val(data[1]);
            });
            $("#txtCommodity").blur(function() {
            var hidcommodity = document.getElementById("hidCommodity").value;                        
            if(hidcommodity == "")
            {
                $("#txtCommodity").val("");
            }
            }); 
            
            $('#cmdPO').click(function() {
            summary = Page_ValidationSummaries[0];                        
            if(summary.innerHTML == "")
            {
                if (raiserfqconfirm('PO') == false)
                {		    
		            return false;
		          }
		        else
		        {
                document.getElementById("cmdPO").style.display= "none";
                document.getElementById("cmdRFQ").style.display= "none";
                return true;
                }
              }      
            });
            $('#cmdRFQ').click(function() {
            summary = Page_ValidationSummaries[0];                        
                if(summary.innerHTML == "")
                {
                    if (raiserfqconfirm('RFQ') == false)
                    {		    
		                return false;
		              }
		            else
                    {
                    document.getElementById("cmdPO").style.display= "none";
                    document.getElementById("cmdRFQ").style.display= "none";
                    return true;
                    }     
                }      
            });        
            });
            
		//For check in Datagrid
		function selectAll()
		{
			SelectAllG("dtgPRList_ctl02_chkAll","chkSelection");
		}
		//for filtering check box
		function SelectAll_1()
		{
			checkStatus(true);
		}
		
		function checkStatus(checked)
		{
			var oform = document.forms(0);
			oform.chkSpot.checked=checked;
			oform.chkStock.checked=checked;
			oform.chkMRO.checked=checked;
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtgPRList_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);
			oform.txtPRNo.value="";
			oform.txtDateFr.value="";
			oform.txtDateTo.value="";
			//oform.cboCommodityType.selectedIndex = 0;
			oform.txtCommodity.value="";
			oform.hidCommodity.value="";
			checkStatus(false);
		}
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","help:No;resizable: No;Status:No;dialogHeight: 255px; dialogWidth: 300px");
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
		if (arguments.Value!="" && document.forms(0).txtDateTo.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		function checkDateFr(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtDateFr.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		function raiserfqconfirm(type)
        {	
            ans=confirm("Are you sure that you want to Raise " + type + "?");
            if (ans){
                if (CheckAtLeastOne('chkSelection'))
                {	                
	                return true;
	            }
	            else
	            {
	                return false;
	            }
	        }
            else
            {             
	            return false;
	        }
        }	
		
		-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
 		<%  Response.Write(Session("w_ConvertPR_tabs"))%>
            <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
            <tr>
	            <td class="linespacing1" colspan="8"></td>
            </tr>
            <tr>
                <td class="EmptyCol" colspan="8">
                    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
                            Text="Fill in the search criteria and click Search button to list the relevant PR."
                    ></asp:label>

                </td>
            </tr>
            <tr>
	            <td class="linespacing2" colspan="8"></td>
            </tr>
            <tr>
	            <td class="tableheader" align="left" colspan="8" style="height: 19px">&nbsp;Search Criteria</td>
            </tr>
            <tr>
	            <td class="tablecol" width="20%" >&nbsp;<strong><asp:Label ID="Label11" runat="server" Text="PR No. :"></asp:Label></strong></td>
	            <td class="tablecol" width="15%">
		            <asp:textbox id="txtPRNo" runat="server" CssClass="txtbox"></asp:textbox></td>
	            <td class="tablecol" style="width: 1914px"></td>
	            <td class="tablecol" width="25%"><strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="Commodity Type :"></asp:Label></strong></td>
	            <%--<td class="tablecol" width="15%"><strong><asp:Label ID="Label3" runat="server" Text="Commodity Type " CssClass="lbl"></asp:Label></strong>:</td>--%>
	            <td class="TableCol" colspan="5" style="width: 30%"><asp:textbox id="txtCommodity" runat="server" CssClass="txtbox"></asp:textbox><input type="hidden" id="hidCommodity" runat="server"/></td>
                   
	            <%--<td class="tablecol" width="30%"><asp:DropDownList ID="cboCommodityType" runat="server" CssClass="ddl" Width="99%"></asp:DropDownList></td>--%>
	            <%--<td class="tablecol" width="15%" >
	                <asp:Label id="lblVendor" runat="server" Font-Bold="True">Vendor Name :</asp:Label></td>--%>
	            <%--<td class="TableInput" width="30%" colspan="2">   
		            <asp:textbox id="txtVendor" width="100%" runat="server" CssClass="txtbox"></asp:textbox></td>--%>
	            <%--<td class="tablecol" width="10%" colspan="3"></td>--%>
            </tr>
            <tr>
                <td class="tableCOL" align="left">&nbsp;<strong><asp:Label ID="Label14" runat="server" Text="Start Date :"></asp:Label></strong></td>
                <td	class="tablecol"><asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateFr');" href="javascript:;"><%Response.Write(CalPicture) %></a></td>
                <td class="tablecol" style="width: 1914px"></td> 
                <td class="tableCOL" align="left"><strong><asp:Label ID="Label15" runat="server" Text="End Date :"></asp:Label></strong></td>
                <td	class="tablecol"><asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateTo');" href="javascript:;"><%Response.Write(CalPicture) %></a></td>
                <td class="tablecol" colspan="3"></td> 
            </tr>
            <tr width="100%">
                <td class="tableCOL" >&nbsp;<strong>Item Type</strong> :</td>
                <td class="tablecol" colspan="3" ><asp:checkbox id="chkSpot" Text="Spot" Runat="server" Checked="false"></asp:checkbox>&nbsp;
                <asp:checkbox id="chkStock" Text="Stock" Runat="server" Checked="false"></asp:checkbox>&nbsp;
                <asp:checkbox id="chkMRO" Text="MRO" Runat="server" Checked="false"></asp:checkbox></td>
                <td class="tablecol" colspan="1"></td>
                <td class="tableCOL" colspan="3"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
                <input class="button" id="cmdSelectAll" onclick="SelectAll_1();" type="button" value="Select All"
                name="cmdSelectAll"/>&nbsp; <input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/>&nbsp;</td>
            </tr>
            </table>
				<%--<table>
				<tr>
					<td colspan="3"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" ShowSummary="False" ShowMessageBox="True"
							DisplayMode="List"></asp:validationsummary><asp:comparevalidator id="vldDateFtDateTo" runat="server" Type="Date" Operator="GreaterThanEqual" Display="None"
							ControlToValidate="txtDateTo" ControlToCompare="txtDateFr" ErrorMessage="End Date must greater than or equal to Start Date"></asp:comparevalidator><asp:customvalidator id="vldDateFr" runat="server" Display="None" ControlToValidate="txtDateFr" ErrorMessage="Date To cannot be empty"
							Enabled="False" ClientValidationFunction="checkDateTo"></asp:customvalidator><asp:customvalidator id="vldDateTo" runat="server" Display="None" ControlToValidate="txtDateTo" ErrorMessage="Date From cannot be empty"
							Enabled="False" ClientValidationFunction="checkDateFr"></asp:customvalidator></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				</table>--%>
				<table class="AllTable" id="tblSearchResult" width="100%" cellspacing="0" cellpadding="0"
							border="0" runat="server">
							<tr>
								<td class="emptycol" colspan="5"></td>
							</tr>
							<tr>
								<td class="EmptyCol" colspan="5"><asp:datagrid id="dtgPRList" runat="server" OnSortCommand="SortCommand_Click">
										<Columns>
											<%--<asp:TemplateColumn SortExpression="PRM_PR_No" HeaderText="PR Number">
												<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
												<ItemStyle VerticalAlign="Middle"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>--%>
											<asp:TemplateColumn HeaderText="Delete">
								                <HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
								                <ItemStyle HorizontalAlign="Center"></ItemStyle>
								                <HeaderTemplate>
									                <asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
								                </HeaderTemplate>
								                <ItemTemplate>
									                <asp:checkbox id="chkSelection" Width="5%" Runat="server"></asp:checkbox>
								                </ItemTemplate>
							                </asp:TemplateColumn>
											<%--<asp:BoundColumn DataField="PRM_PR_NO" SortExpression="PRM_PR_NO" HeaderStyle-HorizontalAlign=Left HeaderText="PR Number">
												<HeaderStyle Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>																				
											</asp:BoundColumn>--%>
											<asp:TemplateColumn SortExpression="PRM_PR_No" HeaderText="PR Number">
												<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
												<ItemTemplate>
													<asp:Label ID="lblPRNo" Runat="server" style="display:none"></asp:Label>
													<asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" HeaderText="Buyer Name">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRD_VENDOR_ITEM_CODE" SortExpression="PRD_VENDOR_ITEM_CODE" HeaderStyle-HorizontalAlign=Left HeaderText="Item Code">
												<HeaderStyle Width="12%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<%--<asp:BoundColumn DataField="PRD_PRODUCT_DESC" SortExpression="PRD_PRODUCT_DESC" HeaderText="Item Name">
												<HeaderStyle Width="20%"></HeaderStyle>
											</asp:BoundColumn>--%>
											<asp:TemplateColumn SortExpression="PRD_PRODUCT_DESC" HeaderText="Item Name" >
									            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									            <ItemStyle HorizontalAlign="Left"></ItemStyle>
									            <ItemTemplate>
										            <asp:Label ID="lblProductDesc" style="width:90px" Runat="server"></asp:Label>
										            <asp:Label ID="lblPRIndex" Runat="server" Visible="false" ></asp:Label>
										            <asp:Label ID="lblPRLine" Runat="server" Visible="false" ></asp:Label>
										            <asp:Label ID="lblBill" Runat="server" Visible="false" ></asp:Label>
										            <asp:Label ID="lblAtt" Runat="server" Visible="false" ></asp:Label>
									            </ItemTemplate>
								            </asp:TemplateColumn>
											<asp:BoundColumn DataField="PRM_PR_DATE" SortExpression="PRM_PR_DATE" HeaderText="Approval Date">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PM_LAST_TXN_S_COY_NAME" SortExpression="PRD_S_COY_ID" HeaderText="Vendor">
												<HeaderStyle Width="18%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRD_ORDERED_QTY" SortExpression="PRD_ORDERED_QTY" HeaderText="Quantity">
												<HeaderStyle Width="5%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PM_LAST_TXN_PRICE_CURR" SortExpression="PM_LAST_TXN_PRICE_CURR" HeaderText="Currency">
												<HeaderStyle Width="6%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PM_LAST_TXN_PRICE" SortExpression="PM_LAST_TXN_PRICE"  HeaderText="Last Txn. Price" >
												<HeaderStyle Width="6%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRD_UNIT_COST" SortExpression="PRD_UNIT_COST"  HeaderText="Amount" >
												<HeaderStyle Width="6%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRD_GST" SortExpression="PRD_GST"  HeaderText="Tax" >
												<HeaderStyle Width="5%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRD_GST" SortExpression="PRD_GST"  HeaderText="SST Amount" >
												<HeaderStyle Width="5%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRD_ACCT_INDEX" SortExpression="PRD_ACCT_INDEX"  HeaderText="Budget Account" >
												<HeaderStyle Width="5%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<%--<asp:BoundColumn DataField="STATUS_DESC" HeaderText="Status">
												<HeaderStyle Width="30%"></HeaderStyle>
											</asp:BoundColumn>--%>
											<%--<asp:BoundColumn HeaderText="PO Number">
									            <HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left"/>
								            </asp:BoundColumn>--%>
											<%--<asp:BoundColumn Visible="False" DataField="PRM_PR_No" HeaderText="PR Number"></asp:BoundColumn>--%>
										</Columns>
									</asp:datagrid></td>
							</tr>
							<tr>
					            <td class="emptycol" colspan="5">
						            <asp:button id="cmdPO" runat="server" CssClass="Button" Text="Raise PO" Visible="false" ></asp:button>
						            <asp:button id="cmdRFQ" runat="server" CssClass="Button" Text="Raise RFQ" Visible="false" ></asp:button>                                    
                                </td>
				            </tr>
						</table>
				<table>
				<tr>
					<td class="emptycol" colspan="3"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" ShowSummary="False" ShowMessageBox="True"
							DisplayMode="List"></asp:validationsummary><asp:comparevalidator id="vldDateFtDateTo" runat="server" Type="Date" Operator="GreaterThanEqual" Display="None"
							ControlToValidate="txtDateTo" ControlToCompare="txtDateFr" ErrorMessage="End Date must greater than or equal to Start Date"></asp:comparevalidator><asp:customvalidator id="vldDateFr" runat="server" Display="None" ControlToValidate="txtDateFr" ErrorMessage="Date To cannot be empty"
							Enabled="False" ClientValidationFunction="checkDateTo"></asp:customvalidator><asp:customvalidator id="vldDateTo" runat="server" Display="None" ControlToValidate="txtDateTo" ErrorMessage="Date From cannot be empty"
							Enabled="False" ClientValidationFunction="checkDateFr"></asp:customvalidator></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				</table>
		</form>
	</body>
</html>
