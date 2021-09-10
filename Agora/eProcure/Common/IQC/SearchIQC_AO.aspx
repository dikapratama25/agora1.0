<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchIQC_AO.aspx.vb" Inherits="eProcure.SearchIQC_AO" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>IQC Approval</title>
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
//		$(document).ready(function(){
//            $('#cmdMassApp').click(function() {
//                 if (CheckAtLeastOne('chkSelection') == false)
//                 {		    
//		            return false;
//		          }
//		        else
//                 {  
//                     document.getElementById("cmdMassApp").style.display= "none";
//                     return true;
//                 }
//               
////                    if (Page_IsValid){  
////            //            document.getElementById("cmdMassApp").style.display= "none";
////                        
////                        $("#cmdMassApp").removeAttr("disabled");
////                    }
//            });   
//        });
        		
		function Reset(){
			var oform = document.forms(0);
			var a = document.getElementById('txtIQCNo');
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
			var e = document.getElementById('txtManu');
			if(e){
				e.value = "";
			}
			var f = document.getElementById('txtItemCode');
			if(f){
				f.value = "";
			}
			var g = document.getElementById('cboIQC');
			if(g){
				g.selectedIndex = 0;
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
			{
			arguments.IsValid=false;}
		else
		{
			arguments.IsValid=true;}
		}
				
//		function addCommas(argNum, argThouSeparator, argDecimalPoint)
//		{
//          // default separator values (should resolve to local standard)
//          var sThou = (argThouSeparator) ? argThouSeparator : ","
//          var sDec = (argDecimalPoint) ? argDecimalPoint : "."
// 
//          // split the number into integer & fraction
//          var aParts = argNum.split(sDec)
// 
//          // isolate the integer & add enforced decimal point
//          var sInt = aParts[0] + sDec
// 
//          // tests for four consecutive digits followed by a thousands- or  decimal-separator
//          var rTest = new RegExp("(\\d)(\\d{3}(\\" + sThou + "|\\" + sDec + "))")
//			alert("(\\d)(\\d{3}(\\" + sThou + "|\\" + sDec + "))");
//          while (sInt.match(rTest))
//          {
//                  // insert thousands-separator before the three digits
//                  sInt = sInt.replace(rTest, "$1" + sThou + "$2")
//          }
// 
//          // plug the modified integer back in, removing the temporary 	decimal point
//          aParts[0] = sInt.replace(sDec, "")
//          //alert(aParts.join(sDec));
//          //document.forms[0].txtVendor.value=aParts.join(sDec);
//          //alert(Test(aParts.join(sDec)));
//          //document.forms[0].txtPRNo.value=Test(aParts.join(sDec));
//          return aParts.join(sDec)
//        }

		-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchPRAO_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
				<%--<tr>
					<td class="header"><FONT size="1">&nbsp;</FONT>Purchase Requisition Approval List</td>
				</tr>--%>
				<%--<tr>
					<td class="emptycol"></td>
				</tr>--%>				
				<tr>
					<td class="header" colspan="7" style="height: 3px"></td>
				</tr>
				<tr>
					<td class="linespacing1" colspan="7"></td>
			    </tr>
				<tr>
	                <td colspan="6">
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant PR for approval. Click the PR Number to go to PR approval page."
		                ></asp:label>

	                </td>
                </tr>
                <tr>
					<td class="linespacing2" colspan="4"></td>
			    </tr>
				<tr>
					<td class="emptycol">
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" border="0">
							<tr>
								<td class="tableheader" align="left" colspan="7" style="height: 19px">&nbsp;Search Criteria</td>
							</tr>
							<tr>
								<td class="tablecol" width="17%" >&nbsp;<strong><asp:Label ID="Label11" runat="server" Text="IQC Number :"></asp:Label></strong></td>
								<td class="Tableinput" width="19%">
									<asp:textbox id="txtIQCNo" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="5%"></td>
                                <td class="tablecol" width="15%">
								    <asp:Label id="lblVendor" runat="server" Font-Bold="True">Vendor Name :</asp:Label></td>
								<td class="Tableinput" width="30%" colspan="2">   
									<asp:textbox id="txtVendor" width="100%" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="15%"></td>
							</tr>
							<tr>
								<td class="tablecol" width="17%" >&nbsp;<strong><asp:Label ID="Label2" runat="server" Text="Item Code :"></asp:Label></strong></td>
								<td class="Tableinput" width="19%">
									<asp:textbox id="txtItemCode" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="5%"></td>
                                <td class="tablecol" width="15%">
								    <asp:Label id="Label3" runat="server" Font-Bold="True">Manufacturer Name :</asp:Label></td>
								<td class="Tableinput" width="30%" colspan="2">   
									<asp:textbox id="txtManu" width="100%" runat="server" CssClass="txtbox"></asp:textbox></td>
								<td class="tablecol" width="15%"></td>
							</tr>
							<tr>
								<td class="tablecol" width="17%" >&nbsp;<strong><asp:Label ID="Label5" runat="server" Text="Start Date :"></asp:Label></strong></td>
								<td class="Tableinput" width="19%" style="height: 24px">
									<asp:textbox id="txtDateFr" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateFr');" href="javascript:;"><% Response.Write(sCal)%></a></td>
								<td class="tablecol" width="5%"></td>
                                <td class="tablecol" width="15%">
								    <asp:Label id="Label6" runat="server" Font-Bold="True" Text="End Date :"></asp:Label></td>
								<td class="Tableinput" width="30%" colspan="2" style="height: 24px">   
									<asp:textbox id="txtDateTo" runat="server" Width="80px" CssClass="txtbox" contentEditable="false" ></asp:textbox><a onclick="popCalendar('txtDateTo');" href="javascript:;"><% Response.Write(sCal)%></a></td>
								<td class="tablecol" width="15%"></td>
							</tr>
							<tr>
					            <td class="tablecol" style="height:30px;">&nbsp;<strong><asp:Label ID="Label4" runat="server" Text="IQC Type :"></asp:Label></strong></td>
					            <td class="Tableinput" style="height: 24px"><asp:dropdownlist id="cboIQC" Width="80px" Runat="server" CssClass="ddl"></asp:dropdownlist></td>
					            <td class="tablecol" colspan="5" width="15%" align="right" style="height: 24px"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
									<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"/>&nbsp;</td>
				            </tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="3"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="List" ShowMessageBox="True"
							ShowSummary="False"></asp:validationsummary><asp:comparevalidator id="vldDateFtDateTo" runat="server" ErrorMessage="Date From should be >= Date To"
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
								<td><asp:datagrid id="dtgIQCList" runat="server" OnSortCommand="SortCommand_Click">
										<Columns>							
											<asp:TemplateColumn SortExpression="IVL_IQC_NO" HeaderText="IQC Number">
												<HeaderStyle Width="10%"></HeaderStyle>
												<ItemTemplate>
													<asp:HyperLink Runat="server" ID="lnkIQCNo"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="IM_ITEM_CODE" SortExpression="IM_ITEM_CODE" HeaderText="Item Code">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="IM_INVENTORY_NAME" SortExpression="IM_INVENTORY_NAME" HeaderText="Item Name">
												<HeaderStyle Width="14%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="IV_GRN_NO" SortExpression="IV_GRN_NO" HeaderText="GRN No.">
												<HeaderStyle HorizontalAlign="left" Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="GM_DATE_RECEIVED" SortExpression="GM_DATE_RECEIVED" HeaderText="Submitted Date">
												<HeaderStyle Width="10%"></HeaderStyle>
												<ItemStyle HorizontalAlign="left"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="IVL_LOT_QTY" SortExpression="IVL_LOT_QTY" HeaderText="Lot Qty">
												<HeaderStyle Width="8%" HorizontalAlign="Right"></HeaderStyle>
												<ItemStyle HorizontalAlign="right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="DOL_LOT_NO" SortExpression="DOL_LOT_NO" HeaderText="Lot No">
												<HeaderStyle Width="8%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Vendor Name">
												<HeaderStyle Width="18%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="DOL_DO_MANUFACTURER" SortExpression="DOL_DO_MANUFACTURER" HeaderText="Manufacturer Name">
												<HeaderStyle Width="12%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PM_IQC_TYPE"></asp:BoundColumn>
										</Columns>
									</asp:datagrid></td>
							</tr>
							<tr>
								<td class="emptycol"></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
