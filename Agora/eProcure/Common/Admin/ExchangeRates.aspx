<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ExchangeRates.aspx.vb" Inherits="eProcure.ExchangeRates" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>ExchangeRates</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim FromCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_DateFrom") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim ToCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_DateTo") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim SFromCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_SDateFrom") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim SToCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_SDateTo") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--  
		function selectAll()
		{
			SelectAllG("dtgExRate_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgExRate_ctl02_chkAll","chkSelection");
		}


		function Display(num)
			{
				var check = num;
				var div_add = document.getElementById("Hide_Add1");
				var div_add = document.getElementById("Hide_Add2");
				var div_add = document.getElementById("Hide_Add3");
				
				var cmd_delete = document.getElementById("cmd_delete");
				var hidMode = document.getElementById("hidMode");
				var add_mod = document.getElementById("add_mod");
				div_add.style.display ="";
				
				if (check==1)
				{
					cmd_delete.style.display = "none";
					Form1.hidMode.value = 'm';
					add_mod.value='add';
				}
				else if (check==0)
				{
					cmd_delete.style.display = "none";
				}
			}
	
	        function ShowDialog(filename,height)
		    {
    			
			    var retval="";
			    retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 750px");
			    //retval=window.open(filename);
			    if (retval == "1" || retval =="" || retval==null)
			    {  
			        window.close;
				    return false;

			    } else {
			        window.close;
				    return true;

			    }
		    }
		-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
			<tr>
				<td class="Header" style="padding:0" colspan="4"><asp:label id ="lblTitle" runat="server" Text="Exchange Rate"></asp:label></td>
			</tr>
			<tr>
				<td class="linespacing1" colspan="4"></td>
			</tr>
			<tr>
				<td >
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Fill in the search criteria and click Search button to list the relevant exchange rates. Click the Add button to add new exchange rate."></asp:label><br/>
                    <asp:Label ID="Label3" runat="server" CssClass="lblInfo" Text="Example:"></asp:Label><br/>
                    <asp:Label ID="Label6" runat="server" CssClass="lblInfo" Text="1. Currency Code: USD (US Dollar)"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label8" runat="server" CssClass="lblInfo" Text="Exchange Rate: 3.20"></asp:Label><br />
                    <asp:Label ID="Label7" runat="server" CssClass="lblInfo" Text="2. Currency Code: IDR (Indonesian Rupiah)"></asp:Label>&nbsp;<asp:Label
                        ID="Label10" runat="server" CssClass="lblInfo" Text="Exchange Rate: 0.000279"></asp:Label>
				</td>
			</tr>
            <tr>
				<td class="linespacing1" colspan="4"></td>
			</tr>
				<tr>
					<td class="tablecol">
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0" width="100%">
				<tr>
					<td class="header" colspan="3" width="100%"></td>
				</tr>
							<tr>
								<td class="tableheader" colspan="5" width="100%">&nbsp;Search Criteria</td>
							</tr>
							<tr class="tablecol" id="trGrpType" runat="server">
								<td class="tablecol" style="width: 15%;">&nbsp;<strong>Currency Code </strong>:</td>
								<td class="tablecol" style="width: 20%;"><asp:DropDownList ID="cboCurr1" CssClass="ddl" runat="server" Width="150px"></asp:DropDownList></td>
								<td></td>
								<td></td>							
								<td class="tablecol" width="56%" align="right" style="height: 23px" ><asp:button id="cmd_search" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmd_clear1" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button></td>
                            </tr>
                            <tr class="tablecol" style="width:100%">
					            <td width="15%">&nbsp;<strong>Valid Date From</strong>&nbsp;:</td>
					            <td style="width: 20%"><asp:textbox id="txt_SDateFrom" runat="server" MaxLength="20" width="150px" CssClass="txtbox" contentEditable="false"></asp:textbox><% Response.Write(SFromCalendar)%></td>
					            <td width="15%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Valid Date To</strong>&nbsp;:</td>
						        <td width="20%"><asp:textbox id="txt_SDateTo" runat="server" MaxLength="50" width="150px" CssClass="txtbox" contentEditable="false"></asp:textbox><% Response.Write(SToCalendar)%></td>
					            <td></td>
					</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="3"></td>
				</tr>
			</table>
			<div id="Hide_Add2" style="DISPLAY: none" runat="server">
				<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
					<tr>
						<td colspan="6" class="tableheader" style="height: 19px" >&nbsp;Please
							<asp:label id="lbl_add_mod" Runat="server"></asp:label>&nbsp;the following 
							value.
						</td>
					</tr>
					<tr class="tablecol">
						<td width="15%">&nbsp;<strong>Currency Code</strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
						<td width="20%"><asp:DropDownList ID="cboCurr2" CssClass="ddl" runat="server" Width="150px"></asp:DropDownList></td>
						<td width="15%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Exchange Rate</strong><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
						<td width="20%"><asp:textbox id="txt_add_ExRate" runat="server" MaxLength="50" width="150px" CssClass="txtbox" ></asp:textbox></td>
						<td align="right">	
						    <asp:button id="cmd_save" runat="server" CssClass="button" Text="Save"></asp:button>
							<asp:button id="cmd_clear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
							<asp:button id="cmd_cancel" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"></asp:button>
						</td>
					</tr>
					<tr class="tablecol" style="width:100%">
					    <td width="15%">&nbsp;<strong>Valid Date From</strong><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					    <td width="20%"><asp:textbox id="txt_DateFrom" runat="server" MaxLength="20" width="150px" CssClass="txtbox" contentEditable="false"></asp:textbox><% Response.Write(FromCalendar)%></td>
					    <td width="15%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Valid Date To</strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
						<td width="20%"><asp:textbox id="txt_DateTo" runat="server" MaxLength="50" width="150px" CssClass="txtbox" contentEditable="false"></asp:textbox><% Response.Write(ToCalendar)%></td>
					    <td></td>
					</tr>
					<tr>
						<td colspan="3" class="emptycol" style="height: 19px"><asp:label id="Label9" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
							required field
							<asp:requiredfieldvalidator id="revCurrCode" runat="server" ControlToValidate="cboCurr2" ErrorMessage="Currency Code is required." Display="None"></asp:requiredfieldvalidator>
							<asp:requiredfieldvalidator id="validate_ex_rate" runat="server" ControlToValidate="txt_add_ExRate" ErrorMessage="Exchange Rate is required." Display="None">
							</asp:requiredfieldvalidator><asp:RegularExpressionValidator ID="rev_ex_rate" runat="server" ControlToValidate="txt_add_ExRate"
                                Display="None" ErrorMessage="Invalid Exchange Rate." ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,6})?$"></asp:RegularExpressionValidator>
                            <asp:requiredfieldvalidator id="revDateFrom" runat="server" ControlToValidate="txt_DateFrom" ErrorMessage="Valid Date From is required." Display="None"></asp:requiredfieldvalidator>
                            <asp:requiredfieldvalidator id="revDateTo" runat="server" ControlToValidate="txt_DateTo" ErrorMessage="Valid Date To is required." Display="None"></asp:requiredfieldvalidator>
                            <asp:comparevalidator id="cvDateNow" runat="server" ControlToValidate="txt_DateFrom" ErrorMessage="Valid Date From should be >= today's date."
							Display="None" Operator="GreaterThanEqual" Type="Date"></asp:comparevalidator>       
                            <asp:comparevalidator id="cvDate" runat="server" ControlToValidate="txt_DateTo" ErrorMessage="Valid Date From should be < Valid Date To."
							Display="None" ControlToCompare="txt_DateFrom" Operator="GreaterThan" Type="Date"></asp:comparevalidator>
							</td>
					</tr>
					<tr>
					    <td colspan="3" >
					        <asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label>
					    </td> 
					</tr>
					<tr>
					    <td colspan="3" >
							<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
						</td>
					</tr> 
					<tr>
						<td class="emptycol"></td>
					</tr>
				</table>
			</div>
			<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="emptycol" style="width: 1192px">
						<p><asp:datagrid id="dtgExRate" runat="server" Visible="false" OnPageIndexChanged="dtgExRate_Page" AllowSorting="True"
								OnSortCommand="SortCommand_Click" AutoGenerateColumns="False" Width="100%">
								<Columns>
									<asp:TemplateColumn HeaderText="Delete">
										<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<HeaderTemplate>
											<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
											<!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
										</HeaderTemplate>
										<ItemTemplate>
											<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="CE_CURRENCY_CODE" SortExpression="CE_CURRENCY_CODE"  readonly="True"  HeaderText="Currency Code">
										<HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>	
									<asp:BoundColumn Visible="False" DataField="CE_CURRENCY_INDEX"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="CE_COY_ID"></asp:BoundColumn>
									<asp:BoundColumn DataField="CE_CURRENCY_NAME" HeaderText="Currency Name" SortExpression="CE_CURRENCY_NAME">
										<HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>	
									<asp:BoundColumn DataField="CE_RATE" HeaderText="Exchange Rate" SortExpression="CE_RATE">
										<HeaderStyle HorizontalAlign="Right" Width="15%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Right" />
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CE_VALID_FROM" HeaderText="Valid Date From" SortExpression="CE_VALID_FROM">
										<HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>	
									<asp:BoundColumn DataField="CE_VALID_TO" HeaderText="Valid Date To" SortExpression="CE_VALID_TO">
										<HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>								
								</Columns>
							</asp:datagrid></p>
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 6px; width: 1192px;"></td>
				</tr>
				<tr style="height: 50px">
					<td class="emptycol" style="width: 1192px"><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" Width="59px" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>
                        &nbsp;<asp:button id="cmdBatch" runat="server" Width="100px" CssClass="button" Text="Batch Currency" CausesValidation="False"></asp:button><input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"/><input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server"/>&nbsp;
                        <asp:button id ="btnHidden" runat="server" style="display:none"></asp:button>
                        </td>
				</tr>
			</table>
        </form>
	</body>
</html>
