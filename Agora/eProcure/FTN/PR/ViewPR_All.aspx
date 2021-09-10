<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ViewPR_All.aspx.vb" Inherits="eProcure.ViewPR_AllFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>View All PR</title>
<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim StartCalendar As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr") & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim EndCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            'Dim dDispatcher As New AgoraLegacy.dispatcher
             Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            'Dim sOpen As String = dDispatcher.direct("Initial", "popCalendar.aspx", "textbox="" + val + ""&seldate="" + txtVal.value")
        </script>
        <% Response.Write(css)%>
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
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
			oform.chkOpen.checked=checked;
			oform.chkSubmitted.checked=checked;
			oform.chkConToPO.checked=checked;
			oform.chkApproved.checked=checked;
////			oform.chkPOCreate.checked=checked;
			oform.chkCancel.checked=checked;
			oform.chkVoid.checked=checked;
			oform.chkReject.checked=checked;
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtgPRList_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);
			oform.txtPRNo.value="";
			oform.txtBuyer.value="";
			oform.txtDept.value="";
//			oform.txtVendor.value="";
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
			//window.open('../popCalendar.aspx?textbox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
			window.open('../Calendar/viewCalendar.aspx?TextBox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
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
	</HEAD>
	<body onload="document.forms[0].txtPRNo.focus();" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
			
				<TR>
					<TD class="header" style="height: 19px; padding :0px;">&nbsp;<asp:label id="lblTitle" runat="server"></asp:label></TD>
				</TR>
				
				<tr style="height: 19px;">
	                <td class="emptycol" colSpan="7">
		                &nbsp;<asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Fill in the search criteria and click Search button to list the relevant PR."></asp:label>
	                </td>
                </tr>
                
				<%--<TR  class="header" style="height: 19px;">
					<TD class="tablecol">--%>
							<TR>
								<TD class="tableheader" colspan="7">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol" style="WIDTH: 147px">&nbsp;<STRONG>PR No.</STRONG>&nbsp;:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									
								</TD>
								<td class="tablecol" style="width: 181px"><asp:textbox id="txtPRNo" runat="server" CssClass="txtbox" Width="150px"></asp:textbox></td>
								<%--<TD class="tablecol"  style="WIDTH: 302px">
									&nbsp;<asp:Label id="lblVendor" runat="server" Font-Bold="True" Width="107px">Vendor Name :</asp:Label>									
								</TD>--%>
								<td class="tablecol" colspan ="1"></td>
								<%--<td class="tablecol">
								<asp:textbox id="txtVendor" runat="server" CssClass="txtbox" Width="150px"></asp:textbox>&nbsp;</td>--%>
									<TD class="tablecol" style="width: 165px" align=right>
									&nbsp;<asp:Label id="lblPRType" runat="server" Font-Bold="True" Width="150px"  >PR Type :</asp:Label>
									<td class="tablecol" style="width: 16%"><asp:checkbox id="chkContPR" Text="Contract PR" Runat="server" Width ="100px"></asp:checkbox></td>									
								<td class="tablecol"  colspan ="2"></td>
							</TR>
							<tr >
								<TD class="tablecol" style="height: 19px; width: 147px;">
									&nbsp;<asp:Label Runat="server" ID="lblStartDate" Font-Bold="True">Start Date :</asp:Label>
									
								</TD>
								<td class="tablecol" colspan="" rowspan="" style="width: 181px"><asp:textbox id="txtDateFr" runat="server" CssClass="txtbox" Width="110px" contentEditable="false" ></asp:textbox><% Response.Write(StartCalendar)%></td>
								<td class="tablecol" style="width: 166px">
									&nbsp;<asp:Label Runat="server" ID="lblEndDate" Font-Bold="True" Width="75px">End Date :</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									
								</td>
								<TD class="tablecol"><asp:textbox id="txtDateTo" runat="server" CssClass="txtbox" Width="110px" contentEditable="false" ></asp:textbox><% Response.Write(EndCalendar)%></td>
								<td class="tablecol" ><asp:checkbox id="chkNonContPR" Text="Non-Contract PR" Runat="server" cssclass ="chk" Width="135px"></asp:checkbox></td>
								<td class="tablecol" colspan ="2"></td>
							</tr>
							<TR id="trAdmin" runat="server">
								<TD class="tablecol" style="width: 147px" >&nbsp;<STRONG>Requisitioner</STRONG> :
									
								</TD>
								<td class="tablecol" style="width: 181px"><asp:textbox id="txtBuyer" runat="server" CssClass="txtbox" Width="169px"></asp:textbox>&nbsp;</td>
								<TD class="tablecol" style="width: 166px" >&nbsp;<STRONG>Dept.</STRONG> :
									
								</TD>
								<td class="tablecol"><asp:textbox id="txtDept" runat="server" CssClass="txtbox" Width="162px"></asp:textbox>&nbsp;</td>
								<td class="tablecol" colspan="3"></td>
							</TR>
							
							<%--<TR>--%>
							    <td class="tablecol" valign ="top" style="width: 10px" >&nbsp;<STRONG>Status</STRONG> :</td>
								<td class="tablecol" colspan="6">
									<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" border="0" runat="server" width="100%">
										<tr class="tablecol">
											<td class="tablecol" style="width: 21%; height: 20px;" ><asp:checkbox id="chkOpen" Text="Draft" Runat="server"></asp:checkbox></td>
											<td class="tablecol" style="width: 21%; height: 20px"><asp:checkbox id="chkSubmitted" Text="Submitted" Runat="server"></asp:checkbox></td>
											<%--<td width="15%"><asp:checkbox id="chkPendingAppr" Text="Pending Approval" Runat="server"></asp:checkbox></td>--%>							
											<td class="tablecol" style="width: 21%; height: 20px"><asp:checkbox id="chkApproved" Text="Approved" Runat="server"></asp:checkbox></td>
											<td class="tablecol" style="height: 14px; width: 21%;"><asp:checkbox id="chkConToPO" Text="Converted To PO"  TextAlign=Right Runat="server" Width="147px"></asp:checkbox></td>
											    <td class="tablecol" style="width: 18%; height: 10px;"></td>
										    <td class="tablecol" style="width: 18%; height: 10px;"></td>
									</tr>
										<tr class="tablecol">
											<%--<td width="15%"><asp:checkbox id="chkPOCreate" Text="Converted To PO" Runat="server"></asp:checkbox></td>--%>
											<td class="tablecol" ><asp:checkbox id="chkVoid" Text="Void " Runat="server"></asp:checkbox></td>
											<td class="tablecol" ><asp:checkbox id="chkCancel" Text="Cancelled" Runat="server"></asp:checkbox></td>											
											<td class="tablecol" ><asp:checkbox id="chkReject" Text="Rejected" Runat="server"></asp:checkbox></td>
										    <td class="tablecol" ></td>
										    <td class="tablecol" ></td>
										    <td class="tablecol" ></td>
										</tr>
										
									</TABLE>
								</td>
							<%--</TR>--%>
							<tr class="tablecol">
								<td class="tablecol" colSpan="7" style="height: 24px" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
									<input class="button" id="cmdSelectAll" onclick="SelectAll_1();" type="button" value="Select All"
										name="cmdSelectAll"/>&nbsp; <input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/>&nbsp;</td>
							</tr>
				<%--	</TD>
				</TR>--%>
				<tr>
					<TD class="emptycol" colSpan="3" style="height: 18px"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" ShowSummary="False" ShowMessageBox="True"
							DisplayMode="List"></asp:validationsummary><asp:comparevalidator id="vldDateFtDateTo" runat="server" Type="Date" Operator="GreaterThanEqual" Display="None"
							ControlToValidate="txtDateTo" ControlToCompare="txtDateFr" ErrorMessage="End Date should be >= Start date"></asp:comparevalidator><asp:customvalidator id="vldDateFr" runat="server" Display="None" ControlToValidate="txtDateFr" ErrorMessage="Date To cannot be empty"
							Enabled="False" ClientValidationFunction="checkDateTo"></asp:customvalidator><asp:customvalidator id="vldDateTo" runat="server" Display="None" ControlToValidate="txtDateTo" ErrorMessage="Date From cannot be empty"
							Enabled="False" ClientValidationFunction="checkDateFr"></asp:customvalidator></TD>
				</tr>			
				</TABLE>
				<TABLE class="AllTable" id="tblSearchResult" width="100%" cellSpacing="0" cellPadding="0"
							border="0" runat="server">
					<TR>
						<TD class="emptycol" colSpan="5">
						    <asp:datagrid id="dtgPRList" runat="server" OnSortCommand="SortCommand_Click" CssClass="grid">
								<Columns>
									<asp:TemplateColumn SortExpression="PRM_PR_No" HeaderText="PR Number">
										<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
										<ItemStyle VerticalAlign="Middle"></ItemStyle>
										<ItemTemplate>
											<asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="PRM_SUBMIT_DATE" SortExpression="PRM_SUBMIT_DATE" HeaderText="Submission Date">
										<HeaderStyle Width="8%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" HeaderText="Requisitioner">
										<HeaderStyle Width="14%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Department">
										<HeaderStyle Width="12%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PRM_PR_TYPE" SortExpression="PRM_PR_TYPE" HeaderText="PR Type">
										<HeaderStyle Width="8%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status">
										<HeaderStyle Width="13%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="PRM_PR_No" HeaderText="PR Number"></asp:BoundColumn>
								</Columns>
							</asp:datagrid></TD>
					</TR>
					<TR>
						<TD class="emptycol" colSpan="5" style="width: 965px"></TD>
					</TR>
					<TR>
						<TD class="emptycol" colSpan="5" style="width: 965px">&nbsp;&nbsp;</TD>
					</TR>
				</TABLE>				
		</form>
	</body>
</HTML>
