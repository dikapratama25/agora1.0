<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConvertPRListing.aspx.vb" Inherits="eProcure.ConvertPRListing" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Convert PR</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
            dim CalPicture as string = "<IMG src="& dDispatcher.direct("Plugins/images","i_Calendar2.gif") &" border=""0""></A>"
        </script> 
		
		<% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--		
		
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
			oform.txtConvertDoc.value="";
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
                <td colspan="8">
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
	            <td class="tablecol" width="15%" >&nbsp;<strong><asp:Label ID="Label11" runat="server" Text="PR No. :"></asp:Label></strong></td>
	            <td class="TableInput" width="20%">
		            <asp:textbox id="txtPRNo" runat="server" CssClass="txtbox"></asp:textbox></td>
	            <td class="tablecol" style="width: 1914px"></td>
	            <td class="tablecol" width="15%"><strong>&nbsp;<%--<asp:Label ID="Label2" runat="server" Text="Commodity Type :"></asp:Label>--%></strong></td>
	            <td class="tablecol" width="30%"><%--<asp:DropDownList ID="cboCommodityType" runat="server" CssClass="ddl" Width="99%"></asp:DropDownList>--%></td>
	            <%--<td class="tablecol" width="15%" >
	                <asp:Label id="lblVendor" runat="server" Font-Bold="True">Vendor Name :</asp:Label></td>--%>
	            <%--<td class="TableInput" width="30%" colspan="2">   
		            <asp:textbox id="txtVendor" width="100%" runat="server" CssClass="txtbox"></asp:textbox></td>--%>
	            <td class="tablecol" width="15%" colspan="3"></td>
            </tr>
            <tr>
                <td class="tableCOL" align="left" >&nbsp;<strong><asp:Label ID="Label14" runat="server" Text="Start Date :"></asp:Label></strong></td>
                <td	 class="TableInput"><asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateFr');" href="javascript:;"><%Response.Write(CalPicture) %></A></td>
                <td class="tablecol" style="width: 1914px"></td>
                <td class="tableCOL" align="left" ><strong><asp:Label ID="Label15" runat="server" Text="End Date :"></asp:Label></strong></td>
                <td	 class="TableInput"><asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateTo');" href="javascript:;"><%Response.Write(CalPicture) %></A></td>
                <td class="tablecol" colspan="3"></td>
            </tr>
            <tr width="100%">
                <td class="tableCOL" >&nbsp;<strong>Status</strong> :</td>
                <td class="TableInput" colspan="2" ><asp:checkbox id="chkSpot" Text="PO" Runat="server" Checked="false"></asp:checkbox>&nbsp;
                <asp:checkbox id="chkStock" Text="RFQ" Runat="server" Checked="false"></asp:checkbox>&nbsp;</td>
                <td class="tablecol" width="15%" ><strong><asp:Label ID="Label1" runat="server" Text="Converted Doc No. :"></asp:Label></strong></td>
	            <td class="TableInput" width="20%">
		            <asp:textbox id="txtConvertDoc" runat="server" CssClass="txtbox"></asp:textbox></td>
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
								<td colspan="5"><asp:datagrid id="dtgPRList" runat="server" OnSortCommand="SortCommand_Click">
										<Columns>
											<%--<asp:TemplateColumn SortExpression="PRM_PR_No" HeaderText="PR Number">
												<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
												<ItemStyle VerticalAlign="Middle"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>--%>
											<%--<asp:TemplateColumn HeaderText="Delete">
								                <HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
								                <ItemStyle HorizontalAlign="Center"></ItemStyle>
								                <HeaderTemplate>
									                <asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
								                </HeaderTemplate>
								                <ItemTemplate>
									                <asp:checkbox id="chkSelection" Width="5%" Runat="server"></asp:checkbox>
								                </ItemTemplate>
							                </asp:TemplateColumn>--%>
											<%--<asp:BoundColumn DataField="PRM_PR_NO" SortExpression="PRM_PR_NO" HeaderStyle-HorizontalAlign=Left HeaderText="PR Number">
												<HeaderStyle Width="8%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>--%>
											<asp:TemplateColumn SortExpression="PRM_PR_No" HeaderText="PR Number">
												<HeaderStyle HorizontalAlign="Left" Width="13%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
												<ItemTemplate>
													<%--<asp:Label ID="lblPRNo" Runat="server"></asp:Label>--%>
													<asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="PRD_VENDOR_ITEM_CODE" SortExpression="PRD_VENDOR_ITEM_CODE" HeaderStyle-HorizontalAlign=Left HeaderText="Item Code">
												<HeaderStyle Width="12%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRD_PRODUCT_DESC" SortExpression="PRD_PRODUCT_DESC" HeaderText="Item Name">
												<HeaderStyle Width="20%"></HeaderStyle>
											</asp:BoundColumn>
											<%--<asp:BoundColumn DataField="PRM_PR_DATE" SortExpression="PRM_PR_DATE" HeaderText="Approval Date">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>--%>
											<asp:BoundColumn DataField="PRD_CONVERT_TO_DATE" SortExpression="PRD_CONVERT_TO_DATE" HeaderText="Converted Date">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<%--<asp:BoundColumn DataField="PRD_CONVERT_TO_DOC" SortExpression="PRD_CONVERT_TO_DOC" HeaderText="Converted Doc No.">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>--%>
											<asp:TemplateColumn SortExpression="PRD_CONVERT_TO_DOC" HeaderText="Converted Doc No.">
												<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkConvert"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
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
											<asp:BoundColumn DataField="PRD_UNIT_COST" SortExpression="PRD_UNIT_COST"  HeaderText="Last Txn. Price" >
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
								<td class="emptycol" colspan="5">&nbsp;&nbsp;</td>
							</tr>
						</table>
				<table>
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
				</table>
		</form>
	</body>
</HTML>
