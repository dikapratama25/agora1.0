<%@ Page Language="vb" AutoEventWireup="false" Codebehind="IPPParamMaint.aspx.vb" Inherits="eProcure.IPPParamMaint" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>E2P Parameter Maintenance</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <%--<script runat="server">
            'Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> --%>
        <%response.write(Session("WheelScript"))%>
        
        <script type="text/javascript">
        	function isNumberKey(evt)
            {
                 var charCode = (evt.which) ? evt.which : event.keyCode
                 if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;

                 return true;
            }    
		function isDecimalKey(evt)
            {
                 var charCode = (evt.which) ? evt.which : event.keyCode
                 if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode != 46)
                    return false;

                 return true;
            }   
            
    function isNumberCharKey(evt)
            {
                 var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode > 31 && (charCode < 48 || charCode > 57) && (charCode < 65 || charCode > 90) && (charCode < 97 || charCode > 122))
                    return false;

                 return true;
            }   
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
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
      
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" >
			 <tr >
					<td class="header" ><asp:label id="lblTitle" runat="server" Text="E2P Parameter Maintenance"></asp:label></td>
				</tr>
            <tr>
					<td class="linespacing1" colspan="4" ></td>
			</tr>
				<tr>
					<td >
						<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text="Fill in the relevant information and Save"></asp:label>
					</td>
				</tr>
				 <tr>
					<td class="linespacing2" colspan="4" ></td>
			</tr>
				
			<tr>
			</tr>
            <tr>
					<td class="linespacing2" colspan="4" ></td>
			</tr>
				<tr>
					<td valign="middle" align="left" style="width:340px;">
						<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0">
							<tr>
								<td class="tableheader" align="left"  style="height: 19px"><asp:label id="lblHeader" runat="server"></asp:label></td>
								<td class="tableheader" align="left"  style="height: 19px"><asp:label id="lblHeader2" runat="server"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" align="left" style="height: 19px;width:200px">&nbsp;<asp:label ID="lblLateSubmit" runat ="server" CssClass = "lbl" ></asp:label></td>
								<td class="TableInput" ><asp:textbox id="txt_LateSubmit" runat="server" CssClass="txtbox" Width="140px"></asp:textbox></td>
									<%--<asp:RegularExpressionValidator id="revLateSubmit" runat="server" Display="None" ControlToValidate="txt_LateSubmit" ErrorMessage="Invalid Quotation Prefix."
										ValidationExpression="^[^ '][^']*$"></asp:RegularExpressionValidator></td>
										--%>
								</tr>
							<tr runat="server" id="trDefaultCASA">
								<td class="tablecol">&nbsp;<asp:label ID="lblBankCodeCASA" runat ="server" CssClass = "lbl" ></asp:label></td>
								<td class="TableInput"><asp:textbox id="txt_BCCASA" runat="server" CssClass="txtbox" Width="100%"></asp:textbox></td><%--<asp:regularexpressionvalidator id="revQuoLun" runat="server" ErrorMessage="Quotation Number is expecting numeric value."
										ControlToValidate="txt_BCCASA" ValidationExpression="^[0-9]{0,10}$" Display="None"></asp:regularexpressionvalidator></td>
										--%>
								</tr>
							<tr runat="server" id="trDefaultBC">
								<td class="tablecol">&nbsp;<asp:label ID="lblBankCodeCheque" runat ="server" CssClass = "lbl" ></asp:label></td>
								<td class="TableInput"><asp:textbox id="txt_BCCheque" runat="server" CssClass="txtbox" Width="100%"></asp:textbox></td>
								
							</tr>
							<tr runat="server" id="trCASALimit">
								<td class="tablecol">&nbsp;<asp:label ID="lblCASALimit" runat ="server" CssClass = "lbl" ></asp:label></td>
								<td class="TableInput"><asp:textbox id="txt_CASALimit" runat="server" CssClass="txtbox" Width="100%"></asp:textbox></td><%--<asp:regularexpressionvalidator id="revCASALimit" runat="server" ErrorMessage="DO Number is expecting numeric value."
										ControlToValidate="txt_CASALimit" ValidationExpression="^[0-9]{0,10}$" Display="None"></asp:regularexpressionvalidator></td>
										--%>
							</tr>
							<tr runat="server" id="trIBGLimit">
								<td class="tablecol">&nbsp;<asp:label ID="lblIBGLimit" runat ="server" CssClass = "lbl" >&nbsp;</asp:label></td>
								<td class="TableInput"><asp:textbox id="txt_IBGLimit" runat="server" CssClass="txtbox" Width="100%"></asp:textbox></td>
									<%--<asp:RegularExpressionValidator id="revIBGLimit" runat="server" Display="None" ControlToValidate="txt_IBGLimit" ErrorMessage="Invalid CSR Prefix."
										ValidationExpression="^[^ '][^']*$"></asp:RegularExpressionValidator></td>
										--%>
							</tr>
							<tr runat="server" id="trIBGCharge">
								<td class="tablecol">&nbsp;<asp:label ID="lblBankChargeIBG" runat ="server" CssClass = "lbl" ></asp:label></td>
								<td class="TableInput"><asp:textbox id="txt_BCIBG" runat="server" CssClass="txtbox" Width="100%"></asp:textbox></td><%--<asp:regularexpressionvalidator id="revCSRLun" runat="server" ErrorMessage="CSR Number is expecting numeric value."
										ControlToValidate="txt_BCIBG" ValidationExpression="^[0-9]{0,10}$" Display="None"></asp:regularexpressionvalidator></td>
										--%>
								</tr>
							<tr runat="server" id="trRentasCharge">
								<td class="tablecol">&nbsp;<asp:label ID="lblBankChargeRENTAS" runat ="server" CssClass = "lbl" ></asp:label></td>
								<td class="tableinput"><asp:textbox id="txt_BCRENTAS" runat="server" CssClass="txtbox" Width="100%"></asp:textbox></td>
									<%--<asp:RegularExpressionValidator id="revBCRENTAS" runat="server" Display="None" ControlToValidate="txt_BCRENTAS" ErrorMessage="Invalid Invoice Prefix."
										ValidationExpression="^[^ '][^']*$"></asp:RegularExpressionValidator></td>
										--%>
								</tr>
							
                            <tr runat="server" id="trBCCharge">
							    <td class="tablecol">&nbsp;<asp:label ID="lblBankChargeBC" runat ="server" CssClass = "lbl" ></asp:label></td>
							    <td class="tableinput"><asp:textbox id="txt_BCBC" runat="server" CssClass="txtbox" Width="100%"></asp:textbox></td>
							        <%--<asp:RegularExpressionValidator id="revBCBC" runat="server" Display="None" ControlToValidate="txt_BCBC" ErrorMessage="Invalid Invoice Prefix."
										ValidationExpression="^[^ '][^']*$"></asp:RegularExpressionValidator></td>
										--%>
							</tr>
							<tr runat="server" id="trTTCharge1">
							    <td class="tablecol">&nbsp;<asp:label ID="lblBankChargeTT1" runat ="server" CssClass = "lbl" ></asp:label></td>
							    <td class="tableinput"><asp:textbox id="txt_BCTT1" runat="server" CssClass="txtbox" Width="100%"></asp:textbox></td>
							         <%--<asp:RegularExpressionValidator id="revBCTT1" runat="server" Display="None" ControlToValidate="txt_BCTT1" ErrorMessage="Invalid Invoice Prefix."
										ValidationExpression="^[^ '][^']*$"></asp:RegularExpressionValidator></td>
										--%>
							</tr>
							<tr runat="server" id="trTTCharge2">
							    <td class="tablecol">&nbsp;<asp:label ID="lblBankChargeTT2" runat ="server" CssClass = "lbl" ></asp:label></td>
							    <td class="tableinput"><asp:textbox id="txt_BCTT2" runat="server" CssClass="txtbox" Width="100%"></asp:textbox></td>
							         <%--<asp:RegularExpressionValidator id="revBCTT2" runat="server" Display="None" ControlToValidate="txt_BCTT2" ErrorMessage="Invalid Invoice Prefix."
										ValidationExpression="^[^ '][^']*$"></asp:RegularExpressionValidator></td>
										--%>
							</tr>
							<tr>
							    <td class="tablecol">&nbsp;<asp:label ID="lblGSTInputTC" runat ="server" CssClass = "lbl" ></asp:label></td>
							    <td class="tableinput"><asp:DropDownList ID="ddl_GSTInputTC" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList></td>
							</tr>
							<tr>
							    <td class="tablecol">&nbsp;<asp:label ID="lblGSTOutputTC" runat ="server" CssClass = "lbl" ></asp:label></td>
							    <td class="tableinput"><asp:DropDownList ID="ddl_GSTOutputTC" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList></td>
							</tr>
							<tr>
							    <td class="tablecol">&nbsp;<asp:label ID="lblTCTaxInv" runat ="server" CssClass = "lbl" ></asp:label></td>
							    <td class="tableinput"><asp:DropDownList ID="ddl_TCTaxInv" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList></td>
							</tr>
							<tr>
							    <td class="tablecol">&nbsp;<asp:label ID="lblTCDebitNote" runat ="server" CssClass = "lbl" ></asp:label></td>
							    <td class="tableinput"><asp:DropDownList ID="ddl_TCDebitNote" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList></td>
							</tr>
							<tr>
							    <td class="tablecol">&nbsp;<asp:label ID="lblTCCreditNote" runat ="server" CssClass = "lbl" ></asp:label></td>
							    <td class="tableinput"><asp:DropDownList ID="ddl_TCCreditNote" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList></td>
							</tr>
							<tr runat="server" id="trBLInputTax" visible="false">
							    <td class="tablecol">&nbsp;<asp:label ID="lblBlockTC" runat ="server" CssClass = "lbl" ></asp:label></td>
							    <td class="tableinput"><asp:DropDownList ID="ddl_BlockTC" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList></td>
							</tr>
							<tr>
							    <td class="tablecol">&nbsp;<asp:label ID="lblGiftLuckyInput" runat ="server" CssClass = "lbl" ></asp:label></td>
							    <td class="tableinput"><asp:DropDownList ID="ddl_GiftLuckyInput" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList></td>
							</tr>
							<tr>
							    <td class="tablecol">&nbsp;<asp:label ID="lblGiftLuckyOutput" runat ="server" CssClass = "lbl" ></asp:label></td>
							    <td class="tableinput"><asp:DropDownList ID="ddl_GiftLuckyOutput" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList></td>
							</tr>
							
							<%--'Zulham 28/03/2016 - IM5/IM6 Enhancement--%>
							<tr>
							    <td class="tablecol">&nbsp;<asp:label ID="lblIM5" runat ="server" CssClass = "lbl" ></asp:label></td>
							    <td class="tableinput"><asp:DropDownList ID="ddl_IM5" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList></td>
							</tr>
							<tr>
							    <td class="tablecol">&nbsp;<asp:label ID="lblIM6" runat ="server" CssClass = "lbl" ></asp:label></td>
							    <td class="tableinput"><asp:DropDownList ID="ddl_IM6" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList></td>
							</tr>
							
						<%--	<tr>
								<td class="tablecol">&nbsp;<asp:label ID="lblDefaultDueDate" runat ="server" CssClass = "lbl" ></asp:label></td>
								<td class="tableinput"><asp:textbox id="txt_DEFAULTDUEDATE" runat="server" CssClass="txtbox"></asp:textbox></td>
									<asp:RegularExpressionValidator id="revBCRENTAS" runat="server" Display="None" ControlToValidate="txt_BCRENTAS" ErrorMessage="Invalid Invoice Prefix."
										ValidationExpression="^[^ '][^']*$"></asp:RegularExpressionValidator></td>
										
								</tr>--%>
							
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td><asp:button id="cmd_save" runat="server" CssClass="button" Text="Save"></asp:button>
						<%--<INPUT class="button" id="cmd_Reset" type="button" value="Reset" name="Reset1" onclick="ValidatorReset();">
							<runat="server">--%></td>
					
				</tr>
				<tr>
					<td class="emptycol" valign="middle" align="center"></td>
				</tr>
				<tr>
					<td class="emptycol" valign="middle" align="left"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary></td>
				</tr>
			</table>
		</form>
	</body>
</html>
