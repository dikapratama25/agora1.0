<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ApprovedVendor.aspx.vb" Inherits="eProcure.ApprovedVendorSEH" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>Approved_Vendor</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
        </script> 
        <%response.write(Session("WheelScript"))%>
		<script type="text/javascript">
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
		
		function ShowDialog(filename,height)
		{
				
			var retval="";
			retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 500px");
			//retval=window.open(filename);
			if (retval == "1" || retval =="" || retval==null)
			{  window.close;
				return false;

			} else {
			    window.close;
				return true;

			}
		}
		-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td class="header" style="HEIGHT: 16px"><font size="1">&nbsp;</font><asp:label id="lblHeader" runat="server"></asp:label></td>
				</tr>
            <tr>
					<td class="header" colspan="4" style="height: 7px"></td>
			</tr>
				<tr>
					<td align="center">
						<div align="left"><asp:label id="lblAction" runat="server"  CssClass="lblInfo"
						Text=""></asp:label>
                        </div>
					</td>
				</tr>
            <tr>
					<td class="header" colspan="4" style="height: 7px"></td>
			</tr>
				<tr>
					<td>
						<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
								<td class="tableheader" colspan="2">&nbsp;Search Criteria :</td>
							</tr>
							<tr class="tablecol">
								<td noWrap>&nbsp;<strong>Vendor ID </strong>:
									<asp:textbox id="txtVID" runat="server" CssClass="txtbox" MaxLength="20"></asp:textbox><strong>&nbsp;Name
									</strong>:<strong> </strong>
									<asp:textbox id="txtVName" runat="server" CssClass="txtbox" MaxLength="100" Width="121px"></asp:textbox><strong>&nbsp;&nbsp;
									</strong>
								</td>
								<td align="right">
									<asp:button id="cmdSearch" runat="server" CssClass="button" CausesValidation="False" Text="Search"></asp:button>&nbsp;<input class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear"><!--<input Type= "Hidden" Name= "hidVendors" Value= "">-->
								</td>
							</tr>
							<tr>
								<asp:label id="lblDisplay" runat="server" CssClass="errormsg"></asp:label></tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtgVDetail" runat="server" AutoGenerateColumns="False" AllowSorting="True" OnSortCommand="SortVendorDetail_Click">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <input onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Cm_COY_ID" HeaderText="Vendor ID">
								    <HeaderStyle Width="8%"></HeaderStyle>
								    <ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkVendId"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Name">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_BUSINESS_REG_NO" SortExpression="CM_BUSINESS_REG_NO" HeaderText="Business Registration No.">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_TAX_REG_NO" SortExpression="CM_TAX_REG_NO" HeaderText="GST Registration No.">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Vendor Code">
								    <HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="CV_GST_RATE" HeaderText="GST Rate">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemTemplate>
										<asp:dropdownlist id="cboGSTRate" runat="server" CssClass="ddl" Width="80px" AutoPostBack="true"></asp:dropdownlist>
										<asp:Label runat="server" ID="lblGSTRate" Visible="false"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CV_GST_TAX_CODE" HeaderText="GST Tax Code (Purchase)">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemTemplate>
										<asp:dropdownlist id="cboGSTTaxCode" runat="server" CssClass="ddl" Width="123px"></asp:dropdownlist>
									</ItemTemplate>
								</asp:TemplateColumn>
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
										<asp:dropdownlist id="cboPayMeth" runat="server" CssClass="ddl" Width="123px"></asp:dropdownlist>
										<asp:requiredfieldvalidator id="PayMethod" runat="server" Display="None" ErrorMessage="Invalid Payment Method Value"
											ControlToValidate="cboPayMeth"></asp:requiredfieldvalidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CV_BILLING_METHOD" HeaderText="Invoice Based On">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemTemplate>
										<asp:dropdownlist id="cboInvoiceInd" runat="server" CssClass="ddl" Width="80px">
											<asp:ListItem Value="">---Select---</asp:ListItem>
											<asp:ListItem Value="FPO">FPO</asp:ListItem>
											<asp:ListItem Value="DO">DO</asp:ListItem>
											<asp:ListItem Value="GRN">GRN</asp:ListItem>
										</asp:dropdownlist>
										<asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" Display="None" ControlToValidate="cboInvoiceInd"></asp:requiredfieldvalidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CV_BILLING_METHOD" HeaderText="Est. Date of Delivery (days)">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtEstDate" Width="80px" CssClass="numerictxtbox" Runat="server"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CV_GRN_CTRL_TERM" HeaderText="GRN Outstanding Control">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemTemplate>
										<asp:dropdownlist id="cboGRN" runat="server" CssClass="ddl" Width="80px"></asp:dropdownlist>
										<asp:requiredfieldvalidator id="GRN" runat="server" Display="None" ErrorMessage="Invalid Value"
											ControlToValidate="cboGRN"></asp:requiredfieldvalidator>
									</ItemTemplate>
								</asp:TemplateColumn>
						</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;</td>
				</tr>
				<tr>
					<td><asp:button id="cmdAdd" runat="server" CssClass="button" CausesValidation="False" Text="Add"></asp:button>
						<asp:button id="cmdSave" runat="server" CssClass="button" CausesValidation="False" Text="Save"></asp:button>
						<asp:button id="cmdDelete" runat="server" CssClass="button" Text="Delete"></asp:button>
						<input class="button" id="cmdReset" style="DISPLAY: none" onclick="resetForm()" type="button"
							value="Reset" name="cmdReset" runat="server"/></td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="emptycol"><asp:label id="lblMsg" runat="server" CssClass="errormsg"></asp:label></td>
				</tr>
				<tr>
					<td class="emptycol">&nbsp;&nbsp;&nbsp;
					</td>
				</tr>
				<tr>
					<td class="emptycol">
						<div id="back" style="DISPLAY: inline" runat="server"><asp:hyperlink id="lnkBack" Runat="server">
								<strong>&lt; Back</strong></asp:hyperlink></div>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
