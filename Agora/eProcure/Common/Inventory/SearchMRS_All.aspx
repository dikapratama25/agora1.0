<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchMRS_All.aspx.vb" Inherits="eProcure.SearchMRS_All" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>SearchMRS_All</title>
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
			SelectAllG("dtgMRSList_ctl02_chkAll","chkSelection");
		}
		//for filtering check box
		function SelectAll_1()
		{
			checkStatus(true);
		}
		
		function checkStatus(checked)
		{
			var oform = document.forms(0);
			oform.chkIssued.checked=checked;
			oform.chkReject.checked=checked;
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtgIRList_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);
			oform.txtMRSNo.value="";
			oform.txtAccCode.value="";
			oform.txtDateFr.value=oform.hidDateS.value;
            oform.txtDateTo.value=oform.hidDateE.value;
            oform.txtIssueDateFr.value=oform.hidIssueDateS.value;
            oform.txtIssueDateTo.value=oform.hidIssueDateE.value;
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
	<body onload="document.forms[0].txtMRSNo.focus();">
		<form id="Form1" method="post" runat="server">
 		<%  Response.Write(Session("w_SearchMRSAll_tabs"))%>
              <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
              <tr>
					<td class="linespacing1" colspan="7"></td>
			    </tr>
				<tr>
	                <td colspan="7">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant approved or rejected IR."
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
					<td class="tablecol" width="23%" >&nbsp;<strong><asp:Label ID="Label11" runat="server" Text="MRS No. :"></asp:Label></strong></td>
					<td class="Tableinput" width="20%">
						<asp:textbox id="txtMRSNo" runat="server" CssClass="txtbox"></asp:textbox></td>
				    <td class="tablecol" width="12%"></td>
					<td class="tablecol" width="20%" ><strong><asp:Label ID="Label1" runat="server" Text="Account Code :"></asp:Label></strong></td>
					<td class="Tableinput" width="34%" colspan="2"><asp:textbox id="txtAccCode" runat="server" CssClass="txtbox"></asp:textbox></td>
					<td class="tablecol" width="15%"></td>
				</tr>
				<tr>
					<td class="tableCOL" align="left" >&nbsp;<strong><asp:Label ID="Label14" runat="server" Text="Start Date :"></asp:Label></strong></td>
					<td	class="Tableinput"><asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateFr');" href="javascript:;"><%Response.Write(CalPicture) %></a><input id="hidDateS" style="width: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidDateS"
							runat="server"/><asp:requiredfieldvalidator id="rfv_txtDateFr" runat="server" ErrorMessage="Start Date is required."
								ControlToValidate="txtDateFr"></asp:requiredfieldvalidator></td>
					<td class="tablecol" style="width: 1914px"></td> 
					<td class="tableCOL" align="left" ><strong><asp:Label ID="Label15" runat="server" Text="End Date :"></asp:Label></strong></td>
					<td	class="Tableinput"><asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateTo');" href="javascript:;"><%Response.Write(CalPicture) %></a>
					<input id="hidDateE" style="width: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidDateE"
							runat="server"/><asp:requiredfieldvalidator id="rfv_txtDateTo" runat="server" ErrorMessage="End Date is required."
								ControlToValidate="txtDateTo"></asp:requiredfieldvalidator></td>
					<td class="tablecol" colspan="2"></td>
				</tr>
				<tr>
					<td class="tableCOL" align="left" >&nbsp;<strong><asp:Label ID="Label2" runat="server" Text="Issued Start Date :"></asp:Label></strong></td>
					<td	class="Tableinput"><asp:textbox id="txtIssueDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtIssueDateFr');" href="javascript:;"><%Response.Write(CalPicture) %></a><input id="hidIssueDateS" style="width: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidDateS"
							runat="server"/><asp:requiredfieldvalidator id="rfv_txtIssueDateFr" runat="server" ErrorMessage="Issued Start Date is required."
								ControlToValidate="txtIssueDateFr"></asp:requiredfieldvalidator></td>
					<td class="tablecol" style="width: 1914px"></td> 
					<td class="tableCOL" align="left" ><strong><asp:Label ID="Label3" runat="server" Text="Issued End Date :"></asp:Label></strong></td>
					<td	class="Tableinput"><asp:textbox id="txtIssueDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtIssueDateTo');" href="javascript:;"><%Response.Write(CalPicture) %></a>
					<input id="hidIssueDateE" style="width: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidIssueDateE"
							runat="server"/><asp:requiredfieldvalidator id="rfv_txtIssueDateTo" runat="server" ErrorMessage="Issued End Date is required."
								ControlToValidate="txtIssueDateTo"></asp:requiredfieldvalidator></td>
					<td class="tablecol" colspan="2"></td>
				</tr>
				<tr>
					<td class="tableCOL" >&nbsp;<strong>My Approval Status :</strong></td>
					<td class="Tableinput" colspan="2"><asp:checkbox id="chkIssued" Text="Issued" Runat="server"></asp:checkbox>
					    &nbsp;&nbsp;&nbsp;&nbsp;<asp:checkbox id="chkReject" Text="Rejected" Runat="server"></asp:checkbox></td>
				    <td class="tablecol" colspan="5" align="right"></td>
				</tr>
				<tr>
					<td class="tableCOL" >&nbsp;<strong>Include Rejected MRS? :</strong></td>
					<td class="Tableinput" colspan="2"><asp:checkbox id="chkIncludeRejMRS" Text="(Tick to include)" Runat="server" Checked="true"></asp:checkbox></td>
				    <td class="tablecol" colspan="5" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
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
							Enabled="False" ClientValidationFunction="checkDateFr"></asp:customvalidator><asp:comparevalidator id="vldIDateFtIDateTo" runat="server" Type="Date" Operator="GreaterThanEqual" Display="None"
							ControlToValidate="txtIssueDateTo" ControlToCompare="txtIssueDateFr" ErrorMessage="Issued End Date must greater than or equal to Issued Start Date"></asp:comparevalidator></td>
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
								<td colspan="5"><asp:datagrid id="dtgMRSList" runat="server" OnSortCommand="SortCommand_Click">
										<Columns>
											<asp:TemplateColumn SortExpression="IRSM_IRS_NO" HeaderText="MRS Number">
												<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
												<ItemStyle VerticalAlign="Middle"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkMRSNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="IRSM_IRS_DATE" SortExpression="IRSM_IRS_DATE" HeaderStyle-HorizontalAlign="Left" HeaderText="MRS Date">
												<HeaderStyle Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="IRSM_IRS_APPROVED_DATE" SortExpression="IRSM_IRS_APPROVED_DATE" HeaderStyle-HorizontalAlign="Left" HeaderText="Issued Date">
												<HeaderStyle Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="IRSM_IRS_ACCOUNT_CODE" SortExpression="IRSM_IRS_ACCOUNT_CODE" HeaderText="Account Code">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="IRSM_IRS_ISSUE_TO" SortExpression="IRSM_IRS_ISSUE_TO" HeaderText="Issue To">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Department">
												<HeaderStyle Width="15%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="IRSM_IRS_REF_NO" SortExpression="IRSM_IRS_REF_NO" HeaderText="Reference No">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="IRSM_IRS_REMARK" SortExpression="IRSM_IRS_REMARK" HeaderText="Remark">
												<HeaderStyle Width="15%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status">
												<HeaderStyle Width="10%"></HeaderStyle>
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
