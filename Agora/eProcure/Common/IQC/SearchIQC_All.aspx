<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchIQC_All.aspx.vb" Inherits="eProcure.SearchIQC_All" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>IQC Approval</title>
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
			SelectAllG("dtgPOList_ctl02_chkAll","chkSelection");
		}
		//for filtering check box
		function SelectAll_1()
		{
			checkStatus(true);
		}
		
		function checkStatus(checked)
		{
			var oform = document.forms(0);
			oform.chkApproved.checked=checked;
			oform.chkReject.checked=checked;
			oform.chkRetest.checked=checked;
			oform.chkClosed.checked=checked;
			oform.chkRejected.checked=checked;
			oform.chkOutstand.checked=checked;
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtgIQCList_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);
			oform.txtIQCNo.value="";
			oform.txtVendor.value="";
			oform.txtItemCode.value="";
			oform.txtManuName.value="";
			oform.txtIQCDateFr.value="";
			oform.txtIQCDateTo.value="";
			oform.txtExpDateFr.value="";
			oform.txtExpDateTo.value="";
			oform.cboIQC.selectedIndex=0;
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
		if (arguments.Value!="" && document.forms(0).txtIQCDateTo.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		function checkDateFr(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtIQCDateFr.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		function checkExpDateTo(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtExpDateTo.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		function checkExpDateFr(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtExpDateFr.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		-->
		</script>
	</head>
	<body onload="document.forms[0].txtIQCNo.focus();" ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
 		<%  Response.Write(Session("w_SearchIQCAll_tabs"))%>
              <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
              <tr>
					<td class="linespacing1" colspan="7"></td>
			    </tr>
				<tr>
	                <td colspan="7">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant closed or outstanding IQC."
		                ></asp:label>

	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colspan="7"></td>
			    </tr>
				<tr>
								<td class="tableheader" align="left" colspan="7" style="height: 19px">&nbsp;Search Criteria</td>
							</tr>
							<tr>
								<td class="tablecol" width="23%" >&nbsp;<strong><asp:Label ID="Label11" runat="server" Text="IQC No."></asp:Label></strong> :</td>
								<td class="Tableinput" width="20%">
									<asp:textbox id="txtIQCNo" runat="server" CssClass="txtbox" width="130px"></asp:textbox></td>
								<td class="tablecol" width="12%"></td>
								<td class="tablecol" width="20%" >
								    <strong><asp:Label id="lblVendor" runat="server" Font-Bold="True">Vendor Name</asp:Label></strong> :</td>
								<td class="Tableinput" width="34%" colspan="2">   
									<asp:textbox id="txtVendor" width="130px" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="15%"></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%" >&nbsp;<strong><asp:Label ID="Label1" runat="server" Text="Item Code"></asp:Label></strong> :</td>
								<td class="Tableinput" width="20%">
									<asp:textbox id="txtItemCode" runat="server" CssClass="txtbox" width="130px"></asp:textbox></td>
								<td class="tablecol" width="12%"></td>
								<td class="tablecol" width="20%" >
								    <strong><asp:Label id="Label2" runat="server" Font-Bold="True">Manufacturer Name</asp:Label></strong> :</td>
								<td class="Tableinput" width="34%" colspan="2">   
									<asp:textbox id="txtManuName" width="130px" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="15%"></td>
							</tr>
							<tr>
								<td class="tableCOL" align="left" >&nbsp;<strong><asp:Label ID="Label14" runat="server" Text="Start Date"></asp:Label></strong> :</td>
								<td	 class="Tableinput"><asp:textbox id="txtIQCDateFr" runat="server" Width="100px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtIQCDateFr');" href="javascript:;"><%Response.Write(CalPicture) %></a></td>
								<td class="tablecol" style="width: 1914px"></td>
								<td class="tableCOL" align="left" ><strong><asp:Label ID="Label15" runat="server" Text="End Date"></asp:Label></strong> :</td>
								<td	 class="Tableinput"><asp:textbox id="txtIQCDateTo" runat="server" Width="100px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtIQCDateTo');" href="javascript:;"><%Response.Write(CalPicture) %></a></td>
								<td class="tablecol" colspan="2"></td>
							</tr>
							<tr>
								<td class="tableCOL" align="left" >&nbsp;<strong><asp:Label ID="Label3" runat="server" Text="Expiry Start Date"></asp:Label></strong> :</td>
								<td	 class="Tableinput"><asp:textbox id="txtExpDateFr" runat="server" Width="100px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtExpDateFr');" href="javascript:;"><%Response.Write(CalPicture) %></a></td>
								<td class="tablecol" style="width: 1914px"></td>
								<td class="tableCOL" align="left" ><strong><asp:Label ID="Label4" runat="server" Text="Expiry End Date"></asp:Label></strong> :</td>
								<td	 class="Tableinput"><asp:textbox id="txtExpDateTo" runat="server" Width="100px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtExpDateTo');" href="javascript:;"><%Response.Write(CalPicture) %></a></td>
								<td class="tablecol" colspan="2"></td>
							</tr>
							<tr>
								<td class="tableCOL" style="height: 20px" >&nbsp;<strong>IQC Type</strong> :</td>
								<td class="Tableinput" colspan="2" style="height: 20px"><asp:dropdownlist id="cboIQC" Width="80px" Runat="server" CssClass="ddl"></asp:dropdownlist></td>	
								<td class="Tableinput" colspan="4" style="height: 20px"></td>
							</tr>
							<tr>
								<td class="tableCOL" style="height: 20px" >&nbsp;<strong>My Approval Status</strong> :</td>
								<td class="Tableinput" colspan="6" style="height: 20px"><asp:checkbox id="chkApproved" Text="Approved/Verify" Runat="server"></asp:checkbox>
								&nbsp;<asp:checkbox id="chkReject" Text="Rejected" Runat="server"></asp:checkbox>
								&nbsp;&nbsp;&nbsp;<asp:checkbox id="chkRetest" Text="Retest" Runat="server"></asp:checkbox>
								</td>
								
							</tr>		
							<tr>
								<td class="tableCOL" >&nbsp;<strong>IQC Status</strong> :</td>
								<td class="Tableinput" colspan="3"><asp:checkbox id="chkClosed" Text="Closed" Runat="server"></asp:checkbox>
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:checkbox id="chkRejected" Text="Rejected" Runat="server"></asp:checkbox>
								&nbsp;&nbsp;&nbsp;<asp:checkbox id="chkOutstand" Text="Outstanding" Runat="server"></asp:checkbox></td>
								<td class="Tableinput"></td>
								<td class="tableCOL" colspan="3"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
									<input class="button" id="cmdSelectAll" onclick="SelectAll_1();" type="button" value="Select All"
													name="cmdSelectAll"/>&nbsp; <input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/>&nbsp;</td>
							</tr>
				</table>
				<table>
				<tr>
					<td colspan="3"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" ShowSummary="False" ShowMessageBox="True"
							DisplayMode="List"></asp:validationsummary><asp:comparevalidator id="vldDateFtDateTo" runat="server" Type="Date" Operator="GreaterThanEqual" Display="None"
							ControlToValidate="txtIQCDateTo" ControlToCompare="txtIQCDateFr" ErrorMessage="End Date must greater than or equal to Start Date"></asp:comparevalidator><asp:customvalidator id="vldDateFr" runat="server" Display="None" ControlToValidate="txtIQCDateFr" ErrorMessage="Date To cannot be empty"
							Enabled="False" ClientValidationFunction="checkDateTo"></asp:customvalidator><asp:customvalidator id="vldDateTo" runat="server" Display="None" ControlToValidate="txtIQCDateTo" ErrorMessage="Date From cannot be empty"
							Enabled="False" ClientValidationFunction="checkDateFr"></asp:customvalidator><asp:comparevalidator id="vldExpDateFtDateTo" runat="server" Type="Date" Operator="GreaterThanEqual" Display="None"
							ControlToValidate="txtExpDateTo" ControlToCompare="txtExpDateFr" ErrorMessage="Expiry End Date must greater than or equal to Expiry Start Date"></asp:comparevalidator><asp:customvalidator id="vldExpDateFr" runat="server" Display="None" ControlToValidate="txtExpDateFr" ErrorMessage="Expiry Date To cannot be empty"
							Enabled="False" ClientValidationFunction="checkExpDateTo"></asp:customvalidator><asp:customvalidator id="vldExpDateTo" runat="server" Display="None" ControlToValidate="txtExpDateTo" ErrorMessage="Expiry Date From cannot be empty"
							Enabled="False" ClientValidationFunction="checkExpDateFr"></asp:customvalidator></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				</table>
				<table class="AllTable" id="tblSearchResult" width="100%" cellspacing="0" cellpadding="0"
							border="0" runat="server">
							<%--<tr>
								<td class="emptycol" colspan="5"></td>
							</tr>--%>
							<tr>
								<td colspan="5"><asp:datagrid id="dtgIQCList" runat="server" OnSortCommand="SortCommand_Click">
										<Columns>
											<asp:TemplateColumn SortExpression="IVL_IQC_NO" HeaderText="IQC Number">
												<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
												<ItemStyle VerticalAlign="Middle"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkIQCNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="IM_ITEM_CODE" SortExpression="IM_ITEM_CODE" HeaderStyle-HorizontalAlign="Left" HeaderText="Item Code">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="IM_INVENTORY_NAME" SortExpression="IM_INVENTORY_NAME" HeaderStyle-HorizontalAlign="Left" HeaderText="Item Name">
												<HeaderStyle Width="20%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="GM_DATE_RECEIVED" SortExpression="GM_DATE_RECEIVED" HeaderText="Submitted Date">
												<HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="IVL_LOT_QTY" SortExpression="IVL_LOT_QTY" HeaderText="Lot Qty">
												<HeaderStyle Width="6%" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor Name">
												<HeaderStyle Width="20%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="DOL_DO_MANUFACTURER" SortExpression="DOL_DO_MANUFACTURER"  HeaderText="Manufacturer Name" HeaderStyle-HorizontalAlign="Left">
												<HeaderStyle Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="APP_STATUS" SortExpression="APP_STATUS" HeaderText="My Approval Status">
												<HeaderStyle Width="17%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="IQC_STATUS_DESC" SortExpression="IQC_STATUS_DESC" HeaderText="IQC Status">
												<HeaderStyle Width="15%"></HeaderStyle>
											</asp:BoundColumn>
										</Columns>
									</asp:datagrid></td>
							</tr>
							<tr>
								<td class="emptycol" colspan="5"></td>
							</tr>
							<tr>
								<td class="emptycol" colspan="5">&nbsp;&nbsp;</td>
							</tr>
						</table>
		</form>
	</body>
</html>
