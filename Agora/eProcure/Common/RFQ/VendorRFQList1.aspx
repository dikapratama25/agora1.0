<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VendorRFQList1.aspx.vb" Inherits="eProcure.VendorRFQList1" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>VendorRFQList</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sOpen As String = dDispatcher.direct("ExtraFunc", "GeneratePDF.aspx", "pageid='" + strPageId + "'&type=INV'")
		</script>
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		
			function cmdAddClick()
			{
				var result = confirm("Are you sure that you want to pamanently detele this item(s)?", "Yes", "No");
				if(result == true)
					Form1.hidAddItem.value = "1";
				else 
					Form1.hidAddItem.value = "0";
			}
			function selectAll()
			{
				SelectAllG("dtg_VendorList_ctl02_chkAll","chkSelection");
			}
			
			function checkChild(id)
			{
				checkChildG(id,"dtg_VendorList_ctl02_chkAll","chkSelection");
			}
				
			function selectAll2()
			{
				SelectAllG("dtg_quote_ctl02_chkAll2","chkSelection2");
			}
			
			function checkChild2(id)
			{
				checkChildG(id,"dtg_quote_ctl02_chkAll2","chkSelection2");
			}
				
			function selectAll3()
			{
				SelectAllG("dtg_trash_ctl02_chkAll3","chkSelection3");
			}
			
			function checkChild3(id)
			{
				checkChildG(id,"dtg_trash_ct0l2_chkAll3","chkSelection3");
			}
			
			function PDFWindow(strPageId)
			{
				window.open('<% Response.Write(sOpen)%>','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
				
			}
				-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header" style="HEIGHT: 19px"><FONT size="1"></FONT><asp:label id="lbl_title" runat="server"></asp:label><INPUT type="hidden" runat="server" id="hidAddItem"></TD>
					<TD class="header" style="HEIGHT: 19px" align="right"><A href="#"></A></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 19px" colSpan="2"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2">
						<DIV class="div" align="left" ms_positioning="FlowLayout"><asp:label id="lbl_disc" runat="server"></asp:label></DIV>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"></TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="TableHeader" colSpan="2">&nbsp;Search Criteria</TD>
							</TR>
							<TR class="tablecol">
								<TD width="100%">&nbsp;<STRONG>Document Number&nbsp;</STRONG>:
									<asp:textbox id="txt_Num" runat="server" CssClass="txtbox" Width="104px"></asp:textbox>&nbsp;&nbsp;
									<B>Buyer Company Name</B> :
									<asp:textbox id="txt_com_name" runat="server" CssClass="txtbox"></asp:textbox>&nbsp;
									<asp:button id="cmd_Search" runat="server" CssClass="button" Width="56px" Text="Search"></asp:button>&nbsp;
									<asp:button id="cmd_clear" runat="server" CssClass="button" Width="40px" Text="Clear"></asp:button></TD>
								<TD align="left">&nbsp;&nbsp;&nbsp;</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2">
						<TABLE class="AllTable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="TableHeader" colSpan="2">&nbsp;Action</TD>
							</TR>
							<TR class="tablecol">
								<TD align="left" width="500">&nbsp;<STRONG>Current Folder </STRONG>:
									<asp:dropdownlist id="ddl_folder" tabIndex="4" runat="server" CssClass="ddl" AutoPostBack="True">
										<asp:ListItem Value="0" Selected="True">Inbox</asp:ListItem>
										<asp:ListItem Value="1">Sent</asp:ListItem>
										<asp:ListItem Value="2">Trash</asp:ListItem>
									</asp:dropdownlist>&nbsp;</TD>
								<TD align="right">&nbsp;
									<asp:imagebutton id="img_delete" runat="server" ToolTip="Delete Selected" ImageUrl="#"></asp:imagebutton>&nbsp;<asp:imagebutton id="img_pdf" runat="server" ToolTip="Get Printable PDF" ImageUrl="#"></asp:imagebutton>&nbsp;</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"></TD>
				</TR>
				<TR>
					<TD vAlign="top" colSpan="2"></TD>
				</TR>
				<TR>
					<TD colSpan="2"><asp:datagrid id="dtg_VendorList" runat="server" CssClass="grid" OnPageIndexChanged="dtg_VendorList_Page"
							OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
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
								<asp:TemplateColumn SortExpression="RM_RFQ_No" HeaderText="RFQ Number">
									<HeaderStyle Width="18%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_rfqnum" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="RM_RFQ_Name" SortExpression="RM_RFQ_Name" HeaderText="RFQ Name">
									<HeaderStyle Width="27%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RM_Created_On"  readonly="true"  HeaderText="Creation Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RM_Expiry_Date" HeaderText="Expire Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Buyer Company Name">
									<HeaderStyle Width="22%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Status">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_status" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="RM_RFQ_ID"></asp:BoundColumn>
							</Columns>
						</asp:datagrid><asp:datagrid id="dtg_quote" runat="server" CssClass="grid" OnPageIndexChanged="dtg_VendorList_Page"
							OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll2" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection2" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RRM_Actual_Quot_Num" HeaderText="Quotation Number">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_quo" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RM_RFQ_No" HeaderText="RFQ Number">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_RFQ_Num" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="RM_RFQ_Name" SortExpression="RM_RFQ_Name" HeaderText="RFQ Name">
									<HeaderStyle Width="19%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RRM_Created_On" HeaderText="Creation Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RRM_Offer_Till" HeaderText="Quotation Validity ">
									<HeaderStyle Width="9%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Buyer Company Name">
									<HeaderStyle Width="22%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Status">
									<HeaderStyle Width="6%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_status1" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="RRM_RFQ_ID"></asp:BoundColumn>
							</Columns>
						</asp:datagrid><asp:datagrid id="dtg_trash" runat="server" CssClass="grid" OnPageIndexChanged="dtg_VendorList_Page"
							OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll3" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection3" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RM_RFQ_No" HeaderText="Document Number">
									<HeaderStyle Width="21%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="Label1" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn SortExpression="RM_Created_On"  readonly="true"   HeaderText="Creation Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RM_Expiry_Date" HeaderText="Expire Date/Validity Date">
									<HeaderStyle Width="18%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" SortExpression="CM_COY_NAME" HeaderText="Buyer Company Name">
									<HeaderStyle Width="36%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Status">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_status2" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" HeaderText="CHECKDOC"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RM_RFQ_ID"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EMPTYCOL" colSpan="2"></TD>
				</TR>
				<TR>
					<TD vAlign="top" colSpan="2">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR class="tablecol">
								<TD align="left" width="70%"><asp:button id="cmd_emp_trash" runat="server" CssClass="button" Width="104px" Text="Empty Trash Folder"
										Visible="False"></asp:button><asp:label id="lblCurrentIndex" runat="server" CssClass="lbl"></asp:label></TD>
								<TD align="right"><asp:imagebutton id="Imagebutton1" runat="server" ToolTip="Delete Selected" ImageUrl="#"></asp:imagebutton>&nbsp;<asp:imagebutton id="img_pdf2" runat="server" ToolTip="Get Printable PDF" ImageUrl="#"></asp:imagebutton>&nbsp;</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
