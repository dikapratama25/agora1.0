<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReliefStaff_Consol.aspx.vb" Inherits="eProcure.ReliefStaff_Consol" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>ReliefStaff_Consol</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim aDtFr As String = "<A onclick=""window.open('" & dispacther.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr") & "','cal','width=180,height=155,left=270,top=180')""><IMG style=""CURSOR: hand"" height=""16"" src=""" & dDispatcher.direct("Images", "i_calendar2.gif") & """ width=""16""></A>"
		    Dim aDtTo As String = "<A onclick=""window.open('" & dispacther.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo") & "','cal','width=180,height=155,left=530,top=180')""><IMG style=""CURSOR: hand"" height=""16"" src=""" & dDispatcher.direct("Images", "i_calendar2.gif") & """ width=""16""></A>"
		    Dim aDtNew As String = "<A onclick=""window.open('" & dispacther.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDatenew") & "','cal','width=180,height=155,left=290,top=240')""><IMG style=""CURSOR: hand"" height=""16"" src=""" & dDispatcher.direct("Images", "i_calendar2.gif") & """ width=""16""></A>"
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
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="HEIGHT: 25px"><STRONG><FONT size="1">&nbsp;</FONT>Relief 
							Staff Assignment</STRONG></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tableheader">&nbsp;For Consolidator</TD>
				</TR>
				<TR>
					<TD class="tablecol" nowrap>&nbsp;<STRONG>Start Date<asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:
						<asp:textbox id="txtDateFr" runat="server" CssClass="txtbox"  contentEditable="false"  Width="90px"></asp:textbox>
						<span id="cal1" style="DISPLAY:none" runat="server"><% Response.Write(aDtFr)%> </span>
						&nbsp;<STRONG>&nbsp;End Date<asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:
						<asp:textbox id="txtDateTo" runat="server" CssClass="txtbox"  contentEditable="false"  Width="90px"></asp:textbox>
						<span id="cal2" style="DISPLAY:none" runat="server"><% Response.Write(aDtTo)%></span>
						&nbsp;<STRONG>&nbsp;Relief Staff</STRONG> :
						<asp:dropdownlist id="cboConsolRelief" runat="server" CssClass="txtbox"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 18px"></TD>
				</TR>
				<TR>
					<TD>
						<DIV id="hide" style="DISPLAY: none" runat="server">
							<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
								<TR>
									<TD class="tableheader">&nbsp;Please
										<asp:label id="lbl_add_mod" Runat="server"></asp:label>
										&nbsp;the following value
									</TD>
								</TR>
								<TR>
									<TD class="tablecol">&nbsp;<STRONG>New End Date</STRONG><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:
										<asp:textbox id="txtDatenew" runat="server" CssClass="txtbox"  contentEditable="false"  Width="128px" MaxLength="50"></asp:textbox><% Response.Write(aDtNew)%><asp:button id="cmdsave_newdate" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;
										<asp:button id="cmdclear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>&nbsp;
										<asp:button id="cmd_cancel" runat="server" CssClass="button" Text="Cancel" CausesValidation="False"></asp:button></TD>
								</TR>
								<TR>
									<TD class="emptycol"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
										required field
										<asp:requiredfieldvalidator id="rfv_date" runat="server" ErrorMessage="New End Date is required." ControlToValidate="txtDatenew"
											Display="None"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="vldDateFr" runat="server" ErrorMessage="Start Date is required." ControlToValidate="txtDateFr"
											Display="None"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="vldDateTo" runat="server" ErrorMessage="End Date is required." ControlToValidate="txtDateTo"
											Display="None"></asp:requiredfieldvalidator><asp:comparevalidator id="vldDateFtDateTo" runat="server" ErrorMessage="End Date should be >= Start Date"
											ControlToValidate="txtDateTo" Display="None" Type="Date" Operator="GreaterThanEqual" ControlToCompare="txtDateFr"></asp:comparevalidator></TD>
								</TR>
							</TABLE>
						</DIV>
					</TD>
				</TR>
  <TR>
    <TD class=emptycol></TD></TR>
				<TR>
					<TD class="emptycol"><asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;
						<asp:button id="cmdcancel" runat="server" CssClass="button" Width="59px" Text="Cancel" CausesValidation="False"></asp:button>&nbsp;&nbsp;
						<asp:button id="cmdExtend" runat="server" CssClass="button" Text="Extend" CausesValidation="False"></asp:button>&nbsp;&nbsp;&nbsp;<INPUT id="hidMode" style="WIDTH: 42px; HEIGHT: 22px" type="hidden" size="1" name="hidMode"
							runat="server"> <INPUT id="hidIndex" style="WIDTH: 49px; HEIGHT: 22px" type="hidden" size="2" name="hidIndex"
							runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<asp:Label id="lbldate" runat="server" ForeColor="Red"></asp:Label>
						<asp:Label id="lblnewdate" runat="server" ForeColor="Red"></asp:Label>
					</TD>
				</TR>				
				<TR>
					<TD style="HEIGHT: 64px"><br>
						<asp:validationsummary id="Validationsummary1" runat="server" CssClass="errormsg"></asp:validationsummary>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
