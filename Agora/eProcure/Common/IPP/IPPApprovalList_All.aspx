<%@ Page Language="vb" AutoEventWireup="false" Codebehind="IPPApprovalList_All.aspx.vb" Inherits="eProcure.IPPApprovalList_All" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PR Approval</title>
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
		<% Response.write(Session("typeahead2")) %>
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","help:No;resizable: No;Status:No;dialogHeight: 255px; dialogWidth: 300px");
			return false;
		}
		
		function popCalendar(val)
		{
			txtVal= document.getElementById(val);
			//window.open('../popCalendar.aspx?textbox=' + val + '&seldate=' + txtVal.value ,'cal','status=no,resizable=no,width=200,height=180,left=270,top=180');
			window.open('<%Response.Write(PopCalendar) %>','cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
		}
		function checkDateTo(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtDocEndDate.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		function checkDateFr(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtDocStartDate.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		function removeCommas(val)
		{
			var r = new RegExp(",","gi");
			//if (val=="")
			//	aa="99,999,999,999.3444";
			//else
			//	aa=val;
			var newstr = val.replace(r, '') ;
			//alert("newstr=" + newstr);
			return newstr;

		}
		
		function addCommas(argNum, argThouSeparator, argDecimalPoint)
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
			alert("(\\d)(\\d{3}(\\" + sThou + "|\\" + sDec + "))");
          while (sInt.match(rTest))
          {
                  // insert thousands-separator before the three digits
                  sInt = sInt.replace(rTest, "$1" + sThou + "$2")
          }
 
          // plug the modified integer back in, removing the temporary 	decimal point
          aParts[0] = sInt.replace(sDec, "")
          //alert(aParts.join(sDec));
          //document.forms[0].txtVendor.value=aParts.join(sDec);
          //alert(Test(aParts.join(sDec)));
          //document.forms[0].txtPRNo.value=Test(aParts.join(sDec));
          return aParts.join(sDec)
 }

		function round(number,X) {
		// rounds number to X decimal places, defaults to 2
			X = (!X ? 2 : X);
			val=Math.round(number*Math.pow(10,X))/Math.pow(10,X);
			alert(val);
			//return val;
			//var newnumber = Math.round(numberField.value*Math.pow(10,rlength))/Math.pow(10,rlength);
		}



		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
 		<form id="Form1" method="post" runat="server">
 		<%  Response.Write(Session("w_IPP_tabs"))%>
              <TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0" width="100%">				
				<TR>
					<TD class="header" colSpan="7" style="height: 3px"></TD>
				</TR>
				<tr>
					<TD class="linespacing1" colSpan="7"></TD>
			    </TR>
				<TR>
	                <TD class="emptycol" colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant E2P document."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			    </TR>
				<tr>
								<td class="tableheader" align="left" colSpan="7" style="height: 19px">&nbsp;Search Criteria</td>
							</tr>
		<%--		<TR id="tdAO" style="VISIBILITY: visible" runat="server">
					<TD class="tablecol" style="height:30px;">&nbsp;<STRONG><asp:Label ID="Label1" runat="server" Text="Approve/Endorse By :"></asp:Label></STRONG></TD>
					<td class="TableInput" colspan="6"><asp:dropdownlist id="cboAO" Width="180px" Runat="server" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist></td>
				</TR>--%>
							 <tr>
                <td class="TableCol">
                    <asp:Label ID="Label3" CssClass="lbl" runat="server" Text=" Document No. :"></asp:Label></td>
                <td class="TableCol">
                    <asp:TextBox ID="txtDocNo" CssClass="txtbox" runat="server" style="width:135px;"></asp:TextBox></td>                
                <td class="TableCol">
                    <asp:Label ID="Label4" CssClass="lbl" runat="server" Text="Document Type : "></asp:Label></td>
                <td class="TableCol">
                    <asp:DropDownList ID="ddlDocType" CssClass="ddl" style="width:135px;" runat="server">
                        <asp:ListItem Value="">---Select---</asp:ListItem>
                        <asp:ListItem Value="INV">Invoice</asp:ListItem>
                        <asp:ListItem Value="BILL">Bill</asp:ListItem>
                        <asp:ListItem Value="CN">Credit Note</asp:ListItem>
                        <asp:ListItem Value="DN">Debit Note</asp:ListItem>                        
                        <asp:ListItem Value="LETTER">Letter</asp:ListItem>
                    </asp:DropDownList></td>
                <td class="TableCol"></td>                                  
            </tr>
            <tr>
                <td class="TableCol">
                    <asp:Label ID="Label5" CssClass="lbl" runat="server" Text=" Document Start Date :"></asp:Label></td>
                <td class="TableCol">
                    <input id="txtDocStartDate" runat="server" type="text" class="txtbox" readonly="readonly" style="width:130px;"></input><A onclick="popCalendar('txtDocStartDate');" href="javascript:;"><% Response.Write(sCal)%></A></td>
                <td class="TableCol">
                    <asp:Label ID="Label6" CssClass="lbl" runat="server" Text="Document End Date :"></asp:Label></td>
                <td class="TableCol">
                    <input id="txtDocEndDate" runat="server" type="text" class="txtbox" readonly="readonly" style="width:130px;"></input><A onclick="popCalendar('txtDocEndDate');" href="javascript:;"><% Response.Write(sCal)%></A></td>
                <td class="TableCol"></td>
            </tr>
           <tr>
                <td class="TableCol">
                    <asp:Label ID="Label1" CssClass="lbl" runat="server" Text=" Verified Start Date :"></asp:Label></td>
                <td class="TableCol">
                    <input id="txtVerifiedStartDate" runat="server" type="text" class="txtbox" readonly="readonly" style="width:130px;"></input><A onclick="popCalendar('txtVerifiedStartDate');" href="javascript:;"><% Response.Write(sCal)%></A></td>
                <td class="TableCol">
                    <asp:Label ID="Label2" CssClass="lbl" runat="server" Text="Verified End Date :"></asp:Label></td>
                <td class="TableCol">
                    <input id="txtVerifiedEndDate" runat="server" type="text" class="txtbox" readonly="readonly" style="width:130px;"></input><A onclick="popCalendar('txtVerifiedEndDate');" href="javascript:;"><% Response.Write(sCal)%></A></td>
                <td class="TableCol"></td>
            </tr>
    <%--        <tr>
                <td class="TableCol">
                    <asp:Label ID="Label8" CssClass="lbl" runat="server" Text=" Vendor :"></asp:Label></td>
                <td class="TableCol">
                    <input id="txtVendor" runat="server" type="text" class="txtbox" style="width:120px;"/></td>                    
                   <td class="TableCol">
                    <asp:Label ID="Label11" colspan="2" CssClass="lbl" runat="server" Text=" Vendor Address (1st Line):"></asp:Label></td>
                <td class="TableCol">
                    <input id="txtVendorAddr" runat="server" type="text" class="txtbox" style="width:120px;"/></td>                    
                <td class="TableCol"></td>                
           
            </tr> --%>
                    <tr>
                <td class="TableCol">
                    <asp:Label ID="Label8" CssClass="lbl" runat="server" Text=" Vendor :"></asp:Label></td>
                <td class="TableCol">
                    <input id="txtVendor" runat="server" type="text" class="txtbox" style="width:135px;"/></td>                    
                <td class="TableCol"></td>                    
                <td class="TableCol"></td>                    
                <td class="TableCol"></td>
            </tr>
            <tr>
                <td class="TableCol">
                    <asp:Label ID="Label7" CssClass="lbl" runat="server" Text="Status :"></asp:Label>
                </td>
                <td class="TableCol" colspan="3">
                    <%--'Zulham 21112018--%>
                    <asp:CheckBoxList ID="chkdocstatus" CssClass="chklist" runat="server" RepeatDirection="Horizontal"  RepeatColumns="5">
                        <asp:ListItem Value="10">Draft</asp:ListItem>
                        <asp:ListItem Value="16">Submitted</asp:ListItem>
                        <asp:ListItem Value="18">E2P Verified</asp:ListItem>
                        <asp:ListItem Value="17">Department Approved</asp:ListItem>
                        <asp:ListItem Value="11">Finance Verified</asp:ListItem>
                        <asp:ListItem Value="12">Finance Approved</asp:ListItem>
                        <asp:ListItem Value="13">FM Approved</asp:ListItem>
                        <asp:ListItem Value="4">Paid</asp:ListItem>
                        <asp:ListItem Value="15">Void</asp:ListItem>
                        <asp:ListItem Value="14">Rejected</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
                <TD class="tablecol" colspan="2" width="25%" align="right">
                <asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>
                <asp:Button ID="cmdSelectAll" CssClass="button" runat="server" Text="Select All" />   
				    <asp:button cssclass="button" id="cmdClear" text="Clear" runat="server"></asp:button>
				</TD>
                </tr>
						</TABLE>
              <TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<tr>
					<TD class="emptycol" colSpan="3" style="height: 20px"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
							ShowSummary="False"></asp:validationsummary><asp:comparevalidator id="vldDateFtDateTo" runat="server" ErrorMessage="End Date must greater than or equal to Start Date"
							ControlToCompare="txtDocStartDate" ControlToValidate="txtDocEndDate" Display="None" Operator="GreaterThanEqual" Type="Date"></asp:comparevalidator><asp:customvalidator id="vldDateFr" runat="server" ErrorMessage="Date To cannot be empty" ControlToValidate="txtDocStartDate"
							Display="None" ClientValidationFunction="checkDateTo" Enabled="False"></asp:customvalidator><asp:customvalidator id="vldDateTo" runat="server" ErrorMessage="Date From cannot be empty" ControlToValidate="txtDocEndDate"
							Display="None" ClientValidationFunction="checkDateFr" Enabled="False"></asp:customvalidator></TD>
							
				</tr>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="AllTable" id="tblSearchResult" width="100%" cellSpacing="0" cellPadding="0"
							border="0" runat="server">
							<TR>
								<TD class="emptycol"><asp:datagrid id="dtgIPPList" runat="server" OnSortCommand="SortCommand_Click" DataKeyField="IM_INVOICE_INDEX">
										<Columns>
	                                        <asp:BoundColumn SortExpression="IM_INVOICE_NO" DataField="IM_INVOICE_NO" HeaderText="Document No.">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								           <asp:BoundColumn DataField="IM_INVOICE_TYPE" SortExpression="IM_INVOICE_TYPE" HeaderText="Document Type">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IM_DOC_DATE" SortExpression="IM_DOC_DATE" HeaderText="Document Date">
									    <HeaderStyle Width="10%"></HeaderStyle>
								    </asp:BoundColumn>
							    <asp:BoundColumn DataField="IM_S_COY_NAME" SortExpression="IM_S_COY_NAME" HeaderText="Vendor">
                                        <HeaderStyle Width="15%" />
                                </asp:BoundColumn>	
                                     <asp:BoundColumn DataField="IC_BANK_CODE" SortExpression="IC_BANK_CODE" HeaderText="Bank Code">
                            <HeaderStyle Width="5%" />
                        </asp:BoundColumn>			
                        <asp:BoundColumn DataField="IC_BANK_ACCT" SortExpression="IC_BANK_ACCT" HeaderText="Bank Account" ItemStyle-HorizontalAlign = "Right">
                            <HeaderStyle Width="10%" HorizontalAlign = "Right" />
                        </asp:BoundColumn>				
                                <asp:BoundColumn DataField="IM_CURRENCY_CODE" SortExpression="IM_CURRENCY_CODE" HeaderText="Currency">
                                        <HeaderStyle Width="5%" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="IM_INVOICE_TOTAL" SortExpression="IM_INVOICE_TOTAL" HeaderText="Payment Amount" ItemStyle-HorizontalAlign = "Right">
                                        <HeaderStyle Width="10%" HorizontalAlign = "Right" />
                                </asp:BoundColumn>	
                                <asp:BoundColumn DataField="IM_PAYMENT_TERM" SortExpression="IM_PAYMENT_TERM" HeaderText="Payment Method">
                                        <HeaderStyle Width="5%" />                                        
                                </asp:BoundColumn>		
                                      <asp:BoundColumn DataField="IM_PAYMENT_DATE" SortExpression="IM_PAYMENT_DATE" HeaderText="Payment Date">
                            <HeaderStyle Width="10%" />                                        
                        </asp:BoundColumn>	
                         <asp:BoundColumn DataField="IM_PAYMENT_NO" SortExpression="IM_PAYMENT_NO" HeaderText="Payment Advice No.">
                            <HeaderStyle Width="15%" />                                        
                        </asp:BoundColumn>	 
                          <asp:BoundColumn DataField="IM_INVOICE_STATUS" SortExpression="IM_INVOICE_STATUS" HeaderText="Status"  Visible="false">
                                        <HeaderStyle Width="10%" />
                                </asp:BoundColumn>	
                            <asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status">
                                        <HeaderStyle Width="10%" />
                                </asp:BoundColumn>                                  		
										</Columns>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD class="emptycol"></TD>
							</TR>
							<%--<TR>
								<TD><asp:button id="cmdMassApp" runat="server" Width="147px" CssClass="button" Text="Mass Approval"></asp:button>&nbsp;
									<INPUT class="button" id="cmdReset" onclick="DeselectAllG('dtgIPPList_ctl02_chkAll','chkSelection')"
										type="button" value="Reset" name="cmdReset" runat="server" style="DISPLAY: none"></TD>
							</TR>--%>
								<tr>				
				            <td class="emptycol" colSpan="4"><ul class="errormsg" id="vldsum" runat="server">
				                </ul></td>
				            </tr>	
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
