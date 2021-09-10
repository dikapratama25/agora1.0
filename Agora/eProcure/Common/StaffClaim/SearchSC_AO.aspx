<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchSC_AO.aspx.vb" Inherits="eProcure.SearchSC_AO" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>SC Approval</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    dim PopCalendar as string =dDispatcher.direct("Calendar","viewCalendar.aspx","TextBox='+val+'&seldate='+txtVal.value+'")
		    Dim sCal As String = "<IMG src=" & dDispatcher.direct("Plugins/Images", "i_Calendar2.gif") & " border=""0"">"
        </script>
		    
		<% Response.Write(Session("WheelScript"))%>
		<% Response.Write(Session("JQuery"))%>
		
		<script type="text/javascript">
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
			SelectAllG("dtgSCList_ctl02_chkAll","chkSelection");
		}
		//for filtering check box
		function SelectAll_1()
		{
			checkStatus(true);
		}
		
	
		function checkChild(id)
		{
			checkChildG(id,"dtgSCList_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);
			var a = document.getElementById('txtSCNo');
			if(a){
				a.value = "";
			}
			var b = document.getElementById('txtStaffID');
			if(b){
				b.value = "";
			}
			var e = document.getElementById('txtStaffName');
			if(e){
				e.value = "";
			}
			var c = document.getElementById('txtDateFr');
			if(c){
				c.value = "";
			}
			var d = document.getElementById('txtDateTo');
			if(d){
				d.value = "";
			}
			oform.cboDeptName.selectedIndex = 0;
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
	</head>
	<body ms_positioning="GridLayout">
 		<form id="Form1" method="post" runat="server">
 		<%  Response.Write(Session("w_SearchSC_tabs"))%>
              <table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0" width="100%">				
				<tr>
					<td class="header" colspan="7" style="height: 3px"></td>
				</tr>
				<tr>
					<td class="linespacing1" colspan="7"></td>
			    </tr>
				<tr>
	                <td colspan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant SC for approval. Click the Staff Claim Number to go to SC approval page."
		                ></asp:label>

	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colspan="4"></td>
			    </tr>
				<tr>
								<td class="tableheader" align="left" colspan="7" style="height: 19px">&nbsp;Search Criteria</td>
							</tr>
				<tr id="tdAO" style="VISIBILITY: visible" runat="server">
					<td class="tablecol" style="height:30px;">&nbsp;<strong><asp:Label ID="Label1" runat="server" Text="Approve/Endorse By :"></asp:Label></strong></td>
					<td class="TableInput" colspan="6"><asp:dropdownlist id="cboAO" Width="180px" Runat="server" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist></td>
				</tr>
							<tr>
								<td class="tablecol" width="17%" >&nbsp;<strong><asp:Label ID="Label11" runat="server" Text="Staff Claim No. :"></asp:Label></strong></td>
								<td class="TableInput" width="19%">
									<asp:textbox id="txtSCNo" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="5%"></td>
								<td class="tablecol" width="15%" style="height: 24px"><strong><asp:Label ID="Label4" runat="server" Text="Department :"></asp:Label></strong></td>
								<td class="TableInput" width="30%" style="height: 24px" colspan="2"><asp:dropdownlist id="cboDeptName" Width="250px" CssClass="ddl" Runat="server"></asp:dropdownlist></td>
								<td class="tablecol" width="15%"></td>
								</tr>
								<tr>
								<td class="tablecol" width="17%" >&nbsp;<strong><asp:Label ID="Label2" runat="server" Text="Staff ID. :"></asp:Label></strong></td>
								<td class="TableInput" width="19%">
									<asp:textbox id="txtStaffID" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="5%"></td>
								<td class="tablecol" width="15%" ><strong><asp:Label ID="Label3" runat="server" Text="Staff Name :"></asp:Label></strong></td>
								<td class="TableInput" width="30%" colspan="2"><asp:textbox id="txtStaffName" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="15%"></td>
								</tr>
								<tr>
								<td class="tableCOL" align="left" width="15%">&nbsp;<strong><asp:Label ID="Label14" runat="server" Text="Start Date :"></asp:Label></strong></td>
								<td	 class="TableInput" width="20%"><asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateFr');" href="javascript:;"><% Response.Write(sCal)%></a></td>
								<td class="tablecol" width="5%"></td>
								<td class="tableCOL" align="left" width="15%"><strong><asp:Label ID="Label15" runat="server" Text="End Date :"></asp:Label></strong></td>
								<td	 class="TableInput" width="30%"><asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateTo');" href="javascript:;"><% Response.Write(sCal)%></a></td>
								<td class="tablecol" colspan="2" width="15%" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
									<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/>&nbsp;</td>
							</tr>
						</table>
              <table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" width="100%">
				<tr>
					<td colspan="3"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
							ShowSummary="False"></asp:validationsummary><asp:comparevalidator id="vldDateFtDateTo" runat="server" ErrorMessage="End Date must greater than or equal to Start Date"
							ControlToCompare="txtDateFr" ControlToValidate="txtDateTo" Display="None" Operator="GreaterThanEqual" Type="Date"></asp:comparevalidator><asp:customvalidator id="vldDateFr" runat="server" ErrorMessage="Date To cannot be empty" ControlToValidate="txtDateFr"
							Display="None" ClientValidationFunction="checkDateTo" Enabled="False"></asp:customvalidator><asp:customvalidator id="vldDateTo" runat="server" ErrorMessage="Date From cannot be empty" ControlToValidate="txtDateTo"
							Display="None" ClientValidationFunction="checkDateFr" Enabled="False"></asp:customvalidator></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td>
						<table class="AllTable" id="tblSearchResult" width="100%" cellspacing="0" cellpadding="0"
							border="0" runat="server">
							<tr>
								<td><asp:datagrid id="dtgSCList" runat="server" OnSortCommand="SortCommand_Click">
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
											<asp:BoundColumn DataField="STAFF_NAME" SortExpression="STAFF_NAME" HeaderText="Staff Name">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CDM_DEPT_NAME" SortExpression="CDM_DEPT_NAME" HeaderText="Department">
												<HeaderStyle Width="15%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn SortExpression="SCM_CLAIM_DOC_NO" HeaderText="Staff Claim No">
												<HeaderStyle Width="10%"></HeaderStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkScNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="SCM_CREATED_DATE" SortExpression="SCM_CREATED_DATE" HeaderText="Document Date">
												<HeaderStyle Width="12%"></HeaderStyle>
												<ItemStyle HorizontalAlign="left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="SCM_SUBMIT_DATE" SortExpression="SCM_SUBMIT_DATE" HeaderText="Submitted Date">
												<HeaderStyle Width="12%"></HeaderStyle>
												<ItemStyle HorizontalAlign="left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="ENT_AMT" SortExpression="ENT_AMT" HeaderText="Entertainment Claims" HeaderStyle-HorizontalAlign="Right">
												<HeaderStyle Width="9%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="L_MILEAGE_AMT" SortExpression="L_MILEAGE_AMT" HeaderText="Local Mileage Claims" HeaderStyle-HorizontalAlign="Right">
												<HeaderStyle Width="9%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CLAIM_AMT" SortExpression="CLAIM_AMT" HeaderText="Total Claimed Amount" HeaderStyle-HorizontalAlign="Right">
												<HeaderStyle Width="9%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status" HeaderStyle-HorizontalAlign="Right">
												<HeaderStyle Width="8%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="SCM_CLAIM_DOC_No"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="SCM_STAFF_ID"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="SCM_CLAIM_INDEX"></asp:BoundColumn>
										</Columns>
									</asp:datagrid></td>
							</tr>
							<tr>
								<td class="emptycol"></td>
							</tr>
							<tr>
								<td><asp:button id="cmdMassApp" runat="server" Width="147px" CssClass="button" Text="Mass Approval"></asp:button>&nbsp;
									<input class="button" id="cmdReset" onclick="DeselectAllG('dtgSCList_ctl02_chkAll','chkSelection')"
										type="button" value="Reset" name="cmdReset" runat="server" style="DISPLAY: none"/></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
