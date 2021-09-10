<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RaiseDebitNote.aspx.vb"
    Inherits="eProcure.RaiseDebitNote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RaiseDebitNote</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <% Response.Write(Session("WheelScript")) %> 
    
    <script type="text/javascript">
        function resetPostBack()
		{
			ValidatorReset();
			//Form1.hidPostBack.value = "1";				
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
    		    		    		    		
		    var Quantity = removeQuot(eval("Form1." + qty + ".value"));
		    var UnitPrice = removeQuot(eval("Form1." + price + ".value"));
		    
            if (UnitPrice=="") { UnitPrice=0; }
		    ctlAmount = document.getElementById(amt);
		    ctlAmount.value = Quantity * UnitPrice;	
    		ctlAmount.value = addCommas(ctlAmount.value, 2); 
    		
//    		var GstIndex = eval("Form1." + tax + ".selectedIndex");
//		    var Gst = eval("Form1." + tax + ".options[GstIndex].text");
//		    
//		    if (Gst==null) {Gst = 0; }
//    		if (Gst=="") {Gst = 0; }
//    		if (Gst=="NR") {Gst = 0; }
//    		if (Gst=="N/A") {Gst = 0; }
//    		if (Gst=="---Select---") {Gst = 0; }
//    		
//    		if (Gst.length > 5)
//    		{
//    		    Gst = Gst.slice(-3,-2);
//    		    if (isNaN(Gst)){
//    		        Gst = 0;		         
//    		    }	        
//    		}
            var Gst = removeQuot(eval("Form1." + tax + ".value"));
    		Gst = parseFloat(Gst);   
		    
		    if (Form1.hidGst.value == "True") {
                ctlGstAmt = document.getElementById(taxamt);
                ctlGstAmt.value = (Gst * (Quantity * UnitPrice)) / 100;
                ctlGstAmt.value = addCommas(ctlGstAmt.value, 2); 
            }
	        calculateGrandTotal();
	    }
	    
	    function calculateAllIndividualTotal()
		{
		    //Calculate subtotal, total tax, and Grand total
			var sAllClient, iPos, sCurrentClientId, iInd;
			var dtg_DebitNoteDetail;
			sAllClient = Form1.hidClientId.value;
			for (iInd=0; iInd < Form1.hidTotalClientId.value; iInd++)
			{
				iPos = sAllClient.indexOf('|');	
				
			    sCurrentClientId = sAllClient.substring(0, iPos);	
			    
			    	        	
			    calculateTotal("dtg_DebitNoteDetail_" + sCurrentClientId+"_txtQty","dtg_DebitNoteDetail_" + sCurrentClientId+"_txtPrice","dtg_DebitNoteDetail_" + sCurrentClientId+"_txtAmount","dtg_DebitNoteDetail_" + sCurrentClientId+"_txtGST","dtg_DebitNoteDetail_" + sCurrentClientId+"_txtGSTValue");
				sAllClient = sAllClient.substring(iPos+1);
			}				
		}
			
    	function calculateGrandTotal()
	    {
	        //Calculate subtotal, total tax, and Grand total
		    var sAllClient, iPos, sCurrentClientId;
		    var dSubtotal=0, dTaxVal,dTotalTax=0, dGrandTotal=0;
		    var dtg_DebitNoteDetail;
		    sAllClient = Form1.hidClientId.value;
		    for (i=0; i < Form1.hidTotalClientId.value; i++)
		    {
			    iPos = sAllClient.indexOf('|');	
		        sCurrentClientId = sAllClient.substring(0, iPos);		
    		    
    		    var selectedIndex;
    		    
		        dtg_DebitNoteDetail = eval("Form1.dtg_DebitNoteDetail_" + sCurrentClientId+"_txtAmount.value");
		        dtg_DebitNoteDetail = dtg_DebitNoteDetail.replace(",","");
				dtg_DebitNoteDetail = dtg_DebitNoteDetail.replace(",","");
//		        selectedIndex = eval("Form1.dtg_DebitNoteDetail_" + sCurrentClientId+"_ddlGSTRate.selectedIndex");
//		        dTaxVal = eval("Form1.dtg_DebitNoteDetail_" + sCurrentClientId+"_ddlGSTRate.options[selectedIndex].text");
//		        
//		        if (dTaxVal==null) {dTaxVal = 0; }
//    		    if (dTaxVal=="") {dTaxVal = 0; }
//    		    if (dTaxVal=="N/A") {dTaxVal = 0; }
//    		    if (dTaxVal=="NR") {dTaxVal = 0; }
//    		    if (dTaxVal=="---Select---") {dTaxVal = 0; }
//    		    
//    		    if (dTaxVal.length > 5)
//    		    {
//    		        dTaxVal = dTaxVal.slice(-3,-2);
//    		        if (isNaN(dTaxVal)){
//    		            dTaxVal = 0;		         
//    		        }	        
//    		    }
                dTaxVal = eval("Form1.dtg_DebitNoteDetail_" + sCurrentClientId+"_txtGST.value");

		        var temp = parseFloat(dtg_DebitNoteDetail.replace(",",""));
		        dTaxVal = parseFloat(dTaxVal);
		    
		        dTaxVal  = ((dTaxVal * temp)/100);
		        dTaxVal = round(dTaxVal,2);
		        dSubtotal = (dSubtotal + temp);	
		        dTotalTax = (dTotalTax + dTaxVal);	
		        
			    sAllClient = sAllClient.substring(iPos+1);
		    }
    					     
		    document.getElementById('sSubTotal').innerHTML = addCommas(dSubtotal.toFixed(2), 2);
		    document.getElementById('sTax').innerHTML = addCommas(dTotalTax.toFixed(2), 2);
            var sGrandtotal = dSubtotal + dTotalTax;		    				
		    document.getElementById('sGrandTotal').innerHTML = addCommas(sGrandtotal.toFixed(2), 2);
		    //alert(sGrandtotal);
    		Form1.hidDNTotal.value = sGrandtotal.toFixed(2);
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
	    
	    //Jules 2015-Feb-23 Agora Stage 2
	    function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}				
		
    </script>
</head>
<body ms_positioning="GridLayout" runat="server" id="body1">
    <form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_Debit_Credit_tabs"))%>
        <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%"
            border="0">
            <tr>
                <td class="linespacing1">
                </td>
            </tr>
            <tr>
                <td class="header">
                    Raise Debit Note
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblAction" runat="server" CssClass="lblInfo" Text="Fill in the relevant info and click the Submit button to submit the debit note to the selected buyer."></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="linespacing2">
                </td>
            </tr>
            <tr>
                <td>
                    <table class="AllTable" id="Table2" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td class="TableHeader" colspan="4">
                                &nbsp;Debit Note</td>
                        </tr>
                        <tr class="tablecol">
                            <td class="tableinput" width="17%">
                                &nbsp;<strong>Debit Note Number</strong>&nbsp;:</td>
                                <td class="tableinput" width="30%">
                                    <asp:Label ID="lblDebitNoteNum" runat="server" Text="To Be Allocated By System"></asp:Label>
                                </td>
                                <td class="tableinput" width="20%">
                                    <strong>Invoice No.</strong>&nbsp;:
                                </td>
                                <td class="tableinput">
                                    <strong>
                                        <asp:Label ID="lblInvNo" runat="server" Text=""></asp:Label></strong>
                                </td>
                        </tr>
                        <tr class="tablecol">
                            <td class="tableinput" width="17%">
                                &nbsp;<strong>Date</strong>&nbsp;:</td>
                                <td class="tableinput">
                                    <asp:Label ID="lblDebitNoteDate" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="tableinput" width="20%">
                                    <strong>Date</strong>&nbsp;:
                                </td>
                                <td class="tableinput">
                                    <asp:Label ID="lblInvoiceDate" runat="server" Text=""></asp:Label>
                                </td>
                        </tr>
                        <tr class="tablecol">
                            <td class="tableinput" width="17%" valign="top">
                                &nbsp;<strong>Bill To</strong>&nbsp;:
                            </td>
                            <td class="tableinput">
                                <asp:Label ID="lblBuyerAddress" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="tableinput" colspan="2" valign="top">
                                <table class="AllTable" id="Table4" cellspacing="0" cellpadding="0" border="0">
                                    <tr class="tablecol">
                                        <td class="tableinput" width="39%">
                                            <strong>Currency</strong>&nbsp;:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCurrency" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="tablecol">
                                        <td class="tableinput">
                                            <strong>Invoice Amount</strong>&nbsp;:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInvoiceAmount" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="tablecol">
                                        <td class="tableinput">
                                            <strong>Related Debit Note</strong>&nbsp;:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRelatedDebitNote" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="tablecol">
                                        <td class="tableinput">
                                            <strong>Total Related Debit Note Amount</strong>&nbsp;:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRelatedDebitNoteAmt" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="tablecol">
                                        <td class="tableinput">
                                            <strong>Net Amount</strong>&nbsp;:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNetAmt" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr class="tablecol">
                            <td colspan="4">
                            </td>
                        </tr>                        
                    </table>
                    <table class="AllTable" id="Table3" cellspacing="0" cellpadding="0" border="0">
                        <tr class="tablecol">
                            <td class="tableinput" width="17%">
                                &nbsp;<strong>Attachment</strong>&nbsp;:
                                <asp:Label ID="lblAttach" runat="server" CssClass="small_remarks" Width="175px">Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
                            <td class="tablecol" style="HEIGHT: 22px" colspan="3">
                                <input class="button" id="File1Int" style="WIDTH: 300px; HEIGHT: 18px" type="file" size="17"
											name="uploadedFile3Int" runat="server"/>&nbsp;<asp:button id="cmdUpload" runat="server" CssClass="button" CausesValidation="False" Text="Upload"></asp:button><asp:label id="lblFileInt" Visible="False" Runat="server"></asp:label></td>
                        </tr>
                        <tr class="tablecol">
                            <td class="tableinput" width="17%">
                                &nbsp;<strong>File Attached</strong>&nbsp;:
                            </td>
                            <td class="tableinput" width="30%">
                                <asp:Panel ID="pnlAttach" Width="95%" runat="server">
                                </asp:Panel>
                            </td>
                            <td colspan="2">
                                &nbsp;</td>
                        </tr>
                        <tr class="tablecol">
                            <td class="tableinput" width="17%">
                                &nbsp;<strong>Remarks</strong>&nbsp;:
                            </td>
                            <td class="tableinput">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="listtxtbox" Width="300px" MaxLength="1000"
                                    Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                            <td colspan="2">
                                &nbsp;</td>
                        </tr>
                        <tr class="tablecol">
                            <td colspan="4">
                            </td>
                        </tr>
                        <tr class="emptycol">
                            <td colspan="4">
                            </td>
                        </tr>
                    </table>
                    <table class="AllTable" id="Table5" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td class="emptycol">
                                <asp:DataGrid ID="dtg_DebitNoteDetail" runat="server" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundColumn DataField="ID_INVOICE_LINE" HeaderText="Line" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_PRODUCT_DESC" HeaderText="Item Name">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_UOM" HeaderText="UOM">
                                            <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Right" Width="10%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQty" CssClass="numerictxtbox" Width="90%" MaxLength="9" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator id="valQty" Display="none" ErrorMessage="Invalid quantity." ControlToValidate="txtQty"
											Runat="server"></asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator ID="reqValQty"
                                                runat="server" Display="none" ControlToValidate="txtQty" ErrorMessage="Require Quantity."></asp:RequiredFieldValidator>
                                                <%--<asp:RegularExpressionValidator id="revQty" ControlToValidate="txtQty" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$" Runat="server"></asp:RegularExpressionValidator>--%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Unit Price">
                                            <HeaderStyle HorizontalAlign="Right" Width="10%" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPrice" CssClass="numerictxtbox" Width="90%" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator id="revPrice" runat="server" ControlToValidate="txtPrice" ValidationExpression="(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$"></asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator ID="reqValPrice"
                                                runat="server" Display="none" ControlToValidate="txtPrice" ErrorMessage="Require Price."></asp:RequiredFieldValidator>
                                                <%--<asp:RegularExpressionValidator id="rev_price" runat="server" ControlToValidate="txtPrice" ValidationExpression="(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$"></asp:RegularExpressionValidator>--%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn SortExpression="Amount" HeaderText="Amount">
                                            <HeaderStyle HorizontalAlign="Right" Width="10%" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAmount" CssClass="numerictxtbox" Width="100%" MaxLength="9" runat="server" readonly="true">0.00></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="GST_RATE" HeaderText="SST Rate">
                                            <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="SST Amount">
                                            <HeaderStyle HorizontalAlign="Right" Width="10%" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGSTValue" CssClass="numerictxtbox" Width="100%" MaxLength="9"
                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                                <asp:TextBox ID="txtGST" Width="100%" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="SST Tax Code (Supply)">
                                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlTaxCode" runat="server" CssClass="ddl" Width="100%">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Remarks">
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemark" CssClass="listtxtbox" TextMode="MultiLine" runat="server"
                                                    Width="100%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="ID_GST_RATE" HeaderText="SST Rate" Visible="false">
                                        </asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td class="emptycol" style="width: 100%;">
                                <div style="float: right;">
                                    <table>
                                        <tr>
                                            <td class="emptycol" style="width: 150px; text-align: right; font-weight: bold; color: Black;">Sub Total :</td>
                                            <td class="emptycol" style="width: 100px;">
                                                <div id="sSubTotal" name="sSubTotal" style="text-align: right; color: Black;">0.00</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="GSTLabel" runat="server" class="emptycol" style="text-align: right; font-weight: bold; color: Black;">
                                                SST Amount :</td>
                                            <td class="emptycol">
                                                <div id="sTax" name="sTax" style="text-align: right; color: Black;">0.00</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="emptycol" colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="emptycol" style="text-align: right; font-weight: bold; color: Black;">Grand Total :</td>
                                            <td class="emptycol">
                                                <div id="sGrandTotal" style="text-align: right; font-weight: bold; color: Black;">0.00</div>
                                            </td>
                                        </tr>
                                    </table>
                            </td>
                        </tr>
                        <tr class="emptycol">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="emptycol" style="height: 24px">
                                <asp:Button ID="cmdSubmit" runat="server" CssClass="Button" Text="Submit"></asp:Button>&nbsp;
                                <input class="button" id="cmdreset" onclick="resetPostBack();" type="button" value="Reset" name="Button1" runat="server"/>&nbsp;
                                <asp:Button ID="cmdView" runat="server" CssClass="Button" Text="View"></asp:Button>&nbsp;
                            </td>
                        </tr>
				        <tr>
				            <td>				                		               
				                <input id="hid1" runat="server" class="txtbox" name="hid1" style="width: 17px; height: 18px" type="hidden" />
				                <input id="hid2" runat="server" class="txtbox" name="hid2" style="width: 15px; height: 18px" type="hidden" />
                                <input id="hid3" runat="server" class="txtbox" name="hid3" style="width: 14px; height: 18px" type="hidden" />
                                <input id="hidClientId" runat="server" name="hidClientId" style="width: 21px" type="hidden" />
                                <input id="hidTotalClientId" runat="server" name="hidTotalClientId" style="width: 22px" type="hidden" value="0" />
                                <input id="hid4" runat="server" class="txtbox" name="hid4" style="width: 19px; height: 18px" type="hidden" />
                                <input id="hid5" runat="server" class="txtbox" name="hid5" style="width: 19px; height: 18px" type="hidden" />
                                <input id="hidGst" runat="server" name="hidGst" style="width: 22px" type="hidden"/>
                                <input id="hidDNTotal" runat="server" name="hidDNTotal" style="width: 22px" type="hidden"/>
				            </td>
				        </tr>
				        <tr>
					        <td class="emptycol">&nbsp;</td>
				        </tr>
				        <tr>
					        <td class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							    <strong>&lt; Back</strong></asp:hyperlink></td>
				        </tr>
				        <tr>
					        <td class="emptycol">
					            <%--<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary>--%>
					            <asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label>
					            <asp:validationsummary id="ValidationSummary1" runat="server"></asp:validationsummary>
					        </td>
				        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
