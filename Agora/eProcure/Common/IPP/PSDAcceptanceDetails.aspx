<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PSDAcceptanceDetails.aspx.vb" Inherits="eProcure.PSDAcceptanceDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//Dtd XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/Dtd/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IPP Document</title>
    <% Response.Write(Session("JQuery")) %> 
    <% Response.Write(Session("AutoComplete")) %>
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
         <script type="text/javascript">
            function PopWindow(myLoc)
		    {
			    window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			    return false;
		    }
		    function popCalendar(val)
		    {
			txtVal= document.getElementById(val);
			//window.open('../popCalendar.aspx?textbox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
			window.open('<% response.write(PopCalendar) %>' ,'cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
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
         </script>
</head>
<body>
	<form id="Form1" method="post" runat="server">
		  <% Response.Write(Session("w_IPP_tabs")) %>
    <div>
        <table cellpadding="0" cellspacing="0" width="100%" class="AllTable">
            <tr>
                <td class="TableHeader" colspan="6">&nbsp;Document Header</td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;&nbsp;<asp:Label ID="Label10" runat="server" Text=" Master Document :" Width="110px"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%">
                    <asp:Label ID="lblMasterDoc" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%"></td>
                <td class="TableCol" style="width: 15%"></td>
                <td colspan="2" class="TableCol" style="width: 180px;"></td>
                <%--<td class="TableCol" style="width: 1%" rowspan="14"></td>--%>
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
                <%--<td class="TableCol" style="width: 1%" rowspan="14">&nbsp;</td>--%>
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
                <%--Zulham 17/02/2016 - IPP Stage 4 Phase 2--%>
                <%--Zulham 03102018 - PAMB SST--%>
                <td class="TableCol" style="width: 15%">
                <strong><asp:Label ID="Label15" runat="server" Text=" Total Amount (excl. SST) :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                <asp:Label ID="lblTotalAmtNoGST" runat="server"></asp:Label></td>
                <%--<td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label8" runat="server" Text=" Payment Amount :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblPaymentAmt" runat="server"></asp:Label></td>--%>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label16" runat="server" Text=" Document Due Date :"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%">
                    <asp:Label ID="lblDocDueDate" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%">
                </td>
                <%--Zulham 17/02/2016 - IPP Stage 4 Phase 2--%>
                <%--Zulham 09102018 - PAMB SST--%>
                <td class="TableCol" style="width: 15%">
                <strong><asp:Label ID="Label9" runat="server" Text=" SST Amount :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                <asp:Label ID="lblGSTAmt" runat="server"></asp:Label></td>
                <%--<td class="TableCol" style="width: 15%; height: 19px;">
                    <strong><asp:Label ID="Label9" runat="server" Text=" Payment Mode :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px; height: 19px;">
                    <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label></td>--%>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label21" runat="server" Text=" PSD Sent Date :"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%">
                    <asp:Label ID="lblpsdSentDate" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%">
                </td>
                <%--Zulham 17/02/2016 - IPP Stage 4 Phase 2--%>
                <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label8" runat="server" Text=" Payment Amount :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px;">
                    <asp:Label ID="lblPaymentAmt" runat="server"></asp:Label></td>
<%--                  <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label14" runat="server" Text=" Bank Code[Bank A/C No.] :"></asp:Label></strong></td>
                  <td class="TableCol" colspan="2"> <asp:Label ID="lblBankNameAccountNo" runat="server" ></asp:Label></td>
--%>    
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label25" runat="server" Text=" PSD Received Date :"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%">
                                            <input id="txtPRCSSentDate" runat="server" type="text" class="txtbox" readonly="readonly" style="width:120px;"/>
                        <a onclick="popCalendar('txtPRCSSentDate');" href="javascript:;"><% Response.Write(sCal)%></a>
                        </td>
                <td class="TableCol" style="width: 1%">
                </td>
                <%--Zulham 17/02/2016 - IPP Stage 4 Phase 2--%>
                <td class="TableCol" style="width: 15%; height: 19px;">
                    <strong><asp:Label ID="Label91" runat="server" Text=" Payment Mode :"></asp:Label></strong></td>
                <td colspan="2" class="TableCol" style="width: 180px; height: 19px;">
                    <%--Zulham 21112018--%>
                    <asp:Label ID="lblPaymentMethod" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblPaymentMethodFull" runat="server"></asp:Label>
                </td>
                <%--<td class="TableCol" rowspan="10" valign="top" style="width: 15%">
                    <strong><asp:Label ID="Label18" runat="server" Text=" Withholding Tax :"></asp:Label></strong></td>
                <td class="TableCol" rowspan="10" valign="top" style="width: 5%">
                    <asp:TextBox ID="txtTax" runat="server" CssClass="txtbox" Width="25px"></asp:TextBox>
                    <strong>(%)</strong>
                </td>
                
                <td class="TableCol" rowspan="10">
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
                <td class="TableCol" style="width: 20%; height: 19px;">
                    <strong>&nbsp;<asp:Label ID="Label4" runat="server" Text=" Manual PO No. :"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%; height: 19px;">
                    <asp:Label ID="lblManualPONo" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%; height: 19px;">
                </td>
                <%--'Zulham 17/02/2016 - IPP Stage 4 Phase 2--%>
                <td class="TableCol" style="width: 15%">
                    <strong><asp:Label ID="Label14" runat="server" Text=" Bank Code[Bank A/C No.] :"></asp:Label></strong></td>
                  <td class="TableCol" colspan="2"> <asp:Label ID="lblBankNameAccountNo" runat="server" ></asp:Label></td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label5" runat="server" Text=" Vendor :"></asp:Label></strong></td>
                <td class="TableCol" style="width: 25%">
                    <asp:Label ID="lblVendor" runat="server"></asp:Label></td>
                <td class="TableCol" style="width: 1%">
                </td>
            <%--Zulham 13072018 - PAMB--%> 
            <%--Zulham 17/02/2016 - IPP Stage 4 Phase 2--%>
            <td class="TableCol" rowspan="10" valign="top" style="width: 15%">
                    <%--<strong><asp:Label ID="Label18" runat="server" Text=" Withholding Tax :"></asp:Label></strong>--%>
            </td>
                <td class="TableCol" rowspan="10" valign="top" style="width: 5%">
                    <%--<asp:TextBox ID="txtTax" runat="server" CssClass="txtbox" Width="25px"></asp:TextBox>
                    <strong>(%)</strong>--%>
                </td>
                
                <td class="TableCol" rowspan="10">
                    <%--<asp:RadioButtonList ID="rbtnWHTOpt" runat="server" CssClass="rbtn">
                        <asp:ListItem Selected="True" Value="1">WHT applicable and payable by Company</asp:ListItem>
                        <asp:ListItem Value="2">WHT applicable and payable by Vendor</asp:ListItem>
                        <asp:ListItem Value="3">No WHT</asp:ListItem>
                    </asp:RadioButtonList>
                    If no WHT, please key in reason:<br />
                    <asp:TextBox ID="txtNoWHT" runat="server" TextMode="MultiLine" Height="111px" Width="280px"></asp:TextBox>--%>
                </td>
            </tr>
            
            <tr>
                <td class="TableCol" style="width: 20%; height: 19px;">
                    <strong>&nbsp;<asp:Label ID="Label11" runat="server" Text=" Vendor Address :"></asp:Label></strong>
                </td>
                <td class="TableCol" rowspan="7" style="width: 25%" valign="top">
                    <asp:Label ID="lblVendorAddr" runat="server"></asp:Label>
                </td>
                <td class="TableCol" rowspan="7" style="width: 1%">
                </td>
             
            </tr>
          
               <%--<tr>
                <td class="TableCol" style="width: 20%">
                </td>
                <td class="TableCol" rowspan="7" valign="top" style="width: 15%">
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
                </td>
            </tr>--%>
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
            <tr>
                <td class="TableCol" colspan="3">
                </td>
            </tr>
            <tr>
                <td class="TableCol" colspan="6">
                </td>
            </tr>
            <tr>
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label13" runat="server" Text=" Internal Remarks"></asp:Label></strong></td>
                <td class="TableCol" colspan="5" style="width: 80%">
                    <asp:TextBox ID="txtRemarks" runat="server" Height="55px" TextMode="MultiLine" Width="800px"></asp:TextBox></td>
            </tr>
            <tr id="trLateReason" runat="server">
                <td class="TableCol" style="width: 20%">
                    <strong>&nbsp;<asp:Label ID="Label12" runat="server" Text=" Late Submission Reason"></asp:Label></strong></td>
                <td class="TableCol" colspan="6" style="width: 80%">
                    <asp:TextBox ID="txtLateReason" runat="server" Enabled="false" Height="55px" TextMode="MultiLine" Width="800px"></asp:TextBox></td>
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
                <td class="TableCol" colspan="6"></td>
            </tr>
            <tr>
				<td class="EmptyCol" colspan="6"></td>
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
								        <%--Zulham Aug 27, 2014--%>
								        <asp:TemplateColumn HeaderText="GST Amount" ItemStyle-Width="13%">
									        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									        <ItemTemplate>
										        <asp:textbox id="txtGSTAmount" style="margin-right:0px;width:100%" runat="server" CssClass="numerictxtbox" autopostback="true"/>
								            </ItemTemplate>
								        </asp:TemplateColumn>
								        <%--End--%>
								        <asp:BoundColumn>
								            <HeaderStyle Width="48%"/> 
								        </asp:BoundColumn>
						            </Columns>
						        </asp:datagrid>

				        </td>
				   </tr>
            </table>
            <table class="AllTable" cellpadding="0" cellspacing="0" width="100%">
                 <tr>
                    <td class="TableHeader" style="height: 19px">&nbsp;Document Line Detail</td>
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
							             <asp:BoundColumn DataField="ID_PRODUCT_DESC" HeaderText="Transaction Description">
							                <HeaderStyle Width="12%" />
							            </asp:BoundColumn>
                                        <%--Zulham 04102018 - PAMB SST--%>
							            <asp:BoundColumn DataField="ID_UOM" HeaderText="UOM" Visible="false">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							           <%-- <asp:BoundColumn DataField="ID_DR_CURRENCY" HeaderText="Currency">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>--%>
							            <asp:BoundColumn DataField="ID_RECEIVED_QTY" HeaderText="QTY" ItemStyle-HorizontalAlign="Center" Visible="false">
							                <HeaderStyle Width="5%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="ID_UNIT_COST" HeaderText="Unit Price" ItemStyle-HorizontalAlign="Right" Visible="false">
							                <HeaderStyle Width="6%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn HeaderText="Amount(FCY) (excl. SST)" ItemStyle-HorizontalAlign="Right">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							             <asp:BoundColumn HeaderText="Amount(RM) (excl. SST)" ItemStyle-HorizontalAlign="Right">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							            <%--Zulham Sept 17, 2014--%>
                                        <%--Zulham 04102018 - PAMB SST--%>
							            <asp:TemplateColumn  HeaderText="SST Amount">
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
							            <asp:BoundColumn DataField="ID_ASSET_GROUP" HeaderText="Asset Group" Visible="false">
							                <HeaderStyle Width="10%" />
							            </asp:BoundColumn>
							            <asp:BoundColumn DataField="ID_ASSET_SUB_GROUP" HeaderText="Asset Sub Group" Visible="false">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							   	        <asp:BoundColumn DataField="ID_GLRULE_CATEGORY" HeaderText="Sub Description" Visible="false">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
							          	<asp:BoundColumn DataField="ID_BRANCH_CODE_2" HeaderText="HO/BR" Visible="false">
							                <HeaderStyle Width="6%" />
							            </asp:BoundColumn>
                                        <%--Zulham 19102018 - PAMB--%>
							            <asp:BoundColumn DataField="ID_COST_CENTER_2" HeaderText="Cost Centre(L7)">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
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
                                        <asp:BoundColumn DataField="ID_ANALYSIS_CODE4" HeaderText="Reinsurance Comp.(L4)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_ANALYSIS_CODE5" HeaderText="Asset Code(L5)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_ANALYSIS_CODE8" HeaderText="Project Code(L8)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <asp:BoundColumn DataField="ID_ANALYSIS_CODE9" HeaderText="Person Code(L9)">
							                <HeaderStyle Width="8%" />
							            </asp:BoundColumn>
                                        <%--End--%>
                                        <%--'Zulham 09072018 - PAMB--%>
                                        <asp:BoundColumn DataField="ID_WITHHOLDING_TAX" HeaderText="Withholding Tax (%)">
							                <HeaderStyle Width="7%" />
							            </asp:BoundColumn>
                                        <%--End--%>
							             <asp:TemplateColumn HeaderText="Cost Allocation" Visible="false" >
							                <HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
							                <ItemStyle HorizontalAlign="Left"></ItemStyle>								  
								      	    <ItemTemplate>
							                    <asp:HyperLink Runat="server" ID="lnkCostAlloc"></asp:HyperLink>							            
						                     </ItemTemplate>
								        </asp:TemplateColumn>	
							        </Columns>
							    </asp:datagrid>						   				
					</td>
				</tr>
				<tr>
				    <td class="emptycol">
				        <table width="100%">
				            <tr>
				                <td style="width:20%">
				                    <strong><asp:label ID="lbll" text="Remarks :" runat="server"/></strong>		                    
				                </td>
				                <td style="width:80%">
				                    <asp:TextBox runat="server" ID="txtApprRejRemark" Width="87%" textmode="MultiLine" Rows="4" />				                    
				                </td>
				            </tr>
				        </table>
				    </td>
				</tr>
				<tr>
				    <td class="emptycol">
				        <asp:Button runat="server" ID="cmdAccept" Text="Accept" CssClass="button" />
				        <asp:Button runat="server" ID="cmdReject" Text="Reject" CssClass="button"  />
				    </td>
				</tr>		        
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
