<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQ_List1.aspx.vb" Inherits="eProcure.RFQ_List1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>RFQ_List</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sOpen As String = dDispatcher.direct("ExtraFunc", "GeneratePDF.aspx", "pageid=" + strPageId + "&type=INV")
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
				SelectAllG("dtg_Draft_ctl02_chkAll2","chkSelection2");
			}
			
			function checkChild2(id)
			{
				checkChildG(id,"dtg_Draft_ctl02_chkAll2","chkSelection2");
			}
				
			function selectAll3()
			{
				SelectAllG("dtg_Qoute_ctl02_chkAll3","chkSelection3");
			}
			
			function checkChild3(id)
			{
				checkChildG(id,"dtg_Qoute_ctl02_chkAll3","chkSelection3");
			}
			
				function selectAll4()
			{
				SelectAllG("dtg_trash_ctl02_chkAll4","chkSelection4");
			}
					
							
			function clear()
			{
			var oform = document.forms(0);
			alert("ok");
			oform.txt_DocNum.value="";
			oform.txt_VenName.value="";
			}
			
			function checkChild4(id)
			{
				checkChildG(id,"dtg_trash_ctl02_chkAll4","chkSelection4");
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
					<TD class="header" style="HEIGHT: 19px"><FONT size="1"></FONT><asp:label id="lbl_title" runat="server"></asp:label><INPUT id="hidAddItem" type="hidden" runat="server"></TD>
					<TD class="header" style="HEIGHT: 19px" align="right"><A href="#"></A></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 19px" colSpan="2"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"><asp:label id="lbl_display" runat="server"></asp:label></TD>
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
								<TD width="550"><STRONG>&nbsp;Document Number </STRONG>:
									<asp:textbox id="txt_DocNum" runat="server" CssClass="txtbox"></asp:textbox>&nbsp;&nbsp;
									<STRONG>Vendor Name </STRONG>:
									<asp:textbox id="txt_VenName" runat="server" CssClass="txtbox"></asp:textbox>&nbsp;
								</TD>
								<TD align="left">&nbsp;
									<asp:button id="cmd_Search" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;
									<asp:button id="cmd_clear" runat="server" CssClass="button" Text="Clear"></asp:button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 27px" colSpan="2">
						<TABLE class="AllTable" id="Table3" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="TableHeader" colSpan="2">&nbsp;Action</TD>
							</TR>
							<TR class="tablecol">
								<TD style="WIDTH: 441px" align="left" width="441">&nbsp;<STRONG>Current Folder </STRONG>
									:
									<asp:dropdownlist id="ddl_folder" tabIndex="4" runat="server" CssClass="ddl" AutoPostBack="True">
										<asp:ListItem Value="0" Selected="True">Sent</asp:ListItem>
										<asp:ListItem Value="1">Drafts</asp:ListItem>
										<asp:ListItem Value="2">Inbox</asp:ListItem>
										<asp:ListItem Value="3">Trash</asp:ListItem>
									</asp:dropdownlist>&nbsp;<asp:button id="cmd_NewRFQ" tabIndex="6" runat="server" CssClass="button" Text="Create New RFQs"
										Width="104px"></asp:button>&nbsp;</TD>
								<TD align="right">&nbsp;
									<asp:imagebutton id="img_delete" runat="server" ImageUrl="../Images/i_delete.gif" ToolTip="Delete Selected"></asp:imagebutton>&nbsp;
									<asp:imagebutton id="img_pdf" runat="server" ImageUrl="../Images/pdf.bmp" ToolTip="Get Printable PDF"></asp:imagebutton>&nbsp;
									<asp:imagebutton id="ImageButton2" runat="server" ImageUrl="../Images/i_duplicate.gif" ToolTip="Copy Selected RFQs To Draft"></asp:imagebutton>&nbsp;
									<asp:imagebutton id="ImageButton5" runat="server" ImageUrl="../Images/i_submission.gif" Visible="False"
										tooltip="Submit Selected"></asp:imagebutton></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="2"></TD>
				</TR>
				<TR>
					<TD vAlign="top" colSpan="2"><asp:datagrid id="dtg_VendorList" runat="server" CssClass="grid" Visible="False" OnPageIndexChanged="dtg_VendorList_Page"
							OnSortCommand="SortCommand_Click" AutoGenerateColumns="False" Enabled="False">
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
									<HeaderStyle Width="19%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_rfqnum" runat="server"></asp:Label>
										<input type="hidden" id="hidRfqId" name="hidRfqId" runat="server">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="RM_RFQ_Name" SortExpression="RM_RFQ_Name" HeaderText="RFQ Name">
									<HeaderStyle Width="23%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RM_Created_On"  readonly="true"   HeaderText="Creation Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RM_Expiry_Date" HeaderText="Expire Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Vendor List(s)/Vendor(s)">
									<HeaderStyle Width="24%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Status">
									<HeaderStyle Width="11%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_status" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" HeaderText="rfq_no"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RM_Expiry_Date"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RM_RFQ_ID"></asp:BoundColumn>
							</Columns>
						</asp:datagrid><asp:datagrid id="dtg_Draft" runat="server" CssClass="grid" Visible="False" OnPageIndexChanged="dtg_VendorList_Page"
							OnSortCommand="SortCommand_Click" AutoGenerateColumns="False" Enabled="False">
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
								<asp:TemplateColumn SortExpression="RM_RFQ_No" HeaderText="RFQ Number">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_rfqnum2" runat="server"></asp:Label>
										<input type="hidden" id="hidRfqId2" name="hidRfqId2" runat="server">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="RM_RFQ_Name" SortExpression="RM_RFQ_Name" HeaderText="RFQ Name">
									<HeaderStyle Width="21%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RM_Created_On"  readonly="true"   HeaderText="Creation Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RM_Expiry_Date" HeaderText="Expire Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Vendor List(s)/Vendor(s)">
									<HeaderStyle Width="24%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Edit">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemTemplate>
										<A id="img_link" runat="server"></A>
										<asp:HyperLink id="link_edit" runat="server" ImageUrl="../Images/i_edit2.gif"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" HeaderText="rfq_no"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RM_RFQ_No"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="RM_RFQ_ID"></asp:BoundColumn>
							</Columns>
						</asp:datagrid><asp:datagrid id="dtg_Qoute" runat="server" CssClass="grid" Visible="False" OnPageIndexChanged="dtg_VendorList_Page"
							OnSortCommand="SortCommand_Click" AutoGenerateColumns="False" Enabled="False">
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
								<asp:TemplateColumn SortExpression="RM_RFQ_No" HeaderText="RFQ Number">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_rfqnum3" runat="server"></asp:Label>
										<input type="hidden" id="hidRfqId3" name="hidRfqId3" runat="server">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RM_RFQ_Name" HeaderText="RFQ Name">
									<HeaderStyle Width="18%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_name" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RRM_Actual_Quot_Num" HeaderText="Quotation Number ">
									<HeaderStyle Width="15%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_qouteNum" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RRM_Offer_Till" HeaderText="Quotation Validity ">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_QuoValidity" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn SortExpression="CM_COY_NAME" HeaderText="Vendor(s)">
									<HeaderStyle Width="23%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RM_Currency_Code" SortExpression="RM_Currency_Code" HeaderText="Currency">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="RRM_TotalValue" HeaderText="Amount">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_total" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" HeaderText="rfq_no"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" HeaderText="RM_RFQ_ID" DataField="RM_RFQ_ID"></asp:BoundColumn>
							</Columns>
						</asp:datagrid><asp:datagrid id="dtg_Trash" runat="server" CssClass="grid" Visible="False" OnPageIndexChanged="dtg_VendorList_Page"
							OnSortCommand="SortCommand_Click" AutoGenerateColumns="False" Enabled="False">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll4" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox><!-- <INPUT onclick="javascript:SelectAllCheckboxes(this);" type="checkbox"> -->
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection4" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RM_RFQ_No" HeaderText="RFQ Number">
									<HeaderStyle Width="22%"></HeaderStyle>
									<ItemStyle></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_rfqnum4" runat="server"></asp:Label>
										<input type="hidden" id="hidRfqId4" name="hidRfqId" runat="server">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="RM_RFQ_Name" SortExpression="RM_RFQ_Name" HeaderText="RFQ Name">
									<HeaderStyle Width="23%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RM_Created_On"  readonly="true"  HeaderText="Creation Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="RM_Expiry_Date" HeaderText="Expire Date">
									<HeaderStyle Width="9%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Vendor List(s)/Vendor(s)">
									<HeaderStyle Width="24%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Status">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" HeaderText="rfq_no">
									<HeaderStyle></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD colSpan="2"></TD>
				</TR>
				<TR>
					<TD vAlign="top" colSpan="2">
						<TABLE class="alltable" id="Table4" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR class="tablecol">
								<TD align="left" width="70%"><asp:button id="cmd_emp_trash" runat="server" CssClass="button" Text="Empty Trash Folder" Width="104px"
										Visible="False"></asp:button></TD>
								<TD align="right"><asp:imagebutton id="Imagebutton1" runat="server" ImageUrl="../Images/i_delete.gif" ToolTip="Delete Selected"></asp:imagebutton>&nbsp;
									<asp:imagebutton id="img_pdf2" runat="server" ImageUrl="../Images/pdf.bmp" ToolTip="Get Printable PDF"></asp:imagebutton>&nbsp;
									<asp:imagebutton id="ImageButton3" runat="server" ImageUrl="../Images/i_duplicate.gif" ToolTip="Copy Selected RFQs To Draft"></asp:imagebutton>&nbsp;
									<asp:imagebutton id="ImageButton6" runat="server" ImageUrl="../Images/i_submission.gif" Visible="False"
										tooltip="Submit Selected"></asp:imagebutton></TD>
							</TR>
						</TABLE>
						<asp:label id="lblCurrentIndex" runat="server" CssClass="lbl"></asp:label></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
