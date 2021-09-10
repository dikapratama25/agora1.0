<%--Copyright © 2013 STRATEQ GLOBAL SERVICES. All rights reserved.--%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReturnOutwardSearch.aspx.vb" Inherits="eProcure.ReturnOutwardSearch" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>ReturnOutwardSearch</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
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
	</head>
	<body class="body" runat="server">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_SearchRO_tabs"))%>
               <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
				<tr>
					<td class="linespacing1" colspan="6"></td>
			    </tr>
				<tr>
	                <td colspan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo" Text="Fill in the search criteria and click Search button to list the relevant Return Outward. Click the Return Outward No. to go to raise Return Outward page."></asp:label>
	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colspan="6"></td>
			    </tr>
				<tr>
				<td class="tableheader" width="100%" align="left" colspan="6" style="height: 19px">&nbsp;Search Criteria</td>
				</tr>
				<tr>
					<td class="tableCOL" width="15%">&nbsp;<strong>RO No.</strong> :</td>
					<td class="tableCOL" width= "25%"><asp:textbox id="txtRONo" runat="server" CssClass="TXTBOX"></asp:textbox></td>
					<td class="tableCOL" width= "5%"></td>
					<td class="tableCOL" width= "15%">	<strong>Vendor Name&nbsp;</strong>: </td>
					<td class="tableCOL" width= "25%"><asp:textbox id="txtVendorName" runat="server" width="200px" CssClass="TXTBOX"></asp:textbox></td>
					<td class="tableCOL" width= "15%"></td>
				</tr>
				<tr>
					<td class="tableCOL" width="15%">&nbsp;<strong>Item Code</strong> :</td>
					<td class="tableCOL" width= "25%"><asp:textbox id="txtItemCode" runat="server" CssClass="TXTBOX"></asp:textbox></td>
					<td class="tableCOL" width= "5%"></td>
					<td class="tableCOL" width= "15%"></td>
					<td class="tableCOL" width= "25%"></td>
					<td class="tableCOL" width= "15%"></td>
				</tr>
				<tr>
					<td class="tableCOL" width="15%"><strong>&nbsp;Start Date</strong>&nbsp;: </td>
					<td class="tableCOL" width= "25%">
					    <asp:textbox id="txt_startdate" runat="server" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(StartCalendar)%><asp:requiredfieldvalidator id="rfv_txt_startdate" runat="server" ErrorMessage="Start Date is required."
								ControlToValidate="txt_startdate"></asp:requiredfieldvalidator></td>
					<td class="tableCOL" width= "5%"></td>
					<td class="tableCOL" width= "15%"><strong>End Date</strong>&nbsp;:</td>
					<td class="tableCOL" width= "25%"><asp:textbox id="txt_enddate" runat="server" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(EndCalendar)%><asp:requiredfieldvalidator id="rfv_txt_enddate" runat="server" ErrorMessage="End Date is required."
								ControlToValidate="txt_enddate"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="CompareValidator1" runat="server" Display="None" Operator="GreaterThanEqual"
							Type="Date" ControlToValidate="txt_enddate" ControlToCompare="txt_startdate" ErrorMessage="End Date must greater than or equal to Start Date">*</asp:comparevalidator>&nbsp;</td>
				    <td class="tableCOL" width= "15%" align="right">
				    <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
				    <asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear"></asp:button>
					</td>
				</tr>
			</table>
			<table class="AllTable" id="tblSearchResult" width="100%" cellspacing="0" cellpadding="0"
				border="0" runat="server">
				<tr>
					<td class="emptycol" colspan="5" style="width: 703px"></td>
				</tr>
				<tr>
					<td colspan="5" style="width: 100%;">
					    <asp:datagrid id="dtg_ROList" runat="server" DataKeyField="IROM_RO_NO" OnSortCommand="SortCommand_Click"
							AutoGenerateColumns="False" OnPageIndexChanged="dtg_ROList_Page">
							<Columns>
								<asp:TemplateColumn SortExpression="IROM_RO_NO" HeaderText="RO No">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle VerticalAlign="Middle"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkRONo"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="IROM_RO_DATE" SortExpression="IROM_RO_DATE" HeaderText="RO Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor Name">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="IROM_GRN_NO" HeaderText="GRN No">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle VerticalAlign="Middle"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkGRNNo"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="GM_DATE_RECEIVED" SortExpression="GM_DATE_RECEIVED" HeaderText="GRN Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
								</asp:BoundColumn>					
							</Columns>
						</asp:datagrid>
                        <asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List"
                            ShowMessageBox="True" ShowSummary="False" Width="22%" />
                    </td>
				</tr>
			</table>
		</form>
	</body>
</html>
