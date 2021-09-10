<%@ Page Language="vb" AutoEventWireup="false" Codebehind="POViewB2Cancel.aspx.vb" Inherits="eProcure.POViewB2CancelFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>POViewB2</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim StartCalendar As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_startdate") & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim EndCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_enddate") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            
           ' Dim sOpen As String = dDispatcher.direct("Initial", "popCalendar.aspx", "textbox=" + val + "&seldate=" + txtVal.value)
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
        </script>
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
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
		
		function Reset(){
			var oform = document.forms(0);
			oform.txt_po_no.value=""
			oform.txt_vendor.value=""
			oform.txt_startdate.value=""
			oform.txt_enddate.value=""
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
		
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_POViewBuyerCancel_tabs"))%>
               <TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<tr>
					<TD class="linespacing1" colSpan="6"></TD>
			    </TR>
				<TR>
	                <TD colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant PO. Click the PO No. to go to PO Cancellation page."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="6"></TD>
			    </TR>
				<tr>
								<td class="tableheader" width="100%" align="left" colSpan="6" style="height: 19px">&nbsp;Search Criteria</td>
							</tr>
							<TR >
								<TD class="tableCOL" width="15%" >&nbsp;<STRONG>PO No.</STRONG> :</td>
								<td class="tableCOL" width= "25%" ><asp:textbox id="txt_po_no" runat="server" CssClass="TXTBOX"></asp:textbox></TD>
								<td class="tableCOL" width= "5%" ></td>
								<td class="tableCOL" width= "15%" >	<STRONG>Vendor Name&nbsp;</STRONG>: </td>
								<td class="tableCOL" width= "25%" ><asp:textbox id="txt_vendor" runat="server" width="200px" CssClass="TXTBOX"></asp:textbox></td>
								<td class="tableCOL" width= "15%"></td>
							</TR>
							<tr>
								<td class="tableCOL" width="15%"><STRONG>&nbsp;Start Date</STRONG>&nbsp;: </td>
								<td class="tableCOL" width= "25%">
								    <asp:textbox id="txt_startdate" runat="server" CssClass="TXTBOX" contentEditable="false" ></asp:textbox><% Response.Write(StartCalendar)%></td>
								<td class="tableCOL" width= "5%"></td>
								<td class="tableCOL" width= "15%"><STRONG>End Date</STRONG>&nbsp;:</td>
								<td class="tableCOL" width= "25%"><asp:textbox id="txt_enddate" runat="server" CssClass="TXTBOX" contentEditable="false" ></asp:textbox><% Response.Write(EndCalendar)%>
									<asp:comparevalidator id="CompareValidator1" runat="server" Display="None" Operator="GreaterThanEqual"
										Type="Date" ControlToValidate="txt_enddate" ControlToCompare="txt_startdate" ErrorMessage="End Date must greater than or equal to Start Date">*</asp:comparevalidator>&nbsp;</TD>
							<td class="tableCOL" width= "15%" align="right">
							    <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
							    <INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">
								</TD>
							</TR>
						</TABLE>
						<TABLE class="AllTable" id="tblSearchResult" width="100%" cellSpacing="0" cellPadding="0"
							border="0" runat="server">
							<TR>
								<TD class="emptycol" colSpan="5" style="width: 703px"></TD>
							</TR>
							<TR>
								<td colspan="5" style="width: 100%;">
								    <asp:datagrid id="dtg_POList" runat="server" DataKeyField="POM_PO_NO" OnSortCommand="SortCommand_Click"
										AutoGenerateColumns="False" OnPageIndexChanged="dtg_POList_Page">
										<Columns>
											<asp:TemplateColumn SortExpression="POM_PO_No" HeaderText="PO No.">
												<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
												<ItemStyle VerticalAlign="Middle"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkPONo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn SortExpression="POM_PO_DATE" HeaderText="PO Date">
												<HeaderStyle Width="9%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="POM_S_COY_NAME" SortExpression="POM_S_COY_NAME" HeaderText="Vendor Name">
												<HeaderStyle Width="23%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn SortExpression="POM_ACCEPTED_DATE" HeaderText="PO Accepted Date">
												<HeaderStyle Width="14%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="STATUS_DESC" HeaderText="PO Status">
												<HeaderStyle Width="20%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Remark1" HeaderText="Fulfilment Status">
												<HeaderStyle Width="13%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="POM_PO_INDEX"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="POM_S_COY_ID"></asp:BoundColumn>
											<asp:BoundColumn HeaderText="PR No.">
									            <HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left"/>
								            </asp:BoundColumn>
								            <%--<asp:TemplateColumn SortExpression="PR_No" HeaderText="PR No.">
												<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
												<ItemStyle VerticalAlign="Middle"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>--%>
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
