<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BComVendor.aspx.vb" Inherits="eProcure.BComVendor" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BComVendor</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
        
        <script language="javascript">
        function confirmChanged(strmsg)
        {	
	    
	    ans=confirm(strmsg);
	//alert(ans);
	    if (ans){	
	    
		return true;
		    }
	    else
	    {
		
		return false;
		}
        }
        </script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_CompVParam_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
            <tr>
					<TD class="linespacing1" colSpan="4" ></TD>
			</TR>
				<TR>
					<TD >
						<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text="The document prefixes will appear in your transaction document as standard prefix with auto-numbering, e.g. RFQ000001"></asp:label>
					</TD>
				</TR>
				 <tr>
					<TD class="linespacing2" colSpan="4" ></TD>
			</TR>
				<tr>
					<TD align="center">
						<div align="left"><asp:label id="Label6" runat="server"  CssClass="lblInfo"
						Text="Note: Changing document prefixes may affect the Buyer Company interface files (GL and Payment files). Please consult with your Buyer before changing the prefixes below."
						></asp:label>
                        </div>
					</TD>
			</tr>
			<tr>
			</tr>
            <tr>
					<TD class="linespacing2" colSpan="4" ></TD>
			</TR>
				<TR>
					<TD vAlign="middle" align="left">
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader" align="left" colSpan="4"><asp:label id="lblHeader" runat="server"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tablecol" align="left">&nbsp;<strong>Quotation Prefix</strong> :</TD>
								<TD class="TableInput"><asp:textbox id="txt_QuoPre" runat="server" CssClass="txtbox" MaxLength="25" Width="135px"></asp:textbox>
									<asp:RegularExpressionValidator id="revQuoPre" runat="server" Display="None" ControlToValidate="txt_QuoPre" ErrorMessage="Invalid Quotation Prefix."
										ValidationExpression="^[^ '][^']*$"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>Quotation Last Used No.</strong> :</TD>
								<TD class="TableInput"><asp:textbox id="txt_QuoLun" runat="server" CssClass="txtbox" MaxLength="10"></asp:textbox><asp:regularexpressionvalidator id="revQuoLun" runat="server" ErrorMessage="Quotation Number is expecting numeric value."
										ControlToValidate="txt_QuoLun" ValidationExpression="^[0-9]{0,10}$" Display="None"></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<strong>DO Prefix</strong> :</TD>
								<TD class="TableInput"><asp:textbox id="txt_DoPre" runat="server" CssClass="txtbox" MaxLength="25" Width="136px"></asp:textbox>
									<asp:RegularExpressionValidator id="revDoPre" runat="server" Display="None" ControlToValidate="txt_DoPre" ErrorMessage="Invalid DO Prefix."
										ValidationExpression="^[^ '][^']*$"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>DO Last Used No.</strong> :</TD>
								<TD class="TableInput"><asp:textbox id="txt_DoLun" runat="server" CssClass="txtbox" MaxLength="10"></asp:textbox><asp:regularexpressionvalidator id="revDoLun" runat="server" ErrorMessage="DO Number is expecting numeric value."
										ControlToValidate="txt_DoLun" ValidationExpression="^[0-9]{0,10}$" Display="None"></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<strong>CSR Prefix</strong> :&nbsp;</TD>
								<TD class="TableInput"><asp:textbox id="txt_CSRPre" runat="server" CssClass="txtbox" MaxLength="25" Width="135px"></asp:textbox>
									<asp:RegularExpressionValidator id="revCSRPre" runat="server" Display="None" ControlToValidate="txt_CSRPre" ErrorMessage="Invalid CSR Prefix."
										ValidationExpression="^[^ '][^']*$"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>CSR Last&nbsp;Used No.</strong> :</TD>
								<TD class="TableInput"><asp:textbox id="txt_CSRLun" runat="server" CssClass="txtbox" MaxLength="10"></asp:textbox><asp:regularexpressionvalidator id="revCSRLun" runat="server" ErrorMessage="CSR Number is expecting numeric value."
										ControlToValidate="txt_CSRLun" ValidationExpression="^[0-9]{0,10}$" Display="None"></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<strong>Invoice Prefix</strong> :</TD>
								<TD class="tableinput"><asp:textbox id="txt_InvPre" runat="server" CssClass="txtbox" MaxLength="25" Width="134px"></asp:textbox>
									<asp:RegularExpressionValidator id="revInvPre" runat="server" Display="None" ControlToValidate="txt_InvPre" ErrorMessage="Invalid Invoice Prefix."
										ValidationExpression="^[^ '][^']*$"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>Invoice Last Used No.</strong> :</TD>
								<TD class="tableinput"><asp:textbox id="txt_InvLun" runat="server" CssClass="txtbox" MaxLength="10"></asp:textbox><asp:regularexpressionvalidator id="revInvLun" runat="server" ErrorMessage="Invoice Number is expecting numeric value."
										ControlToValidate="txt_InvLun" ValidationExpression="^[0-9]{0,10}$" Display="None"></asp:regularexpressionvalidator></TD>
							</TR>
                            <%--Jules 2015.02.02 Agora Stage 2--%>							
							<TR>
								<TD class="tablecol">&nbsp;<strong>Debit Note / Debit Advice Prefix</strong> :&nbsp;</TD>
								<TD class="TableInput"><asp:textbox id="txt_DebitPre" runat="server" CssClass="txtbox" MaxLength="25" Width="135px"></asp:textbox>
									<asp:RegularExpressionValidator id="revDebitPre" runat="server" Display="None" ControlToValidate="txt_DebitPre" ErrorMessage="Invalid Debit Note / Debit Advice Prefix."
										ValidationExpression="^[^ '][^']*$"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>Debit Note / Debit Advice Last Used No.</strong> :</TD>
								<TD class="TableInput"><asp:textbox id="txt_DebitLun" runat="server" CssClass="txtbox" MaxLength="10"></asp:textbox><asp:regularexpressionvalidator id="revDebitLun" runat="server" ErrorMessage="Debit Note / Debit Advice Number is expecting numeric value."
										ControlToValidate="txt_DebitLun" ValidationExpression="^[0-9]{0,10}$" Display="None"></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<strong>Credit Note / Credit Advice Prefix</strong> :</TD>
								<TD class="tableinput"><asp:textbox id="txt_CreditPre" runat="server" CssClass="txtbox" MaxLength="25" Width="134px"></asp:textbox>
									<asp:RegularExpressionValidator id="revCreditPre" runat="server" Display="None" ControlToValidate="txt_CreditPre" ErrorMessage="Invalid Credit Note / Credit Advice Prefix."
										ValidationExpression="^[^ '][^']*$"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>Credit Note / Credit Advice Last Used No.</strong> :</TD>
								<TD class="tableinput"><asp:textbox id="txt_CreditLun" runat="server" CssClass="txtbox" MaxLength="10"></asp:textbox><asp:regularexpressionvalidator id="revCrediLun" runat="server" ErrorMessage="Credit Note / Credit Advice Number is expecting numeric value."
										ControlToValidate="txt_CreditLun" ValidationExpression="^[0-9]{0,10}$" Display="None"></asp:regularexpressionvalidator></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:button id="cmd_save" runat="server" CssClass="button" Text="Save"></asp:button>
						<INPUT class="button" id="cmd_Reset" type="button" value="Reset" name="Reset1" onclick="ValidatorReset();"
							runat="server"></TD>
					
				</TR>
				<TR>
					<TD class="emptycol" vAlign="middle" align="center"></TD>
				</TR>
				<TR>
					<TD class="emptycol" vAlign="middle" align="left"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
