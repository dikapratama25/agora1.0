<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IPPFITRMaint.aspx.vb" Inherits="eProcure.IPPFITRMaint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<title>E2PFITRMaint</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim FromCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtAddValidSDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
            Dim ToCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtAddValidEDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG style=""CURSOR: hand"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "></A>"
        </script> 
        <%response.write(Session("WheelScript"))%>
        <% Response.Write(Session("JQuery")) %>	
		<script type="text/javascript">
		<!--  
		    
		function selectAll()
		{
			SelectAllG("dtgFITR_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgFITR_ctl02_chkAll","chkSelection");
		}
	
		-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
			<tr>
				<td class="Header" colspan="4"><asp:label id="lblTitle" runat="server">FITR Maintenance</asp:label></td>
			</tr>
			<tr>
				<td>
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Fill in the search criteria and click Search button to list the relevant FITR code. Click the Add button to add new FITR code."></asp:label>
				</td>
			</tr>
            <tr>
					<td class="linespacing2" colspan="4"></td>
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
								<td class="tablecol" width="12%">&nbsp;<strong>FITR Code </strong>:</td>
								<td class="tablecol" width="20%"><asp:textbox id="txtFITRCode" runat="server" MaxLength="20" CssClass="txtbox" width="150px"></asp:textbox></td>							
								<td class="tablecol" width="12%"></td>
								<td class="tablecol" width="20%"></td>
								<td class="tablecol" width="36%" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdClear1" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button></td>
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
						<td width="15%" style="height: 24px">&nbsp;<strong>FITR Code</strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
						<td width="20%" style="height: 24px"><asp:textbox id="txtAddFITRCode" runat="server" MaxLength="20" width="150px" CssClass="txtbox"></asp:textbox></td>
						<td width="15%" style="height: 24px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>FITR Description</strong><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
						<td width="20%" style="height: 24px"><asp:textbox id="txtAddDesc" runat="server" MaxLength="200" width="200px" CssClass="txtbox"></asp:textbox></td>
						<%--<td width="12%" style="height: 24px"></td>--%>
						<td style="height: 24px" colspan="2"></td>
					</tr>
					<tr class="tablecol">
						<td width="15%" style="height: 24px">&nbsp;<strong>FITR Recoverable</strong><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<br/>(eg:70%=0.7)</td>
						<td width="20%" style="height: 24px"><asp:textbox id="txtAddFITRR" runat="server" MaxLength="50" width="150px" CssClass="txtbox"></asp:textbox></td>
						<!--<td width="15%" style="height: 24px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Valid Start Date</strong><asp:label id="Label8" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>-->
						<td width="15%" style="height: 24px"></td>
						<td width="20%" style="height: 24px"><asp:textbox id="txtAddValidSDate" runat="server" MaxLength="20" width="150px" CssClass="txtbox" contentEditable="false" Visible="false"></asp:textbox>
						<!--<% Response.Write(FromCalendar)%></td>-->
						<td style="height: 24px" colspan="2"></td>
					</tr>
					<tr class="tablecol">
						<td width="15%" style="height: 24px">&nbsp;<strong>FITR Irrecoverable</strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:<br/>(eg:70%=0.7)</td>
						<td width="20%" style="height: 24px"><asp:textbox id="txtAddFITRI" runat="server" MaxLength="50" width="150px" CssClass="txtbox"></asp:textbox></td>
						<!--<td width="15%" style="height: 24px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Valid End Date</strong><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>-->
						<td width="15%" style="height: 24px"></td>
						<td width="20%" style="height: 24px"><asp:textbox id="txtAddValidEDate" runat="server" MaxLength="20" width="150px" CssClass="txtbox" contentEditable="false" Visible="false"></asp:textbox><%--<% Response.Write(ToCalendar)%>--%></td>
						<td align="right" colspan="2">
						    <asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>
							<asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>
							<asp:button id="cmdCancel" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"></asp:button>
						</td>
					</tr>
					<tr>
						<td colspan="6" class="emptycol" style="height: 19px"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
							required field<asp:requiredfieldvalidator id="validateFITRCode" runat="server" ControlToValidate="txtAddFITRCode" ErrorMessage="FITR Code is required."
								Display="None"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="validateFITRR" runat="server" ControlToValidate="txtAddFITRR" ErrorMessage="FITR Recoverable is required."
								Display="None"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="validateFITRDesc" runat="server" ControlToValidate="txtAddDesc" ErrorMessage="FITR Description is required."
								Display="None"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="validateFITRI" runat="server" ControlToValidate="txtAddFITRI" ErrorMessage="FITR Irrecoverable is required."
								Display="None"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="validateValidSDate" runat="server" ControlToValidate="txtAddValidSDate" ErrorMessage="Valid Start Date is required."
								Display="None"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="validateValidEDate" runat="server" ControlToValidate="txtAddValidEDate" ErrorMessage="Valid End Date is required."
								Display="None"></asp:requiredfieldvalidator><asp:RegularExpressionValidator ID="revFITRI" runat="server" ControlToValidate="txtAddFITRI"
                                Display="None" ErrorMessage="Invalid FITR Irecoverable." ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$"></asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="revFITRR" runat="server" ControlToValidate="txtAddFITRR"
                                Display="None" ErrorMessage="Invalid FITR Recoverable." ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$"></asp:RegularExpressionValidator>
                                <asp:comparevalidator id="cvDateNow" runat="server" ControlToValidate="txtAddValidSDate" ErrorMessage="Valid Start Date must not less than current date."
							Display="None" Operator="GreaterThanEqual" Type="Date"></asp:comparevalidator>
                            <asp:comparevalidator id="cvDate" runat="server" ControlToValidate="txtAddValidEDate" ErrorMessage="Valid Start Date should be < Valid End Date."
							Display="None" ControlToCompare="txtAddValidSDate" Operator="GreaterThan" Type="Date"></asp:comparevalidator>
						</td>
					</tr>
					    
					<tr>
						<td colspan="6">
							<asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label>
						</td>
					</tr>
					<tr>
						<td colspan="6">
							<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" Width="80%"></asp:validationsummary>	
						</td>
					</tr>
					<tr>
						<td colspan="6" class="emptycol"></td>
					</tr>
				</table>
			</div>
			<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="emptycol" style="width: 1192px">
						<p><asp:datagrid id="dtgFITR" runat="server" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False" Width="100%">
								<Columns>
									<asp:TemplateColumn HeaderText="Delete">
										<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<HeaderTemplate>
											<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
										</HeaderTemplate>
										<ItemTemplate>
											<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="FM_FITR_CODE" SortExpression="FM_FITR_CODE" HeaderText="FITR Code">
										<HeaderStyle Width="15%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FM_FITR_DESC" SortExpression="FM_FITR_DESC" HeaderText="FITR Description">
										<HeaderStyle Width="20%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FM_FITR_RECOVERABLE" SortExpression="FM_FITR_RECOVERABLE" HeaderText="FITR Recoverable">
										<HeaderStyle Width="10%"></HeaderStyle>
									</asp:BoundColumn>	
									<asp:BoundColumn DataField="FM_FITR_IRRECOVERABLE" SortExpression="FM_FITR_IRRECOVERABLE" HeaderText="FITR Irrecoverable">
										<HeaderStyle Width="10%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FM_VALID_FROM" SortExpression="FM_VALID_FROM" HeaderText="Valid Start Date" Visible="false">
										<HeaderStyle Width="10%"></HeaderStyle>
									</asp:BoundColumn>	
									<asp:BoundColumn DataField="FM_VALID_TO" SortExpression="FM_VALID_TO" HeaderText="Valid End Date" Visible="false">
										<HeaderStyle Width="10%"></HeaderStyle>
									</asp:BoundColumn>			    		
									<asp:BoundColumn Visible="False" DataField="FM_FITR_INDEX"></asp:BoundColumn>				
								</Columns>
							</asp:datagrid></p>
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 6px; width: 1192px;"></td>
				</tr>
				<tr style="height: 50px">
					<td class="emptycol" style="width: 1192px"><asp:button id="cmdAdd" runat="server" CssClass="button" Text="Add" CausesValidation="False" Visible="false"></asp:button>&nbsp;<asp:button id="cmdModify" runat="server" Width="59px" CssClass="button" Text="Modify" CausesValidation="False"></asp:button>&nbsp;<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete" Visible="false"></asp:button><input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"/>
					    <input id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server"/><input id="hidGLCode" type="hidden" size="1" name="hidGLCode" runat="server"/>&nbsp;
                        </td>
				</tr>
			</table>
		</form>
	</body>
</html>
