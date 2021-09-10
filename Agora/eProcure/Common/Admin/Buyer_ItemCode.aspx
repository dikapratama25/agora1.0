<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Buyer_ItemCode.aspx.vb" Inherits="eProcure.Buyer_ItemCode" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Buyer_ItemCode</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
			//function check(){
			//var change = document.getElementById("onchange");
			//change.value ="1";
			//}
			
			function CheckAll(val1, val2)
			{
				var v1 = eval("Form2." + val1 + ".value");
				var v2 = eval("Form2." + val2 + ".value");
				
				//alert(v1 + ',' + v2);
				if (v1 != v2)
					Form2.hidChange.value=1;				
			}

/*function check(){
			var change = document.getElementById("onchange");
			change.value ="1";
			}
*/		
			
		-->
		</script>
		<script language="vbscript">
			
			sub Change()
			
				'msgbox(Form2.ddl_Select.selectedIndex)
				'v = msgbox("aa",4)
				'msgbox(Form2.hidChange.value)
				dim msg
				if Form2.hidChange.value=1 then
						msg = msgbox("Do you want to update your changes?",4)
						'//yes=6, no=7
			'msgbox(msg, 4,"change") 
					if msg=vbNo then
						Form2.hidChange.value=0
						Form2.submit()
					
					else					
						Form2.ddl_Select.selectedIndex=Form2.hidItem.value
						
					end if
									
			else
					Form2.submit()
			end if
			
			
				
			end sub
		
					
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form2" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TBODY>
					<TR>
						<TD class="header" style="WIDTH: 580px"><FONT size="1">&nbsp;</FONT><STRONG>Buyer Item 
								Code Assignment </STRONG>
						</TD>
					</TR>
					<TR class="emptycol">
						<TD></TD>
					</TR>
					<TR>
						<TD>
							<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" border="0" width="100%">
								<TBODY>
									<TR>
										<TD class="TableHeader" colSpan="2">&nbsp;Search Criteria</TD>
									</TR>
									<TR>
										<TD class="tablecol" style="HEIGHT: 18px"><STRONG>&nbsp;Vendor</STRONG><asp:label id="lblName" runat="server" CssClass="errormsg">*</asp:label>&nbsp;:
											<asp:dropdownlist id="ddl_Select" runat="server" CssClass="txtbox" AutoPostBack="false" Width="350px"></asp:dropdownlist>&nbsp;<INPUT id="hidChange" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" runat="server">
											<INPUT id="hidItem" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="Hidden1"
												runat="server"><asp:requiredfieldvalidator id="vldName2" runat="server" ErrorMessage="Vendor is required." ControlToValidate="ddl_Select"></asp:requiredfieldvalidator></TD>
									<TR>
										<TD class="tablecol"><STRONG>&nbsp;Item ID </STRONG>:&nbsp;&nbsp;<asp:textbox id="txtID" runat="server" CssClass="txtbox" Width="184px" MaxLength="20"></asp:textbox>&nbsp;&nbsp;&nbsp;
											<STRONG>Description </STRONG>:<STRONG>&nbsp;</STRONG><asp:textbox id="txtDesc" runat="server" CssClass="txtbox" Width="192px" MaxLength="250"></asp:textbox>&nbsp;<asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<asp:button id="cmdClear" runat="server" CssClass="button" Text="Clear" CausesValidation="False"></asp:button>&nbsp;</TD>
									</TR>
									<TR class="emptycol">
										<TD style="WIDTH: 463px; HEIGHT: 7px"><asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label>indicates 
											required field</TD>
									</TR>
									<TR class="emptycol">
										<TD></TD>
									</TR>
									<TR>
										<TD><asp:datagrid id="dtg_BuyerCode" runat="server" AutoGenerateColumns="False" OnPageIndexChanged="dtg_BuyerCode_Page"
												OnSortCommand="SortCommand_Click" DataKeyField="PM_PRODUCT_CODE">
												<Columns>
													<asp:BoundColumn DataField="PM_PRODUCT_CODE" SortExpression="PM_PRODUCT_CODE"  readonly="true"    HeaderText="Item ID">
														<HeaderStyle Width="7%"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="PM_PRODUCT_DESC" SortExpression="PM_PRODUCT_DESC" HeaderText="Description">
														<HeaderStyle Width="23%"></HeaderStyle>
													</asp:BoundColumn>
													<asp:TemplateColumn SortExpression="CBC_B_ITEM_CODE" HeaderText="Buyer Item Code">
														<HeaderStyle Width="20%"></HeaderStyle>
														<ItemTemplate>
															<asp:TextBox id="txt_code" runat="server" CssClass="txtbox" MaxLength="100" Width="100"></asp:TextBox>
															<input type="hidden" id="hidCode" runat="server">
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn SortExpression="CBC_B_CATEGORY_CODE" HeaderText="Buyer Category Code">
														<HeaderStyle Width="20%"></HeaderStyle>
														<ItemTemplate>
															<asp:TextBox id="txtCategory" runat="server" CssClass="txtbox" MaxLength="100" Width="100"></asp:TextBox>
															<input type="hidden" id="hidCategory" runat="server" NAME="hidCategory">
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn SortExpression="CBC_B_GL_CODE" HeaderText="Buyer GL Code">
														<HeaderStyle Width="20%"></HeaderStyle>
														<ItemTemplate>
															<asp:TextBox id="txtGL" runat="server" CssClass="txtbox" Width="100" MaxLength="100"></asp:TextBox><INPUT id="hidGL" type="hidden" name="hidGL" runat="server">
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn SortExpression="CBC_B_TAX_CODE" HeaderText="Buyer Tax Code">
														<HeaderStyle Width="10%"></HeaderStyle>
														<ItemTemplate>
															<asp:TextBox id="txtTax" runat="server" CssClass="txtbox" Width="50px" MaxLength="25"></asp:TextBox><INPUT id="hidTax" type="hidden" name="hidTax" runat="server">
														</ItemTemplate>
													</asp:TemplateColumn>
												</Columns>
											</asp:datagrid></TD>
									</TR>
									<TR>
										<TD>&nbsp;</TD>
									</TR>
									<TR>
										<TD><asp:button id="cmd_Save" runat="server" CssClass="Button" Text="Save" CausesValidation="False"></asp:button>&nbsp;<INPUT class="button" id="cmd_Reset2" type="reset" value="Reset" name="Reset1" runat="server"></TD>
									</TR>
								</TBODY>
							</TABLE>
						</TD>
					</TR>
				</TBODY>
			</TABLE>
		</form>
	</body>
</HTML>
