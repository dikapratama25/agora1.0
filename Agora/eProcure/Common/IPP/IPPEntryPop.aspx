<%@ Page Language="vb" AutoEventWireup="false" Codebehind="IPPEntryPop.aspx.vb" Inherits="eProcure.IPPEntryPop" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Raise E2P Pop</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<% Response.Write(Session("JQuery")) %> 
        <% Response.Write(Session("AutoComplete")) %>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher            
        </script>
        <%--<% Response.write(Session("ventypeahead")) %>--%>
        <% Response.write(Session("typeahead")) %>
		<% Response.Write(Session("WheelScript"))%>
	<script type="text/javascript" >
<!--    
        
        var indicator = 0; 
		$(document).ready(function(){
		var add = 0;
		$(".amount input[type='text']").each(function(){
	
        var str = this.value;
        str = str.replace(/,/g,"");
            add += Number(str);
                     
        });        
        add = add.toFixed(2);      
        
        var num2 = add.toString().split('.');
        var thousands = num2[0].split('').reverse().join('').match(/.{1,3}/g).join(',');
        var decimals = (num2[1]) ? '.'+num2[1] : '';
        var add2 =  thousands.split('').reverse().join('')+decimals;
        //Zulham Aug 28, 2014
        var GSTInputTotal = 0; 
		var GSTOttputTotal = 0;
		
		//Zulham 06052015 IPP GST Stage 1
		var tx4GSTValue = 0;
		
		//Zulham 21/01/2016 - IPP Stage 4 Phase 2
		//amount w/o GST
		var sTotalNoGST = 0;
		
		var counter = 1
		//alert(document.getElementById("hidIsGST").value);
        //Zulham 07052015 IPP GST Stage 1
        //For NR, when IM2 is selected, the amount is not calculated
        //if (document.getElementById("hidexceedCutOffDt").value == 'Yes' && document.getElementById("hidIsGST").value == 'Yes')
        if (document.getElementById("hidexceedCutOffDt").value == 'Yes')
        {
        if (document.getElementById("hidMode").value !== 'edit'){counter = 10}
        for(i = 0; i < counter; i++)
             {
                parm  = document.getElementById("ddlInputTax" + i);
                if (parm.options[parm.selectedIndex].text.indexOf('EX') == -1 && parm.options[parm.selectedIndex].text.indexOf('ZR') == -1 && parm.options[parm.selectedIndex].text !== 'N/A' && parm.options[parm.selectedIndex].text !== '---Select---')
                  {
                    var subs = 0;
                    if (removeQuot(eval("Form1.txtGSTAmount" + i + ".value")) !== ""){subs = removeQuot(eval("Form1.txtGSTAmount" + i + ".value"))}
                    GSTInputTotal = parseFloat(GSTInputTotal) + parseFloat(subs)
                    //Zulham 08102018 - PAMB SST
                    //if (parm.options[parm.selectedIndex].text.indexOf('0') !== -1){GSTInputTotal = parseFloat(GSTInputTotal) - parseFloat(subs)}
                    
                    //Zulham 06052015 IPP GST Stage 1
                    if (parm.options[parm.selectedIndex].text.indexOf('TX4') == 0)
                    {tx4GSTValue = parseFloat(tx4GSTValue) + parseFloat(subs);}
                    
                  }
            }
        GSTInputTotal =  GSTInputTotal.toFixed(2)
        
        if (document.getElementById("hidMode").value !== 'edit'){counter = 10}
          //for(i = 0; i < counter; i++)
          //   {
          //      parm  = document.getElementById("ddlOutputTax" + i);
          //      if (parm.options[parm.selectedIndex].text.indexOf('EX') == -1 && parm.options[parm.selectedIndex].text.indexOf('ZR') == -1 && parm.options[parm.selectedIndex].text !== 'N/A' && parm.options[parm.selectedIndex].text !== '---Select---')
          //        {
          //          var subs = 0;
          //          if (removeQuot(eval("Form1.txtGSTAmount" + i + ".value")) !== ""){subs = removeQuot(eval("Form1.txtGSTAmount" + i + ".value"))}
          //          GSTOttputTotal = parseFloat(GSTOttputTotal) + parseFloat(subs)
          //          if (parm.options[parm.selectedIndex].text.indexOf('0') !== -1){GSTOttputTotal = parseFloat(GSTOttputTotal) - parseFloat(subs)}
                  
          //          //Zulham 06052015 IPP GST Stage 1
          //          if (parm.options[parm.selectedIndex].text.indexOf('TX4') == 0)
          //          {tx4GSTValue = parseFloat(tx4GSTValue) + parseFloat(subs);}
                  
          //        }
          //  }
        
        //Zulham 07052015 IPP GST Stage 1
        //TX4 Change Request
        if(parseFloat(GSTOttputTotal) > 0){
        GSTOttputTotal = parseFloat(GSTOttputTotal) - parseFloat(tx4GSTValue);}
        
        GSTOttputTotal =  GSTOttputTotal.toFixed(2)

//        //Zulham 21/01/2016 - IPP GST Stage 4 Phase 2
//        //Total w/o GST
        sTotalNoGST =  parseFloat(add2.replace(new RegExp(',','g'),''))
        sTotalNoGST = sTotalNoGST.toFixed(2)
        num2 = sTotalNoGST.toString().split('.');
        thousands = num2[0].split('').reverse().join('').match(/.{1,3}/g).join(',');
        decimals = (num2[1]) ? '.'+num2[1] : '';
        sTotalNoGST =  thousands.split('').reverse().join('')+decimals;
        $("#sTotalNoGST").html(sTotalNoGST);
               
        add2 = parseFloat(add2.replace(new RegExp(',','g'),'')) + parseFloat(GSTInputTotal) - parseFloat(GSTOttputTotal)
        
        add = add2
        add2 =  add2.toFixed(2)
        
        num2 = add2.toString().split('.');
        thousands = num2[0].split('').reverse().join('').match(/.{1,3}/g).join(',');
        decimals = (num2[1]) ? '.'+num2[1] : '';
        add2 =  thousands.split('').reverse().join('')+decimals;
        //alert('1');        
        $("#sGSTInputTotal").html(GSTInputTotal);
        $("#hidTotal").val(GSTInputTotal); 
        $("#sGSTOutputTotal").html(GSTOttputTotal);
        $("#hidTotal").val(GSTOttputTotal);
        
        //Zulham 08/03/2016 - IPP Stage 4 Phase 2
            var inputTotal = GSTInputTotal
            inputTotal = addCommas(inputTotal, 2)           
            $("#hidInputGST").val(inputTotal);
            var OutputTotal = GSTOttputTotal
            OutputTotal = addCommas(OutputTotal, 2)           
            $("#hidOutputGST").val(OutputTotal);
            var NoGSTTotal = sTotalNoGST
            NoGSTTotal = addCommas(NoGSTTotal, 2)           
            $("#hidTotalNoGST").val(NoGSTTotal);
        }              
        
        //Zulham 07052015 IPP GST Stage 1
        //For NR, when IM2 is selected, the amount is not calculated
        //if (document.getElementById("hidexceedCutOffDt").value == 'Yes' && document.getElementById("hidIsGST").value !== 'Yes')
        //{
        //GSTInputTotal = 0; 
        //GSTOttputTotal = 0;
        //GSTInputTotal =  GSTInputTotal.toFixed(2)
        //GSTOttputTotal =  GSTOttputTotal.toFixed(2)
        ////alert('2'); 
        //$("#sGSTInputTotal").html(GSTInputTotal);
        //$("#hidTotal").val(GSTInputTotal); 
        //$("#sGSTOutputTotal").html(GSTOttputTotal);
        //$("#hidTotal").val(GSTOttputTotal);
        //}
        //alert('7'); 
        $("#sTotal").html(add2);
        $("#hidTotal").val(add);      
        //End
                     
        $('#cmdSave').click(function() {          
            document.getElementById("cmdSave").style.display= "none";                 
        });
        $(".qty input[type='text']").live('textchange',function(){
        var add = 0;
  
        $(".amount input[type='text']").each(function(){
            add += Number(this.value);
        });
        add = add.toFixed(2);
        var num2 = add.toString().split('.');
        var thousands = num2[0].split('').reverse().join('').match(/.{1,3}/g).join(',');
        var decimals = (num2[1]) ? '.'+num2[1] : '';

        var add2 =  thousands.split('').reverse().join('')+decimals;
        //alert('8'); 
        $("#sTotal").html(add2);
        $("#hidTotal").val(add);
        
        //Zulham Aug 28, 2014
        if (document.getElementById("hidexceedCutOffDt").value == 'Yes' && document.getElementById("hidIsGST").value == 'Yes')
        {
        $("#sGSTInputTotal").html(add2);
        $("#hidTotal").val(add); 
        
        $("#sGSTOutputTotal").html(add2);
        $("#hidTotal").val(add);
        }   
        //End
        });
        
        $(".unit input[type='text']").blur(function(){
        var add = 0;
  
        $(".amount input[type='text']").each(function(){
        var str = this.value;
        str = str.replace(/,/g,"");
            add += Number(str);
        });
        add = add.toFixed(2);
        var num2 = add.toString().split('.');
        var thousands = num2[0].split('').reverse().join('').match(/.{1,3}/g).join(',');
        var decimals = (num2[1]) ? '.'+num2[1] : '';

        var add2 =  thousands.split('').reverse().join('')+decimals;
        if (document.getElementById("hidexceedCutOffDt").value != 'Yes' && document.getElementById("hidIsGST").value == 'Yes')
        {
        //alert('4'); 
        $("#sTotal").html(add2);
        $("#hidTotal").val(add);
        }
     
        });
         $(".qty input[type='text']").blur(function(){
        var add = 0;
  
        $(".amount input[type='text']").each(function(){
        var str = this.value;
        str = str.replace(/,/g,"");
        add += Number(str);
        });
        add = add.toFixed(2);
        var num2 = add.toString().split('.');
        var thousands = num2[0].split('').reverse().join('').match(/.{1,3}/g).join(',');
        var decimals = (num2[1]) ? '.'+num2[1] : '';

        var add2 =  thousands.split('').reverse().join('')+decimals;
      
        //$("#sTotal").html(add2);
        //$("#hidTotal").val(add);
            
        //Zulham Aug 28, 2014
        //$("#sGSTInputTotal").html(add2);
        //$("#hidTotal").val(add); 
        //        
        //$("#sGSTOutputTotal").html(add2);
        //$("#hidTotal").val(add);     
        //End
        
        });                                        
        });
        
        //Zulham Sept 15, 2014
        $(".GSTamount input[type='text']").blur(function(){
        var add = 0;        

        $(".amount input[type='text']").each(function(){
        var str = this.value;
        str = str.replace(/,/g,"");
            add += Number(str);
        });
        add = add.toFixed(2);
        var num2 = add.toString().split('.');
        var thousands = num2[0].split('').reverse().join('').match(/.{1,3}/g).join(',');
        var decimals = (num2[1]) ? '.'+num2[1] : '';

        var add2 =  thousands.split('').reverse().join('')+decimals;
        //alert('5'); 
        $("#sTotal").html(add2);
        $("#hidTotal").val(add);
     
        });
         $(".qty input[type='text']").blur(function(){
        var add = 0;
  
        $(".amount input[type='text']").each(function(){
        var str = this.value;
        str = str.replace(/,/g,"");
        add += Number(str);
        });
        add = add.toFixed(2);
        var num2 = add.toString().split('.');
        var thousands = num2[0].split('').reverse().join('').match(/.{1,3}/g).join(',');
        var decimals = (num2[1]) ? '.'+num2[1] : '';

        var add2 =  thousands.split('').reverse().join('')+decimals;
        //alert('6'); 
        $("#sTotal").html(add2);
        $("#hidTotal").val(add);
     
        //Zulham Aug 28, 2014
        if (document.getElementById("hidexceedCutOffDt").value == 'Yes' && document.getElementById("hidIsGST").value == 'Yes')
        {
        $("#sGSTInputTotal").html(add2);
        $("#hidTotal").val(add); 
        
        $("#sGSTOutputTotal").html(add2);
        $("#hidTotal").val(add);  
        }   
        //End
        
        });                                        
        //End
        function updateparam(oldstr,keystr,newstr)
        {                           
            return oldstr.replace(keystr,newstr)
        }
        function updatebtnURL(fieldCode,hidFieldCode,btnFieldCode,keystr,newkeystr)
        {
            var clickevent = $("#" + hidFieldCode).val();
            var changeclick = updateparam(clickevent,keystr,newkeystr + "=" + encodeURIComponent(fieldCode) + "&");            
            var newclick = Function(changeclick);       
            document.getElementById(btnFieldCode).onclick = newclick;
        }
         function updatebtnURLCostAlloc(fieldCode,hidFieldCode,btnFieldCode,keystr,newkeystr)
        {
            var clickevent = $("#" + hidFieldCode).val();
            var changeclick = updateparam(clickevent,keystr,newkeystr + "=" + encodeURIComponent(fieldCode) + "&");            
            var newclick = Function(changeclick); 
          document.getElementById(btnFieldCode).onclick = newclick;

           var abc = btnFieldCode.substring(12);
           document.getElementById("txtBranch" + abc).setAttribute("disabled","disabled");
           document.getElementById("txtCC" + abc).setAttribute("disabled","disabled");               
           //document.getElementById("btnBranch" + abc).setAttribute("disabled","disabled");
           //document.getElementById("btnCC" + abc).setAttribute("disabled","disabled");
        }
        function updatebtnCostAlloc(fieldCode,hidFieldCode,btnFieldCode,keystr,newkeystr)
        {
            var amount = $("#" + hidFieldCode.replace("hidCostAlloc2","txtAmt")).val();
            var clickevent = $("#" + hidFieldCode).val();
            var changeclick = updateparam(clickevent,keystr,newkeystr + "=" + encodeURIComponent(fieldCode) + "&Amount=" + amount + "&");            
            var newclick = Function(changeclick);    
            document.getElementById(btnFieldCode).onclick = newclick;
            
//            var abc = btnFieldCode.substring(13);
//            $("#txtBranch" + abc).val('');
//            $("#txtCC" + abc).val('');   
         }     

        //Jules 2018.07.05
        function updatebtnWithHoldTaxURL(fieldCode, hidFieldCode, btnFieldCode, keystr, newkeystr, opt) {
            var clickevent = $("#" + hidFieldCode).val();
            var changeclick = updateparam(clickevent, keystr, newkeystr + "=" + encodeURIComponent(fieldCode) + "&opt=" + encodeURIComponent(opt) + "&");
            var newclick = Function(changeclick);
            document.getElementById(btnFieldCode).onclick = newclick;
            var abc = btnFieldCode.substring(13);                        
            document.getElementById("txtWithholdingOpt" + abc).value = opt;
            document.getElementById("hidHoldingTaxOpt" + abc).value = opt;
        }

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
          
        		    function showHide1(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
                    }
            }
            function focusControl(qty, price,amt) 
			{					
//				Form1.hid1.value = type;
				Form1.hid1.value = qty;
				Form1.hid2.value = price;
				Form1.hid3.value = amt;
//				Form1.hid5.value = tax;
//				Form1.hid6.value = gst;
//				Form1.hid7.value = taxAmt;
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
		function onClick(i) 
        {
                var bt = document.getElementById("hidButton");
                if(i){
                    //$("#txtBranch"+ i).val("");
                    //$("#txtCC"+ i).val("");
                }
                bt.click();
            }
            
        function getLineNo(i)
            {
                var lineNo = document.getElementById("lineNo"); 
                $("#lineNo").val(i);                    
            }    
            
        function onClick2() 
            { 
                var bt2 = document.getElementById("hidButton1"); 
                bt2.click(); 
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
            strErrMsg = '';
			sAllClient = Form1.hidClientId.value;
			
			for (j=0; j < Form1.hidTotalClientId.value; j++)
			{
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
                    if (strValidChars.indexOf(strChar) == -1) { strErrMsg = strErrMsg + '<li>Invalid quantity.</li>'; return false; }
                }
			    if (iQty.length == 0) { strErrMsg = strErrMsg + '<li>Quantity outside tolerance range.</li>'; return false };
                if (iQty <= 0) { strErrMsg = strErrMsg + '<li>Invalid quantity.</li>'; return false };				    
			    
			    iPrice = eval("Form1.dtgShopping_" + sCurrentClientId+"_txtPrice.value");
                for (i = 0; i < iPrice.length; i++)
                {
                    strChar = iPrice.charAt(i);
                    if (strValidChars.indexOf(strChar) == -1) { strErrMsg = strErrMsg + '<li>Invalid unit price.</li>'; return false; }
                }
			    if (iPrice.length == 0) { strErrMsg = strErrMsg + '<li>Unit price outside tolerance range.</li>'; return false };
			    sDevAdd = eval("Form1.dtgShopping_" + sCurrentClientId + "_hidDelCode.value");
			    if (sDevAdd.length == 0) {strErrMsg = strErrMsg + '<li>Invalid delivery address.</li>'; return false;}
			    
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
				return formatDecimal(aParts.join(sDec), cnt);				
				//return round(aParts.join(sDec),4);
				//return aParts.join(sDec);
			}					
		    function isValidDecimalCheck()
            {
                var Amt = Form1.txtPaymentAmt.value
                var i = 0;
                var counter = 0;
                for(i = 0; i <= Amt.length; i++ )
                {
                    var check = Amt.charAt(i);
                    if(check == ".")
                    {
                        counter = counter + 1;
                    }
                }
                if(counter >= 2)
                {
                    alert("Invalid Amount");
                    return false;
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
			function calculateTotal(qty, price,amt)
			{ 
			    var itemSum = 0
			    $(".amount input[type='text']").each(function(){
                str = this.value;
                str = str.replace(/,/g,"");
                    itemSum += Number(str);
                });		

				Form1.hid1.value = qty;
				Form1.hid2.value = price;
				Form1.hid3.value = amt;

				var ctlName, ctlAmount, ctlTotal, ctlTax, ctlTaxTotal, hidGST;
                var subTotal, taxTotal, taxVal, totalAll;
                //Zulham 05102018 - PAMB SST
                //Set qty to 1 and unitprice to amt
				var Quantity = 1//removeQuot(eval("Form1." + qty + ".value"));
                var UnitPrice = removeQuot(eval("Form1." + amt + ".value"));
                var Amount = removeQuot(eval("Form1." + amt + ".value"));
                //Zulham 08102018 - PAMB SST
				var getIndex = amt.substr(6, 1); 

                var i = 0;
                var counter = 0;
                for(i = 0; i <= UnitPrice.length; i++ )
                {
                    var check = UnitPrice.charAt(i);
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
                
                counter = 0;
                for(i = 0; i <= Quantity.length; i++ )
                {
                    var check = Quantity.charAt(i);
                    if(check == ".")
                    {
                        counter = counter + 1;
                    }
                }
                if(counter >= 2)
                {
                    alert("Invalid Quantity");
                    return false;
                } 

                if (UnitPrice=="") { UnitPrice=0; }
				ctlAmount = document.getElementById(amt);
				ctlAmount.value = Quantity * UnitPrice;	
				//Zulham 11102018 - PAMB											
				//ctlAmount.value = addCommas(ctlAmount.value, 2);
               
				if (document.getElementById("hidexceedCutOffDt").value == 'Yes' && document.getElementById("hidIsGST").value == 'Yes' || document.getElementById("hidResidentType").value == 'N' || document.getElementById("hidCoyType").value == 'E')
                {
                    calculateTotalWithGST('ddlInputTax' + getIndex,'ddlOutputTax' + getIndex,'txtGSTAmount' + getIndex, ctlAmount.value) //Zulham Oct 02, 2014	
                }
                else
               {      
                     var itemSum = 0
			         $(".amount input[type='text']").each(function(){
                     str = this.value;
                     str = str.replace(/,/g,"");
                        itemSum += Number(str);
                     });	
                        
                      var totalAll = 0
                      totalAll = parseFloat(itemSum);
                      totalAll = totalAll.toFixed(2);//(totalAll, 2);
                      $("#sTotal").html(totalAll);
                      $("#hidTotal").val(totalAll);
                }
            }
            
            //'Zulham Aug 28, 2014
            function calculateTotalWithGST(ddlInput, ddlOutput, amt, invoiceAmt)
			{ 
			    var Percentage;
			    //Zulham 06052015 IPP GST Stage 1
			    var tx4GSTValue;
			    tx4GSTValue = 0;
			     
                Percentage = 0;
			    var add = 0
			    $(".GSTamount input[type='text']").each(function(){
                var str = this.value;                
                str = str.replace(/,/g,"");
                    add += Number(str);
                });
                               
                var itemSum = 0
			    $(".amount input[type='text']").each(function(){
                str = this.value;
                str = str.replace(/,/g,"");
                    itemSum += Number(str);
                });	
				var ctlName, ctlAmount, ctlTotal, ctlTax, ctlTaxTotal, hidGST
				var GSTInputTotal = 0; 
				var GSTOttputTotal = 0;
				var subTotal, taxTotal, taxVal ;
				var ddlInputVal = removeQuot(eval("Form1." + ddlInput + ".value"));
				var ddlOutputVal = removeQuot(eval("Form1." + ddlOutput + ".value"));
				var Amount = removeQuot(eval("Form1." + amt + ".value"));
				var i = 0;
                var counter = 0;
                var rowCount = 1;

                if (Amount=="") { Amount=0; }
				ctlAmount = document.getElementById(amt);

				var GSTItemAmount = ctlAmount.value; 
				ctlAmount.value = GSTItemAmount//addCommas(GSTItemAmount.toFixed(2), 2)
                var parm  = document.getElementById(ddlInput);
                if (parm.options[parm.selectedIndex].text.indexOf('EX') == -1 && parm.options[parm.selectedIndex].text.indexOf('ZR') == -1 && parm.options[parm.selectedIndex].text.indexOf('NR') == -1 && parm.options[parm.selectedIndex].text !== 'N/A' && parm.options[parm.selectedIndex].text !== '---Select---')
                {
                   //GSTInputTotal = add;
                }
                //GSTInputTotal = add - ctlAmount.value
                if (document.getElementById("hidMode").value !== 'edit'){counter = 10}
                else if(document.getElementById("hidMode").value == 'edit')
                {
                //Calculation for item
                //Jules 2018.08.16 - Commented this line.
                //parm  = document.getElementById("ddlInputTax" + i);
                //End modification.
                        //if (parm.options[parm.selectedIndex].text.indexOf('IM2') !== -1 || parm.options[parm.selectedIndex].text.indexOf('TX4') !== -1)
                          //{    
                            if (parm.options[parm.selectedIndex].text.indexOf('IM5') == -1 && parm.options[parm.selectedIndex].text !== 'N/A' && parm.options[parm.selectedIndex].text !== '---Select---')
                            {                                           
                                Percentage = parm.options[parm.selectedIndex].text.split('(')[1].split('%')[0];                                
                                //GSTItemAmount = parseFloat(invoiceAmt) * (parseFloat(Percentage)/100);
                                GSTItemAmount = parseFloat(invoiceAmt.replace(/,/g,'')) * (parseFloat(Percentage)/100);
                                GSTItemAmount = GSTItemAmount.toFixed(2);                                
                                ctlAmount.value = GSTItemAmount;
                                GSTInputTotal = parseFloat(GSTItemAmount);
                            }
                            //Zulham 06052015 IPP GST Stage 1
                            if (parm.options[parm.selectedIndex].text.indexOf('IM2') !== -1 && parm.options[parm.selectedIndex].text !== 'N/A' && parm.options[parm.selectedIndex].text !== '---Select---')
                            {GSTOttputTotal = parseFloat(GSTItemAmount);}
                            
                          //}
                        if (parm.options[parm.selectedIndex].text.indexOf('EX') == -1 && parm.options[parm.selectedIndex].text.indexOf('ZR') == -1 && parm.options[parm.selectedIndex].text.indexOf('NR') == -1 && parm.options[parm.selectedIndex].text !== 'N/A' && parm.options[parm.selectedIndex].text !== '---Select---')
                        {                            
                            GSTItemAmount = ctlAmount.value;                            
                            GSTItemAmount = GSTItemAmount.replace(' ','');
                            if (GSTItemAmount == '') { GSTItemAmount = 0 }
                            //Zulham 08102018 - PAMB SST
                            //if (parm.options[parm.selectedIndex].text.indexOf('0') !== -1){GSTItemAmount = parseFloat(GSTItemAmount) - parseFloat(GSTItemAmount)}                          
                            //GSTItemAmount = GSTItemAmount.toFixed(2);
                            ctlAmount.value = GSTItemAmount;
                            GSTInputTotal = parseFloat(GSTItemAmount);
                            parm  = document.getElementById("ddlOutputTax" + i);
                            if (parm.options[parm.selectedIndex].text.indexOf('EX') == -1 && parm.options[parm.selectedIndex].text.indexOf('ZR') == -1 && parm.options[parm.selectedIndex].text.indexOf('NR') == -1 && parm.options[parm.selectedIndex].text !== 'N/A' && parm.options[parm.selectedIndex].text !== '---Select---')
                            {GSTOttputTotal = parseFloat(GSTItemAmount);}
                        }
                        //Jules 2018.08.17
                        if (parm.options[parm.selectedIndex].text == '---Select---') {
                            GSTItemAmount = 0;
                            ctlAmount.value = GSTItemAmount;
                        }
                        //End modification.
                //
                }else{}
                for(i = 0; i < counter; i++)
                {                        
                    parm  = document.getElementById("ddlInputTax" + i);                        
                    if (parm.options[parm.selectedIndex].text.indexOf('EX') == -1 && parm.options[parm.selectedIndex].text.indexOf('NR') == -1 && parm.options[parm.selectedIndex].text.indexOf('ZR') == -1 && parm.options[parm.selectedIndex].text.indexOf('NR') == -1 && parm.options[parm.selectedIndex].text !== 'N/A' && parm.options[parm.selectedIndex].text !== '---Select---' )
                    {                            
                    //Zulham 15/01/2016 - IPP Stage 4 Phase 2
                    //Removed the condition below
                        //if (parm.options[parm.selectedIndex].text.indexOf('IM2') !== -1 || parm.options[parm.selectedIndex].text.indexOf('TX4') !== -1)
                            //{
                            if (typeof invoiceAmt !== 'undefined')
                            {
                                if(parm.options[parm.selectedIndex].text.indexOf('IM5') == -1)
                                {                                    
                                    Percentage = parm.options[parm.selectedIndex].text.split('(')[1].split('%')[0];
                                    GSTItemAmount = parseFloat(invoiceAmt.replace(/,/g,'')) * (parseFloat(Percentage)/100);
                                    GSTItemAmount = GSTItemAmount.toFixed(2);                                    
                                    if (ctlAmount.id.substring(12) == i ){
                                        ctlAmount.value = GSTItemAmount;                                        
                                    }
                                }                                   
                            }
                            //}
//                            else
//                                  { 
                            var subs = 0;
                            if (removeQuot(eval("Form1.txtGSTAmount" + i + ".value")) !== ""){
                                subs = removeQuot(eval("Form1.txtGSTAmount" + i + ".value"));                                        
                            }
                            GSTInputTotal = parseFloat(GSTInputTotal) + parseFloat(subs);
                                    
                            //Zulham 06052015 IPP GST STage 1
                            if (parm.options[parm.selectedIndex].text.indexOf('TX4') !== -1)
                            {                                           
                                tx4GSTValue = parseFloat(tx4GSTValue) + parseFloat(subs);                                        
                            }

                            //Zulham 09102018 - PAMB SST        
                            //if (parm.options[parm.selectedIndex].text.indexOf('0') !== -1){                                        
                            //    GSTInputTotal = parseFloat(GSTInputTotal) - parseFloat(subs);                                        
                            //}
//                                  }
                               
                        //Zulham 28/01.2016 - IPP GST Stage 4 Phase 2
//                                if (i == ddlInput.substring(ddlInput.length-1,ddlInput.length))
//                                {alert(document.getElementById("hidHeaderGstAmount").value)
//                                }                                                               
                    }
                    //Jules 2018.08.16
                    else if (parm.options[parm.selectedIndex].text == '---Select---') {
                        if (typeof invoiceAmt !== 'undefined' && ctlAmount.id.substring(12) == i) {
                            GSTItemAmount = 0;
                            ctlAmount.value = GSTItemAmount.toFixed(2);
                        }
                    }
                    //End modification.
                }
                parm  = document.getElementById(ddlOutput);
                if (parm.options[parm.selectedIndex].text.indexOf('EX') == -1 && parm.options[parm.selectedIndex].text.indexOf('ZR') == -1 && parm.options[parm.selectedIndex].text.indexOf('NR') == -1 && parm.options[parm.selectedIndex].text !== 'N/A' && parm.options[parm.selectedIndex].text !== '---Select---')
                {   
                    //GSTOttputTotal = add 
                }

                //GSTOttputTotal = add - ctlAmount.value
                if (document.getElementById("hidMode").value !== 'edit'){counter = 10}
                  for(i = 0; i < counter; i++)
                     {
                        parm  = document.getElementById("ddlOutputTax" + i);
                        if (parm.options[parm.selectedIndex].text.indexOf('EX') == -1 && parm.options[parm.selectedIndex].text.indexOf('ZR') == -1 && parm.options[parm.selectedIndex].text.indexOf('NR') == -1 && parm.options[parm.selectedIndex].text !== 'N/A' && parm.options[parm.selectedIndex].text !== '---Select---')
                          {
                            var subs = 0;
                            if (removeQuot(eval("Form1.txtGSTAmount" + i + ".value")) !== ""){subs = removeQuot(eval("Form1.txtGSTAmount" + i + ".value"))}
                            GSTOttputTotal = parseFloat(GSTOttputTotal) + parseFloat(subs)
                            if (parm.options[parm.selectedIndex].text.indexOf('0') !== -1){GSTOttputTotal = parseFloat(GSTOttputTotal) - parseFloat(subs)}
                          }
                    }
                
                //Zulham Aug 28, 2014

                $("#sGSTInputTotal").html(addCommas(GSTInputTotal.toFixed(2), 2));
                $("#hidInputGST").val(addCommas(GSTInputTotal.toFixed(2), 2));
                
                //Zulham 06052015 IPP GST Stage 1
                GSTOttputTotal = parseFloat(GSTOttputTotal) - parseFloat(tx4GSTValue);           
                $("#sGSTOutputTotal").html(addCommas(GSTOttputTotal.toFixed(2), 2));
                $("#hidOutputGST").val(addCommas(GSTOttputTotal.toFixed(2), 2));  
                
                //hidOutputGST
                var totalAll = parseFloat(itemSum) + GSTInputTotal - GSTOttputTotal;
                $("#sTotal").html(addCommas(totalAll.toFixed(2), 2));
                $("#hidTotal").val(totalAll);
                
                //zulham 21/01/2016 - IPP Stage 4 Phase 2
                //show total w/o gst Amount
                var sTotalNoGST = parseFloat(itemSum);
                $("#sTotalNoGST").html(addCommas(sTotalNoGST.toFixed(2), 2));
                $("#hidTotalNoGST").val(addCommas(sTotalNoGST.toFixed(2), 2));
               
                //End
            }
            //End   
                     
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
            //Jules 2018.07.09
            function RecalculateGSTafterGLCode(i, qty, price, amt) {
                onClick(i);
                calculateTotal(qty, price, amt);
            }
            
		-->
		</script>
	</head>
	<body runat="server" id="body1">
		<form id="Form1" method="post" runat="server">        
			<table class="alltable" id="table1" cellspacing="0" cellpadding="0" border="0" style="width: 900px">			
				<tr>
					<td class="header"><strong>
                        <asp:Label ID="lbltitle" runat="server" Text="Add E2P Document Line"></asp:Label></strong></td>
				</tr>					
				<tr>
                    <td class="EmptyCol">
	                    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
	                            Text="You can save the E2P as a draft copy by pressing the Save button or Cancel button to cancel the changes."></asp:label>
                    </td>
                </tr>
              </table>        					
            <div id="divInvDetail" runat="server">
			<% response.write(Session("ConstructTable"))%>
			</div>     					
			<table class="alltable" id="table4" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td class = "emptycol" style="height: 24px">
					    <asp:button id="cmdSave" runat="server" CssClass="Button" Text="Save"></asp:button>&nbsp;
					    <asp:button id="cmdCancel" runat="server" CssClass="Button" Text="Cancel"></asp:button>					    
					    <input class="button" id="hidButton" type="button" value="hidButton" name="hidButton" runat="server" style=" display :none" />				    
					    <input class="button" id="hidButton0" type="button" value="hidButton" name="hidButton" runat="server" style=" display :none" />
						<input class="button" id="hidButton1" type="button" value="hidButton1" name="hidButton1" runat="server" style=" display :none" />
    					<asp:HiddenField runat="server" ID = "lineNo"/>
						<%--<input class="button" id="hidButton2" type="button" value="hidButton2" name="hidButton2" runat="server" style=" display :none" />										    										   --%>
					   <%-- Zulham 01/02/2016 - IPP STAGE 4 Phase 2--%>
						<input class="button" id="hidButtonTest" type="button" value="hidButtonTest" name="hidButtonTest" runat="server" style=" display :none" />
						<input class="button" id="hidButtonCancel" type="button" value="hidButtonCancel" name="hidButtonCancel" runat="server" style=" display :none" />
				    
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 27px">&nbsp;</td>
				</tr>
				<tr>
				    <td class="emptycol"><ul class="errormsg" id="vldsum" runat="server">
				    </ul>
				    </td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;</td>
				</tr>			
			</table>  
			<input class="txtbox" id="hid1" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid1" runat="server" />
			<input class="txtbox" id="hid2" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid2" runat="server" />
			<input class="txtbox" id="hid3" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid3" runat="server" />
			<input class="txtbox" id="hid4" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid4" runat="server" />
			<input class="txtbox" id="hidTotal" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidTotal" runat="server" />
			<%--Zulham Aug 28, 2014  --%>
			<input class="txtbox" id="hidDdlGST" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidDdlGST" runat="server" />
			<input class="txtbox" id="hidGSTAmount" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidGSTAmount" runat="server" />
		    <input class="txtbox" id="sGSTInputTotal" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="sGSTInputTotal" runat="server" /> 
		    <input class="txtbox" id="sGSTOutputTotal" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="sGSTOutputTotal" runat="server" /> 
		    <asp:HiddenField ID="hidexceedCutOffDt" runat="server" Value="" />
		    <asp:HiddenField ID="hidIsGST" runat="server" Value="" />
		    <asp:HiddenField ID="hidMode" runat="server" Value="" />
		    <asp:HiddenField ID="hidResidentType" runat="server" Value="" />
		    <%--End--%>
		    <%--Zulham 27/01/2016 - IPP GST Stage 4 Phase 2--%>
		    <asp:HiddenField ID="hidHeaderGstAmount" runat="server" Value="" />
			<input class="txtbox" id="hidInputGST" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidInputGST" runat="server" />
			<input class="txtbox" id="hidOutputGST" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidOutputGST" runat="server" />
			<input class="txtbox" id="hidTotalNoGST" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidTotalNoGST" runat="server" />    
		    <asp:HiddenField ID="hidCoyType" runat="server" Value="" />
            <%--Zulham 30/4/2018 - PAMB--%>
            <asp:HiddenField ID="hidGLCodeTest" runat="server" Value="" />
            <%--End--%>
		</form>
	</body>
</html>
