<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchPR_AO.aspx.vb" Inherits="eProcure.SearchPR_AOFTN" %>
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
			SelectAllG("dtgPRList_ctl02_chkAll","chkSelection");
		}
		//for filtering check box
		function SelectAll_1()
		{
			checkStatus(true);
		}
		
		function checkStatus(checked)
		{
			var oform = document.forms(0);
			oform.chkOpen.checked=checked;
			oform.chkAwaitAppr.checked=checked;
			oform.chkAppr.checked=checked;
			oform.chkPOCreate.checked=checked;
			oform.chkCancel.checked=checked;
			oform.chkHold.checked=checked;
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtgPRList_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);
			var a = document.getElementById('txtPRNo');
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
			window.open('<% response.write(PopCalendar) %>' ,'cal','status=no,resizable=no,width=180,height=155,left=270,top=180');
			
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
		<%  Response.Write(Session("w_SearchPRAO_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<%--<TR>
					<TD class="header"><FONT size="1">&nbsp;</FONT>Purchase Requisition Approval List</TD>
				</TR>--%>
				<%--<TR>
					<TD class="emptycol"></TD>
				</TR>--%>				
				<TR>
					<TD class="header" colSpan="7" style="height: 3px"></TD>
				</TR>
				<tr>
					<TD class="linespacing1" colSpan="7"></TD>
			    </TR>
				<TR>
	                <TD colSpan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant PR for approval. Click the PR Number to go to PR approval page."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			    </TR>
				<TR>
					<TD class="emptycol">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<td class="tableheader" align="left" colSpan="7" style="height: 19px">&nbsp;Search Criteria</td>
							</TR>
				<TR id="tdAO" style="VISIBILITY: visible" runat="server">
					<TD class="tablecol" style="height:30px;">&nbsp;<STRONG><asp:Label ID="Label1" runat="server" Text="Approve/Endorse By :"></asp:Label></STRONG></TD>
					<td class="TableInput" colspan="6"><asp:dropdownlist id="cboAO" Width="180px" Runat="server" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist></td>
				</TR>
							<TR>
								<TD class="tablecol" width="17%" >&nbsp;<STRONG><asp:Label ID="Label11" runat="server" Text="PR No. :"></asp:Label></STRONG></TD>
								<td class="TableInput" width="19%">
									<asp:textbox id="txtPRNo" runat="server" CssClass="txtbox"></asp:textbox></TD>
								<td class="tablecol" width="5%" colspan="3"></td>
								<%--<TD class="tablecol" width="15%" >
								    <asp:Label id="lblVendor" runat="server" Font-Bold="True">Vendor Name :</asp:Label></TD>--%>
								<%--<td class="TableInput" width="30%" colspan="2">   
									<asp:textbox id="txtVendor" width="100%" runat="server" CssClass="txtbox"></asp:textbox></td>--%>
								<td class="tablecol" width="15%"></td>
							</TR>
							</tr>
								 <tr class="tablecol">
							 <TD class="tablecol" style="width: 90px">
									&nbsp;<asp:Label id="lblPRType" runat="server" Font-Bold="True" Width="80px">PR Type :</asp:Label>
									<td class="tablecol" colspan = "7"><asp:checkbox id="chkContPR" Text="Contract PR" Runat="server" ></asp:checkbox><%--</td>--%>									
									<%--<td class="tablecol" colspan = "6">--%>&nbsp;<asp:checkbox id="chkNonContPR" Text="Non-Contract PR" Runat="server" ></asp:checkbox></td>
							 </tr>

							<tr class="tablecol">
								<td class="tableCOL" align="left" width="15%">&nbsp;<STRONG><asp:Label ID="Label14" runat="server" Text="Start Date :"></asp:Label></STRONG></td>
								<TD	 class="TableInput" width="20%"><asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><A onclick="popCalendar('txtDateFr');" href="javascript:;"><% Response.Write(sCal)%></A></td>
								<TD class="tablecol" width="5%">
								<TD class="tableCOL" align="left" width="15%"><STRONG><asp:Label ID="Label15" runat="server" Text="End Date :"></asp:Label></STRONG></td>
								<TD	 class="TableInput" width="30%"><asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><A onclick="popCalendar('txtDateTo');" href="javascript:;"><% Response.Write(sCal)%></A>
								<TD class="tablecol" colspan="2" width="15%" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
									<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;</TD>
							</tr>
						</TABLE>
					</TD>
				</TR>
				<tr>
					<TD colSpan="3"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
							ShowSummary="False"></asp:validationsummary><asp:comparevalidator id="vldDateFtDateTo" runat="server" ErrorMessage="Date From should be >= Date To"
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
								<TD><asp:datagrid id="dtgPRList" runat="server" OnSortCommand="SortCommand_Click" DataKeyField="PRM_PR_INDEX">
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
											
											<asp:TemplateColumn SortExpression="PRM_PR_No" HeaderText="PR Number">
												<HeaderStyle Width="18%"></HeaderStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="PRM_PR_TYPE" SortExpression="PRM_PR_TYPE" HeaderText="PR Type">
												<HeaderStyle HorizontalAlign="left" Width="12%"></HeaderStyle>
												<ItemStyle HorizontalAlign="left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PRM_SUBMIT_DATE" SortExpression="PRM_SUBMIT_DATE" HeaderText="Submitted Date">
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemStyle HorizontalAlign="left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" HeaderText="Buyer">
												<HeaderStyle Width="20%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Buyer Department">
												<HeaderStyle Width="60%"></HeaderStyle>
											</asp:BoundColumn>
											<%--<asp:BoundColumn DataField="PRM_S_COY_NAME" SortExpression="PRM_S_COY_NAME" HeaderText="Vendor Name">
												<HeaderStyle Width="21%"></HeaderStyle>
											</asp:BoundColumn>--%>
											<%--<asp:BoundColumn DataField="PRM_CURRENCY_CODE" SortExpression="PRM_CURRENCY_CODE" HeaderText="Currency">
												<HeaderStyle Width="6%"></HeaderStyle>
											</asp:BoundColumn>--%>
											<%--<asp:BoundColumn DataField="PR_AMT" SortExpression="PR_AMT" HeaderText="Amount">
												<HeaderStyle Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>--%>
											<%--<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status">
												<HeaderStyle Width="12%"></HeaderStyle>
											</asp:BoundColumn>--%>
											<asp:BoundColumn Visible="False" DataField="PRM_PR_No"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PRM_Buyer_ID"></asp:BoundColumn>
										</Columns>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD class="emptycol"></TD>
							</TR>
							<TR>
								<TD><asp:button id="cmdMassApp" runat="server" Width="147px" CssClass="button" Text="Mass Approval"></asp:button>&nbsp;
									<INPUT class="button" id="cmdReset" onclick="DeselectAllG('dtgPRList_ctl02_chkAll','chkSelection')"
										type="button" value="Reset" name="cmdReset" runat="server" style="DISPLAY: none"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
