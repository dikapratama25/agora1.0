<%@ Page Language="vb" AutoEventWireup="false" Codebehind="viewShoppingCart.aspx.vb" Inherits="eProcure.viewShoppingCart" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>View Shopping Cart</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		
			function checkAtLeastOneResetSummary(p1, p2, cnt1, cnt2)
			{
				if (CheckAtLeastOne(p1,p2)== true) {
					if (resetSummary(cnt1,cnt2)==true)
						return true;
					else
						return false;
				}
				else {
					return false;
				}				
			}

			function selectAll()
			{
				SelectAllG("dtgShopping_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgShopping_ctl02_chkAll","chkSelection");
			}

			function removeQuot(str)
			{
				var r = new RegExp(",","gi");
				var newstr = str.replace(r, '') ;
				return newstr;
			}
			
			function formatDec(amt)
			{
				var len = amt.length;
				switch (parseInt(len)) {
					case 1:
						return "000" + amt;
						
					case 2:
						return "00" + amt;
						
					case 3:
						return "0" + amt;
						
					case 4:
						return amt;						
				}					
			}
			
			function formatDecimal(amt)
			{
				var len, posDot; 
				var num, numDec;
				posDot = amt.indexOf('.');
				
				if (posDot > 0) {
					num = amt.substring(0, posDot);
					numDec = amt.substring(posDot + 1, amt.length);
					len = numDec.length;
					
					if (len > 4)
						len = 5;
					switch (parseInt(len)) {
						case 1:
							return num + "." + numDec + "000";
							
						case 2:
							return num + "." + numDec + "00";
							
						case 3:
							return num + "." + numDec + "0";
							
						case 4:
							return num + "." + numDec;
							
						case 5:
							if (parseInt(numDec.substr(4,1))>4){
								numDec = parseInt(numDec.substr(0,4), 10) + 1;	
								if (numDec < 1000) 
									numDec = formatDec('' + numDec);
							}
							else{
								numDec = numDec.substr(0,4);								
							}
							return num + "." + numDec;				
					}					
				
				}
				else {
					return amt + '.' + '0000';
				}					
			}
			
			function addCommas(argNum, argThouSeparator, argDecimalPoint)
			{
				var sThou = (argThouSeparator) ? argThouSeparator : ","
				var sDec = (argDecimalPoint) ? argDecimalPoint : "."
		 
				// split the number into integer & fraction
				var aParts = argNum.split(sDec)
		 
				// isolate the integer & add enforced decimal point
				var sInt = aParts[0] + sDec
		 
				// tests for four consecutive digits followed by a thousands- or  decimal-separator
				var rTest = new RegExp("(\\d)(\\d{3}(\\" + sThou + "|\\" + sDec + "))")
		 
				while (sInt.match(rTest))
				{
					// insert thousands-separator before the three digits
					sInt = sInt.replace(rTest, "$1" + sThou + "$2")
				}
	 
				// plug the modified integer back in, removing the temporary 	decimal point
				aParts[0] = sInt.replace(sDec, "");
				return formatDecimal(aParts.join(sDec));
			}
			
			function calculateTotal(qty, price, amt)
			{
				// ctlAmount - textbox in datagrid for amount
				//resetValue(qty, 1); 
				var ctlAmount, i;
				var Quantity = removeQuot(eval("Form1." + qty + ".value"));
				//var UnitPrice = removeQuot(eval("Form1." + price + ".value"));
				var Amount = removeQuot(eval("Form1." + amt + ".value"));				

				i = Quantity.indexOf(' ');
				if (!isNaN(Quantity) && (i == -1)){
					ctlAmount = document.getElementById(amt);
					ctlAmount.value = Quantity * price;	
					ctlAmount.value = addCommas(ctlAmount.value);
				}				
			}

		-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header"><FONT size="1"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="TableHeader" colSpan="2">&nbsp;Search Criteria</TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<strong>Vendor Name<asp:label id="Label4" runat="server" CssClass="errormsg">*</asp:label></strong>&nbsp;:&nbsp;
									<asp:dropdownlist id="cboSupplier" runat="server" CssClass="ddl" AutoPostBack="True" Width="300px"></asp:dropdownlist>&nbsp;&nbsp;
								</TD>
							</TR>
							<TR>
								<TD colSpan="2"><asp:label id="Label7" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
									required field
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgShopping" runat="server" DataKeyField="SC_CART_INDEX" OnSortCommand="SortCommand_Click"
							OnPageIndexChanged="dtgShopping_Page" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
										<!--<asp:Label ID="lblSupplierCode" Runat="server" Visible="False"></asp:Label>
										<asp:Label ID="lblProductCode" Runat="server" Visible="False"></asp:Label>-->
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CBC_B_ITEM_CODE" SortExpression="CBC_B_ITEM_CODE"  readonly="true"    HeaderText="Buyer Item Code">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SC_VENDOR_ITEM_CODE" SortExpression="SC_VENDOR_ITEM_CODE"  readonly="true"  
									HeaderText="Vendor Item Code">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="CM_COY_NAME" HeaderText="Vendor Name">
									<HeaderStyle HorizontalAlign="Left" Width="21%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCoyName"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="SC_PRODUCT_DESC" SortExpression="SC_PRODUCT_DESC"  readonly="true"   HeaderText="Item Description">
									<HeaderStyle Width="18%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SC_UOM" SortExpression="SC_UOM"  readonly="true"   HeaderText="UOM">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="SC_QUANTITY" HeaderText="Quantity">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtQty" CssClass="numerictxtbox" Width="50px" Runat="server"></asp:TextBox>
										<asp:RegularExpressionValidator id="revQty" ValidationExpression="^\d+$" ControlToValidate="txtQty" Runat="server"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="SC_CURRENCY_CODE" SortExpression="SC_CURRENCY_CODE"  readonly="true"   HeaderText="Currency">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="SC_UNIT_COST" HeaderText="Item Price">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtPrice" CssClass="numerictxtbox" Width="80px" Runat="server" Rows="1"></asp:TextBox>
										<asp:RegularExpressionValidator id="revPrice" ValidationExpression="^\d+$" ControlToValidate="txtPrice" Runat="server"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="TOTAL" HeaderText="Total">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtAmount" Width="80px" CssClass="lblnumerictxtbox" Runat="server"  contentEditable="false" ></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="SC_REMARK" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtRemark" CssClass="listtxtbox" Runat="server" Rows="2" TextMode="MultiLine"></asp:TextBox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red"
											 contentEditable="false" ></asp:TextBox>
										<INPUT class="txtbox" id="hidCode" type="hidden" runat="server" NAME="hidCode">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="SC_S_COY_ID" SortExpression="SC_S_COY_ID" Visible="False" HeaderText="SC_S_COY_ID">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SC_PRODUCT_CODE" SortExpression="SC_PRODUCT_CODE" Visible="False" HeaderText="SC_PRODUCT_CODE">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 18px">&nbsp;&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="AllTable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="TableHeader" colSpan="2">&nbsp;Add Item to PR</TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;Add Item to PR&nbsp;:&nbsp;
									<asp:dropdownlist id="cboPR" runat="server" CssClass="ddl" Width="120px"></asp:dropdownlist>&nbsp;
									<asp:button id="cmdAddPR" runat="server" CssClass="Button" Text="Add To PR" Enabled="False"></asp:button>&nbsp;&nbsp;
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 18px"><INPUT id="hidIndex" style="WIDTH: 36px; HEIGHT: 22px" type="hidden" size="1" name="Hidden1"
							runat="server"></TD>
				</TR>
				<TR>
					<TD><asp:button id="cmd_Update" runat="server" CssClass="Button" Text="Save" Enabled="False"></asp:button>&nbsp;<asp:button id="cmd_Raise" runat="server" CssClass="Button" Text="Raise PR"></asp:button>&nbsp;<asp:button id="cmd_Delete" runat="server" CssClass="Button" Text="Delete" Enabled="False"></asp:button>&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"><INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="emptycol"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
