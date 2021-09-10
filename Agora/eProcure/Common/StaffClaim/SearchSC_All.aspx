<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchSC_All.aspx.vb" Inherits="eProcure.SearchSC_All" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>SC Approval</title>
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
			SelectAllG("dtgSCList_ctl02_chkAll","chkSelection");
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
			oform.chkInclude.checked=true;
			//oform.chkIncludeHold.checked=true;
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtgSCList_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);
			oform.txtSCNo.value="";
			oform.txtStaffId.value="";
			oform.txtStaffName.value="";
			oform.txtDateFr.value="";
			oform.txtDateTo.value="";
			oform.cboDeptName.selectedIndex = 0;
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
	<body onload="document.forms[0].txtSCNo.focus();" ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
 		<%  Response.Write(Session("w_SearchSCAll_tabs"))%>
              <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
              <tr>
					<td class="linespacing1" colspan="7"></td>
			    </tr>
				<tr>
	                <td colspan="7">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant approved or rejected Staff Claim."
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
								<td class="tablecol" width="23%" >&nbsp;<strong><asp:Label ID="Label11" runat="server" Text="Staff Claim No. :"></asp:Label></strong></td>
								<td class="TableInput" width="20%">
									<asp:textbox id="txtSCNo" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="12%"></td>
								<td class="tablecol" width="20%" style="height: 24px"><strong><asp:Label ID="Label3" runat="server" Text="Department :"></asp:Label></strong></td>
								<td class="TableInput" width="34%" style="height: 24px" colspan="2"><asp:dropdownlist id="cboDeptName" Width="250px" CssClass="ddl" Runat="server"></asp:dropdownlist></td>
								<td class="tablecol" width="15%"></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%" >&nbsp;<strong><asp:Label ID="Label1" runat="server" Text="Staff ID :"></asp:Label></strong></td>
								<td class="TableInput" width="20%">
									<asp:textbox id="txtStaffId" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="12%"></td>
								<td class="tablecol" width="20%"><strong><asp:Label ID="Label2" runat="server" Text="Staff Name :"></asp:Label></strong></td>
								<td class="TableInput" width="34%" colspan="2"><asp:textbox id="txtStaffName" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="15%"></td>
							</tr>
							<tr>
								<td class="tableCOL" align="left" >&nbsp;<strong><asp:Label ID="Label14" runat="server" Text="Start Date :"></asp:Label></strong></td>
								<td	 class="TableInput"><asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><A onclick="popCalendar('txtDateFr');" href="javascript:;"><%Response.Write(CalPicture) %></td>
								<td class="tablecol" style="width: 1914px"></td>
								<td class="tableCOL" align="left" ><strong><asp:Label ID="Label15" runat="server" Text="End Date :"></asp:Label></strong></td>
								<td	 class="TableInput"><asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><A onclick="popCalendar('txtDateTo');" href="javascript:;"><%Response.Write(CalPicture) %></A></td>
								<td class="tablecol" colspan="2"></td>
							</tr>
							<tr>
											<td class="tableCOL">&nbsp;<strong>My Approval Status :</strong></td>
											<td class="TableInput" colspan="2"><asp:checkbox id="chkApproved" Text="Approved" Runat="server"></asp:checkbox>
											    &nbsp;&nbsp;&nbsp;&nbsp;<asp:checkbox id="chkReject" Text="Rejected" Runat="server"></asp:checkbox></td>
								        <td class="tablecol" colspan="5"></td>
										</tr>
										
							<tr>
											<td class="tableCOL" >&nbsp;<strong>Include Rejected SC ? :</strong></td>
											<td class="TableInput"><asp:checkbox id="chkInclude" Text="(Tick to include)" Runat="server" Checked="True"></asp:checkbox></td>
								        <td class="tablecol"></td>
								        <td class="tableCOL" ></td>
											<td class="TableInput"></td>
											<td class="tableCOL" colspan="3"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
												<input class="button" id="cmdSelectAll" onclick="SelectAll_1();" type="button" value="Select All"
													name="cmdSelectAll"/>&nbsp; <input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/>&nbsp;</td>
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
				<table class="AllTable" id="tblSearchResult" width="100%" cellspacing="0" cellpadding="0"
							border="0" runat="server">
							<tr>
								<td colspan="5"><asp:datagrid id="dtgSCList" runat="server" OnSortCommand="SortCommand_Click">
										<Columns>
											<asp:TemplateColumn SortExpression="SCM_CLAIM_DOC_NO" HeaderText="SC Number">
												<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
												<ItemStyle VerticalAlign="Middle"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkSCNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="SCM_SUBMIT_DATE" SortExpression="SCM_SUBMIT_DATE" HeaderStyle-HorizontalAlign="Left" HeaderText="Submitted Date">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="STAFF_NAME" SortExpression="STAFF_NAME" HeaderText="Staff Name">
												<HeaderStyle Width="20%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Department">
												<HeaderStyle Width="23%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CLAIM_AMT" SortExpression="CLAIM_AMT" HeaderText="Amount" HeaderStyle-HorizontalAlign="Right">
												<HeaderStyle Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="SCM_CLAIM_DOC_NO" HeaderText="SC Number"></asp:BoundColumn>
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
