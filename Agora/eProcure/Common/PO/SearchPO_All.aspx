<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchPO_All.aspx.vb" Inherits="eProcure.SearchPO_All" %>
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
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
            dim CalPicture as string = "<IMG src="& dDispatcher.direct("Plugins/images","i_Calendar2.gif") &" border=""0""></A>"
        </script> 
		
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
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
			oform.chkInclude.checked=true;
			oform.chkIncludeHold.checked=true;
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtgPOList_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);
			oform.txtPONo.value="";
			oform.txtVendor.value="";
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
	</HEAD>
	<body onload="document.forms[0].txtPONo.focus();" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
 		<%  Response.Write(Session("w_SearchPOAll_tabs"))%>
              <TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
              <tr>
					<TD class="linespacing1" colSpan="7"></TD>
			    </TR>
				<TR>
	                <TD colSpan="7">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant approved or rejected PO."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="7"></TD>
			    </TR>
				<tr>
								<td class="tableheader" align="left" colSpan="7" style="height: 19px">&nbsp;Search Criteria</td>
							</tr>
							<TR>
								<TD class="tablecol" width="23%" >&nbsp;<STRONG><asp:Label ID="Label11" runat="server" Text="PO No. :"></asp:Label></STRONG></TD>
								<td class="TableInput" width="20%">
									<asp:textbox id="txtPONo" runat="server" CssClass="txtbox"></asp:textbox></TD>
								<td class="tablecol" width="12%"></td>
								<TD class="tablecol" width="20%" >
								    <asp:Label id="lblVendor" runat="server" Font-Bold="True">Vendor Name :</asp:Label></TD>
								<td class="TableInput" width="34%" colspan="2">   
									<asp:textbox id="txtVendor" width="100%" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="15%"></td>
							</TR>
							<tr>
								<td class="tableCOL" align="left" >&nbsp;<STRONG><asp:Label ID="Label14" runat="server" Text="Start Date :"></asp:Label></STRONG></td>
								<TD	 class="TableInput"><asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><A onclick="popCalendar('txtDateFr');" href="javascript:;"><%Response.Write(CalPicture) %></td>
								<TD class="tablecol" style="width: 1914px">
								<TD class="tableCOL" align="left" ><STRONG><asp:Label ID="Label15" runat="server" Text="End Date :"></asp:Label></STRONG></td>
								<TD	 class="TableInput"><asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><A onclick="popCalendar('txtDateTo');" href="javascript:;"><%Response.Write(CalPicture) %></A>
								<TD class="tablecol" colspan="2">
							</tr>
							<TR width="100%">
											<td class="tableCOL" >&nbsp;<STRONG>My Approval Status :</STRONG></td>
											<td class="TableInput" colspan="2"><asp:checkbox id="chkApproved" Text="Approved" Runat="server"></asp:checkbox>
											    &nbsp;&nbsp;&nbsp;&nbsp;<asp:checkbox id="chkReject" Text="Rejected" Runat="server"></asp:checkbox></td>
								        <TD class="tablecol" colspan="5"></td>
										</tr>
										
							<TR width="100%">
											<td class="tableCOL" >&nbsp;<STRONG>Include Rejected PO ? :</STRONG></td>
											<td class="TableInput"><asp:checkbox id="chkInclude" Text="(Tick to include)" Runat="server" Checked="True"></asp:checkbox></td>
								        <TD class="tablecol" style="width: 1914px">
								        <td class="tableCOL" >&nbsp;<STRONG>Include PO On Hold ? :</STRONG></td>
											<td class="TableInput"><asp:checkbox id="chkIncludeHold" Text="(Tick to include)" Runat="server" Checked="True"></asp:checkbox></td>
											<td class="tableCOL" colSpan="3"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
												<INPUT class="button" id="cmdSelectAll" onclick="SelectAll_1();" type="button" value="Select All"
													name="cmdSelectAll">&nbsp; <INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;</td>
										</tr>
								        </Table>
				<table>
				<tr>
					<TD colSpan="3"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" ShowSummary="False" ShowMessageBox="True"
							DisplayMode="List"></asp:validationsummary><asp:comparevalidator id="vldDateFtDateTo" runat="server" Type="Date" Operator="GreaterThanEqual" Display="None"
							ControlToValidate="txtDateTo" ControlToCompare="txtDateFr" ErrorMessage="End Date must greater than or equal to Start Date"></asp:comparevalidator><asp:customvalidator id="vldDateFr" runat="server" Display="None" ControlToValidate="txtDateFr" ErrorMessage="Date To cannot be empty"
							Enabled="False" ClientValidationFunction="checkDateTo"></asp:customvalidator><asp:customvalidator id="vldDateTo" runat="server" Display="None" ControlToValidate="txtDateTo" ErrorMessage="Date From cannot be empty"
							Enabled="False" ClientValidationFunction="checkDateFr"></asp:customvalidator></TD>
				</tr>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				</Table>
				<TABLE class="AllTable" id="tblSearchResult" width="100%" cellSpacing="0" cellPadding="0"
							border="0" runat="server">
							<%--<TR>
								<TD class="emptycol" colSpan="5"></TD>
							</TR>--%>
							<TR>
								<TD colSpan="5"><asp:datagrid id="dtgPOList" runat="server" OnSortCommand="SortCommand_Click">
										<Columns>
											<asp:TemplateColumn SortExpression="POM_PO_No" HeaderText="PO Number">
												<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
												<ItemStyle VerticalAlign="Middle"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkPONo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="POM_CREATED_Date" SortExpression="POM_CREATED_Date" HeaderStyle-HorizontalAlign=Left HeaderText="Creation Date">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" HeaderText="Purchaser">
												<HeaderStyle Width="20%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="POM_S_COY_NAME" SortExpression="POM_S_COY_NAME" HeaderText="Vendor Name">
												<HeaderStyle Width="23%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="POM_CURRENCY_CODE" SortExpression="POM_CURRENCY_CODE" HeaderText="Currency">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PO_AMT" SortExpression="PO_AMT"  HeaderText="Amount" HeaderStyle-HorizontalAlign=Right>
												<HeaderStyle Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="STAT" SortExpression="STAT" HeaderText="Status">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="POM_PO_No" HeaderText="PO Number"></asp:BoundColumn>
											<asp:BoundColumn HeaderText="PR No."> 
									            <HeaderStyle Width="25%" HorizontalAlign="Left"></HeaderStyle> 
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left"/>
								            </asp:BoundColumn>
										</Columns>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="5"></TD>
							</TR>
							<TR>
								<TD class="emptycol" colSpan="5">&nbsp;&nbsp;</TD>
							</TR>
						</TABLE>
		</form>
	</body>
</HTML>
