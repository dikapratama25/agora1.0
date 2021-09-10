<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchCategoryCode.aspx.vb" Inherits="eProcure.WebForm2"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>WebForm2</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<% Response.Write(Session("WheelScript"))%>
		<script>
		<!--
			function selectOne(){
				var targetID =  document.forms(0).targetID.value;
				if( !targetID || targetID.length < 1 ){
					return ;
				} 
				eval("var r = window.opener.document.Form1." + targetID );
				var i;
				if( r ){
					for (i=0; i<r.options.length; i++){
						if (r.options[i].value == document.Form1.tempValue.value){
							r.selectedIndex = i;	
						}					
					}
					window.close();
				}
				
			}
			function Reset(){
				var oform = document.forms(0);					
				oform.txtCode.value="";	
				oform.txtDesc.value="";				
			}
			
			function setHiddenValue( value ){
				var hiddenF = document.getElementById('tempValue');
				if( hiddenF ){
					hiddenF.value = value ;
				}
			}			
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%">
				<TR>
					<TD colspan="2" class="header" style="WIDTH: 529px"><STRONG>Select&nbsp;Category Code</STRONG></TD>
				</TR>
				<TR>
					<TD colspan="2" class="emptycol" style="WIDTH: 529px">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="tableheader" style="WIDTH: 529px" colspan="2">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
					<TD class="tablecol" width="203" style="WIDTH: 203px">&nbsp;&nbsp;<STRONG>Category Code</STRONG>
						&nbsp;:
					</TD>
					<td class="tablecol" align="left">
						<asp:textbox id="txtCode" runat="server" CssClass="txtbox"></asp:textbox>
					</td>
				</TR>
				<tr>
					<td colspan="2" class="tablecol" style="WIDTH: 529px; HEIGHT: 23px"><asp:button id="cmd_search" runat="server" CssClass="button" Text="Search" CausesValidation="False"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">&nbsp;&nbsp;</STRONG>
					</td>
				</tr>
				<TR>
					<TD colspan="2" class="emptycol" style="WIDTH: 529px">&nbsp;</TD>
				</TR>
				<TR>
					<TD colspan="2"><asp:datagrid id="dtgCatCode" runat="server" DataSource='<%# GetData() %>' AutoGenerateColumns="False"
							AllowSorting="True" Width="100%">
							<AlternatingItemStyle BackColor="#f6f9fe"></AlternatingItemStyle>
							<HeaderStyle CssClass="GridHeader"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn>
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<span runat="server" id="container"></span>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CBC_B_CATEGORY_CODE" SortExpression="CBC_B_CATEGORY_CODE" HeaderText=" Category Code">
									<HeaderStyle Width="35%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right" CssClass="gridPager"
								Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR id="trP" runat="server">
					<TD colspan="2" style="WIDTH: 211px"><INPUT class="button" id="cmdSelect" disabled onclick="selectOne();" type="button" value="Save"
							name="cmdSelect" runat="server"> <INPUT class="button" id="cmdClose" onclick="window.close();" type="button" value="Close"><INPUT id="targetID" type="hidden" name="Hidden1" runat="server"><INPUT type="hidden" id="tempValue" name="Hidden1" runat="server"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
