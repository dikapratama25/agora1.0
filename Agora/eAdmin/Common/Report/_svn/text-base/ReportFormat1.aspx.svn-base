<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportFormat1.aspx.vb" Inherits="eAdmin.ReportFormat1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Report</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	    <script runat="server">
		     Dim dDispatcher As New Dispatcher.dispatcher
	        Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
	        
	        Dim sStartDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtSDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"
	        Dim sEndDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEndDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"

		</script>
		
		<% Response.Write(sCSS) %>
		<%Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
				/*function Verifycheck(a){
					if (a==1)
						Form1.optSelectOne1.checked = false;
						
						document.getElementById("cboCompany").e="";
						
					else 
						Form1.optCom1.checked = false;
				} */
				
				
						
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="HEIGHT: 16px" colspan="2"><FONT size="1">&nbsp;</FONT><asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="2"></TD>
				</TR>
				<TR>
					<TD class="tableheader" colspan="2"><FONT size="1">&nbsp;</FONT><asp:label id="label1" runat="server">Report Criteria:</asp:label></TD>
				</TR>
				<!--
				<TR>
					<TD class="tablecol" width="20%" nowrap><asp:label id="label" runat="server">&nbsp;<STRONG>Vendor
							</STRONG>:
						</asp:label>&nbsp;</TD>
					<TD class="TableInput" width="80%">
						<input name="optCom" runat="server" type="radio" onclick="Verifycheck(1)" checked id="optCom1">All 
						Company &nbsp;<input name="optSelectOne" type="radio" runat="server" onclick="Verifycheck(2)" id="optSelectOne1">
						<asp:dropdownlist id="cboCompany1" runat="server" Width="120px" CssClass="txtbox"></asp:dropdownlist>
					</TD>
					-->
				<TR>
					<TD class="tablecol" width="20%" nowrap><asp:label id="Label3" runat="server">&nbsp;<STRONG>Vendor
							</STRONG>:
						</asp:label>&nbsp;</TD>
					<TD class="TableInput" width="80%">
						<asp:RadioButton id="optCom" runat="server" Text="All Company" GroupName="optComVen" AutoPostBack="True"></asp:RadioButton>
						<asp:RadioButton id="optSelectOne" runat="server" Text=" " GroupName="optComVen" AutoPostBack="True"></asp:RadioButton>
						<asp:dropdownlist id="cboCompany" runat="server" Width="120px" CssClass="txtbox"></asp:dropdownlist>
					</TD>
				</TR>
				</TR>
				<TR>
					<TD class="tablecol" width="20%" nowrap>&nbsp;<STRONG>Start Date</STRONG><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;
						</STRONG>:
					</TD>
					<TD class="TableInput" width="80%">&nbsp;
						<asp:textbox id="txtSDate" runat="server" Width="128px" CssClass="txtbox" MaxLength="10" contentEditable="false" ></asp:textbox><% response.write (sStartDt) %>
						<asp:requiredfieldvalidator id="ValSDate" runat="server" ErrorMessage="Start Date is required." ControlToValidate="txtSDate"
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD class="tablecol">&nbsp;<STRONG>End Date</STRONG><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput">&nbsp;
						<asp:textbox id="txtEndDate" runat="server" Width="128px" CssClass="txtbox" MaxLength="50" contentEditable="false" ></asp:textbox><% response.write (sEndDt) %>
						<asp:requiredfieldvalidator id="ValEDate" runat="server" ErrorMessage="End Date is required." ControlToValidate="txtEndDate"
							Display="None"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="cvDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="Start Date should be <= End Date."
							ControlToCompare="txtSDate" Operator="GreaterThanEqual" Type="Date" Display="None"></asp:comparevalidator></TD>
				</TR>
				<TR>
					<TD colspan="2"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field
					</TD>
				</TR>
				<tr>
					<td class="emptycol" colspan="2"></td>
				</tr>
				<tr>
					<td colspan="2"><asp:button id="cmdSubmit" runat="server" Text="Submit" CssClass="button"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" type="button" value="Clear" onclick="ValidatorReset();"
							runat="server"></td>
				</tr>
				<TR>
					<TD colspan="2"><BR>
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="2">
					    <asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="#"><STRONG>&lt; Back</STRONG></asp:hyperlink>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
