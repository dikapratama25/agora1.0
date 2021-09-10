<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchPO_AO.aspx.vb" Inherits="eProcure.SearchPO_AO" %>
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
		    
		<% Response.Write(Session("WheelScript"))%>
		<% Response.Write(Session("JQuery"))%>
		
		<script language="javascript">
		<!--		
		$(document).ready(function(){
            $('#cmdMassApp').click(function() {
                 if (CheckAtLeastOne('chkSelection') == false)
                 {		    
		            return false;
		          }
		        else
                 {  
                     document.getElementById("cmdMassApp").style.display= "none";
                     return true;
                 }             
            });   
        });
        
		//For check in Datagrid
		function selectAll()
		{
			SelectAllG("dtgPOList_ctl02_chkAll","chkSelection");
		}
		//for filtering check box
		function SelectAll_1()
		{
			checkStatus(true);
		}
		
	
		function checkChild(id)
		{
			checkChildG(id,"dtgPOList_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);
			var a = document.getElementById('txtPONo');
			if(a){
				a.value = "";
			}
			var b = document.getElementById('txtVendor');
			if(b){
				b.value = "";
			}
			var c = document.getElementById('txtDateFr');
			if(c){
				c.value = "";
			}
			var d = document.getElementById('txtDateTo');
			if(d){
				d.value = "";
			}
		}
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
		if (arguments.Value!="" && document.forms(0).txtDateTo.value=="") 
			{//alert("false");
			arguments.IsValid=false;}
		else
		{//alert("true");
			arguments.IsValid=true;}
		}
		
		function checkDateFr(source, arguments)
		{
		if (arguments.Value!="" && document.forms(0).txtDateFr.value=="") 
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
 		<%  Response.Write(Session("w_SearchPOAO_tabs"))%>
              <TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0" width="100%">				
				<TR>
					<TD class="header" colSpan="7" style="height: 3px"></TD>
				</TR>
				<tr>
					<TD class="linespacing1" colSpan="7"></TD>
			    </TR>
				<TR>
	                <TD colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant PO for approval. Click the PO Number to go to PO approval page."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			    </TR>
				<tr>
								<td class="tableheader" align="left" colSpan="7" style="height: 19px">&nbsp;Search Criteria</td>
							</tr>
				<TR id="tdAO" style="VISIBILITY: visible" runat="server">
					<TD class="tablecol" style="height:30px;">&nbsp;<STRONG><asp:Label ID="Label1" runat="server" Text="Approve/Endorse By :"></asp:Label></STRONG></TD>
					<td class="TableInput" colspan="6"><asp:dropdownlist id="cboAO" Width="180px" Runat="server" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist></td>
				</TR>
							<TR>
								<TD class="tablecol" width="17%" >&nbsp;<STRONG><asp:Label ID="Label11" runat="server" Text="PO No. :"></asp:Label></STRONG></TD>
								<td class="TableInput" width="19%">
									<asp:textbox id="txtPONo" runat="server" CssClass="txtbox"></asp:textbox></TD>
								<td class="tablecol" width="5%"></td>
								<TD class="tablecol" width="15%" >
								    <asp:Label id="lblVendor" runat="server" Font-Bold="True">Vendor Name :</asp:Label></TD>
								<td class="TableInput" width="30%" colspan="2">   
									<asp:textbox id="txtVendor" width="100%" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="15%"></td>
								</TR>
								<tr>
								<td class="tableCOL" align="left" width="15%">&nbsp;<STRONG><asp:Label ID="Label14" runat="server" Text="Start Date :"></asp:Label></STRONG></td>
								<TD	 class="TableInput" width="20%"><asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><A onclick="popCalendar('txtDateFr');" href="javascript:;"><% Response.Write(sCal)%></A></td>
								<TD class="tablecol" width="5%">
								<TD class="tableCOL" align="left" width="15%"><STRONG><asp:Label ID="Label15" runat="server" Text="End Date :"></asp:Label></STRONG></td>
								<TD	 class="TableInput" width="30%"><asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><A onclick="popCalendar('txtDateTo');" href="javascript:;"><% Response.Write(sCal)%></A>
								<TD class="tablecol" colspan="2" width="15%" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
									<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;</TD>
							</tr>
						</TABLE>
              <TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%">
				<tr>
					<TD colSpan="3"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
							ShowSummary="False"></asp:validationsummary><asp:comparevalidator id="vldDateFtDateTo" runat="server" ErrorMessage="End Date must greater than or equal to Start Date"
							ControlToCompare="txtDateFr" ControlToValidate="txtDateTo" Display="None" Operator="GreaterThanEqual" Type="Date"></asp:comparevalidator><asp:customvalidator id="vldDateFr" runat="server" ErrorMessage="Date To cannot be empty" ControlToValidate="txtDateFr"
							Display="None" ClientValidationFunction="checkDateTo" Enabled="False"></asp:customvalidator><asp:customvalidator id="vldDateTo" runat="server" ErrorMessage="Date From cannot be empty" ControlToValidate="txtDateTo"
							Display="None" ClientValidationFunction="checkDateFr" Enabled="False"></asp:customvalidator></TD>
				</tr>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="tblSearchResult" width="100%" cellSpacing="0" cellPadding="0"
							border="0" runat="server">
							<TR>
								<TD><asp:datagrid id="dtgPOList" runat="server" OnSortCommand="SortCommand_Click" DataKeyField="POM_PO_INDEX">
										<Columns>
											<asp:TemplateColumn HeaderText="Delete">
												<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<HeaderTemplate>
													<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
												</HeaderTemplate>
												<ItemTemplate>
													<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="POM_PO_No" HeaderText="PO Number">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkPONo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="POM_SUBMIT_DATE" SortExpression="POM_SUBMIT_DATE" HeaderText="Submitted Date">
												<HeaderStyle Width="12%"></HeaderStyle>
												<ItemStyle HorizontalAlign="left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" HeaderText="Buyer">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="POM_S_COY_NAME" SortExpression="POM_S_COY_NAME" HeaderText="Vendor Name">
												<HeaderStyle Width="25%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="POM_CURRENCY_CODE" SortExpression="POM_CURRENCY_CODE" HeaderText="Currency">
												<HeaderStyle Width="8%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PO_AMT" SortExpression="PO_AMT" HeaderText="Amount" HeaderStyle-HorizontalAlign="Right">
												<HeaderStyle Width="9%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="POM_PO_STATUS" SortExpression="POM_PO_STATUS" HeaderText="Status" HeaderStyle-HorizontalAlign="Right">
												<HeaderStyle Width="8%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="POM_PO_No"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="POM_Buyer_ID"></asp:BoundColumn>
											<asp:BoundColumn HeaderText="PR No."> 
									            <HeaderStyle Width="25%" HorizontalAlign="Left"></HeaderStyle> 
                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" HorizontalAlign="Left"/>
								            </asp:BoundColumn>
										</Columns>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD class="emptycol"></TD>
							</TR>
							<TR>
								<TD><asp:button id="cmdMassApp" runat="server" Width="147px" CssClass="button" Text="Mass Approval"></asp:button>&nbsp;
									<INPUT class="button" id="cmdReset" onclick="DeselectAllG('dtgPOList_ctl02_chkAll','chkSelection')"
										type="button" value="Reset" name="cmdReset" runat="server" style="DISPLAY: none"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
