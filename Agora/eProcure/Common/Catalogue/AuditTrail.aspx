<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AuditTrail.aspx.vb" Inherits="eProcure.AuditTrail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <title>Audit Trail</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            Dim calStartDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateFr") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
            Dim calEndDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDateTo") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"

      </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
</head>
<body class="body" MS_POSITIONING="GridLayout">
    <form id="form1" method="post" runat="server">
    <%  Response.Write(Session("w_ConCat_tabs"))%>
    <TABLE class="alltable" id="Table1" width="100%" cellSpacing="0" cellPadding="0">
        <tr><td class="rowspacing" colspan="6"></td></tr>
        <TR>
			<TD class="EmptyCol" colspan="6">
			    <asp:label id="lblAction1" runat="server" CssClass="lblInfo" Text="Microsoft Excel is required in order to open the report in Excel format. "></asp:label>					    
			</TD>
		</TR>
		<tr><td class="rowspacing"></td></tr>
		<TR>
			<TD class="tableheader" colspan="6">Report Criteria</TD>
		</TR>
		<tr class="tablecol">
		    <TD class="tablecol" width="15%"><STRONG><asp:label id="Label3" runat="server" Text="Start Date " CssClass="lbl"></asp:label></STRONG>:</TD>
		    <TD class="tablecol" width="30%"><asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(calStartDate)%></TD>
		    <TD class="tablecol" width="15%"><STRONG><asp:label id="Label4" runat="server" Text="End Date " CssClass="lbl"></asp:label></STRONG>:</TD>
		    <TD class="tablecol" width="30%"><asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><% Response.Write(calEndDate)%></TD>
	    </tr>
	    <TR class="tablecol">
	        <TD class="tablecol" width="15%"><STRONG>Contract Ref. No.</STRONG><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
			    </STRONG>
		    </TD>
		    <TD class="tablecol" width="30%"><asp:dropdownlist id="ddlCode" runat="server" CssClass="txtbox" Width="250px">                    
                </asp:dropdownlist><asp:requiredfieldvalidator id="valGC" runat="server" ErrorMessage="Contract Ref. No. is required." ControlToValidate="ddlCode"
				    Display="None"></asp:requiredfieldvalidator>
		    </TD>
		    <TD class="tablecol" width="15%"><STRONG>Report Type</STRONG><asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
			    </STRONG>
		    </TD>
		    <TD class="tablecol" width="30%"><asp:dropdownlist id="cboReportType" runat="server" CssClass="txtbox" Width="128px">
                    <asp:ListItem Selected="True">Excel</asp:ListItem>
                <asp:ListItem>PDF</asp:ListItem>
                </asp:dropdownlist><asp:requiredfieldvalidator id="ValReportType" runat="server" ErrorMessage="Report Type is required." ControlToValidate="cboReportType"
				    Display="None"></asp:requiredfieldvalidator>
		    </TD>
	    </TR>
	    <TR class="tablecol">
			<TD class="tablecol" colSpan="4"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>indicates required field
			</TD>
		</TR>
		<TR>
			<TD class="emptycol" colSpan="4"></TD>
		</TR>
		<TR>
			<TD class="emptycol" >
			    <asp:button id="cmdView" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" type="button" value="Clear" onclick="ValidatorReset();"	runat="server" name="cmdClear">
				<asp:comparevalidator id="cvDate" runat="server" Type="Date" Operator="GreaterThanEqual" ControlToCompare="txtDateFr"
							Display="None" ErrorMessage="Start Date should be <= End Date." ControlToValidate="txtDateTo"></asp:comparevalidator>
			</TD>
		</TR>
		<TR>
			<TD class="emptycol" colSpan="4" ><BR>
				<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" Width="600px"></asp:validationsummary>
			</TD>
		</TR>
    </TABLE>
    </form>
</body>
</html>
