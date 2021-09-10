<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReliefStaff_AO.aspx.vb" Inherits="eProcure.ReliefStaff_AO" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ReliefStaff_AO</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">	
		
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim aDtFr As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr") & "','cal','width=180,height=155,left=270,top=180')""><IMG style=""CURSOR: hand"" height=""16"" src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """ align=""absBottom"" vspace=""0""></A>"
		    Dim aDtTo As String = "<A onclick=""window.open('" & dDispatcher.direct ("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo") & "', 'cal','width=180,height=155,left=530,top=180')""><IMG style=""CURSOR: hand"" height=""16"" src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """align=""absBottom"" vspace=""0""></A>"
		    
		    Dim aDtNew As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDatenew") & "','cal','width=180,height=155,left=290,top=240')""><IMG style=""CURSOR: hand"" height=""16"" src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """ align=""absBottom"" vspace=""0""></A>"
		    'Dim aDtNew As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txt_validity") & "','cal','width=190,height=165,left=270,top=180');""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " width=""16""></A>"		    
        </script>
		
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--  
		function selectAll()
		{
			SelectAllG("MyDataGrid_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"MyDataGrid_ctl02_chkAll","chkSelection");
		}
	
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="header" style="HEIGHT: 25px"><STRONG><FONT size="1">&nbsp;</FONT>Relief 
							Staff Assignment</STRONG></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tableheader">&nbsp;For Approving Officer</TD>
				</TR>
				<TR>
					<TD class="tablecol">&nbsp;<STRONG>Start Date<asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:
						<asp:textbox id="txtDateFr" runat="server" CssClass="txtbox" Width="90px"  contentEditable="false" ></asp:textbox>
						<span id="cal1" style="DISPLAY:none" runat="server"><% Response.Write(aDtFr)%></span><STRONG>End Date<asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:
						<asp:textbox id="txtDateTo" runat="server" CssClass="txtbox"  contentEditable="false"  Width="90px"></asp:textbox>
						<span id="cal2" style="DISPLAY:none" runat="server"><% Response.Write(aDtTo)%></span>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD>
						<DIV id="hide" style="DISPLAY: none" runat="server">
							<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
								<TR>
									<TD class="tableheader">&nbsp;Please
										<asp:label id="lbl_add_mod" Runat="server"></asp:label>&nbsp;the following 
										value.
									</TD>
								</TR>
								<TR>
									<TD class="tablecol" nowrap>&nbsp;<STRONG>New End Date</STRONG><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:
										<asp:textbox id="txtDatenew" runat="server" CssClass="txtbox" MaxLength="8"  contentEditable="false" ></asp:textbox><% Response.Write(aDtNew)%>
										<asp:button id="cmdsave_newdate" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;
										<asp:button id="cmdclear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>&nbsp;
										<asp:button id="cmd_canceldiv" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"></asp:button></TD>
								</TR>
								<TR>
									<TD class="emptycol"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
										required field<asp:requiredfieldvalidator id="rfv_date" runat="server" ErrorMessage="New End Date is required." ControlToValidate="txtDatenew"
											Display="None"></asp:requiredfieldvalidator>
										<asp:requiredfieldvalidator id="vldDateFr" runat="server" Display="None" ControlToValidate="txtDateFr" ErrorMessage="Start Date is required."></asp:requiredfieldvalidator>
										<asp:requiredfieldvalidator id="vldDateTo" runat="server" Display="None" ControlToValidate="txtDateTo" ErrorMessage="End Date is required."></asp:requiredfieldvalidator>
										<asp:comparevalidator id="vldDateFtDateTo" runat="server" ErrorMessage="End Date should be >= Start Date"
											ControlToValidate="txtDateTo" Display="None" ControlToCompare="txtDateFr" Operator="GreaterThanEqual"
											Type="Date"></asp:comparevalidator></TD>
								</TR>
							</TABLE>
						</DIV>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<asp:Label id="lbldate" runat="server" ForeColor="Red"></asp:Label>
						<asp:Label id="lblnewdate" runat="server" ForeColor="Red"></asp:Label></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:datagrid id="MyDataGrid" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="MyData_Page">
							<Columns>
								<asp:BoundColumn DataField="AGM_GRP_NAME" SortExpression="AGM_GRP_NAME"  readonly="true"   HeaderText="Approval Group">
									<HeaderStyle Width="30%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME" SortExpression="AGA_A_AO" readonly="true"  HeaderText="Alternative Approving Officer">
									<HeaderStyle Width="50%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AGA_RELIEF_IND" SortExpression="AGA_RELIEF_IND" readonly="true"    HeaderText="Relief Staff Control">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="AAO_ID"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="AGA_GRP_INDEX"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;
						<asp:button id="cmdcancel" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"
							Width="59px"></asp:button>&nbsp;
						<asp:button id="cmdExtend" runat="server" CssClass="button" Text="Extend" CausesValidation="False"></asp:button>&nbsp;&nbsp;&nbsp;
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 44px">
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
