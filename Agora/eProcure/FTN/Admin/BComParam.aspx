<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BComParam.aspx.vb" Inherits="eProcure.BComParamFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>BComParam</title>
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
		
				function Verifycheck(){
				
					var ind, i, theForm, valcheck;
					valcheck=Form1.hidMode.value; 
					//theForm = document.Form1[0];
					for(i = 0; i < Form1.elements.length; i++){
						if(Form1.elements[i].type == 'checkbox' && Form1.elements[i].name == 'ChkConsolidation'){
							if (valcheck=='1'){
								if (!Form1.elements[i].checked){
									alert('All Consolidator that have been set for the Approval Group(if any) will be cleared');
								}
							}
						}
					}
					
				}

										
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_CompBParam_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
            <tr>
					<TD class="linespacing1" colSpan="4" ></TD>
			</TR>
				<TR>
					<TD align="center">
						<div align="left"><asp:label id="lblAction" runat="server" 
						Text="The document prefixes will appear in your transaction document as standard prefix with auto-numbering, e.g. RFQ000001"  CssClass="lblInfo"></asp:label>
                        </div>
					</TD>
				</TR>
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
								<TD class="tablecol" align="left">&nbsp;<strong>RFQ Prefix</strong> :</TD>
								<TD class="TableInput"><asp:textbox id="txt_RfqPre" runat="server" Width="135px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revRFQPre" runat="server" ErrorMessage="Invalid RFQ Prefix." ControlToValidate="txt_RfqPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>RFQ Last Used No.</strong> :</TD>
								<TD class="TableInput"><asp:textbox id="txt_RfqLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revRFQLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_RfqLun" ErrorMessage="RFQ Number is expecting numeric value."></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<strong>PR Prefix</strong> :</TD>
								<TD class="TableInput"><asp:textbox id="txt_PrPre" runat="server" Width="136px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revPRPre" runat="server" ErrorMessage="Invalid PR Prefix." ControlToValidate="txt_PrPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>PR Last Used No.</strong> :</TD>
								<TD class="TableInput"><asp:textbox id="txt_PrLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revPRLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_PrLun" ErrorMessage="PR Number is expecting numeric value."></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<strong>PO Prefix</strong> :&nbsp;</TD>
								<TD class="TableInput"><asp:textbox id="txt_PoPre" runat="server" Width="135px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revPOPre" runat="server" ErrorMessage="Invalid PO Prefix." ControlToValidate="txt_PoPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>PO Last&nbsp;Used No.</strong> :</TD>
								<TD class="TableInput"><asp:textbox id="txt_PoLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revPOLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_PoLun" ErrorMessage="PO Number is expecting numeric value."></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<strong>GRN Prefix</strong> :</TD>
								<TD class="tableinput"><asp:textbox id="txt_GrnPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revGRNPre" runat="server" ErrorMessage="Invalid GRN Prefix." ControlToValidate="txt_GrnPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>GRN Last Used No.</strong> :</TD>
								<TD class="tableinput"><asp:textbox id="txt_GrnLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revGRNLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_GrnLun" ErrorMessage="GRN Number is expecting numeric value."></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<strong>SRN Prefix</strong> :&nbsp;</TD>
								<TD class="tableinput"><asp:textbox id="txt_SrnPre" runat="server" Width="135px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revSRNPre" runat="server" ErrorMessage="Invalid SRN Prefix." ControlToValidate="txt_SrnPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>SRN Last Used No.</strong> :&nbsp;</TD>
								<TD class="tableinput"><asp:textbox id="txt_SrnLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revSRNLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_SrnLun" ErrorMessage="SRN Number is expecting numeric value."></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<strong>Payment Prefix</strong> :</TD>
								<TD class="tableinput"><asp:textbox id="txt_PayPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revPayPre" runat="server" ErrorMessage="Invalid Payment Prefix." ControlToValidate="txt_PayPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>Payment Last Used No.</strong> :</TD>
								<TD class="tableinput"><asp:textbox id="txt_PayLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revPayLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_PayLun" ErrorMessage="Payment Number is expecting numeric value."></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<strong>Tender Prefix</strong> :</TD>
								<TD class="tableinput"><asp:textbox id="txt_TenPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revTenPre" runat="server" ErrorMessage="Invalid Tender Prefix." ControlToValidate="txt_TenPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>Tender Last Used No.</strong> :</TD>
								<TD class="tableinput"><asp:textbox id="txt_TenLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revTenLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_TenLun" ErrorMessage="Tender Number is expecting numeric value."></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">
									<P><STRONG>&nbsp;Cancellation Request Prefix </STRONG>:</P>
								</TD>
								<TD class="tableinput"><asp:textbox id="txt_DebitPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revDebitPre" runat="server" ErrorMessage="Invalid Cancellation Request Prefix."
										ControlToValidate="txt_DebitPre" ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong>Cancellation Request Last Used No.</strong> :</TD>
								<TD class="tableinput"><asp:textbox id="txt_DebitLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox>
								    <asp:regularexpressionvalidator id="revDebitLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_DebitLun" ErrorMessage="Cancellation Request Number is expecting numeric value."></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">
									<P><STRONG>&nbsp;<asp:Label ID="Label1" runat="server" Text="Inventory Requisition Prefix :" ></asp:Label></STRONG></P>
								</TD>
								<TD class="tableinput"><asp:textbox id="txt_IRPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revIRPre" runat="server" ErrorMessage="Invalid Inventory Requisition Prefix."
										ControlToValidate="txt_IRPre" ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong><asp:Label ID="Label2" runat="server" Text="Inventory Requisition Last Used No. :" ></asp:Label></strong></TD>
								<TD class="tableinput"><asp:textbox id="txt_IRLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox>
								    <asp:regularexpressionvalidator id="revIRLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_IRLun" ErrorMessage="Inventory Requisition Number is expecting numeric value."></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD class="tablecol">
									<P><STRONG>&nbsp;<asp:Label ID="Label3" runat="server" Text="Inventory Transfer Prefix :" ></asp:Label></STRONG></P>
								</TD>
								<TD class="tableinput"><asp:textbox id="txt_ITPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revITPre" runat="server" ErrorMessage="Invalid Inventory Transfer Prefix."
										ControlToValidate="txt_ITPre" ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></TD>
								<TD class="tablecol">&nbsp;<strong><asp:Label ID="Label5" runat="server" Text="Inventory Transfer Last Used No. :" ></asp:Label></strong></TD>
								<TD class="tableinput"><asp:textbox id="txt_ITLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox>
								    <asp:regularexpressionvalidator id="revITLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_ITLun" ErrorMessage="Inventory Transfer Number is expecting numeric value."></asp:regularexpressionvalidator></TD>
							</TR>
							
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="WIDTH: 713px" vAlign="middle" align="center"></TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="left">
						<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader" style="HEIGHT: 21px" align="left" colSpan="2">&nbsp;Company 
									Setting</TD>
							</TR>
							<TR>
								<TD class="tablecol" width="25%">&nbsp;<STRONG>RFQ Option</STRONG>&nbsp;:</TD>
								<TD class="TableInput" width="75%"><asp:radiobuttonlist id="opl_RfqOp" runat="server" Width="245px" RepeatDirection="Horizontal" Height="14px">
										<asp:ListItem Value="0" Selected="True">Open</asp:ListItem>
										<asp:ListItem Value="1">Closed</asp:ListItem>
										<asp:ListItem Value="2">Defined By Buyer</asp:ListItem>
									</asp:radiobuttonlist>&nbsp;&nbsp;</TD>
							</TR>
							<TR>
								<TD class="TableInput" style="HEIGHT: 26px" colSpan="2">&nbsp;
									<asp:checkbox id="ChkFreeForm" runat="server" Width="226px" Text="Allow Free Form Billing Address"></asp:checkbox>
									<asp:checkbox id="ChkConsolidation" runat="server" Width="228px" Text="Consolidation Required"
										AutoPostBack="True" Visible="false" style="display:none"></asp:checkbox>
									<asp:checkbox id="ChkLevelsRec" style="display:none" runat="server" Width="228px" Text="2 Levels Receiving" Visible="false"></asp:checkbox>	
										</TD>
									
							</TR>
							<%--<TR>
								<TD class="TableInput" colSpan="2">&nbsp;
									<asp:checkbox id="ChkConsolidation" runat="server" Width="228px" Text="Consolidation Required"
										AutoPostBack="True" Visible="false"></asp:checkbox></TD>
							</TR>--%>
							<%--<TR>
								<TD class="TableInput" style="WIDTH: 129px; HEIGHT: 27px" width="129" colSpan="2">&nbsp;
									<asp:checkbox id="ChkLevelsRec" runat="server" Width="228px" Text="2 Levels Receiving" Visible="false"></asp:checkbox></TD>
							</TR>--%>
						</TABLE>
						<TR>
					<TD class="emptycol" style="WIDTH: 713px" vAlign="middle" align="center"></TD>
				</TR>
					</TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="left">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader" align="left" colSpan="3">&nbsp;Contract Item PR 
									Setting</TD>
							</TR>
							<TR>
								<TD class="TableInput" width="600px"><asp:radiobuttonlist id="Radiobuttonlist1" runat="server" AutoPostBack="true">
										<asp:ListItem Value="B" Selected="True">Buyer As Owner of Purchase Order (for order cancellation)</asp:ListItem>
										<asp:ListItem Value="P">Purchasing Officer As Owner of Purchase Order (for order cancellation) :</asp:ListItem>
									</asp:radiobuttonlist>
								<asp:label id="Label4" CssClass="lblInfo" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Select Purchasing Officer &nbsp;</asp:label>
								<asp:dropdownlist id="cboAO" runat="server" CssClass="ddl" width="250px" Enabled="false"></asp:dropdownlist></TD>
							</TR>	
							<TR>
					<TD class="emptycol" vAlign="middle" align="center"></TD>
				</TR>
												
						</TABLE>
						
						<asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="WIDTH: 713px" vAlign="middle" align="center"></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 713px"><asp:button id="cmd_save" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<INPUT class="button" id="cmd_Reset" type="button" onclick="ValidatorReset();" value="Reset"
							name="Reset1" runat="server"><INPUT id="hidMode" type="hidden" size="1" name="hidMode" runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="WIDTH: 713px" vAlign="middle" align="center"></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="WIDTH: 713px" vAlign="middle" align="left"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
