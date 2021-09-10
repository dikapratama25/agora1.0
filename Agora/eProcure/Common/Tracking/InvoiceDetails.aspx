<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InvoiceDetails.aspx.vb" Inherits="eProcure.InvoiceDetails" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>InvDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sCollapseUp As String = dDispatcher.direct("Plugins/Images", "collapse_up.gif")
		    Dim sCollapseDown As String = dDispatcher.direct("Plugins/Images", "collapse_down.gif")
        </script>
        
		<% Response.Write(Session("JQuery")) %>
		<% Response.Write(Session("WheelScript"))%>
		
		<script type="text/javascript">
		<!--  
		
		$(document).ready(function(){
        $('#cmdSave').click(function() {
        if(document.getElementById("cmdSave"))
        { document.getElementById("cmdSave").style.display= "none"; }
        if(document.getElementById("cmdMark"))
        { document.getElementById("cmdMark").style.display= "none"; }
        if(document.getElementById("cmdAppInv"))
        { document.getElementById("cmdAppInv").style.display= "none"; }
        if(document.getElementById("cmdHoldInv"))
        { document.getElementById("cmdHoldInv").style.display= "none"; }
        if(document.getElementById("cmdRejectInv"))
        { document.getElementById("cmdRejectInv").style.display= "none"; }
        if(document.getElementById("cmdVerify"))
        { document.getElementById("cmdVerify").style.display= "none"; }
        });
        $('#cmdRejectInv').click(function() {
        if(document.getElementById("cmdSave"))
        { document.getElementById("cmdSave").style.display= "none"; }
        if(document.getElementById("cmdMark"))
        { document.getElementById("cmdMark").style.display= "none"; }
        if(document.getElementById("cmdAppInv"))
        { document.getElementById("cmdAppInv").style.display= "none"; }
        if(document.getElementById("cmdHoldInv"))
        { document.getElementById("cmdHoldInv").style.display= "none"; }
        if(document.getElementById("cmdRejectInv"))
        { document.getElementById("cmdRejectInv").style.display= "none"; }
        if(document.getElementById("cmdVerify"))
        { document.getElementById("cmdVerify").style.display= "none"; }
        });
        $('#cmdVerify').click(function() {
        if(document.getElementById("cmdSave"))
        { document.getElementById("cmdSave").style.display= "none"; }
        if(document.getElementById("cmdMark"))
        { document.getElementById("cmdMark").style.display= "none"; }
        if(document.getElementById("cmdAppInv"))
        { document.getElementById("cmdAppInv").style.display= "none"; }
        if(document.getElementById("cmdHoldInv"))
        { document.getElementById("cmdHoldInv").style.display= "none"; }
        if(document.getElementById("cmdRejectInv"))
        { document.getElementById("cmdRejectInv").style.display= "none"; }
        if(document.getElementById("cmdVerify"))
        { document.getElementById("cmdVerify").style.display= "none"; }
        });
        $('#cmdAppInv').click(function() {
        if(document.getElementById("cmdSave"))
        { document.getElementById("cmdSave").style.display= "none"; }
        if(document.getElementById("cmdMark"))
        { document.getElementById("cmdMark").style.display= "none"; }
        if(document.getElementById("cmdAppInv"))
        { document.getElementById("cmdAppInv").style.display= "none"; }
        if(document.getElementById("cmdHoldInv"))
        { document.getElementById("cmdHoldInv").style.display= "none"; }
        if(document.getElementById("cmdRejectInv"))
        { document.getElementById("cmdRejectInv").style.display= "none"; }
        if(document.getElementById("cmdVerify"))
        { document.getElementById("cmdVerify").style.display= "none"; }
        });
        $('#cmdHoldInv').click(function() {
        if(document.getElementById("cmdSave"))
        { document.getElementById("cmdSave").style.display= "none"; }
        if(document.getElementById("cmdMark"))
        { document.getElementById("cmdMark").style.display= "none"; }
        if(document.getElementById("cmdAppInv"))
        { document.getElementById("cmdAppInv").style.display= "none"; }
        if(document.getElementById("cmdHoldInv"))
        { document.getElementById("cmdHoldInv").style.display= "none"; }
        if(document.getElementById("cmdRejectInv"))
        { document.getElementById("cmdRejectInv").style.display= "none"; }
        if(document.getElementById("cmdVerify"))
        { document.getElementById("cmdVerify").style.display= "none"; }
        });
        $('#cmdMark').click(function() {
        if(document.getElementById("cmdSave"))
        { document.getElementById("cmdSave").style.display= "none"; }
        if(document.getElementById("cmdMark"))
        { document.getElementById("cmdMark").style.display= "none"; }
        if(document.getElementById("cmdAppInv"))
        { document.getElementById("cmdAppInv").style.display= "none"; }
        if(document.getElementById("cmdHoldInv"))
        { document.getElementById("cmdHoldInv").style.display= "none"; }
        if(document.getElementById("cmdRejectInv"))
        { document.getElementById("cmdRejectInv").style.display= "none"; }
        if(document.getElementById("cmdVerify"))
        { document.getElementById("cmdVerify").style.display= "none"; }
        });
        });

        function showHide(lnkdesc)
        {
             if (document.getElementById(lnkdesc).style.display == 'none')
             {
	              document.getElementById(lnkdesc).style.display = '';
	              document.getElementById("Image1").src = '<% Response.Write(sCollapseUp)%>';
             } 
             else 
             {
	              document.getElementById(lnkdesc).style.display = 'none';
	              document.getElementById("Image1").src = '<% Response.Write(sCollapseDown)%>';
             }
        }
        
        function showHide2(lnkdesc)
        {
             if (document.getElementById(lnkdesc).style.display == 'none')
             {
	              document.getElementById(lnkdesc).style.display = '';
	              document.getElementById("Image2").src = '<% Response.Write(sCollapseUp)%>';
             } 
             else 
             {
	              document.getElementById(lnkdesc).style.display = 'none';
	              document.getElementById("Image2").src = '<% Response.Write(sCollapseDown)%>';
             }
        }
        
        function PromptMsg(msg){
            var result = confirm (msg,"OK", "Cancel");		
			if(result == true)
				Form1.hidresult.value = "1";
			else 
				Form1.hidresult.value = "0";
        }
            
		function PopWindow(myLoc)
		{
			window.open(myLoc,"Wheel","width=700,height=500,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
			return false;
		}	

		-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
		<%  Response.Write(Session("w_InvTracking_tabs"))%>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">			
				<tr>
					<td class="emptycol" align="left" colspan="4"></td>
				</tr>
				<tr>
					<td>
						<table class="alltable" id="Table4" cellspacing="0" cellpadding="0" width="400" border="0">
							<tr>
								<td class="tableheader" colspan="4">&nbsp;Invoice Header</td>
							</tr>
							<tr class="tableinput">
								<td valign="top" width="50%">
									<table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
										<tr class="tablecol">
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Vendor</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblComName" runat="server"></asp:label><br>
												<asp:label id="lblAddr" runat="server"></asp:label></td>
										</tr>
										<tr class="tablecol">
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Business 
												Reg. No.</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblBusRegNo" runat="server" Width="145px"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top">Tel</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblTel" runat="server" Width="145px"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top">Email</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblEmail" runat="server"></asp:label></td>
										</tr>
										<tr class="tablecol">
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Bill To</td>
											<td class="tablecol " valign="top" align="center" width="4%">:</td>
											<td class="tableinput " valign="top" width="60%"><asp:label id="lblBCoyName" runat="server"></asp:label><br>
												<asp:label id="lblBillTo" runat="server"></asp:label></td>
										</tr>
										<tr>
										    <td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">File(s) Attached</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblFileAttach" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Vendor 
												Remarks</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblVenRemarks" runat="server"></asp:label></td>
										</tr>
									</table>
								</td>
								<td class="emptycol" style="WIDTH: 220px" width="220">
									<table class="alltable" id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Vendor Code</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblVendorCode" runat="server"></asp:label><br /><br /></td>
										</tr>
										<tr>
											<td class="tablecol" style="FONT-WEIGHT: bold" valign="top" width="36%">Invoice No.</td>
											<td class="tablecol" valign="top" align="center" width="4%">:</td>
											<td class="tableinput" valign="top" width="60%"><asp:label id="lblInvNo" runat="server" Font-Bold="True"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top" noWrap style="height: 19px"><strong>Date</strong>
											</td>
											<td class="tablecol" valign="top" align="center" style="height: 19px">:</td>
											<td class="tableinput" valign="top" style="height: 19px"><asp:label id="lblDate" runat="server"></asp:label></td>
										</tr>
										<tr id="tr_curr" runat="server">
											<td class="tablecol" valign="top"><strong>Currency Code</strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblCurr" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top"><strong>Vendor Ref.</strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblOurRef" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top"><strong>PAMB Ref.</strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblYourRef" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top"><strong>Payment Terms</strong>
											</td>
											<td class="tablecol" valign="top" align="center" width="5%">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblPayTerm" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top" noWrap><strong>Payment Method </strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblPayMethod" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top" noWrap><strong>Shipment&nbsp;Terms&nbsp; </strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblShipType" runat="server"></asp:label></td>
										</tr>
										<tr>
											<td class="tablecol" valign="top"><strong>Shipment Mode</strong></td>
											<td class="tablecol" valign="top" align="center" width="5%">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblShipMode" runat="server"></asp:label></td>
										</tr>
										<tr id="tr_dt" runat="server">
											<td class="tablecol" valign="top"><strong>Delivery Term</strong>
											</td>
											<td class="tablecol" valign="top" align="center">:</td>
											<td class="tableinput" valign="top"><asp:label id="lblDelTerm" runat="server"></asp:label></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td class="emptycol"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="EMPTYCOL"></td>
				</tr>
				<tr>
					<td class="tableheader">Approval Workflow</td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtgAppFlow" runat="server">
							<Columns>
								<asp:BoundColumn DataField="FA_SEQ" HeaderText="Level">
									<HeaderStyle Width="1%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
									<HeaderStyle Width="29%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
									<HeaderStyle Width="20%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FA_APPROVAL_TYPE" HeaderText="Approval Type">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FA_ACTION_DATE" HeaderText="Approved Date">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FA_AO_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="25%"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="EMPTYCOL"></td>
				</tr>
				<tr>
					<td><asp:datagrid id="dtg_invDetail" runat="server" AutoGenerateColumns="False">
							<Columns>
								<asp:BoundColumn HeaderText="Line">
									<HeaderStyle HorizontalAlign="Right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								
                                <%--Jules 2018.10.25 U00019--%>
                                <%--Jules 2018.05.16 - PAMB Scrum 3--%>                               
                                <%--<asp:TemplateColumn SortExpression="FUNDTYPE" HeaderText="Fund Type">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:DropDownList id="cboFundType" Width="100px" CssClass="ddl" Runat="server"></asp:DropDownList>										
                                        <asp:Label id="lblFundType" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>                                 --%>
                                <asp:TemplateColumn SortExpression="FUNDTYPE" HeaderText="Fund Type (L1)">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:label id="txtFundType" Runat="server" ></asp:label>
										<asp:textbox id="hidFundType" Runat="server" style="display:none;"></asp:textbox>
										<input class="button" id="cmdFundType" style="WIDTH: 15px; HEIGHT: 18px" type="button"
											value="&nbsp;>&nbsp;" name="cmdFundType" runat="server" Width="30px"  />											
									</ItemTemplate>
								</asp:TemplateColumn>
                                <%--End modification--%>

                                <asp:BoundColumn DataField="PERSONCODE" HeaderText="Person Code (L9)">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="PROJECTCODE" HeaderText="Project / ACR (L8) Code">
									<HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
                                <%--End modification.--%>

								<asp:BoundColumn DataField="ID_PRODUCT_DESC" HeaderText="Item Description">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								
								<asp:BoundColumn DataField="ID_ASSET_CODE" HeaderText="Asset Code">
									<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								
								<asp:BoundColumn DataField="ID_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_RECEIVED_QTY" HeaderText="Qty">
									<HeaderStyle HorizontalAlign="Right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_UNIT_COST" HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="Right" Width="13%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Total">
									<HeaderStyle HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_GST" SortExpression="ID_GST" HeaderText="Tax">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GST_RATE" SortExpression="GST_RATE" HeaderText="SST Rate">
									<HeaderStyle HorizontalAlign="Left" Width="10%" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_GST" SortExpression="ID_GST" HeaderText="SST Amount">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="SST Tax Code (Purchase)">
								    <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
									    <asp:DropDownList id="ddlTaxCode" runat="server" CssClass="ddl" Width="100%"></asp:DropDownList>
									    <asp:Label id="lblTaxCode" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="ID_WARRANTY_TERMS" HeaderText="Warranty Terms">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="IM_SHIP_AMT" visible="false">
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ID_GST_RATE" visible="false">
								</asp:BoundColumn>
                                <%--Jules 2018.05.16 - PAMB Scrum 3--%>
                                <asp:BoundColumn DataField="ID_B_GL_CODE" visible="false">
								</asp:BoundColumn>
                                <%--End modification.--%>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
				    <td>
				        <div id="div2" runat="server" style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide2('CnLine')">
			                <asp:label id="Label1" runat="server">Credit Note/Credit Advice Summary</asp:label>
                            <asp:Image ID="Image2" runat="server" ImageUrl="#" />
                        </div>
                        <div id="CnLine" style="display:inline; width:100%;" runat="server">
			                <table class="alltable" id="Table2" border="0" width="100%" cellspacing="0">
				                <tr>
					                <td><asp:datagrid id="dtgCn" runat="server"  CssClass="Grid" AutoGenerateColumns="False" Width="100%">
							                <Columns>
							                    <asp:TemplateColumn SortExpression="CNM_CN_NO" HeaderText="Credit Note No.">
									                <HeaderStyle Width="20%" HorizontalAlign="Left"></HeaderStyle>
									                    <ItemTemplate>
										                    <asp:HyperLink Runat="server" ID="lnkCnNo"></asp:HyperLink>
									                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
								                </asp:TemplateColumn>
								                <asp:BoundColumn DataField="CNM_CREATED_DATE" SortExpression="CNM_CREATED_DATE" HeaderText="Credit Note Creation Date">
									                <HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									                <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                </asp:BoundColumn>
								                <asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" HeaderText="Credit Note Created By">
									                <HeaderStyle Width="20%"></HeaderStyle>
								                </asp:BoundColumn>    
								                <asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status">
									                <HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									                <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                </asp:BoundColumn>
								                <asp:BoundColumn DataField="AMOUNT" SortExpression="AMOUNT" HeaderText="Amount">
									                <HeaderStyle HorizontalAlign="Right" Width="20%"></HeaderStyle>
									                <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundColumn>             								
							                </Columns>
						                </asp:datagrid>
						            </td>
				                </tr>
			                </table>
	                    </div> 		
                    </td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
				    <td>
				        <div id="div1" runat="server" style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide('DnLine')">
			                <asp:label id="Label30" runat="server">Debit Note/Debit Advice Summary</asp:label>
                            <asp:Image ID="Image1" runat="server" ImageUrl="#" />
                        </div>
                        <div id="DnLine" style="display:inline; width:100%;" runat="server">
			                <table class="alltable" id="Table15" border="0" width="100%" cellspacing="0">
				                <tr>
					                <td><asp:datagrid id="dtgDn" runat="server"  CssClass="Grid" AutoGenerateColumns="False" Width="100%">
							                <Columns>
							                    <asp:TemplateColumn SortExpression="DNM_DN_NO" HeaderText="Debit Note No.">
									                <HeaderStyle Width="20%" HorizontalAlign="Left"></HeaderStyle>
									                    <ItemTemplate>
										                    <asp:HyperLink Runat="server" ID="lnkDnNo"></asp:HyperLink>
									                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
								                </asp:TemplateColumn>
								                <asp:BoundColumn DataField="DNM_CREATED_DATE" SortExpression="DNM_CREATED_DATE" HeaderText="Debit Note Creation Date">
									                <HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									                <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                </asp:BoundColumn>
								                <asp:BoundColumn DataField="UM_USER_NAME" SortExpression="UM_USER_NAME" HeaderText="Debit Note Created By">
									                <HeaderStyle Width="20%"></HeaderStyle>
								                </asp:BoundColumn> 
								                <asp:BoundColumn DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderText="Status">
									                <HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									                <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                </asp:BoundColumn>
								                <asp:BoundColumn DataField="AMOUNT" SortExpression="AMOUNT" HeaderText="Amount">
									                <HeaderStyle HorizontalAlign="Right" Width="20%"></HeaderStyle>
									                <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundColumn>               								
							                </Columns>
						                </asp:datagrid>
						            </td>
				                </tr>
			                </table>
	                    </div> 		
                    </td>
				</tr> 
				<tr>
					<td>
						<table class="AllTable" id="tblApproval" cellspacing="0" cellpadding="0" border="0" runat="server">
							<tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<tr>
								<td valign="middle" style="width: 197px">&nbsp;<strong>Remarks</strong>&nbsp;:</td>
								<td>
									<asp:textbox id="txtRemark" Runat="server" CssClass="listtxtbox" Rows="3" MaxLength="1000" TextMode="MultiLine"
										Width="600px"></asp:textbox></td>
							</tr>
							<tr>
								<td class="emptycol" colspan="2"></td>
							</tr>
							<tr id="Tr1" runat="server">
								<td colspan="2">
									<asp:button id="cmdSave" runat="server" CssClass="button" Text="Save" ></asp:button>
									<asp:button id="cmdMark" runat="server" CssClass="button" Text="Mark as Paid" Width="100px"></asp:button>
									<asp:button id="cmdAppInv" runat="server" CssClass="button" Width="100px" Text="Approve Invoice"></asp:button>
									<asp:button id="cmdHoldInv" runat="server" CssClass="button" Width="100px" Text="Hold Invoice" Visible="False"></asp:button>
									<asp:button id="cmdRejectInv" runat="server" CssClass="button" Width="100px" Text="Reject Invoice"
										Visible="False"></asp:button>
                                    <asp:Button ID="cmdVerify" runat="server" CssClass="button" Text="Reject Invoice"
                                        Visible="False" Width="100px" />
                                    <asp:button id ="btnHidden" runat="server" style="display:none" onclick="btnHidden_Click"></asp:button>
                                    <input id="hidresult" type="hidden" name="hidresult" runat="server"/></td>
							</tr>
							<tr>
								<td class="emptycol" style="width: 197px"></td>
							</tr>
							<tr id="trMessage" runat="server">
								<td colspan="2">
									<asp:label id="lblMsg" Runat="server" CssClass="ErrorMsg"></asp:label></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr id="trPreview" runat="server">
					<td colspan="2">
						<asp:button id="cmdPreviewInvoice" runat="server" CssClass="button" Width="100px" Text="View Invoice" CausesValidation="False" UseSubmitBehavior="False"></asp:button>
					</td>
				</tr>
				<tr>
					<td class="emptycol"></td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4"><a id="back" href="#" runat="server"><strong>&lt; Back</strong></a></td>
				</tr>
			</table>
		</form>
	</body>
</html>
