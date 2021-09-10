<%@ Page Language="vb" AutoEventWireup="false" Codebehind="POVendorList.aspx.vb" Inherits="eProcure.POVendorList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>POViewB</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">		
		
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim calStartDate As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_start_date") & "','cal','width=190,height=165,left=270,top=180');""><IMG src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"                       
            Dim calEndDate As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_end_date") & "','cal','width=190,height=165,left=270,top=180');""><IMG src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
            Dim sOpen As String = dDispatcher.direct("Initial", "popCalendar.aspx", "textbox='+val+'&seldate= '+ txtVal.value +'")
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
		
		function checkStatus(checked)
		{
			var oform = document.forms(0);
			oform.chk_New.checked=checked;
			oform.chk_Outstdg.checked=checked;
			oform.chk_Cancel.checked=checked;
			oform.chk_close.checked=checked;
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtg_POList_ctl02_chkAll","chkSelection");
		}
		
		
		function Reset(){
			var oform = document.forms(0);					
			oform.txt_po_no.value="";
			oform.txt_start_date.value="";
			oform.txt_end_date.value="";
			oform.txt_buyer_com.value="";
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
			window.open('<% Response.Write(sOpen)%>','cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
			
			
			
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
        <%  Response.Write(Session("w_VendorPOList_tabs"))%>
               <TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				 <tr>
					<TD class="linespacing1" colSpan="6"></TD>
			    </TR>
			    <TR>
				    <TD  colSpan="6">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					        Text="Fill in the search criteria and click Search button to list the relevant PO. Click the PO Number to see the PO details."
					    ></asp:label>

				    </TD>
			    </TR>
                <tr>
					    <TD class="linespacing2" colSpan="6"></TD>
			    </TR>
				<tr>
								<td class="tableheader" align="left" colSpan="6" style="height: 19px">&nbsp;Search Criteria</td>
							</tr>
							<tr>
								<td class="tableCOL" align="left" width="15%">&nbsp;<STRONG><asp:Label ID="Label11" runat="server" Text="PO No. :"></asp:Label></STRONG></td>
								<td class="TableInput" width="20%"><asp:textbox id="txt_po_no" runat="server" CssClass="TXTBOX" MaxLength="50"></asp:textbox></td>
								<td class="TableInput" width="9%"></td>
								<td class="tablecol" align="left" width="15%"><STRONG><asp:Label ID="Label12" runat="server" Text="Buyer Company :"></asp:Label></STRONG></td>
								<td class="TableInput" width="30%"><asp:textbox id="txt_buyer_com" runat="server" CssClass="TXTBOX" MaxLength="100"></asp:textbox></td>
								<td class="TableInput" width="16%"></td>
							</tr>
							<tr>
								<td class="tableCOL" align="left" >&nbsp;<STRONG><asp:Label ID="Label14" runat="server" Text="Start Date :"></asp:Label></STRONG></td>
								<TD class="TableInput" >
								    <asp:textbox id="txt_start_date" runat="server" CssClass="TXTBOX" contentEditable="false" ></asp:textbox><% Response.Write(calStartDate)%></TD>
								<td class="TableInput" ></td>
								<td class="tableCOL" align="left" ><STRONG><asp:Label ID="Label15" runat="server" Text="End Date :"></asp:Label></STRONG></td>
								<TD class="TableInput" >
								<asp:textbox id="txt_end_date" runat="server" CssClass="TXTBOX" contentEditable="false" >
								</asp:textbox><% Response.Write(calEndDate)%></TD>
								<td class="tableCOL" align="left" ><asp:comparevalidator id="CompareValidator1" runat="server" Display="None" Operator="GreaterThanEqual"
										Type="Date" ControlToValidate="txt_end_date" ControlToCompare="txt_start_date" ErrorMessage="End Date must greater than or equal to Start Date">*</asp:comparevalidator>&nbsp;</td>
							</tr>
							<tr>
								<td class="tablecol" colSpan="6" >
									<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" border="0" runat="server" width="100%">
										<tr class="tablecol">
								        <td class="tableCOL" align="left" width="14%">&nbsp;<STRONG><asp:Label ID="Label1" runat="server" Text="PO Status :"></asp:Label></STRONG></td>
											<td  width="15%"><asp:checkbox id="chk_New" Runat="server" Text="New"></asp:checkbox></td>
											<td  width="15%"><asp:checkbox id="chk_Outstdg" Runat="server" Text="Outstanding"></asp:checkbox></td>
											<td  width="15%"><asp:checkbox id="chk_Cancel" Runat="server" Text="Cancelled"></asp:checkbox></td>
											<td width="15%"><asp:checkbox id="chk_close" Runat="server" Text="Closed"></asp:checkbox></td>
											<td colspan="2" class="tablecol" width="70%" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdSelectAll" onclick="SelectAll_1();" type="button" value="Select All"
													name="cmdSelectAll">&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"></td>
										</tr>
										<tr>
										</tr>
									</TABLE>
							</tr>
						</TABLE>
				
				<table>
				<tr>
					<td colSpan="3"><asp:validationsummary id="vldSumm" width="100%" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
							ShowSummary="False"></asp:validationsummary></td>
				</tr>
				</table>
						<TABLE class="AllTable" id="tblSearchResult" cellSpacing="0" cellPadding="0" width="100%"
							border="0" runat="server">
							<tr>
								<td class="emptycol" colSpan="5"></td>
							</tr>
							<tr>
								<td colSpan="5"><asp:datagrid id="dtg_POList" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click"
										AllowSorting="True" DataKeyField="POM_PO_NO" OnPageIndexChanged="dtg_POList_Page">
										<Columns>
											<asp:TemplateColumn SortExpression="POM_PO_NO" HeaderText="PO No.">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemTemplate>
													<asp:Label id="lbl_po_no" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn SortExpression="POM_PO_DATE" HeaderText="PO Date">
												<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Buyer Company">
												<HeaderStyle Width="27%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="POM_BUYER_NAME" SortExpression="POM_BUYER_NAME" HeaderText="Buyer">
												<HeaderStyle Width="27%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="PO Status">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
										</Columns>
									</asp:datagrid></td>
							</tr>
							<tr>
								<td class="emptycol" colSpan="5"></td>
							</tr>
							<tr>
								<td class="emptycol" colSpan="5"></td>
							</tr>
						</TABLE>
		</form>
	</body>
</HTML>
