<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HubContractPrice.aspx.vb" Inherits="eAdmin.HubContractPrice" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Contract Price</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
       <script runat="server">
           Dim dDispatcher As New AgoraLegacy.dispatcher
           Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "Styles.css") & """ rel='stylesheet'>"
           Dim calStartDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtStartDate") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
           Dim calEndDate As String = "<A onclick=""window.open('" & dDispatcher.direct("Calendar", "viewCalendar.aspx", "TextBox=txtEndDate") & "','cal','width=180,height=155,left=270,top=180');""><IMG style='CURSOR:hand' height='16' src='" & dDispatcher.direct("Plugins/Images", "i_calendar2.gif") & "' align='absBottom' vspace='0'></A>"
      </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		
			
		-->
		</script>
	</HEAD>
	<body onload="" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header" colSpan="4"><asp:label id="lblTitle" runat="server" CssClass="header"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<TR>
					<TD class="TableHeader" colSpan="4">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol" noWrap width="15%">&nbsp;<STRONG><asp:label id="lblCodeLabel" runat="server"></asp:label></STRONG>&nbsp;:</TD>
					<TD class="TableInput" width="30%"><asp:label id="lblCode" runat="server"></asp:label></TD>
					<TD class="tablecol" noWrap width="15%">&nbsp;<STRONG>Description</STRONG>&nbsp;:</TD>
					<TD class="TableInput" width="40%"><asp:label id="lblDesc" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol" noWrap>&nbsp;<STRONG>Start Date</STRONG>&nbsp;:</TD>
					<TD class="TableInput"><asp:label id="lblStartDate" runat="server"></asp:label><% Response.Write(calStartDate)%></A></TD>
					<TD class="tablecol" noWrap>&nbsp;<STRONG>End Date</STRONG>:</TD>
					<TD class="TableInput"><asp:label id="lblEndDate" runat="server"></asp:label><% Response.Write(calEndDate)%></A></TD>
				</TR>
				<TR id="trBuyer" vAlign="top" runat="server">
					<TD class="tablecol" noWrap>&nbsp;<STRONG>Buyer Company</STRONG>&nbsp;:</TD>
					<TD class="TableInput" colSpan="3"><asp:label id="lblBuyer" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<tr>
					<td colSpan="4"><asp:datagrid id="dtgCatalogue" runat="server" OnSortCommand="SortCommand_Click" AutoGenerateColumns="False"
							Width="100%">
							<Columns>
								<asp:TemplateColumn SortExpression="PRODUCT_CODE" HeaderText="Item ID">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink><INPUT class="txtbox" id="hidCode" type="hidden" runat="server" NAME="hidCode">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="VENDOR_ITEM_CODE" SortExpression="VENDOR_ITEM_CODE" ReadOnly="True" HeaderText="Vendor Item Code">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRODUCT_DESC" SortExpression="PRODUCT_DESC" ReadOnly="True" HeaderText="Item Description">
									<HeaderStyle Width="32%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="UNIT_COST" SortExpression="UNIT_COST" ReadOnly="True" HeaderText="List Price">
									<HeaderStyle Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="CONTRACT_PRICE" HeaderText="Contract Price&lt;font color=red&gt;*&lt;/font&gt;">
									<HeaderStyle HorizontalAlign="Left" Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtPrice" CssClass="numerictxtbox" Width="100px" Runat="server" Rows="1"></asp:TextBox>
										<asp:RegularExpressionValidator id="revPrice" Runat="server" ControlToValidate="txtPrice" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
										<input runat="server" id="hidPrice" name="hidPrice" type="hidden">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="REMARK" HeaderText="Remarks">
									<HeaderStyle Width="17%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
									<ItemTemplate>
										<asp:TextBox id="txtRemark" Width="150px" CssClass="listtxtbox" Rows="2" Runat="server" TextMode="MultiLine"
											MaxLength="400"></asp:TextBox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red"
											contentEditable="false"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="GST" SortExpression="GST" ReadOnly="True" HeaderText="GST">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="UOM" SortExpression="UOM" ReadOnly="True" HeaderText="UOM">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CURRENCYCODE" SortExpression="CURRENCYCODE" ReadOnly="True"
									HeaderText="CURRENCYCODE">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CONTRACT_PRICE" SortExpression="CONTRACT_PRICE" ReadOnly="True"
									HeaderText="CONTRACT_PRICE">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="REMARK" SortExpression="REMARK" ReadOnly="True" HeaderText="REMARK">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<TR>
					<TD colSpan="4">&nbsp;</TD>
				</TR>
				<TR>
					<TD colSpan="4"><asp:button id="cmdSave" runat="server" CssClass="Button" Text="Save"></asp:button>
						<INPUT class="button" id="cmdReset" onclick="ValidatorReset();" type="button" value="Reset"
							name="cmdReset" runat="server"></TD>
				</TR>
				<TR>
					<TD colSpan="4">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"><INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"></TD>
				</TR>
				<TR>
					<TD colSpan="4">&nbsp;</TD>
				</TR>
				<TR>
					<TD colSpan="4"><asp:hyperlink id="lnkBack" Runat="server">
							<STRONG>&lt; Back</STRONG></asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
