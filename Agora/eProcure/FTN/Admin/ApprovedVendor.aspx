<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ApprovedVendor.aspx.vb" Inherits="eProcure.ApprovedVendorFTN" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Approved_Vendor</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--		
		
		function selectAll()
		{
			SelectAllG("dtgVDetail_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgVDetail_ctl02_chkAll","chkSelection");
		}
				
		function Reset(){
			var oform = document.forms(0);					
			oform.txtVID.value="";
			oform.txtVName.value="";
		}
		
		function resetForm()
		{		
			//var oform = document.forms(0);				
			Form1.reset();
			//alert(document.getElementById("lblMsg").innerHTML);
			document.getElementById("lblMsg").innerText="";
			//DeselectAllG('dtgVDetail__ctl2_chkAll','chkSelection');
			
			//oform.lblMsg.value="";
						
		}
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD class="header" style="HEIGHT: 16px"><FONT size="1">&nbsp;</FONT><asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
            <tr>
					<TD class="header" colSpan="4" style="height: 7px"></TD>
			</TR>
				<TR>
					<TD align="center">
						<div align="left"><asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text=""></asp:label>
                        </div>
					</TD>
				</TR>
            <tr>
					<TD class="header" colSpan="4" style="height: 7px"></TD>
			</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD class="tableheader" colSpan="2">&nbsp;Search Criteria :</TD>
							</TR>
							<TR class="tablecol">
								<TD noWrap>&nbsp;<STRONG>Vendor ID </STRONG>:
									<asp:textbox id="txtVID" runat="server" CssClass="txtbox" MaxLength="20"></asp:textbox><STRONG>&nbsp;Name
									</STRONG>:<STRONG> </STRONG>
									<asp:textbox id="txtVName" runat="server" CssClass="txtbox" MaxLength="100" Width="121px"></asp:textbox><STRONG>&nbsp;&nbsp;
									</STRONG>
								</TD>
								<TD align="right">
									<asp:button id="cmdSearch" runat="server" CssClass="button" CausesValidation="False" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"><!--<Input Type= "Hidden" Name= "hidVendors" Value= "">-->
								</TD>
							</TR>
							<TR>
								<asp:label id="lblDisplay" runat="server" CssClass="errormsg"></asp:label></TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dtgVDetail" runat="server" AutoGenerateColumns="False" AllowSorting="True" OnSortCommand="SortVendorDetail_Click">
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
								<asp:BoundColumn DataField="Cm_COY_ID" SortExpression="Cm_COY_ID" HeaderText="Vendor ID">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Name">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_BUSINESS_REG_NO" SortExpression="CM_BUSINESS_REG_NO" HeaderText="Business Registration No.">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<%--Jules GST enhancement begin--%>
								<asp:BoundColumn DataField="CM_TAX_REG_NO" SortExpression="CM_TAX_REG_NO" HeaderText="GST Registration No.">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="CV_GST_RATE" HeaderText="GST Rate">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemTemplate>
										<asp:dropdownlist id="cboGSTRate" runat="server" CssClass="ddl" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="cboGSTRate_SelectedIndexChanged"></asp:dropdownlist>
										<asp:requiredfieldvalidator id="GSTRate" runat="server" Display="None" ErrorMessage="Invalid GST Rate Value"
											ControlToValidate="cboGSTRate"></asp:requiredfieldvalidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CV_GST_TAX_CODE" HeaderText="GST Tax Code (Purchase)">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemTemplate>
										<asp:dropdownlist id="cboGSTTaxCode" runat="server" CssClass="ddl" Width="100px"></asp:dropdownlist>
										<asp:requiredfieldvalidator id="GSTTaxCode" runat="server" Display="None" ErrorMessage="Invalid GST Tax Code Value"
											ControlToValidate="cboGSTTaxCode"></asp:requiredfieldvalidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<%--Jules GST enhancement end--%>
								<asp:TemplateColumn SortExpression="CV_PAYMENT_TERM" HeaderText="Payment Terms">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemTemplate>
										<asp:dropdownlist id="cboPayTerm" runat="server" CssClass="ddl" Width="100px"></asp:dropdownlist>
										<asp:requiredfieldvalidator id="PayTerm" runat="server" Display="None" ErrorMessage="Invalid Payment Term Value"
											ControlToValidate="cboPayTerm"></asp:requiredfieldvalidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CV_PAYMENT_METHOD" HeaderText="Payment Method">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemTemplate>
										<asp:dropdownlist id="cboPayMeth" runat="server" CssClass="ddl" Width="100px"></asp:dropdownlist>
										<asp:requiredfieldvalidator id="PayMethod" runat="server" Display="None" ErrorMessage="Invalid Payment Method Value"
											ControlToValidate="cboPayMeth"></asp:requiredfieldvalidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CV_BILLING_METHOD" HeaderText="Invoice Based On">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemTemplate>
										<asp:dropdownlist id="cboInvoiceInd" runat="server" CssClass="ddl" Width="123px">
											<asp:ListItem Value="">---Select---</asp:ListItem>
											<asp:ListItem Value="FPO">FPO</asp:ListItem>
											<asp:ListItem Value="DO">DO</asp:ListItem>
											<asp:ListItem Value="GRN">GRN</asp:ListItem>
										</asp:dropdownlist>
										<asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" Display="None" ControlToValidate="cboInvoiceInd"></asp:requiredfieldvalidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CV_BILLING_METHOD" HeaderText="Est. Date of Delivery (days)">
									<HeaderStyle HorizontalAlign="Left" Width="10px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtEstDate" Width="10" CssClass="numerictxtbox" Runat="server"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Cm_COY_ID" SortExpression="Cm_COY_ID" HeaderText="Vendor ID" Visible="false">
									<HeaderStyle Width="3%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;</TD>
				</TR>
				<TR>
					<TD><asp:button id="cmdAdd" runat="server" CssClass="button" CausesValidation="False" Text="Add"></asp:button>
						<asp:button id="cmdSave" runat="server" CssClass="button" CausesValidation="False" Text="Save"></asp:button>
						<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>
						<INPUT class="button" id="cmdReset" style="DISPLAY: none" onclick="resetForm()" type="button"
							value="Reset" name="cmdReset" runat="server"></TD>
				</TR>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<TR>
					<TD class="emptycol"><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol">&nbsp;&nbsp;&nbsp;
					</TD>
				</TR>
				<TR>
					<TD class="emptycol">
						<div id="back" style="DISPLAY: inline" runat="server"><asp:hyperlink id="lnkBack" Runat="server">
								<STRONG>&lt; Back</STRONG></asp:hyperlink></div>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
