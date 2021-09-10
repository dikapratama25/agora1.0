<%@ Page Language="vb" AutoEventWireup="false" Codebehind="VendorRFQListExp.aspx.vb" Inherits="eProcure.VendorRFQListExp" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>VendorRFQListExp</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		//For check in Datagrid
		function selectAll()
		{
			SelectAllG("dtgOutstandingRFQ_ctl02_chkAll","chkSelection");
		}
		
		function checkChild(id)
		{
			checkChildG(id,"dtgOutstandingRFQ_ctl02_chkAll","chkSelection");
		}
		
				-->
		</script>
	</HEAD>
	<body class="body" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_RFQ_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">			
										
				<tr>
					<TD class="linespacing1" colSpan="2"></TD>
			</TR>
			<TR>
				<TD >
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					Text="Fill in the search criteria and click Search button to list the relevant expired RFQ. To delete the RFQ, check the box and click the Delete button."></asp:label>
				</TD>
			</TR>
            <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
				
				<TR>
					<TD colSpan="2">
						<TABLE class="AllTable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD class="TableHeader" colSpan="2">&nbsp;Search Criteria</TD>
							</TR>
							<TR class="tablecol">
								<TD width="100%">&nbsp;<STRONG>RFQ Number&nbsp;</STRONG>:
									<asp:textbox id="txt_Num" runat="server" CssClass="txtbox" Width="140px"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;
									<B>Purchaser Company </B> :
									<asp:textbox id="txt_com_name" runat="server" CssClass="txtbox"></asp:textbox>&nbsp;</TD>
								<TD align="right"><asp:button id="cmd_Search" runat="server" CssClass="button" Width="56px" Text="Search"></asp:button>&nbsp;
									<asp:button id="cmd_clear" runat="server" CssClass="button" Width="40px" Text="Clear"></asp:button></TD>
							</TR>
						</TABLE>
						</TD>
				<TR>
					<TD colSpan="2">
					    <asp:datagrid id="dtgOutstandingRFQ" runat="server" CssClass="grid" OnSortCommand="SortCommandOutStandingRFQ_Click" AutoGenerateColumns="False" OnPageIndexChanged="dtgOutstandingRFQ_Page">
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
								<asp:TemplateColumn SortExpression="RFQ Number" HeaderText="RFQ Number">
						            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
						            <ItemStyle HorizontalAlign="Left"></ItemStyle>
						            <ItemTemplate>
							            <asp:HyperLink Runat="server" ID="lnkRFQNum"></asp:HyperLink>
						            </ItemTemplate>
					            </asp:TemplateColumn>
                                <asp:BoundColumn DataField="RM_RFQ_ID" HeaderText="RM_RFQ_ID" SortExpression="RM_RFQ_ID" Visible="False">
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="RFQ Name" HeaderText="RFQ Description" SortExpression="RFQ Name">
                                    <HeaderStyle HorizontalAlign="Left" Width="31%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Creation Date" HeaderText="Creation Date" SortExpression="Creation Date">
                                    <HeaderStyle HorizontalAlign="Left" Width="14%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Expiry Date" HeaderText="Expiry Date" SortExpression="Expiry Date">
                                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
                                 
                               <asp:BoundColumn DataField="Buyer Company" HeaderText="Purchaser Company" SortExpression="Buyer Company">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="EMPTYCOL" colSpan="2"></TD>
				</TR>
				<TR>
					<TD vAlign="top" colSpan="2">
						<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR >
								<TD align="left" width="70%"><asp:button id="cmdDelete" runat="server" CssClass="button" Width="70px" Text="Delete"></asp:button>
										<asp:label id="lblCurrentIndex" runat="server" CssClass="lbl"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
