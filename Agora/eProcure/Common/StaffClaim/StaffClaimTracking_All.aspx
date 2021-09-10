<%@ Page Language="vb" AutoEventWireup="false" Codebehind="StaffClaimTracking_All.aspx.vb" Inherits="eProcure.StaffClaimTracking_All" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>View All SC</title>
<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim StartCalendar As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr") & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim EndCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
        </script>
        <% Response.Write(css)%>
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
			oform.chkDraft.checked=checked;
			oform.chkSubmit.checked=checked;
			oform.chkPending.checked=checked;
			oform.chkApproved.checked=checked;
			oform.chkRejected.checked=checked;
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtgScList_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);
			oform.txtSCNo.value="";
			oform.cboDeptName.selectedIndex = 0;
			oform.txtStaffId.value="";
			oform.txtStaffName.value="";
			oform.txtDateFr.value="";
			oform.txtDateTo.value="";
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
			window.open('../Calendar/viewCalendar.aspx?TextBox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
		}
		
		function checkDateTo(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtDateTo.value=="") 
			{
			arguments.IsValid=false;}
		else
		{
			arguments.IsValid=true;}
		}
		
		function checkDateFr(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtDateFr.value=="") 
			{
			arguments.IsValid=false;}
		else
		{
			arguments.IsValid=true;}
		}
		
		-->
		</script>
	</HEAD>
	<body onload="document.forms[0].txtSCNo.focus();" ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
				<tr>
					<td class="header"  colspan="5" style="height: 19px; padding :0px;">&nbsp;<asp:label id="lblTitle" runat="server">Staff Claim Tracking (ALL)</asp:label></td>
				</tr>
				<tr>
					<td class="linespacing1" colspan="5"></td>
			    </tr>
				<tr style="height: 19px;">
	                <td class="emptycol" colspan="5">
		                &nbsp;<asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Fill in the search criteria and click Search button to list the relevant Staff Claim."></asp:label>
	                </td>
                </tr>
		        <tr>
			        <td class="tableheader" colspan="5">&nbsp;Search Criteria</td>
		        </tr>
			    <tr>
				    <td class="tablecol" style="WIDTH: 15%">&nbsp;<strong>Staff Claim No.</strong>&nbsp;:</td>
				    <td class="tablecol" style="width: 20%"><asp:textbox id="txtSCNo" runat="server" CssClass="txtbox" Width="150px"></asp:textbox></td>
				    <td class="tablecol" style="width: 15%; height: 24px">&nbsp;<strong>Department</strong> :</td>
					<td class="tablecol" style="width: 35%; height: 24px"><asp:dropdownlist id="cboDeptName" Width="250px" CssClass="ddl" Runat="server"></asp:dropdownlist></td>
					<td class="tablecol" style="height: 24px">&nbsp;</td>
			    </tr>
			    <tr>
					<td class="tablecol">&nbsp;<strong>Staff ID</strong> :</td>
					<td class="tablecol"><asp:textbox id="txtStaffId" runat="server" CssClass="txtbox" Width="150px"></asp:textbox>&nbsp;</td>
					<td class="tablecol">&nbsp;<strong>Staff Name</strong> :</td>
					<td class="tablecol"><asp:textbox id="txtStaffName" runat="server" CssClass="txtbox" Width="162px"></asp:textbox>&nbsp;</td>
					<td class="tablecol"></td>
				</tr>
				<tr>
					<td class="tablecol" style="height: 19px;">
						&nbsp;<asp:Label Runat="server" ID="lblStartDate" Font-Bold="True">Start Date :</asp:Label>
					</td>
					<td class="tablecol" style="width: 181px"><asp:textbox id="txtDateFr" runat="server" CssClass="txtbox" Width="110px" contentEditable="false" ></asp:textbox><% Response.Write(StartCalendar)%></td>
					<td class="tablecol">
						&nbsp;<asp:Label Runat="server" ID="lblEndDate" Font-Bold="True" Width="75px">End Date :</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					</td>
					<td class="tablecol"><asp:textbox id="txtDateTo" runat="server" CssClass="txtbox" Width="110px" contentEditable="false" ></asp:textbox><% Response.Write(EndCalendar)%></td>
					<td class="tablecol"></td>
				</tr>
				<tr>
				    <td class="tablecol">&nbsp;<strong>Status</strong> :</td>
					<td class="tablecol" colspan="3"><asp:CheckBox ID="chkDraft" runat="server" Text="Draft"></asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkSubmit" runat="server" Text="Submitted"></asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkPending" runat="server" Text="Pending Approval"></asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkApproved" runat="server" Text="Approved"></asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:CheckBox ID="chkRejected" runat="server" Text="Rejected"></asp:CheckBox></td>
					<td class="tablecol"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
						<input class="button" id="cmdSelectAll" onclick="SelectAll_1();" type="button" value="Select All"
							name="cmdSelectAll"/>&nbsp; <input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="5" style="height: 18px"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" ShowSummary="False" ShowMessageBox="True"
							DisplayMode="List"></asp:validationsummary><asp:comparevalidator id="vldDateFtDateTo" runat="server" Type="Date" Operator="GreaterThanEqual" Display="None"
							ControlToValidate="txtDateTo" ControlToCompare="txtDateFr" ErrorMessage="End Date should be >= Start date"></asp:comparevalidator><asp:customvalidator id="vldDateFr" runat="server" Display="None" ControlToValidate="txtDateFr" ErrorMessage="Date To cannot be empty"
							Enabled="False" ClientValidationFunction="checkDateTo"></asp:customvalidator><asp:customvalidator id="vldDateTo" runat="server" Display="None" ControlToValidate="txtDateTo" ErrorMessage="Date From cannot be empty"
							Enabled="False" ClientValidationFunction="checkDateFr"></asp:customvalidator></td>
				</tr>			
			</table>
			<table class="AllTable" id="tblSearchResult" width="100%" cellspacing="0" cellpadding="0" border="0" runat="server">
				<tr>
					<td class="emptycol" colspan="5">
					    <asp:datagrid id="dtgScList" runat="server" OnSortCommand="SortCommand_Click" CssClass="grid">
							<Columns>
								<asp:TemplateColumn SortExpression="SCM_CLAIM_DOC_NO" HeaderText="SC Number">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle VerticalAlign="Middle"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkSCNo"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="SCM_CREATED_DATE" SortExpression="SCM_CREATED_DATE" HeaderStyle-HorizontalAlign="Left" HeaderText="Document Date">
									<HeaderStyle Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SCM_SUBMIT_DATE" SortExpression="SCM_SUBMIT_DATE" HeaderStyle-HorizontalAlign="Left" HeaderText="Submitted Date">
									<HeaderStyle Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SCM_STAFF_ID" SortExpression="SCM_STAFF_ID" HeaderText="Staff ID">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="STAFF_NAME" SortExpression="STAFF_NAME" HeaderText="Staff Name">
									<HeaderStyle Width="18%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Department">
									<HeaderStyle Width="18%"></HeaderStyle>
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
						</asp:datagrid>
					</td>
				</tr>
			</table>				
		</form>
	</body>
</HTML>
