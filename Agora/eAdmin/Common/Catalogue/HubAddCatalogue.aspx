<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HubAddCatalogue.aspx.vb" Inherits="eAdmin.HubAddCatalogue" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Contract Catalogue</title>
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
		
			function clearEndDate()
			{
				Form1.txtEndDate.value = "";
			}
			/*function resetForm()
			{
				Form1.reset();
				ValidatorReset();
			}
			
			function ValidatorReset() {			
				for (i = 0; i < Page_Validators.length; i++) {		
					Page_Validators[i].isvalid = true;	
				}
				ValidationSummaryOnSubmit()	
			}*/
		
			function CheckDeleteMaster(pChkSelName){
				var oform = document.forms[0];
				var itemCnt, itemCheckCnt;
				itemCnt = 0;
				itemCheckCnt = 0;
				
				re = new RegExp(':' + pChkSelName + '$');  //generated control name starts with a colon	
				for (var i=0;i<oform.elements.length;i++){
					var e = oform.elements[i];
					if (e.type=="checkbox"){						
						if (re.test(e.name)){
							itemCnt ++;
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
						var result = confirm('Are you sure that you want to permanently delete this item(s) ?');
						if (result == true){
							var result2 = confirm('Delete Master record too ?');
							if (result2 == true) 
								Form1.hidDelete.value = "1";								
							else
								Form1.hidDelete.value = "0";
							return true;
						}
						else
							return false;
					}
					else {
						Form1.hidDelete.value = "0";
						return confirm('Are you sure that you want to permanently delete this item(s) ?');
					}
				}				
			}

		-->
		</script>
	</HEAD>
	<body onload="" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="TableHeader" colSpan="2">&nbsp;<asp:Label id="lblHeader" runat="server"></asp:Label></TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="left">
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR id="trCode" vAlign="top" runat="server">
								<TD class="tablecol" noWrap width="20%">&nbsp;<STRONG><asp:label id="lblCodeLabel" runat="server"></asp:label><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="80%"><asp:textbox id="txtCode" runat="server" CssClass="txtbox" MaxLength="30" Rows="1" Width="220px"></asp:textbox><asp:requiredfieldvalidator id="revCode" runat="server" Display="None" ErrorMessage="Discount Group Code is required"
										ControlToValidate="txtCode"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR id="trDate" vAlign="top" runat="server">
								<TD class="tablecol" noWrap>&nbsp;<STRONG>Description<asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
								<TD class="TableInput"><asp:textbox id="txtDesc" runat="server" CssClass="txtbox" MaxLength="100" Width="300px"></asp:textbox><asp:requiredfieldvalidator id="revDesc" runat="server" Display="None" ErrorMessage="Description is required"
										ControlToValidate="txtDesc"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR id="trCodeRead" vAlign="top" runat="server">
								<TD class="tablecol" noWrap>&nbsp;<STRONG>Start Date<asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
								<TD class="TableInput"><asp:textbox id="txtStartDate" runat="server" CssClass="txtbox" MaxLength="50" contentEditable="false" ></asp:textbox><% Response.Write(calStartDate)%> 
									<asp:requiredfieldvalidator id="revStartDate" runat="server" Display="None" ErrorMessage="Start Date is required"
										ControlToValidate="txtStartDate"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR id="trDateRead" vAlign="top" runat="server">
								<TD class="tablecol" noWrap>&nbsp;<STRONG>End Date<asp:label id="lblEndDateMsg" runat="server" CssClass="errormsg">*</asp:label>
										&nbsp;</STRONG>:</TD>
								<TD class="TableInput"><asp:textbox id="txtEndDate" runat="server" CssClass="txtbox" MaxLength="50" contentEditable="false" ></asp:textbox><% Response.Write(calEndDate)%><asp:Label id="lblClear" runat="server"></asp:Label>
									<asp:requiredfieldvalidator id="revEndDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="End Date id required"
										Display="None"></asp:requiredfieldvalidator><asp:comparevalidator id="cvDate" runat="server" ErrorMessage="Start Date should be < End Date." ControlToValidate="txtEndDate"
										Type="Date" Operator="GreaterThan" ControlToCompare="txtStartDate" Display="None"></asp:comparevalidator>
									<asp:comparevalidator id="cvDateNow" runat="server" ControlToValidate="txtEndDate" ErrorMessage="End Date should be >= today's date."
										Display="None" Operator="GreaterThanEqual" Type="Date"></asp:comparevalidator></TD>
							</TR>
							<TR id="trBuyer" vAlign="top" runat="server">
								<TD class="tablecol" noWrap>&nbsp;<STRONG>Buyer Company<asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:</TD>
								<TD class="TableInput"><asp:dropdownlist id="cboBuyer" runat="server" CssClass="ddl"></asp:dropdownlist><asp:requiredfieldvalidator id="revBuyer" runat="server" Display="None" ErrorMessage="Buyer Company Required"
										ControlToValidate="cboBuyer"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR class="emptycol">
								<TD colspan="2"><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
									required field</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD>&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:button id="cmdSave" runat="server" CssClass="Button" Text="Save"></asp:button>
						<asp:button id="cmdCompany" runat="server" CssClass="Button" Width="128px" Text="Company Assignment"></asp:button>
						<asp:button id="cmdItem" runat="server" CssClass="Button" Width="120px" Text="Contract Group Item"
							CausesValidation="False"></asp:button>
						<asp:button id="cmdDelete" runat="server" CssClass="Button" Text="Delete" CausesValidation="False"></asp:button>
						<INPUT class="button" id="cmdReset" type="button" value="Reset" name="cmdReset" runat="server"
							onclick="ValidatorReset()"> <INPUT class="txtbox" id="hidDelete" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidDelete" runat="server">
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 18px"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary></TD>
				</TR>
				<TR>
					<TD><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
