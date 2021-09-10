<%@ Page Language="vb" AutoEventWireup="false" Codebehind="POViewB2.aspx.vb" Inherits="eProcure.POViewB2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>POViewB2</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim StartCalendar As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_startdate") & "','cal','width=190,height=165,left=270,top=180');""><IMG src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim EndCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_enddate") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            
            Dim sOpen As String = dDispatcher.direct("Initial", "popCalendar.aspx", "textbox="" + val + ""&seldate="" + txtVal.value")
        </script>
        
        <% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
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
			oform.ChK_open2.checked=checked;
			oform.chk_fully2.checked=checked;
			oform.chk_part2.checked=checked;
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtg_POList_ctl02_chkAll","chkSelection");
		}
		
		function setFulfilStatus()
		{
				alert("Yes");
		var oform = document.forms(0);
			alert("Yes");
			alert(oform.cboPOStatus.selectedValue);
        if (oform.cboPOStatus.selectedValue = 4)
            {
            ChK_open2.enabled = true;
            chk_part2.enabled = true;
            chk_fully2.enabled = true;
            ChK_open2.checked = false;
            chk_part2.checked = false;
            chk_fully2.checked = false;
            }
         else
            {
            ChK_open2.Enabled = false;
            chk_part2.Enabled = false;
            chk_fully2.Enabled = false;
            ChK_open2.Checked = false;
            chk_part2.Checked = false;
            chk_fully2.Checked = false;
            }
		}

		function Reset(){
			var oform = document.forms(0);
			oform.txt_po_no.value=""
			oform.txt_vendor.value=""
			oform.txt_startdate.value=""
			oform.txt_enddate.value=""
			oform.cboPOStatus.selectedIndex = 0
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
			window.open('<% Response.Write(sOpen)%>' ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
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
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_POViewBuyer_tabs"))%>
               <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
				<tr>
					<td class="linespacing1" colspan="6"></td>
			    </tr>
				<tr>
	                <td colspan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant PO."
		                ></asp:label>

	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colspan="4"></td>
			    </tr>
				<tr>
					<td class="tableheader" width="100%" align="left" colspan="6" style="height: 19px">&nbsp;Search Criteria</td>
							</tr>
							<tr >
								<td class="tableCOL" width="15%" >&nbsp;<strong>PO No.</strong> :</td>
								<td class="tableCOL" width= "25%" ><asp:textbox id="txt_po_no" runat="server" CssClass="TXTBOX"></asp:textbox></td>
								<td class="tableCOL" width= "5%" ></td>
								<td class="tableCOL" width= "15%" >	<strong>Vendor Name&nbsp;</strong>: </td>
								<td class="tableCOL" width= "25%" ><asp:textbox id="txt_vendor" runat="server" width="200px" CssClass="TXTBOX"></asp:textbox></td>
								<td class="tableCOL" ></td>
							</tr>
							<tr >
								<td class="tableCOL" width="15%" >&nbsp;<strong>Item Code</strong> :</td>
								<td class="tableCOL" width= "25%" ><asp:textbox id="txt_item_code" runat="server" CssClass="TXTBOX"></asp:textbox></td>
								<td class="tableCOL" width= "5%" ></td>
								<td class="tableCOL" width= "15%" ></td>
								<td class="tableCOL" width= "25%" ></td>
								<td class="tableCOL" ></td>
							</tr>
							<tr>
								<td class="tableCOL"><strong>&nbsp;Start Date</strong>&nbsp;: </td>
								<td class="tableCOL">
								    <asp:textbox id="txt_startdate" runat="server" CssClass="TXTBOX" contentEditable="false" ></asp:textbox><% Response.Write(StartCalendar)%></td>
								<td class="tableCOL" ></td>
								<td class="tableCOL"><strong>End Date</strong>&nbsp;:</td>
								<td class="tableCOL" colspan="2"><asp:textbox id="txt_enddate" runat="server" CssClass="TXTBOX" contentEditable="false" ></asp:textbox><% Response.Write(EndCalendar)%>
									<asp:comparevalidator id="CompareValidator1" runat="server" Display="None" Operator="GreaterThanEqual"
										Type="Date" ControlToValidate="txt_enddate" ControlToCompare="txt_startdate" ErrorMessage="End Date must greater than or equal to Start Date">*</asp:comparevalidator>&nbsp;</td>
							</tr>
										<tr><td class="tablecol" colspan="6"  style="height: 3px"></td></tr>
							<tr>
							<td class="tableCOL">&nbsp;<strong>PO Status</strong> :</td>
											<td class="tableCOL" colspan = "4" >
										<asp:dropdownlist id="cboPOStatus" runat="server" CssClass="ddl" autopostback="true">
											<asp:ListItem Value="">---Select---</asp:ListItem>
											<asp:ListItem Value="1">Draft</asp:ListItem>
											<asp:ListItem Value="2">Submitted for approval (Internal)</asp:ListItem>
											<asp:ListItem Value="3">Approved by management (Official)</asp:ListItem>
											<asp:ListItem Value="4">Accepted by vendor</asp:ListItem>
											<asp:ListItem Value="5">Completed delivery and paid</asp:ListItem>
											<asp:ListItem Value="6">Cancelled by buyer</asp:ListItem>
											<asp:ListItem Value="7">Rejected by management / vendor</asp:ListItem>
											<asp:ListItem Value="8">Void draft PO</asp:ListItem>
											<asp:ListItem Value="9">Held by management</asp:ListItem>
										</asp:dropdownlist>
								<td class="tableCOL" ></td>
										</tr>
										<tr>
											<td class="tablecol" >&nbsp;<strong>Fulfilment</strong>&nbsp;:</td>
											<td class="tablecol" colspan = "4" >
											<asp:checkbox id="ChK_open2" Text="Open" width= "100px" Runat="server" enabled="false"></asp:checkbox>
											<asp:checkbox id="chk_part2" Text="Partially Delivered" width= "125px" Runat="server" enabled="false"></asp:checkbox>
											<asp:checkbox id="chk_fully2" Text="Uninvoice / Unpaid Completed Delivery" width= "220px" Runat="server" enabled="false"></asp:checkbox></td>
											<td class="tablecol" ><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<input class="button" id="cmdSelectAll" onclick="SelectAll_1();" type="button" value="Select All"
													name="cmdSelectAll">&nbsp;<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"></td>
				</tr>
											</table>
						<table class="AllTable" id="tblSearchResult" width="100%" cellspacing="0" cellpadding="0"
							border="0" runat="server">
							<tr>
								<td class="emptycol" colspan="5"></td>
							</tr>
							<tr>
								<td colspan="6"><asp:datagrid id="dtg_POList" runat="server" DataKeyField="POM_PO_NO" OnSortCommand="SortCommand_Click"
										AutoGenerateColumns="False" OnPageIndexChanged="dtg_POList_Page">
										<Columns>
											<asp:TemplateColumn SortExpression="POM_PO_No" HeaderText="PO No.">
												<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
												<ItemStyle VerticalAlign="Middle"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkPONo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn SortExpression="POM_CREATED_DATE" HeaderText="PO Creation Date">
												<HeaderStyle Width="9%"></HeaderStyle>
												<ItemStyle HorizontalAlign="left" Width="10%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn SortExpression="POM_PO_DATE" HeaderText="PO Date">
												<HeaderStyle Width="9%"></HeaderStyle>
												<ItemStyle HorizontalAlign="left" Width="7%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="POM_S_COY_NAME" SortExpression="POM_S_COY_NAME" HeaderText="Vendor Name">
												<HeaderStyle Width="23%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn SortExpression="POM_ACCEPTED_DATE" HeaderText="PO Accepted Date">
												<HeaderStyle Width="9%"></HeaderStyle>
												<ItemStyle HorizontalAlign="left" Width="10%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="STATUS_DESC" HeaderText="PO Status">
												<HeaderStyle Width="18%"></HeaderStyle>
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
								            <%--<asp:TemplateColumn HeaderText="PR No.">
												<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
												<ItemStyle VerticalAlign="Middle"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>--%>
										</Columns>
									</asp:datagrid></td>
							</tr>
							<tr>
								<td class="emptycol" colspan="5"></td>
							</tr>
							<tr>
								<td class="emptycol" colspan="5">
                                    <asp:ValidationSummary ID="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List"
                                        ShowMessageBox="True" ShowSummary="False" Width="15%" />
                                </td>
							</tr>
						</table>
		</form>
	</body>
</html>
