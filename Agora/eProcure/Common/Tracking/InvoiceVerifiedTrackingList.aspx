<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InvoiceVerifiedTrackingList.aspx.vb" Inherits="eProcure.InvoiceVerifiedTrackingList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Invoice Tracking</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <script runat="server">
            Dim dDispatcher As New AgoraLegacy.dispatcher
            dim PrintWindow as string = dDispatcher.direct("ExtraFunc","FramePrinting.aspx", "pageid="" + ""strPageId ""+ ""type=INV")
            dim PDFWindow as string = dDispatcher.direct("ExtraFunc","GeneratePDF.aspx", "pageid="" + ""strPageId ""+ ""type=INV")
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
		//craven 
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
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <%  Response.Write(Session("w_InvTracking_tabs"))%>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<TD class="linespacing1" colSpan="5"></TD>
			    </TR>
				<TR>
				    <TD colSpan="5">
					    <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Fill in the search criteria and click Search button to list the relevant verified Invoice."
					    ></asp:label>

				    </TD>
			    </TR>
			    <tr>
					<TD class="linespacing2" colSpan="5"></TD>
			    </TR>
				<TR>
					<TD class="tableheader" colSpan="5">&nbsp;Search Criteria</TD>
				</TR>
				<TR class="tablecol">
					<TD noWrap width="15%">&nbsp;<STRONG>Document No.</STRONG> :</TD>
					<TD width="20%"><asp:textbox id="txtDocNo" runat="server" MaxLength="50" Width="200px" CssClass="txtbox"></asp:textbox></TD>
					<TD noWrap width="15%">&nbsp;<STRONG>Vendor Name</STRONG> :</TD>
					<TD width="35%"><asp:textbox id="txtVendorName" runat="server" MaxLength="50" Width="200px" CssClass="txtbox"></asp:textbox></TD>
					<TD width="15%" align="right"><asp:button id="cmdSearch" runat="server" CssClass="button" Text="Search"></asp:button>&nbsp;<INPUT class="button" id="cmdClear" onclick="Reset();" type="button" value="Clear" name="cmdClear">
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="5"></TD>
				</TR>			
				<TR class="tablecol">
					<TD noWrap>&nbsp;</TD>
					<td align="left" colSpan="2"><asp:dropdownlist id="cboFolder" runat="server" CssClass="txtbox" AutoPostBack="True"  Visible="False"></asp:dropdownlist></td>
					<TD class="emptycol" align="right" colSpan="3"><asp:imagebutton id="cmdDelete" runat="server" ImageUrl="#" ToolTip="Archive Selected" Visible="False"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="cmdPdf" runat="server" ImageUrl="#" ToolTip="Get Printable PDF" Visible="False"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="cmdSaveInv" runat="server" Width="17px" ImageUrl="#" ToolTip="Download Invoice"
							Height="17px" Visible="False"></asp:imagebutton>&nbsp;
						<asp:imagebutton id="cmdPrint" runat="server" ImageUrl="#" ToolTip="Spool to Printer" Visible="False"></asp:imagebutton>&nbsp;
					</TD>
				</TR>				
				<TR>
					<TD class="emptycol" colSpan="5"><asp:datagrid id="dtgInvoice" runat="server" OnSortCommand="SortCommand_Click">
							<Columns>
								<asp:TemplateColumn HeaderText="Delete" Visible="false">
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
									<HeaderStyle Width="8%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
									<ItemTemplate>
										<asp:HyperLink Runat="server" ID="lnkINVNo"></asp:HyperLink>
									</ItemTemplate>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="IM_INVOICE_INDEX" SortExpression="IM_INVOICE_INDEX" HeaderText="Index">
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IM_PAYMENT_DATE" SortExpression="IM_PAYMENT_DATE"  readonly="True"   HeaderText="Due Date">
									<HeaderStyle Width="5%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="IM_PO_INDEX" SortExpression="IM_PO_INDEX" readonly="True"  
									HeaderText="IM_PO_INDEX">
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="IM_S_COY_ID" SortExpression="IM_S_COY_ID" readonly="True"  
									HeaderText="Vendor Name">
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
                                </asp:BoundColumn>
								<asp:BoundColumn DataField="POM_S_COY_NAME" SortExpression="POM_S_COY_NAME"  readonly="True"    HeaderText="Vendor Name">
									<HeaderStyle Width="17%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POM_CURRENCY_CODE" SortExpression="POM_CURRENCY_CODE"  readonly="True"  
									HeaderText="Currency">
									<HeaderStyle Width="5%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IM_INVOICE_TOTAL" SortExpression="IM_INVOICE_TOTAL"  readonly="True"    HeaderText="Amount">
									<HeaderStyle Width="7%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn readonly="True"   HeaderText="Related Document">
									<HeaderStyle Width="15%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="IM_PAYMENT_TERM" HeaderText="Payment Method">
									<ItemStyle Width="10%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboPay" CssClass="ddl" Runat="server"></asp:DropDownList>
										<asp:Label Runat="server" ID="lblPay"></asp:Label>
									</ItemTemplate>
                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="POM_BUYER_NAME" SortExpression="POM_BUYER_NAME"  readonly="True"    HeaderText="Purchaser/Teller">
									<HeaderStyle Width="10%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DEPT" SortExpression="DEPT"  readonly="True" HeaderText="Department" Visible="False">
									<HeaderStyle Width="10%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IM_PRINTED" SortExpression="IM_PRINTED" HeaderText="Printed" Visible="False">
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
                                </asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="IM_INVOICE_STATUS" SortExpression="IM_INVOICE_STATUS"
									HeaderText="Status">
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
                                    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
                                </asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC"  readonly="True"   HeaderText="Status" Visible="False">
									<HeaderStyle Width="5%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Approval Remarks">
									<HeaderStyle Width="10%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblRemarks" Runat="server"></asp:Label><BR>
										<asp:TextBox id="txtRemark" CssClass="listtxtbox" Width="150px" MaxLength="400" Runat="server"
											TextMode="MultiLine" Rows="2"></asp:TextBox>
										<asp:TextBox id="txtQ" runat="server" CssClass="lblnumerictxtbox" Width="6px"  contentEditable="false" 
											ForeColor="Red"></asp:TextBox><INPUT class="txtbox" id="hidCode" type="hidden" name="hidCode" runat="server">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="IM_FM_APPROVED_DATE" SortExpression="IM_FM_APPROVED_DATE"  readonly="True"  
									HeaderText="Approved Payment Date">
									<HeaderStyle Width="10%" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Left" />
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="POM_BILLING_METHOD" SortExpression="POM_BILLING_METHOD"
									 readonly="True"    HeaderText="POM_BILLING_METHOD"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4">
						<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save" CausesValidation="False"></asp:button>
						<asp:button id="cmdSubmit" runat="server" Width="96px" CssClass="button" Text="Verify"
							CausesValidation="False"></asp:button>
						<asp:button id="cmdApprove" runat="server" Width="96px" CssClass="button" Text="Verify"
							CausesValidation="False"></asp:button>
						<asp:button id="cmdMark" runat="server" Width="80px" CssClass="button" Text="Mark As Paid" CausesValidation="False"></asp:button>
						<INPUT class="button" id="cmdReset" onclick="ValidatorReset();" type="button" value="Reset" name="cmdReset" runat="server"> 
					    <INPUT id="hidMode" type="hidden" size="1" name="hidMode" runat="server"><INPUT id="hidIndex" type="hidden" size="1" name="hidIndex" runat="server">&nbsp;
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4">&nbsp;&nbsp;
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary>
						<INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary" runat="server">
						<INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl" runat="server"></TD>
				</TR>
				</TABLE>
		</form>
</body>
</html>