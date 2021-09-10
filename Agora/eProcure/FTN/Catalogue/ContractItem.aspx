<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ContractItem.aspx.vb" Inherits="eProcure.ContractItemFTN" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
	<HEAD>
		<title>Contract Catalogue</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            Dim css As String = "<LINK href=""" & dDispatcher.direct("Plugins/css", "SPP.css") & """ rel='stylesheet'>"
       </script>
        <% Response.Write(css)%>   
        <% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
		<!--
		    			
		     function ShowDialog(filename,height)
			{				
				var retval="";
				retval=window.showModalDialog(filename,"Wheel","help:No;resizable: Yes;Status:Yes;dialogHeight:" + height + "; dialogWidth: 1000px");
				//retval=window.open(filename);
				if (retval == "1" || retval =="" || retval==null)
				{  window.close;
					return false;

				} else {
				    window.close;
					return true;

				}
			}
			
			function selectAll()
			{
				SelectAllG("dtgCatalogue_ctl02_chkAll","chkSelection");
			}
					
			function checkChild(id)
			{
				checkChildG(id,"dtgCatalogue_ctl02_chkAll","chkSelection");
			}
			
			function CheckDeleteMaster(pChkSelName){
				var oform = document.forms[0];
				var itemCnt, itemCheckCnt;
				var result, result2;
				itemCnt = parseInt(Form1.hidCnt.value);
				itemCheckCnt = 0;
//				alert(pChkSelName);
				re = new RegExp(pChkSelName + '$');  //generated control name starts with a colon	
				//alert (re);
				for (var i=0;i<oform.elements.length;i++){
				
					var e = oform.elements[i];
					if (e.type=="checkbox"){
					//alert (e.name);						
						if (re.test(e.name)){
						//alert ('A3!');
							//itemCnt ++;
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
						if (Form1.hidCatType.value == 'D'){
							if (Form1.hidComCnt.value == 0 ){
								result = confirm('Are you sure that you want to permanently delete this item(s) ?');
								if (result == true){
									CheckDeleteMaster2();
									/*result2 = confirm('Delete Master record too ?');
									if (result2 == true) 
										Form1.hidDelete.value = "1";								
									else
										Form1.hidDelete.value = "0";
									return true;*/
								}
								else
									return false;
							}
							else {
								Form1.hidDelete.value = "0";
								return confirm('Are you sure that you want to permanently delete this item(s) ?');
							}
						}
						else { // cattype=='C'
							result = confirm('Are you sure that you want to permanently delete this item(s) ?');
							if (result == true){
								CheckDeleteMaster2();
								/*result2 = confirm('Delete Master record too ?');
								if (result2 == true) 
									Form1.hidDelete.value = "1";								
								else
									Form1.hidDelete.value = "0";
								return true;*/
							}
							else
								return false;
						}
					}	
					else {
						Form1.hidDelete.value = "0";
						return confirm('Are you sure that you want to permanently delete this item(s) ?');
					}
				}				
			}

		-->
		</script>
		<script language="vbscript">
			
			sub CheckDeleteMaster2		
				dim msg
				msg = msgbox ("Do you want to delete Contract Header?",4)
				'if Form1.hidDeleteCnt.value = "1" then						
					'//yes=6, no=7
					'msgbox(msg, 4,"change") 
					if msg = vbYes then
						Form1.hidDelete.value = "1"
					else					
						Form1.hidDelete.value = "0"						
					end if
				'end if
			end sub				
					
		</script>
		
	</HEAD>
	<body class="body"  MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_ConCat_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
			    <tr><td class="rowspacing" colspan="6"></td></tr>
				<TR>
					<TD class="EmptyCol" colspan="6">
					    <asp:label id="lblAction1" runat="server" CssClass="lblInfo" Text="Step 1: Create, delete or modify Contract Catalogue.<br><b>=></b> Step 2: Assign item master to Contract Catalogue.<br>Step 3: Assign User to Contract Catalogue."></asp:label>					    
					</TD>
				</TR>
				<tr><td class="rowspacing"></td></tr>
				<TR>
					<TD class="tableheader" colspan="6">Search Criteria</TD>
				</TR>
				<TR>
					<TD class="tablecol" style="height: 25px; width: 100%;"><strong><asp:label id="lblCodeLabel" runat="server" Text="Contract Ref. No. " CssClass="lbl"></asp:label></strong>:&nbsp; &nbsp;&nbsp;&nbsp;<asp:dropdownlist id="ddlCode" runat="server" Width="300px" CssClass="ddl" AutoPostBack="True"></asp:dropdownlist></TD>
				</TR>				
				<tr><td class="rowspacing"></td></tr>
				<tr>
					<td class="EmptyCol" colspan="4">
					    <asp:datagrid id="dtgCatalogue" runat="server" AutoGenerateColumns="False" OnSortCommand="SortCommand_Click">
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
								<asp:TemplateColumn SortExpression="CDI_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkCode"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>															
								<asp:BoundColumn DataField="CDI_PRODUCT_DESC" SortExpression="CDI_PRODUCT_DESC" ReadOnly="True" HeaderText="Item Name">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDI_CURRENCY_CODE" SortExpression="CDI_CURRENCY_CODE" ReadOnly="True"
									HeaderText="Currency">
									<HeaderStyle Width="8%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDI_UNIT_COST" SortExpression="CDI_UNIT_COST" ReadOnly="True" HeaderText="Price">
									<HeaderStyle HorizontalAlign="Right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<%--Jules 2014.07.14 GST Enhancement begin--%>
								<asp:BoundColumn DataField="CDI_GST" SortExpression="CDI_GST" ReadOnly="True" HeaderText="Tax (%)">
									<HeaderStyle HorizontalAlign="Right" Width="7%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GSTRATE" SortExpression="CDI_GST_RATE" ReadOnly="True" HeaderText="GST Rate">
									<HeaderStyle HorizontalAlign="Left" Width="7%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="CDI_GST_TAX_CODE" SortExpression="CDI_GST_TAX_CODE" ReadOnly="True" HeaderText="GST Tax Code (Purchase)">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<%--Jules 2014.07.14 GST Enhancement end--%>
								<asp:BoundColumn DataField="CDI_UOM" SortExpression="CDI_UOM" ReadOnly="True" HeaderText="UOM">
									<HeaderStyle Width="7%"></HeaderStyle>
								</asp:BoundColumn>								
								<asp:BoundColumn DataField="CDI_REMARK" ReadOnly="True" HeaderText="Remarks">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CDI_PRODUCT_CODE" ReadOnly="True" HeaderText="Product Code" Visible="false">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
					</td>
				</tr>
				<TR>
					<TD class="emptycol" colspan="4">
						<asp:button id="cmdAdd" runat="server" CssClass="Button" Text="Add"></asp:button>
						<asp:button id="cmdModify" runat="server" CssClass="Button" Text="Modify" CausesValidation="False"></asp:button>
						<asp:button id="cmdDelete" runat="server" CssClass="Button" Text="Delete" CausesValidation="False"></asp:button>
						<asp:button id ="btnHidden" CausesValidation="false" runat="server" style="display:none" onclick="btnHidden_Click"></asp:button> 
						<INPUT class="txtbox" id="hidDelete" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidDelete" runat="server"> 
						<INPUT class="txtbox" id="hidComCnt" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidComCnt" runat="server"> 
						<INPUT class="txtbox" id="hidCatType" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidCatType" runat="server">
						<INPUT class="txtbox" id="hidCnt" style="WIDTH: 45px; HEIGHT: 18px" type="hidden" size="2" name="hidCnt" runat="server">
					</TD>
				</TR>
				<tr><td class="rowspacing"></td></tr>
				<TR>
					<TD class="EmptyCol" colspan="6">
					    <asp:label id="Label1" runat="server" CssClass="lblInfo" Text="a) Click Add button to add new item master to the selected Contract Catalogue. <br>b) Click Modify button to modify the currency, price or remark of the contract item. <br>c) Click Remove button to delete the contract item from the selected Contract Catalogue."></asp:label>					    
					</TD>
				</TR>
				<TR>
					<TD colspan="4">
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg"></asp:validationsummary>
					</TD>
				</TR>			
				
			</TABLE>
		</form>
	</body>
</HTML>
