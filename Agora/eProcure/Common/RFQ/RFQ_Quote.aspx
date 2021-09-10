<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQ_Quote.aspx.vb" Inherits="eProcure.RFQ_Quote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

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
				var result = confirm("Are you sure that you want to permanently delete this item(s)?", "Yes", "No");
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
		<%  Response.Write(Session("w_CreateRFQ_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">		
				<TR>
					<TD class="linespacing1" colSpan="2"></TD>
				</TR>
				<TR>
	                <TD >
		                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
		                        Text="Fill in the search criteria and click Search button to list the relevant RFQ and quotation details."
		                ></asp:label>

	                </TD>
                </TR>
                <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			    </TR>
				<TR>
					<TD colSpan="2">
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" border="0" width="100%">
							<TR>
								<TD class="TableHeader" colSpan="2">&nbsp;Search Criteria</TD>
							</TR>
							<TR class="tablecol">
								<TD width="80%"><STRONG>&nbsp;RFQ / Quotation Number </STRONG>:
									<asp:textbox id="txt_DocNum" runat="server" CssClass="txtbox" Width="200px"></asp:textbox>&nbsp;&nbsp;
									<STRONG>Vendor </STRONG>:
									<asp:textbox id="txt_VenName" runat="server" CssClass="txtbox" Width="200px"></asp:textbox>&nbsp;
								</TD>
								<TD align="right" width="20%">&nbsp;
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
					<TD vAlign="top" colSpan="2">
							<asp:datagrid id="dtg_Qoute" runat="server" CssClass="grid" OnPageIndexChanged="dtg_VendorList_Page"
							OnSortCommand="SortCommand_Click" AutoGenerateColumns="False">
							<Columns>								
								<asp:TemplateColumn SortExpression="RM_RFQ_No" HeaderText="RFQ Number">
									<HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_rfqnum3" runat="server"></asp:Label>
										<input type="hidden" id="hidRfqId3" name="hidRfqId3" runat="server">
									</ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RM_RFQ_Name" HeaderText="RFQ Description">
									<HeaderStyle Width="18%" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_name" runat="server"></asp:Label>
									</ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RRM_Actual_Quot_Num" HeaderText="Quotation Number ">
									<HeaderStyle Width="15%" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_qouteNum" runat="server"></asp:Label>
									</ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="RRM_Offer_Till" HeaderText="Quotation Validity ">
									<HeaderStyle Width="8%" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lbl_QuoValidity" runat="server"></asp:Label>
									</ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:TemplateColumn>
								<asp:BoundColumn SortExpression="CM_COY_NAME" HeaderText="Vendor(s)">
									<HeaderStyle Width="23%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="RM_Currency_Code" SortExpression="RM_Currency_Code" HeaderText="Currency">
									<HeaderStyle Width="8%" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="RRM_TotalValue" HeaderText="Amount">
									<HeaderStyle Width="8%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lbl_total" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" HeaderText="rfq_no"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" HeaderText="RM_RFQ_ID" DataField="RM_RFQ_ID"></asp:BoundColumn>
							</Columns>
						</asp:datagrid>		
					</TD>
				</TR>
				<TR>
					<TD colSpan="2"></TD>
				</TR>
			
			</TABLE>
		</form>
	</body>
</HTML>
