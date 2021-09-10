<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BillingDocument.aspx.vb" Inherits="eProcure.BillingDocument" %>

<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">

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
        </script>
         <% Response.Write(css)%> 
         <script type="text/javascript">
            function PopWindow(myLoc)
		    {
			    window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			    return false;
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
<body>
	<form id="Form1" method="post" runat="server">
		 <%  Response.Write(Session("w_AddPO_tabs"))%>
    <div>
        <table cellpadding="0" cellspacing="0" width="100%" class="AllTable">
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
                    <strong><asp:Label ID="Label6" runat="server" Text=" Status :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblStatus" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%" rowspan="17">&nbsp;</td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label2" runat="server" Text=" Document No. :"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%">
                    <asp:Label ID="lblDocNo" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%">
                </td>
                <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label7" runat="server" Text=" Currency :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblCurrency" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;</strong></td>
                <td class="TableCol" style="width: 25%">
                    </td>
                <td class="TableCol" style="width: 1%">
                </td>
                <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label8" runat="server" Text="Total Amount :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblPaymentAmt" runat="server"></asp:Label></td>
            </tr>
            
            <tr>
            <td class="TableCol" style="width: 20%">                   
                    <strong></strong></td>                
               <td class="TableCol" style="width: 25%">
                    </td>
                <td class="TableCol" style="width: 1%">
                </td>
                <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label9" runat="server" Text="Total Amount Excluding Tax :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblPaymentAmtwthGST" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label5" runat="server" Text="Customer :"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%">
                    <asp:Label ID="lblVendor" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%">
                </td>
            <td class="TableCol" rowspan="6" valign="top" style="width: 15%">
                    <strong></strong></td>
                <td class="TableCol" rowspan="6" valign="top" style="width: 8%">
                </td>
                
                <td class="TableCol" rowspan="6">
                </td>
            </tr>
            
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label11" runat="server" Text="Customer Address :"></asp:Label></strong>
                </td>
                <td class="TableCol" rowspan="5" style="width: 25%" valign="top">
                    <asp:Label ID="lblVendorAddr" runat="server"></asp:Label>
                </td>
                <td class="TableCol" rowspan="5" style="width: 1%">
                </td>
             <%-- <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label15" runat="server" Text=" Payment Date :"></asp:Label></strong></td>
            <td class="TableCol" colspan="2"> <asp:Label ID="lblPaymentDate" runat="server" ></asp:Label></td>--%>
              <%--  <td class="TableCol" rowspan="6"></td> 
                <td class="TableCol" rowspan="6" style="width: 180px">
                </td>--%>
                 <%--<td class="TableCol" rowspan="7" valign="top" style="width: 15%">
                    <strong><asp:Label ID="Label10" runat="server" Text=" Withholding Tax :"></asp:Label></strong></td>
                <td class="TableCol" rowspan="7" valign="top" style="width: 8%">
                    <asp:TextBox ID="txtTax" runat="server" CssClass="txtbox" Width="25px"></asp:TextBox>
                    <strong>(%)</strong>
                </td>
                
                <td class="TableCol" rowspan="7">
                    <asp:RadioButtonList ID="rbtnWHTOpt" runat="server" CssClass="rbtn">
                        <asp:ListItem Selected="True" Value="1">WHT applicable and payable by Company</asp:ListItem>
                        <asp:ListItem Value="2">WHT applicable and payable by Vendor</asp:ListItem>
                        <asp:ListItem Value="3">No WHT</asp:ListItem>
                    </asp:RadioButtonList>
                    If no WHT, please key in reason:<br />
                    <asp:TextBox ID="txtNoWHT" runat="server" TextMode="MultiLine" Height="111px" Width="280px"></asp:TextBox>
                </td>--%>
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
                <td class="TableCol" style="width: 20%; height: 25px;">
                    </td>
            </tr>
            <%--<tr>     
                <td class="TableCol">
                    </td>          
            </tr>
            <tr>     
                <td class="TableCol">
                    </td>          
            </tr>  
            <tr>     
                <td class="TableCol"></td>          
            </tr> 
            <tr>     
                <td class="TableCol"></td>          
            </tr>           --%>
            <tr>
                <td class="TableCol" colspan="7">
                </td>
            </tr>
            <!--Zulham-->
     <%--       <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label16" runat="server" Text=" Bill/Invoice Approved by"></asp:Label></strong></td>
                <td class="TableCol" colspan="6" style="width: 80%">
                    <asp:TextBox ID="txtRemarks1" runat="server" CssClass="txtbox2" style="width: 200px" ></asp:TextBox></td>
            </tr>--%>
            <!--End-->
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label13" runat="server" Text=" Internal Remarks :"></asp:Label></strong></td>
                <td class="TableCol" colspan="6" style="width: 80%">
                    <asp:TextBox ID="txtRemarks" runat="server" Height="55px" TextMode="MultiLine" Width="600px" CssClass="txtbox"></asp:TextBox></td>
            </tr>
             <tr id="trBeneficiaryDetails">
					<td class="TableCol"  ></td>
					<td class="TableCol" colspan = "6"></td>
			</tr>	
            <%--<tr id="trLateReason" runat="server">
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;</strong></td>
                <td class="TableCol" colspan="6" style="width: 80%">
                    </td>
            </tr> --%>         
        </table>
    
    </div>
           <table cellpadding="0" cellspacing="0" class="AllTable" style="height: 54px" width="100%">
                 <tr>
                    <td class="TableHeader">&nbsp;Approval Flow</td>
                 </tr>
                 <tr>	                    			   
			         <td class="EmptyCol">
				              <asp:datagrid id="dtgApprvFlow" runat="server">
							        <Columns>								
							        <%--  <asp:BoundColumn DataField="FA_SEQ" HeaderText="Level">
							                <HeaderStyle Width="10%" />
							            </asp:BoundColumn>--%>
								        <asp:BoundColumn DataField="Action" HeaderText="Performed By">
									        <HeaderStyle Width="10%"></HeaderStyle>
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>	
								  <%--      <asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
								            <HeaderStyle Width="20%" />
								        </asp:BoundColumn>	--%>
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
						        </asp:datagrid></td>
				   </tr>
            </table>
            <div class="linespacing1"></div> 
            <table class="AllTable" cellpadding="0" cellspacing="0" width="100%">
                 <tr>
                    <td class="TableHeader">&nbsp;Document Line Detail</td>
                 </tr>
                 <tr>	                    			   
			         <td class="EmptyCol">
				                <asp:datagrid id="dtgDocDetail" runat="server" OnPageIndexChanged="dtgDocDetail_Page" CssClass="Grid">
							        <Columns>								
							            <asp:BoundColumn DataField="BM_INVOICE_LINE" HeaderText="S/No">
							                <HeaderStyle Width="3%" />
							            </asp:BoundColumn>
							            <%--Zulham 28/07/2017 - IPP Stage 3--%>
							            <asp:BoundColumn DataField="BM_REF_NO" HeaderText="Billing No.">
							                <HeaderStyle Width="7%" />
									    </asp:BoundColumn>
							            <%--'''--%>
							             <asp:BoundColumn DataField="BM_PRODUCT_DESC" HeaderText="Description">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="BM_UOM" HeaderText="UOM">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							              <asp:BoundColumn DataField="BM_DR_CURRENCY" HeaderText="Currency">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="BM_RECEIVED_QTY" HeaderText="QTY" ItemStyle-HorizontalAlign="Center">
							                <HeaderStyle Width="5%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="BM_UNIT_COST" HeaderText="Unit Price" ItemStyle-HorizontalAlign="Right">
							                <HeaderStyle Width="6%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn HeaderText="Amount(FCY)" ItemStyle-HorizontalAlign="Right">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							             <asp:BoundColumn HeaderText="Amount(MYR)" ItemStyle-HorizontalAlign="Right">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <%--Zulham 13/10/2016 - IPP Stage 4 Phase 2--%>
								        <asp:TemplateColumn  HeaderText="GST Amount(FCY)">
								          <HeaderStyle HorizontalAlign="Right" Width="7%"/>
									      <ItemStyle HorizontalAlign="Right"></ItemStyle>
								            <ItemTemplate>
								                <asp:label runat="server" ID="lblForeignGSTAmount" Width="55px" CssClass="numerictxtbox"/>
								            </ItemTemplate>
								        </asp:TemplateColumn>
							            <%--Zulham Sept 17, 2014--%>
							            <asp:TemplateColumn  HeaderText="GST Amount(MYR)">
								          <HeaderStyle HorizontalAlign="Right" Width="7%"/>
									      <ItemStyle HorizontalAlign="Right"></ItemStyle>
								            <ItemTemplate>
								                <asp:label runat="server" ID="lblGSTAmount" Width="55px" CssClass="numerictxtbox"/>
								            </ItemTemplate>
								        </asp:TemplateColumn>
								        <asp:TemplateColumn  HeaderText="Input Tax Code">
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
							            <asp:BoundColumn DataField="BM_B_GL_CODE" HeaderText="GL Code">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
							   	        <asp:BoundColumn DataField="BM_GLRULE_CATEGORY" HeaderText="Sub Description">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							          	<asp:BoundColumn DataField="BM_BRANCH_CODE_2" HeaderText="HO/BR">
							                <HeaderStyle Width="6%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="BM_COST_CENTER_2" HeaderText="Cost Centre">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="BM_DR_EXCHANGE_RATE" HeaderText="Exchange Rate">
							                <HeaderStyle Width="5%" />
							            </asp:BoundColumn>
							        </Columns>
							    </asp:datagrid>						   				
					</td>
				</tr>
			<%--	<tr>
				    <td class="emptycol">
				        <a id="A1" runat="server" onclick="history.back();" href="#"><strong>&lt; Back</strong></a>
				        
				    </td>
				</tr>		    --%>  
			
			<tr id="TRExchangeRate" visible ="false" runat="server">
			        <td align="left" style="height: 19px; ">
	                    <strong><asp:Label ID="Label12" runat="server" Text="Exchange Rate :"></asp:Label></strong>
	                    <asp:textbox id="txtExchangeRate" runat="server" CssClass="txtbox"></asp:textbox>
	                </td>
			</tr>
						
			<tr>
				<td class="EmptyCol">					
					<asp:button id="cmdViewAudit" runat="server" CssClass="button" Text="View Audit" CausesValidation="False"></asp:button>
					<%--<asp:button id ="btnHidden1" CausesValidation="false" runat="server" style="display:none"  ></asp:button>--%>
                </td>
			</tr>	  
			<td class="EmptyCol">	</td>
									<tr>
					<td class="emptycol">
						<p><asp:hyperlink id="lnkBack" Runat="server">
								<strong>&lt; Back</strong></asp:hyperlink></p>
					</td>
					</tr>
				<%--</td> --%>   
            </table>
            
    </form>
       
</body>
</html>
