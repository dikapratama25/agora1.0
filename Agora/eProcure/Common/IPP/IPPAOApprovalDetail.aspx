<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IPPAOApprovalDetail.aspx.vb" Inherits="eProcure.IPPAOApprovalDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>E2P Document</title>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "STYLES.css") & """ rel='stylesheet'>"
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
		    Dim sCal As String = "<IMG src=" & dDispatcher.direct("Plugins/Images", "i_Calendar2.gif") & " border=""0"">"            
        </script>
         <% Response.Write(css)%>
         <% Response.Write(Session("JQuery"))%>
         <script type="text/javascript">
         
         $(document).ready(function(){
         if ($("#txtExchangeRate").val() == "")
         {
            $("#cmdPreview").attr("disabled","disabled");
         }
         else
         {
            $("#cmdPreview").attr("disabled","");
         }
         
         $("#txtExchangeRate").keyup(function(){
             if ($("#txtExchangeRate").val() == "")
             {
                $("#cmdPreview").attr("disabled","disabled");
             }
             else
             {
                $("#cmdPreview").attr("disabled","");
             }
         });
         });
         
        //'Zulham Sept 18, 2014
         function taxChange(ctrlid, ctrlid2, itemAmt, lblGSTAmt, predefinedOutputTax)
            {
               var inputTax = document.getElementById(ctrlid);
               var outputTax = document.getElementById(ctrlid2);
               var outputTaxIndex0 = outputTax.selectedIndex
               var outputTaxText0 = outputTax.options[outputTax.selectedIndex].text
               var lblGSTAmount = document.getElementById(lblGSTAmt);
               var Percentage;
                                     
               if (inputTax.options[inputTax.selectedIndex].text !== 'EX' && inputTax.options[inputTax.selectedIndex].text !== '---Select---' && inputTax.options[inputTax.selectedIndex].text !== 'N/A')
                {
                  if (inputTax.options[inputTax.selectedIndex].text == 'IM2 (6%)')
                  {
                    outputTax.options[outputTax.selectedIndex].text = predefinedOutputTax + ' (6%)';
                    //-----
                        for (var i = 0; i < outputTax.options.length; i++) {
                            if (outputTax.options[i].text== predefinedOutputTax) {
                                outputTax.options[i].selected = true;
                                return;
                            }
                        }
                    //-----
                   document.getElementById(lblGSTAmt).disabled = true;
                    document.getElementById(ctrlid2).disabled = true;
                  }
                 
                  else if (inputTax.options[inputTax.selectedIndex].text == 'IM1 (6%)' || inputTax.options[inputTax.selectedIndex].text == 'IM3 (6%)')
                  {
                      document.getElementById(lblGSTAmt).disabled = false;
                      outputTax.options[outputTax.selectedIndex].text = 'N/A';
                      document.getElementById(ctrlid2).disabled = true;
                  } 
                  
                  else{outputTax.selectedIndex = outputTaxIndex0;outputTax.options[outputTax.selectedIndex].text = outputTaxText0;document.getElementById(lblGSTAmt).disabled = false;document.getElementById(ctrlid2).disabled = false;}
                  if (inputTax.options[inputTax.selectedIndex].text == 'IM2 (6%)')
                  {Percentage = inputTax.options[inputTax.selectedIndex].text.split('(')[1].split('%')[0];
                  //parseFloat(invoiceAmt.replace(',','')) * (parseFloat(Percentage)/100);
                  lblGSTAmount.value = (parseFloat(itemAmt.replace(',','')) * (parseFloat(Percentage) / 100)).toFixed(2);
				  }
                }
                var bt = document.getElementById("btnPostBack"); 
                bt.click();
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
			
         function isDecimalKey(evt)
            {
                 var charCode = (evt.which) ? evt.which : event.keyCode
                 if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode != 46)
                    return false;

                 return true;
            }   
         //End
         
        function PopWindow(myLoc)
	    {
		    window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
		    return false;
	    }
		function confirmReject()
        {		        
	        ans=confirm("Are you sure that you want to reject this Invoice ?");
	        //alert(ans);
	        if (ans){		            
		        return true;
		        }
	        else
	        {	           
		        return false;
		    }
        }
        function validate()
        {
            var b = 0;
            var counter = 0; 
            var inputs = document.getElementById("txtExchangeRate");                                             
            for(b = 0; b <= inputs.value.length; b++ )
            {
                
                var check = inputs.value.charAt(b);                
                if(check == ".")
                {
                    counter = counter + 1;
                }
            }
            if(counter >= 2)
            {
                alert("Invalid Exchange Rate.");
                return false;
            }
        }	
        function popCalendar(val)
		{
			txtVal= document.getElementById(val);		
			window.open('<% response.write(PopCalendar) %>' ,'cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
		}	
		 function ShowDialog(filename,height)
		    {
    			
			    var retval="";
			    retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 800px");
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
		    
         </script>
</head>
<body class="body" ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_IPP_tabs"))%>
    <div>
        <table cellpadding="0" cellspacing="0" width="100%" class="AllTable">
        				<tr>
					<td class="header" colspan="7" style="height: 3px"></td>
				</tr>
				<tr>
					<td class="linespacing1" colspan="7"></td>
			    </tr>
				<tr>
                    <%--Zulham 15102018 - PAMB--%> 
	                <td class="emptycol" colspan="7">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Click the Verify button to verify the E2P Document or Reject button to reject the E2P Document."></asp:label>

	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colspan="7"></td>
			    </tr>
            <tr>
                <td class="TableHeader" colspan="7">&nbsp;Document Header</td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;&nbsp;<asp:Label ID="Label17" runat="server" Text=" Master Document :" Width="110px"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%">
                    <asp:Label ID="lblMasterDoc" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%">&nbsp;</td>
                <td class="TableCol" style="width: 15%"></td>
                <td colspan="3" class="TableCol" style="width: 180px;"></td> 
                <%--<td class="TableCol" style="width: 1%" rowspan="15">&nbsp;</td>--%>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text=" Document Type :" Width="110px"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%">
                    <asp:Label ID="lblDocType" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%">&nbsp;</td>
                <td class="TableCol" style="width: 15%">
                    <%--<strong><asp:Label ID="Label14" runat="server" Text=" Payment Type :"></asp:Label></strong>--%>
                    <strong><asp:Label ID="Label6" runat="server" Text=" Status :"></asp:Label></strong>
                    </td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <%--<asp:Label ID="lblPaymentType" runat="server"></asp:Label>--%>
                     <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </td> 
                <td class="TableCol" style="width: 1%" rowspan="15">&nbsp;</td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label2" runat="server" Text=" Document No. :"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%">
                    <asp:Label ID="lblDocNo" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%">
                </td>
                <td class="TableCol" style="width: 15%">
                     <strong><asp:Label ID="Label7" runat="server" Text=" Currency :"></asp:Label></strong>
                    </td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                   <asp:Label ID="lblCurrency" runat="server"></asp:Label>
                    </td>                
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label3" runat="server" Text=" Document Date :"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%">
                    <asp:Label ID="lblDocDate" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%">
                </td>
                <%--Zulham 17/02/2016 - IPP Stage 4 Phase 2--%>
                <%--<td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label8" runat="server" Text=" Payment Amount :"></asp:Label></strong>
                    </td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblPaymentAmt" runat="server"></asp:Label>
                    </td>--%>
                <%--Zulham 10102018 - PAMB SST--%>
                <td class="TableCol" style="width: 15%">
                <strong><asp:Label ID="Label8" runat="server" Text=" Total Amount (excl. SST) :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblTotalAmtNoGST" runat="server"></asp:Label></td>             
            </tr>
            <tr>
             <td class="TableCol" style="width: 20%">
                    <%--<strong>&nbsp;<asp:Label ID="Label18" runat="server" Text=" Document Received Date :"></asp:Label></strong>--%>
                    <strong>&nbsp;<asp:Label ID="Label20" runat="server" Text=" Document Due Date :"></asp:Label></strong>
                    </td>
                <td class="TableCol" style="width: 25%">
                    <%--<asp:Label ID="lblDocReceivedDate" runat="server"></asp:Label>--%>
                    <asp:Label ID="lblDocDueDate" runat="server"></asp:Label>
                    </td>
               <td class="TableCol" style="width: 1%"></td>
               <%--Zulham 17/02/2016 - IPP Stage 4 Phase 2--%>
                <%--<td class="TableCol" style="width: 15%">
                   <strong><asp:Label ID="Label9" runat="server" Text=" Payment Mode :"></asp:Label></strong>
                    </td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label>
                    </td>--%>
                <%--Zulham 09102018 - PAMB SST--%>
                <td class="TableCol" style="width: 15%">
                <strong><asp:Label ID="Label9" runat="server" Text=" SST Amount :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblGSTAmt" runat="server"></asp:Label></td>  
            </tr>

            <tr>
             <td class="TableCol" style="width: 20%">                   
                    <strong>&nbsp;<asp:Label ID="Label11" runat="server" Text=" Credit Term :"></asp:Label></strong>
                    </td>
                <td class="TableCol" style="width: 25%">                  
                    <asp:Label ID="lblCreditTerm" runat="server"></asp:Label>
                    </td>
               <td class="TableCol" style="width: 1%">
                </td>
                <%--Zulham 17/02/2016 - IPP Stage 4 Phase 2--%>
                <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label88" runat="server" Text=" Payment Amount :"></asp:Label></strong>
                    </td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblPaymentAmt" runat="server"></asp:Label>
                    </td>
            </tr>
              <tr>
                 <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label4" runat="server" Text=" Manual PO No. :"></asp:Label></strong>
                    </td>
                <td class="TableCol" style="width: 25%">
                     <asp:Label ID="lblManualPONo" runat="server"></asp:Label>
                    </td>
                <td class="TableCol" style="width: 1%">
                </td>
                <%--Zulham 17/02/2016 - IPP Stage 4 Phase 2--%>
                <td class="TableCol" style="width: 15%">
                   <strong><asp:Label ID="Label98" runat="server" Text=" Payment Mode :"></asp:Label></strong>
                    </td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <%--Zulham 21112018--%>
                    <asp:Label ID="lblPaymentMethod" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblPaymentMethodFull" runat="server"></asp:Label>
                    </td>
              <%--<td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label16" runat="server" Text=" Bank Code[Bank A/C No.] :"></asp:Label></strong>
                    </td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                     <asp:Label ID="lblBankNameAccountNo" runat="server" ></asp:Label>
                    </td>--%>
            </tr>
			
		
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label5" runat="server" Text=" Vendor :"></asp:Label></strong>
                    </td>
                <td class="TableCol" style="width: 25%">
                   <asp:Label ID="lblVendor" runat="server"></asp:Label>
                    </td>
                <td class="TableCol" style="width: 1%">
                </td>
                
              <%--Zulham 17/02/2016 - IPP Stage 4 Phase 2--%>
              <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label16" runat="server" Text=" Bank Code[Bank A/C No.] :"></asp:Label></strong>
                    </td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                     <asp:Label ID="lblBankNameAccountNo" runat="server" ></asp:Label>
                    </td>
            </tr>
            <%--Zulham 05122018--%>
            <tr id="trJobGrade" runat="server">
                <td class="TableCol">
                    <strong>&nbsp;<asp:Label ID="lblJobGrade" runat="server" Text=" Job Grade :"></asp:Label></strong>
                </td>
                <td class="TableCol">
                     <asp:Label ID="lblJobGrade_Val" runat="server"></asp:Label>
                </td>
                <td class="TableCol" colspan="4">
                </td>
                
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label14" runat="server" Text=" Vendor Address :"></asp:Label></strong>
                    </td>
                <td class="TableCol"  rowspan="10" style="width: 25%" valign="top">
                      <asp:Label ID="lblVendorAddr" runat="server"></asp:Label>
                    </td>
               
               <td class="TableCol" rowspan="10" style="width: 1%">
                &nbsp;
                </td>
                <%--Zulham 09072018 - PAMB
                'Hide withholding tax--%>
                <%--Zulham 17/02/2016 - IPP Stage 4 Phase 2--%>                   
                <td class="TableCol" rowspan="10" style="width: 15%">
                <%--<strong><asp:Label ID="Label108" runat="server" Text=" Withholding Tax :"></asp:Label></strong>--%>
                </td>
                <td class="TableCol" rowspan="10" valign="top" style="width: 8%">
                    <%--<asp:TextBox ID="txtTax" runat="server" CssClass="txtbox" Width="25px"></asp:TextBox>
                    <strong>(%)</strong>--%>
                </td> 
                 <td class="TableCol" rowspan="10" colspan="2">
                    <%--<asp:RadioButtonList ID="rbtnWHTOpt" runat="server" CssClass="rbtn">
                        <asp:ListItem Selected="True" Value="1">WHT applicable and payable by Company</asp:ListItem>
                        <asp:ListItem Value="2">WHT applicable and payable by Vendor</asp:ListItem>
                        <asp:ListItem Value="3">No WHT</asp:ListItem>
                    </asp:RadioButtonList>
                    If no WHT, please key in reason:<br />
                    <asp:TextBox ID="txtNoWHT" runat="server" TextMode="MultiLine" Height="111px" Width="280px"></asp:TextBox>--%>
                </td> 
                <%--End--%>
             </tr>           
          
            <tr>
            <td class="TableCol" style="width: 20%">
                </td>
   
                
            </tr>
            
           <tr>
                <td class="TableCol" style="width: 20%">
                </td>
                  
            </tr>
            
             <tr>
                <td class="TableCol" style="width: 20%">
                </td>
   
            </tr>
          
            <tr>
                <td class="TableCol" style="width: 20%">
                </td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%; height: 21px;">
                </td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                </td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%"></td>
            </tr>
            <tr>     
                <td class="TableCol"></td>          
            </tr>        
            <tr>
                <td class="TableCol" colspan="3">
                </td>
            </tr>	
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label13" runat="server" Text=" Internal Remarks"></asp:Label></strong></td>
                <td class="TableCol" colspan="10" style="width: 80%">
                    <asp:TextBox ID="txtRemarks" runat="server" Height="55px" TextMode="MultiLine" Width="800px" CssClass="txtbox"></asp:TextBox></td>
            </tr>
             <tr id="trBeneficiaryDetails">
					<td class="TableCol"  >&nbsp;<strong>Beneficiary Details</strong>&nbsp;:</td>
					<td class="TableCol" colspan = "6"><asp:textbox id="txtBenficiaryDetails" runat="server" CssClass="txtbox" ></asp:textbox></td>
			</tr>	
            <tr id="trLateReason" runat="server"> 
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label15" runat="server" Text=" Late Submission Reason"></asp:Label></strong></td>
                <td class="TableCol" colspan="10" style="width: 80%">
                    <asp:TextBox ID="txtLateSubmit" runat="server" Height="55px" TextMode="MultiLine" Enabled="false"  Width="800px" CssClass="txtbox"></asp:TextBox></td>
            </tr>
            
            <%--Zulham PAMB 10/04/2018--%>
			<tr valign="top" >
			    <td class="tablecol" align="left">&nbsp;<strong>Attachment </strong>:&nbsp;<br />&nbsp;<asp:Label id="lblAttach" runat="server" CssClass="small_remarks" >Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:Label></td>
                <td class="tableinput" colspan="6">
                <input class="button" id="File1Int" style="HEIGHT: 20px; BACKGROUND-COLOR: #ffffff;  width: 400px;" 
                type="file" name="uploadedFile3Int" runat="server" />&nbsp;
                <asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button></td>				            
			</tr>
			<tr valign="top">
				<td class="tablecol" style="HEIGHT: 19px">&nbsp;<strong>File Attached </strong>:</td>
				<td class="tableinput" style="HEIGHT: 19px" colspan="6">
				    <asp:panel id="pnlAttach" runat="server" ></asp:panel>
				</td>
			</tr>
			<%--End--%>
            
             <tr>
                <td class="TableCol" colspan="7"></td>
            </tr>
            <tr>
				<td class="EmptyCol" colspan="7"></td>
		    </tr>
            <tr style="display:none">
                <td>
                    <asp:Button runat="server" ID="btnPostBack"/>
                </td>
            </tr>
        </table>
 
        <%--Zulham PAMB 11/04/2018--%>
        <table>
			<tr>
			    <td class="emptycol">
			        <input class="txtbox" id="hidFileIndex" type="hidden" size="2" name="hidFileIndex" runat="server" />				    
			    </td>
		   </tr>
        </table>
        <%--End--%>
    
    </div>
           <table cellpadding="0" cellspacing="0" class="AllTable" style="height: 54px" width="100%">
                 <tr>
                    <td class="TableHeader">&nbsp;Approval Flow</td>
                 </tr>
                 <tr>	                    			   
			         <td class="EmptyCol">
				              <asp:datagrid id="dtgApprvFlow" runat="server" AutoGenerateColumns="False">
							        <Columns>								
								        <asp:BoundColumn DataField="Action" HeaderText="Performed By">
									        <HeaderStyle Width="10%"></HeaderStyle>
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>	
                                        <%--Zulham 13082018 - PAMB--%>
								        <asp:BoundColumn DataField="FA_AO" HeaderText="User ID">
								            <HeaderStyle Width="10%" />
								            <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="FA_ACTION_DATE" HeaderText="Date Time">
								            <HeaderStyle Width="10%" /> 
								            <ItemStyle HorizontalAlign="Left"></ItemStyle>   
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="FA_AO_REMARK" HeaderText="Remarks">
								            <HeaderStyle Width="60%" />
								            <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
						            </Columns>
						        </asp:datagrid>

				        </td>
				   </tr>
            </table>
        
					<div class="linespacing1"></div>
			    <table class="AllTable" cellpadding="0" cellspacing="0" width="100%" id="tbSubDoc" runat="server">
                 <tr>
                    <td class="TableHeader">&nbsp;Sub-Document Detail</td>
                 </tr>
                 <tr>	                    			   
			         <td class="EmptyCol">
				              <asp:datagrid id="dtgSubDocDetail" runat="server" OnPageIndexChanged="dtgSubDocDetail_Page" CssClass="Grid" AutoGenerateColumns="False">
							        <Columns>								
								        <asp:BoundColumn DataField="ISD_DOC_NO" HeaderText="Document No.">
									        <HeaderStyle Width="13%"></HeaderStyle>
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>	
								        <asp:BoundColumn DataField="ISD_DOC_DATE" HeaderText="Document Date">
								            <HeaderStyle Width="13%" />
								            <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="ISD_DOC_AMT" HeaderText="Amount">
								            <HeaderStyle Width="13%" HorizontalAlign="Right" /> 
								            <ItemStyle HorizontalAlign="Right"></ItemStyle>   
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="isd_doc_gst_value" HeaderText="GST Amount">
								            <HeaderStyle Width="13%" HorizontalAlign="Right" /> 
								            <ItemStyle HorizontalAlign="Right"></ItemStyle>   
								        </asp:BoundColumn>
								        <asp:BoundColumn>
								            <HeaderStyle Width="48%"/>
								        </asp:BoundColumn>
						            </Columns>
						        </asp:datagrid>

				        </td>
				   </tr>
            </table>
            <div class="linespacing1"></div>  
            <table class="AllTable" cellpadding="0" cellspacing="0" width="100%">
                 <tr>
                    <td class="TableHeader">&nbsp;Document Line Detail</td>
                 </tr>
                 <tr>	                    			   
			         <td class="EmptyCol">
				                <asp:datagrid id="dtgDocDetail" runat="server" OnPageIndexChanged="dtgDocDetail_Page" CssClass="Grid" AutoGenerateColumns="False">
							        <Columns>								
							            <asp:BoundColumn DataField="ID_INVOICE_LINE" HeaderText="S/No">
							                <HeaderStyle Width="3%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="ID_PAY_FOR" HeaderText="Pay For">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <%--Zulham Sept 17, 2014--%>
							            <asp:BoundColumn DataField="id_gst_reimb" HeaderText="Disb./Reimb.">
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
							            <%--End--%>
							            <asp:BoundColumn DataField="ID_REF_NO" HeaderText="Invoice No.">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							             <asp:BoundColumn DataField="ID_PRODUCT_DESC" HeaderText="Description">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
                                        <%--Zulham 03102018 - PAMB SST--%>
							            <asp:BoundColumn DataField="ID_UOM" HeaderText="UOM" Visible="false">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="ID_RECEIVED_QTY" HeaderText="QTY" ItemStyle-HorizontalAlign="Center" Visible="false">
							                <HeaderStyle Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="ID_UNIT_COST" HeaderText="Unit Price" ItemStyle-HorizontalAlign="Right" Visible="false">
							                <HeaderStyle Width="7%" />
                                            <ItemStyle HorizontalAlign="Right" />
							            </asp:BoundColumn>
                                        <%--Zulham 13112018--%>
                                        <asp:BoundColumn HeaderText="Amount (FCY) (excl. SST)" ItemStyle-HorizontalAlign="Right">
							                <HeaderStyle Width="7%" />
                                            <ItemStyle HorizontalAlign="Right" />
							            </asp:BoundColumn>
							            <asp:BoundColumn HeaderText="Amount (MYR) (excl. SST)" ItemStyle-HorizontalAlign="Right">
							                <HeaderStyle Width="7%" />
                                            <ItemStyle HorizontalAlign="Right" />
							            </asp:BoundColumn>
							            <%--Zulham Sept 17, 2014--%>
                                        <%--<asp:BoundColumn DataField="id_gst_value" HeaderText="GST Amount">
								            <HeaderStyle HorizontalAlign="Right" />
									        <ItemStyle HorizontalAlign="Right"></ItemStyle>
								        </asp:BoundColumn>--%>
                                        <%--Zulham 04102018 - PAMB SST--%>
								        <asp:TemplateColumn  HeaderText="SST Amount">
								          <HeaderStyle HorizontalAlign="Right" Width="7%"/>
									      <ItemStyle HorizontalAlign="Right"></ItemStyle>
								            <ItemTemplate>
								                <asp:textbox runat="server" ID="lblGSTAmount" Width="55px" CssClass="numerictxtbox"/>
								            </ItemTemplate>
								        </asp:TemplateColumn>
								        <asp:TemplateColumn HeaderText="Input Tax Code">
								            <ItemTemplate>
								                <asp:DropDownList ID="ddlInputTax" runat="server" CssClass="ddl" />
								            </ItemTemplate>
								        </asp:TemplateColumn>
                                        <asp:TemplateColumn  HeaderText="Output Tax Code">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlOutputTax" runat="server" CssClass="ddl" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
							            <%--End--%>
							            <asp:BoundColumn DataField="ID_B_GL_CODE" HeaderText="GL Code">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <%--'Zulham 11072018 -PAMB Moved to here--%>
                                        <%--Zulham 19102018 - PAMB--%>
                                        <asp:BoundColumn DataField="ID_COST_CENTER_2" HeaderText="Cost Centre(L7)">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
                                        <%--End--%>
                                        <%--Zulham 11072018 - PAMB
                                        Added columns--%>
                                        <%--Zulham 03102018 - PAMB SST--%>
                                        <asp:BoundColumn DataField="ID_GIFT" HeaderText="Gift" Visible="false">
							                <HeaderStyle Width="5%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_CATEGORY" HeaderText="Category">
							                <HeaderStyle Width="5%" />
							            </asp:BoundColumn>
                                        <%--End--%>
                                        <%--Zulham 19102018 - PAMB
                                        Added columns--%>
                                        <asp:BoundColumn DataField="ID_ANALYSIS_CODE1" HeaderText="Fund Type(L1)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_ANALYSIS_CODE2" HeaderText="Product Type(L2)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_ANALYSIS_CODE3" HeaderText="Channel(L3)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_ANALYSIS_CODE4" HeaderText="Reinsurance Comp.(L4)" >
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_ANALYSIS_CODE5" HeaderText="Asset Code(L5)"  >
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_ANALYSIS_CODE8" HeaderText="Project Code(L8)" >
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_ANALYSIS_CODE9" HeaderText="Person Code(L9)" >
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <%--End--%>
							            <asp:BoundColumn DataField="ID_ASSET_GROUP" HeaderText="Asset Group" Visible="false"> 
							                <HeaderStyle Width="10%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="ID_ASSET_SUB_GROUP" HeaderText="Asset Sub Group" Visible="false">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="ID_GLRULE_CATEGORY" HeaderText="Sub Description" Visible="false" >
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_BRANCH_CODE_2" HeaderText="HO/BR" Visible="false">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
                                        <%--'Zulham 09072018 - PAMB--%>
                                        <asp:BoundColumn DataField="ID_WITHHOLDING_TAX" HeaderText="Withholding Tax (%)">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
                                        <%--End--%>
                                        <%--'Zulham 09072018 - PAMB--%>
							            <asp:TemplateColumn HeaderText="Cost Allocation" Visible="false">
							                <HeaderStyle HorizontalAlign="Center" Width="7%"></HeaderStyle>
							                <ItemStyle HorizontalAlign="Left"></ItemStyle>								  
								      	    <ItemTemplate>
							                    <asp:HyperLink Runat="server" ID="lnkCostAlloc"></asp:HyperLink>							            
						                     </ItemTemplate>
								        </asp:TemplateColumn>		
							          	<%--End--%>
                                        <%--'Zulham 12082018 - PAMB--%>
                                        <asp:BoundColumn DataField="ID_DR_CURRENCY" HeaderText="Currency" Visible="false">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
                                        <%--End--%>
							        </Columns>
							    </asp:datagrid>						   				
					</td>
				</tr>	
				    
            </table>
            	
            <table class="AllTable" cellpadding="0" cellspacing="0" width="100%">
                <tr>
					<td class="linespacing1" colspan="7"></td>
			    </tr>
			    <tr id="TRExchangeRate" visible ="false" runat="server">
			        <td width= "15%" align="left" style="height: 19px; ">
	                    <strong><asp:Label ID="Label12" runat="server" Text="Exchange Rate :"></asp:Label></strong>
	                </td>
		            <td width= "85%" >
		                <asp:textbox id="txtExchangeRate" runat="server" CssClass="txtbox"></asp:textbox>
		            </td>
			    </tr>
                <tr>
	                <td width= "15%" align="left" style="height: 19px; ">
	                    <strong><asp:Label ID="Label43" runat="server" Text="Remarks "></asp:Label></strong>
                        <%--Zulham 12082018 - PAMB--%>
                        <asp:label id="Label26" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:
	                </td>
		            <td width= "85%" >
		                <asp:textbox id="txtApprRejRemark" width="100%" runat="server" CssClass="txtbox" MaxLength="3000" Height="37px" Rows="2" TextMode="MultiLine" ></asp:textbox>
		            </td>
				</tr>
				   	<tr>
					<td class="linespacing1" colspan="7"></td>
			    </tr>
		        <tr id="trButton" runat="server">
				    <td colspan="4" style="height: 24px">
				        <asp:button id="cmdAppIPP" runat="server" CssClass="button" Text="Verify"></asp:button>
				        <asp:button id="cmdRejectIPP" runat="server" CssClass="button" Text="Reject"></asp:button>
				        <asp:button id="cmdViewAudit" runat="server" CssClass="button" Text="View Audit" CausesValidation="False"></asp:button>
				       <%-- <asp:button id="cmdSave" Visible="false" runat="server" CssClass="button" Text="Save"></asp:button>
				        <asp:button id="cmdPreview" Visible="false" runat="server" CssClass="button" Text="Preview"></asp:button>--%>
					</td>							
				</tr>	
				  	<tr>
					<td class="linespacing1" colspan="7"></td>
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
