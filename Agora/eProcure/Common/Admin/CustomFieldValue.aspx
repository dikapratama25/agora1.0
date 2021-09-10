<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CustomFieldValue.aspx.vb" Inherits="eProcure.CustomFieldValue" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CustomFieldValue</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
			function selectAll()
			{
				SelectAllG("dtgCustomField_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCustomField_ctl02_chkAll","chkSelection");
			}
		
		    /*function Select()
			{	
				if (document.Form1.hidMode.value=='all')
				{
					selectAllItem(document.Form1.hidID.value,document.Form1.hidItem.value);
					}
				else{
					selectOne();
					}
			}
			
			function selectOne()
			{
                var r = (eval("window.opener.document.Form1." + document.Form1.hidID.value));
                r.value = document.Form1.hidItem.value;
                var lmtAddrDesc = document.Form1.hidAddrDesc.value;
                window.opener.document.getElementById(document.Form1.hidAddrId.value).innerHTML = lmtAddrDesc.substring(0,9);
                window.close();
			}
			
			function selectAllItem(val,v)
			{ 
				var oform = window.opener.document.Form1;
				var j;
				re = new RegExp('$' ) 
				for (var i=0;i<oform.elements.length;i++)
				{
				    var foo = "$";
					var e = oform.elements[i];
					var sEvents = e.name;
					if (sEvents.indexOf("$") > 0)
					{
						if (sEvents.substring(sEvents.lastIndexOf("$")+1) == "hidDelCode")
					    {
					        var lmtAddrDesc = document.Form1.hidAddrDesc.value;
					        var sDelivery = sEvents.substring(0, sEvents.indexOf("$")) + "_" + sEvents.substring(sEvents.indexOf("$")+1 , sEvents.lastIndexOf("$")) + "_txtDelivery";
					                       
					        window.opener.document.getElementById(sDelivery).innerHTML = lmtAddrDesc.substring(0,9);
					        
					        var r = (eval("window.opener.document.Form1." + sEvents));
                            r.value = document.Form1.hidItem.value;
					    }
					}					
				}
				window.close();
			}*/
			
			function Select()
			{
				var oform = window.opener.document.Form1;
				var j, v, val;
				val = document.Form2.hidID.value;
				v = document.Form2.hidItem.value;
				// re = new RegExp(':' + val + '$');
				re = new RegExp('' + val + '$');
				for (var i=0;i<oform.elements.length;i++)				
				{
					var e = oform.elements[i];
					
					if (e.type=="select-one" && re.test(e.name)){
						if (Form2.hidIndex.value == ''){
							for (j=0; j<e.length; j++){
								if (e.options[j].value == v){
									e.selectedIndex = j;
									Form2.hidIndex.value = j;
								}	
							}
						}
						else
							e.selectedIndex = Form2.hidIndex.value;
					}
				}
				window.close();
			}

			function Chk(val)
			{
				document.Form2.hidItem.value = val;
			}
									
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form2" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="WIDTH: 529px"><STRONG><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></STRONG></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="WIDTH: 529px">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="WIDTH: 529px; color:Black">&nbsp;<STRONG>Field Name </STRONG>:&nbsp;<asp:label id="lblFieldName" Runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="WIDTH: 529px"></TD>
				</TR>
				<TR>
					<TD class="tableheader" style="WIDTH: 529px">&nbsp;Search Criteria</TD>
				</TR>
				<TR>
					<TD class="tablecol" style="WIDTH: 529px">&nbsp;<STRONG>Field Value </STRONG>:<STRONG> </STRONG>
						<asp:textbox id="txtsearch" runat="server" CssClass="txtbox"></asp:textbox>&nbsp;<asp:button id="cmd_search" runat="server" CssClass="button" CausesValidation="False" Text="Search"></asp:button>&nbsp;<asp:button id="cmd_clear1" runat="server" CssClass="button" CausesValidation="False" Text="Clear"></asp:button>&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="WIDTH: 529px"></TD>
				</TR>
				<TR>
					<TD>
						<div id="custom" style="DISPLAY: none" runat="server">
							<table class="alltable" cellSpacing="0" cellPadding="0"	border="0" width="100%">
								<TR>
									<TD class="tableheader" style="WIDTH: 623px">&nbsp;<asp:label id="lbl_add_mod" Runat="server"></asp:label>
									</TD>
								</TR>
								<tr>
									<td class="tablecol" style="WIDTH: 623px">&nbsp;<STRONG>Field Value</STRONG><asp:label id="lblName" runat="server" CssClass="errormsg">*</asp:label><STRONG>&nbsp;</STRONG>:&nbsp;<asp:textbox id="TxtValue" runat="server" CssClass="txtbox" MaxLength="100"></asp:textbox>&nbsp;<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmdClear" runat="server" CssClass="button" CausesValidation="False" Text="Clear"></asp:button>&nbsp;<asp:button id="cmdCancel" runat="server" CssClass="button" CausesValidation="False" Text="Cancel"></asp:button></td>
								</tr>
								<TR class="emptycol">
									<TD><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>indicates 
										required field
									</TD>
								</TR>
								<TR class="emptycol">
									<TD><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary><asp:requiredfieldvalidator id="vldValue" runat="server" Display="None" ErrorMessage="Field Value is required."
											ControlToValidate="txtValue"></asp:requiredfieldvalidator></TD>
								</TR>
							</table>
						</div>
					</TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgCustomField" runat="server" CssClass="grid" DataKeyField="CF_FIELD_INDEX"
							OnPageIndexChanged="dtgCustomField_Page" AllowSorting="True" OnSortCommand="SortCommandValues_Click"
							AutoGenerateColumns="False">
							<AlternatingItemStyle BackColor="#f6f9fe"></AlternatingItemStyle>
							<HeaderStyle CssClass="GridHeader"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										<asp:Label id="lblSelection" Runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CF_Field_Value" SortExpression="CF_Field_Value" HeaderText="Field Value">
									<HeaderStyle Width="94%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right" CssClass="gridPager"
								Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="WIDTH: 529px"></TD>
				</TR>
				<TR id="trT" runat="server">
					<TD style="WIDTH: 529px"><asp:button id="cmd_Add" runat="server" CssClass="Button" CausesValidation="False" Text="Add"></asp:button>&nbsp;<asp:button id="cmd_Modify" runat="server" CssClass="Button" CausesValidation="False" Text="Modify"
							Enabled="False"></asp:button>&nbsp;<asp:button id="cmd_Delete" runat="server" CssClass="Button" CausesValidation="False" Text="Delete"
							Enabled="False"></asp:button>
						<INPUT class="button" id="cmdReset" onclick="DeselectAllG('dtgCustomField_ctl02_chkAll','chkSelection')"
							type="button" value="Reset" name="cmdReset" runat="server" style="DISPLAY:none">
					</TD>
				</TR>
				<TR id="trP" runat="server">
					<TD style="WIDTH: 529px"><INPUT class="button" id="cmdSelect" disabled onclick="Select();" type="button" value="Save"
							name="cmdSelect" runat="server"> <INPUT class="button" id="cmdClose" onclick="window.close();" type="button" value="Close">
						<INPUT id="hidID" style="WIDTH: 35px; HEIGHT: 22px" type="hidden" size="1" name="hidID"
							runat="server">&nbsp; <INPUT id="hidItem" style="WIDTH: 36px; HEIGHT: 22px" type="hidden" size="1" name="hidItem"
							runat="server">&nbsp; <INPUT id="hidMode" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="hidMode"
							runat="server"> <INPUT id="hidIndex" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="hidIndex"
							runat="server"><INPUT id="hidValue" style="WIDTH: 40px; HEIGHT: 22px" type="hidden" size="1" name="hidIndex"
							runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="WIDTH: 529px"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
