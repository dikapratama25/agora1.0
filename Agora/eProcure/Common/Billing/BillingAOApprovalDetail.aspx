<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BillingAOApprovalDetail.aspx.vb" Inherits="eProcure.BillingAOApprovalDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IPP Document</title>
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
                  lblGSTAmount.value = (itemAmt * (Percentage / 100)).toFixed(2);
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
	                <td class="emptycol" colspan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Click the Approve button to approve the IPP Document or Reject button to reject the IPP Document."></asp:label>

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
                    <strong>&nbsp;<asp:Label ID="Label2" runat="server" Text="Document No. :"></asp:Label></strong></td>
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
                    <strong>&nbsp;</strong></td>
                <td class="TableCol" style="width: 25%">
                    <asp:Label ID="lblDocDate" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%">
                </td>
                <td class="TableCol" style="width: 15%">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Text="Total Amount Excluding Tax :"></asp:Label></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblNoGSTAmount" runat="server"></asp:Label></td>                
            </tr>
            <tr>
             <td class="TableCol" style="width: 20%">
                    <%--<strong>&nbsp;<asp:Label ID="Label18" runat="server" Text=" Document Received Date :"></asp:Label></strong>--%>
                    <strong>&nbsp;</strong></td>
                <td class="TableCol" style="width: 25%">
                    <%--<asp:Label ID="lblDocReceivedDate" runat="server"></asp:Label>--%>
                    <asp:Label ID="lblDocDueDate" runat="server"></asp:Label>
                    </td>
               <td class="TableCol" style="width: 1%">
                </td>
                           <td class="TableCol" style="width: 15%">
                   <strong>
                       <asp:Label ID="Label8" runat="server" Text="Total Amount :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblPaymentAmt" runat="server"></asp:Label>
                    </td>  
            </tr>

            <tr>
             <td class="TableCol" style="width: 20%; height: 19px;">                   
                    <strong>&nbsp;</strong></td>
                <td class="TableCol" style="width: 25%; height: 19px;">                  
                    <asp:Label ID="lblCreditTerm" runat="server"></asp:Label>
                    </td>
               <td class="TableCol" style="width: 1%; height: 19px;">
                </td>
                           <td class="TableCol" style="width: 15%; height: 19px;">                  
                    </td>
                <td colspan="2" class="TableCol" style="width: 180px; height: 19px;">                    
                    </td>  
            </tr>
              <tr>
                 <td class="TableCol" style="width: 20%; height: 19px;">
                    <strong>&nbsp;</strong></td>
                <td class="TableCol" style="width: 25%; height: 19px;">
                     <asp:Label ID="lblManualPONo" runat="server"></asp:Label>
                    </td>
                <td class="TableCol" style="width: 1%; height: 19px;">
                </td>
              <td class="TableCol" style="width: 15%; height: 19px;">
                    <strong></strong>
                    </td>
                <td colspan="2" class="TableCol" style="width: 180px; height: 19px;">
                    </td>
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
                       <td class="TableCol" style="width: 15%">
                <strong></strong>
             </td>
                <td class="TableCol" rowspan="6" valign="top" style="width: 8%">
                </td> 
                 <td class="TableCol" rowspan="6" colspan="2">
                </td> 
            </tr>
            <%--<tr id="trJobGrade" runat="server">
                <td>
                    <strong>&nbsp;</strong></td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>--%>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label14" runat="server" Text=" Vendor Address :"></asp:Label></strong>
                    </td>
                <td class="TableCol"  rowspan="5" style="width: 25%" valign="top">
                      <asp:Label ID="lblVendorAddr" runat="server"></asp:Label>
                    </td>
               
               <td class="TableCol" rowspan="5" style="width: 1%">
                &nbsp;
                </td>
                <td class="TableCol" rowspan="5" style="width: 1%">
                &nbsp;
                </td>
               

             </tr>
             <%--<tr>
            
                <td class="TableCol" style="width: 20%">
                    
                    </td>
                <td class="TableCol" style="width: 25%" >
                   
                    </td>
                     <td class="TableCol"   style="width: 1%">&nbsp;</td>
           
            </tr>--%>
            
          
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
<%--            <tr>
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
            </tr>    --%>    
            <tr>
                <td class="TableCol" colspan="7">
                </td>
            </tr>
      <%--      <tr>
				<td class="tablecol"  >&nbsp;<strong>Bill/Invoice Approved by</strong>
				<asp:label id="lblBillInvApprByMan" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:</td>
				<td class="tablecol" colspan = "6"><asp:textbox id="txtBillInvApprBy" runat="server" CssClass="txtbox2" style="width: 200px" ></asp:textbox></td>
			</tr>	--%>	
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label13" runat="server" Text=" Internal Remarks :"></asp:Label></strong></td>
                <td class="TableCol" colspan="10" style="width: 80%">
                    <asp:TextBox ID="txtRemarks" runat="server" Height="55px" TextMode="MultiLine" Width="600px" CssClass="txtbox"></asp:TextBox></td>
            </tr>
<%--             <tr id="trBeneficiaryDetails">
					<td class="TableCol"  ></td>
					<td class="TableCol" colspan = "6"></td>
			</tr>	
            <tr id="trLateReason" runat="server"> 
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label15" runat="server" Text=" Late Submission Reason"></asp:Label></strong></td>
                <td class="TableCol" colspan="10" style="width: 80%">
                    <asp:TextBox ID="txtLateSubmit" runat="server" Height="55px" TextMode="MultiLine" Enabled="false"  Width="600px" CssClass="txtbox"></asp:TextBox></td>
            </tr>
--%>             
            <%--<tr>
                <td class="TableCol" colspan="7"></td>
            </tr>--%>
            <tr>
				<td class="TableCol" colspan="7"></td>
		    </tr>
            <tr style="display:none">
                <td>
                    <asp:Button runat="server" ID="btnPostBack" />
                </td>
            </tr>
        </table>
 
    
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
								        <asp:BoundColumn DataField="FA_ACTIVE_AO" HeaderText="User ID">
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
			        <div class="linespacing1"></div>  
            <table class="AllTable" cellpadding="0" cellspacing="0" width="100%">
                 <tr>
                    <td class="TableHeader">&nbsp;Document Line Detail</td>
                 </tr>
                 <tr>	                    			   
			         <td class="EmptyCol">
				                <asp:datagrid id="dtgDocDetail" runat="server" OnPageIndexChanged="dtgDocDetail_Page" CssClass="Grid" AutoGenerateColumns="False">
							        <Columns>								
							            <asp:BoundColumn DataField="bm_INVOICE_LINE" HeaderText="S/No">
							                <HeaderStyle Width="3%" />
							            </asp:BoundColumn>
							            <%-- Zulham Stage 3 (20/07/2016) --%>
							            <asp:BoundColumn DataField="bm_ref_no" HeaderText="Billing No.">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <%-- --%>
							             <asp:BoundColumn DataField="bm_PRODUCT_DESC" HeaderText="Description">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="bm_UOM" HeaderText="UOM">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="bm_RECEIVED_QTY" HeaderText="QTY" ItemStyle-HorizontalAlign="Center">
							                <HeaderStyle Width="5%" />
                                            <ItemStyle HorizontalAlign="Center" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="bm_UNIT_COST" HeaderText="Unit Price" ItemStyle-HorizontalAlign="Right">
							                <HeaderStyle Width="7%" />
                                            <ItemStyle HorizontalAlign="Right" />
							            </asp:BoundColumn>
							            <asp:BoundColumn HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
							                <HeaderStyle Width="7%" />
                                            <ItemStyle HorizontalAlign="Right" />
							            </asp:BoundColumn>
								        <asp:TemplateColumn  HeaderText="GST Amount">
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
							            <asp:BoundColumn DataField="BM_B_GL_CODE" HeaderText="GL Code">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="bm_GLRULE_CATEGORY" HeaderText="Sub Description">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="bm_BRANCH_CODE_2" HeaderText="HO/BR">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="bm_COST_CENTER_2" HeaderText="Cost Centre">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
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
	                    <strong><asp:Label ID="Label43" runat="server" Text="Remarks :"></asp:Label></strong>
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
