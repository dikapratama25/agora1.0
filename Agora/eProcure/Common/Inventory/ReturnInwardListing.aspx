<%--Copyright © 2011 STRATEQ GLOBAL SERVICES. All rights reserved.--%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReturnInwardListing.aspx.vb" Inherits="eProcure.ReturnInwardListing" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ReturnInwardListing</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim StartCalendar As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_startdate") & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim EndCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_enddate") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            
            Dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
        </script>
        <% Response.Write(Session("WheelScript"))%>
	    <% Response.Write(Session("JQuery")) %>
        <% Response.Write(Session("AutoComplete")) %>
		<script type="text/javascript">
		
		//For check in Datagrid
		function selectAll()
		{
			SelectAllG("dtg_POList_ctl02_chkAll","chkSelection");
		}
		//for filtering check box
		function SelectAll_1()
		{
			checkStatus(true);
		}
			
		function checkChild(id)
		{
			checkChildG(id,"dtg_POList_ctl02_chkAll","chkSelection");
		}
		
	    function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","help:No;resizable: No;Status:No;dialogHeight: 255px; dialogWidth: 300px");
			return false;
		}
		
		function popCalendar(val)
		{
			txtVal= document.getElementById(val);
			window.open('<% Response.Write(PopCalendar)%>','cal','status=no,resizable=no,width=200,height=180,left=270,top=180');						
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
		
		</script>
	</HEAD>
	<body class="body" runat="server">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_ReturnInward_tabs"))%>
               <TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<tr>
					<TD class="linespacing1" colSpan="6"></TD>
			    </TR>
				<TR>
	                <TD colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Fill in the search criteria and click Search button to list the relevant MRS. Click the MRS No. to go to raise Return Inward page."></asp:label>
	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="6"></TD>
			    </TR>
				<tr>
				<td class="tableheader" width="100%" align="left" colSpan="6" style="height: 19px">&nbsp;Search Criteria</td>
				</tr>
				<TR >
					<TD class="tableCOL" width="15%" >&nbsp;<STRONG>MRS Number</STRONG> :</td>
					<td class="tableCOL" width= "25%" ><asp:textbox id="txtMRSNo" runat="server" CssClass="TXTBOX"></asp:textbox></TD>
					<td class="tableCOL" width= "5%" ></td>
					<td class="tableCOL" width= "15%" >	<STRONG>Issue To&nbsp;</STRONG>: </td>
					<td class="tableCOL" width= "25%" ><asp:textbox id="txtIssueTo" runat="server" width="200px" CssClass="TXTBOX"></asp:textbox></td>
					<td class="tableCOL" width= "15%"></td>
				</TR>
				<TR >
					<TD class="tableCOL" width="15%" >&nbsp;<STRONG>Department</STRONG> :</td>
					<td class="tableCOL" width= "25%" ><asp:textbox id="txtDepartment" runat="server" CssClass="TXTBOX"></asp:textbox></TD>
					<td class="tableCOL" width= "5%" ></td>
					<td class="tableCOL" width= "15%" >	<STRONG>Account Code&nbsp;</STRONG>: </td>
					<td class="tableCOL" width= "25%" ><asp:textbox id="txtAccCode" runat="server" width="200px" CssClass="TXTBOX"></asp:textbox></td>
					<td class="tableCOL" width= "15%"></td>
				</TR>
				<tr>
					<td class="tableCOL" width="15%" style="height: 24px"><STRONG>&nbsp;Start Date</STRONG>&nbsp;: </td>
					<td class="tableCOL" width= "25%" style="height: 24px">
					    <asp:textbox id="txt_startdate" runat="server" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(StartCalendar)%><asp:requiredfieldvalidator id="rfv_txt_startdate" runat="server" ErrorMessage="Start Date is required."
								ControlToValidate="txt_startdate"></asp:requiredfieldvalidator></td>
					<td class="tableCOL" width= "5%" style="height: 24px"></td>
					<td class="tableCOL" width= "15%" style="height: 24px"><STRONG>End Date</STRONG>&nbsp;:</td>
					<td class="tableCOL" width= "25%" style="height: 24px"><asp:textbox id="txt_enddate" runat="server" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(EndCalendar)%><asp:requiredfieldvalidator id="rfv_txt_enddate" runat="server" ErrorMessage="End Date is required."
								ControlToValidate="txt_enddate"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="CompareValidator1" runat="server" Display="None" Operator="GreaterThanEqual"
							Type="Date" ControlToValidate="txt_enddate" ControlToCompare="txt_startdate" ErrorMessage="End Date must greater than or equal to Start Date">*</asp:comparevalidator>&nbsp;</TD>
				<td class="tableCOL" width= "15%" align="right" style="height: 24px">
				    <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
				    <asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear"></asp:button>
					</TD>
				</TR>
			</TABLE>
			<TABLE class="AllTable" id="tblSearchResult" width="100%" cellSpacing="0" cellPadding="0"
				border="0" runat="server">
				<TR>
					<TD class="emptycol" colSpan="5" style="width: 703px; height: 18px;"></TD>
				</TR>
				<TR>
					<td colspan="5" style="width: 100%;">
					    <asp:datagrid id="dtg_RIList" runat="server" DataKeyField="IRSM_IRS_NO" OnSortCommand="SortCommand_Click"
							AutoGenerateColumns="False" OnPageIndexChanged="dtg_RIList_Page">
							<Columns>
								<asp:TemplateColumn SortExpression="IRSM_IRS_NO" HeaderText="MRS Number">
									<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle VerticalAlign="Middle"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkMRSNo"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="IRSM_IRS_DATE" SortExpression="IRSM_IRS_DATE" HeaderText="MRS Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRSM_IRS_ACCOUNT_CODE" SortExpression="IRSM_IRS_ACCOUNT_CODE" HeaderText="Account Code">
									<HeaderStyle Width="14%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRSM_IRS_ISSUE_TO" SortExpression="IRSM_IRS_ISSUE_TO" HeaderText="Issue To">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Department">
									<HeaderStyle Width="13%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRSM_IRS_REF_NO" SortExpression="IRSM_IRS_REF_NO" HeaderText="Reference No">
									<HeaderStyle Width="13%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IRSM_IRS_REMARK" SortExpression="IRSM_IRS_REMARK" HeaderText="Remark">
									<HeaderStyle Width="13%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
                        <asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List"
                            ShowMessageBox="True" ShowSummary="False" Width="22%" />
                        </TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
