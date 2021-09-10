<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BillingEntry.aspx.vb" Inherits="eProcure.BillingEntry" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Raise IPP Document</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<% Response.Write(Session("JQuery")) %> 
        <% Response.Write(Session("AutoComplete")) %>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim sCollapseUp As String = dDispatcher.direct("Plugins/Images", "collapse_up.gif")
		    Dim sCollapseDown As String = dDispatcher.direct("Plugins/Images", "collapse_down.gif")
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
		    Dim sCal As String = "<IMG src=" & dDispatcher.direct("Plugins/Images", "i_Calendar2.gif") & " border=""0"">"
            dim edit as string = dDispatcher.direct("IPP", "IPPEntryPop.aspx", "")
        </script>
        <% Response.Write(css)%>
        <% response.write(Session("Block")) %>
        <% Response.write(Session("ventypeahead")) %>
		<% Response.Write(Session("WheelScript"))%>
	<script type="text/javascript" >
<!--
        var indicator = 0; 
		
		$(document).ready(function(){       
        $('#cmdSubmit').click(function() {
        if (indicator == 0)
        {
            document.getElementById("cmdSubmit").style.display= "none";
            document.getElementById("cmdSave").style.display= "none";
            document.getElementById("cmdAddLine").style.display= "none";
            if(document.getElementById("cmdVoid"))
            { document.getElementById("cmdVoid").style.display= "none"; }
        }
        else
        {
            indicator = 0;
        }
        }); 
        $('#cmdSave').click(function() {
            document.getElementById("cmdSubmit").style.display= "none";
            document.getElementById("cmdSave").style.display= "none";
            document.getElementById("cmdAddLine").style.display= "none";
           if(document.getElementById("cmdVoid"))
        { document.getElementById("cmdVoid").style.display= "none"; }                   
        });  
        
         $("#txtPRCSReceivedDate").live("focus",function() {

            var strUserSelectedDate = $("#txtPRCSReceivedDate").val().split("/");
            var strSelectedDate = strUserSelectedDate[2] + "/" + strUserSelectedDate[1] + "/" + strUserSelectedDate[0];           
            var strConvertSelectedDate = new Date(strSelectedDate);
             var thisDate = new Date();
           var twoDigitMonth = ((thisDate.getMonth().length+1) === 1)? (thisDate.getMonth()+1) : '0' + (thisDate.getMonth()+1);
              var strThisDate = thisDate.getDate() + "/" + twoDigitMonth + "/" + thisDate.getFullYear();
              
              if (strConvertSelectedDate > thisDate)  
                  {
                   $("#txtPRCSSentDate").val($("#txtPRCSReceivedDate").val());
                  } 
                   else
                 {
                 strConvertSelectedDate.setDate(thisDate.getDate() - strConvertSelectedDate.getDate());
                 
                   if (strConvertSelectedDate.getDate() > 3)
                {
                alert("PSD Received Date can not back date more than 3 days");
                $("#txtPRCSReceivedDate").val(strThisDate);             
                $("#txtPRCSReceivedDate").blur();
                }
                else
                {
               $("#txtPRCSSentDate").val($("#txtPRCSReceivedDate").val());
                }
                }

		       });   
              });    
                                             
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
            function PopWindow(myLoc)
			{
				window.open(myLoc,"Wheel","width=800,height=600,location=no,toolbar=no,menubar=no,scrollbars=yes,resizable=yes");
				return false;
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
		function onClick() 
            { 
                var bt = document.getElementById("hidButton"); 
                bt.click(); 
            }
            
        function onClick2() 
            { 
                var bt2 = document.getElementById("hidButton1"); 
                bt2.click(); 
            }
			
			function late_submission(latesubmit)
            {
 
            var doc = document.getElementById("txtDocDate");
            var docdate = doc.value
           
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth()+1;
            var year = date.getFullYear();
            var todaydate = (day + "/" + month + "/" + year);
         
            
            var date1 = docdate.split("/");
            var date2 = todaydate.split("/"); 
            var sDate = new Date(date1[2]+"/"+date1[1]+"/"+date1[0]);
            var eDate = new Date(date2[2]+"/"+date2[1]+"/"+date2[0]); 
           
            var datediff = Math.abs(Math.round((eDate-sDate)/86400000));
         
            if (sDate <eDate){
  
            if (datediff >latesubmit){
            
              var late=prompt("Document cannot be save without the reason for late submission.","");
               
                if (late!=null && late!="")
                {
                Form1.hidLateSubmit.value = late;
               
                }
               

              else {
               indicator = 1;
                return false;
                } 
                
            
            }
			}
			}
//			
        function checkduplication2()
            {
                    var doc = document.getElementById("divInvDetail");
            var save = document.getElementById("cmdSave");
            var submit = document.getElementById("cmdSubmit");
            var add = document.getElementById("cmdAddLine");
             var cont = document.getElementById("btnContinue");
             var voidinv = document.getElementById("cmdVoid");
                   
              var check=confirm("There is same Total Amount, Vendor & Document Date, do you want to continue?","");
                             
                if (check == true)
                {                
                document.getElementById("divInvDetail").style.display= "";
                document.getElementById("cmdSave").style.display= "none";
                document.getElementById("cmdSubmit").style.display= "none";
                document.getElementById("cmdAddLine").style.display= "none";
                document.getElementById("cmdVoid").style.display= "none";
                document.getElementById("btnContinue").style.display= "none";
                $("#btnhidden3").click();
                }

            }
            
     function checkduplication3()
            {
            var doc = document.getElementById("divInvDetail");
            var save = document.getElementById("cmdSave");
            var submit = document.getElementById("cmdSubmit");
            var add = document.getElementById("cmdAddLine");
             var cont = document.getElementById("btnContinue");
             var voidinv = document.getElementById("cmdVoid");

          var check=confirm("There is same Total Amount, Vendor & Document Date, do you want to continue?","");
                if (check == true)
                {                
              document.getElementById("divInvDetail").style.display= "";
              document.getElementById("cmdSave").style.display= "none";
             document.getElementById("cmdSubmit").style.display= "none";
            document.getElementById("cmdAddLine").style.display= "none";
           document.getElementById("cmdVoid").style.display= "none";
                document.getElementById("btnContinue").style.display= "none";
                $("#btnhidden4").click();
                $.blockUI({ 
                message: '<h1>Please wait...</h1>',
                css: { 
                border: 'none',                 
                padding: '15px', 
                backgroundColor: '#000', 
                '-webkit-border-radius': '10px', 
                '-moz-border-radius': '10px', 
                opacity: .5, 
                color: '#fff' 
                },
                overlayCSS: { backgroundColor: '#fff',
                opacity: 0.2,
		        cursor:	'wait' 
                }
                 }); 
                
                }
           }
            
function checkduplication()
            {                            
              var check=confirm("There is same Total Amount, Vendor & Document Date, do you want to continue?","");
               
                if (check)
                {                            
                document.getElementById("cmdSubmit").style.display= "";
                document.getElementById("cmdSave").style.display= "";                 
                document.getElementById("btnContinue").style.display= "none";
                var docno = document.getElementById("txtDocNo").value;
		        var doctype = document.getElementById("ddlDocType").value;
		        var vencomp = encodeURI(document.getElementById("txtVendor").value);

                $("#btnhidden5").click();
                }
        
            }
function checkduplicationMultiGL()
            {                            
              var check=confirm("There is same Total Amount, Vendor & Document Date, do you want to continue?","");
               
                if (check)
                {                            
                document.getElementById("cmdSubmit").style.display= "";
                document.getElementById("cmdSave").style.display= "";                 
                document.getElementById("btnContinue").style.display= "none";
                var docno = document.getElementById("txtDocNo").value;
		        var doctype = document.getElementById("ddlDocType").value;
		        var vencomp = encodeURI(document.getElementById("txtVendor").value);

                $("#btnhidden6").click();
                }
        
            }
                        
			function BrowseClick()
			{
				Form1.File1.click();
				Form1.txtAttached.value = Form1.File1.value;
			}
		function popCalendar(val)
		{
			txtVal= document.getElementById(val);
			window.open('<%Response.Write(PopCalendar) %>','cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
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
                var counter2 = 0
                for(i = 0; i <= Amt.length; i++ )
                {
                
                    var check = Amt.charAt(i);
                  
                    if(check == ".")
                    
                    {
                   
                        counter = counter + 1;
                       counter2 = Amt.length - i;
               
                    }
                    
                }
               
                if(counter >= 2)
                {
                    alert("Invalid Amount");
                    return false;
                } 
                
                if(counter2 > 3)
                {
                    alert("Payment amount allow 2 decimal only.");
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
			function ShowDialog(filename,height)
		    {
    			
			    var retval="";
			    retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 1125px");
			    if (retval == "1" || retval =="" || retval==null)
			    {  
			        window.close;
				    return false;

			    } else {
			        window.close;
				    return true;

			    }
		    }
		    
		    function refreshgrid()
		    {
		        document.all("btnhidden3").click();
		    }
		    
		    
		    function edit(line)
		    {
                document.getElementById("hidlinepointer").value = line;
		        document.all("btnhidden9").click();
		    }
		    
		    
		    function remove(line)
		    {
		        var check=confirm("Are you sure that you want to delete this item?","");
		        if (check)
		        
		        {
		            document.getElementById("hidlinepointer").value = line;
		            document.all("btnremoveline").click();
		        }
		    }
		    function removeSubDoc(line)
		    {
		        var check=confirm("Are you sure that you want to delete this item?","");
		        if (check)
		        
		        {
		            document.getElementById("hidlinepointer").value = line;
		            document.all("btnremoveSubDocLine").click();
		        }
		    }
		    function editSubDoc(line)
		    {
                document.getElementById("hidlinepointer").value = line;
		        document.all("btnEditSubDoc").click();
		    }
		     function removeSubDoc_All()
		    {
		        var check=confirm("Are you sure that you want to delete the sub documents?","");
		        if (check)
		        
		        {
		            document.all("btnremoveSubDoc").click();
		        }
		    }
		-->
		</script>
	</head>
	<body runat="server" id="body1">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_AddPO_tabs"))%>
			<table class="alltable" id="table1" cellspacing="0" cellpadding="0" border="0" style="width: 100%">
				<tbody>
				
				<tr>
					<td class="header" colspan="4" ><strong>Create Billing Document</strong></td>
				</tr>
					
					<tr>
	                    <td class="EmptyCol" colspan="6" style="width: 841px">
		                    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                            Text="When raising a new Billing document, click Continue button to continue entering the line items. To add more line items, click on the Add Line button. Once it is ready, click on the Submit button to submit to the approving officers."></asp:label>

	                    </td>
                    </tr>
         <tr><td class="header" style="height: 7px" colspan="4"></td></tr>

					<tr>
						<td class="tableHeader" colspan="5" style="width: 750px">&nbsp;Document Header</td>
					</tr>
					<tr>
						<td class = "tablecol" valign="middle" align="left" colspan="5">
							<table class="alltable" id="table2" cellspacing="0" cellpadding="0" border="0"  > 
							<tr valign="top">
								<td class="tablecol" style="height: 19px; width: 220px;">&nbsp;<strong>Document Type :</strong>&nbsp;</td>
									<td class="tablecol" style="width: 226px"  ><asp:dropdownlist id="ddlDocType" Width="70%" runat="server" CssClass="ddl"  AutoPostBack="True"></asp:dropdownlist></td>
									<td class="tablecol" style="width: 1px"  ></td>
								<td class="tablecol" style="width: 134px" >
								<strong>Currency</strong>&nbsp;:</td>
									<td class="tablecol" colspan = "3">
                                        <asp:DropDownList ID="ddlCurrency" runat="server" Width="39%" CssClass = "ddl" autopostback="true"  >
                                        </asp:DropDownList>
                                    </td>                                   
								</tr>
								<tr valign="top">
									<td class="tablecol" style="width: 220px; height: 24px;" >&nbsp;<strong>Document No</strong><asp:label id="Label9" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tablecol" style="width: 226px; height: 24px;"  >
                                        <asp:TextBox ID="txtDocNo" runat="server" Width="75%" cssclass="txtbox" Enabled="False"></asp:TextBox></td>
					                <td class="tablecol" style="width: 1px; height: 24px;" ></td>
					                <td class="tablecol" style="width: 134px; height: 24px;" >
                                        <strong>Total Amount Excluding Tax </strong>
                                        <asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tablecol"  colspan = "3" style="height: 24px"><asp:textbox id="txtNoTaxTotal" runat="server" CssClass="txtbox" Width="40%" MaxLength="50"></asp:textbox></td>					                									
								</tr>
								<tr valign="top">
									<td class="tablecol" style="height: 19px; width: 220px;" ></td>
									<td class="tablecol" style="width: 226px; height: 19px;" >
                                         <%--<a onclick="popCalendar('txtDocDate');" href="javascript:;"><% Response.Write(sCal)%></a> --%>
                                    </td>
					                <td class="tablecol" style="width: 1px; height: 19px;" ></td>
					                <td class="tablecol" style="width: 134px; height: 19px;">
                                        <strong>Payment Amount
                                        <asp:Label ID="Label2" runat="server" CssClass="errormsg" Width="1px">*</asp:Label></strong>:</td>
									<td class="tablecol"   colspan = "3" style="height: 19px">
                                        <asp:TextBox ID="txtPaymentAmt" runat="server" CssClass="txtbox" MaxLength="50" Width="40%"></asp:TextBox></td>					                
									</tr>
								<tr valign="top">
								<td class="tablecol" align="left" style="height: 24px; width: 220px;">
                                    </td>
									<td class="tablecol" style="height: 24px; width: 226px;">
                                        </td>
					                <td class="tablecol" style="height: 24px; width: 1px;"></td>
					                <td class="tablecol" style="width: 180px; height: 24px;" >
                                        <strong> </strong></td>
					                <td class="tablecol"  colspan = "3" style="height: 24px">					                	
                                        <%--<span id="divPRCSSentDate" runat="server"><a onclick="popCalendar('txtPRCSSentDate');" href="javascript:;"><% Response.Write(sCal)%></a> </span>                                --%>
                                        </td>
								</tr>
								<tr valign="top">
								    <td class="TableCol" style="height:24px"></td>
								      <td class="TableCol" colspan="8">
                                        <asp:RadioButtonList ID="rbtnCoyType" runat="server" RepeatDirection="Horizontal" CssClass="rbtn" Width="50%" AutoPostBack="True">
                                            <asp:ListItem Value="C" Selected="True">Customer</asp:ListItem>                                           
                                            <asp:ListItem Value="R">Related Company</asp:ListItem>
                                            <asp:ListItem Value="E">Employee</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
								</tr>
								<tr valign="top">
									<td class="tablecol" style="height: 19px; width: 200px;">&nbsp;<strong>Vendor</strong><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tablecol" style="width: 240px"  ><asp:textbox id="txtVendor" Width="100%" runat="server" CssClass="txtbox" ></asp:textbox>
									<%--<asp:requiredfieldvalidator id="revVendor" runat="server" Enabled="true" ControlToValidate="txtVendor"
											ErrorMessage="Vendor is required " Display="None"></asp:requiredfieldvalidator>--%>
									    </td>
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>
					                <td class="tablecol" style="height: 19px; width: 190px;" >
					                <strong>&nbsp;</strong>
					                </td>
								<td class="tablecol"   colspan = "3">
                                    &nbsp;</td>	
                                       
                                     
								</tr>								
								<tr valign="top">

								    <td class="tablecol" style="height: 19px; width: 200px;">&nbsp;<strong>Vendor Address</strong>&nbsp;:</td>
								    
								    <td class="tablecol" style="width: 226px"  >
                                        <asp:TextBox ID="txtVenAddL1" runat="server" CssClass="txtbox" Width="100%" Enabled = "false" ></asp:TextBox></td>
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>
					                <td class="tablecol" style="height: 19px; width: 134px;" ></td>
									<td class="tablecol" style="width: 35px" >
                                        </td> 
                                        <td class="tablecol" style="width: 25px"><strong></strong></td>
                                    <td class="tablecol" >
                                         <%-- <asp:RadioButtonList ID="rdbWHT" runat="server" width = "100%" Font-Size = "10pt" Height="100%">
                                              <asp:ListItem>WHT applicable and payable by Company</asp:ListItem>
                                              <asp:ListItem>WHT applicable and payable by Vendor</asp:ListItem>
                                              <asp:ListItem>No WHT</asp:ListItem>
                                          </asp:RadioButtonList></td>--%>
                                        </td>
								</tr>
								<tr valign="top">

									<td class="tablecol" style="height: 19px; width: 200px;" ></td>
									<td class="tablecol" style="width: 226px" >
                                    <asp:TextBox ID="txtVenAddL2" runat="server"  CssClass="txtbox" Width="100%" Enabled = "false"></asp:TextBox></td>
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>
					                <td class="tablecol" style="height: 19px;; width: 134px;"></td>
					               <td class="tablecol" style="height: 19px; width: 35px;"></td>
					               <td class="tablecol" style="height: 19px;"></td>
					               	 <td class="tablecol" style="height: 19px;">
                                       </td>
								</tr>			                
								<tr valign="top">

									<td class="tablecol" style="height: 19px; width: 200px;">&nbsp;</td>

									<td class="tablecol" style="width: 226px" >
                                        <asp:TextBox ID="txtVenAddL3" runat="server"  CssClass="txtbox" Width="100%" Enabled = "false"></asp:TextBox></td> 
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>
					                <td class="tablecol" style="height: 19px; width: 134px;"></td>
					               <td class="tablecol" style="height: 19px; width: 35px;"></td>
					               <td class="tablecol" style="height:19px;"></td>
					                <td class="tablecol" style="height: 19px;">
                                        &nbsp;</td>	
					           
                                       
								</tr>
	                            <tr valign="top">
									<td class="tablecol" style="height: 19px; width: 200px;"></td>
									<td class="tablecol" style="width: 226px"  >
                                        <asp:TextBox ID="txtVenAddPostcode" runat="server" CssClass="txtbox" Width="100%" Enabled = "false"></asp:TextBox></td>
					                <td class="tablecol" style="width: 1px" ></td>
					               <td class="tablecol" style="width: 134px;"></td>
					               <td class="tablecol" style="width: 35px;"></td>
					               <td class="tablecol" ></td>
					                    <td class="tablecol" style="width: 256px" >
                                      </td>					                   
                                  <%--  <td class="tablecol" rowspan ="3">&nbsp;
                                      <asp:TextBox ID="txtNoWHtreason" runat="server" cssclass="txtbox2" Rows = "4" MaxLength="1000" Width="98%" TextMode="MultiLine"  Enabled = "false" ></asp:TextBox></td>--%>
					               
									
								</tr>
								<tr valign="top">
									<td class="tablecol" style="height: 19px; width: 200px;"></td>
									<td class="tablecol" style="width: 226px" >
                                     <asp:TextBox ID="txtVenAddCity" runat="server"  CssClass="txtbox" Width="100%" Enabled = "false"></asp:TextBox></td>
					                <td class="tablecol" style="width: 1px" ></td>
					                <td class="tablecol" style="width: 134px" ></td>
					               <td class="tablecol" style="width: 35px"></td>
					               <td class="tablecol"></td>
					                 <td class="tablecol" rowspan ="3">&nbsp;
                                      </td>
									
								</tr>
								<tr valign="top">
									<td class="tablecol" style="height: 19px; width: 200px;" ></td>
									<td class="tablecol" style="height: 19px; width: 226px;" >
                                        <asp:TextBox ID="txtVenAddState" runat="server"  CssClass="txtbox" Width="100%" Enabled = "false"></asp:TextBox></td>
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>
					                     <td class="tablecol" style="height: 19px; width: 134px;"></td>
					               <td class="tablecol" style="height: 19px; width: 35px;"></td>
					               <td class="tablecol" style="height: 19px;"></td>
					           
								
								</tr>
								
								<tr valign="top">
									<td class="tablecol" style="height: 19px; width: 200px;" ></td>
									<td class="tablecol" style="height: 19px; width: 226px;" >
                                        <asp:TextBox ID="txtVenAddCountry" runat="server" CssClass="txtbox" Width="100%" Enabled = "false"></asp:TextBox></td>
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>
					                     <td class="tablecol" style="height: 19px; width: 134px;"></td>
					               <td class="tablecol" style="height: 19px; width: 35px;"></td>
					               <td class="tablecol" style="height: 19px;"></td>
					         
									
								</tr>
								<tr valign="top" >
									<td class="tablecol" style="width: 200px" >&nbsp;<strong>Internal Remarks</strong>&nbsp;:</td>
									<td class="tablecol" colspan = "6"><asp:textbox id="txtIntRemark" runat="server" CssClass="txtbox"  MaxLength="1000"
											TextMode="MultiLine" Height="57px" Width = "100%"></asp:textbox></td>
					            </tr>
					            <tr valign="top" >
									<td class="tablecol"  ></td>
									<td class="tablecol" colspan = "6"></td>
					            </tr>		
								<tr valign="top" id="trLateReason" runat="server">
									<td class="tablecol" style="width: 200px" >
                                    </td>
									<td class="tablecol" colspan = "6"></td>
	
								</tr>									
							</table>
							
							</td>
			
					</tr>
					<tr class="emptycol">
					<td class = "emptycol" colspan="2" ><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates required field</td>
				           </tr>
				</tbody>
			</table>
			<table>
			<tr>
			
					<td class = "emptycol" style="height: 24px">
					   <asp:button id="btnContinue" runat="server" CssClass="Button" Text="Continue"></asp:button>&nbsp;
					  <input class="button" id="hidBtnContinue" type="button" value="hidBtnContinue" name="hidBtnContinue" runat="server" style=" display :none" />
					  <input class="button" id="hidBtnContinue2" type="button" value="hidBtnContinue2" name="hidBtnContinue2" runat="server" style=" display :none" />					  
					  <asp:button id="btnhidden3" runat="server" CssClass="Button"  Text="btnhidden3" style=" display :none"></asp:button>
					  <input class="button" id="hidBtnContinueSubmit" type="button" value="hidBtnContinueSubmit" name="hidBtnContinueSubmit" runat="server" style=" display :none; width: 187px;" />					  
					  <asp:button id="btnhidden4" runat="server" CssClass="Button"  Text="btnhidden4" style=" display :none"></asp:button>
					  <asp:button id="btnhidden5" runat="server" CssClass="Button"  Text="btnhidden5" style=" display :none"></asp:button>
					  <asp:button id="btnhidden9" runat="server" CssClass="Button"  Text="btnhidden9" style=" display :none"></asp:button>					 					    
					  <asp:button id="btnEditSubDoc" runat="server" CssClass="Button"  Text="btnhidden9" style=" display :none"></asp:button>					 					    				  
					  <input class="button" id="hidBtnContinueMultiGL" type="button" value="hidBtnContinueMultiGL" name="hidBtnContinueMultiGL" runat="server" style=" display :none" />
					  <asp:button id="btnhidden6" runat="server" CssClass="Button"  Text="btnhidden6" style=" display :none"></asp:button>
					  <asp:HiddenField ID="hidIsGST" runat="server" Value="" />
				      <asp:HiddenField ID="hidexceedCutOffDt" runat="server" Value="" />	
					</td>
	</tr>
				
			</table>
			<table id="TABLE11" class="tableheader" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
				<tr id="hidApprFlow" style="width:100%; cursor:pointer;" class="tableHeader" onclick="showHide1('ApprFlow')">
						<td valign="top" class="tableHeader" >	
					    Approval Flow<%--<asp:Image ID="Image"  ImageUrl="#" />--%>
					    </td>
					    </tr>
				</table>
				<div id="ApprFlow" style="display:inline"  >
		    <%--<div id="divApprFlow" runat="server" >--%>
			<table  cellpadding="0" cellspacing="0" class="AllTable" width="100%">
			  <tr>
				<td class="EmptyCol"></td>
		    </tr>

                 <tr>	                    			   
			         <td class="EmptyCol">
				              <asp:datagrid id="dtgApprvFlow"  runat="server" AutoGenerateColumns="False" Width="100%">
							        <Columns>								
								        <asp:BoundColumn DataField="Action" HeaderText="Performed By">
									        <HeaderStyle Width="10%" CssClass="GridHeader"></HeaderStyle>
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>	
								        <asp:BoundColumn DataField="FA_ACTIVE_AO" HeaderText="User ID">
								            <HeaderStyle Width="10%" CssClass="GridHeader"></HeaderStyle>
								            <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="FA_ACTION_DATE" HeaderText="Date Time">
								           <HeaderStyle Width="10%" CssClass="GridHeader"></HeaderStyle>
								            <ItemStyle HorizontalAlign="Left"></ItemStyle>   
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="FA_AO_REMARK" HeaderText="Remarks">
								           <HeaderStyle Width="60%" CssClass="GridHeader"></HeaderStyle>
								            <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
								     </Columns>
						        </asp:datagrid>

				        </td>
				   </tr>
            </table>		   
            </div>
            <div id="divSubDoc" runat="server" visible="false">
                <h1 class="tableHeader">
                    &nbsp;</h1>
				</div>
				<div>
                    <asp:Label style="width:1024px" runat="server" ID="lblTitle"></asp:Label>
                </div>
            <div id="divInvDetail" runat="server">
			<asp:datagrid id="dtgInvDetail"  runat="server" CssClass="Grid" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False" Width="100%">
							        <Columns>
							            <asp:TemplateColumn HeaderText="Action">						                
							            <ItemStyle Width="40px" />
							                <ItemTemplate>
                                                <span runat="server" style="cursor:pointer;" id="cmdremove"></span>
                                                <span runat="server" style="cursor:pointer;" id="cmdedit"></span>                                                                                  
							                </ItemTemplate>
						                </asp:TemplateColumn>
						                <asp:TemplateColumn HeaderText="S/NO">												                
							                <ItemTemplate>
                                                <asp:Label ID="LineNo" runat="server" Text=""></asp:Label>
							                </ItemTemplate>
						                </asp:TemplateColumn>								
							            <asp:BoundColumn DataField="BM_INVOICE_LINE" SortExpression="BM_INVOICE_LINE" HeaderText="S/NO">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							            </asp:BoundColumn>
<%--								        <asp:BoundColumn DataField="BM_PAY_FOR" SortExpression="BM_PAY_FOR" HeaderText="Pay For">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>--%>
<%--								        <asp:BoundColumn DataField="_gst_reimb" HeaderText="Disb./Reimb.">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>--%>
								        <asp:BoundColumn DataField="BM_REF_NO" SortExpression="BM_REF_NO" HeaderText="Billing No.">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="BM_PRODUCT_DESC" SortExpression="BM_PRODUCT_DESC" HeaderText="Transaction Description">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>	
								        <asp:BoundColumn DataField="BM_UOM" SortExpression="BM_UOM" HeaderText="UOM">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="BM_RECEIVED_QTY" SortExpression="BM_RECEIVED_QTY" HeaderText="QTY">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>  
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="BM_UNIT_COST" SortExpression="BM_UNIT_COST" HeaderText="Unit Price">
								            <HeaderStyle HorizontalAlign="Right" />
									        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="BM_AMOUNT" HeaderText="Amount">
								            <HeaderStyle HorizontalAlign="Right" />
									        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="BM_gst_value" HeaderText="GST Amount">
								            <HeaderStyle HorizontalAlign="Right" />
									        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="BM_gst_input_tax_code" SortExpression="BM_gst_input_tax_code" HeaderText="Input Tax Code">
								            <HeaderStyle HorizontalAlign="Right" />
									        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="BM_gst_output_tax_code" SortExpression="BM_gst_output_tax_code" HeaderText="Output Tax Code">
								            <HeaderStyle HorizontalAlign="Right" />
									        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        </asp:BoundColumn>

                                        <%--Jules 2018.04.27 - PAMB Scrum 2 - Added Category--%>
                                        <asp:BoundColumn DataField="BM_CATEGORY" SortExpression="BM_CATEGORY" HeaderText="Category">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <%--End modification--%>

								        <asp:BoundColumn DataField="BM_B_GL_CODE" SortExpression="BM_B_GL_CODE" HeaderText="GL Code">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>

                                        <%--Jules 2018.04.27 - PAMB Scrum 2 - Removed Sub Description & HO/BR--%>
								        <%--<asp:BoundColumn DataField="BM_GLRULE_CATEGORY" SortExpression="BM_GLRULE_CATEGORY" HeaderText="Sub Description">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>							     
								        <asp:BoundColumn DataField="BM_BRANCH_CODE_2" SortExpression="BM_BRANCH_CODE" HeaderText="HO/BR">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>--%>                                        

                                        <%--Jules 2018.04.27 - PAMB Scrum 2 - Added Analysis Codes--%>
                                        <asp:BoundColumn DataField="BM_ANALYSIS_CODE1" SortExpression="BM_ANALYSIS_CODE1" HeaderText="Fund Type">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="BM_ANALYSIS_CODE2" SortExpression="BM_ANALYSIS_CODE2" HeaderText="Product Type">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="BM_ANALYSIS_CODE3" SortExpression="BM_ANALYSIS_CODE3" HeaderText="Channel">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="BM_ANALYSIS_CODE4" SortExpression="BM_ANALYSIS_CODE4" HeaderText="Reinsurance Company">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="BM_ANALYSIS_CODE5" SortExpression="BM_ANALYSIS_CODE5" HeaderText="Asset Fund">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="BM_ANALYSIS_CODE8" SortExpression="BM_ANALYSIS_CODE8" HeaderText="Project Code">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="BM_ANALYSIS_CODE9" SortExpression="BM_ANALYSIS_CODE9" HeaderText="Person Code">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <%--End modification--%>

								        <asp:BoundColumn DataField="BM_COST_CENTER_2" SortExpression="BM_COST_CENTER" HeaderText="Cost Center">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
<%--								           <asp:BoundColumn Visible="false" DataField="ID_COST_ALLOC_CODE" SortExpression="ID_COST_ALLOC_CODE" HeaderText="Cost Allocation">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>--%>
						            </Columns>
				</asp:datagrid>
			</div>     
				<table>
				<tr>
				<td class="emptycol">
				    <input class="txtbox" id="hid1" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid1" runat="server" />
				    <input class="txtbox" id="hid2" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid2" runat="server" />
				    <input class="txtbox" id="hid3" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid3" runat="server" />
				    <input class="txtbox" id="hid4" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid4" runat="server" />				
				    <input class="txtbox" id="hid5" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid5" runat="server" />	
				    <input class="txtbox" id="hid6" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hid5" runat="server" />			
				    <input id="hidlinepointer" type="hidden" size="2" name="hidlinepointer" runat="server" />				
				    <input class="txtbox" id="hidLateSubmit" style="WIDTH: 45px; HEIGHT: 18px" type="hidden"  name="hidLateSubmit" runat="server" />
				    <input class="txtbox" id="hidVendorIndex" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidVendorIndex" runat="server" />			
				    <input class="txtbox" id="hidVendorIndexOld" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidVendorIndexOld" runat="server" />			
				    <input class="txtbox" id="hidResidenceType" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidResidenceType" runat="server" />		
				    <input class="txtbox" id="hidVendorId" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidVendorId" runat="server" />			    
				    <input class="txtbox" id="hidPaymentType" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidPaymentType" runat="server" />			    
				    <input class="txtbox" id="hidGSTCode" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidGSTCode" runat="server" />			    
				</td>
			   </tr>
</table>
			<table class="alltable" id="table4" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td class = "emptycol" style="height: 24px">					    
					    <asp:button id="cmdSave" runat="server" CssClass="Button" Text="Save" ></asp:button>&nbsp;
					    <asp:button id="cmdSubmit" runat="server" CssClass="Button" Text="Submit"></asp:button>&nbsp;
					    <asp:button id="cmdAddLine" runat="server" CssClass="Button" Text="Add Line"></asp:button>&nbsp;
					   <%-- <asp:button id="cmdSetup" runat="server" CssClass="Button" Width="94px" style="display:none" Text="Approval Setup"></asp:button>&nbsp;--%>					    
					    <asp:button id="cmdVoid" runat="server" CssClass="Button" CausesValidation="False" Text="Void"></asp:button>&nbsp;&nbsp; &nbsp;&nbsp;				    
					    <input runat="server" onfocus="onClick()" style=" border-style:none;background-color:Transparent;width:92px;margin-right:0px;" class="txtbox2" type="text" id="txtTemp" name="txtTemp" />
					    <input class="button" id="hidButton" type="button" value="hidButton" name="hidButton" runat="server" style=" display :none" />
						<input class="button" id="hidButton1" type="button" value="hidButton1" name="hidButton1" runat="server" style=" display :none" />						
						<asp:button id="btnremoveline" runat="server" style="display :none" Text=""></asp:button>
						<asp:button id="btneditline" runat="server" style="display :none" Text=""></asp:button>	
						<asp:button id="btnremoveSubDocLine" runat="server" style="display :none" Text=""></asp:button>	
						<asp:button id="btnremoveSubDoc" runat="server" style="display :none" Text=""></asp:button>		
						<asp:button id="hidbtnremoveSubDoc" runat="server" style="display :none" Text=""></asp:button>							   
					</td>
				</tr>
				<tr>
					<td class="emptycol" style="height: 27px">&nbsp;</td>
				</tr>
				<tr>
				<td class="emptycol"><ul class="errormsg" id="vldsum" runat="server">
				</ul></td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;</td>
				</tr>
			<tr>
					<td class="emptycol"><asp:Button ID="btnGetAdd" runat="server" Text="Search" style="display:none;"/></td>
				</tr>				
				<tr>
				    <td class="emptycol">
				        <p><asp:hyperlink id="lnkBack" Runat="server">
								<strong>&lt; Back</strong></asp:hyperlink></p>
				    </td>
				</tr>
			</table>  
			     
		</form>
	</body>
</html>
