<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AsgAppOfficer.aspx.vb" Inherits="eProcure.AsgAppOfficerFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>AsgAppOfficer</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script language="javascript">
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
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<TR>
					<TD class="header" colspan="4">Assign Approving Officer<INPUT id="hidchk" type="hidden" runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="4">
						<asp:label id="lblDisAppGroup" visible="false" runat="server" Font-Bold="True"></asp:label></TD>
				</TR>
				<TR class="tableheader">
					<TD colspan="4">&nbsp;<asp:Label id="lblTitle" runat="server">Select Approving Officer</asp:Label></TD>
				</TR>
				<TR>
					<TD class="tablecol" width="15%" >&nbsp;<STRONG>Level</STRONG><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:
					</TD>
					<TD class="tableinput" >&nbsp;<asp:textbox id="txtLevel" runat="server" CssClass="txtbox" MaxLength="20" ></asp:textbox></TD>
					<TD class="tableinput" ><asp:requiredfieldvalidator id="validate_Level" runat="server" ControlToValidate="txtLevel" ErrorMessage="Level is expecting numeric value."
							Display="None"></asp:requiredfieldvalidator></TD>
					<TD class="tableinput"></TD>
				</TR>
				<tr>
					<TD class="tablecol" nowrap>&nbsp;<STRONG>Approving Officer</STRONG><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</TD>
					<TD class="tableinput" width="45%" >&nbsp;<asp:dropdownlist id="cboAO" runat="server" CssClass="txtbox" AutoPostBack="True" width="100%" ></asp:dropdownlist></TD>
					<TD class="tableinput" ><asp:checkbox id="chkmass" runat="server" Enabled="False" Text="Authorised to do Mass Approval"></asp:checkbox></TD>
					<TD class="tableinput"><asp:rangevalidator id="RangeValidator1" runat="server" ControlToValidate="txtLevel" ErrorMessage="Numeric Only"
							MinimumValue="1" MaximumValue="99" Display="None" Type="Integer"></asp:rangevalidator><asp:requiredfieldvalidator id="validate_AO" runat="server" ControlToValidate="cboAo" ErrorMessage="Approving Officer Required"
							Display="None"></asp:requiredfieldvalidator></TD>
				</tr>
				<tr id="hidAAO">
					<TD class="tablecol" colspan="5" vAlign="top" >
						<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
							<TR>
					<TD width="20%" class="tablecol" nowrap>&nbsp;<STRONG>Alternative Approving Officer</STRONG>&nbsp;:</TD>
					<TD class="tableinput" width="226px"><asp:dropdownlist id="cboAAO" runat="server" CssClass="txtbox" Width="100%"></asp:dropdownlist></TD>
					<TD width="27%" class="tableinput" >&nbsp;
								<asp:Label id="lblRelief" runat="server"><STRONG>Relief Staff Control</STRONG></asp:Label>
					</TD>
					<TD width="8%" class="tableinput"><asp:radiobuttonlist id="rdRelief" runat="server" Height="0px" RepeatDirection="Horizontal" AutoPostBack="True">
							<asp:ListItem Value="O" Selected="True">Open</asp:ListItem>
							<asp:ListItem Value="C">Controlled</asp:ListItem>
						</asp:radiobuttonlist></TD>
				</tr>
				</TABLE>
				</TD>
				</tr>
				<tr>
					<TD class="emptycol" colspan="4"><asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</TD>
				</tr>
				<tr>
					<TD class="emptycol" colspan="4">&nbsp;</TD>
				</tr>
				<tr>
					<TD class="emptycol" colspan="4">
						<asp:button id="cmdsave" runat="server" CssClass="button" Text="Save"></asp:button>
						<asp:button id="cmd_delete" runat="server" CssClass="button" Text="Delete" Visible="False"></asp:button>
						<asp:button id="cmd_reset" runat="server" CssClass="button" Text="Reset" CausesValidation="False"></asp:button>
                        <INPUT class="button" id="cmdClose" onclick="Close();" type="button" value="Close" >
					</TD>
				</tr>
				<TR>
					<TD class="emptycol" colspan="4">&nbsp;</TD>
				</TR>
				<TR>
					<TD colspan="4"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" Width="392px"></asp:validationsummary>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colspan="4">&nbsp;</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
