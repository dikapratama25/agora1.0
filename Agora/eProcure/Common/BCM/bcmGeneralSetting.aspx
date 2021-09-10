<%@ Page Language="vb" AutoEventWireup="false" Codebehind="bcmGeneralSetting.aspx.vb" Inherits="eProcure.bcmGeneralSetting" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BCM General Setting</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            dim NewE as string = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtNewE") & "','cal','width=180,height=155,left=270,top=180');""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " align=""absBottom"" vspace=""0""></A>"
            Dim NewS As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtNewS") & "','cal','width=180,height=155,left=270,top=180');""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " align=""absBottom"" vspace=""0""></A>"
            Dim Ext As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtExt") & "','cal','width=180,height=155,left=270,top=180');""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " align=""absBottom"" vspace=""0""></A>"
            Dim BudgetDt As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtNewBudgetDt") & "','cal','width=180,height=155,left=270,top=180');""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " align=""absBottom"" vspace=""0""></A>"
        </script> 
	<%response.write(Session("WheelScript"))%>
	<%response.write(css)%>
	</HEAD>
	<BODY MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="Header" style="padding:0" colspan="3"><asp:label id ="lblTitle" runat="server" Text="BCM General Setting"></asp:label></TD>
				</TR>
				<tr><td class="rowspacing"></td></tr>
				<TR>
	                <TD class="EmptyCol" colSpan="6">
		                <asp:label id="lblAction" runat="server" CssClass="lblInfo" Text="Configure your company's BCM mode and Budgeting Period in the page. You can start a new budget period based on the new start date (which is the current active budget period 'end date'),enter the NEW BUDGET PERIOD END DATE and click on the Start New Period button."></asp:label>
	                </TD>
                </TR>
				<tr><td class="rowspacing"></td></tr>
			</TABLE>
			<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0"  width="100%" border="0">
				<TR>
					<TD class="tableheader" colSpan="3">&nbsp;BCM Mode&nbsp;
						<asp:hyperlink id="lnkBCM" runat="server" Font-Size="Smaller">more info...</asp:hyperlink></TD>
				</TR>				
				<tr>
					<TD class="TableCol" width="160">					    
					    <asp:radiobuttonlist id="rdMode" Runat="server" CssClass="rbtn" Width="150px" RepeatLayout="Table">
						<asp:ListItem Value="1">Absolute</asp:ListItem>
						<asp:ListItem Value="2">Advisory</asp:ListItem>
						<asp:ListItem Value="3" Selected="True">None</asp:ListItem>
						</asp:radiobuttonlist>
					</TD>
				</tr>
				<tr class="tablecol">
					<TD class="TableCol" >
					    <asp:button id="cmdSaveMode" Runat="server" CssClass="button" Width="100px" CausesValidation="False" Text="Save BCM Mode"></asp:button>
					</TD>
				</tr>
				<tr><td class="rowspacing"></td></tr>	
			</TABLE>
			<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="tableheader" colSpan="4">&nbsp;Budgeting Period</TD>
				</TR>				
				<TR>
					<TD class="tablecol" colSpan="4">&nbsp;<strong>Current </strong>:</TD>
				</TR>
				<TR>
					<TD class="tablecol" colSpan="4">&nbsp;Current Budget Period&nbsp;is from
						<asp:label id="lblCBudgetS" Runat="server" Font-Bold="True">N/A</asp:label>&nbsp;to
						<asp:label id="lblCBudgetE" Runat="server" Font-Bold="True">N/A</asp:label>
					</TD>
				</TR>				
			</TABLE>
			<div id="tbFirstNew" style="DISPLAY: none" runat="server">
				<TABLE class="alltable" id="table2" cellSpacing="0" cellPadding="0" border="0" width="100%">
					<TR class="tablecol">
						<TD class="tablecol" width="250">&nbsp;Budget period start date :</TD>
						<TD class="tablecol" width="150"><asp:textbox id="txtNewS" Runat="server" CssClass="txtbox" Width="100px"  contentEditable="false"  MaxLength="50"></asp:textbox><%response.write(NewS)%>
						</TD>
						<TD class="tablecol" width="50"></TD>
						<TD class="tablecol" class="tablecol">&nbsp;<asp:requiredfieldvalidator id="rfv_txtNewS" runat="server" ErrorMessage="Start budget period is required."
								ControlToValidate="txtNewS" Display="Dynamic"></asp:requiredfieldvalidator></TD>
					</TR>
					<TR class="tablecol">
						<TD class="tablecol" width="250">&nbsp;Budget period end date :</TD>
						<TD class="tablecol" width="150"><asp:textbox id="txtNewE" Runat="server" CssClass="txtbox" Width="100px"  contentEditable="false"  MaxLength="50"></asp:textbox><%response.write(NewE)%>
						</TD>
						<TD class="tablecol" width="50"><asp:button id="cmdStartNewBCM" Runat="server" CssClass="button" Width="100px" Text="Start New BCM"></asp:button></TD>
						<TD class="tablecol">&nbsp;<asp:requiredfieldvalidator id="rfv_txtNewE" runat="server" ErrorMessage="End budget period is required." ControlToValidate="txtNewE"
								Display="Dynamic"></asp:requiredfieldvalidator><asp:label id="lblMsg" Runat="server" CssClass="errormsg"></asp:label></TD>
					</TR>
					<tr class="tablecol">
						<td colSpan="4">&nbsp;</td>
					</tr>
				</TABLE>
			</div>
			<div id="tbBudget" style="DISPLAY: none" runat="server">
				<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0"  width="100%" border="0">
					<TR class="tablecol" style="DISPLAY: none">
						<TD class="tablecol" width="250">&nbsp;To extent current budget period :</TD>
						<TD class="tablecol" width="150"><asp:textbox id="txtExt" Runat="server" CssClass="txtbox" Width="100px"  contentEditable="false"  MaxLength="50"></asp:textbox><% Response.Write(Ext) %>
						</TD>
						<TD class="tablecol" width="50"><asp:button id="cmdSaveEndDT" Runat="server" CssClass="button" Width="100px" Text="Save End Date"></asp:button></TD>
						<TD class="tablecol">&nbsp;<asp:label id="lblmsgEDate" Runat="server" CssClass="errormsg"></asp:label></TD>
					</TR>
					<tr class="tablecol">
						<td class="tablecol" colSpan="5">&nbsp;</td>
						<td class="tablecol" colSpan="5">&nbsp;</td>
					</tr>
					<TR>
						<TD class="tablecol" colSpan="10">&nbsp;<strong>New </strong>:</TD>
					</TR>
					<TR class="tablecol">
						<TD class="tablecol" width="250">&nbsp;New budget period start date :</TD>
						<TD class="tablecol" colSpan="9"><asp:label id="txtNewBudgetS" Runat="server" CssClass="lblinfo" Font-Bold="True"></asp:label></TD>
					</TR>
					<TR class="tablecol">
						<TD class="tablecol" style="WIDTH: 92px; HEIGHT: 6px" width="92" colSpan="7"></TD>
					</TR>
					<TR class="tablecol">
						<TD class="tablecol" width="250">&nbsp;New budget period end date :</TD>
						<TD class="tablecol"><asp:textbox id="txtNewBudgetDt" Runat="server" CssClass="txtbox" Width="100px"  contentEditable="false" 
								MaxLength="50"></asp:textbox><% Response.Write(BudgetDt) %>
						</TD>
						<TD class="tablecol" colspan="4"><asp:button id="cmdSaveNewPeriod" Runat="server" CssClass="button" Width="100px" CausesValidation="False"
								Text="Start New Period"></asp:button></TD>
						<TD class="tablecol">&nbsp;<asp:regularexpressionvalidator id="rfv_txtNewBudgetDt" runat="server" ErrorMessage="Invalid new budget date." ControlToValidate="txtNewBudgetDt"
								Display="Dynamic" ValidationExpression="\d{1,2}/\d{1,2}/\d{4}"></asp:regularexpressionvalidator><asp:label id="lblNewBud" Runat="server" CssClass="errormsg"></asp:label></TD>
					</TR>
					<tr class="tablecol" style="HEIGHT: 20px">
						<TD class="tablecol" colSpan="7"><asp:button id="cmdDownload" Runat="server" CssClass="button" Width="130px" Text="Download Current Period Report"></asp:button>&nbsp;<asp:linkbutton id="lnkDownloadPre" Runat="server" CssClass="lnk" Visible="false"></asp:linkbutton></TD>
					</tr>
					<tr class="tablecol">
						<td class="tablecol" colSpan="7">&nbsp;</td>
					</tr>
					<TR>
						<TD class="tablecol" colSpan="5"></TD>
						<td class="tablecol" colSpan="5">&nbsp;</td>
					</TR>
				</TABLE>
				<asp:datagrid id="dgtest" Runat="server" AutoGenerateColumns="true"></asp:datagrid>
			</div>
		</form>
	</BODY>
</HTML>
