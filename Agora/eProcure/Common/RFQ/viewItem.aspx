<%@ Page Language="vb" AutoEventWireup="false" Codebehind="viewItem.aspx.vb" Inherits="eProcure.viewItem1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>viewItem</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!--#include file = "../include/WheelScript.js"-->
		<script language="javascript">
		<!--

			function selectAll()
			{
				SelectAllG("dg_viewitem_ctl01_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dg_viewitem_ctl01_chkAll","chkSelection");
			}
			function check(){
			var change = document.getElementById("onchange");
			change.value ="1";
			}
		-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form onkeypress="check();" id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header">View RFQ Items</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 12px">
						<DIV class="div" style="DISPLAY: inline; WIDTH: 428px; HEIGHT: 12px" ms_positioning="FlowLayout">You 
							may remove items from your RFQ or go back to your previous screen.
						</DIV>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="tableheader" colSpan="2">&nbsp;RFQ Info</TD>
							</TR>
							<TR>
								<TD class="tablecol">&nbsp;<STRONG>RFQ Number </STRONG>:
									<asp:label id="lbl_rfq_number" runat="server"></asp:label></TD>
								<TD></TD>
							</TR>
							<TR class="tablecol">
								<TD>&nbsp;<!--<Input Type= "Hidden" Name= "hidVendors" Value= "">--><STRONG>RFQ Name<asp:label id="Label1" runat="server" CssClass="errormsg">*</asp:label></STRONG>&nbsp;:<STRONG>&nbsp;</STRONG>
									<asp:textbox id="txt_rfq_name" runat="server" CssClass="txtbox" MaxLength="100"></asp:textbox><STRONG>&nbsp;&nbsp; 
										Reference Currency </STRONG>:<STRONG>&nbsp;&nbsp; </STRONG>
									<asp:dropdownlist id="ddl_cur" runat="server" CssClass="ddl"></asp:dropdownlist><INPUT id="onchange" type="hidden" runat="server"></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD class="emptycol"></TD>
							</TR>
						</TABLE>
						<asp:label id="Label3" runat="server" CssClass="errormsg">*</asp:label>&nbsp;indicates 
						required field
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 16px"></TD>
				</TR>
				<TR>
					<TD><asp:datagrid id="dg_viewitem" runat="server" CssClass="grid" AutoGenerateColumns="False">
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
								<asp:TemplateColumn SortExpression="RD_Product_Desc" HeaderText="Item Description *">
									<HeaderStyle Width="50%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_desc" runat="server" CssClass="txtbox" Width="367px" TextMode="MultiLine"
											Height="40px" MaxLength="250" Rows="3"></asp:TextBox>
										<asp:Label id="lbl_limit" runat="server"></asp:Label>
										<asp:RequiredFieldValidator id="val_ItemDesc" runat="server" Display="none" ErrorMessage="Item Description is required."
											ControlToValidate="txt_desc"></asp:RequiredFieldValidator>
										<asp:Label id="lbl_desc" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="UOM ">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemTemplate>
										<asp:DropDownList id="ddl_uom" runat="server" CssClass="ddl"></asp:DropDownList>
										<asp:Label id="lbl_uom" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="QTY *">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_qty" CssClass="numerictxtbox" Width="55px" Runat="server" Rows="2" MaxLength="5"></asp:TextBox>
										<asp:RegularExpressionValidator id="val_Qty" Display="none" ErrorMessage="Invalid quantity." ControlToValidate="txt_qty"
											Runat="server"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Delivery Lead Time (days)+ ">
									<HeaderStyle Width="19%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_delivery" runat="server" CssClass="numerictxtbox" Width="55px" MaxLength="3"></asp:TextBox>
										<asp:RegularExpressionValidator id="val_delivery" Display="none" ControlToValidate="ddl_cur" Runat="server"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Warranty Terms (mths) ">
									<HeaderStyle Width="16%"></HeaderStyle>
									<ItemTemplate>
										<asp:TextBox id="txt_warranty" runat="server" CssClass="numerictxtbox" Width="55px" MaxLength="3"></asp:TextBox>
										<asp:RegularExpressionValidator id="val_warranty" Display="none" ControlToValidate="ddl_cur" Runat="server"></asp:RegularExpressionValidator>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="False" HeaderText="index">
									<ItemTemplate>
										<asp:Label id=index Text='<%# DataBinder.Eval(Container.DataItem,"RD_RFQ_ID") %>' Runat="server">
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="RD_RFQ_Line" HeaderText="LINE_NO"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD class="tablecol">&nbsp;<asp:label id="lblExStock" runat="server" Width="505px" Font-Bold="True"></asp:label>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:button id="cmd_update" runat="server" CssClass="button" Text="Save"></asp:button>&nbsp;<asp:button id="cmd_delete" runat="server" CssClass="button" Text="Delete"></asp:button>&nbsp;<asp:button id="cmd_reset" runat="server" CssClass="Button" Text="Reset" CausesValidation="False"></asp:button></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary><asp:requiredfieldvalidator id="rvl_RFQName" runat="server" ControlToValidate="txt_rfq_name" ErrorMessage="RFQ Name is required."
							Display="None"></asp:requiredfieldvalidator><br>
						<asp:label id="lbl_check" runat="server" ForeColor="Red"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
				<TR>
					<TD><A id="cmd_back" href="#" runat="server"><STRONG>&lt; Back</STRONG></A></TD>
				</TR>
				<TR>
					<TD class="emptycol"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
