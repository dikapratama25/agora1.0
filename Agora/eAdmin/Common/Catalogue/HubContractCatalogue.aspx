<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HubContractCatalogue.aspx.vb" Inherits="eAdmin.HubContractCatalogue" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Contract Catalogue Approval</title>
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
		
			function selectAll()
			{
				SelectAllG("dtgCatalogue__ctl2_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCatalogue__ctl2_chkAll","chkSelection");
			}
			
			function CheckDeleteMaster(pChkSelName){
				var oform = document.forms[0];
				var itemCnt, itemCheckCnt;
				itemCnt = 0;
				itemCheckCnt = 0;
				
				re = new RegExp(':' + pChkSelName + '$');  //generated control name starts with a colon	
				for (var i=0;i<oform.elements.length;i++){
					var e = oform.elements[i];
					if (e.type=="checkbox"){						
						if (re.test(e.name)){
							itemCnt ++;
							if (e.checked==true)
								itemCheckCnt ++;
						}
					}
				}
				
				if (itemCheckCnt == 0) {
					alert ('Please make at least one selection!');
					return false;
				}
				else{
					if (itemCnt == itemCheckCnt) {
						var result = confirm('Are you sure that you want to permanently delete this item(s) ?');
						if (result == true){
							var result2 = confirm('Delete Master record too ?');
							if (result2 == true) 
								Form1.hidDelete.value = "1";								
							else
								Form1.hidDelete.value = "0";
							return true;
						}
						else
							return false;
					}
					else {
						Form1.hidDelete.value = "0";
						return confirm('Are you sure that you want to permanently delete this item(s) ?');
					}
				}				
			}
			
			function confirmReject(remarkCnt, blnLine) {
				if (confirm('Are you sure that you want to reject this contract catalogue?')) {
					return resetSummary(0,1);
				}
				return false;
			}

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
					<TD class="TableHeader" colSpan="4">&nbsp;Contract&nbsp;Group Header</TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol" width="15%">&nbsp;<STRONG>Contract Ref. No.</STRONG>&nbsp;:</TD>
					<TD class="TableInput" width="30%"><asp:label id="lblCode" runat="server"></asp:label></TD>
					<TD class="tablecol" width="15%">&nbsp;<STRONG>Description</STRONG>&nbsp;:</TD>
					<TD class="TableInput" width="40%"><asp:label id="lblDesc" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol">&nbsp;<STRONG>Start Date</STRONG>&nbsp;:</TD>
					<TD class="TableInput"><asp:label id="lblStartDate" runat="server"></asp:label><% Response.Write(calStartDate)%></A></TD>
					<TD class="tablecol">&nbsp;<STRONG>End Date</STRONG>:</TD>
					<TD class="TableInput"><asp:label id="lblEndDate" runat="server"></asp:label><% Response.Write(calEndDate)%></A></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol">&nbsp;<STRONG>Vendor Name</STRONG>&nbsp;:</TD>
					<TD class="TableInput" colSpan="3"><asp:label id="lblVendor" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol">&nbsp;<STRONG>Buyer Company</STRONG>&nbsp;:</TD>
					<TD class="TableInput" colSpan="3"><asp:label id="lblBuyer" runat="server"></asp:label></TD>
				</TR>
				<tr vAlign="top">
					<td class="tablecol" noWrap>&nbsp;<STRONG>No. of Times Rejected</STRONG>&nbsp;:</td>
					<td class="TableInput" colSpan="3"><asp:label id="lblRejCnt" runat="server" Width="78px"></asp:label></td>
				</tr>
				<tr runat="server" id="trApprove2">
					<td colSpan="4" class="emptycol">Please click <i>Save</i> button to save your line 
						item remarks before proceed to next page records or approving/rejecting the 
						contract.</td>
				</tr>
				<tr>
					<td colSpan="4">&nbsp;</td>
				</tr>
				<tr>
					<td colSpan="4"><asp:datagrid id="dtgCatalogue" runat="server" AutoGenerateColumns="false" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn SortExpression="CDUI_Product_Code" HeaderText="Item ID">
									<HeaderStyle HorizontalAlign="left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="CDUI_Vendor_Item_Code" SortExpression="CDUI_Vendor_Item_Code" ReadOnly="True"
									HeaderText="Vendor Item Code">
									<HeaderStyle Width="13%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDUI_Product_Desc" SortExpression="CDUI_Product_Desc" ReadOnly="True"
									HeaderText="Item Description">
									<HeaderStyle Width="24%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDUI_Currency_Code" SortExpression="CDUI_Currency_Code" ReadOnly="True"
									HeaderText="Currency">
									<HeaderStyle Width="7%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDUI_Unit_Cost" SortExpression="CDUI_Unit_Cost" ReadOnly="True" HeaderText="Price">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDUI_UOM" SortExpression="CDUI_UOM" ReadOnly="True" HeaderText="UOM">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="CDUI_Remark" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="left" Width="25%"></HeaderStyle>
									<ItemStyle HorizontalAlign="left" Wrap="False"></ItemStyle>
									<ItemTemplate>
										<asp:Label ID="lblRemark" Runat="server" Visible="True"></asp:Label>
										<asp:TextBox ID="txtRemark" Width="150px" CssClass="listtxtbox" Rows="2" Runat="server" TextMode="MultiLine"
											MaxLength="400"></asp:TextBox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px" ForeColor="Red"
											contentEditable="false"></asp:TextBox>
										<INPUT class="txtbox" id="hidCode" type="hidden" runat="server" NAME="hidCode">
										<asp:ImageButton Runat="server" ID="lnkRemark"></asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<TR>
					<TD colSpan="4">&nbsp;</TD>
				</TR>
				<tr id="trApprove" runat="server">
					<td colSpan="4">
						<asp:button id="cmdSave" runat="server" CssClass="Button" Text="Save"></asp:button>
						<asp:button id="cmdApprove" runat="server" CssClass="Button" Text="Approve"></asp:button>
						<asp:button id="cmdReject" runat="server" CssClass="Button" Text="Reject"></asp:button>
						<INPUT class="txtbox" id="hidDelete" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2"
							name="hidDelete" runat="server"></td>
				</tr>
				<TR>
					<TD class="emptycol" colspan="4">&nbsp;&nbsp;
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"><INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
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
