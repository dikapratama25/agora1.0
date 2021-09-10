<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AsgAppOfficer.aspx.vb" Inherits="eProcure.AsgAppOfficer" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>AsgAppOfficer</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim AutoCompleteCSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "AutoComplete.css") & """ rel='stylesheet'>"
        </script> 
        <% Response.Write(AutoCompleteCSS)%>		
        <% Response.Write(Session("JQuery")) %> 
        <% Response.Write(Session("AutoComplete")) %>
        <% Response.write(Session("WheelScript"))%>
        <% Response.write(Session("typeahead")) %>
        
		<script type="text/javascript">
		<!--
			function Close()
			{
	            window.close();
	         }

		function check(){
			var change = document.getElementById("hidchk");
			change.value ="1";
			}
		-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" cellspacing="0" cellpadding="0" border="0" width="100%">
				<tr>
					<td class="header" colspan="4">Assign Approving Officer<input id="hidchk" type="hidden" runat="server"></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">
						<asp:label id="lblDisAppGroup" visible="false" runat="server" Font-Bold="True"></asp:label></td>
				</tr>
				<tr class="tableheader">
					<td colspan="4">&nbsp;<asp:Label id="lblTitle" runat="server">Select Approving Officer</asp:Label></td>
				</tr>
				<tr>
					<td class="tablecol" width="15%" >&nbsp;<strong>Level</strong><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:
					</td>
					<td class="tableinput" >&nbsp;<asp:textbox id="txtLevel" runat="server" CssClass="txtbox" MaxLength="20" ></asp:textbox></td>
					<td class="tableinput" ><asp:requiredfieldvalidator id="validate_Level" runat="server" ControlToValidate="txtLevel" ErrorMessage="Level is expecting numeric value."
							Display="None"></asp:requiredfieldvalidator></td>
					<td class="tableinput"></td>
				</tr>
				<tr>
					<td class="tablecol" nowrap>&nbsp;<strong>Approving Officer</strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					<td class="tableinput" width="45%" >&nbsp;<asp:dropdownlist id="cboAO" runat="server" CssClass="txtbox" AutoPostBack="True" width="100%" ></asp:dropdownlist></td>
					<td class="tableinput" ><asp:checkbox id="chkmass" runat="server" Enabled="False" Text="Authorised to do Mass Approval"></asp:checkbox></td>
					<td class="tableinput"><asp:rangevalidator id="RangeValidator1" runat="server" ControlToValidate="txtLevel" ErrorMessage="Numeric Only"
							MinimumValue="1" MaximumValue="99" Display="None" Type="Integer"></asp:rangevalidator><asp:requiredfieldvalidator id="validate_AO" runat="server" ControlToValidate="cboAo" ErrorMessage="Approving Officer Required"
							Display="None"></asp:requiredfieldvalidator></td>
				</tr>
				<tr id="tr1" style="display:none;" runat="server">
				    <td class="tablecol">&nbsp;<strong>Branch Code</strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:
					</td>
					<td class="tableinput">&nbsp;<asp:textbox id="txtBranchCode" runat="server" CssClass="txtbox" MaxLength="20" ></asp:textbox></td>
					<td class="tableinput"><asp:requiredfieldvalidator id="validate_BranchCode" runat="server" ControlToValidate="txtBranchCode" ErrorMessage="Branch Code is required."
							Display="None" enabled="false"></asp:requiredfieldvalidator></td>
					<td class="tableinput"></td>
				</tr>
				<tr id="tr2" style="display:none;" runat="server">
				    <td class="tablecol">&nbsp;<strong>Cost Center</strong><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:
					</td>
					<td class="tableinput">&nbsp;<asp:textbox id="txtCostCenter" runat="server" CssClass="txtbox" MaxLength="20" ></asp:textbox></td>
					<td class="tableinput"><asp:requiredfieldvalidator id="validate_CostCenter" runat="server" ControlToValidate="txtCostCenter" ErrorMessage="Cost Center is required."
							Display="None" enabled="false"></asp:requiredfieldvalidator></td>
					<td class="tableinput"></td>
				</tr>
				<tr id="hidAAO">
					<td class="tablecol" colspan="5" valign="top" >
						<table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
							<tr>
					<td width="20%" class="tablecol" nowrap>&nbsp;<strong>Alternative Approving Officer</strong> :&nbsp;&nbsp;&nbsp;</td>
					<td class="tableinput" width="226px"><asp:dropdownlist id="cboAAO" runat="server" CssClass="txtbox" Width="100%"></asp:dropdownlist></td>
					<td width="27%" class="tableinput" >&nbsp;
								<asp:Label id="lblRelief" runat="server" ><strong>Relief Staff Control</strong></asp:Label>
					</td>
					<td width="8%" class="tableinput"><asp:radiobuttonlist id="rdRelief" runat="server" Height="0px" RepeatDirection="Horizontal" AutoPostBack="True"  >
							<asp:ListItem Value="O" Selected="True">Open</asp:ListItem>
							<asp:ListItem Value="C">Controlled</asp:ListItem>
						</asp:radiobuttonlist></td>
				</tr>
				
				</table>
				</td>
				</tr>
				<tr id="hidAAO2" style = "display:none" runat="server">
					<td class="tablecol" colspan="5" valign="top" >
						<table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
							<tr>
					<td width="20%" class="tablecol" nowrap>&nbsp;<strong>Alternative Approving Officer 2</strong>&nbsp;:</td>
					<td class="tableinput" width="226px"><asp:dropdownlist id="cboAAO2" runat="server" CssClass="txtbox" Width="100%"></asp:dropdownlist></td>
					
				</tr>
				</table>
				</td>
				</tr>
				<tr id="hidAAO3" style = "display:none" runat="server">
					<td class="tablecol" colspan="5" valign="top" >
						<table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
							<tr>
					<td width="20%" class="tablecol" nowrap>&nbsp;<strong>Alternative Approving Officer 3</strong>&nbsp;:</td>
					<td class="tableinput" width="226px"><asp:dropdownlist id="cboAAO3" runat="server" CssClass="txtbox" Width="100%"></asp:dropdownlist></td>
					
				</tr>
				</table>
				</td>
				</tr>
			  <tr id="hidAAO4" style = "display:none" runat="server">
					<td class="tablecol" colspan="5" valign="top" >
						<table id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
							<tr>
					<td width="20%" class="tablecol" nowrap>&nbsp;<strong>Alternative Approving Officer 4</strong>&nbsp;:</td>
					<td class="tableinput" width="226px"><asp:dropdownlist id="cboAAO4" runat="server" CssClass="txtbox" Width="100%"></asp:dropdownlist></td>
					
				</tr>
				</table>
				</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4"><asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">
						<asp:button id="cmdsave" runat="server" CssClass="button" Text="Save"></asp:button>
						<asp:button id="cmd_delete" runat="server" CssClass="button" Text="Delete" Visible="False"></asp:button>
						<asp:button id="cmd_reset" runat="server" CssClass="button" Text="Reset" CausesValidation="False"></asp:button>
                        <input class="button" id="cmdClose" onclick="Close();" type="button" value="Close"/ >
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">&nbsp;</td>
				</tr>
				<tr>
					<td colspan="4"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" Width="392px"></asp:validationsummary>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">&nbsp;</td>
				</tr>
			</table>
		</form>
	</body>
</html>
