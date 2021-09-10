<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RaisePO.aspx.vb" Inherits="eProcure.RaisePOSEH" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Raise PO</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim SelectVendor as string = dDispatcher.direct("Search","BuyerCatalogueSearchPopup.aspx","selVendor='+val+'&Raise='+Raise+'")
        </script>
        <% Response.Write(Session("JQuery")) %> 
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		window.history.forward(1);	
		<!--
		
		$(document).ready(function(){
        $('#cmdSubmit').click(function() {
        summary = Page_ValidationSummaries[0];                        
            if(summary.innerHTML == "")
            {
            document.getElementById("cmdSubmit").style.display= "none";
            document.getElementById("cmdRaise").style.display= "none";
            }      
        });
        $('#cmdRaise').click(function() {
        summary = Page_ValidationSummaries[0];                        
            if(summary.innerHTML == "")
            {
            document.getElementById("cmdSubmit").style.display= "none";
            document.getElementById("cmdRaise").style.display= "none";
            }      
        });
        });
				
			var strErrMsg;
			
			function validateQty()
			{ 
//				var j, k, ctlName, Sno;
//				strErrMsg = ''; 
//				for(i = 0; i < Page_Validators.length; i++) {
//					// validate Qty range
//					j = Page_Validators[i].id.indexOf('revRange');
//					k = Page_Validators[i].id.indexOf('revQty');
//					
//					if ((j>1)||(k>1)){ 
//						if (j>1)
//							ctlName = Page_Validators[i].id.substring(0,j) + 'hidItemLine';
//						else
//							ctlName = Page_Validators[i].id.substring(0,k) + 'hidItemLine';
//													
//						Sno = document.getElementById(ctlName).value; 
//							
//						if (Page_Validators[i].isvalid == false){alert(Page_Validators.length);
//							if (j >1)
//							
//								strErrMsg = strErrMsg + '<li>Quantity outside tolerance range.</li>';
//							else	
//								strErrMsg = strErrMsg + '<li>Invalid quantity.</li>';
//						}
//					}					
//				}
//				
//				if (strErrMsg == '')
//				    {
//					return true;}
//				else{
//					return false;	}
			}
		
		    
		    function PopWindow(myLoc)
		    {
			    window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			    return false;
		    }
		
			function deleteAttach(a)
			{
				//Form1.hidDelete.value = a;
				//alert(a);
			}
		
			function BrowseClick()
			{
				Form1.File1.click();
				Form1.txtAttached.value = Form1.File1.value;
				//return true;
			}
			
			function ValidateInput()
			{
			    var sAllClient, iPos, sCurrentClientId;
				var iQty, iPrice, iEstDate, sDevAdd, i, j, strChar;
				var strValidChars = "0123456789.,";
				var counter = 0;
				var intline = 0;
                strErrMsg = '';
				sAllClient = Form1.hidClientId.value;
				
				for (j=0; j < Form1.hidTotalClientId.value; j++)
				{
				    intline = intline + 1;
					iPos = sAllClient.indexOf('|');	
				    sCurrentClientId = sAllClient.substring(0, iPos);
				    
				    if (sCurrentClientId.length < 3)
				    {
				        return true;
				    }
				    iQty = eval("Form1.dtgShopping_" + sCurrentClientId + "_txtQty.value");
				    for (i = 0; i < iQty.length; i++)
                    {
                        strChar = iQty.charAt(i);
                        if (strValidChars.indexOf(strChar) == -1) { strErrMsg = strErrMsg + '<li>' + intline + '. Invalid quantity. Range should be from 0.01 to 999999.99</li>'; return false; }
                    }
				    if (iQty.length == 0) { strErrMsg = strErrMsg + '<li>' + intline + '. Quantity outside tolerance range.</li>'; return false };
                    if (iQty <= 0) { strErrMsg = strErrMsg + '<li>' + intline + '. Invalid quantity. Range should be from 0.01 to 999999.99</li>'; return false };				    
                    
                    /*
                    iUsage1 = eval("Form1.dtgShopping_" + sCurrentClientId + "_txtUsage1.value");
				    for (i = 0; i < iUsage1.length; i++)
                    {
                        strChar = iUsage1.charAt(i);
                        if (strValidChars.indexOf(strChar) == -1) { strErrMsg = strErrMsg + '<li>Invalid usage enter. Range should be from 0.00 to 999999.99</li>'; return false; }
                    }
                    if (iUsage1.length == 0) { strErrMsg = strErrMsg + '<li>Invalid usage enter. Range should be from 0.00 to 999999.99</li>'; return false };
                    if (iUsage1 < 0) { strErrMsg = strErrMsg + '<li>Invalid usage enter. Range should be from 0.00 to 999999.99</li>'; return false };				    
                    
                    iUsage2 = eval("Form1.dtgShopping_" + sCurrentClientId + "_txtUsage2.value");
				    for (i = 0; i < iUsage2.length; i++)
                    {
                        strChar = iUsage2.charAt(i);
                        if (strValidChars.indexOf(strChar) == -1) { strErrMsg = strErrMsg + '<li>Invalid usage enter. Range should be from 0.00 to 999999.99</li>'; return false; }
                    }
                    if (iUsage2.length == 0) { strErrMsg = strErrMsg + '<li>Invalid usage enter. Range should be from 0.00 to 999999.99</li>'; return false };
                    if (iUsage2 < 0) { strErrMsg = strErrMsg + '<li>Invalid usage enter. Range should be from 0.00 to 999999.99</li>'; return false };				    
                    
                    iUsage3 = eval("Form1.dtgShopping_" + sCurrentClientId + "_txtUsage3.value");
				    for (i = 0; i < iUsage3.length; i++)
                    {
                        strChar = iUsage3.charAt(i);
                        if (strValidChars.indexOf(strChar) == -1) { strErrMsg = strErrMsg + '<li>Invalid usage enter. Range should be from 0.00 to 999999.99</li>'; return false; }
                    }
                    if (iUsage3.length == 0) { strErrMsg = strErrMsg + '<li>Invalid usage enter. Range should be from 0.00 to 999999.99</li>'; return false };
                    if (iUsage3 < 0) { strErrMsg = strErrMsg + '<li>Invalid usage enter. Range should be from 0.00 to 999999.99</li>'; return false };				    
                    
                    iUsage8 = eval("Form1.dtgShopping_" + sCurrentClientId + "_txtUsage8.value");
				    for (i = 0; i < iUsage8.length; i++)
                    {
                        strChar = iUsage8.charAt(i);
                        if (strValidChars.indexOf(strChar) == -1) { strErrMsg = strErrMsg + '<li>Invalid current month usage enter. Range should be from 0.00 to 999999.99</li>'; return false; }
                    }
                    if (iUsage8.length == 0) { strErrMsg = strErrMsg + '<li>Invalid current month usage enter. Range should be from 0.00 to 999999.99</li>'; return false };
                    if (iUsage8 < 0) { strErrMsg = strErrMsg + '<li>Invalid current month usage enter. Range should be from 0.00 to 999999.99</li>'; return false };				    
                    */                    		
                    					    
				    iPrice = eval("Form1.dtgShopping_" + sCurrentClientId+"_txtPrice.value");
				    for (i = 0; i < iPrice.length; i++)
                    {
                        strChar = iPrice.charAt(i);
                        if (strValidChars.indexOf(strChar) == -1) { strErrMsg = strErrMsg + '<li>' + intline + '. Invalid unit price.</li>'; return false; }                                                                   
                        if(strChar == ".")
                        {
                            counter = counter + 1;
                            
                        }                                                                 
                    }
                    if(counter >= 2)
                    {
                        strErrMsg = strErrMsg + '<li>' + intline + '. Invalid unit price.</li>'; 
                        return false;
                    } 
                    else
                    {
                        counter = 0;
                    }
                    
				    if (iPrice.length == 0) { strErrMsg = strErrMsg + '<li>' + intline + '. Unit price outside tolerance range.</li>'; return false };
//                    if (iPrice <= 0) { strErrMsg = strErrMsg + '<li>Invalid unit price.</li>'; return false };
				    
//				    iEstDate = eval("Form1.dtgShopping_" + sCurrentClientId+"_txtEstDate.value");
//                    for (i = 0; i < iEstDate.length; i++)
//                    {
//                        strChar = iEstDate.charAt(i);
//                        if (strValidChars.indexOf(strChar) == -1) { strErrMsg = strErrMsg + '<li>Invalid Est. Date of Delivery (days).</li>'; return false; }
//                    }
//				    if (iEstDate.length == 0) { strErrMsg = strErrMsg + '<li>Date of delivery outside tolerance range.</li>'; return false };
//                    if (iEstDate = 0) { strErrMsg = strErrMsg + '<li>Invalid Est. Date of Delivery (days).</li>'; return false };
				    sDevAdd = eval("Form1.dtgShopping_" + sCurrentClientId + "_hidDelCode.value");
				    if (sDevAdd.length == 0) {strErrMsg = strErrMsg + '<li>' + intline + '. Invalid delivery address.</li>'; return false;}
				    
					sAllClient = sAllClient.substring(iPos+1);
				}
				return true;
			}
			
			function InitialValidation()
			{
				if (ValidateInput() == true){
				    summary = Page_ValidationSummaries[0];
					summary.innerHTML = "";
					summary.style.display = "none";
					return true;
				}
				else{
					summary = Page_ValidationSummaries[0];
					summary.innerHTML = '<ul>' + strErrMsg + '</ul>';	
					summary.style.display = "";
					return false;
				}
			}
			
		
			function cmdAddClick()
			{
			    var hidOneVendor;
			    hidOneVendor = eval("Form1.hidOneVendor.value");
                var cboVendor = document.getElementById("cboVendor");
                var val = cboVendor.options[cboVendor.selectedIndex].value;
                var Raise = "RaisePO";
                if (val == "")
                { alert ("Please select vendor."); return false; }
                else { window.open('<% Response.Write(SelectVendor) %>','Wheel','help:No,Height=580,Width=750,resizable=yes,scrollbars=yes'); return false; }
//				if (validateQty() == true){
//					var result = confirm("Save PR?", "Yes", "No");
//					if(result == true)
//						Form1.hidAddItem.value = "1";
//					else 
//						Form1.hidAddItem.value = "0";
//					return true;
//				}
//				else{
//					summary = Page_ValidationSummaries[0];
//					summary.innerHTML = '<ul>' + strErrMsg + '</ul>';	
//					summary.style.display = "";
//					return false;
//				}
                
			}
			//chkSelection
			function RemoveItemCheck(pChkSelName){
			    var oform = document.forms[0];
			    var iTotalCheckbox=0, iTotalChecked=0;
		        for (var i=0;i<oform.elements.length;i++)
			    {
				    var e = oform.elements[i];
				    if (e.type=="checkbox")
				    {
				        if ((e.name.substr(3)=="Urgent") || (e.name.substr(3)=="CustomPR") || (e.name.substr(3)=="RemarkPR"))
				        {
				        }
				        else
				        {
				            if (e.checked==true)
				            {
				                iTotalChecked+=1;
				            }
				            iTotalCheckbox+=1;
				        }
				    }
			    }
			    //if 4, dont allow to delete, at least have 1 record.
			    // if (iTotalCheckbox == 4)
			    // YapCL: 2011Mar08 - cater multiple items
			    if (iTotalCheckbox == iTotalChecked)
				{
				    alert ('You must have at least 1 row of record!');
				    return false;
				}
				if (iTotalChecked == 0)
				{
				    alert ('Please make at least one selection!');
				    return false;
				}
				else
				{
				    // YapCL: 2011Mar08 - cater yes no			    
				    var checkyesno ;
				    checkyesno = CheckAtLeastOne('chkSelection','delete');
				    if (checkyesno == true)
				    {
				        return true;
				    }
				    else
				    {
				        return false;
				    }
				}
			}
			
			
			function selectAll()
			{
				SelectAllG("dtgShopping_ctl01_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgShopping_ctl01_chkAll","chkSelection");
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
			
			function calculateTotal(type, qty, price, amt, tax, gst, taxAmt, indicator)
			{				
				Form1.hid1.value = type;
				Form1.hid2.value = qty;
				Form1.hid3.value = price;
				Form1.hid4.value = amt;
				Form1.hid5.value = tax;
				Form1.hid6.value = gst;
//				Form1.hid7.value = taxAmt;

//				var i, j, strControl, cntCtl;
				var ctlName, ctlAmount, ctlTotal, ctlTax, ctlTaxTotal, hidGST;
				var subTotal, taxTotal, taxVal;
				
				var Quantity = removeQuot(eval("Form1." + qty + ".value"));
				var UnitPrice = removeQuot(eval("Form1." + price + ".value"));
				var Amount = removeQuot(eval("Form1." + amt + ".value"));
                var Taxation = removeQuot(eval("Form1." + tax + ".value"));
                var TaxPercent = removeQuot(eval("Form1." + gst + ".value"));
                
                var counter = 0; 
                var b = 0;
                for(b = 0; b <= UnitPrice.length; b++ )
                {
                    
                    var check = UnitPrice.charAt(b);                
                    if(check == ".")
                    {
                        counter = counter + 1;                     
                    }
                }
                if(counter >= 2)
                {
                    alert("Invalid Unit Price");
                    return false;
                }
                
                if (UnitPrice=="") { UnitPrice=0; }
				ctlAmount = document.getElementById(amt);
				//2015-06-22: CH: Rounding issue (Prod issue)
				ctlAmount.value = (Quantity * UnitPrice).toFixed(2);	
														

				ctlTax = document.getElementById(tax);
				//2015-06-22: CH: Rounding issue (Prod issue)
				ctlTax.value = (ctlAmount.value  * TaxPercent / 100).toFixed(2);
							
				ctlAmount.value = addCommas(ctlAmount.value, 2); 
			    ctlTax.value = addCommas(ctlTax.value, 2);
			    
			    calculateGrandTotal();
			}
			
		    function calculateTotalWithGST(type, qty, price, amt, tax, gst, taxAmt, indicator)
			{				
				Form1.hid1.value = type;
				Form1.hid2.value = qty;
				Form1.hid3.value = price;
				Form1.hid4.value = amt;
				Form1.hid5.value = tax; //ddlGST.ClientId
				Form1.hid6.value = gst; //txtGSTAmount.ClientID
                Form1.hid7.value = taxAmt;

				var ctlName, ctlAmount, ctlTotal, ctlTax, ctlTaxTotal, hidGST;
				var subTotal, taxTotal, taxVal;
				var decAmt;
				var Quantity = removeQuot(eval("Form1." + qty + ".value"));
				var UnitPrice = removeQuot(eval("Form1." + price + ".value"));
				var Amount = removeQuot(eval("Form1." + amt + ".value"));
                var Taxation = removeQuot(eval("Form1." + tax + ".value"));
                var TaxValue = removeQuot(eval("Form1." + gst + ".value"))

                var parm  = document.getElementById(tax);
                var Percentage;
                Percentage = '';
                if (parm.options[parm.selectedIndex].text !== 'EXEMPTED' && parm.options[parm.selectedIndex].text !== '---Select---' && parm.options[parm.selectedIndex].text !== 'N/A')
                {
                    Percentage = parm.options[parm.selectedIndex].text.split('(')[1].split('%')[0];
                }

                var counter = 0; 
                var b = 0;
                for(b = 0; b <= UnitPrice.length; b++ )
                {
                    
                    var check = UnitPrice.charAt(b);                
                    if(check == ".")
                    {
                        counter = counter + 1;                     
                    }
                }
                if(counter >= 2)
                {
                    alert("Invalid Unit Price");
                    return false;
                }
                if (UnitPrice=="") { UnitPrice=0; }
				ctlTaxTotal = document.getElementById(gst);
				//2015-06-22: CH: Rounding issue (Prod issue)
				//ctlTaxTotal.value = (Percentage / 100 * Quantity * UnitPrice).toFixed(2);
				decAmt = (Quantity * UnitPrice).toFixed(2);
				ctlTaxTotal.value = (Percentage / 100 * decAmt).toFixed(2);
                                    
				ctlAmount = document.getElementById(amt);
				ctlAmount.value = (Quantity * UnitPrice).toFixed(2);
				 
				ctlAmount.value = addCommas(ctlAmount.value, 2);
			    
			    calculateGrandTotal();
			}
			
			function calculateAllIndividualTotal()
			{
			    //Calculate subtotal, total tax, and Grand total
				var sAllClient, iPos, sCurrentClientId, iInd;
				var dtgShopping;
				sAllClient = Form1.hidClientId.value;
				for (iInd=0; iInd < Form1.hidTotalClientId.value; iInd++)
				{
					iPos = sAllClient.indexOf('|');	
					
				    sCurrentClientId = sAllClient.substring(0, iPos);		
				    calculateTotal(0,"dtgShopping_" + sCurrentClientId+"_txtQty","dtgShopping_" + sCurrentClientId+"_txtPrice","dtgShopping_" + sCurrentClientId+"_txtAmount","dtgShopping_" + sCurrentClientId+"_txtGSTAmt","dtgShopping_" + sCurrentClientId+"_hidtaxperc", "0","0");
					sAllClient = sAllClient.substring(iPos+1);
				}				
			}
			
			
			function clearQuestionMark()
			{
			    var sAllClient, iPos, sCurrentClientId;
				var dtgShopping;
				sAllClient = Form1.hidClientId.value;
				for (i=0; i < Form1.hidTotalClientId.value; i++)
				{
					iPos = sAllClient.indexOf('|');	
				    sCurrentClientId = sAllClient.substring(0, iPos);
				    document.getElementById("dtgShopping_" + sCurrentClientId+"_revPriceRange").innerHTML = "";
				    
				    sAllClient = sAllClient.substring(iPos+1);
				}
			}
			
			function calculateGrandTotal()
			{
			    //Calculate subtotal, total tax, and Grand total
				var sAllClient, iPos, sCurrentClientId;
				var dSubtotal=0, dTotalTax=0, dGrandTotal=0;
				var dtgShopping;
				var temp;
				sAllClient = Form1.hidClientId.value;
				for (i=0; i < Form1.hidTotalClientId.value; i++)
				{
					iPos = sAllClient.indexOf('|');	
				    sCurrentClientId = sAllClient.substring(0, iPos);
				    dtgShopping = eval("Form1.dtgShopping_" + sCurrentClientId+"_txtAmount.value");
				    dtgShopping = dtgShopping.replace(",","");
				    dtgShopping = dtgShopping.replace(",","");
				    dSubtotal = eval(dSubtotal + parseFloat(dtgShopping.replace(",","")));
				    dtgShopping = eval("Form1.dtgShopping_" + sCurrentClientId+"_txtGSTAmt.value");
				    dtgShopping = dtgShopping.replace(",","");
				    dtgShopping = dtgShopping.replace(",","");
				    dTotalTax = eval(dTotalTax + parseFloat(dtgShopping.replace(",","")));

					sAllClient = sAllClient.substring(iPos+1);
					
				}
							
				document.getElementById('sSubTotal').innerHTML = addCommas(dSubtotal.toFixed(2), 2);
				document.getElementById('sTax').innerHTML = addCommas(dTotalTax.toFixed(2), 2);
								
				var ctlShippingTax = removeQuot(eval("Form1.txtShippingHandling.value"));
				var sGrandtotal = eval(parseFloat(ctlShippingTax) + dSubtotal + dTotalTax);
				document.getElementById('sGrandTotal').innerHTML = addCommas(sGrandtotal.toFixed(2), 2);
				
			}

			function calculateTotal1(type, qty, price, amt, tax, gst, taxAmt, indicator)
			{				
				Form1.hid1.value = type;
				Form1.hid2.value = qty;
				Form1.hid3.value = price;
				Form1.hid4.value = amt;
				Form1.hid5.value = tax;
				Form1.hid6.value = gst;
				Form1.hid7.value = taxAmt;
								
				// type: 0-product; 1-subtotal
				// cnt - number of footer subtotal
				// ctlAmount - textbox in datagrid for amount
				// ctlTotal - textbox in datagrid footer for amount
				// ctlTax - textbox in datagrid for tax
				// ctlTaxTotal - textbox in datagrid footer for tax
				//debugger;
				var i, j, strControl, cntCtl;
				var ctlName, ctlAmount, ctlTotal, ctlTax, ctlTaxTotal, hidGST;
				var subTotal, taxTotal, taxVal;
				
				var Quantity = removeQuot(eval("Form1." + qty + ".value"));
				var UnitPrice = removeQuot(eval("Form1." + price + ".value"));
				var Amount = removeQuot(eval("Form1." + amt + ".value"));
				
				hidGST = document.getElementById(gst);
				var taxVal = removeQuot(eval("Form1." + gst + ".value"));
				
				var pos = Form1.hidTotal.value.indexOf(',');
				var cnt = Form1.hidCnt.value;
				
				strControl = Form1.hidTotal.value;
				strControl = strControl.substring(pos+1, strControl.length);
				cntCtl = 0;
								
				if (!isNaN(Quantity) && (Page_IsValid)){
					// calculate amount for each item
					ctlAmount = document.getElementById(amt);
					ctlAmount.value = Quantity * UnitPrice;	
					ctlAmount.value = addCommas(ctlAmount.value, 4); 					

					for (i=0; i<Form1.hidTotal.value.length; i++){
						pos = strControl.indexOf(',');		
						if (pos > 0){
							ctlName = strControl.substring(0, pos);
							i = pos;
						}
						else{
							ctlName = strControl;
							i = Form1.hidTotal.value.length;
						}
						cntCtl = cntCtl + 1;
												
						subTotal = removeQuot(eval("Form1." + ctlName + ".value"));
						
						if (type == '0'){	// tax by product
							if (taxAmt==0){
								subTotal = subTotal - Amount + (Quantity * UnitPrice);
							}
							else {
								ctlTax = document.getElementById(tax);			
								ctlTax.value = (Quantity * UnitPrice * taxAmt)/100;
								ctlTax.value = addCommas(ctlTax.value, 4);									
								ctlTaxTotal = document.getElementById(Form1.hidTax.value);
								taxTotal = removeQuot(ctlTaxTotal.value);	
																	
								if (cntCtl == 1){	// subtotal	
									subTotal = subTotal - Amount + (Quantity * UnitPrice);
									ctlTaxTotal.value = taxTotal - taxVal + ((Quantity * UnitPrice * taxAmt)/100);
									ctlTaxTotal.value = addCommas(ctlTaxTotal.value, 2); 
									hidGST.value = (Quantity * UnitPrice * taxAmt)/100;																	
								}
								else{	// total
									subTotal = subTotal - Amount + (Quantity * UnitPrice) - taxVal + ((Quantity * UnitPrice * taxAmt)/100); 
								}
							}
						}
						else {		// tax by subtotal							
							switch (parseInt(cnt)) {
								case 2:		// all item without tax
									subTotal = subTotal - Amount + (Quantity * UnitPrice);
									//hidGST.value = (Quantity * UnitPrice * taxAmt)/100;
									break;
									
								case 3:		// all item with tax									
									switch (parseInt(cntCtl)) {
										case 1:		// subtotal
											subTotal = subTotal - Amount + (Quantity * UnitPrice);
											hidGST.value = (Quantity * UnitPrice * taxAmt)/100;
											break;
											
										case 2:		// tax
											subTotal = subTotal - taxVal + ((Quantity * UnitPrice * taxAmt)/100);
											hidGST.value = (Quantity * UnitPrice * taxAmt)/100;
											break;										

										case 3:		// total
											subTotal = subTotal - taxVal + ((Quantity * UnitPrice * taxAmt)/100) - Amount + (Quantity * UnitPrice);
											hidGST.value = (Quantity * UnitPrice * taxAmt)/100;
											break;										
									}
									break;
									
								case 4:		// mix									
									switch (parseInt(cntCtl)) {
										case 1:		// subtotal - non-taxable
											if (taxAmt == 0)
												subTotal = subTotal - Amount + (Quantity * UnitPrice);
											break;

										case 2:		// subtotal - taxable
											if (taxAmt != 0){ 	
												subTotal = subTotal - Amount + (Quantity * UnitPrice); 
												hidGST.value = (Quantity * UnitPrice * taxAmt)/100;
											}
											break;										
											
										case 3:		// tax
											if (taxAmt != 0) {
												subTotal = subTotal - taxVal + ((Quantity * UnitPrice * taxAmt)/100);
												hidGST.value = (Quantity * UnitPrice * taxAmt)/100;
											}
											break;										

										case 4:		// total - taxable
											if (taxAmt != 0){	
												subTotal = subTotal - taxVal + ((Quantity * UnitPrice * taxAmt)/100) - Amount + (Quantity * UnitPrice);
												hidGST.value = (Quantity * UnitPrice * taxAmt)/100;
											}
											else {
												subTotal = subTotal - Amount + (Quantity * UnitPrice);
											}
											break;										
									}
									break;
							}
						}// end if										
						
						ctlTotal = document.getElementById(ctlName);
						ctlTotal.value = subTotal;
						ctlTotal.value = addCommas(ctlTotal.value, 2);
						strControl = strControl.substring(pos + 1, strControl.length);
						Form1.hidCost.value = subTotal;						
					}// end for				
				}// end if
			}
			
			function focusControl(type, qty, price, amt, tax, gst, taxAmt) 
			{					
				Form1.hid1.value = type;
				Form1.hid2.value = qty;
				Form1.hid3.value = price;
				Form1.hid4.value = amt;
				Form1.hid5.value = tax;
				Form1.hid6.value = gst;
				Form1.hid7.value = taxAmt;
			}
			
			function refreshDatagrid()
			{ 		
				if (Form1.hid2.value != ''){
					
					var type, qty, price, amt, tax, gst, taxAmt;
					type = Form1.hid1.value;
					qty = Form1.hid2.value;
				    price = Form1.hid3.value;
					amt = Form1.hid4.value;
					tax = Form1.hid5.value;
					gst = Form1.hid6.value;
					taxAmt = Form1.hid7.value;
					var Quantity = eval("Form1." + qty + ".value");
				}
			}
			
			function ShowDialog(filename,height)
		    {
    			
			    var retval="";
			    retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 750px");
			    //retval=window.open(filename);
			    if (retval == "1" || retval =="" || retval==null)
			    {  
			        window.close;
				    return false;

			    } else {
			        window.close;
				    return true;

			    }
		    }
		
		-->
		</script>
	</HEAD>
	<body  MS_POSITIONING="GridLayout" runat="server" id="body1">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_AddPO_tabs"))%>
			<table class="alltable" id="table1" cellspacing="0" cellpadding="0" border="0">
				<tbody>
					<tr>
						<td class="emptycol"></td>
					</tr>
				<tr>
					<td class="header" colspan="4" style="height: 3px; width: 838px"></td>
				</tr>
					<tr>
						<td class="header"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></td>
					</tr>
					<TR>
	                    <TD colSpan="6">
		                    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                            Text="Fill in the required field(s) and click the Save button to create the PO or Submit button to submit the PO to the selected vendor."
		                    ></asp:label>

	                    </TD>
                    </TR>
					<tr>
						<td class="tableHeader" colspan="3">&nbsp;Purchase Order Header</td>
					</tr>
					<tr>
						<td valign="middle" align="left" colspan="5">
							<table class="alltable" id="table2" cellspacing="0" cellpadding="0" border="0">
								<tr valign="top">
									<td class="tablecol" align="left" width="20%" style="height: 19px">&nbsp;<strong>PO Number</strong>&nbsp;:</td>
									<td class="tableInput" width="30%" style="height: 19px"><asp:label id="lblPONo" runat="server" Width="100%"></asp:label></td>
					                <td class="tablecol" width="1%" style="height: 19px"></td>
					                <td class="tablecol" width="20%" style="height: 19px">&nbsp;<strong>Date</strong>&nbsp;:</td>
									<td class="tableInput" width="20%" style="height: 19px"><asp:label id="lblDate" Width="100%" runat="server"></asp:label></td>
									<TD class="tablecol" style="HEIGHT: 2px">&nbsp;<asp:checkbox id="chkUrgent" runat="server" Width="100px" Text="Urgent" Height="21px"></asp:checkbox></TD>
								</tr>
								<tr valign="top">
									<td class="tablecol">&nbsp;<strong>Vendor</strong><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tableInput" ><asp:dropdownlist id="cboVendor" Width="100%" runat="server" CssClass="txtbox"  AutoPostBack="True"></asp:dropdownlist>
									    <asp:requiredfieldvalidator id="revVendor" runat="server" Enabled="False" ControlToValidate="cboVendor" ErorMessage="Vendor is required " Display="None"></asp:requiredfieldvalidator></td>
					                <td class="tablecol"></td>
									<td class="tablecol" >&nbsp;<strong><asp:label id="lblVendorCodeText" runat="server">Vendor Code</asp:label></strong><asp:label id="lblVendorCodeError" runat="server" CssClass="errormsg">*</asp:label>&nbsp;<asp:label id="lblVendorCodeColon" runat="server">:</asp:label></td>
									<td class="tableinput" colspan="2"><asp:dropdownlist id="cboVendorCode" Width="80%" runat="server" CssClass="txtbox"  AutoPostBack="True"></asp:dropdownlist>
									    <asp:requiredfieldvalidator id="revVendorCode" runat="server" Enabled="False" ControlToValidate="cboVendorCode" ErorMessage="Vendor Code is required " Display="None"></asp:requiredfieldvalidator></td>
								</tr>								
								<tr valign="top">
									<td class="tablecol">&nbsp;<strong>Payment Terms</strong><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tableinput" ><asp:dropdownlist id="cboPayTerm" Width="100%" runat="server" CssClass="ddl" ></asp:dropdownlist><asp:requiredfieldvalidator id="revPaymentTerm" runat="server" Enabled="False" ControlToValidate="cboPayTerm"
											ErrorMessage="Payment Term is required " Display="None"></asp:requiredfieldvalidator></td>
					                <td class="tablecol"></td>
									<td class="tablecol" >&nbsp;<strong>Currency Code</strong>&nbsp;:</td>
									<td class="tableinput" colspan="2"><asp:label id="lblCurrencyCode" runat="server"></asp:label></td>
								</tr>								
								<tr valign="top">
									<td class="tablecol" >&nbsp;<strong>Shipment Terms</strong><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tableinput" ><asp:dropdownlist id="cboShipmentTerm" Width="100%" runat="server" CssClass="ddl"></asp:dropdownlist></td>
					                <td class="tablecol"></td>
									<td class="tablecol" >&nbsp;<strong>Payment&nbsp;Method</strong><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tableinput" colspan="2"><asp:dropdownlist id="cboPayMethod" Width="80%" runat="server" CssClass="ddl" ></asp:dropdownlist><asp:requiredfieldvalidator id="revPaymentMethod" runat="server" Enabled="False" ControlToValidate="cboPayMethod"
											ErrorMessage="Payment Method is required " Display="None"></asp:requiredfieldvalidator></td>
								</tr>
								<tr valign="top">
									<td class="tablecol">&nbsp;<strong>Ship via</strong>&nbsp;:</td>
									<td class="tableinput" ><asp:textbox id="txtShipVia" runat="server" CssClass="txtbox" Width="100%" MaxLength="30" Rows="1"></asp:textbox></td>
					                <td class="tablecol"></td>
									<td class="tablecol" >&nbsp;<strong>Shipment Mode</strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tableinput" colspan="2"><asp:dropdownlist id="cboShipmentMode" Width="80%" runat="server" CssClass="ddl"></asp:dropdownlist></td>
								</tr>
								<tr valign="top">
									<td class="tablecol">&nbsp;<strong>Bill To</strong><asp:label id="Label6" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tableinput" ><asp:dropdownlist id="cboBillCode" Width="100%" runat="server" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist></td>
									<td class="tablecol" ></td>
									<td class="tablecol" noWrap>&nbsp;<strong>Attention To</strong>&nbsp;:</td>
									<td class="tableinput" colspan="2"><asp:textbox id="txtAttention" runat="server" CssClass="txtbox" Width="80%" MaxLength="50"></asp:textbox></td>
								</tr>
								<tr valign="top">
									<td class="tablecol">&nbsp;</td>
									<td class="tableinput"><asp:textbox id="txtBillAdd1" runat="server" CssClass="txtbox" Width="100%" MaxLength="50"></asp:textbox></td>
									<td class="tablecol"></td>
									<td class="tablecol">&nbsp;<strong><asp:label id="lblDeliveryTermText" runat="server">Delivery Term</asp:label></strong><asp:label id="lblDeliveryError" runat="server" CssClass="errormsg">*</asp:label>&nbsp;<asp:label id="lblDeliveryTermColon" runat="server">:</asp:label></td>
									<td class="tableinput" colspan="2"><asp:dropdownlist id="cboDeliveryTerm" Width="80%" runat="server" CssClass="ddl" ></asp:dropdownlist>
									        <asp:requiredfieldvalidator id="revDeliveryTerm" runat="server" Enabled="False" ControlToValidate="cboDeliveryTerm" ErrorMessage="Delivery Term is required " Display="None"></asp:requiredfieldvalidator></td>
								</tr>
								<tr valign="top">
									<td class="tablecol">&nbsp;</td>
									<td class="tableinput"><asp:textbox id="txtBillAdd2" runat="server" CssClass="txtbox" Width="100%" MaxLength="50"></asp:textbox></td>
									<td class="tablecol">&nbsp;</td>
									<td class="tablecol" ></td>
									<td class="tableinput" colspan="2"></td>
								</tr>
								<tr valign="top">
									<td class="tablecol">&nbsp;</td>
									<td class="tableinput" ><asp:textbox id="txtBillAdd3" runat="server" CssClass="txtbox" Width="100%" MaxLength="50"></asp:textbox></td>
									<td class="tablecol" ></td>
									<td class="tablecol" ></td>
									<td class="tableinput" colspan="2"></td>
								</tr>
								<tr valign="top">
									<td class="tablecol">&nbsp;<strong>PostCode</strong>&nbsp;:</td>
									<td class="tableinput" ><asp:textbox id="txtBillPostCode" runat="server" CssClass="txtbox" Width="100%" MaxLength="10"></asp:textbox><asp:regularexpressionvalidator id="revPostcode" runat="server" Enabled="False" ControlToValidate="txtBillPostCode"
											ErrorMessage="Invalid Post Code" Display="None" ValidationExpression="\d{5}"></asp:regularexpressionvalidator></td>
									<td class="tablecol" ></td>
									<td class="tablecol"></td>
									<td class="tableinput" colspan="2"></td>
								</tr>
								<tr valign="top">
									<td class="tablecol">&nbsp;<strong>City</strong>&nbsp;:</td>
									<td class="tableinput" ><asp:textbox id="txtBillCity" runat="server" CssClass="txtbox" Width="100%" MaxLength="30"></asp:textbox></td>
									<td class="tablecol" ></td>
									<td class="tablecol"></td>
									<td class="tableinput" colspan="2"></td>
								</tr>
								<tr valign="top">
									<td class="tablecol">&nbsp;<strong>State</strong>&nbsp;:</td>
									<td class="tableinput" ><asp:dropdownlist id="cboState" runat="server" CssClass="txtbox" Width="100%" ></asp:dropdownlist></td>
									<td class="tablecol" ></td>
									<td class="tablecol"></td>
									<td class="tableinput" colspan="2"></td>
								</tr>
								<tr valign="top">
									<td class="tablecol">&nbsp;<strong>Country</strong>&nbsp;:</td>
									<td class="tableinput" ><asp:dropdownlist id="cboCountry" runat="server" CssClass="txtbox" Width="100%"  AutoPostBack="True"></asp:dropdownlist></td>
									<td class="tablecol" ></td>
									<td class="tablecol"></td>
									<td class="tableinput" colspan="2"></td>
								</tr>
								<tr valign="top">
									<td class="tablecol">&nbsp;<strong>Internal Remarks</strong>&nbsp;:</td>
									<td class="tableinput" ><asp:textbox id="txtInternal" runat="server" CssClass="listtxtbox" Width="100%"  MaxLength="1000"
											Rows="2" TextMode="MultiLine"></asp:textbox></td>
					                <td class="tablecol"></td>
									<td class="tablecol">&nbsp;<strong>External Remarks</strong>&nbsp;:</td>
									<td class="tableinput" colspan="2"><asp:textbox id="txtExternal" runat="server" CssClass="listtxtbox" Width="80%" MaxLength="1000"
											Rows="2" TextMode="MultiLine"></asp:textbox></td>
								</tr>
								<tr valign="top">
								
								    <td class="tablecol" noWrap align="left">&nbsp;<strong>Internal Attachment </strong>:&nbsp;<br />&nbsp;<asp:Label id="lblAttach2Int" runat="server" CssClass="small_remarks" >Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
			                        <td class="tableinput" colspan="5">
			                        <input class="button" id="File1Int" style="HEIGHT: 20px; BACKGROUND-COLOR: #ffffff;  width: 400px;" 
			                        type="file" name="uploadedFile3Int" runat="server" />&nbsp;
			                        <asp:button id="cmdUploadInt" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button></td>				            
								</tr>
								<tr valign="top">
									<td class="tablecol" style="HEIGHT: 19px">&nbsp;<strong>Internal File Attached </strong>:</td>
									<td class="tableinput" style="HEIGHT: 19px" colspan="5"><asp:panel id="pnlAttachInt" runat="server"></asp:panel></td>
								</tr>
								<tr valign="top">
								    <td class="tablecol" noWrap align="left">&nbsp;<strong>External Attachment </strong>:&nbsp;<br />&nbsp;<asp:Label id="lblAttach2" runat="server" CssClass="small_remarks" >Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
			                        <td class="tableinput" colspan="5">
			                        <input class="button" id="File1" style="HEIGHT: 20px; BACKGROUND-COLOR: #ffffff;  width: 400px;" 
			                        type="file" name="uploadedFile3" runat="server" />&nbsp;
			                        <asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button></td>				            
								</tr>
								<tr valign="top">
									<td class="tablecol" style="HEIGHT: 19px">&nbsp;<strong>External File Attached </strong>:</td>
									<td class="tableinput" style="HEIGHT: 19px" colspan="5"><asp:panel id="pnlAttach" runat="server"></asp:panel></td>
								</tr>
							</table>
						</td>
					</tr>
				</tbody>
			</table>
					    <div id="divHide" style="display:none" runat="server" >
				        <table class="alltable" id="table5" border="0" width="100%" cellspacing="0" cellpadding="0" >
								<tr valign="top">
									<td class="tableinput" colspan="5">&nbsp;If you do not want "Custom Fields" or 
										"Remark" to appear in PO, please uncheck the appropriate boxes.&nbsp;:</td>
								</tr>
								<tr valign="top">
									<td class="tableinput" colspan="5" style="HEIGHT: 2px" >&nbsp;<asp:checkbox id="chkCustomPR" runat="server" Text="Custom Fields" Height="21px"></asp:checkbox></td>
								</tr>
								<tr valign="top">
									<td class="tableinput" colspan="5">&nbsp;<asp:checkbox id="chkRemarkPR" runat="server" Text="Remark"></asp:checkbox></td>
								</tr>
								</table>
								</div>
				        <table class="alltable" id="table6" width="100%"cellspacing="0" cellpadding="0" border="0">
				
				<tr class="emptycol">
					<td colspan="2" style="width: 838px"><asp:label id="Label5" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field</td>
				</tr>
				</table>
			<table class="alltable" id="table3" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td><asp:datagrid id="dtgShopping" runat="server" AutoGenerateColumns="False" OnPageIndexChanged="dtgShopping_Page"
							DataKeyField="PRODUCTCODE">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										<asp:Label ID="lblItemLine" Runat="server" style="display:none"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="ITEMINDEX" SortExpression="ITEMINDEX"  readonly="true"  HeaderText="Line">
									<HeaderStyle HorizontalAlign="Right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="PRODUCTCODE" SortExpression="PRODUCTCODE" HeaderText="Product Code">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
								</asp:BoundColumn>
						        <asp:BoundColumn DataField="VENDORITEMCODE" SortExpression="VENDORITEMCODE"  readonly="true"    HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="115px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="GLCODE" SortExpression="GLCODE"  readonly="true"    HeaderText="GLCODE">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="GL CODE" HeaderText="(GL CODE) GL Description">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboGLCode" Width="170px" CssClass="ddl" Runat="server"></asp:DropDownList>
										<%--<input type="button" runat="server" class="button" id="btnGLSearch" 
											value=">" />--%>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CATEGORY CODE" HeaderText="Category Code">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboCategoryCode" Width="170px" CssClass="ddl" Runat="server"></asp:DropDownList>
										<%--<input type="button" runat="server" class="button" id="btnCatSearch" 
											value=">" />--%>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="TAXCODE" SortExpression="TAXCODE"  readonly="true"    HeaderText="Tax Code">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
								</asp:BoundColumn>
								
								<asp:TemplateColumn SortExpression="ASSET GROUP" HeaderText="Asset Group">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboAssetGroup" Width="100px" CssClass="ddl" Runat="server"></asp:DropDownList>										
										<asp:TextBox ID="hidAssetGroupNo" Runat="server" style="display:none;"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								
								<asp:TemplateColumn SortExpression="PRODUCTDESC" HeaderText="Item Name" >
									<HeaderStyle HorizontalAlign="Left" Width="100%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="lblProductDesc" Runat="server"></asp:Label>
										<asp:Label ID="lblProductCode" Runat="server" Visible="false" ></asp:Label>
										<asp:Label ID="lblProductGrp" Runat="server" Visible="false" ></asp:Label>
										<asp:Label id="lblItemType" Width="60px" Runat="server" Style="display:none"></asp:Label>
										<asp:Label id="lblOversea" Width="60px" Runat="server" Style="display:none"></asp:Label>
										<asp:Label id="lblPackingType" Width="60px" Runat="server" Style="display:none"></asp:Label>
										<asp:Label id="lblPackingQty" Width="60px" Runat="server" Style="display:none"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="MOQ" SortExpression="MOQ"  readonly="true"    HeaderText="MOQ">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="MPQ" SortExpression="MPQ"  readonly="true"    HeaderText="MPQ">
									<HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RFQ_QTY" SortExpression="RFQ_QTY"  Visible="false" readonly="true"    HeaderText="RFQ Qty">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="TOLERANCE" SortExpression="TOLERANCE"  Visible="false" readonly="true"    HeaderText="Quotation Qty Tolerance">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								
								<asp:TemplateColumn SortExpression="QUANTITY" HeaderText="PR Quantity">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtQty" OnTextChanged="showPrice" CssClass="numerictxtbox" Width="60px"  MaxLength="9" Runat="server" AutoPostBack="true"></asp:TextBox>
										<asp:RegularExpressionValidator id="revQty" ControlToValidate="txtQty" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,6}(\.\d{1,2})?$" Runat="server"></asp:RegularExpressionValidator>
										<asp:RangeValidator id="revRange" ControlToValidate="txtQty" Runat="server"></asp:RangeValidator>
										<input class="txtbox" id="hidItemLine" type="hidden" runat="server" name="hidItemLine" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="UOM" SortExpression="UOM"  readonly="true"    HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
								</asp:BoundColumn>		
													
								<asp:TemplateColumn SortExpression="UNITCOST" HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtPrice" CssClass="numerictxtbox" Width="60px" Runat="server"></asp:TextBox>
										<asp:RegularExpressionValidator id="revPrice" ControlToValidate="txtPrice" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,10}(\.\d{1,4})?$" Runat="server"></asp:RegularExpressionValidator>
                                        <asp:RangeValidator ID="revPriceRange" runat="server" ControlToValidate="txtPrice"></asp:RangeValidator>                                        
                                        <asp:Label id="lblNoTax" CssClass="lbl" Runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Amount" HeaderText="Amount" >
									<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtAmount" Width="58px" CssClass="numerictxtbox" Runat="server"  readonly="true"   ></asp:TextBox><input id="hidGSTAmt" type="hidden" runat="server" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="GSTRate" HeaderText="GST Rate">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:DropDownList id="cboGSTRate" Width="100px" CssClass="ddl" Runat="server"></asp:DropDownList>
										<asp:TextBox id="hidGSTRate" Runat="server" style="display:none;"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="GST" HeaderText="GST Amount">
									<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtGST" CssClass="lblnumerictxtbox" Runat="server"  readonly="true" style="display:none"></asp:TextBox>
										<asp:TextBox id="txtGSTAmt" CssClass="numerictxtbox" Runat="server"  readonly="true"  Width="45px" ></asp:TextBox>
										<asp:TextBox id="hidtaxperc" Runat="server" style="display:none;"></asp:TextBox>
										<asp:TextBox id="hidTaxID" Runat="server" style="display:none;"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<%--Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH begin--%>	
								<asp:TemplateColumn SortExpression="GstTaxCode" HeaderText="GST Tax Code (Purchase)">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:DropDownList id="cboGSTTaxCode" Width="100px" CssClass="ddl" Runat="server"></asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateColumn>
								<%--Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH end--%>
								<asp:BoundColumn Visible="False" DataField="SOURCE" SortExpression="SOURCE" HeaderText="SOURCE">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CATEGORYCODE" SortExpression="CATEGORYCODE"  readonly="true"   
									HeaderText="CATEGORYCODE">
									<HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CDGROUP" SortExpression="CDGROUP" HeaderText="CDGROUP">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								</asp:BoundColumn>
								<%--<asp:TemplateColumn SortExpression="Budget Account" HeaderText="Budget Account" Visible="false">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboBudget" CssClass="ddl" Runat="server"></asp:DropDownList>
										<input class="button" id="cmdBudget" style="HEIGHT: 18px" type="button" value=">"
											name="cmdBudget" runat="server">
									</ItemTemplate>
								</asp:TemplateColumn>--%>
								<asp:TemplateColumn SortExpression="Budget Account" HeaderText="Budget Account">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:label id="txtBudget" Runat="server" ></asp:label>
										<asp:textbox id="hidBudgetCode" Runat="server" style="display:none;"></asp:textbox>
										<input class="button" id="cmdBudget" style="WIDTH: 15px; HEIGHT: 18px" type="button"
											value="&nbsp;>&nbsp;" name="cmdBudget" runat="server" Width="30px"  />											
									</ItemTemplate>
								</asp:TemplateColumn>
								
								<asp:TemplateColumn SortExpression="Delivery Address" HeaderText="Delivery Address">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:label id="txtDelivery" Runat="server" ></asp:label>
										<asp:textbox id="hidDelCode" Runat="server" style="display:none;"></asp:textbox>
										<input class="button" id="cmdDelivery" style="WIDTH: 15px; HEIGHT: 18px" type="button"
											value="&nbsp;>&nbsp;" name="cmdDelivery" runat="server" Width="30px"  />											
									</ItemTemplate>
								</asp:TemplateColumn>
								
								<asp:TemplateColumn SortExpression="Usage4" HeaderText="Usage4">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
									    <asp:label id="lblUsage4" Width="60px" Runat="server" ></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Usage5" HeaderText="Usage5">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
									    <asp:label id="lblUsage5" Width="60px" Runat="server" ></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Usage6" HeaderText="Usage6">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
									    <asp:label id="lblUsage6" Width="60px" Runat="server" ></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Usage7" HeaderText="Past 3 Mth Average">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
									    <asp:label id="lblUsage7" Width="60px" Runat="server" ></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Usage8" HeaderText="Curr Mth Usage">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
									    <asp:TextBox id="txtUsage8" CssClass="numerictxtbox" Width="65px"  MaxLength="9" Runat="server"></asp:TextBox>
										<asp:RegularExpressionValidator id="revUsage8" ControlToValidate="txtUsage8" ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" Runat="server"></asp:RegularExpressionValidator>
										<asp:RangeValidator id="revUsage8Range" ControlToValidate="txtUsage8" Runat="server"></asp:RangeValidator>										
									</ItemTemplate>
								</asp:TemplateColumn>								
								<asp:TemplateColumn SortExpression="Usage1" HeaderText="Usage1">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:TextBox id="txtUsage1" CssClass="numerictxtbox" Width="65px"  MaxLength="9" Runat="server"></asp:TextBox>
										<asp:RegularExpressionValidator id="revUsage1" ControlToValidate="txtUsage1" ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" Runat="server"></asp:RegularExpressionValidator>
										<asp:RangeValidator id="revUsage1Range" ControlToValidate="txtUsage1" Runat="server"></asp:RangeValidator>										
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Usage2" HeaderText="Usage2">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:TextBox id="txtUsage2" CssClass="numerictxtbox" Width="65px"  MaxLength="9" Runat="server"></asp:TextBox>
										<asp:RegularExpressionValidator id="revUsage2" ControlToValidate="txtUsage2" ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" Runat="server"></asp:RegularExpressionValidator>
										<asp:RangeValidator id="revUsage2Range" ControlToValidate="txtUsage2" Runat="server"></asp:RangeValidator>										
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Usage3" HeaderText="Usage3">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:TextBox id="txtUsage3" CssClass="numerictxtbox" Width="65px"  MaxLength="9" Runat="server"></asp:TextBox>
										<asp:RegularExpressionValidator id="revUsage3" ControlToValidate="txtUsage3" ValidationExpression="^\d{1,6}(?:\.(?:\d{0,1})?\d)?$" Runat="server"></asp:RegularExpressionValidator>
										<asp:RangeValidator id="revUsage3Range" ControlToValidate="txtUsage3" Runat="server"></asp:RangeValidator>										
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Stock On Hand" HeaderText="Stock On Hand">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
									    <asp:label id="lblStockOnHand" Width="80px" Runat="server" ></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="PO Balance" HeaderText="PO Balance">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
									    <asp:label id="lblPOBalance" Width="80px" Runat="server" ></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="PO In Progress" HeaderText="PO In Progress">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
									    <asp:label id="lblPOInProgress" Width="80px" Runat="server" ></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Pur Spec No" HeaderText="Pur Spec No">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:label id="lblPurSpecNo" Width="100px" Runat="server" ></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Spec 1" HeaderText="Spec 1">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:label id="lblSpec1" Width="100px" Runat="server" ></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Spec 2" HeaderText="Spec 2">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:label id="lblSpec2" Width="100px" Runat="server" ></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Spec 3" HeaderText="Spec 3">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:label id="lblSpec3" Width="100px" Runat="server" ></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>								
								<asp:TemplateColumn SortExpression="Manufacturer Name" HeaderText="Mfg Name">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboMfgName" Width="100px" CssClass="ddl" Runat="server"></asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Lead" HeaderText="Lead Time (Days)">
									<HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
									    <asp:label id="lblLead" Width="60px" Runat="server" ></asp:label>
									</ItemTemplate>
								</asp:TemplateColumn>
								
								<asp:TemplateColumn SortExpression="Delivery Address" HeaderText="Est. Date of Delivery (dd/mm/yyyy)">
									<HeaderStyle HorizontalAlign="Left" Width="80%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtEstDate" CssClass="numerictxtbox" Width="60" Runat="server"></asp:TextBox>
										<%--<asp:RegularExpressionValidator id="revETD" ValidationExpression="^\d+$" ControlToValidate="txtEstDate" Runat="server"></asp:RegularExpressionValidator>--%>
										<%--<asp:RequiredFieldValidator ID="reqETD" ControlToValidate="txtEstDate" Runat="server"></asp:RequiredFieldValidator>--%>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Delivery Address" HeaderText="Warranty &lt;BR&gt;Terms &lt;BR&gt;(mths)">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Width="60"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtWarranty" CssClass="numerictxtbox" Runat="server"></asp:TextBox>
										<asp:RegularExpressionValidator id="revWarranty" ValidationExpression="^\d+$" ControlToValidate="txtWarranty" Runat="server"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								
								
								<asp:TemplateColumn SortExpression="REMARK" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtRemark" CssClass="listtxtbox" TextMode="MultiLine" Runat="server"></asp:TextBox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" ForeColor="Red"
											 contentEditable="false" ></asp:TextBox>										
									</ItemTemplate>
								</asp:TemplateColumn>
								
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr><td style="width:100%;"><div style="float:right;">
				    <table>
				        <tr><td style="width: 150px; text-align:right; font-weight:bold; ">Sub Total :</td><td style="width: 100px;"><div id="sSubTotal" name="sSubTotal" style="text-align:right;" >0.00</div></td></tr>
				        <tr><td id="GSTLabel" runat="server" style="text-align:right; font-weight:bold;">GST Amount :</td><td><div id="sTax" name="sTax" style="text-align:right;" >0.00</div></td></tr>
				        <tr><td style="text-align:right; font-weight:bold;">Shipping & Handling :</td><td><asp:textbox id="txtShippingHandling" runat="server" CssClass="numerictxtbox" Width="100%" MaxLength="50" Text="0.00"></asp:textbox></td></tr>
				        <tr><td colspan="2"><hr />
                            &nbsp;<asp:RegularExpressionValidator ID="revShpH" runat="server" ControlToValidate="txtShippingHandling"
                                Display="None" EnableClientScript="False" ErrorMessage="Shipping & Handling is expecting numeric value."
                                ValidationExpression="^\d{1,10}(\.\d{1,2})?$" Enabled="False"></asp:RegularExpressionValidator></td></tr>
				        <tr><td style="text-align:right; font-weight:bold;">Grand Total :</td><td><div id="sGrandTotal" style="text-align:right; font-weight:bold;">0.00</div></td></tr>
				    </table>
				</div></td></tr>
				<tr>
					<td class="emptycol" style="HEIGHT: 5px"><input class="txtbox" id="hidTotal" style="WIDTH: 43px; HEIGHT: 18px" type="hidden" size="1"
							name="hidTotal" runat="server" /> <input class="txtbox" id="hidTax" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidTax" runat="server" /> <input class="txtbox" id="hidCnt" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidCnt" runat="server" /> <input class="txtbox" id="hidCost" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidCost" runat="server" />&nbsp;<input class="txtbox" id="hidNewPO" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidNewPO" runat="server" />&nbsp;<input class="txtbox" id="hidAddItem" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidAddItem" runat="server" />&nbsp;<input class="txtbox" id="hidSupplier" style="WIDTH: 45px; HEIGHT: 18px" type="hidden"
							size="2" name="hidSupplier" runat="server" />&nbsp;<input class="txtbox" id="hidApproval" style="WIDTH: 45px; HEIGHT: 18px" type="hidden"
							size="2" name="hidApproval" runat="server" /> <input class="txtbox" id="hidDelete" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidDelete" runat="server" /> <input class="txtbox" id="hid1" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid1" runat="server" /><input class="txtbox" id="hid2" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid2" runat="server" /><input class="txtbox" id="hid3" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid3" runat="server" /><input class="txtbox" id="hid4" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid4" runat="server" /><input class="txtbox" id="hid5" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid5" runat="server" /><input class="txtbox" id="hid6" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid6" runat="server" /><input class="txtbox" id="hid7" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hid7" runat="server" />
							<input id="hidClientId" type="hidden" name="hidClientId" runat="server" />
							<input id="hidOneVendor" type="hidden" name="hidOneVendor" runat="server" />
							<input id="hidTotalClientId" type="hidden" name="hidTotalClientId" value="0" runat="server" />
					</td>
				</tr>
			</table>
			<table class="alltable" id="table4" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td style="height: 24px">
					    <asp:button id="cmdRaise" runat="server" CssClass="Button" Text="Save"></asp:button>&nbsp;
					    <asp:button id="cmdSubmit" runat="server" CssClass="Button" Text="Submit"></asp:button>&nbsp;
					    <asp:button id="cmdSetup" runat="server" CssClass="Button" Width="94px" style="display:none" Text="Approval Setup"></asp:button>&nbsp;
					    <asp:button id="cmdAdd" runat="server" CssClass="Button" CausesValidation="false" Text="Add Item"></asp:button>&nbsp;
					    <asp:button id="cmdRemove" runat="server" CssClass="Button" Width="94px" CausesValidation="False" Text="Remove Item" ></asp:button>&nbsp;
					    <asp:button id="cmdDelete" runat="server" CssClass="Button" CausesValidation="False" Text="Void PO"></asp:button>&nbsp;
					    <asp:button id="cmdDupPOLine" runat="server" CssClass="Button" Width="100px" CausesValidation="False" Text="Duplicate Line Item"></asp:button>&nbsp;&nbsp;
					    <asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none" onclick="btnHidden1_Click"></asp:button> 
					</td>
				</tr>
				<tr>
					<td style="height: 27px">&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<strong>&lt; Back</strong></asp:hyperlink></td>
				</tr>
			</table>

			
		</form>
	</body>
</HTML>
