<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InvTaxDetail.aspx.vb" Inherits="eProcure.InvTaxDetail1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>InvTaxDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		
		<% Response.Write(Session("JQuery")) %>   
		<% Response.Write(Session("WheelScript"))%>
		
		<script type="text/javascript">
		
		$(document).ready(function(){
        $('#cmdSubmit').click(function() {
            document.getElementById("cmdSubmit").style.display= "none";
            document.getElementById("cmdClear").style.display= "none";
        });
        });
        
        function isDecimalKey(evt)
        {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode != 46)
                return false;

                return true;
        }
        
//        function isNumericKey(evt)
//        {
//            var charCode = (evt.which) ? evt.which : event.keyCode;
//            return (charCode<=31 || (charCode>=48 && charCode<=57));
//        }
        
        function focusControl(total, shipamt, grandtotal) 
		{					
		    Form1.hid1.value = total;
			//Form1.hid2.value = invtax;
			Form1.hid3.value = shipamt;
			//Form1.hid4.value = taxamt;
			Form1.hid5.value = grandtotal;
		}
		
		function PromptMsg(msg){
            var result = confirm (msg,"OK", "Cancel");		
			if(result == true)
				Form1.hidresult.value = "1";
			else 
				Form1.hidresult.value = "0";
        }
        
        function Reset(){
			document.forms[0].txtRef.value="";
			document.forms[0].txtRemark.value="";
		}		
        
        function calculateGrandTotal(total, shipamt, grandtotal)
	    {	
	        Form1.hid1.value = total;		
		    //Form1.hid2.value = invtax;
		    Form1.hid3.value = shipamt;
		    //Form1.hid4.value = taxamt;
		    Form1.hid5.value = grandtotal;
		    
		    //var ctlGrandtotal, ctlInvTax;
            //var decGrandtotal, decInvTax;
            var ctlGrandtotal, decGrandtotal;
		    //var TaxVal = removeQuot(eval("Form1." + invtax + ".value"));
		    var ShipAmtVal = removeQuot(eval("Form1." + shipamt + ".value"));
		    
            //if (TaxVal=="") { TaxVal=0; }
            if (ShipAmtVal=="") { ShipAmtVal=0; }
            
            //Form1.hidInvTax.value = TaxVal;
            Form1.hidShipAmt.value = ShipAmtVal;
            
            decGrandtotal = parseFloat(total) + parseFloat(ShipAmtVal);
     
//            //Calculate Invoice Tax Amount
//            ctlInvTax = document.getElementById(taxamt);
//            if (TaxVal > 0){
//                decInvTax = (parseFloat(decGrandtotal) * parseFloat(TaxVal)) / 100;
//                ctlInvTax.Value = parseFloat(decInvTax).toFixed(2);     
//        
//                $("#" + taxamt + "").html(addCommas(ctlInvTax.Value, 2)); 
//            }
//            else
//            {   
//                ctlInvTax.Value = 0;
//                $("#" + taxamt + "").html("0.00");    
//            }

            //Grand Total
            ctlGrandtotal = document.getElementById(grandtotal);
            //decGrandtotal = (parseFloat(decGrandtotal) + parseFloat(ctlInvTax.Value));
            decGrandtotal = (parseFloat(decGrandtotal));
            ctlGrandtotal.Value = parseFloat(decGrandtotal).toFixed(2); 
            $("#" + grandtotal + "").html(addCommas(ctlGrandtotal.Value, 2));  

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
	    
	    function round(number,X) {
		    // rounds number to X decimal places, defaults to 2
		    X = (!X ? 2 : X);
		    val=Math.round(number*Math.pow(10,X))/Math.pow(10,X);
		    return val;
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
	    
	    function removeQuot(str)
		{
			var r = new RegExp(",","gi");
			var newstr = str.replace(r, '') ;
			return newstr;
		}
		
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}	
        
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
					<td class="linespacing1"></td>
			</tr>
				<tr>
					<td class="header">Invoice Details
					</td>
				</tr>
            <tr>
					<td class="linespacing2"></td>
			</tr>
				<tr>
					<td>
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" width="300" border="0">
							<tr class="tableheader">
								<td width="50%" colspan="2">&nbsp;Invoice&nbsp;Details&nbsp;
								</td>
								<td width="50%" colspan="2">&nbsp;</td>
							</tr>
							<tr class="tablecol">
								<td style="HEIGHT: 28px" width="23%"><strong>&nbsp;Invoice Number </strong>:</td>
								<td class="tableinput " style="HEIGHT: 28px" width="27%">&nbsp;
									<asp:label id="lbl_InvNum" runat="server" CssClass="lblinfo"></asp:label></td>
								<td style="HEIGHT: 28px" width="23%"><strong>&nbsp;Requisitioner </strong>:
								</td>
								<td class="tableinput " style="HEIGHT: 28px" width="27%">&nbsp;
									<asp:label id="lbl_req" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%"><strong>&nbsp;Date </strong>:</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_date" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol" width="23%"><strong>&nbsp;Contact Number </strong>:
								</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_contect" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%"><strong>&nbsp;GST Reg. No. </strong>:</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_gst" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol" width="23%"><strong>&nbsp;Currency Code </strong>:
								</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_cur_code" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%"><strong>&nbsp;Your Ref. </strong>:</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_YourRef" runat="server" CssClass="lblinfo"></asp:label>
								</td>
								<td class="tablecol" width="23%"><strong>&nbsp;Attention To </strong>:&nbsp;</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_attention" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" style="HEIGHT: 24px" width="23%"><strong>&nbsp;Payment Terms&nbsp;</strong>:</td>
								<td class="tableinput " style="HEIGHT: 24px" width="27%">&nbsp;
									<asp:label id="lbl_Payterm" runat="server" CssClass="lblInfo"></asp:label></td>
								<td class="tablecol" style="HEIGHT: 24px" width="23%"><strong>&nbsp;Shipment Terms </strong>
									:
								</td>
								<td class="tableinput " style="HEIGHT: 24px" width="27%">&nbsp;
									<asp:label id="lbl_st" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr>
								<td class="tablecol" width="23%"><strong>&nbsp;Payment&nbsp;Method </strong>:</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_pm" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol" width="23%"><strong>&nbsp;Shipment Mode </strong>:</td>
								<td class="tableinput " width="27%">&nbsp;
									<asp:label id="lbl_sm" runat="server" CssClass="lblinfo"></asp:label></td>
							</tr>
							<tr id="tr_dt" runat="server">
								<td class="tablecol" width="23%"><strong>&nbsp;Delivery Term </strong>:</td>
								<td class="tableinput" width="27%">&nbsp;<asp:label id="lbl_dt" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol" width="23%"></td>
								<td class="tableinput" width="27%"></td>
							</tr>
							<tr>
								<td class="tablecol" style="HEIGHT: 51px" width="23%" valign="top"><strong>&nbsp;Bill 
										To </strong>:</td>
								<td class="tableinput " width="27%">&nbsp;<asp:label id="lbl_bill" runat="server" CssClass="lblinfo"></asp:label></td>
								<td class="tablecol" style="HEIGHT: 51px" valign="middle" width="23%"></td>
								<td class="tableinput " style="HEIGHT: 51px" width="27%"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="EMPTYCOL"></td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtg_invDetail" runat="server" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn HeaderText="Line">
									<HeaderStyle HorizontalAlign="Right" Width="8%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_Product_Desc" HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="QTY" HeaderText="Qty">
									<HeaderStyle HorizontalAlign="Right" Width="8%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UNIT_COST" HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="Right" Width="12%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Sub Total">
									<HeaderStyle HorizontalAlign="Right" Width="14%" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="GST Rate">
									<HeaderStyle HorizontalAlign="Left" Width="10%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="GST Amount">
									<HeaderStyle HorizontalAlign="Right" Width="10%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="GST Tax Code (Supply)">
								    <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:DropDownList id="ddlTaxCode" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="POD_WARRANTY_TERMS" HeaderText="Warranty Terms">
									<HeaderStyle HorizontalAlign="Right" Width="15%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
				    <td>
				        <table cellspacing="0" cellpadding="0" border="0" runat="server">
				            <tr>
				                <td>&nbsp;<strong>Reference :</strong></td>
				                <td><asp:textbox id="txtRef" Runat="server" Width="250px" MaxLength="50" CssClass="txtbox"></asp:textbox></td>
				            </tr> 
				            <tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
				            <tr>
				                <td valign="top">&nbsp;<strong>Add Remarks :</strong></td>
								<td><asp:textbox id="txtRemark" Runat="server" Width="600px" TextMode="MultiLine" MaxLength="500"
										Rows="3" CssClass="listtxtbox"></asp:textbox></td>
				            </tr> 
				            <tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<tr>
								<td colspan="2"><asp:button id="cmdSubmit" runat="server" Width="100px" CssClass="button" Text="Submit"></asp:button>
								<input class="button" id="cmdClear" onclick="Reset();" type="button"
										value="Reset" name="cmdClear" runat="server"/>
								<asp:button id="cmdView" runat="server" Width="100px" CssClass="button" Text="View Invoice"></asp:button>
								<asp:button id ="btnHidden" runat="server" style="display:none" onclick="btnHidden_Click"></asp:button>
								</td>	
							</tr>
				        </table> 
				    </td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol"><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol"><input id="hidClientId" type="hidden" name="hidClientId" runat="server" />
					    <input id="hidTotalClientId" type="hidden" name="hidTotalClientId" value="0" runat="server" />
					    <input id="hid1" type="hidden" name="hid1" runat="server"/><input id="hid2" type="hidden" name="hid2" runat="server"/>
					    <input id="hid3" type="hidden" name="hid3" runat="server"/><input id="hid4" type="hidden" name="hid4" runat="server"/>
					    <input id="hid5" type="hidden" name="hid5" runat="server"/><input id="hidInvTax" type="hidden" name="hidInvTax" runat="server"/>
					    <input id="hidShipAmt" type="hidden" name="hidShipAmt" runat="server"/><input id="hidTaxAmt" type="hidden" name="hidTaxAmt" runat="server"/>
					    <input id="hidBalShip" type="hidden" name="hidBalShip" runat="server"/><input id="hidresult" type="hidden" name="hidresult" runat="server"/>
					</td>
				</tr>
				<tr>
					<td>
					<asp:hyperlink id="lnkBack" Runat="server">
							<strong>&lt; Back</strong></asp:hyperlink>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
