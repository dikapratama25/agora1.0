<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HubCatalogueDetail.aspx.vb" Inherits="eAdmin.HubCatalogueDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Contract Catalogue Approval</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
            Dim calStartDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtStartDate") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
            Dim calEndDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEndDate") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
       </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		
			function selectAll()
			{
				SelectAllG("dtgCatalogue__ctl2_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCatalogue__ctl2_chkAll","chkSelection");
			}
			
			function CheckDeleteMaster(pChkSelName){
				var oform = document.forms[0];
				var itemCnt, itemCheckCnt;
				var result, result2;
				itemCnt = parseInt(Form1.hidCnt.value);
				itemCheckCnt = 0;
				
				re = new RegExp(':' + pChkSelName + '$');  //generated control name starts with a colon	
				for (var i=0;i<oform.elements.length;i++){
					var e = oform.elements[i];
					if (e.type=="checkbox"){						
						if (re.test(e.name)){
							//itemCnt ++;
							if (e.checked==true)
								itemCheckCnt ++;
						}
					}
				}
				
				if (itemCheckCnt == 0) {
					alert ('Please make at least one selection!');
					return false;
				}
				else{
					if (itemCnt == itemCheckCnt) {
						if (Form1.hidCatType.value == 'D'){
							if (Form1.hidComCnt.value == 0 ){
								result = confirm('Are you sure that you want to permanently delete this item(s) ?');
								if (result == true){
									CheckDeleteMaster2();
									/*result2 = confirm('Delete Master record too ?');
									if (result2 == true) 
										Form1.hidDelete.value = "1";								
									else
										Form1.hidDelete.value = "0";
									return true;*/
								}
								else
									return false;
							}
							else {
								Form1.hidDelete.value = "0";
								return confirm('Are you sure that you want to permanently delete this item(s) ?');
							}
						}
						else { // cattype=='C'
							result = confirm('Are you sure that you want to permanently delete this item(s) ?');
							if (result == true){
								CheckDeleteMaster2();
								/*result2 = confirm('Delete Master record too ?');
								if (result2 == true) 
									Form1.hidDelete.value = "1";								
								else
									Form1.hidDelete.value = "0";
								return true;*/
							}
							else
								return false;
						}
					}	
					else {
						Form1.hidDelete.value = "0";
						return confirm('Are you sure that you want to permanently delete this item(s) ?');
					}
				}				
			}

		-->
		</script>
		<script language="vbscript">
			
			sub CheckDeleteMaster2		
				dim msg
				msg = msgbox ("Delete Master record too?",4)
				'if Form1.hidDeleteCnt.value = "1" then						
					'//yes=6, no=7
					'msgbox(msg, 4,"change") 
					if msg = vbYes then
						Form1.hidDelete.value = "1"
					else					
						Form1.hidDelete.value = "0"						
					end if
				'end if
			end sub				
					
		</script>
	</HEAD>
	<body onload="" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header" colspan="4"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="4"></TD>
				</TR>
				<TR>
					<TD class="TableHeader" colSpan="4">&nbsp;<asp:Label id="lblHeader" runat="server"></asp:Label></TD>
				</TR>
				<TR id="trCode" vAlign="top" runat="server">
					<TD class="tablecol" noWrap width="15%">&nbsp;<STRONG>Contract Ref. No.<asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
					<TD class="TableInput" width="30%"><asp:textbox id="txtCode" runat="server" CssClass="txtbox" MaxLength="20" Rows="1"></asp:textbox><asp:requiredfieldvalidator id="revCode" runat="server" Display="None" ErrorMessage="Contract Ref. No. Required"
							ControlToValidate="txtCode"></asp:requiredfieldvalidator></TD>
					<TD class="tablecol" noWrap width="15%">&nbsp;<STRONG>Description<asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
					<TD class="TableInput" width="40%"><asp:textbox id="txtDesc" runat="server" CssClass="txtbox" MaxLength="250" Width="200px"></asp:textbox><asp:requiredfieldvalidator id="revDesc" runat="server" Display="None" ErrorMessage="Contract Description Required"
							ControlToValidate="txtDesc"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR id="trDate" vAlign="top" runat="server">
					<TD class="tablecol" noWrap>&nbsp;<STRONG>Start Date<asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
					<TD class="TableInput"><asp:textbox id="txtStartDate" runat="server" CssClass="txtbox" MaxLength="50" contentEditable="false" ></asp:textbox><% Response.Write(calStartDate)%><asp:requiredfieldvalidator id="revStartDate" runat="server" Display="None" ErrorMessage="Start Date Required"
							ControlToValidate="txtStartDate"></asp:requiredfieldvalidator></TD>
					<TD class="tablecol" noWrap>&nbsp;<STRONG>End Date<asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label></STRONG>:</TD>
					<TD class="TableInput"><asp:textbox id="txtEndDate" runat="server" CssClass="txtbox" MaxLength="50" contentEditable="false" ></asp:textbox><% Response.Write(calEndDate)%><asp:requiredfieldvalidator id="revEndDate" runat="server" Display="None" ErrorMessage="End Date Required" ControlToValidate="txtEndDate"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR id="trCodeRead" vAlign="top" runat="server">
					<TD class="tablecol" noWrap width="15%">&nbsp;<STRONG><asp:label id="lblCodeLabel" runat="server"></asp:label></STRONG>&nbsp;:</TD>
					<TD class="TableInput" width="30%"><asp:label id="lblCode" runat="server"></asp:label></TD>
					<TD class="tablecol" noWrap width="15%">&nbsp;<STRONG>Description</STRONG>&nbsp;:</TD>
					<TD class="TableInput" width="40%"><asp:label id="lblDesc" runat="server"></asp:label></TD>
				</TR>
				<TR id="trDateRead" vAlign="top" runat="server">
					<TD class="tablecol" noWrap>&nbsp;<STRONG>Start Date</STRONG>&nbsp;:</TD>
					<TD class="TableInput"><asp:label id="lblStartDate" runat="server"></asp:label><% Response.Write(calStartDate)%></A></TD>
					<TD class="tablecol" noWrap>&nbsp;<STRONG>End Date</STRONG>:</TD>
					<TD class="TableInput"><asp:label id="lblEndDate" runat="server"></asp:label><% Response.Write(calEndDate)%></A></TD>
				</TR>
				<TR id="trBuyer" vAlign="top" runat="server">
					<TD class="tablecol" noWrap>&nbsp;<STRONG>Buyer Company<asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
					<TD class="TableInput" colSpan="3"><asp:dropdownlist id="cboBuyer" runat="server" CssClass="ddl"></asp:dropdownlist><asp:requiredfieldvalidator id="revBuyer" runat="server" Display="None" ErrorMessage="Buyer Company Required"
							ControlToValidate="cboBuyer"></asp:requiredfieldvalidator><asp:comparevalidator id="cvDate" runat="server" ErrorMessage="End Date Must be Later Than Start Date"
							ControlToValidate="txtEndDate" Type="Date" Operator="GreaterThan" ControlToCompare="txtStartDate" Display="None"></asp:comparevalidator></TD>
				</TR>
				<TR id="trBuyerRead" vAlign="top" runat="server">
					<TD class="tablecol" noWrap>&nbsp;<STRONG>Buyer Company</STRONG>&nbsp;:</TD>
					<TD class="TableInput" colSpan="3"><asp:label id="lblBuyer" runat="server"></asp:label></TD>
				</TR>
				<tr>
					<td colSpan="4">&nbsp;</td>
				</tr>
				<tr>
					<td colspan="4"><asp:datagrid id="dtgCatalogue" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CDI_PRODUCT_CODE" HeaderText="Item ID">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CDI_VENDOR_ITEM_CODE" SortExpression="CDI_VENDOR_ITEM_CODE" ReadOnly="True"
									HeaderText="Vendor Item Code">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDI_PRODUCT_DESC" SortExpression="CDI_PRODUCT_DESC" ReadOnly="True" HeaderText="Item Description">
									<HeaderStyle Width="27%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDI_CURRENCY_CODE" SortExpression="CDI_CURRENCY_CODE" ReadOnly="True"
									HeaderText="Currency">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDI_UNIT_COST" SortExpression="CDI_UNIT_COST" ReadOnly="True" HeaderText="Price">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDI_UOM" SortExpression="CDI_UOM" ReadOnly="True" HeaderText="UOM">
									<HeaderStyle Width="7%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDI_REMARK" SortExpression="CDI_REMARK" ReadOnly="True" HeaderText="Remarks">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<TR>
					<TD colspan="4">&nbsp;</TD>
				</TR>
				<TR>
					<TD colspan="4"><asp:button id="cmdSave" runat="server" CssClass="Button" Text="Save"></asp:button>
						<asp:button id="cmdAdd" runat="server" CssClass="Button" Text="Add"></asp:button>
						<asp:button id="cmdModify" runat="server" CssClass="Button" Text="Modify" CausesValidation="False"></asp:button>
						<asp:button id="cmdCompany" runat="server" CssClass="Button" Width="128px" Text="Company Assignment"
							CausesValidation="False"></asp:button>
						<asp:button id="cmdDelete" runat="server" CssClass="Button" Text="Delete" CausesValidation="False"></asp:button>
						<INPUT class="txtbox" id="hidDelete" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidDelete" runat="server"> <INPUT class="txtbox" id="hidComCnt" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidComCnt" runat="server"> <INPUT class="txtbox" id="hidCatType" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidCatType" runat="server"><INPUT class="txtbox" id="hidCnt" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidCnt" runat="server"></TD>
				</TR>
				<TR>
					<TD colspan="4">&nbsp;</TD>
				</TR>
				<TR>
					<TD colspan="4">
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
					</TD>
				</TR>
				<TR>
					<TD colspan="4">&nbsp;</TD>
				</TR>
				<TR>
					<TD colspan="4"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
