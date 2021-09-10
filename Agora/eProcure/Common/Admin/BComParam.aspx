<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BComParam.aspx.vb" Inherits="eProcure.BComParam" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>BComParam</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>		
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim CSS As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "AutoComplete.css") & """ rel='stylesheet'>"
            Dim collapse_up as string = dDispatcher.direct("Plugins/images","collapse_up.gif")
            Dim collapse_down as string = dDispatcher.direct("Plugins/images","collapse_down.gif")
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script type="text/javascript">
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
				
				function isNumberKey(evt)
                {
                    var charCode = (evt.which) ? evt.which : event.keyCode
                    if (charCode > 31 && (charCode < 48 || charCode > 57))
                        return false;

                    return true;
                }
				
				function showHide1(lnkdesc)
                {
                    if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';
	                    document.getElementById("Image1").src = '<%response.write(collapse_up) %>';
                    } 
                    else 
                    {
    	                document.getElementById(lnkdesc).style.display = 'none';
	                    document.getElementById("Image1").src = '<%response.write(collapse_down) %>';
                    }
                }  

										
		-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_CompBParam_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
            <tr>
					<td class="linespacing1" colspan="4" ></td>
			</tr>
				<tr>
					<td align="center">
						<div align="left"><asp:label id="lblAction" runat="server" 
						Text="The document prefixes will appear in your transaction document as standard prefix with auto-numbering, e.g. RFQ000001"  CssClass="lblInfo"></asp:label>
                        </div>
					</td>
				</tr>
            <tr>
					<td class="linespacing2" colspan="4" ></td>
			</tr>
				<tr>
					<td valign="middle" align="left">
						<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0">
							<tr>
								<td class="tableheader" align="left" colspan="4"><asp:label id="lblHeader" runat="server"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" align="left">&nbsp;<strong>RFQ Prefix</strong> :</td>
								<td class="TableInput"><asp:textbox id="txt_RfqPre" runat="server" Width="135px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revRFQPre" runat="server" ErrorMessage="Invalid RFQ Prefix." ControlToValidate="txt_RfqPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></td>
								<td class="tablecol">&nbsp;<strong>RFQ Last Used No.</strong> :</td>
								<td class="TableInput"><asp:textbox id="txt_RfqLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revRFQLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_RfqLun" ErrorMessage="RFQ Number is expecting numeric value."></asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td class="tablecol">&nbsp;<strong>PR Prefix</strong> :</td>
								<td class="TableInput"><asp:textbox id="txt_PrPre" runat="server" Width="136px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revPRPre" runat="server" ErrorMessage="Invalid PR Prefix." ControlToValidate="txt_PrPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></td>
								<td class="tablecol">&nbsp;<strong>PR Last Used No.</strong> :</td>
								<td class="TableInput"><asp:textbox id="txt_PrLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revPRLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_PrLun" ErrorMessage="PR Number is expecting numeric value."></asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td class="tablecol">&nbsp;<strong>PO Prefix</strong> :&nbsp;</td>
								<td class="TableInput"><asp:textbox id="txt_PoPre" runat="server" Width="135px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revPOPre" runat="server" ErrorMessage="Invalid PO Prefix." ControlToValidate="txt_PoPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></td>
								<td class="tablecol">&nbsp;<strong>PO Last&nbsp;Used No.</strong> :</td>
								<td class="TableInput"><asp:textbox id="txt_PoLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revPOLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_PoLun" ErrorMessage="PO Number is expecting numeric value."></asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td class="tablecol">&nbsp;<strong>GRN Prefix</strong> :</td>
								<td class="tableinput"><asp:textbox id="txt_GrnPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revGRNPre" runat="server" ErrorMessage="Invalid GRN Prefix." ControlToValidate="txt_GrnPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></td>
								<td class="tablecol">&nbsp;<strong>GRN Last Used No.</strong> :</td>
								<td class="tableinput"><asp:textbox id="txt_GrnLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revGRNLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_GrnLun" ErrorMessage="GRN Number is expecting numeric value."></asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td class="tablecol">&nbsp;<strong>SRN Prefix</strong> :&nbsp;</td>
								<td class="tableinput"><asp:textbox id="txt_SrnPre" runat="server" Width="135px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revSRNPre" runat="server" ErrorMessage="Invalid SRN Prefix." ControlToValidate="txt_SrnPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></td>
								<td class="tablecol">&nbsp;<strong>SRN Last Used No.</strong> :&nbsp;</td>
								<td class="tableinput"><asp:textbox id="txt_SrnLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revSRNLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_SrnLun" ErrorMessage="SRN Number is expecting numeric value."></asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td class="tablecol">&nbsp;<strong>Payment Prefix</strong> :</td>
								<td class="tableinput"><asp:textbox id="txt_PayPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revPayPre" runat="server" ErrorMessage="Invalid Payment Prefix." ControlToValidate="txt_PayPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></td>
								<td class="tablecol">&nbsp;<strong>Payment Last Used No.</strong> :</td>
								<td class="tableinput"><asp:textbox id="txt_PayLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revPayLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_PayLun" ErrorMessage="Payment Number is expecting numeric value."></asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td class="tablecol">&nbsp;<strong>Tender Prefix</strong> :</td>
								<td class="tableinput"><asp:textbox id="txt_TenPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revTenPre" runat="server" ErrorMessage="Invalid Tender Prefix." ControlToValidate="txt_TenPre"
										ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></td>
								<td class="tablecol">&nbsp;<strong>Tender Last Used No.</strong> :</td>
								<td class="tableinput"><asp:textbox id="txt_TenLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox><asp:regularexpressionvalidator id="revTenLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_TenLun" ErrorMessage="Tender Number is expecting numeric value."></asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td class="tablecol">
									<p><strong>&nbsp;Cancellation Request Prefix </strong>:</p>
								</td>
								<td class="tableinput"><asp:textbox id="txt_DebitPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revDebitPre" runat="server" ErrorMessage="Invalid Cancellation Request Prefix."
										ControlToValidate="txt_DebitPre" ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></td>
								<td class="tablecol">&nbsp;<strong>Cancellation Request Last Used No.</strong> :</td>
								<td class="tableinput"><asp:textbox id="txt_DebitLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox>
								    <asp:regularexpressionvalidator id="revDebitLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_DebitLun" ErrorMessage="Cancellation Request Number is expecting numeric value."></asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td class="tablecol">
									<p><strong>&nbsp;<asp:Label ID="Label1" runat="server" Text="Inventory Requisition Prefix :" ></asp:Label></strong></p>
								</td>
								<td class="tableinput"><asp:textbox id="txt_IRPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revIRPre" runat="server" ErrorMessage="Invalid Inventory Requisition Prefix."
										ControlToValidate="txt_IRPre" ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></td>
								<td class="tablecol">&nbsp;<strong><asp:Label ID="Label2" runat="server" Text="Inventory Requisition Last Used No. :" ></asp:Label></strong></td>
								<td class="tableinput"><asp:textbox id="txt_IRLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox>
								    <asp:regularexpressionvalidator id="revIRLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_IRLun" ErrorMessage="Inventory Requisition Number is expecting numeric value."></asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td class="tablecol">
									<p><strong>&nbsp;<asp:Label ID="Label3" runat="server" Text="Inventory Transfer Prefix :" ></asp:Label></strong></p>
								</td>
								<td class="tableinput"><asp:textbox id="txt_ITPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revITPre" runat="server" ErrorMessage="Invalid Inventory Transfer Prefix."
										ControlToValidate="txt_ITPre" ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></td>
								<td class="tablecol">&nbsp;<strong><asp:Label ID="Label5" runat="server" Text="Inventory Transfer Last Used No. :" ></asp:Label></strong></td>
								<td class="tableinput"><asp:textbox id="txt_ITLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox>
								    <asp:regularexpressionvalidator id="revITLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_ITLun" ErrorMessage="Inventory Transfer Number is expecting numeric value."></asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td class="tablecol">
									<p><strong>&nbsp;<asp:Label ID="Label7" runat="server" Text="Return Inward Prefix :" ></asp:Label></strong></p>
								</td>
								<td class="tableinput"><asp:textbox id="txt_RIPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revRIPre" runat="server" ErrorMessage="Invalid Return Inward Prefix."
										ControlToValidate="txt_RIPre" ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></td>
								<td class="tablecol">&nbsp;<strong><asp:Label ID="Label8" runat="server" Text="Return Inward Last Used No. :" ></asp:Label></strong></td>
								<td class="tableinput"><asp:textbox id="txt_RILun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox>
								    <asp:regularexpressionvalidator id="revRILun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_RILun" ErrorMessage="Return Inward Number is expecting numeric value."></asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td class="tablecol">
									<p><strong>&nbsp;<asp:Label ID="Label9" runat="server" Text="Return Outward Prefix :" ></asp:Label></strong></p>
								</td>
								<td class="tableinput"><asp:textbox id="txt_ROPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revROPre" runat="server" ErrorMessage="Invalid Return Outward Prefix."
										ControlToValidate="txt_ROPre" ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></td>
								<td class="tablecol">&nbsp;<strong><asp:Label ID="Label10" runat="server" Text="Return Outward Last Used No. :" ></asp:Label></strong></td>
								<td class="tableinput"><asp:textbox id="txt_ROLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox>
								    <asp:regularexpressionvalidator id="revROLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_ROLun" ErrorMessage="Return Outward Number is expecting numeric value."></asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td class="tablecol">
									<p><strong>&nbsp;<asp:Label ID="Label11" runat="server" Text="Write Off Prefix :" ></asp:Label></strong></p>
								</td>
								<td class="tableinput"><asp:textbox id="txt_WOPre" runat="server" Width="134px" MaxLength="25" CssClass="txtbox"></asp:textbox>
									<asp:RegularExpressionValidator id="revWOPre" runat="server" ErrorMessage="Invalid Write Off Prefix."
										ControlToValidate="txt_WOPre" ValidationExpression="^[^ '][^']*$" Display="None"></asp:RegularExpressionValidator></td>
								<td class="tablecol">&nbsp;<strong><asp:Label ID="Label12" runat="server" Text="Write Off Last Used No. :" ></asp:Label></strong></td>
								<td class="tableinput"><asp:textbox id="txt_WOLun" runat="server" MaxLength="10" CssClass="txtbox"></asp:textbox>
								    <asp:regularexpressionvalidator id="revWOLun" runat="server" Display="None" ValidationExpression="^[0-9]{0,10}$"
										ControlToValidate="txt_WOLun" ErrorMessage="Write Off Number is expecting numeric value."></asp:regularexpressionvalidator></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="WIDTH: 713px" valign="middle" align="center"></td>
				</tr>
            </table>
            
            <div style="width:100%; cursor:pointer;"  class="tableheader" onclick="showHide1('IQC')"><asp:label id="Label17" runat="server">&nbsp;IQC Document Prefix And Last Used Number</asp:label>
                            <asp:Image ID="Image1" runat="server" ImageUrl="#" /></div>
			<div id="IQC" style="display:inline; width:100%">
			    <% Response.Write(Session("ConstructTableIQC")) %>
			 
			    <asp:Button ID="btnIQC" runat="server" CssClass="button" Text="Add Line" />
			</div>
			<table class="alltable" id="Table7" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td class="emptycol" style="WIDTH: 713px" valign="middle" align="center"></td>
				</tr>
				<tr>
					<td valign="middle" align="left" style="height: 95px">
						<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" border="0">
							<tr>
								<td class="tableheader" align="left" colspan="2">&nbsp;Company 
									Setting</td>
							</tr>
							<tr>
								<td class="tablecol" width="25%" Height="10px">&nbsp;<strong>RFQ Option</strong>&nbsp;:</td>
								<td class="TableInput" width="75%"><asp:radiobuttonlist id="opl_RfqOp" runat="server" Width="245px" RepeatDirection="Horizontal" Height="10px">
										<asp:ListItem Value="0" Selected="True">Open</asp:ListItem>
										<asp:ListItem Value="1">Closed</asp:ListItem>
										<asp:ListItem Value="2">Defined By Buyer</asp:ListItem>
									</asp:radiobuttonlist>&nbsp;&nbsp;</td>
							</tr>
							<tr>
								<td class="TableInput" style="HEIGHT: 10px" colspan="2">&nbsp;
									<asp:checkbox id="ChkFreeForm" runat="server" Width="226px" Text="Allow Free Form Billing Address"></asp:checkbox>
									<asp:checkbox id="ChkConsolidation" runat="server" Width="228px" Text="Consolidation Required"
										AutoPostBack="True" Visible="false" style="display:none"></asp:checkbox>
									<asp:checkbox id="ChkLevelsRec" style="display:none" runat="server" Width="228px" Text="2 Levels Receiving" Visible="false"></asp:checkbox><br/>&nbsp;	
									<asp:checkbox id="ChkAccCode" runat="server" Width="226px" Text="Display Account Code Option (PDF)"></asp:checkbox>	
										</td>
									
							</tr>
							<%--<tr>
								<td class="TableInput" colspan="2">&nbsp;
									<asp:checkbox id="ChkConsolidation" runat="server" Width="228px" Text="Consolidation Required"
										AutoPostBack="True" Visible="false"></asp:checkbox></td>
							</tr>--%>
							<%--<tr>
								<td class="TableInput" style="WIDTH: 129px; HEIGHT: 27px" width="129" colspan="2">&nbsp;
									<asp:checkbox id="ChkLevelsRec" runat="server" Width="228px" Text="2 Levels Receiving" Visible="false"></asp:checkbox></td>
							</tr>--%>
						</table>
					</td>
				</tr>
				<tr>
					<td valign="middle" align="left">
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0">
							<tr>
								<td class="tableheader" align="left" colspan="3">&nbsp;Contract Item PR Setting</td>
							</tr>
							<tr>
								<td class="TableInput" width="600px"><asp:radiobuttonlist id="Radiobuttonlist1" runat="server" AutoPostBack="true">
										<asp:ListItem Value="B" Selected="True">Buyer As Owner of Purchase Order (for order cancellation)</asp:ListItem>
										<asp:ListItem Value="P">Purchasing Officer As Owner of Purchase Order (for order cancellation) :</asp:ListItem>
									</asp:radiobuttonlist>
								<asp:label id="Label4" CssClass="lblInfo" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Select Purchasing Officer &nbsp;</asp:label>
								<asp:dropdownlist id="cboAO" runat="server" CssClass="ddl" width="250px" Enabled="false"></asp:dropdownlist></td>
							</tr>	
						</table>
					</td>
				</tr>
				<tr>
					<td valign="middle" align="left" style="height: 81px" rowspan="">
						<table class="alltable" id="Table5" cellspacing="0" cellpadding="0" border="0">
							<tr>
								<td class="tableheader" align="left" colspan="3">&nbsp;Non-Contract Item PR 
									Setting</td>
							</tr>
							<tr>
								<td class="TableInput" style="width: 600px; height: 41px;"><asp:radiobuttonlist id="Radiobuttonlist2" runat="server" AutoPostBack="true">
										<asp:ListItem Value="NB" Selected="True">Buyer As Owner of Purchase Order (for order cancellation)</asp:ListItem>
										<asp:ListItem Value="NP">Purchasing Officer As Owner of Purchase Order (for order cancellation) :</asp:ListItem>
									</asp:radiobuttonlist></td>
							</tr>	
						</table>
					</td>						
				</tr>
				<tr>
					<td valign="middle" align="left" colspan="2">
						<table class="alltable" id="Table6" cellspacing="0" cellpadding="0" border="0">
							<tr>
								<td class="tableheader" align="left" colspan="2">&nbsp;PR Submission Access with GRN Control</td>
							</tr>
							<tr>
					<td class="tablecol" style="width: 28%" >&nbsp;PR Submission Access with GRN Control&nbsp;:</td>
								<td class="TableInput" valign="bottom" ><asp:radiobuttonlist id="Radiobuttonlist3" runat="server" RepeatDirection="Horizontal" CssClass="rbtn" >
										<asp:ListItem Value="0" Text="On"></asp:ListItem>
										<asp:ListItem Value="1" Selected="True" Text="Off"></asp:ListItem>
									</asp:radiobuttonlist></td>

							</tr>	

								<%--<asp:label id="Label7" CssClass="lblInfo" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Select Purchasing Officer &nbsp;</asp:label>--%>
								<%--<asp:dropdownlist id="cboNAO" runat="server" CssClass="ddl" width="250px" Enabled="false"></asp:dropdownlist>--%>
			
												
						</table>
					</td>						
				</tr>
				<tr>
				    <td valign="middle" align="left" colspan="2" style="height: 82px">
				        <table class="alltable" id="Table8" cellspacing="0" cellpadding="0" border="0">
				            <tr>
								<td class="tableheader" align="left" colspan="2">&nbsp;Inventory Setting</td>
							</tr>
							<tr>
							    <td class="TableInput" colspan="2">&nbsp;Stock Received Auto Acknowledge&nbsp;<asp:TextBox ID="txt_Stk_Received" runat="server" CssClass="txtbox"></asp:TextBox>
							    &nbsp;Working Days (Requestor Item)</td>    
							</tr>
							<tr>
							    <td class="TableInput" style="HEIGHT: 10px" Width="50%">&nbsp;
							        <asp:checkbox id="ChkUrgentIREmail" runat="server" Text="Urgent Inventory Request Email Notification"></asp:checkbox>
							    </td> 
							    <td class="TableInput" style="HEIGHT: 10px" Width="50%">&nbsp;
							        <asp:checkbox id="ChkSafetyLvlEmail" runat="server" Text="Safety Level Email Notification"></asp:checkbox>
							    </td> 
							</tr>
							<tr>
							    <td class="TableInput" style="HEIGHT: 10px" Width="50%">&nbsp;
							        <asp:checkbox id="ChkRejectIREmail" runat="server" Text="Reject MRS Email Notification"></asp:checkbox>
							    </td> 
							    <td class="TableInput" style="HEIGHT: 10px" Width="50%">&nbsp;
							        <asp:checkbox id="ChkReorderLvlEmail" runat="server" Text="Reorder Level Email Notification"></asp:checkbox>
							    </td> 
							</tr>
							<tr>
							    <td class="TableInput" style="HEIGHT: 10px" Width="50%">&nbsp;
							        <asp:checkbox id="ChkLocSecEmail" runat="server" Text="Allow Inventory Location Selection (Inventory Requisition)"></asp:checkbox>
							    </td> 
							    <td class="TableInput" style="HEIGHT: 10px" Width="50%">&nbsp;
							        <asp:checkbox id="ChkMaxInvLvlEmail" runat="server" Text="Maximum Inventory Level Email Notification"></asp:checkbox>
							    </td> 
							</tr>
				        </table> 
				    </td>
				</tr>
				<%--mimi : 20/03/2017 - enhancement smart pay ref. --%>
				<tr>
				    <td valign="middle" align="left" colspan="2" style="height: 82px">
				        <table class="alltable" id="Table9" cellspacing="0" cellpadding="0" border="0">
				            <tr>
								<td class="tableheader" align="left" colspan="2">&nbsp;Staff Claim Setting</td>
							</tr>
							<tr>
							    <td class="TableInput" style="width: 15%"><strong>&nbsp;Smart Pay Cap Limit&nbsp; :&nbsp;&nbsp;</strong><asp:TextBox ID="txt_Smart_Pay" runat="server" CssClass="txtbox"></asp:TextBox></td>
							    <%--<td class="TableInput" colspan="2">&nbsp;<strong>Smart Pay Cap Limit&nbsp; :&nbsp;&nbsp;</strong><asp:TextBox ID="txt_Smart_Pay" runat="server" CssClass="txtbox"></asp:TextBox></td>--%>
							</tr>
				        </table> 
				    </td>
				</tr>
				<%--end--%>
				<tr>
					<td valign="middle" align="left" style="height: 19px">
										<asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="WIDTH: 713px" valign="middle" align="center"></td>
				</tr>
				<tr>
					<td style="WIDTH: 713px"><asp:button id="cmd_save" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<input class="button" id="cmd_Reset" type="button" onclick="ValidatorReset();" value="Reset"
							name="Reset1" runat="server"/><input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"/></td>
				</tr>
				<tr>
					<td class="emptycol" style="WIDTH: 713px" valign="middle" align="center"></td>
				</tr>
				<tr>
					<td class="emptycol" style="WIDTH: 713px" valign="middle" align="left"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary></td>
				</tr>
			</table>
		</form>
	</body>
</html>
