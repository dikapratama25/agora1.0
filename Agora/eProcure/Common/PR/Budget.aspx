<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Budget.aspx.vb" Inherits="eProcure.Budget" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Budget Account</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
			/*function Select()
			{	
				if (document.Form1.hidMode.value=='all')
					selectAllItem(document.Form1.hidID.value, document.Form1.hidItem.value);
				else
					selectOne();
			}*/
			
			function Select()
			{	
				if (document.Form1.hidMode.value=='all')
				{
				   selectAllItem(document.Form1.hidID.value,document.Form1.hidItem.value);
					}
				else{
					selectOne();
					}
			}

			/*function selectOne()
			{
				var r =eval("window.opener.document.Form1." + document.Form1.hidID.value);
				var i;
				for (i=0; i<r.length; i++){
					if (r.options[i].value == document.Form1.hidItem.value){
						r.selectedIndex = i;	
					}					
				}
				window.close();
			}
			
			function selectAllItem(val,v)
			{
				var oform = window.opener.document.Form1;
				var j;
				re = new RegExp(':' + val + '$')
				for (var i=0;i<oform.elements.length;i++)
				{
					var e = oform.elements[i];
					if (e.type=="select-one" && re.test(e.name)){
						if (Form1.hidIndex.value == ''){
							for (j=0; j<e.length; j++){
								if (e.options[j].value == v){
									e.selectedIndex = j;
									Form1.hidIndex.value = j;
								}	
							}
						}
						else
							e.selectedIndex = Form1.hidIndex.value;
					}
				}
				window.close();
			}*/
			
			function selectOne()
			{
			
			    //alert(document.Form1.hidBudget.value);
                var r = (eval("window.opener.document.Form1." + document.Form1.hidBudget.value));
                r.value = document.Form1.hidBudgetValue.value;
                var lmtDesc = document.Form1.hidItem.value;
                window.opener.document.getElementById(document.Form1.hidID.value).innerHTML = lmtDesc.substring(0,9);
                window.opener.document.getElementById(document.Form1.hidID.value).title = lmtDesc;
                window.close();                               
                
			}
			
			function selectAllItem(val,v)
			{ 
				var oform = window.opener.document.Form1;
				var j;
				re = new RegExp('$')				 
				for (var i=0;i<oform.elements.length;i++)
				{
				    var foo = "$";
					var e = oform.elements[i];
					var sEvents = e.name;
					 
					if (sEvents.indexOf("$") > 0)
					{					 
						if (sEvents.substring(sEvents.lastIndexOf("$")+1) == "hidBudgetCode")
					    {					     
					        var lmtDesc = document.Form1.hidItem.value;
					        var sBudget = sEvents.substring(0, sEvents.indexOf("$")) + "_" + sEvents.substring(sEvents.indexOf("$")+1 , sEvents.lastIndexOf("$")) + "_txtBudget";
					       
					        window.opener.document.getElementById(sBudget).innerHTML = lmtDesc.substring(0,9);
					        window.opener.document.getElementById(sBudget).title = lmtDesc;  
					        var r = (eval("window.opener.document.Form1." + sEvents));
                            r.value = document.Form1.hidBudgetValue.value;
					    }
					}
				}
				window.close();
			}
			
			function Chk(val,bval)
			{
				document.Form1.hidItem.value=val;
				document.Form1.hidBudgetValue.value=bval;
			}
			
			function Reset(){
				var oform = document.forms(0);					
				oform.txtCode.value="";	
				oform.txtDesc.value="";				
			}
									
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
				<TR>
					<TD colspan="3" class="header" style="WIDTH: 529px"><STRONG><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></STRONG></TD>
				</TR>
				<TR>
					<TD colspan="3" class="emptycol" style="WIDTH: 529px">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="tableheader" style="WIDTH: 529px" colspan="3">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
					<TD class="tablecol" >&nbsp; <STRONG>Budget Account Code</STRONG> &nbsp;:
					</TD>
					<td class="tablecol" align="left">
						<asp:textbox id="txtCode" runat="server" CssClass="txtbox"></asp:textbox>
					</td>
					<td class="tablecol"></td>
				</TR>
				<tr>
					<td class="tablecol" >
						&nbsp;<STRONG> Budget Account Description</STRONG>&nbsp;: &nbsp;
					</td>
					<td class="tablecol">
						<asp:textbox id="txtDesc" CssClass="txtbox" Runat="server"></asp:textbox>
					</td>
					<td class="tablecol" ></td>
				</tr>
				<tr>
				    <td class="tablecol" ></td>
				    <td class="tablecol"></td>
					<td class="tablecol" align="Right">
					<asp:button id="cmd_search" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>
					<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;&nbsp; 
					</td>
				</tr>
				<TR>
					<TD colspan="3" class="emptycol" style="WIDTH: 529px">&nbsp;</TD>
				</TR>
				<TR>
					<TD colspan="3"><asp:datagrid id="dtgBudget" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommandValues_Click"
							AllowSorting="True">
							<AlternatingItemStyle BackColor="#f6f9fe"></AlternatingItemStyle>
							<HeaderStyle CssClass="GridHeader"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn>
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblSelection" Runat="server"></asp:Label>
										<asp:Label id="lblIndex" Runat="server" Visible="False"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Acct_Code" SortExpression="Acct_Code" HeaderText="Budget Account Code">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Acct_List" SortExpression="Acct_List" HeaderText="Budget Account">
									<HeaderStyle Width="70%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right" CssClass="gridPager"
								Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR id="trP" runat="server">
					<TD colspan="3" style="WIDTH: 211px"><INPUT class="button" id="cmdSelect" disabled onclick="Select();" type="button" value="Save"
							name="cmdSelect" runat="server"> <INPUT class="button" id="cmdClose" onclick="window.close();" type="button" value="Close">
						<INPUT id="hidID" style="WIDTH: 35px; HEIGHT: 22px" type="hidden" size="1" name="hidID"
							runat="server">&nbsp; <INPUT id="hidItem" style="WIDTH: 36px; HEIGHT: 22px" type="hidden" size="1" name="hidItem"
							runat="server">&nbsp; <INPUT id="hidMode" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="hidMode"
							runat="server"> <INPUT id="hidIndex" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="hidIndex"
							runat="server"><INPUT id="hidValue" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="hidIndex"
							runat="server"><INPUT id="hidBudget" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="hidBudget"
							runat="server"><INPUT id="hidBudgetValue" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="hidBudgetValue"
							runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="WIDTH: 203px"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
