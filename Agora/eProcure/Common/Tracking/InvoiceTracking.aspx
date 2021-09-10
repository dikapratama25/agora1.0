<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InvoiceTracking.aspx.vb" Inherits="eProcure.InvoiceTracking" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>InvoiceTracking</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim PrintWindow as string = dDispatcher.direct("ExtraFunc","FramePrinting.aspx", "pageid="" + ""strPageId"" + ""type=INV")
            dim PDFWindow as string = dDispatcher.direct("ExtraFunc","GeneratePDF.aspx", "pageid="" + ""strPageId"" + ""type=INV")
        </script> 
		<%response.write(Session("WheelScript"))%>
		<script language="javascript">
		<!--  
		function checkAtLeastOneResetSummary(p1, p2, cnt1, cnt2)
			{
				if (CheckAtLeastOne(p1,p2)== true) {
					if (resetSummary(cnt1,cnt2)==true)
						return true;
					else
						return false;
				}
				else {
					return false;
				}				
			}
			
		function selectAll()
		{
			SelectAllG("dtgInvoice_ctl02_chkAll","chkSelection");
		}
				
		function checkChild(id)
		{
			checkChildG(id,"dtgInvoice_ctl02_chkAll","chkSelection");
		}
		
		function Reset(){
			var oform = document.forms(0);			
			oform.txtDocNo.value="";
			oform.txtVendorName.value="";
			oform.txtDept.value="";
			oform.txtBuyer.value="";
		}	
		
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}	
		
		function CheckAtLeastOnePrint(pChkSelName){
			var oform = document.forms[0];
			re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
			for (var i=0;i<oform.elements.length;i++)
			{
				var e = oform.elements[i];
				if (e.type=="checkbox"){
					if (e.checked==true)	{
						var strMsg;
						strMsg = "Warning :\nClicking OK will update the status of the document to 'Printed', even though you may cancel the print job at the last minute.\nDo you wish to continue ?";
						var result = confirm(strMsg);
						if (result==true){							
							//window.open('../ExtraFunc/FramePrinting.aspx?type=INV','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
							return true;
						}
						else
							return false;
					}
				}
			}
			alert('Please make at least one selection!');
			return false;
		}
		
		function PrintWindow(strPageId)
		{
			//window.open('../ExtraFunc/FramePrinting.aspx?type=INV','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
			window.open('<%response.write(PrintWindow) %>','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
		}
		function PDFWindow(strPageId)
		{
			window.open('<%response.write(PDFWindow) %>','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
		}
		
		function CheckAtLeastOnePdf(pChkSelName){
			var oform = document.forms[0];
			re = new RegExp(':' + pChkSelName + '$')  //generated control name starts with a colon	
			for (var i=0;i<oform.elements.length;i++)
			{
				var e = oform.elements[i];
				if (e.type=="checkbox"){
					if (e.checked==true)	{						
						//window.open('../ExtraFunc/GeneratePDF.aspx?type=INV','Spool','Width=450,Height=180,toolbar=0,status=0,z-lock=yes,scrollbars=0,resizable=0');
						//window.open('../Template/TestPrint.aspx?type=INV','Spool','status=no,toolbar=no,location=no,menu=no,scrollbars=no,width=1,height=1');
						return true;
					}
				}
			}
			alert('Please make at least one selection!');
			return false;
		}	
		-->
		</script>
	</HEAD>
	<body >
		<form id="Form1" method="post" runat="server">
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="header" colSpan="4"><STRONG><asp:label id="lblType" runat="server"></asp:label></STRONG></TD>
				</TR>
				<TR>
					<TD class="emptycol" style="HEIGHT: 16px" colSpan="4"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"><asp:label id="lblRemark" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4">&nbsp;</TD>
				</TR>
				<TR>
					<TD class="tableheader" colSpan="4">&nbsp;Search Criteria</TD>
				</TR>
				<TR class="tablecol">
					<TD noWrap width="15%">&nbsp;<STRONG>Document No.</STRONG> :</TD>
					<TD width="25%"><asp:textbox id="txtDocNo" runat="server" MaxLength="50" Width="200px" CssClass="txtbox"></asp:textbox></TD>
					<TD noWrap width="15%">&nbsp;<STRONG>Vendor Name</STRONG> :</TD>
					<TD width="45%"><asp:textbox id="txtVendorName" runat="server" MaxLength="50" Width="200px" CssClass="txtbox"></asp:textbox></TD>
				</TR>
				<TR class="tablecol">
					<TD noWrap>&nbsp;<STRONG>Buyer</STRONG> :</TD>
					<TD><asp:textbox id="txtBuyer" runat="server" MaxLength="50" Width="200px" CssClass="txtbox"></asp:textbox></TD>
					<TD noWrap>&nbsp;<STRONG>Dept.</STRONG> :</TD>
					<TD><asp:textbox id="txtDept" runat="server" MaxLength="50" Width="200px" CssClass="txtbox"></asp:textbox></TD>
				</TR>
				<TR class="tablecol">
					<TD colSpan="4"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<TR>
					<TD class="tableheader" colSpan="4">&nbsp;Action</TD>
				</TR>
				<TR class="tablecol">
					<TD noWrap>&nbsp;<STRONG>Current Folder </STRONG>:</TD>
					<td align="left" colSpan="2"><asp:dropdownlist id="cboFolder" runat="server" CssClass="txtbox" AutoPostBack="True"></asp:dropdownlist></td>
					<TD class="emptycol" align="right" colSpan="3"><asp:imagebutton id="cmdDelete" runat="server" ImageUrl="#" ToolTip="Archive Selected"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="cmdPdf" runat="server" ImageUrl="#" ToolTip="Get Printable PDF"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="cmdSaveInv" runat="server" Width="17px" ImageUrl="#" ToolTip="Download Invoice"
							Height="17px"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="cmdPrint" runat="server" ImageUrl="#" ToolTip="Spool to Printer"></asp:imagebutton>&nbsp;
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"><asp:datagrid id="dtgInvoice" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete">
									<HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<HeaderTemplate>
										<asp:checkbox id="chkAll" runat="server" AutoPostBack="false" ToolTip="Select/Deselect All"></asp:checkbox>
									</HeaderTemplate>
									<ItemTemplate>
										<asp:checkbox id="chkSelection" Runat="server"></asp:checkbox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="IM_INVOICE_NO" HeaderText="Invoice Number">
									<HeaderStyle Width="8%"></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkINVNo"></asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="IM_INVOICE_INDEX" SortExpression="IM_INVOICE_INDEX" HeaderText="Index"></asp:BoundColumn>
								<asp:BoundColumn DataField="IM_PAYMENT_DATE" SortExpression="IM_PAYMENT_DATE"  readonly="true"   HeaderText="Due Date">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="IM_PO_INDEX" SortExpression="IM_PO_INDEX" readonly="true"  
									HeaderText="IM_PO_INDEX"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="IM_S_COY_ID" SortExpression="IM_S_COY_ID" readonly="true"  
									HeaderText="Vendor Name"></asp:BoundColumn>
								<asp:BoundColumn DataField="POM_S_COY_NAME" SortExpression="POM_S_COY_NAME"  readonly="true"    HeaderText="Vendor Name">
									<HeaderStyle Width="17%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POM_CURRENCY_CODE" SortExpression="POM_CURRENCY_CODE"  readonly="true"  
									HeaderText="Currency">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IM_INVOICE_TOTAL" SortExpression="IM_INVOICE_TOTAL"  readonly="true"    HeaderText="Amount">
									<HeaderStyle Width="7%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn readonly="true"   HeaderText="Related Document">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="IM_PAYMENT_TERM" HeaderText="Payment Method">
									<ItemStyle Width="10%"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboPay" CssClass="ddl" Runat="server"></asp:DropDownList>
										<asp:Label Runat="server" ID="lblPay"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="POM_BUYER_NAME" SortExpression="POM_BUYER_NAME"  readonly="true"    HeaderText="Buyer">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DEPT" SortExpression="DEPT"  readonly="true"   HeaderText="Department">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IM_PRINTED" SortExpression="IM_PRINTED" HeaderText="Printed"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="IM_INVOICE_STATUS" SortExpression="IM_INVOICE_STATUS"
									HeaderText="Status"></asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC"  readonly="true"   HeaderText="Status">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Approval Remarks">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblRemarks" Runat="server"></asp:Label><BR>
										<asp:TextBox id="txtRemark" CssClass="listtxtbox" Width="150px" MaxLength="400" Runat="server"
											TextMode="MultiLine" Rows="2"></asp:TextBox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px"  contentEditable="false" 
											ForeColor="Red"></asp:TextBox><INPUT class="txtbox" id="hidCode" type="hidden" name="hidCode" runat="server">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="IM_FM_APPROVED_DATE" SortExpression="IM_FM_APPROVED_DATE"  readonly="true"  
									HeaderText="Approved Payment Date">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="POM_BILLING_METHOD" SortExpression="POM_BILLING_METHOD"
									 readonly="true"    HeaderText="POM_BILLING_METHOD"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4">
						<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save" CausesValidation="False"></asp:button>
						<asp:button id="cmdSubmit" runat="server" Width="96px" CssClass="button" Text="Mass Approve"
							CausesValidation="False"></asp:button>
						<asp:button id="cmdApprove" runat="server" Width="96px" CssClass="button" Text="Mass Approve"
							CausesValidation="False"></asp:button>
						<asp:button id="cmdMark" runat="server" Width="80px" CssClass="button" Text="Mark As Paid" CausesValidation="False"></asp:button>
						<INPUT class="button" id="cmdReset" onclick="ValidatorReset();" type="button" value="Reset"
							name="cmdReset" runat="server"> <INPUT id="hidMode" type="hidden" size="1" name="hidMode" runat="server"><INPUT id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server">&nbsp;
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4">&nbsp;&nbsp;
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"><INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"></TD>
				</TR>
				</TABLE>
		</form>
	</body>
</HTML>
