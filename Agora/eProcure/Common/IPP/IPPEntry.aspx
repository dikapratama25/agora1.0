<%@ Page Language="vb" AutoEventWireup="false" Codebehind="IPPEntry.aspx.vb" Inherits="eProcure.IPPEntry" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Raise E2P Document</title>
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
                 //Zulham PAMB 09/04/2018
                //if (charCode > 31 && (charCode < 48 || charCode > 57) && (charCode < 65 || charCode > 90) && (charCode < 97 || charCode > 122))
                if (charCode > 31 && (charCode < 33 || charCode > 33) && (charCode < 35 || charCode > 37) && (charCode < 40 || charCode > 42) && (charCode < 44 || charCode > 47) && (charCode < 48 || charCode > 57) && (charCode < 64 || charCode > 91) && (charCode < 92 || charCode > 96) && (charCode < 97 || charCode > 126))
                ///
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
			
		//Zulham 18/02/2016 - IPP Stage 4 Phase 2
//		function CalculateAll() 
//            { 
//               alert('K');
//            }
			
			
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
		    
		    //Zulham PAMB - 11/04/2018
		    function removeFile(index)
		    {
		        var check=confirm("Are you sure that you want to remove this item?","");
		        if (check)
		        {
		            document.getElementById("hidFileIndex").value = index;
		        }
		    }
		    //End
		    
		-->
		</script>
	</head>
	<body runat="server" id="body1">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_AddPO_tabs"))%>
			<table class="alltable" id="table1" cellspacing="0" cellpadding="0" border="0" style="width: 100%">
				<tbody>
				
				<tr>
					<td class="header" colspan="4" ><strong>Create E2P Document</strong></td>
				</tr>
					
					<tr>
	                    <td class="EmptyCol" colspan="6" style="width: 841px">
		                    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                            Text="When raising a new E2P document, click Continue button to continue entering the line items. To add more line items, click on the Add Line button. Once it is ready, click on the Submit button to submit to the approving officers."></asp:label>

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
								    <td class="TableCol" style="height: 19px; width: 220px;">&nbsp;<strong>Master Document :</strong>&nbsp;</td>
								      <td class="TableCol"  colspan="8">
                                        <asp:RadioButtonList ID="rbtnMasterDoc" runat="server" Enabled="false" RepeatDirection="Horizontal" CssClass="rbtn" Width="200px" AutoPostBack="True">
                                            <asp:ListItem Value="1">Yes</asp:ListItem>                                           
                                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
								</tr>
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
									<td class="tablecol" style="width: 220px" >&nbsp;<strong>Document No</strong><asp:label id="Label9" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tablecol" style="width: 226px"  >
                                        <asp:TextBox ID="txtDocNo" runat="server" Width="75%" cssclass="txtbox"></asp:TextBox></td>
					                <td class="tablecol" style="width: 1px" ></td>
					                <%--Zulham 22/01/2016 - IPP GST Stage 4 Phase 2
					                Added total excluding GST--%>
                                    <%--'Zulham 10102018 - PAMB SST--%>
					                <td class="tablecol" style="width: 140px" >
					                <strong>Total Amount(excl.SST)</strong><asp:label id="Label11" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tablecol"  colspan = "3"><asp:textbox id="txtTotalAmountNoGST" runat="server" CssClass="txtbox" Width="40%" MaxLength="50"></asp:textbox>
                                    </td>	
					                <%--<td class="tablecol" style="width: 134px" >
					                <strong>Payment Amount</strong><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tablecol"  colspan = "3"><asp:textbox id="txtPaymentAmt" runat="server" CssClass="txtbox" Width="40%" MaxLength="50"></asp:textbox>
                                    </td>--%>					                									
								</tr>
								<tr valign="top">
									<td class="tablecol" style="height: 19px; width: 220px;" >&nbsp;<strong>Document Date</strong><asp:label id="Label2" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tablecol" style="width: 226px" >
                                        <input id="txtDocDate" runat="server" style="width: 130px" class="txtbox" readonly="readonly"  />
                                         <a onclick="popCalendar('txtDocDate');" href="javascript:;"><% Response.Write(sCal)%></a> 
                                        </td>
                                    <%--Zulham 22/01/2016 - IPP GST Stage 4 Phase 2
					                Added total excluding GST--%>
                                    <%--Zulham 09102018 - PAMB SST--%>
                                    <td class="tablecol" style="width: 1px" ></td>
					                <td class="tablecol" style="width: 134px"><strong>SST Amount</strong>&nbsp;:</td>
									<td class="tablecol" colspan = "3">
                                       <asp:textbox id="txtGSTAmount" runat="server" CssClass="txtbox" Width="40%" MaxLength="50"/>
                                    </td>	    
					                <%--<td class="tablecol" style="width: 1px" ></td>
					                <td class="tablecol" style="width: 134px"><strong>Payment Mode</strong>&nbsp;:</td>
									<td class="tablecol"   colspan = "3">
                                        <asp:Label ID="lblPaymentMethod" runat="server" CssClass="lbl" Width="136px"></asp:Label>
                                    </td>--%>					                
									</tr>
								<tr valign="top">
								<td class="tablecol" align="left" style="height: 24px; width: 220px;">&nbsp;<strong>Manual PO Number</strong>&nbsp;:</td>
									<td class="tablecol" style="height: 24px; width: 226px;">
                                        <asp:TextBox ID="txtPONo" runat="server" Width="75%" cssclass="txtbox"></asp:TextBox></td>
					                <td class="tablecol" style="height: 24px; width: 1px;"></td>
					                <%--Zulham 22/01/2016 - IPP GST Stage 4 Phase 2--%>
					                <td class="tablecol" style="width: 134px" >
					                <strong>Payment Amount</strong><asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tablecol"  colspan = "3"><asp:textbox id="txtPaymentAmt" runat="server" CssClass="txtbox" Width="40%" MaxLength="50" ReadOnly="True" Enabled="False"></asp:textbox>
                                    </td>
					                <%--<td class="tablecol" style="width: 180px; height: 24px;" ><strong>PSD Sent Date</strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					                <td class="tablecol"  colspan = "3" style="height: 24px">					                	
                                        <input id="txtPRCSSentDate" runat="server" style="width: 130px" class="txtbox" readonly="readonly"  />
                                    </td>--%>
								</tr>	
									<tr valign="top">
								<td class="tablecol" align="left" style="height: 19px; width: 220px;">&nbsp;<strong>Document Due Date</strong>&nbsp;:</td>
									<td class="tablecol" style="height: 19px; width: 226px;">
                                        <asp:Label ID="lblDocDueDate" runat="server" Width="75%" cssclass="lbl"></asp:Label></td>
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>
					                <%--Zulham 22/01/2016 - IPP GST Stage 4 Phase 2--%>
					                <td class="tablecol" style="width: 134px"><strong>Payment Mode</strong>&nbsp;:</td>
									<td class="tablecol"   colspan = "3">
                                        <%--Zulham 21112018--%>
                                        <asp:Label ID="lblPaymentMethod" runat="server" CssClass="lbl" Width="136px" Visible="false"></asp:Label>
                                        <asp:Label ID="lblPaymentMethodFull" runat="server" CssClass="lbl"></asp:Label>
                                    </td>
					                <%--<td class="tablecol" style="width: 180px" ><strong>PSD Received Date</strong><asp:label id="Label10" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					                <td class="tablecol"  colspan = "3">				                	
                                        <input id="txtPRCSReceivedDate" runat="server" style="width: 130px" class="txtbox" readonly="readonly"  />
                                        <span id="divPRCSReceivedDate" runat="server"><a onclick="popCalendar('txtPRCSReceivedDate');" href="javascript:;"><% Response.Write(sCal)%></a></span>                                    
                                        </td>--%>
								</tr>	
								<tr valign="top">
								<td class="tablecol" align="left" style="height: 19px; width: 220px;">&nbsp;<strong>Credit Term</strong>&nbsp;:</td>
									<td class="tablecol" style="height: 19px; width: 226px;">
                                        <asp:Label ID="lblCreditTerm" runat="server" Width="75%" cssclass="lbl"></asp:Label></td>
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>
					                <%--Zulham 26/01/2015 - IPP GST Stage 4 Phase 2--%>
					                <%--<td class="tablecol" style="width: 180px" >
                                        </td>
					                <td class="tablecol"  colspan = "3"></td>--%>
					                <td class="tablecol" style="width: 180px; height: 24px;" ><strong>PSD Sent Date</strong><asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					                <td class="tablecol"  colspan = "3" style="height: 24px">					                	
                                        <input id="txtPRCSSentDate" runat="server" style="width: 130px" class="txtbox" readonly="readonly" disabled="disabled" />
                                    </td>
								</tr>	
								<tr valign="top">
								    <td class="TableCol" ></td>
								      <td class="TableCol"  colspan="2">
                                        <asp:RadioButtonList ID="rbtnCoyType" runat="server" RepeatDirection="Horizontal" CssClass="rbtn" Width="200px" AutoPostBack="True">
                                            <asp:ListItem Value="V" Selected="True">Vendor</asp:ListItem>                                           
                                        <asp:ListItem Value="E">Employee</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <%--'Zulham 26/01/2016 - IPP GST Stage 4 Phase 2--%>
                                    <td class="tablecol" style="width: 180px" ><strong>PSD Received Date</strong><asp:label id="Label10" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
					                <td class="tablecol"  colspan = "3">				                	
                                        <input id="txtPRCSReceivedDate" runat="server" style="width: 130px" class="txtbox" readonly="readonly" disabled="disabled"  />
                                        <span id="divPRCSReceivedDate" runat="server"><a onclick="popCalendar('txtPRCSReceivedDate');" href="javascript:;"><% Response.Write(sCal)%></a></span>                                    
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
					                <strong>&nbsp;<asp:Label ID="Label8" runat="server" CssClass="lbl" Width="180px" Text="Bank Code[Bank A/C No.] :"></asp:Label></strong>
					                </td>
								<td class="tablecol"   colspan = "3">
                                        <asp:Label ID="lblBankNameAccountNo" runat="server" CssClass="lbl" Width="276px"></asp:Label>
                                        </td>	
                                       
                                     
								</tr>								
								<tr valign="top">

								    <td class="tablecol" style="height: 19px; width: 200px;">&nbsp;<strong>Vendor Address</strong>&nbsp;:</td>
								    
								    <td class="tablecol" style="width: 226px"  >
                                        <asp:TextBox ID="txtVenAddL1" runat="server" CssClass="txtbox" Width="100%" Enabled = "false" ></asp:TextBox></td>
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>

                                    <%--Jules 2018.04.17 - PAMB Scrum 1--%>
					                <%--<td class="tablecol" style="height: 19px; width: 134px;" >&nbsp;<strong>Withholding Tax</strong>&nbsp;:</td>
									<td class="tablecol" style="width: 35px" >
                                        <asp:TextBox ID="txtWHT" runat="server"  CssClass = "numerictxtbox" Width="34px" Enabled = "false" ></asp:TextBox></td> 
                                        <td class="tablecol" style="width: 25px"><strong>(%)</strong></td>
                                    <td class="tablecol" >
                                         <%-- <asp:RadioButtonList ID="rdbWHT" runat="server" width = "100%" Font-Size = "10pt" Height="100%">
                                              <asp:ListItem>WHT applicable and payable by Company</asp:ListItem>
                                              <asp:ListItem>WHT applicable and payable by Vendor</asp:ListItem>
                                              <asp:ListItem>No WHT</asp:ListItem>
                                          </asp:RadioButtonList></td>--%>
                                        <%--<asp:RadioButton ID="rdbWHTComp" runat="server" cssclass ="rbtn td" enabled ="false" autopostback="true"  Text="WHT applicable and payable by Company" /></td>--%>

                                    <td class="tablecol" style="height: 19px; width: 134px;" >&nbsp;</td>
									<td class="tablecol" style="width: 35px" >
                                        <asp:TextBox ID="txtWHT" runat="server"  CssClass = "numerictxtbox" Width="34px" Enabled = "false" Visible="false" ></asp:TextBox></td> 
                                        <td class="tablecol" style="width: 25px"></td>
                                    <td class="tablecol" >
                                         <%-- <asp:RadioButtonList ID="rdbWHT" runat="server" width = "100%" Font-Size = "10pt" Height="100%">
                                              <asp:ListItem>WHT applicable and payable by Company</asp:ListItem>
                                              <asp:ListItem>WHT applicable and payable by Vendor</asp:ListItem>
                                              <asp:ListItem>No WHT</asp:ListItem>
                                          </asp:RadioButtonList></td>--%>
                                        <asp:RadioButton ID="rdbWHTComp" Visible="false" runat="server" cssclass ="rbtn td" enabled ="false" autopostback="true"  Text="WHT applicable and payable by Company" /></td>
                                    <%--End modification--%>
								</tr>
								<tr valign="top">

									<td class="tablecol" style="height: 19px; width: 200px;" ></td>
									<td class="tablecol" style="width: 226px" >
                                    <asp:TextBox ID="txtVenAddL2" runat="server"  CssClass="txtbox" Width="100%" Enabled = "false"></asp:TextBox></td>
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>
					                <td class="tablecol" style="height: 19px;; width: 134px;"></td>
					               <td class="tablecol" style="height: 19px; width: 35px;"></td>
					               <td class="tablecol" style="height: 19px;"></td>

                                    <%--Jules 2018.04.17 - PAMB Scrum 1--%>
					               	 <%--<td class="tablecol" style="height: 19px;">
                                       <asp:RadioButton ID="rdbWHTVendor" runat="server"  autopostback="true"  Width="282px" cssclass ="rbtn td" enabled ="false" Text="WHT applicable and payable by Vendor" /></td>--%>
                                    <td class="tablecol" style="height: 19px;">
                                       <asp:RadioButton ID="rdbWHTVendor" Visible="false" runat="server"  autopostback="true"  Width="282px" cssclass ="rbtn td" enabled ="false" Text="WHT applicable and payable by Vendor" /></td>
                                    <%--End modification--%>
								</tr>			                
								<tr valign="top">

									<td class="tablecol" style="height: 19px; width: 200px;">&nbsp;</td>

									<td class="tablecol" style="width: 226px" >
                                        <asp:TextBox ID="txtVenAddL3" runat="server"  CssClass="txtbox" Width="100%" Enabled = "false"></asp:TextBox></td> 
					                <td class="tablecol" style="height: 19px; width: 1px;"></td>
					                <td class="tablecol" style="height: 19px; width: 134px;"></td>
					               <td class="tablecol" style="height: 19px; width: 35px;"></td>
					               <td class="tablecol" style="height:19px;"></td>

                                    <%--Jules 2018.04.17 - PAMB Scrum 1--%>
					                <%--<td class="tablecol" style="height: 19px;">
                                       <asp:RadioButton ID="rdbNoWHT" runat="server" autopostback="true"  Text="No WHT" enabled ="false" cssclass ="rbtn td" />
                                    </td>	--%>
					                <td class="tablecol" style="height: 19px;">
                                       <asp:RadioButton ID="rdbNoWHT" Visible="false" runat="server" autopostback="true"  Text="No WHT" enabled ="false" cssclass ="rbtn td" />
                                    </td>
                                    <%--End modification--%>
                                       
								</tr>
	                            <tr valign="top">
									<td class="tablecol" style="height: 19px; width: 200px;"></td>
									<td class="tablecol" style="width: 226px"  >
                                        <asp:TextBox ID="txtVenAddPostcode" runat="server" CssClass="txtbox" Width="100%" Enabled = "false"></asp:TextBox></td>
					                <td class="tablecol" style="width: 1px" ></td>
					               <td class="tablecol" style="width: 134px;"></td>
					               <td class="tablecol" style="width: 35px;"></td>
					               <td class="tablecol" ></td>

                                    <%--Jules 2018.04.17 - PAMB Scrum 1--%>
					                    <%--<td class="tablecol" style="width: 256px" >
                                      <asp:label id = "lblwht" runat = "server" Width="215px">&nbsp;&nbsp;If no WHT, Please key in reason : </asp:label></td>--%>
                                    <td class="tablecol" style="width: 256px" >
                                      <asp:label id = "lblwht" Visible="false" runat = "server" Width="215px">&nbsp;&nbsp;If no WHT, Please key in reason : </asp:label></td>
                                    <%--End modification--%>

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

                                    <%--Jules 2018.04.17 - PAMB Scrum 1--%>
					                 <%--<td class="tablecol" rowspan ="3">&nbsp;
                                      <asp:TextBox ID="txtNoWHtreason" runat="server" cssclass="txtbox2" Rows = "4" MaxLength="1000" Width="98%" TextMode="MultiLine"  Enabled = "false" ></asp:TextBox></td>--%>
                                    <td class="tablecol" rowspan ="3">&nbsp;
                                      <asp:TextBox ID="txtNoWHtreason" Visible="false" runat="server" cssclass="txtbox2" Rows = "4" MaxLength="1000" Width="98%" TextMode="MultiLine"  Enabled = "false" ></asp:TextBox></td>
                                    <%--End modification--%>
									
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
									<td class="tablecol"  >&nbsp;<strong>Beneficiary Details</strong>
									<asp:label id="lblBeneficiaryDetailsMan" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
									<td class="tablecol" colspan = "6"><asp:textbox id="txtBeneficiaryDetails" runat="server" CssClass="txtbox" MaxLength="50"></asp:textbox></td>
					            </tr>		
								<tr valign="top" id="trLateReason" runat="server">
									<td class="tablecol" style="width: 200px" >&nbsp;<strong><asp:Label ID="Label5" runat="server"
                                        Text="Late Submission Reason"></asp:Label></strong>&nbsp;<asp:Label ID="Label6" runat="server"
                                        Text=":"></asp:Label>
                                    </td>
									<td class="tablecol" colspan = "6"><asp:textbox id="txtLateReason" runat="server" CssClass="txtbox"  MaxLength="1000"
											TextMode="MultiLine" Height="57px" Width = "100%"></asp:textbox></td>
	
								</tr>
								<%--Zulham PAMB 10/04/2018--%>
								<tr valign="top" >
								    <td class="tablecol" align="left">&nbsp;<strong>Attachment </strong>:&nbsp;<br />&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="small_remarks" >Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
			                        <td class="tablecol" colspan="5">
			                        <input class="button" id="File1Int" style="HEIGHT: 20px; BACKGROUND-COLOR: #ffffff;  width: 400px;" 
			                        type="file" name="uploadedFile3Int" runat="server" />&nbsp;
			                        <asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button></td>				            
								</tr>
								<tr valign="top">
									<td class="tablecol" style="HEIGHT: 19px">&nbsp;<strong>File Attached </strong>:</td>
									<td class="tablecol" style="HEIGHT: 19px" colspan="5">
									    <asp:panel id="pnlAttach" runat="server" ></asp:panel>
									</td>
								</tr>
								<%--End--%>
										
																			
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
					   <asp:button id="btnContinue" runat="server" CssClass="Button" Text="Continue"></asp:button>
					  <asp:button id="btnContinueMultiGL" runat="server" CssClass="Button" Text="Continue with Multi GL" Width="120px" Visible="False"></asp:button>
					  <%--Zulham 27032019--%>
					  <asp:button id="cmdMultiInvoice" runat="server" CssClass="Button" Text="Multi Invoices" Width="120px" Visible="False" ToolTip="Multiple invoice for the same vendor"></asp:button>&nbsp;
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
            <br />
            <div id="divSubDoc" runat="server" visible="false">
                <h1 class="tableHeader">Sub-Document</h1>
			    <asp:datagrid id="dtgSubDoc"  runat="server"  CssClass="Grid" AutoGenerateColumns="False" Width="100%">
					<Columns>
							<asp:TemplateColumn HeaderText="Action"  ItemStyle-Width="4%">						                
							<ItemStyle />
							    <ItemTemplate>
                                     <span runat="server" style="cursor:pointer;" id="cmdremovesubdoc"></span>
                                     <span runat="server" style="cursor:pointer;" id="cmdeditsubdoc"></span>                                                                                  
							    </ItemTemplate>
						    </asp:TemplateColumn>						
							<asp:BoundColumn DataField="isd_doc_no" HeaderText="Document No" ItemStyle-Width="13%">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="isd_doc_date" HeaderText="Document Date" ItemStyle-Width="13%">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundColumn>	
							<asp:BoundColumn DataField="isd_doc_amt" HeaderText="Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="13%">
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundColumn>
						    <asp:BoundColumn DataField="isd_doc_gst_value" HeaderText="GST Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="13%">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn ItemStyle-Width="48%" >
							    <ItemStyle HorizontalAlign="Center" />
							</asp:BoundColumn>
							<asp:BoundColumn DataField="LineNo" ItemStyle-Width="1px" >
							</asp:BoundColumn>	
					</Columns>
				 </asp:datagrid>
				</div>
				<div>
                    <asp:Label style="width:1024px" runat="server" ID="lblTitle"></asp:Label>
                </div>
            <div id="divInvDetail" runat="server">
                <%--Zulham 16102018 - PAMB--%>
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
							            <asp:BoundColumn DataField="ID_INVOICE_LINE" SortExpression="ID_INVOICE_LINE" HeaderText="S/NO">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
							            </asp:BoundColumn>
								        <asp:BoundColumn DataField="ID_PAY_FOR" SortExpression="ID_PAY_FOR" HeaderText="Pay For">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="id_gst_reimb" HeaderText="Disb./Reimb.">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="ID_REF_NO" SortExpression="ID_REF_NO" HeaderText="Invoice No.">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>	
								        <asp:BoundColumn DataField="ID_PRODUCT_DESC" SortExpression="ID_PRODUCT_DESC" HeaderText="Transaction Description">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>	
                                        <%--Zulham 27092018 - PAMB
                                        UAT U00007--%>
								        <asp:BoundColumn DataField="ID_UOM" SortExpression="ID_UOM" HeaderText="UOM" Visible="false">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="ID_RECEIVED_QTY" SortExpression="ID_RECEIVED_QTY" HeaderText="QTY" Visible="false">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>  
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="ID_UNIT_COST" SortExpression="ID_UNIT_COST" HeaderText="Unit Price" Visible="false">
								            <HeaderStyle HorizontalAlign="Right" />
									        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        </asp:BoundColumn>
                                        <%--Zulham 04102018 - PAMB SST--%>
								        <asp:BoundColumn DataField="ID_AMOUNT" HeaderText="Amount (excl.SST)">
								            <HeaderStyle HorizontalAlign="Right" />
									        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="id_gst_value" HeaderText="SST Amount">
								            <HeaderStyle HorizontalAlign="Right" />
									        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="id_gst_input_tax_code" SortExpression="id_gst_input_tax_code" HeaderText="Input Tax Code">
								            <HeaderStyle HorizontalAlign="Right" />
									        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="id_gst_output_tax_code" SortExpression="id_gst_output_tax_code" HeaderText="Output Tax Code">
								            <HeaderStyle HorizontalAlign="Right" />
									        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="ID_B_GL_CODE" SortExpression="ID_B_GL_CODE" HeaderText="GL Code">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>

                                        <%--Jules 2018.07.06 - Remove Sub Description--%>
								        <%--<asp:BoundColumn DataField="ID_GLRULE_CATEGORY" SortExpression="ID_GLRULE_CATEGORY" HeaderText="Sub Description">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>--%>

								        <asp:BoundColumn DataField="ID_ASSET_GROUP" SortExpression="ID_ASSET_GROUP" HeaderText="Asset Group" Visible="False">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="ID_ASSET_SUB_GROUP" SortExpression="ID_ASSET_SUB_GROUP" HeaderText="Asset Sub Group" Visible="False">								    
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>		
                                        
                                        <%--Jules 2018.04.17 - PAMB Scrum 1 - Removed Branch --%>
								        <%--<asp:BoundColumn DataField="ID_BRANCH_CODE_2" SortExpression="ID_BRANCH_CODE" HeaderText="HO/BR">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>--%>
                                        <%--End modification--%>

								        <asp:BoundColumn DataField="ID_COST_CENTER_2" SortExpression="ID_COST_CENTER" HeaderText="Cost Center(L7)">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>

                                        <%--Zulham PAMB - 25/04/2018--%>
                                        <%--Zulham 02102018 - PAMB SST--%>
								        <asp:BoundColumn DataField="ID_GIFT" SortExpression="ID_GIFT" HeaderText="Gift" Visible="false">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
								        <%--End--%>

                                        <%--Jules 2018.04.17 - PAMB Scrum 1 - Added Category and Analysis Codes--%>
                                        <asp:BoundColumn DataField="ID_CATEGORY" SortExpression="ID_CATEGORY" HeaderText="Category">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="FUNDTYPE" SortExpression="FUNDTYPE" HeaderText="Fund Type(L1)">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PRODUCTTYPE" SortExpression="PRODUCTTYPE" HeaderText="Product Type(L2)">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CHANNEL" SortExpression="CHANNEL" HeaderText="Channel(L3)">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="REINSURANCECO" SortExpression="REINSURANCECO" HeaderText="Reinsurance Company(L4)">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ASSETCODE" SortExpression="ASSETCODE" HeaderText="Asset Code(L5)">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <%--<asp:BoundColumn DataField="ID_ANALYSIS_CODE6" SortExpression="ID_ANALYSIS_CODE6" HeaderText="Tax Code">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_ANALYSIS_CODE7" SortExpression="ID_ANALYSIS_CODE7" HeaderText="Cost Centre">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>--%>
                                        <asp:BoundColumn DataField="PROJECTCODE" SortExpression="PROJECTCODE" HeaderText="Project Code(L8)">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PERSONCODE" SortExpression="PERSONCODE" HeaderText="Person Code(L9)">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_WITHHOLDING_TAX2" SortExpression="ID_WITHHOLDING_TAX2" HeaderText="Withholding Tax (%)">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
                                        <%--End modification--%>
								           <asp:BoundColumn Visible="false" DataField="ID_COST_ALLOC_CODE" SortExpression="ID_COST_ALLOC_CODE" HeaderText="Cost Allocation">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
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
				    <%--Zulham 17042015 IPP GST STAGE 2B--%>
				    <input class="txtbox" id="hidCountry" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidCountry" runat="server" />		
				    <%--Zulham 21/12/2015 IPP STAGE 4 Phase 2 --%>
				    <input class="txtbox" id="hidEmpId" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidEmpId" runat="server" />	    
				    <input class="txtbox" id="hidPaymentAmount" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidPaymentAmount" runat="server" />			    			
				    <%--Zulham PAMB 11/04/2018--%>
				    <input class="txtbox" id="hidFileIndex" type="hidden" size="2" name="hidFileIndex" runat="server" />				    
				</td>
			   </tr>
</table>
			<table class="alltable" id="table4" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td class = "emptycol" style="height: 24px">	
                        <%--Zulham 16102018 - PAMB--%>
					    <asp:button id="cmdAddLine" runat="server" CssClass="Button" Text="Add Line" ToolTip="Line item charge to single GL Code"></asp:button>&nbsp;
					    <asp:button id="cmdMultiGL" runat="server" CssClass="Button" Text="Multi GL" Visible="False" ToolTip="Multiple line items charge to different GL Code"></asp:button>&nbsp;
					    <asp:button id="cmdSave" runat="server" CssClass="Button" Text="Save" ></asp:button>&nbsp;
					    <asp:button id="cmdSubmit" runat="server" CssClass="Button" Text="Submit"></asp:button>&nbsp;
					    <%-- <asp:button id="cmdSetup" runat="server" CssClass="Button" Width="94px" style="display:none" Text="Approval Setup"></asp:button>&nbsp;--%>					    
					    <asp:button id="cmdVoid" runat="server" CssClass="Button" CausesValidation="False" Text="Void"></asp:button>&nbsp;
					    <asp:button id="cmdSubDoc" runat="server" CssClass="Button" Text="Add Sub" Visible="False"></asp:button>&nbsp;				    
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
