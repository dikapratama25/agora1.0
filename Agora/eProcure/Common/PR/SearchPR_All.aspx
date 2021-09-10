<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchPR_All.aspx.vb" Inherits="eProcure.SearchPR_All" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PR Approval</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim StartCalendar As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr") & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim EndCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            
            Dim sOpen As String = dDispatcher.direct("Initial", "popCalendar.aspx", "textbox="" + val + ""&seldate="" + txtVal.value")
        </script>
        
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		//For check in Datagrid
		function selectAll()
		{
			SelectAllG("dtgPRList_ctl02_chkAll","chkSelection");
		}
		//for filtering check box
		function CheckAll()
		{
			checkStatus(true);
		}
		
		function checkStatus(checked)
		{
			var oform = document.forms(0);
			oform.chkOpen.checked=checked;
            oform.chkSubmitted.checked=checked;
            oform.chkApproved.checked=checked;
            oform.chkConverted.checked=checked;
            oform.chkVoid.checked=checked;
            oform.chkCancel.checked=checked;
            oform.chkReject.checked=checked;
            oform.chkSource.checked=checked;
            oform.chkHold.checked=checked;
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtgPRList_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);			
			oform.txtPRNo.value="";
            oform.txtItemCode.value="";
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
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchPR_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<tr>
					<td class="linespacing1" colSpan="6"></td>
			    </tr>
				<tr>
	                <td colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Fill in the search criteria and click Search button to list the relevant PR."></asp:label>
	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colSpan="7"></td>
			    </tr>
				<tr>
					<td class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
							<tr>
								<td class="tableheader" colspan="7">&nbsp;Search Criteria</td>
							</tr>
							<tr>
								<td class="tablecol" width="15%" >&nbsp;<STRONG>PR No.</STRONG> :</td>
								<td class="tablecol" colspan="2" width= "25%" ><asp:textbox id="txtPRNo" runat="server" CssClass="TXTBOX"></asp:textbox></td>
								<td class="tablecol" width= "15%" >	<STRONG>Item Code&nbsp;</STRONG>: </td>
								<td class="tablecol" width= "25%" ><asp:textbox id="txtItemCode" runat="server" width="200px" CssClass="TXTBOX"></asp:textbox></td>
								<td class="tablecol" ></td>
								<td class="tablecol" ></td>
							</tr>
							<tr>
								<%--<td class="tablecol" valign="middle" style="WIDTH: 243px">
									&nbsp;<asp:Label Runat="server" ID="lblStartDate" Font-Bold="True">Start Date :</asp:Label>
									<asp:textbox id="txtDateFr" runat="server" CssClass="txtbox" Width="80px" contentEditable="false" ></asp:textbox><A onclick="popCalendar('txtDateFr');" href="javascript:;"><IMG src="../images/i_Calendar2.gif" border="0"></A>
								</td>--%>
								<%--<td class="tablecol" valign="middle">
									&nbsp;<asp:Label Runat="server" ID="lblEndDate" Font-Bold="True">End Date :</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
									<asp:textbox id="txtDateTo" runat="server" CssClass="txtbox" Width="80px" contentEditable="false" ></asp:textbox><A onclick="popCalendar('txtDateTo');" href="javascript:;"><IMG src="../images/i_Calendar2.gif" border="0"></A>
								</td>--%>
								<td class="tablecol"><STRONG>&nbsp;Start Date</STRONG>&nbsp;: </td>
								<td class="tablecol" colspan="2">
								    <asp:textbox id="txtDateFr" runat="server" CssClass="TXTBOX" contentEditable="false" ></asp:textbox><% Response.Write(StartCalendar)%></td>
								<td class="tablecol"><STRONG>End Date</STRONG>&nbsp;:</td>
								<td class="tablecol" colspan="2"><asp:textbox id="txtDateTo" runat="server" CssClass="TXTBOX" contentEditable="false" ></asp:textbox><% Response.Write(EndCalendar)%></td>
								<td class="tablecol" ></td>
								
							</tr>
							<%--<tr id="trAdmin" runat="server">
								<td class="tablecol" colspan="2">&nbsp;<STRONG>Requisitioner</STRONG> :
									<asp:textbox id="txtBuyer" runat="server" CssClass="txtbox" Width="200px"></asp:textbox>&nbsp;
								</td>
							</tr>--%>
							 <tr class="tablecol">
							 <TD class="tablecol" style="width: 90px">
									&nbsp;<asp:Label id="lblPRType" runat="server" Font-Bold="True" Width="80px">PR Type :</asp:Label>
									<td class="tablecol" style="width: 16%" ><asp:checkbox id="chkContPR" Text="Contract PR" Runat="server" Width ="100px"></asp:checkbox></td>									
									
							 <td class="tablecol" colspan = "5"><asp:checkbox id="chkNonContPR" Text="Non-Contract PR" Runat="server" cssclass ="chk" Width="135px"></asp:checkbox></td>
							 
							 </tr>
							
							 
							 
								
							<tr class="tablecol">
								<td width="10%">&nbsp;<STRONG>PR Status</STRONG> :</td>
								<td width="15%"><asp:checkbox id="chkOpen" Text="Draft" Runat="server"></asp:checkbox></td>
								<td width="18%"><asp:checkbox id="chkSubmitted" Text="Submitted" Runat="server"></asp:checkbox></td>
								<td width="15%"><asp:checkbox id="chkApproved" Text="Approved" Runat="server"></asp:checkbox></td>
								<td width="15%"><asp:checkbox id="chkConverted" Text="Converted To PO" Runat="server"></asp:checkbox></td>
								<td class="tablecol" ></td>
								<td class="tablecol" ></td>
							</tr>
							<tr class="tablecol">
								<td width="10%">&nbsp;</td>
								<td width="15%"><asp:checkbox id="chkVoid" Text="Void" Runat="server"></asp:checkbox></td>
								<td width="18%"><asp:checkbox id="chkCancel" Text="Cancelled" Runat="server"></asp:checkbox></td>								
								<td width="15%"><asp:checkbox id="chkReject" Text="Rejected" Runat="server"></asp:checkbox></td>
								<td width="15%" colSpan="5"><asp:checkbox id="chkSource" Text="Sourcing" Runat="server"></asp:checkbox></td>
								
							</tr>
							<tr class="tablecol">
							<td width="10%">&nbsp;</td>
							<td width="15%"><asp:checkbox id="chkHold" Text="Held" Runat="server"></asp:checkbox></td>
							<td width="18%">&nbsp;</td>
							<td width="15%">&nbsp;</td>
							<td width="15%">&nbsp;</td>
							<td colSpan="4"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
								<INPUT class="button" id="cmdSelectAll" onclick="CheckAll();" type="button" value="Select All"
									name="cmdSelectAll">&nbsp; <INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;</td>
							</tr>
								 
						 
						</TABLE>
					</td>
				</tr>				
				
				<tr>
					<td>
						<TABLE class="AllTable" id="tblSearchResult" width="100%" cellSpacing="0" cellPadding="0"
							border="0" runat="server">
							<tr>
								<td class="emptycol" colSpan="5"></td>
							</tr>
							<tr>
								<td colSpan="5">
								    <asp:datagrid id="dtgPRList" runat="server" OnSortCommand="SortCommand_Click" AutoGenerateColumns="false">
										<Columns>
											<asp:TemplateColumn SortExpression="PRM_PR_No" HeaderText="PR No.">
												<HeaderStyle HorizontalAlign="Left" Width="18%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="PRM_PR_TYPE" SortExpression="PRM_PR_TYPE" HeaderText="PR Type">
												<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRM_CREATED_DATE" SortExpression="PRM_CREATED_DATE" HeaderText="Creation Date">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRM_SUBMIT_DATE" SortExpression="PRM_SUBMIT_DATE" HeaderText="Submission Date">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRM_PR_DATE" SortExpression="PRM_PR_DATE" HeaderText="Approved Date">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>											
											<%--<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="PR Status">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>--%>
											
											<asp:BoundColumn DataField="STATUS_DESC" HeaderText="PR Status">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											
											<asp:BoundColumn HeaderText="PO Number">
									            <HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left"/>
								            </asp:BoundColumn>	
								            <%--<asp:TemplateColumn SortExpression="PO_No" HeaderText="PO Number">
												<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkPONo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>--%>
										</Columns>
									</asp:datagrid>
								</td>
							</tr>
							<tr>
							    <td colSpan="3"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" ShowSummary="False" ShowMessageBox="True"
							            DisplayMode="List"></asp:validationsummary><asp:comparevalidator id="vldDateFtDateTo" runat="server" Type="Date" Operator="GreaterThanEqual" Display="None"
							            ControlToValidate="txtDateTo" ControlToCompare="txtDateFr" ErrorMessage="End Date should be >= Start date"></asp:comparevalidator><asp:customvalidator id="vldDateFr" runat="server" Display="None" ControlToValidate="txtDateFr" ErrorMessage="Date To cannot be empty"
							            Enabled="False" ClientValidationFunction="checkDateTo"></asp:customvalidator><asp:customvalidator id="vldDateTo" runat="server" Display="None" ControlToValidate="txtDateTo" ErrorMessage="Date From cannot be empty"
							            Enabled="False" ClientValidationFunction="checkDateFr"></asp:customvalidator></td>
				            </tr>
							
						</TABLE>
					</td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
