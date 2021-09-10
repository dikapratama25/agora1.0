<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PRApprDetail.aspx.vb" Inherits="eProcure.PRApprDetail_SEH" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>PR Approval</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<% Response.Write(Session("WheelScript"))%>
		<script language="javascript">
        <!--
        function confirmReject()
        {	
	        result=resetSummary(1,0);	
	        ans=confirm("Are you sure that you want to reject this PR ?");
	        //alert(ans);
	        //return false;	
	        if (ans){		
	            document.getElementById("cmdRejectPR").style.display= "none";
	            document.getElementById("cmdAppPR").style.display= "none";	
	            document.getElementById("cmdHoldPR").style.display= "none";				
		        return result;
		        }
	        else
	            document.getElementById("cmdRejectPR").style.display= "";
	            document.getElementById("cmdAppPR").style.display= "";
	            document.getElementById("cmdHoldPR").style.display= "";
		        return false;
        }
        function confirmApprove(action)
        {	
	        result=resetSummary(1,0);	
	        ans=confirm("Are you sure that you want to " + action + " this PR ?");
	        //alert(ans);
	        //return false;	
	        if (ans){		
	            document.getElementById("cmdAppPR").style.display= "none";
	            document.getElementById("cmdRejectPR").style.display= "none";
	            document.getElementById("cmdHoldPR").style.display= "none";		
		        return result;
		        }
	        else
	            document.getElementById("cmdAppPR").style.display= "";
		        document.getElementById("cmdRejectPR").style.display= "";
		        document.getElementById("cmdHoldPR").style.display= "";
		        return false;
        }
        //-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_SearchPRAO_tabs"))%>
			    <table class="alltable" id="Table10" cellspacing="0" cellpadding="0" border="0">
            <tr>
					<td class="linespacing2" colspan="5"></td>
			</tr>
			<tr>
                <td colspan="6">
	                <asp:label id="lblAction" runat="server"  CssClass="lblInfo"
	                        Text="Click the Approve PR button to approve the PR or Reject PR button to reject the PR."
	                ></asp:label>

                </td>
            </tr>
            <tr>
				<td class="linespacing2" colspan="4"></td>
		    </tr>
			</table>
			<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
	                <td class="tableheader" width="100%" colspan="5">&nbsp;Purchase Request Header</td>
                </tr>
			    <tr valign="top">
				    <td class="tablecol" align="left" width="20%">&nbsp;<strong>PR Number</strong>&nbsp;:</td>
				    <td class="TableInput" width="30%"><asp:label id="lblPRNo" runat="server" width="202px"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Status</strong>&nbsp;:</td>
				    <td class="TableInput" width="30%"><asp:label id="lblStatus" Runat="server"></asp:label></td><%--<td class="TableInput" width="30%"><asp:label id="lblDate" runat="server"></asp:label></td>--%>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol">
                        <strong>&nbsp;Requester Name</strong>&nbsp;:</td>
				    <td class="TableInput" width="25%"><asp:label id="lblReqName" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Attention To</strong> :&nbsp;</td>
				    <td class="TableInput" width="25%"><asp:label id="lblAtt" Runat="server"></asp:label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol">&nbsp;<strong>Requester Contact</strong>&nbsp;:</td>
				    <td class="TableInput" width="25%"><asp:label id="lblReqCon" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Currency</strong> :&nbsp;</td>
				    <td class="TableInput" width="25%"><asp:Label ID="lblCurr" runat="server"></asp:Label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol">&nbsp;<strong>Payment Term</strong>&nbsp;:</td>
				    <td class="TableInput" width="25%"><asp:label id="lblPayTerm" Runat="server"></asp:label></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>Payment Method</strong> :&nbsp;</td>
				    <td class="TableInput" width="25%"><asp:Label ID="lblPayMethod" runat="server"></asp:Label></td>
			    </tr>
			    <tr valign="top">
				    <td class="tablecol">
                        <strong>&nbsp;Internal Remarks</strong>&nbsp;:</td>
				    <td class="tableinput" width="25%"><asp:textbox id="txtInternal" ReadOnly="True" runat="server" CssClass="listtxtbox" width="100%" MaxLength="1000"
						    Rows="2" TextMode="MultiLine"></asp:textbox></td>
				    <td class="tablecol" style="width: 165px">&nbsp;<strong>External Remarks</strong>&nbsp;:</td>
				    <td class="tableinput" width="25%"><asp:textbox id="txtExternal" ReadOnly="True" runat="server" CssClass="listtxtbox" width="100%" MaxLength="1000"
						    Rows="2" TextMode="MultiLine"></asp:textbox></td>
			    </tr>   
			    
			    <tr valign="top">
					<td class="tablecol" valign="top" >&nbsp;<strong>Internal File(s) Attached</strong>&nbsp;:</td>
					<td class="TableInput" colspan="4"><asp:label id="lblFileInt" Runat="server"></asp:label></td>
				</tr>
			    
			    <tr valign="top">
					<td class="tablecol" valign="top" >&nbsp;<strong>External File(s) Attached</strong>&nbsp;:</td>
					<td class="TableInput" colspan="4"><asp:label id="lblFile" Runat="server"></asp:label></td>
				</tr>			
			    
			</table>
						<%--<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
								<td class="tableheader" colspan="4">&nbsp;Purchase Requisition Header</td>
							</tr>
							<tr valign="top">
								<td class="tablecol" width="25%">&nbsp;<strong>Purchase Requisition Number</strong>&nbsp;:</td>
								<td class="TableInput" width="25%" colspan="3"><asp:label id="lblPR" Runat="server">PR001</asp:label></td>
							</tr>
							<tr id="trAdmin" valign="top" runat="server">
								<td class="tablecol" width="25%" style="height: 19px">&nbsp;<strong>Requestor Name</strong>&nbsp;:</td>
								<td class="TableInput" width="25%" colspan="3" style="height: 19px"><asp:label id="lblReqName" Runat="server">MOO</asp:label></td>
							</tr>
							<tr valign="top">
								<td class="tablecol" width="25%">&nbsp;<strong>Submission Date</strong>&nbsp;:</td>
								<td class="TableInput" width="25%" colspan="3"><asp:label id="lblPRDate" Runat="server">MOO</asp:label></td>
							</tr>
							<tr valign="top">
								<td class="tablecol" width="25%">&nbsp;<strong>Status</strong>&nbsp;:</td>
								<td class="TableInput" width="25%" colspan="3"><asp:label id="lblStatus" Runat="server"></asp:label></td>
							</tr>
							<tr valign="top">
								<td class="tablecol" valign="top" width="25%">&nbsp;<strong>Billing Address</strong>&nbsp;:</td>
								<td class="TableInput" width="25%" colspan="5"><asp:label id="lblBillAddr" Runat="server">
								12, Jalan 13/4(Bersatu)<br>
								46200 Petaling Jaya<br>
								Selangor Malaysia<br>
								</asp:label></td>
							<tr valign="top">
								<td class="tablecol" valign="top" width="25%">&nbsp;<strong>For Internal Use</strong>&nbsp;:</td>
								<td class="TableInput" width="25%" colspan="3"><asp:label id="lblInternalRemark" Runat="server">
								TEST TEST TEST TEST TEST<br>
								TEST TEST TEST TEST TEST
								</asp:label></td>
							</tr>
							<tr valign="top">
								<td class="tablecol" width="25%">&nbsp;<strong>Vendor</strong>&nbsp;:</td>
								<td class="TableInput" width="25%" colspan="3"><asp:label id="lblVendor" Runat="server">Kompakar Group of companies</asp:label></td>
							</tr>
							<tr valign="top">
								<td class="tablecol" valign="top" width="25%" style="height: 38px">&nbsp;<strong>Remarks</strong>&nbsp;:</td>
								<td class="TableInput" width="25%" colspan="3" style="height: 38px"><asp:label id="lblPRRemark" Runat="server">
								TEST TEST TEST TEST TEST<br>
								TEST TEST TEST TEST TEST<br>
								</asp:label></td>
							</tr>
							<tr valign="top">
								<td class="tablecol" valign="top" width="25%">&nbsp;<strong>File(s) Attached</strong>&nbsp;:</td>
								<td class="TableInput" width="25%" colspan="3"><asp:label id="lblFile" Runat="server"></asp:label></td>
							</tr>
							<tr valign="top">
								<td class="tablecol" width="25%">&nbsp;<strong>Currency</strong>&nbsp;:</td>
								<td class="TableInput" width="25%"><asp:label id="lblCurrency" Runat="server">MOO</asp:label></td>
								<td class="tablecol" width="25%">&nbsp;<strong>Exchange Rate</strong>&nbsp;:</td>
								<td class="TableInput"><asp:label id="lblExRate" Runat="server">MOO</asp:label></td>
							</tr>
							<tr valign="top">
								<td class="tablecol" width="25%">&nbsp;<strong>Payment Terms</strong>&nbsp;:</td>
								<td class="TableInput" width="25%"><asp:label id="lblPT" Runat="server">MOO</asp:label></td>
								<td class="tablecol" width="25%">&nbsp;<strong>Payment&nbsp;Method</strong>&nbsp;:</td>
								<td class="TableInput"><asp:label id="lblPM" Runat="server">MOO</asp:label></td>
							</tr>
							<tr valign="top">
								<td class="tablecol" width="25%">&nbsp;<strong>Shipment Terms</strong>&nbsp;:</td>
								<td class="TableInput" width="25%"><asp:label id="lblST" Runat="server">MOO</asp:label></td>
								<td class="tablecol" width="25%">&nbsp;<strong>Shipment Mode</strong>&nbsp;:</td>
								<td class="TableInput"><asp:label id="lblSM" Runat="server">MOO</asp:label></td>
							</tr>
						</table>--%>
						
				<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">		
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="tableheader">Approval Workflow</td>
				</tr>
				<tr>
					<td width="100%"><asp:datagrid id="dtgAppFlow" runat="server" AutoGenerateColumns="False" width="100%">
							<Columns>
								<asp:BoundColumn DataField="PRA_SEQ" HeaderText="Level">
									<HeaderStyle width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
									<HeaderStyle width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
									<HeaderStyle width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRA_APPROVAL_TYPE" HeaderText="Approval Type">
									<HeaderStyle width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRA_ACTION_DATE" HeaderText="Action Date">
									<HeaderStyle width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRA_AO_REMARK" HeaderText="Remarks">
									<HeaderStyle width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Attachment">
									<HeaderStyle Width="20%"></HeaderStyle>
									<ItemTemplate>
										<asp:DataList id="DataList1" runat="server" DataSource='<%# CreateFileLinks( DataBinder.Eval( Container.DataItem, "PRA_AO") , DataBinder.Eval( Container.DataItem, "PRA_A_AO") , DataBinder.Eval( Container.DataItem, "PRA_SEQ" ) ) %>' ShowFooter="False" Width="100%" BorderColor=#0000ff ShowHeader="False">
											<ItemTemplate>
												<%# DataBinder.Eval( Container.DataItem ,"Hyperlink") %>
											</ItemTemplate>
										</asp:DataList>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<%--<tr id="trConsolidator" runat="server">
					<td>
						<table class="alltable" cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
								<td class="emptycol">&nbsp;</td>
							</tr>
							<tr valign="top">
								<td class="tablecol">&nbsp;<strong>Consolidator</strong>&nbsp;:&nbsp;<asp:label id="lblConsolidator" Runat="server"></asp:label></td>
							</tr>
						</table>
					</td>
				</tr>--%>
			 </table>
			<table class="AllTable" id="tblSearchResult" cellspacing="0" cellpadding="0" border="0"
				runat="server">
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtgPRList" runat="server" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn DataField="PRD_PR_LINE" HeaderText="Line">
									<HeaderStyle width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CM_COY_NAME" HeaderText="Vendor">
									<HeaderStyle width="5%"></HeaderStyle>									
								</asp:BoundColumn>
								<%--<asp:BoundColumn DataField="PRD_B_ITEM_CODE" HeaderText="Buyer Item Code">
									<HeaderStyle width="5%"></HeaderStyle>
								</asp:BoundColumn>--%>
								<asp:BoundColumn DataField="PRD_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle width="14%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn>
									<HeaderTemplate>
										(GL Code) GL Description
									</HeaderTemplate>
									<ItemTemplate>
										<%# GenerateGLString( DataBinder.Eval( Container.DataItem , "PRD_B_GL_CODE" ) , DataBinder.Eval( Container.DataItem , "CBG_B_GL_DESC" )  ) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PRD_B_CATEGORY_CODE" HeaderText="Category Code"></asp:BoundColumn>
								<%--<asp:BoundColumn DataField="PRD_B_TAX_CODE" HeaderText="Tax Code"></asp:BoundColumn>--%>
								
								<asp:BoundColumn DataField="ASSET_CODE" HeaderText="Asset Code">
									<HeaderStyle width="16%"></HeaderStyle>
								</asp:BoundColumn>
								
								<asp:BoundColumn DataField="PRD_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle width="19%"></HeaderStyle>
								</asp:BoundColumn>
								<%--<asp:TemplateColumn HeaderText="MOQ">
									<HeaderStyle width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label runat="server" Text='1' ID="Label1"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>--%>
								<%--<asp:TemplateColumn HeaderText="MPQ">
									<HeaderStyle width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label runat="server" Text='1' ID="Label2"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>--%>
								<%--<asp:BoundColumn DataField="PRD_RFQ_QTY" HeaderText="RFQ Qty">
									<HeaderStyle width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>--%>
								<%--<asp:BoundColumn DataField="PRD_QTY_TOLERANCE" HeaderText="Quotation Qty Tolerance">
									<HeaderStyle width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>--%>
								<asp:BoundColumn DataField="PRD_ORDERED_QTY" HeaderText="Quantity">
									<HeaderStyle width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Datafield="CT_NAME" HeaderText="Commodity <br>Type">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DELIVERY_TERM" HeaderText="Delivery <br>Term">
									<HeaderStyle width="3%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_UOM" HeaderText="UOM">
									<HeaderStyle width="3%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_CURRENCY_CODE" HeaderText="Currency">
									<HeaderStyle width="3%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_UNIT_COST" HeaderText="Unit Price">
									<HeaderStyle width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_UNIT_COST" HeaderText="Sub Total">
									<HeaderStyle width="7%" HorizontalAlign="Right"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AMOUNT" HeaderText="Amount" Visible="false">
									<HeaderStyle width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<%--<asp:BoundColumn DataField="PRD_UNIT_COST" HeaderText="Sub Total">
									<HeaderStyle width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>--%>								
								<asp:BoundColumn HeaderText="Tax">
									<HeaderStyle width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="GST Rate">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="GST Amount">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="GST Tax Code (Purchase)">
									<HeaderStyle Width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Budget Account">
									<HeaderStyle width="3%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Delivery Address">
									<HeaderStyle width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Item Type">
									<HeaderStyle width="5%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_LEAD_TIME" HeaderText="Lead Time <br>(Days)">
									<HeaderStyle width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_ETD" HeaderText="Est. Date of Delivery">
									<HeaderStyle width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_WARRANTY_TERMS" HeaderText="Warranty Terms (mths)">
									<HeaderStyle width="2%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRD_REMARK" HeaderText="Remarks">
									<HeaderStyle width="7px"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td>
						<table class="AllTable" id="tblApproval" cellspacing="0" cellpadding="0" border="0" runat="server">
							<tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<tr>
								<td valign="middle">&nbsp;<strong>Remarks</strong>&nbsp;:</td>
								<td><asp:textbox id="txtRemark" Runat="server" width="600px" TextMode="MultiLine" MaxLength="1000"
										Rows="3" CssClass="listtxtbox"></asp:textbox></td>
							</tr>
							<tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<%--<tr valign="top">
								<td style="HEIGHT: 17px">&nbsp;<strong>File Attachment </strong>&nbsp;:</td>
								<td style="HEIGHT: 22px" colspan="3" rowSpan="2"><INPUT class="button" id="File1" style="width: 222px; HEIGHT: 17px" type="file" size="17"
										name="uploadedFile3" runat="server">&nbsp;<asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button><asp:label id="lblFileAO" Runat="server" Visible="False"></asp:label></td>
							</tr>--%>
							<%--<tr valign="top">
								<td>&nbsp;<asp:label id="lblAttach" runat="server" width="176px" CssClass="div">Recommended file size is 300KB</asp:label></td>
							</tr>--%>
							<%--<tr valign="top">
								<td style="HEIGHT: 19px">&nbsp;<strong>File Attached </strong>:</td>
								<td style="HEIGHT: 19px" colspan="3"><asp:panel id="pnlAttach" runat="server"></asp:panel></td>
							</tr>--%>
							<tr valign="top">
								<td style="HEIGHT: 17px">&nbsp;<strong>Internal Attachment </strong>&nbsp;:</td>
								<td style="HEIGHT: 22px" colspan="4" rowSpan="2"><INPUT class="button" id="File1" style="width: 448px; HEIGHT: 17px" type="file"
										name="uploadedFile3" runat="server">&nbsp;<asp:button id="cmdUpload" runat="server" CssClass="button" Text="Upload" CausesValidation="False"></asp:button><asp:label id="lblFileAO" Runat="server" Visible="False"></asp:label></td>
							</tr>
							<tr valign="top">
								<td>&nbsp;<asp:label id="lblAttach" runat="server" width="176px"  CssClass="small_remarks">Recommended file size is <%  Response.Write(Session("FileSize"))%> KB</asp:label></td>
							</tr>
							<tr valign="top">
								<td style="HEIGHT: 19px">&nbsp;<strong>Internal File Attached </strong>:</td>
								<td style="HEIGHT: 19px" colspan="3"><asp:panel id="pnlAttach" runat="server"></asp:panel></td>
							</tr>
							<tr valign="top">
								<td colspan="1" style="height: 38px"><br>
								</td>
								<td style="height: 38px"></td>
							</tr>
							<tr id="trButton" runat="server">
								<td colspan="2"><asp:button id="cmdAppPR" runat="server" width="100px" CssClass="button" Text="Approve PR"></asp:button><asp:button id="cmdRejectPR" runat="server" width="100px" CssClass="button" Text="Reject PR"></asp:button><asp:button id="cmdHoldPR" runat="server" width="100px" CssClass="button" Text="Hold PR"></asp:button><INPUT class="button" id="cmdClear" onclick="document.forms(0).txtRemark.value=''" type="button" 
										value="Clear" name="cmdClear" runat="server"></td>
							</tr>
							<tr id="trMessage" runat="server">
								<td colspan="2"><asp:label id="lblMsg" Runat="server" CssClass="ErrorMsg">The amount has exceeded your approval limit.</asp:label></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<table id="tblBuyer" cellspacing="0" cellpadding="0" border="0" runat="server">
							<tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<tr>
								<td valign="middle">&nbsp;<strong>
										<asp:label id="lblRemarkCR" runat="server" Visible="False">Cancel Remarks :</asp:label></strong></td>
								<td><asp:textbox id="txtRemarkCR" Runat="server" width="600px" TextMode="MultiLine" MaxLength="1000"
										Rows="3" CssClass="listtxtbox" Visible="False"></asp:textbox></td>
							</tr>
							<tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<tr>
								<td class="emptycol" colspan="2"><asp:button id="cmdDup" runat="server" width="100px" CssClass="button" Text="Duplicate PR"></asp:button><asp:button id="cmdCancel" runat="server" width="100px" CssClass="button" Text="Cancel PR" Visible="False"></asp:button>&nbsp;&nbsp;&nbsp;
								</td>
							</tr>
						</table>
						<P><asp:textbox id="TextBox1" runat="server" Visible="False"></asp:textbox></P>
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol"><asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><INPUT id="hidSummary" style="width: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"><INPUT id="hidControl" style="width: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol">
						<P><asp:hyperlink id="lnkBack" Runat="server">
								<strong>&lt; Back</strong></asp:hyperlink></P>
					</td>
				</tr>
			</table>
			<asp:button id="cmdPrReport" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 1200px"
				runat="server" Text="View PR Report" Visible="False"></asp:button></form>
	</body>
</HTML>
