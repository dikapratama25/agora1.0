<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportFormatSEH1.aspx.vb" Inherits="eProcure.ReportFormatSEH1" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
  <head>
		<title>Report</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
		     Dim dDispatcher As New AgoraLegacy.dispatcher
		     Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
		     
		     Dim sStartDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtSDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"
             Dim sEndDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEndDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"
             Dim sGRNStartDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtGRNSDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"
             Dim sGRNEndDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtGRNEndDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"
             Dim sDt As String = "<A style=""CURSOR: hand"" onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtDate") & "','cal','width=190,height=165,left=270,top=180')""><IMG src=""" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & """</A>"
            
		</script>
		<% Response.Write(sCSS) %>
		<%Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		function compareDates()
		{
		    var index1, index2, index3, index4;
		    var y1 = document.getElementById("cmbYearFrom");
		    var y2 = document.getElementById("cmbYearTo");
		    var m1 = document.getElementById("cmbMonthFrom");
		    var m2 = document.getElementById("cmbMonthTo");
		    index1 = parseInt(y1.options[y1.selectedIndex].value);
		    index2 = parseInt(y2.options[y2.selectedIndex].value);
		    index3 = parseInt(m1.options[m1.selectedIndex].value);
		    index4 = parseInt(m2.options[m2.selectedIndex].value);		    
		   
		    if (index1 > index2)
		    {
		        alert('Year From cannot be greater than Year To.');
		        return false;
		    }
		    else 
		    {   
		        if (index1==index2)
		        {
		            if (index3 > index4)
		            {
		                alert('Month From cannot be greater than Month To.');
		                return false;
		            }       
		        }   
		    }
		}
		</script>
  </head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td class="header" style="HEIGHT: 16px" colspan="2"><font size="1">&nbsp;</font><asp:label id="lblHeader" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4"></td>
				</tr>
				<tr>
				    <td class="EmptyCol" colspan="4">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Microsoft Excel is required in order to open the report in Excel format."></asp:label>
				    </td>
			    </tr>
			    <tr><td class="emptycol" colspan="4"></td></tr>
				<tr>
					<td class="tableheader" colspan="4"><font size="1">&nbsp;</font><asp:label id="label2" runat="server">Report Criteria</asp:label></td>
				</tr>
				<tr id="tr_1" runat="server" style="display:none">
					<td class="tablecol" width="20%" nowrap>&nbsp;<strong>Month From</strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
                        &nbsp;<asp:dropdownlist id="cmbMonthFrom" runat="server" CssClass="ddl" Width="80px"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvMonthFrom" runat="server" ErrorMessage="Month From is required."
							ControlToValidate="cmbMonthFrom" Display="None"></asp:requiredfieldvalidator></td>
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Year From</strong><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
						<asp:dropdownlist id="cmbYearFrom" runat="server" CssClass="ddl" Width="80px"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvYearFrom" runat="server" ErrorMessage="Year From is required." ControlToValidate="cmbYearFrom"
							Display="None"></asp:requiredfieldvalidator></td>
				</tr>
				<tr id="tr_2" runat="server" style="display:none">
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Month To</strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
                        &nbsp;<asp:dropdownlist id="cmbMonthTo" runat="server" CssClass="ddl" Width="80px"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvMonthTo" runat="server" ErrorMessage="Month To is required." ControlToValidate="cmbMonthTo"
							Display="None"></asp:requiredfieldvalidator></td>
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Year To</strong><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
						<asp:dropdownlist id="cmbYearTo" runat="server" CssClass="ddl" Width="80px"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvYearTo" runat="server" ErrorMessage="Year To is required." ControlToValidate="cmbYearTo"
							Display="None"></asp:requiredfieldvalidator></td>
				</tr>
				<tr id="tr_10" runat="server" style="display:none">
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Search By</strong><asp:label id="Label18" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
				    <td colspan="3" class="TableInput" width="80%" nowrap style="height: 20px">
				        &nbsp;<asp:radiobuttonlist ID="dtRadioBtn" runat="server" BorderStyle="None" CssClass="rbtn" RepeatDirection="Horizontal"> 
                            <asp:ListItem Value="DO" Selected="True">DO Date</asp:ListItem>
							<asp:ListItem Value="INV">Invoice Date</asp:ListItem>
						</asp:radiobuttonlist>
					</td>
				</tr>
				<tr id="tr_3" runat="server" style="display:none">
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Start Date</strong><asp:label id="Label8" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
                        &nbsp;<asp:textbox id="txtSDate" runat="server" CssClass="txtbox" MaxLength="10" contentEditable="false"></asp:textbox><% response.write (sStartDt) %>
						<asp:requiredfieldvalidator id="ValSDate" runat="server" ControlToValidate="txtSDate" ErrorMessage="Start Date is required."
							Display="None"></asp:requiredfieldvalidator>
					</td>
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>End Date</strong><asp:label id="Label9" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
						<asp:textbox id="txtEndDate" runat="server" CssClass="txtbox" MaxLength="10" contentEditable="false" ></asp:textbox><% response.write (sEndDt) %>
						<asp:requiredfieldvalidator id="ValEDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="End Date is required."
							Display="None"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="cvDate" runat="server" ErrorMessage="Start Date should be <= End Date." ControlToValidate="txtEndDate"
							Display="None" Type="Date" Operator="GreaterThanEqual" ControlToCompare="txtSDate"></asp:comparevalidator>
					</td>
				</tr>
				<tr id="tr_14" runat="server" style="display:none">
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>GRN Start Date</strong><asp:label id="Label15" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
                        &nbsp;<asp:textbox id="txtGRNSDate" runat="server" CssClass="txtbox" MaxLength="10" contentEditable="false"></asp:textbox><% response.write (sGRNStartDt) %>
						<asp:requiredfieldvalidator id="ValGRNSDate" runat="server" ControlToValidate="txtGRNSDate" ErrorMessage="GRN Start Date is required."
							Display="None"></asp:requiredfieldvalidator>
					</td>
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>GRN End Date</strong><asp:label id="Label16" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
						<asp:textbox id="txtGRNEndDate" runat="server" CssClass="txtbox" MaxLength="10" contentEditable="false" ></asp:textbox><% response.write (sGRNEndDt) %>
						<asp:requiredfieldvalidator id="ValGRNEDate" runat="server" ControlToValidate="txtGRNEndDate" ErrorMessage="GRN End Date is required."
							Display="None"></asp:requiredfieldvalidator>
						<asp:comparevalidator id="cvGRNDate" runat="server" ErrorMessage="GRN Start Date should be <= GRN End Date." ControlToValidate="txtGRNEndDate"
							Display="None" Type="Date" Operator="GreaterThanEqual" ControlToCompare="txtGRNSDate"></asp:comparevalidator>
					</td>
				</tr>
				<tr id="tr_4" runat="server" style="display:none">
				    <td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Month</strong><asp:label id="Label10" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>    
				    <td class="TableInput" width="30%" nowrap style="height: 20px">
				        &nbsp;<asp:dropdownlist id="cmbMonth" runat="server" CssClass="ddl" Width="80px"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvMonth" runat="server" ErrorMessage="Month is required."
				            ControlToValidate="cmbMonth" Display="None"></asp:requiredfieldvalidator></td>
				    <td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Year</strong><asp:label id="Label11" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
				    <td class="TableInput" width="30%" nowrap style="height: 20px">
				        <asp:dropdownlist id="cmbYear" runat="server" CssClass="ddl" Width="80px"></asp:dropdownlist><asp:requiredfieldvalidator id="rfvYear" runat="server" ErrorMessage="Year is required."
				            ControlToValidate="cmbYear" Display="None"></asp:requiredfieldvalidator></td>
				</tr>
				<tr id="tr_6" runat="server" style="display:none">
				    <td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>End Date</strong><asp:label id="Label13" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
				    <td colspan="3" class="TableInput" width="80%" nowrap style="height: 20px" >
				        &nbsp;<asp:textbox id="txtDate" runat="server" CssClass="txtbox" MaxLength="10" contentEditable="false"></asp:textbox><% response.write (sDt) %>
				        <asp:requiredfieldvalidator id="ValDate" runat="server" ControlToValidate="txtDate" ErrorMessage="End Date is required."
							Display="None"></asp:requiredfieldvalidator>
					</td>
				</tr>
				<tr id="tr_7" runat="server" style="display:none">
				    <td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Department Code</strong><asp:label id="Label14" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
				    <td colspan="3" class="TableInput" width="80%" nowrap style="height: 20px" >
				        &nbsp;<asp:textbox id="txtDptCode" runat="server" CssClass="txtbox" MaxLength="20"></asp:textbox>
				        <asp:requiredfieldvalidator id="ValDptCode" runat="server" ControlToValidate="txtDptCode" ErrorMessage="Department Code is required."
							Display="None"></asp:requiredfieldvalidator>
					</td>
				</tr>
				<tr id="tr_15" runat="server" style="display:none">
				    <td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Department Code</strong><asp:label id="Label17" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
				    <td colspan="3" class="TableInput" width="80%" nowrap style="height: 20px" >
				        &nbsp;<asp:DropDownList ID="ddlDptCode" runat="server" CssClass="ddl"></asp:DropDownList>
				        <asp:requiredfieldvalidator id="ValDptCode2" runat="server" ControlToValidate="ddlDptCode" ErrorMessage="Department Code is required."
							Display="None"></asp:requiredfieldvalidator>
					</td>
				</tr>
				<tr id="tr_8" runat="server" style="display:none">
				    <td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Storekeeper Name</strong>&nbsp;:</td>
				    <td colspan="3" class="TableInput" width="80%" nowrap style="height: 20px">
				        &nbsp;<asp:textbox id="txtSKName" runat="server" CssClass="txtbox" MaxLength="20"></asp:textbox>
					</td>
				</tr>
				<tr id="tr_9" runat="server" style="display:none">
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Item Code</strong>&nbsp;:</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
                        &nbsp;<asp:textbox id="txtItemCode2" runat="server" CssClass="txtbox" MaxLength="100"></asp:textbox>
					</td>
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Section Code</strong>&nbsp;:</td>
					<td class="TableInput"  width="30%" nowrap style="height: 20px">
						<asp:textbox id="txtSectionCode" runat="server" CssClass="txtbox" MaxLength="100"></asp:textbox>
					</td>
				</tr>
				<tr id="tr_11" runat="server" style="display:none">
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>PO Number</strong>&nbsp;:</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
                        &nbsp;<asp:textbox id="txtPONo" runat="server" CssClass="txtbox" MaxLength="10"></asp:textbox>
					</td>
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Supplier Name</strong>&nbsp;:</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
						<asp:textbox id="txtSuppName" runat="server" CssClass="txtbox" MaxLength="10"></asp:textbox>
					</td>
				</tr>
				<tr id="tr_12" runat="server" style="display:none">
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Item Code</strong>&nbsp;:</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
                        &nbsp;<asp:textbox id="txtItemCode" runat="server" CssClass="txtbox" MaxLength="100"></asp:textbox>
					</td>
					<td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Supplier Code</strong>&nbsp;:</td>
					<td class="TableInput" width="30%" nowrap style="height: 20px">
						<asp:textbox id="txtSuppCode" runat="server" CssClass="txtbox" MaxLength="100"></asp:textbox>
					</td>
				</tr>
				<tr id="tr_13" runat="server" style="display:none">
				    <td class="tablecol" style="height: 20px">&nbsp;<strong>PO Balance Qty</strong><asp:label id="Label24" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
				    <td colspan="3" class="TableInput" style="height: 20px">&nbsp;<asp:dropdownlist id="cmbPOBal" runat="server" CssClass="ddl" Width="80px">
				        <asp:ListItem Selected="True" Value="2">All</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:dropdownlist>
					</td>
				</tr>
				<tr id="tr_5" runat="server" style="display:none">
				    <td class="tablecol" width="20%" nowrap style="height: 20px">&nbsp;<strong>Oversea/Local</strong><asp:label id="Label12" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
				    <td colspan="3" class="TableInput" width="80%" nowrap style="height: 20px" >&nbsp;<asp:dropdownlist id="cmbOversea" runat="server" CssClass="ddl" Width="80px">
				        <asp:ListItem Selected="True" Value="Y">Oversea</asp:ListItem>
                        <asp:ListItem Value="N">Local</asp:ListItem>
                        </asp:dropdownlist>
                    </td>
				</tr>
				<tr>
					<td class="tablecol">&nbsp;<strong>Report Type</strong><asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					<td colspan="3" class="TableInput" style="height: 20px" >&nbsp;<asp:dropdownlist id="cboReportType" runat="server" CssClass="ddl" Width="80px">
                        <asp:ListItem Selected="True">Excel</asp:ListItem>
                        <asp:ListItem>PDF</asp:ListItem>
                        </asp:dropdownlist><asp:requiredfieldvalidator id="ValReportType" runat="server" ErrorMessage="Report Type is required." ControlToValidate="cboReportType"
							Display="None"></asp:requiredfieldvalidator>
					</td>
				</tr>
				<tr>
					<td colspan="4"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>indicates 
						required field
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4" style="height: 19px"></td>
				</tr>
				<tr>
					<td colspan="4"><asp:button id="cmdSubmit" runat="server" CssClass="button" Text="Submit"></asp:button>&nbsp;
					<asp:button id="cmdClear" runat="server" CausesValidation="False" CssClass="button" Text="Clear"></asp:button></td>
				</tr>
				<tr>
					<td colspan="4"><br/>
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">
					    <asp:hyperlink id="lnkBack" Runat="server" ForeColor="blue" NavigateUrl="#"><strong>&lt; Back</strong></asp:hyperlink>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
