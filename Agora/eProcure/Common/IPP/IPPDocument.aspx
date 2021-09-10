<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IPPDocument.aspx.vb" Inherits="eProcure.IPPDocument" %>

<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">

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
                <td class="TableHeader" colspan="6">&nbsp;Document Header</td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%;">
                    <strong>&nbsp;&nbsp;<asp:Label ID="Label20" runat="server" Text=" Master Document :" Width="110px"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%;">
                    <asp:Label ID="lblMasterDoc" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%;">&nbsp;</td>
                <td class="TableCol" style="width: 15%;"></td>
                <td colspan="2" class="TableCol" style="width: 180px;"></td>
                <%--<td class="TableCol" style="width: 1%" rowspan="17"></td>--%>
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
                <%--<td class="TableCol" style="width: 1%" rowspan="17">&nbsp;</td>--%>
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
                    <strong>&nbsp;<asp:Label ID="Label3" runat="server" Text=" Document Date :"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%">
                    <asp:Label ID="lblDocDate" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%">
                </td>
                <%--Zulham 15/02/2016 - Stage 4 Phase 2
                <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label8" runat="server" Text=" Payment Amount :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblPaymentAmt" runat="server"></asp:Label></td>--%>
                <%--Zulham 03102018 - PAMB SST--%>
                <td class="TableCol" style="width: 15%">
                <strong><asp:Label ID="Label8" runat="server" Text=" Total Amount (excl. SST) :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblTotalAmtNoGST" runat="server"></asp:Label></td>
            </tr>
            
            <tr>
            <td class="TableCol" style="width: 20%">                   
                    <strong>&nbsp;<asp:Label ID="Label18" runat="server" Text=" Document Due Date :"></asp:Label></strong></td>                
               <td class="TableCol" style="width: 25%">
               <asp:Label ID="lblDocDueDate" runat="server"></asp:Label>               
                    </td>
                <td class="TableCol" style="width: 1%">
                </td>
                <%--Zulham 15/02/2016 - Stage 4 Phase 2--%>
                <%--<td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label9" runat="server" Text=" Payment Mode :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label></td>--%>
                <%--Zulham 09102018 - PAMB SST--%>
                <td class="TableCol" style="width: 15%">
                <strong><asp:Label ID="Label9" runat="server" Text=" SST Amount :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblGSTAmt" runat="server"></asp:Label></td>
            </tr>
              <tr>
            <td class="TableCol" style="width: 20%">                   
                    <strong>&nbsp;<asp:Label ID="Label16" runat="server" Text=" Credit Term :"></asp:Label></strong></td>                
               <td class="TableCol" style="width: 25%">
               <asp:Label ID="lblCreditTerm" runat="server"></asp:Label>               
                    </td>
                <td class="TableCol" style="width: 1%">
                </td>
                <%--Zulham 15/02/2016 - Stage 4 Phase 2--%>
                <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label88" runat="server" Text=" Payment Amount :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblPaymentAmt" runat="server"></asp:Label></td>
            </tr>
            
              <tr>
                 <td class="TableCol" style="width: 20%">
                 <strong>&nbsp;<asp:Label ID="Label19" runat="server" Text=" PSD Sent Date :"></asp:Label></strong>
                   </td>
                <td class="TableCol" style="width: 25%">
                 <asp:Label ID="lblPRCSSentDate" runat="server"></asp:Label>
                    </td>
                <td class="TableCol" style="width: 1%">
                </td>
                <%--Zulham 15/02/2016 - Stage 4 Phase 2--%>
                <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label89" runat="server" Text=" Payment Mode :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <%--Zulham 21112018--%>
                    <asp:Label ID="lblPaymentMethod" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblPaymentMethodFull" runat="server"></asp:Label>
                </td>
              <%--<td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label14" runat="server" Text=" Bank Code[Bank A/C No.] :"></asp:Label></strong></td>
            <td class="TableCol" colspan="2"> <asp:Label ID="lblBankNameAccountNo" runat="server" ></asp:Label></td>--%>
    
            </tr>
             <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label21" runat="server" Text=" PSD Received Date :"></asp:Label></strong>
                    </td>
                <td class="TableCol" style="width: 25%">
                   <asp:Label ID="lblPRCSReceivedDate" runat="server"></asp:Label>
                    </td>
                <td class="TableCol" style="width: 1%">
                </td>
                <%--Zulham 15/02/2016 - Stage 4 Phase 2--%>
                <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label14" runat="server" Text=" Bank Code[Bank A/C No.] :"></asp:Label></strong></td>
                <td class="TableCol" colspan="2"> <asp:Label ID="lblBankNameAccountNo" runat="server" ></asp:Label></td>
                <%--<td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label15" runat="server" Text=" Payment Date :"></asp:Label></strong></td>
                <td class="TableCol" colspan="2"> <asp:Label ID="lblPaymentDate" runat="server" ></asp:Label></td>--%>
               
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
                <%--Zulham 15/02/2016 - Stage 4 Phase 2--%>
                <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label15" runat="server" Text=" Payment Date :"></asp:Label></strong></td>
                <td class="TableCol" colspan="2"> <asp:Label ID="lblPaymentDate" runat="server" ></asp:Label></td>
                <%--<td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label17" runat="server" Text=" Payment Advice No :"></asp:Label></strong></td>
            <td class="TableCol" colspan="2"> <asp:Label ID="lblPaymentNo" runat="server" ></asp:Label></td>--%>
            </tr>
<%--            <tr>
              <td class="TableCol" style="width: 20%">
                    
                    </td>
                <td class="TableCol" style="width: 25%">
                   
                    </td>
                <td class="TableCol" style="width: 1%">
                </td>
                  <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label20" runat="server" Text=" Banker's Cheque No. :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblBankerChequeNo" runat="server"></asp:Label></td>                 
               
               
            </tr>--%>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label5" runat="server" Text=" Vendor :"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%">
                    <asp:Label ID="lblVendor" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%">
                </td>
                <%--Zulham 15/02/2016 - Stage 4 Phase 2--%>
                <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label17" runat="server" Text=" Payment Advice No :"></asp:Label></strong></td>
                <td class="TableCol" colspan="2"> <asp:Label ID="lblPaymentNo" runat="server" ></asp:Label></td>
            <%--<td class="TableCol" rowspan="10" valign="top" style="width: 15%">
                    <strong><asp:Label ID="Label10" runat="server" Text=" Withholding Tax :"></asp:Label></strong></td>
                <td class="TableCol" rowspan="10" valign="top" style="width: 8%">
                    <asp:TextBox ID="txtTax" runat="server" CssClass="txtbox" Width="25px"></asp:TextBox>
                    <strong>(%)</strong>
                </td>
                
                <td class="TableCol" rowspan="10">
                    <asp:RadioButtonList ID="rbtnWHTOpt" runat="server" CssClass="rbtn" autopostback="true" >
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
                    <strong>&nbsp;<asp:Label ID="Label11" runat="server" Text=" Vendor Address :"></asp:Label></strong>
                </td>
                <td class="TableCol" rowspan="9" style="width: 25%" valign="top">
                    <asp:Label ID="lblVendorAddr" runat="server"></asp:Label>
                </td>
                <td class="TableCol" rowspan="9" style="width: 1%">
                </td>
              <%-- Zulham 09072018 - PAMB--%>
               <%--Zulham 15/02/2016 - Stage 4 Phase 2--%>
               <td class="TableCol" rowspan="10" valign="top" style="width: 15%">
                    <%--<strong><asp:Label ID="Label10" runat="server" Text=" Withholding Tax :"></asp:Label></strong>--%>
               </td>
                <td class="TableCol" rowspan="10" valign="top" style="width: 8%">
                    <%--<asp:TextBox ID="txtTax" runat="server" CssClass="txtbox" Width="25px"></asp:TextBox>
                    <strong>(%)</strong>--%>
                </td>
                
                <td class="TableCol" rowspan="10">
                   <%-- <asp:RadioButtonList ID="rbtnWHTOpt" runat="server" CssClass="rbtn" autopostback="true" >
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
                <td class="TableCol" style="width: 20%; height: 25px;"></td>
            </tr>
            <tr>     
                <td class="TableCol"></td>          
            </tr>
            <tr>     
                <td class="TableCol"></td>          
            </tr>  
            <tr>     
                <td class="TableCol"></td>          
            </tr> 
            <tr>     
                <td class="TableCol"></td>          
            </tr>           
            <tr>
                <td class="TableCol" colspan="3">
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
                    <strong>&nbsp;<asp:Label ID="Label13" runat="server" Text=" Internal Remarks"></asp:Label></strong></td>
                <td class="TableCol" colspan="6" style="width: 80%">
                    <asp:TextBox ID="txtRemarks" runat="server" Height="55px" TextMode="MultiLine" Width="800px" CssClass="txtbox"></asp:TextBox></td>
            </tr>
             <tr id="trBeneficiaryDetails">
					<td class="TableCol"  >&nbsp;<strong>Beneficiary Details</strong>&nbsp;:</td>
					<td class="TableCol" colspan = "6"><asp:textbox id="txtBenficiaryDetails" runat="server" CssClass="txtbox" ></asp:textbox></td>
			</tr>	
            <tr id="trLateReason" runat="server">
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label12" runat="server" Text=" Late Submission Reason"></asp:Label></strong></td>
                <td class="TableCol" colspan="6" style="width: 80%">
                    <asp:TextBox ID="txtLateReason" runat="server" Enabled="false" Height="55px" TextMode="MultiLine" Width="800px" CssClass="txtbox"></asp:TextBox></td>
            </tr>
            
            <%--Zulham PAMB 10/04/2018--%>
			<tr valign="top">
				<td class="tablecol" style="HEIGHT: 19px">&nbsp;<strong>File Attached </strong>:</td>
				<td class="tableinput" style="HEIGHT: 19px" colspan="5">
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
									        <HeaderStyle Width="15%"></HeaderStyle>
									        <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>	
								        <asp:BoundColumn DataField="ISD_DOC_DATE" HeaderText="Document Date">
								            <HeaderStyle Width="15%" />
								            <ItemStyle HorizontalAlign="Left"></ItemStyle>
								        </asp:BoundColumn>
								        <asp:BoundColumn DataField="ISD_DOC_AMT" HeaderText="Amount">
								            <HeaderStyle Width="15%" HorizontalAlign="Right" /> 
								            <ItemStyle HorizontalAlign="Right"></ItemStyle>   
								        </asp:BoundColumn>
								         <asp:BoundColumn DataField="isd_doc_gst_value" HeaderText=" GST Amount">
								            <HeaderStyle Width="15%" HorizontalAlign="Right" /> 
								            <ItemStyle HorizontalAlign="Right"></ItemStyle>   
								        </asp:BoundColumn>
								        <asp:BoundColumn>
								            <HeaderStyle Width="40%" HorizontalAlign="Right" /> 
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
				                <asp:datagrid id="dtgDocDetail" runat="server" OnPageIndexChanged="dtgDocDetail_Page" CssClass="Grid">
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
							              <asp:BoundColumn DataField="ID_DR_CURRENCY" HeaderText="Currency">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
                                        <%--Zulham 03102018 - PAMB SST--%>
							            <asp:BoundColumn DataField="ID_RECEIVED_QTY" HeaderText="QTY" ItemStyle-HorizontalAlign="Center" Visible="false">
							                <HeaderStyle Width="5%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="ID_UNIT_COST" HeaderText="Unit Price" ItemStyle-HorizontalAlign="Right" Visible="false">
							                <HeaderStyle Width="6%" />
							            </asp:BoundColumn>
                                        <%--Zulham 03102018 - PAMB SST--%>
							            <asp:BoundColumn HeaderText="Amount(FCY) (excl. SST)" ItemStyle-HorizontalAlign="Right">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							             <asp:BoundColumn HeaderText="Amount(MYR) (excl. SST)" ItemStyle-HorizontalAlign="Right">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <%--Zulham 12/01/2015 - IPP Stage 4 Phase 2--%>
								         <asp:TemplateColumn  HeaderText="SST Amount (FCY)">
								          <HeaderStyle HorizontalAlign="Right" Width="7%"/>
									      <ItemStyle HorizontalAlign="Right"></ItemStyle>
								            <ItemTemplate>
								                <asp:label runat="server" ID="lblForeignGSTAmount" Width="55px" CssClass="numerictxtbox"/>
								            </ItemTemplate>
								        </asp:TemplateColumn>
							            <%--Zulham Sept 17, 2014--%>
                                        <%--Zulham 04102018 - PAMB SST--%>
							            <asp:TemplateColumn  HeaderText="SST Amount (MYR)">
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
							            <asp:BoundColumn DataField="ID_B_GL_CODE" HeaderText="GL Code">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <%--Zulham 11072018 - PAMB Moved to here--%>
                                        <%--Zulham 19102018 - PAMB--%>
                                        <asp:BoundColumn DataField="ID_COST_CENTER_2" HeaderText="Cost Centre(L7)">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
                                        <%--End--%>
                                        <%--Zulham 11072018 - PAMB
                                        Added columns--%>
                                        <%--Zulham 03102018 - PAMB SST--%>
                                        <asp:BoundColumn DataField="ID_GIFT" HeaderText="Gift" Visible="false" >
							                <HeaderStyle Width="5%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_CATEGORY" HeaderText="Category">
							                <HeaderStyle Width="5%" />
							            </asp:BoundColumn>
                                        <%--End--%>

                                        <%--Jules 2018.07.11 - Changed Datafield names in order to display Analysis Codes : Description--%>
                                        <%--Zulham 19102018 - PAMB
                                        Added columns--%>
                                        <asp:BoundColumn DataField="FUNDTYPE" HeaderText="Fund Type(L1)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PRODUCTTYPE" HeaderText="Product Type(L2)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="CHANNEL" HeaderText="Channel(L3)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="REINSURANCECO" HeaderText="Reinsurance Comp.(L4)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ASSETCODE" HeaderText="Asset Code(L5)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PROJECTCODE" HeaderText="Project Code(L8)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PERSONCODE" HeaderText="Person Code(L9)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <%--End--%>
							            <asp:BoundColumn DataField="ID_ASSET_GROUP" HeaderText="Asset Group" Visible="false">
							                <HeaderStyle Width="10%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="ID_ASSET_SUB_GROUP" HeaderText="Asset Sub Group" Visible="false">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
                                        <%--'Zulham 11072018 - PAMB--%>
							   	        <asp:BoundColumn DataField="ID_GLRULE_CATEGORY" HeaderText="Sub Description" Visible="false" >
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
                                        <%--END--%>
							          	<asp:BoundColumn DataField="ID_BRANCH_CODE_2" HeaderText="HO/BR" Visible="false">
							                <HeaderStyle Width="6%" />
							            </asp:BoundColumn>
                                        <%--'Zulham 09072018 - PAMB--%>
                                        <asp:BoundColumn DataField="ID_WITHHOLDING_TAX" HeaderText="Withholding Tax (%)">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
                                        <%--End--%>
                                        <%--Zulham 11072018 - PAMB--%>
							             <asp:TemplateColumn HeaderText="Cost Allocation" Visible="false" >
							                <HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
							                <ItemStyle HorizontalAlign="Left"></ItemStyle>								  
								      	    <ItemTemplate>
							                    <asp:HyperLink Runat="server" ID="lnkCostAlloc"></asp:HyperLink>							            
						                     </ItemTemplate>
								        </asp:TemplateColumn>
                                        <%--End--%>
								          <asp:BoundColumn DataField="ID_DR_EXCHANGE_RATE" HeaderText="Exchange Rate">
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
