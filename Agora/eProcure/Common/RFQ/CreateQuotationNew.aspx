<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CreateQuotationNew.aspx.vb" Inherits="eProcure.CreateQuotationNew" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html>
	<head>
		<title>CreateQuotation</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim ValidCalendar As String = "<A onclick=""window.open('" & dDispatcher.direct("RFQ", "calender.aspx", "TextBox=txt_valid")& "','cal','width=190,height=165,left=270,top=180')""><IMG height=""16"" src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' width=""16""></A>"

        </script>
        <% Response.Write(Session("JQuery")) %>        
		<% Response.Write(Session("WheelScript"))%>
		<script type="text/javascript">
		<!--	
		$(document).ready(function(){
            $('#cmd_update').click(function() {
                if (Page_IsValid)       
                    document.getElementById("cmd_update").style.display= "none";
                    document.getElementById("cmdreset").style.display= "none";
                    //document.getElementById("cmd_supply").style.display= "none";
                });
        });       
        		
		function PopWindow(myLoc)
		{
			window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
		
		function resetPostBack()
			{
				ValidatorReset();
				Form1.hidPostBack.value = "1";				
			}
			
		function check(){
				var change = document.getElementById("onchange");
				change.value ="1";
			}
			
		function removeQuot(str)
			{
				var r = new RegExp(",","gi");
				var newstr = str.replace(r, '') ;
				return newstr;
			}
			
	    function formatDec(amt, cnt)
	    {
		    var len = amt.length;
		    switch (parseInt(cnt)) {
			    case 4:
				    switch (parseInt(len)) {
					    case 1:
						    return "000" + amt;
    						
					    case 2:
						    return "00" + amt;
    						
					    case 3:
						    return "0" + amt;
    						
					    case 4:
						    return amt;						
				    }
				    break;					
    		
			    case 2:
				    switch (parseInt(len)) {
					    case 1:
						    return "0" + amt;
    						
					    case 2:
						    return amt;						
				    }
				    break;					
		    }					
	    }
	    
	    function ShowDialog(filename,height)
		{
				
			var retval="";
			retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 500px");
			//retval=window.open(filename);
			if (retval == "1" || retval =="" || retval==null)
			{  window.close;
				return false;

			} else {
			    window.close;
				return true;

			}
		}
    	
	    function formatDecimal(amt, cnt)
	    {
		    var len, posDot; 
		    var num, numDec;
		    posDot = amt.indexOf('.');

    		
		    if (posDot > 0) {
			    num = amt.substring(0, posDot);
			    numDec = amt.substring(posDot + 1, amt.length);
			    len = numDec.length;
			    switch (parseInt(cnt)) {
				    case 4:
					    if (len > 4)
						    len = 5;
					    switch (parseInt(len)) {
						    case 1:
							    return num + "." + numDec + "000";
    							
						    case 2:
							    return num + "." + numDec + "00";
    							
						    case 3:
							    return num + "." + numDec + "0";
    							
						    case 4:
							    return num + "." + numDec;
    							
						    case 5:
							    if (parseInt(numDec.substr(4,1))>4){
								    numDec = parseInt(numDec.substr(0,4), 10) + 1;	
								    if (numDec < 1000) 
									    numDec = formatDec('' + numDec, cnt);
								    else {
									    if (numDec >= 10000){											
										    var stringValue = String(numDec);
										    return (parseInt(num) + 1) + "." + stringValue.substr(1,4);	
									    }
								    }		
							    }
							    else{
								    numDec = numDec.substr(0,4);								
							    }
							    return num + "." + numDec;				
					    }
					    break;
    					
				    case 2:					
					    if (len > 2)
						    len = 3;
					    switch (parseInt(len)) {
						    case 1:
							    return num + "." + numDec + "0";
    							
						    case 2:
							    return num + "." + numDec;
    							
						    case 3:
							    if (parseInt(numDec.substr(2,1))>4){
								    numDec = parseInt(numDec.substr(0,2), 10) + 1;
								    if (numDec < 10) 
									    numDec = formatDec('' + numDec, cnt);
								    else{
									    if (numDec >= 100){										
										    var stringValue = String(numDec);
										    num = num.replace(",","");
										    return addCommas((parseInt(num) + 1) + "." + stringValue.substr(1,2),2);
									    }
								    }
							    }
							    else{
								    numDec = numDec.substr(0,2);								
							    }
							    return num + "." + numDec;				
					    }
					    break;
			    }				
		    }
		    else {
			    if (parseInt(cnt) == 4)
				    return amt + '.' + '0000';
			    else
				    return amt + '.' + '00';
		    }					
	    }
    	
	    function round(number,X) {
		    // rounds number to X decimal places, defaults to 2
		    X = (!X ? 2 : X);
		    val=Math.round(number*Math.pow(10,X))/Math.pow(10,X);
		    return val;
	    }

    	
	    function addCommas(argNum, cnt, argThouSeparator, argDecimalPoint)
	    {
		    //var argNum;
		    //argNum = '' + round(argNum, 4);
		    //alert(argNum);
		    // default separator values (should resolve to local standard)
		    var sThou = (argThouSeparator) ? argThouSeparator : ","
		    var sDec = (argDecimalPoint) ? argDecimalPoint : "."
     
		    // split the number into integer & fraction
		    var aParts = argNum.split(sDec)
		    // isolate the integer & add enforced decimal point
		    var sInt = aParts[0] + sDec
     
		    // tests for four consecutive digits followed by a thousands- or  decimal-separator
		    var rTest = new RegExp("(\\d)(\\d{3}(\\" + sThou + "|\\" + sDec + "))")
     
		    while (sInt.match(rTest))
		    {
			    // insert thousands-separator before the three digits
			    sInt = sInt.replace(rTest, "$1" + sThou + "$2")
		    }

		    // plug the modified integer back in, removing the temporary 	decimal point
		    aParts[0] = sInt.replace(sDec, "");
		    //alert(aParts.join(sDec));			
		    return formatDecimal(aParts.join(sDec), cnt);				
		    //return round(aParts.join(sDec),4);
		    //return aParts.join(sDec);
	    }
    	
	    function calculateTotal(qty, price, amt, tax, taxamt)
	    {			
		    Form1.hid1.value = qty;
		    Form1.hid2.value = price;
		    Form1.hid3.value = amt;
		    Form1.hid4.value = tax;
		    Form1.hid5.value = taxamt;
		    
		    var ctlName, ctlAmount, ctlTotal, ctlGstAmt;
		    var subTotal, taxTotal, taxVal;
		    var itemTotal;
    		
    		//if (qty==null) {qty = 0; }
    		//if (qty=="") {qty = 0; }
    		    		    		
		    var Quantity = removeQuot(eval("Form1." + qty + ".value"));
		    var UnitPrice = removeQuot(eval("Form1." + price + ".value"));
		    //taxVal = removeQuot(eval("Form1." + tax + ".value"));
		    
            if (UnitPrice=="") { UnitPrice=0; }
		    ctlAmount = document.getElementById(amt);
		    //2015-06-18: CH: Rounding issue (Prod issue)
		    //ctlAmount.value = Quantity * UnitPrice;	
		    itemTotal = (Quantity * UnitPrice).toFixed(2);	
    		ctlAmount.value = addCommas(itemTotal, 2); 
    		
    		var GstIndex = eval("Form1." + tax + ".selectedIndex");
		    var Gst = eval("Form1." + tax + ".options[GstIndex].text");
		    
		    if (Gst==null) {Gst = 0; }
    		if (Gst=="") {Gst = 0; }
    		if (Gst=="N/A") {Gst = 0; }
    		if (Gst=="---Select---") {Gst = 0; }
    		
    		if (Gst.length > 5)
            {
                //Jules 2018.10.08
                if (Gst == "ST (10%)") {
                    Gst = Gst.slice(-4, -2);
                }
                else {
                    Gst = Gst.slice(-3, -2); //original code
                }
                //End modification.
    		    
    		    if (isNaN(Gst)){
    		        Gst = 0;		         
    		    }	        
    		}

            //alert(Form1.hidGst.value);
    		Gst = parseFloat(Gst);   
		    
		    if (Form1.hidGst.value == "True") {
                ctlGstAmt = document.getElementById(taxamt);
                //2015-06-18: CH: Rounding issue (Prod issue)
                //ctlGstAmt.value = (Gst * (Quantity * UnitPrice)) / 100;
                ctlGstAmt.value = ((Gst * itemTotal) / 100).toFixed(2);
                ctlGstAmt.value = addCommas(ctlGstAmt.value, 2); 
            }
	        calculateGrandTotal();
	    }
    	
    	function calculateAllIndividualTotal()
		{
		    //Calculate subtotal, total tax, and Grand total
			var sAllClient, iPos, sCurrentClientId, iInd;
			var dg_viewitem;
			sAllClient = Form1.hidClientId.value;
			for (iInd=0; iInd < Form1.hidTotalClientId.value; iInd++)
			{
				iPos = sAllClient.indexOf('|');	
				
			    sCurrentClientId = sAllClient.substring(0, iPos);	
			    
			    	        	
			    calculateTotal("dg_viewitem_" + sCurrentClientId+"_txt_temp","dg_viewitem_" + sCurrentClientId+"_txt_price","dg_viewitem_" + sCurrentClientId+"_txt_Amount","dg_viewitem_" + sCurrentClientId+"_ddl_tax","dg_viewitem_" + sCurrentClientId+"_txt_TaxAmt");
				sAllClient = sAllClient.substring(iPos+1);
			}				
		}
			
    	function calculateGrandTotal()
	    {
	        //Calculate subtotal, total tax, and Grand total
		    var sAllClient, iPos, sCurrentClientId;
		    var dSubtotal=0, dTaxVal,dTotalTax=0, dGrandTotal=0;
		    var dg_viewitem;
		    sAllClient = Form1.hidClientId.value;
		    for (i=0; i < Form1.hidTotalClientId.value; i++)
		    {
			    iPos = sAllClient.indexOf('|');	
		        sCurrentClientId = sAllClient.substring(0, iPos);		
    		    
    		    var selectedIndex;
    		    
		        dg_viewitem = eval("Form1.dg_viewitem_" + sCurrentClientId+"_txt_Amount.value");
		        dg_viewitem = dg_viewitem.replace(",","");
				dg_viewitem = dg_viewitem.replace(",","");
		        selectedIndex = eval("Form1.dg_viewitem_" + sCurrentClientId+"_ddl_tax.selectedIndex");
		        dTaxVal = eval("Form1.dg_viewitem_" + sCurrentClientId+"_ddl_tax.options[selectedIndex].text");
		        
		        if (dTaxVal==null) {dTaxVal = 0; }
    		    if (dTaxVal=="") {dTaxVal = 0; }
    		    if (dTaxVal=="N/A") {dTaxVal = 0; }
    		    if (dTaxVal=="---Select---") {dTaxVal = 0; }
    		    
    		    if (dTaxVal.length > 5)
                {
                    //Jules 2018.10.08
                    if (dTaxVal == "ST (10%)") {
                        dTaxVal = dTaxVal.slice(-4, -2);
                    }
                    else {
                        dTaxVal = dTaxVal.slice(-3, -2); //original code
                    }
                    //End modification.
    		        
    		        if (isNaN(dTaxVal)){
    		            dTaxVal = 0;		         
    		        }	        
    		    }
		        var temp = parseFloat(dg_viewitem.replace(",",""));
		        dTaxVal = parseFloat(dTaxVal);
		        
		        //2015-06-18: CH: Rounding issue (Prod issue)
		        //dTaxVal  = ((dTaxVal * temp)/100);
		        dTaxVal  = ((dTaxVal * temp)/100).toFixed(2);
		        
		        dSubtotal = (dSubtotal + temp);	
		        dTotalTax = (dTotalTax + parseFloat(dTaxVal));	
		        
			    sAllClient = sAllClient.substring(iPos+1);
		    }
    	    //alert(dSubtotal);
    		//alert(dTotalTax);
		    document.getElementById('sSubTotal').innerHTML = addCommas(dSubtotal.toFixed(2), 2);
		    document.getElementById('sTax').innerHTML = addCommas(dTotalTax.toFixed(2), 2);
            var sGrandtotal = dSubtotal + dTotalTax;		    				
		    document.getElementById('sGrandTotal').innerHTML = addCommas(sGrandtotal.toFixed(2), 2);
    		
	    }

	    function focusControl(qty, price, amt, tax, taxamt) 
	    {					
		    Form1.hid1.value = qty;
		    Form1.hid2.value = price;
		    Form1.hid3.value = amt;
		    Form1.hid4.value = tax;
		    Form1.hid5.value = taxamt;			    	    
	    }
    	
	    function refreshDatagrid()
	    { 		
		    if (Form1.hid2.value != ''){
    			
			    var qty, price, amt, tax;
			    qty = Form1.hid1.value;
			    price = Form1.hid2.value;
		        amt = Form1.hid3.value;
		        tax = Form1.hid4.value;
		        if (qty==null) {qty = 0; }
    		    if (qty=="") {qty = 0; }
    		    if (price==null) {price = 0; }
    		    if (price=="") {price = 0; }
    		    if (amt==null) {amt = 0; }
    		    if (amt=="") {amt = 0; }			   
			    
		    }
	    }
	    
	    function PromptMsg(msg){
        var result = confirm (msg,"OK", "Cancel");		
				if(result == true)
					Form1.hidresult.value = "1";
				else 
					Form1.hidresult.value = "0";
        }	
			-->
		</script>
	</head>
	<body ms_positioning="GridLayout" runat="server" id="body1">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_RFQ_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td class="emptycol">
						<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
							<td class="linespacing1"></td>
							</tr>
							<tr>
							
								<td class="tableheader" colspan="4">&nbsp;RFQ Info</td>
							</tr>
							<tr>
					                <td  class="emptycol"></td>
			                </tr>
			                <tr>
				                <td >
					                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					                Text="Fill in the relevant info and click the Submit button to submit the quotation to the selected buyer."
					                ></asp:label>

				                </td>
			                </tr>
                            <tr>
					                <td class="emptycol"></td>
			                </tr>
							<tr>
								<td class="tablecol" width="80%">&nbsp;
								    <strong>RFQ Number </strong>:<strong> </strong>
									<asp:label id="lbl_RFQ_No" runat="server"></asp:label>
									<strong>&nbsp; Expires on </strong>:<strong> </strong>
									<asp:label id="lbl_exp_on" runat="server"></asp:label>
									<strong>&nbsp; Currency </strong>:<strong> </strong>
									<asp:label id="lbl_cur" runat="server"></asp:label>
                                    <asp:DropDownList ID="ddl_curr" runat="server" CssClass="ddl" Visible="false" >
                                    </asp:DropDownList>
									</td>
								<td class="tablecol"></td>
							</tr>							
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 19px"></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="5">
						<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
								<tr>
									<td class="tableheader" colspan="2" align="center">RFQ Details</td>
									<td width="1%"></td>
									<td class="tableheader" colspan="2" align="center">Vendor Details</td>
								</tr>
												<tr>
													<td class="tablecol" width="17%"><strong>&nbsp;Validity </strong>:</td>
													<td class="tableinput" width="28%" ><asp:label id="lbl_validity" runat="server"></asp:label></td>
									<td></td>
												<td class="tablecol" width="24%"><strong>&nbsp;Valid Till </strong>
													:</td>
												<td class="tableinput" width="30%" ><asp:textbox id="txt_valid" width="27%" runat="server" CssClass="txtbox" ></asp:textbox><% Response.Write(ValidCalendar)%>
													<asp:rangevalidator id="rv_date" runat="server" Type="Date" ControlToValidate="txt_valid" ErrorMessage="Validity Date cannot be earlier than today's date.">*</asp:rangevalidator></td>
												</tr>
												<tr>
													<td class="tablecol" ><strong>&nbsp;Contact
															&nbsp;Person </strong>:</td>
													<td class="tableinput" align="left"><asp:label id="lbl_con_person" runat="server"></asp:label>&nbsp;</td>
									<td></td>
												<td class="tablecol"><strong>&nbsp;Contact Person </strong>:</td>
												<td class="tableinput"><asp:textbox id="txt_con_person" runat="server" CssClass="txtbox" MaxLength="50" width="95%" ></asp:textbox></td>
												</tr>
												<tr>
													<td class="tablecol" ><strong>&nbsp;Contact
															&nbsp;Number </strong>:</td>
													<td class="tableinput" ><asp:label id="lbl_con_num" runat="server"></asp:label></td>
									<td></td>
												<td class="tablecol"><strong>&nbsp;Contact Number </strong>:</td>
												<td class="tableinput"><asp:textbox id="txt_con_num" runat="server" width="95%" CssClass="txtbox" MaxLength="50"></asp:textbox></td>
												</tr>
												<tr>
													<td class="tablecol" ><strong>&nbsp;Email </strong>:</td>
													<td class="tableinput" ><asp:label id="lbl_email" runat="server"></asp:label></td>
									<td></td>
												<td class="tablecol"><strong>&nbsp;Email </strong>:</td>
												<td class="tableinput"><asp:textbox id="txt_email" width="95%" runat="server" CssClass="txtbox" MaxLength="50"></asp:textbox><asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" ControlToValidate="txt_email" ErrorMessage="Invalid Email."
														ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic">*</asp:regularexpressionvalidator></td>
												</tr>
												<tr>
													<td class="tablecol" ><strong>&nbsp;Payment
															&nbsp;Terms </strong>:</td>
													<td class="tableinput" ><asp:label id="lbl_PayTerm" runat="server"></asp:label></td>
										<td></td>
												<td class="tablecol"><strong>&nbsp;Payment Terms </strong>:</td>
												<td class="tableinput"><asp:label id="lbl_QPayTerm" width="95%" runat="server"></asp:label></td>
											</tr>
												<tr>
													<td class="tablecol" ><strong>&nbsp;Payment&nbsp;
															Method </strong>:</td>
													<td class="tableinput" ><asp:label id="lbl_PayMeth" runat="server"></asp:label></td>
										<td></td>
												<td class="tablecol"><strong>&nbsp;Payment Method </strong>:</td>
												<td class="tableinput"><asp:label id="lbl_QPayMeth" width="95%" runat="server"></asp:label></td>
												</tr>
												<tr>
													<td class="tablecol" ><strong>&nbsp;Shipment
															&nbsp;Mode </strong>:</td>
													<td class="tableinput" ><asp:label id="lbl_ShipMode" runat="server"></asp:label></td>
										<td></td>
												<td class="tablecol"><strong>&nbsp;Shipment Mode </strong>:</td>
												<td class="tableinput">
													<asp:DropDownList id="cboShipmentMode" runat="server" width="95%" CssClass="ddl"></asp:DropDownList><asp:label id="lbl_QShipMode" runat="server" Visible="False"></asp:label></td>
												</tr>
												<tr>
													<td class="tablecol" ><strong>&nbsp;Shipment Terms </strong>:</td>
													<td class="tableinput" ><asp:label id="lbl_ShipTerm" runat="server"></asp:label></td>
										<td></td>
												<td class="tablecol"><strong>&nbsp;Shipment Terms </strong>:</td>
												<td class="tableinput">
													<asp:DropDownList id="cboShipmentTerm" runat="server" width="95%" CssClass="ddl"></asp:DropDownList><asp:label id="lbl_QShipTerm" runat="server" Visible="False"></asp:label></td>
												</tr>
												<tr>
													<td class="tablecol" ><strong>&nbsp;Remarks </strong>
														:</td>
													<td class="tableinput" ><asp:label id="lbl_remark" width="100%" runat="server"></asp:label></td>
										<td></td>
												<td class="tablecol"><strong>&nbsp;Remarks </strong>:</td>
												<td class="tableinput"><asp:textbox id="txt_remark" runat="server" width="95%" CssClass="lblInfo" MaxLength="1000"
														TextMode="MultiLine" Rows="3"></asp:textbox></td>
												</tr>												
												<tr>
													<td class="tablecol" ><strong>&nbsp;Attachments </strong>:</td>
													<td class="tableinput" ><asp:panel id="pnlAttach2" runat="server"></asp:panel></td>
										<td></td>
												<td class="tablecol" ><strong> &nbsp;File Attachments&nbsp; </strong>
													:<br/>
													&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="small_remarks" Visible="true">Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label>
												</td>
												<td class="tableinput" >
												<input class="button" id="File1" style="HEIGHT: 20px; BACKGROUND-COLOR: #ffffff;  width: 210px;" type="file" size="20" name="uploadedFile3" runat="server"/>&nbsp;<asp:button id="cmd_upload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button>
												</tr>
											<tr>
										<td class="tablecol" colspan="2"></td>
										<td></td>
												<td class="tablecol"><strong>&nbsp;File(s) Attached </strong>:</td>
												<td class="tableinput"><asp:panel id="pnlAttach" width="95%" runat="server"></asp:panel></td>
											</tr>
						</table>
						
					</td>
					</tr>
            <tr>
					<td class="linespacing2" colspan="4"></td>
			</tr>
				<tr>
					<td><asp:datagrid id="dg_viewitem" runat="server" CssClass="grid" AutoGenerateColumns="False" OnPageIndexChanged="dg_viewitem_Page">
							<Columns>
								<asp:BoundColumn readonly="True"  HeaderText="Item Name">
									<HeaderStyle Width="21%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="UOM">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Pack Qty">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_MPQ" runat="server" CssClass="numerictxtbox" Width="39px"></asp:TextBox>
										<asp:RegularExpressionValidator id="rev_mpq" runat="server" ControlToValidate="txt_MPQ"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="QTY">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
									    <asp:TextBox id="txt_temp"  readonly="true" runat="server" CssClass="lblnumerictxtbox2" Width="39px" MaxLength="5"></asp:TextBox>										
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="MOQ">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_MOQ" runat="server" CssClass="numerictxtbox" Width="39px"></asp:TextBox>
										<asp:RegularExpressionValidator id="rev_moq" runat="server" ControlToValidate="txt_MOQ"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="Left" Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
                                        <asp:Button ID="btn_price" runat="server" CssClass="Button" Text="Set" width="80%" Visible ="False"/>
                                        <input class="txtbox" id="hidPrice" type="hidden" name="hidPrice" runat="server"/>
										<asp:TextBox id="txt_price" runat="server" CssClass="numerictxtbox" Width="110px"></asp:TextBox>
										<asp:RegularExpressionValidator id="rev_price" runat="server" ControlToValidate="txt_price" ValidationExpression="(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Amount">
									<HeaderStyle HorizontalAlign="Right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_Amount" Width="110px" CssClass="numerictxtbox" Runat="server"  readonly="true"   >0.00</asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Tax">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_tax" runat="server" CssClass="ddl"></asp:DropDownList>								
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="SST Amount">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_TaxAmt" Width="70px" CssClass="numerictxtbox" Runat="server"  readonly="true">0.00</asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Delivery Term" Visible="false">
								    <HeaderStyle HorizontalAlign="Left" Width="14%"></HeaderStyle>
								    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
								    <ItemTemplate>
										<asp:DropDownList id="ddl_dt" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList>								
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Delivery Lead Time(days)">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_delivery" runat="server" CssClass="numerictxtbox" Width="47px" MaxLength="3"></asp:TextBox>
										<asp:RegularExpressionValidator id="rev_delivery" runat="server" ControlToValidate="txt_delivery"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Warranty Terms (mths) ">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_warranty" runat="server" CssClass="numerictxtbox" Width="55px" MaxLength="3"></asp:TextBox>
										<asp:RegularExpressionValidator id="rev_warranty" runat="server" ControlToValidate="txt_warranty"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Remarks">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle Wrap="False"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_remark" runat="server" CssClass="Remarks" Width="100%" Rows="2" TextMode="MultiLine"
											MaxLength="250"></asp:TextBox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" ForeColor="Red" Width="2px"
											 contentEditable="false" Visible="false" ></asp:TextBox><input class="txtbox" id="hidCode" type="hidden" name="hidCode" runat="server"/>																				 
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" HeaderText="lineno"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" HeaderText="productID"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" HeaderText="itemtype"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr><td style="width:100%;"><div style="float:right;">
				    <table>
				        <tr><td style="width: 150px; text-align:right; font-weight:bold; ">Sub Total :</td><td style="width: 100px;"><div id="sSubTotal" name="sSubTotal" style="text-align:right;" >0.00</div></td></tr>
				        <tr><td style="text-align:right; font-weight:bold;"><asp:label id="lblTax" runat="server">Tax</asp:label> :</td><td><div id="sTax" name="sTax" style="text-align:right;" >0.00</div></td></tr>
				        <tr><td colspan="2"><hr />
                            &nbsp;</td></tr>
				        <tr><td style="text-align:right; font-weight:bold;">Grand Total :</td><td><div id="sGrandTotal" style="text-align:right; font-weight:bold;">0.00</div></td></tr>
				    </table>
				</div></td></tr>
				<tr>
					<td class="emptycol">
						&nbsp;<asp:button id="cmd_update" runat="server" Text="Submit " CssClass="button"></asp:button>&nbsp;<input class="button" id="cmdreset" onclick="resetPostBack();" type="button" value="Reset"
							name="Button1" runat="server">
                        <asp:Button ID="cmdView" runat="server" CssClass="button" Text="View Quotation" style="WIDTH: 100px" Visible="False" />
                        <asp:Button ID="cmd_next" runat="server" CssClass="button" Text="Next >" Visible="False" /> 
                        <asp:button id="cmd_supply" runat="server" Text="Unable To Supply" style="WIDTH: 100px" CssClass="button"></asp:button>
                        <input class="button" id="cmdPreview" style="WIDTH: 75px" type="button" value="View RFQ" name="cmdPreview" runat="server"/>
                        <asp:button id ="btnHidden" runat="server" style="display:none" onclick="btnHidden_Click"></asp:button> &nbsp;
                        <asp:button id="btnhidden2" runat="server" CssClass="Button"  Text="btnhidden2" style=" display :none"></asp:button>
                                <input class="txtbox" id="hidresult" type="hidden" name="hidresult" runat="server" />
                        </td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td><asp:validationsummary id="ValidationSummary1" runat="server"></asp:validationsummary>
						<asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label><input id="hidPostBack" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidPostBack"
							runat="server"/><input id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"/><input id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"/><input id="hid1" runat="server" class="txtbox" name="hid1"
                                style="width: 17px; height: 18px" type="hidden" /><input id="hid2" runat="server"
                                    class="txtbox" name="hid2" style="width: 15px; height: 18px" type="hidden" />
                        <input id="hid3" runat="server" class="txtbox" name="hid3" style="width: 14px;
                            height: 18px" type="hidden" />
                        <input id="hidClientId" runat="server" name="hidClientId" style="width: 21px" type="hidden" />
                        <input id="hidTotalClientId" runat="server" name="hidTotalClientId" style="width: 22px"
                            type="hidden" value="0" />
                        <input id="hid4" runat="server" class="txtbox" name="hid4" style="width: 19px; height: 18px"
                            type="hidden" /><input id="hid5" runat="server" class="txtbox" name="hid5" style="width: 19px; height: 18px"
                            type="hidden" /><input id="hidGst" runat="server" name="hidGst" style="width: 22px"
                            type="hidden"/>
                            </td>
                            
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol"><a id="cmd_Previous" href="#" runat="server"><strong>&lt; Back</strong></a></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
			</table>
		</form>
	</body>
</html>
