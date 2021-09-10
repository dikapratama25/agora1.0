<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IPPApprovalList.aspx.vb" Inherits="eProcure.IPPApprovalList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
<head>
    <title>E2P Approval List</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
		    Dim sCal As String = "<IMG src=" & dDispatcher.direct("Plugins/Images", "i_Calendar2.gif") & " border=""0"">"            
        </script> 

        
        <% Response.Write(Session("JQuery")) %> 
        <% Response.Write(Session("AutoComplete")) %>
        <% Response.write(Session("typeahead")) %>
		<%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		
		  $(document).ready(function(){		 
          $('#cmdSubmit').click(function() {
        // alert(CheckAtLeastOne('chkSelection'));
      if (CheckAtLeastOne('chkSelection') == true){
            document.getElementById("cmdSubmit").style.display= "none";
          
                       
       }
          });          
		  });
//		function checkAtLeastOneResetSummary(p1, p2, cnt1, cnt2)
//			{
//				if (CheckAtLeastOne(p1,p2)== true) {
//					if (resetSummary(cnt1,cnt2)==true)
//					{
//					    var cmdSubmitEle = document.getElementById("cmdSubmit");
//					    if (cmdSubmitEle == null)
//                        {
//                           // document.getElementById("cmdApprove").style.display= "none";
//                        }
//                        else
//                        {
//                            document.getElementById("cmdSubmit").style.display= "none";
//                        }
//					  
//						return true;
//					}
//					else
//					{
//						return false;
//						}
//				}
//				else {				
//					return false;
//				}				
//			}	
		function selectAll()
		{
			SelectAllG("dtgIPP_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgIPP_ctl02_chkAll","chkSelection");
		}	
		
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}
		function popCalendar(val)
		{
			txtVal= document.getElementById(val);		
			window.open('<% response.write(PopCalendar) %>' ,'cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
		}										
		-->
		</script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <%  Response.Write(Session("w_IPP_tabs"))%>
			<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">								
			    <tr>
				    <TD class="linespacing1"></TD>
		        </TR>
				<TR>
	                <TD class="emptycol" colspan="5">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                Text="Fill in the search criteria and click Search button to list the relevant E2P Document. Click the No. to go to E2P document approval page.">
		                </asp:label>
	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2"></TD>
			    </tr>
				<tr>
					<td class="TableHeader" colspan="5">
                        <asp:Label ID="Label1" runat="server" Text="Search Criteria"></asp:Label>
                    </td>
				</tr>		
				<tr>
                    <td class="TableCol">
                        <asp:Label ID="Label3" CssClass="lbl" runat="server" Text=" Document No. :"></asp:Label>
                    </td>
                    <td class="TableCol">
                        <asp:TextBox ID="txtDocNo" CssClass="txtbox" runat="server" style="width:135px;"></asp:TextBox>
                    </td>                
                    <td class="TableCol">
                        <asp:Label ID="Label4" CssClass="lbl" runat="server" Text="Document Type : "></asp:Label>
                    </td>
                    <td class="TableCol">
                        <asp:DropDownList ID="ddlDocType" runat="server" CssClass="ddl" style="width:135px;">
                            <asp:ListItem Value="">---Select---</asp:ListItem>
                            <asp:ListItem Value="INV">Invoice</asp:ListItem>
                            <asp:ListItem Value="BILL">Bill</asp:ListItem>
                            <asp:ListItem Value="CN">Credit Note</asp:ListItem>
                            <asp:ListItem Value="DN">Debit Note</asp:ListItem>                        
                            <asp:ListItem Value="LETTER">Letter</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="TableCol">
                    </td>                                                    
                </tr>
                <tr>
                    <td class="TableCol">
                        <asp:Label ID="Label5" CssClass="lbl" runat="server" Text=" Document Start Date :"></asp:Label>
                    </td>
                    <td class="TableCol">
                        <input id="txtDocStartDate" runat="server" type="text" class="txtbox" readonly="readonly" style="width:130px;"></input><A onclick="popCalendar('txtDocStartDate');" href="javascript:;"><% Response.Write(sCal)%></A>
                    </td>
                    <td class="TableCol">
                        <asp:Label ID="Label6" CssClass="lbl" runat="server" Text="Document End Date :"></asp:Label>
                    </td>
                    <td class="TableCol" colspan = "2" >
                        <input id="txtDocEndDate" runat="server" type="text" class="txtbox" readonly="readonly" style="width:130px;"></input><A onclick="popCalendar('txtDocEndDate');" href="javascript:;"><% Response.Write(sCal)%></A>
                    </td>
                   
                </tr>  
                <tr style="height:30px">
                    <td class="TableCol">
                        <asp:Label ID="Label2" CssClass="lbl" runat="server" Text=" Vendor :"></asp:Label>
                    </td>
                 <td class="TableCol" colspan = "3" >
                        <input id="txtVendor" runat="server" type="text" class="txtbox" style="width:135px;"></input>
                        <%--<A onclick="popCalendar('txtDocStartDate');" href="javascript:;"><% Response.Write(sCal)%></A>--%>
                    </td>
                     <TD class="tablecol" align="right">
                        <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
					    <asp:button cssclass="button" runat="server" id="cmdClear" text="Clear" CausesValidation="false"></asp:button>
				    </TD>
                </tr>                                     
		</TABLE>
		<div class="linespacing2"></div>
        <TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
		    <tr>
			    <TD class="emptycol">
			        <asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
					    ShowSummary="False">
					</asp:validationsummary>
					<asp:comparevalidator id="vldDateFtDateTo" runat="server" ErrorMessage="End Date must greater than or equal to Start Date"
					    ControlToCompare="txtDocStartDate" ControlToValidate="txtDocEndDate" Display="None" Operator="GreaterThanEqual" Type="Date">
					</asp:comparevalidator>
					<asp:customvalidator id="vldDateFr" runat="server" ErrorMessage="Date To cannot be empty" ControlToValidate="txtDocStartDate"
					    Display="None" ClientValidationFunction="checkDateTo" Enabled="False">
					</asp:customvalidator>
					<asp:customvalidator id="vldDateTo" runat="server" ErrorMessage="Date From cannot be empty" ControlToValidate="txtDocEndDate"
					    Display="None" ClientValidationFunction="checkDateFr" Enabled="False">
					</asp:customvalidator>
				</TD>    					
		    </tr>		    
		    <TR>
			    <TD class="emptycol">
			        <asp:datagrid id="dtgIPP" runat="server" OnSortCommand="SortCommand_Click">
					    <Columns>
					        <asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
                            <%--<asp:BoundColumn SortExpression="IM_INVOICE_NO" DataField="IM_INVOICE_NO" HeaderText="Document No.">
				            <HeaderStyle Width="10%"></HeaderStyle>
			                </asp:BoundColumn>--%>
			                	<asp:TemplateColumn SortExpression="IM_INVOICE_NO" HeaderText="Document No.">
									<HeaderStyle Width="8%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkINVNo"></asp:HyperLink>
									</ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:TemplateColumn>
			                <asp:BoundColumn DataField="IM_INVOICE_TYPE" SortExpression="IM_INVOICE_TYPE" HeaderText="Document Type">
				            <HeaderStyle Width="10%"></HeaderStyle>
			                </asp:BoundColumn>
			                <asp:BoundColumn DataField="IM_DOC_DATE" SortExpression="IM_DOC_DATE" HeaderText="Submitted Date">
				            <HeaderStyle Width="10%"></HeaderStyle>
			                </asp:BoundColumn>
		                    <asp:BoundColumn DataField="IM_S_COY_NAME" SortExpression="IM_S_COY_NAME" HeaderText="Vendor">
                            <HeaderStyle Width="10%" />
                            </asp:BoundColumn>				
                            <asp:BoundColumn DataField="IM_CURRENCY_CODE" SortExpression="IM_CURRENCY_CODE" HeaderText="Currency">
                            <HeaderStyle Width="10%" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="IM_INVOICE_TOTAL" SortExpression="IM_INVOICE_TOTAL" HeaderText="Payment Amount">
                            <HeaderStyle Width="10%" />
                            </asp:BoundColumn>	
                            <asp:BoundColumn DataField="IM_PAYMENT_TERM" SortExpression="IM_PAYMENT_TERM" HeaderText="Payment Method">
                            <HeaderStyle Width="10%" />                                        
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="IM_INVOICE_INDEX" SortExpression="IM_INVOICE_INDEX" HeaderText="INDEX" Visible="false" >                                                            
                            </asp:BoundColumn>		
                            <%--<asp:TemplateColumn HeaderText="Bill/Invoice Approved by">--%>
                            <%--Zulham 12082018 - PAMB--%>
                            <asp:TemplateColumn HeaderText="Approval Remarks *">
									<HeaderStyle Width="10%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
									<ItemTemplate>
									<asp:Label id="lblRemarks" Runat="server"></asp:Label><BR>
										<asp:TextBox id="txtRemarks" CssClass="listtxtbox" Width="150px" MaxLength="400" Runat="server"
											TextMode="MultiLine" Rows="2"></asp:TextBox>
										<%--<asp:Label id="lblBillInvoiceApprBy" Runat="server"></asp:Label><BR>
										<asp:TextBox id="txtBillInvoiceApprBy" CssClass="listtxtbox" Width="150px" MaxLength="400" Runat="server"
											TextMode="MultiLine" Rows="2"></asp:TextBox>--%>
										<%--<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px"  contentEditable="false" 
											ForeColor="Red"></asp:TextBox><INPUT class="txtbox" id="hidCode" type="hidden" name="hidCode" runat="server">--%>
									</ItemTemplate>
								</asp:TemplateColumn>                     		
					    </Columns>
				    </asp:datagrid>
				    
				
				    
				    
				    
				</TD>		 
		    </TR>
		        	<tr>
				<td class="emptycol"><ul class="errormsg" id="vldsum" runat="server">
				</ul></td>
				</tr>
			<tr>
				    <TD class="linespacing1"></TD>
		        </TR>
		    <tr>
					<td class="emptycol" colspan="4">
						<%--<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save" CausesValidation="False"></asp:button>--%>
						<asp:button id="cmdSubmit" runat="server" Width="96px" CssClass="button" Text="Mass Verify"
							CausesValidation="False"></asp:button>						
						<input class="button" id="cmdReset" onclick="ValidatorReset();" type="button" value="Reset" name="cmdReset" runat="server"> 
					    <input id="hidMode" type="hidden" size="1" name="hidMode" runat="server"><INPUT id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server">&nbsp;					   
					</td>
				</tr>
	    </TABLE>
	    </form>
</body>
</html>
