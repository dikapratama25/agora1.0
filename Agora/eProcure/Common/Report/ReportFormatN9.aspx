<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportFormatN9.aspx.vb" Inherits="eProcure.ReportFormatN9" %>

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
		     Dim dt As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDate") & "','cal','width=180,height=155,left=270,top=180')""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " width=""16""></A>"   
		     Dim dt2 As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtSDate") & "','cal','width=180,height=155,left=270,top=180')""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " width=""16""></A>"
		     Dim dt3 As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEDate") & "','cal','width=180,height=155,left=270,top=180')""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " width=""16""></A>" 
		     Dim dt4 As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtStartDate") & "','cal','width=180,height=155,left=270,top=180')""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " width=""16""></A>"
		     Dim dt5 As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEndDate") & "','cal','width=180,height=155,left=270,top=180')""><IMG style=""CURSOR: hand"" height=""16"" src=" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & " width=""16""></A>" 
		     Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
		     Dim CSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "AutoComplete.css") & """ rel='stylesheet'>"
		     Dim typeahead As String = dDispatcher.direct("Initial", "TypeAhead.aspx", "from=GLCodeOnly") 
         </script>
         <% Response.Write(Session("JQuery")) %>	
         <% Response.Write(sCSS) %>
         <% Response.Write(CSS) %>
		 <%Response.Write(Session("WheelScript"))%>
		 <% Response.Write(Session("AutoComplete")) %>
         <script language="javascript">
		     
		    $(document).ready(function(){
            $("#txtFromGLCode").autocomplete("<% Response.write(typeahead) %>", {
            width: 200,
            scroll: true,
            selectFirst: false
            });   
            
            $("#txtFromGLCode").result(function(event, data, formatted) {
            if (data)
            $("#hidFromGLCode").val(data[1]);
            });      
            
            $("#txtToGLCode").autocomplete("<% Response.write(typeahead) %>", {
            width: 200,
            scroll: true,
            selectFirst: false
            });   
            
            $("#txtToGLCode").result(function(event, data, formatted) {
            if (data)
            $("#hidToGLCode").val(data[1]);
            });                  
            });
            //Jules 2015.01.30 IPP Stage 2A
            function PopWindow(myLoc)
		    {
    			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			    return false;
		    }	

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
				    <TD class="EmptyCol" colspan="2">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Microsoft Excel is required in order to open the report in Excel format."
					    ></asp:label>

				    </TD>
			    </TR>
			    <tr><TD class="emptycol" colSpan="2"></TD></tr>
				<TR>
					<TD class="tableheader" colSpan="2"><FONT size="1">&nbsp;</FONT><asp:label id="label2" runat="server">Report Criteria</asp:label></TD>
				</TR>	
				<TR runat="server" id="trDailyGL">
					<TD class="tablecol" colspan="1" width="20%" nowrap>&nbsp;<STRONG><asp:label id="lblCriteria" runat="server" Text="As At Date"></asp:label></STRONG><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG></STRONG></TD>
					<TD class="TableInput" width="80%">&nbsp;
						<asp:textbox id="txtDate" runat="server" CssClass="txtbox" MaxLength="10"  contentEditable="false" ></asp:textbox><% response.write (dt) %>
						<%--<asp:requiredfieldvalidator id="ValDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Date is required." Display="None">
						</asp:requiredfieldvalidator>		--%>				
					</TD>
				</TR>	
				<TR runat="server" id="trStatus" style="display:none">
					<TD class="tablecol" colspan="1" width="20%" nowrap>&nbsp;<STRONG><asp:label id="Label6" runat="server" Text="Status"></asp:label></STRONG><STRONG>&nbsp;</STRONG>:<STRONG></STRONG></TD>
					<TD class="TableInput" width="80%">&nbsp;
                        <asp:CheckBox ID="CheckBox1" runat="server" Text="Active" Checked="true" Width="60px" />
                        <asp:CheckBox ID="CheckBox2" runat="server" Text="Inactive"/>
					</TD>
				</TR>
				<TR runat="server" id="trGLCode" style="display:none">
					<TD class="tablecol" colspan="1" width="20%" nowrap>&nbsp;<STRONG><asp:label id="Label20" runat="server" Text="From GL Code"></asp:label></STRONG><STRONG>&nbsp;</STRONG>:<STRONG></STRONG></TD>
					<TD class="TableInput" width="80%" style="height: 24px">&nbsp;
						<asp:textbox id="txtFromGLCode" runat="server" CssClass="txtbox" ></asp:textbox>
						<asp:TextBox id="hidFromGLCode" runat="server" style="display: none"></asp:TextBox>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<STRONG><asp:label id="Label22" runat="server" Text="To GL Code"></asp:label></STRONG><STRONG>&nbsp;</STRONG>:&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:textbox id="txtToGLCode" runat="server" CssClass="txtbox" ></asp:textbox>
						<asp:TextBox id="hidToGLCode" runat="server" style="display: none"></asp:TextBox>
						<%--<asp:requiredfieldvalidator id="ValDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Date is required." Display="None">
						</asp:requiredfieldvalidator>		--%>				
					</TD>
				</TR>
				<TR runat="server" id="trCode" style="display:none">
					<TD class="tablecol" colspan="1" width="20%" nowrap>&nbsp;<STRONG><asp:label id="Label14" runat="server" Text="Branch Code"></asp:label></STRONG><STRONG>&nbsp;</STRONG>:<STRONG></STRONG></TD>
					<TD class="TableInput" width="80%" style="height: 24px">&nbsp;
						<asp:textbox id="txtBranchCode" runat="server" CssClass="txtbox" ></asp:textbox>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<STRONG><asp:label id="Label16" runat="server" Text="Cost Centre Code"></asp:label></STRONG><STRONG>&nbsp;</STRONG>:&nbsp;
                        &nbsp;
						<asp:textbox id="txtCentreCode" runat="server" CssClass="txtbox" ></asp:textbox>
						<%--<asp:requiredfieldvalidator id="ValDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Date is required." Display="None">
						</asp:requiredfieldvalidator>		--%>				
					</TD>
				</TR>
				<TR runat="server" id="trDate" style="display:none">
					<TD class="tablecol" colspan="1" width="20%" nowrap>&nbsp;<STRONG><asp:label id="Label8" runat="server" Text="Payment Start Date"></asp:label><asp:label id="Label15" runat="server" CssClass="errormsg">*</asp:label></STRONG><STRONG>&nbsp;</STRONG>:<STRONG></STRONG></TD>
					<TD class="TableInput" width="80%" style="height: 24px">&nbsp;
						<asp:textbox id="txtSDate" runat="server" CssClass="txtbox" MaxLength="10"  contentEditable="false" ></asp:textbox><% response.write (dt2) %>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<STRONG><asp:label id="Label11" runat="server" Text="Payment End Date"></asp:label><asp:label id="Label17" runat="server" CssClass="errormsg">*</asp:label></STRONG><STRONG>&nbsp;</STRONG>:&nbsp;
                        &nbsp;
                        <asp:textbox id="txtEDate" runat="server" CssClass="txtbox" MaxLength="10"  contentEditable="false" ></asp:textbox><% response.write (dt3) %>
						<%--<asp:requiredfieldvalidator id="ValDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Date is required." Display="None">
						</asp:requiredfieldvalidator>		--%>				
					</TD>
				</TR>
				<TR runat="server" id="trInvPendingApp" style="display:none">
					<TD class="tablecol" colspan="1" width="20%" nowrap>&nbsp;<STRONG><asp:label id="Label1" runat="server" Text="Finance Officer"></asp:label></STRONG><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG></STRONG></TD>
					<TD class="TableInput" width="80%">&nbsp;
						<asp:dropdownlist id="ddlFO" runat="server" CssClass="txtbox" Width="200px"></asp:dropdownlist>
					</TD>
				</TR>
				<TR runat="server" id="trPANo" style="display:none">
					<TD class="tablecol" colspan="1" width="20%" nowrap>&nbsp;<STRONG><asp:label id="Label9" runat="server" Text="Payment Advice No."></asp:label></STRONG><STRONG>&nbsp;</STRONG>:<STRONG></STRONG></TD>
					<TD class="TableInput" width="80%">&nbsp;
                        <asp:TextBox ID="txtBoxPA" runat="server" CssClass="txtbox" ></asp:TextBox>
					</TD>
				</TR>	
				<TR runat="server" id="trTaxInvoice" style="display:none">
					<TD class="tablecol" colspan="1" width="20%" nowrap>&nbsp;<STRONG><asp:label id="Label19" runat="server" Text="Tax Invoice No."></asp:label></STRONG><STRONG>&nbsp;</STRONG>:<STRONG></STRONG></TD>
					<TD class="TableInput" width="80%">&nbsp;
                        <asp:TextBox ID="txtTaxInvoice" runat="server" CssClass="txtbox" ></asp:TextBox>
					</TD>
				</TR>
				<TR runat="server" id="trBCNo" style="display:none">
					<TD class="tablecol" colspan="1" width="20%" nowrap>&nbsp;<STRONG><asp:label id="Label13" runat="server" Text="Bankers Cheque No."></asp:label></STRONG><STRONG>&nbsp;</STRONG>:<STRONG></STRONG></TD>
					<TD class="TableInput" width="80%">&nbsp;
                        <asp:TextBox ID="txtBoxBC" runat="server" CssClass="txtbox" ></asp:TextBox>
					</TD>
				</TR>
				<TR runat="server" id="trDebitNoteNo" style="display:none">
					<TD class="tablecol" colspan="1" width="20%" nowrap>&nbsp;<STRONG><asp:label id="Label12" runat="server" Text="Debit Note No."></asp:label></STRONG><STRONG>&nbsp;</STRONG>:<STRONG></STRONG></TD>
					<TD class="TableInput" width="80%">&nbsp;
                        <asp:TextBox ID="txtDebitNoteNo" runat="server" CssClass="txtbox" ></asp:TextBox>
					</TD>
				</TR>
				<TR runat="server" id="trType" style="display:none">
					<TD class="tablecol" colspan="1" width="20%" nowrap>&nbsp;<STRONG><asp:label id="Label18" runat="server" Text="Type"></asp:label></STRONG><asp:label id="Label29" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG></STRONG></TD>
					<TD class="TableInput" width="80%">&nbsp;
						<asp:RadioButton ID="rdbtnVendor" runat="server" GroupName="criteria" Text="Vendor"></asp:RadioButton>
						<asp:RadioButton ID="rdbtnEmployee" runat="server" GroupName="criteria" Text="Employee"></asp:RadioButton>
					</TD>
				</TR>
				<TR runat="server" id="trStartandEndDate" style="display:none">
					<TD class="tablecol" colspan="1" width="20%" nowrap>&nbsp;<STRONG><asp:label id="Label0" runat="server" Text="Start Date"></asp:label><asp:label id="Label21" runat="server" CssClass="errormsg">*</asp:label></STRONG><STRONG>&nbsp;</STRONG>:<STRONG></STRONG></TD>
					<TD class="TableInput" width="80%" style="height: 24px">&nbsp;
						<asp:textbox id="txtStartDate" runat="server" CssClass="txtbox" MaxLength="10"  contentEditable="false" ></asp:textbox><% response.write (dt4) %>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<STRONG><asp:label id="Label23" runat="server" Text="End Date"></asp:label><asp:label id="Label24" runat="server" CssClass="errormsg">*</asp:label></STRONG><STRONG>&nbsp;</STRONG>:&nbsp;
                        &nbsp;
                        <asp:textbox id="txtEndDate" runat="server" CssClass="txtbox" MaxLength="10"  contentEditable="false" ></asp:textbox><% response.write (dt5) %>
						<%--<asp:requiredfieldvalidator id="ValDate" runat="server" ControlToValidate="txtDate" ErrorMessage="Date is required." Display="None">
						</asp:requiredfieldvalidator>		--%>				
					</TD>
				</TR>	
				<TR runat="server" id="trDebitAdviceNo" style="display:none">
					<TD class="tablecol" colspan="1" width="20%" nowrap>&nbsp;<STRONG><asp:label id="Label28" runat="server" Text="Debit Advice No."></asp:label></STRONG><STRONG>&nbsp;</STRONG>:<STRONG></STRONG></TD>
					<TD class="TableInput" width="80%">&nbsp;
                        <asp:TextBox ID="txtDebitAdviceNo" runat="server" CssClass="txtbox" ></asp:TextBox>
					</TD>
				</TR>	
				<TR runat="server" id="trReportType" >
					<TD class="tablecol">&nbsp;<STRONG>Report Type</STRONG><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:<STRONG>
						</STRONG>
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
				<TR>
					<TD class="emptycol" colSpan="2"></TD>
				</TR>
				<TR>
					<TD colSpan="2"><asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" type="button" value="Clear" onclick="ValidatorReset();"
							runat="server" NAME="cmdClear"></TD>
				</TR>				
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