<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportFormat11.aspx.vb" Inherits="eProcure.ReportFormat11"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Report</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"

            Dim sStartDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtSDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"
            Dim sEndDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEndDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"
	    </script>
	    <%Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
				/*function Verifycheck(a){
					if (a==1)
						Form1.optSelectOne1.checked = false;
						
						document.getElementById("cboCompany").e="";
						
					else 
						Form1.optCom1.checked = false;
				}*/
				
				
						
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="HEIGHT: 16px" colSpan="2"><FONT size="1">&nbsp;</FONT><asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"></TD>
				</TR>
				<TR>
					<TD class="tableheader" colSpan="2"><FONT size="1">&nbsp;</FONT><asp:label id="label1" runat="server">Report Criteria:</asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" noWrap width="20%"><asp:label id="Label3" runat="server">&nbsp;<STRONG>Sort 
								By </STRONG>:
						</asp:label>&nbsp;</TD>
					<TD class="TableInput" width="80%"><asp:radiobutton id="optPr_Po_No" runat="server" AutoPostBack="True" Text="PR/PO Number" GroupName="SortBy"></asp:radiobutton><asp:radiobutton id="optPr_status" runat="server" AutoPostBack="True" Text="PR Status" GroupName="SortBy"></asp:radiobutton></TD>
				</TR>
				<TR>
					<TD class="tablecol" style="HEIGHT: 29px">&nbsp;<STRONG>Company Name</STRONG><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput" style="HEIGHT: 29px" colSpan="1">&nbsp;
						<asp:dropdownlist id="cboCompy" runat="server" CssClass="txtbox"></asp:dropdownlist><asp:requiredfieldvalidator id="ValCompy" runat="server" Display="None" ControlToValidate="cboCompy" ErrorMessage="Name of the Company is required."></asp:requiredfieldvalidator></TD>
				</TR>
			
				<TR>
					<TD class="tablecol" noWrap width="20%">&nbsp;<STRONG>Start Date</STRONG><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;
						</STRONG>:
					</TD>
					<TD class="TableInput" width="80%">&nbsp;
						<asp:textbox id="txtSDate" runat="server" CssClass="txtbox" Width="128px"  contentEditable="false"  MaxLength="10"></asp:textbox><% response.write (sStartDt) %>
						<asp:requiredfieldvalidator id="ValSDate" runat="server" Display="None" ControlToValidate="txtSDate" ErrorMessage="Start Date is required."></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD class="tablecol">&nbsp;<STRONG>End Date</STRONG><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
					</TD>
					<TD class="TableInput">&nbsp;
						<asp:textbox id="txtEndDate" runat="server" CssClass="txtbox" Width="128px"  contentEditable="false"  MaxLength="50"></asp:textbox><% response.write (sEndDt) %>
						<asp:requiredfieldvalidator id="ValEDate" runat="server" Display="None" ControlToValidate="txtEndDate" ErrorMessage="End Date is required."></asp:requiredfieldvalidator><asp:comparevalidator id="cvDate" runat="server" Display="None" ControlToValidate="txtEndDate" ErrorMessage="Start Date should be <= End Date."
							Type="Date" Operator="GreaterThanEqual" ControlToCompare="txtSDate"></asp:comparevalidator></TD>
				</TR>
				<TR>
					<TD class="tablecol" colspan="1" width="10%">&nbsp;<STRONG><asp:label id="Label6" runat="server" Font-Bold="True">Report Type</asp:label></STRONG>&nbsp;:
					</TD>
					<TD class="TableInput" style="HEIGHT: 17px" colSpan="1">&nbsp;
						<asp:dropdownlist id="cboReportType" runat="server" CssClass="txtbox" Width="128px">
                            <asp:ListItem Selected="True">Excel</asp:ListItem>
                            <asp:ListItem>PDF</asp:ListItem>
                        </asp:dropdownlist><asp:requiredfieldvalidator id="ValReportType" runat="server" ErrorMessage="Report Type is required." ControlToValidate="cboReportType"
							Display="None"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD colSpan="2"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field
					</TD>
				</TR>
				<tr>
					<td class="emptycol" colSpan="2"></td>
				</tr>
				<tr>
					<td colSpan="2"><asp:button id="cmdSubmit" runat="server" Text="Submit" CssClass="button"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="ValidatorReset();" type="button" value="Clear"
							name="cmdClear" runat="server"></td>
				</tr>
				<TR>
					<TD colSpan="2"><BR>
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
