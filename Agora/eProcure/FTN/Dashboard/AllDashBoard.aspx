<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AllDashBoard.aspx.vb" Inherits="eProcure.AllDashBoardFTN" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<script runat="server">
    Dim dDispatcher As New AgoraLegacy.dispatcher
    Dim sCSS As String = "<link href=""" & dDispatcher.direct("Plugins/CSS", "Styles.css") & """ type=""text/css"" rel=""stylesheet"" />"
</script>
<% Response.write(Session("JQuery")) %> 
<%--<script type="text/javascript">

		$(document).ready(function() {

			$('#Label5').click(function () {

				$('#collapsePM').slideToggle('medium');

			});
});
</script>--%>
<% Response.Write(sCSS)%>

</head>
<body style="width:95%;">
    <form id="form1" runat="server">
        <div class="db_title">
            <asp:Label ID="lblTitle" text="Dashboard" runat="server"></asp:Label>
    </div> 
   <%-- Purchasing Manager--%>
          <%-- <div id="collapsePM">--%>
            <div id="divPendingMyAppPR" style="display:none;" runat="server">
                <div class="db_wrapper">
                    <div class="db_tbl_hd" > 
                        <asp:Label ID="PendingMyAppPR" runat="server"></asp:Label>
                    </div>
                    <asp:DataGrid ID="dtgPendingMyAppPR" runat="server" width="100%" OnSortCommand="SortCommandPendingMyAppPR_Click">
                        <Columns>
                            <asp:TemplateColumn SortExpression="PR Number" HeaderText="PR Number">
						        <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
						        <ItemStyle HorizontalAlign="Left"></ItemStyle>
						        <ItemTemplate>
							        <asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>
						        </ItemTemplate>
					        </asp:TemplateColumn>
					        <asp:BoundColumn DataField="Buyer" HeaderText="Buyer" SortExpression="Buyer">
                                <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Buyer Department" HeaderText="Buyer Department" SortExpression="Buyer Department">
                                <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                 <ItemStyle HorizontalAlign="Left" />
                           </asp:BoundColumn>
                            <asp:BoundColumn DataField="Submitted Date" HeaderText="Submitted Date" SortExpression="Submitted Date">
                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PRM_PR_Index" HeaderText="PRM_PR_Index" SortExpression="PRM_PR_Index" Visible="False">
                            </asp:BoundColumn>                      
                        </Columns>
                    </asp:DataGrid>
                </div>
            </div>
            <div id="divPendingConvPR" style="display:none;" runat="server">
            <div class="db_wrapper">
                <div class="db_tbl_hd" > 
                    <asp:Label ID="PendingConvPR" runat="server"></asp:Label>
                </div>
                <asp:DataGrid ID="dtgPendingConvPR" runat="server" width="100%" OnSortCommand="SortCommandPendingConvPR_Click">
                    <Columns>
                        <asp:TemplateColumn SortExpression="PR Number" HeaderText="PR Number">
						    <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Left"></ItemStyle>
						    <ItemTemplate>
							    <asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>
						    </ItemTemplate>
					    </asp:TemplateColumn>
					    <asp:BoundColumn DataField="Buyer" HeaderText="Buyer" SortExpression="Buyer">
                            <HeaderStyle HorizontalAlign="Left" Width="35%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Buyer Department" HeaderText="Buyer Department" SortExpression="Buyer Department">
                            <HeaderStyle HorizontalAlign="Left" Width="35%" />
                             <ItemStyle HorizontalAlign="Left" />
                       </asp:BoundColumn>
                        <asp:BoundColumn DataField="Approved Date" HeaderText="Approved Date" SortExpression="Approved Date">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="PRM_PR_Index" HeaderText="PRM_PR_Index" SortExpression="PRM_PR_Index" Visible="False">
                        </asp:BoundColumn>                      
                    </Columns>
                </asp:DataGrid>
            </div>
        </div>                
          <div id="divPendingMyAppr" style="display:none;" runat="server">
        <div class="db_wrapper">
            <div class="db_tbl_hd" > 
                <asp:Label ID="POPendingMyAppr" runat="server"></asp:Label>
            </div>
            <asp:DataGrid ID="dtgPendingMyAppr" runat="server" width="100%" OnSortCommand="SortCommandPendingMyAppr_Click">
                <Columns>
                    <asp:TemplateColumn SortExpression="PO Number" HeaderText="PO Number">
						<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink Runat="server" ID="lnkPONum"></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:BoundColumn DataField="Submitted Date" HeaderText="Submitted Date" SortExpression="Submitted Date">
                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="POM_PO_Index" HeaderText="POM_PO_Index" SortExpression="POM_PO_Index" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Vendor Name" HeaderText="Vendor Name" SortExpression="Vendor Name">
                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
           <asp:BoundColumn DataField="Currency" HeaderText="Currency" SortExpression="Currency">
                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                         <ItemStyle HorizontalAlign="Left" />
                   </asp:BoundColumn>
                    <asp:BoundColumn DataField="Amount" HeaderText="Amount" SortExpression="Amount">
                        <HeaderStyle HorizontalAlign="Right" Width="15%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
        </div>
        </div>
        <div id="divOutstdPR" style="display:none;" runat="server">
            <div class="db_wrapper">
                <div class="db_tbl_hd" > 
                    <asp:Label ID="OutstdPR" runat="server"></asp:Label>
                </div>
                <asp:DataGrid ID="dtgOutstdPR" runat="server" width="100%" OnSortCommand="SortCommandOutstdPR_Click">
                    <Columns>
                        <asp:TemplateColumn SortExpression="PR Number" HeaderText="PR Number">
						    <HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
						    <ItemStyle HorizontalAlign="Left"></ItemStyle>
						    <ItemTemplate>
							    <asp:HyperLink Runat="server" ID="lnkPRNo"></asp:HyperLink>
						    </ItemTemplate>
					    </asp:TemplateColumn>
					    <asp:BoundColumn DataField="Creation Date" HeaderText="Creation Date" SortExpression="Creation Date">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Submission Date" HeaderText="Submission Date" SortExpression="Submission Date">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                             <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Approved Date" HeaderText="Approved Date" SortExpression="Approved Date">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Status" HeaderText="Status" SortExpression="Status">
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="PRM_PR_Index" HeaderText="PRM_PR_Index" SortExpression="PRM_PR_Index" Visible="False">
                        </asp:BoundColumn>                      
                    </Columns>
                </asp:DataGrid>
            </div>
        </div>
        <div id="divPendingApprPM" style="display:none;" runat="server">
        <div class="db_wrapper">
            <div class="db_tbl_hd" > 
                <asp:Label ID="POPendingApprPM" runat="server"></asp:Label>
            </div>
            <asp:DataGrid ID="dtgPendingApprPM" runat="server" width="100%" OnSortCommand="SortCommandPendingApprPM_Click">
                <Columns>
                    <asp:TemplateColumn SortExpression="PO Number" HeaderText="PO Number">
						<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink Runat="server" ID="lnkPONum"></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:BoundColumn DataField="Submitted Date" HeaderText="Submitted Date" SortExpression="Submitted Date">
                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="POM_PO_Index" HeaderText="POM_PO_Index" SortExpression="POM_PO_Index" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Vendor Name" HeaderText="Vendor Name" SortExpression="Vendor Name">
                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
           <asp:BoundColumn DataField="Currency" HeaderText="Currency" SortExpression="Currency">
                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                         <ItemStyle HorizontalAlign="Left" />
                   </asp:BoundColumn>
                    <asp:BoundColumn DataField="Amount" HeaderText="Amount" SortExpression="Amount">
                        <HeaderStyle HorizontalAlign="Right" Width="15%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
        </div>
        </div>
        <div id="divOutstdPO" style="display:none;" runat="server">
        <div class="db_wrapper">
            <div class="db_tbl_hd">
                      <asp:Label ID="OutstdPO" runat="server" ></asp:Label>
            </div>
            <asp:DataGrid ID="dtgOutstdPO" runat="server" width="100%" OnSortCommand="SortCommandOutstdPO_Click">
                <Columns>
                     <asp:TemplateColumn HeaderText="Status">
						<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:label Runat="server" ID="lblStatus"></asp:label>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:TemplateColumn SortExpression="PO Number" HeaderText="PO Number">
						<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink Runat="server" ID="lnkPONum2"></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:BoundColumn DataField="POM_PO_Index" HeaderText="POM_PO_Index" SortExpression="POM_PO_Index"
                        Visible="False">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Vendor Name" HeaderText="Vendor Name" SortExpression="Vendor Name">
                        <HeaderStyle HorizontalAlign="Left" Width="30%"/>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Accepted Date" HeaderText="Accepted Date" SortExpression="Accepted Date">
                        <HeaderStyle HorizontalAlign="Left" Width="14%"/>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Total PO Qty" HeaderText="Total PO Qty" SortExpression="Total PO Qty">
                        <HeaderStyle HorizontalAlign="Right" Width="12%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Outstanding PO Qty" HeaderText="Outstanding PO Qty" SortExpression="Outstanding PO Qty">
                        <HeaderStyle HorizontalAlign="Right" Width="15%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
        </div>
        </div>
        <div id="divOutstandingRFQ" style="display:none;" runat="server">
        <div class="db_wrapper">
            <div class="db_tbl_hd">
                  <asp:Label ID="OutstdRFQ" runat="server"></asp:Label>
            </div>
            <asp:DataGrid ID="dtgOutstandingRFQ" runat="server" width="100%" OnSortCommand="SortCommandOutstandingRFQ_Click">
                <Columns>
                     <asp:TemplateColumn HeaderText="Status">
						<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink Runat="server" ID="lnkRFQViewRes"></asp:HyperLink>
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
                        <HeaderStyle HorizontalAlign="Left" Width="45%" />
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
                </Columns>
            </asp:DataGrid>
            </div>
            </div>
            <div id="divInInv" style="display:none;" runat="server">
        <div class="db_wrapper">
            <div class="db_tbl_hd">
                 <asp:Label ID="InInv" runat="server"></asp:Label>
            </div>
            <asp:DataGrid ID="dtgInInv" runat="server" OnSortCommand="SortCommandInInv_Click">
                <Columns>
                    <asp:TemplateColumn SortExpression="Invoice Number" HeaderText="Invoice Number">
						<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink Runat="server" ID="lnkInv"></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:BoundColumn DataField="IM_INVOICE_INDEX" HeaderText="IM_INVOICE_INDEX" SortExpression="IM_INVOICE_INDEX" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Due Date" HeaderText="Due Date" SortExpression="Due Date">
                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Vendor Name" HeaderText="Vendor Name" SortExpression="Vendor Name">
                        <HeaderStyle HorizontalAlign="Left"  Width="40%"  />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Currency" HeaderText="Currency" SortExpression="Currency">
                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Amount" HeaderText="Amount" SortExpression="Amount">
                        <HeaderStyle HorizontalAlign="Right" Width="15%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>
                    
                </Columns>
            </asp:DataGrid>
        </div>
        </div>
        <div id="divInPendingPymt" style="display:none;" runat="server">
        <div class="db_wrapper">
            <div class="db_tbl_hd">
                 <asp:Label ID="InPendingPymt" runat="server"></asp:Label>
            </div>
            <asp:DataGrid ID="dtgInPendingPymt" runat="server" OnSortCommand="SortCommandInPendingPymt_Click">
                <Columns>
                    <asp:TemplateColumn SortExpression="Invoice Number" HeaderText="Invoice Number">
						<HeaderStyle HorizontalAlign="Left" Width="14%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink Runat="server" ID="lnkInvnum"></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:BoundColumn DataField="IM_INVOICE_INDEX" HeaderText="IM_INVOICE_INDEX" SortExpression="IM_INVOICE_INDEX" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Due Date" HeaderText="Due Date" SortExpression="Due Date">
                        <HeaderStyle HorizontalAlign="Left" Width="14%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Vendor Name" HeaderText="Vendor Name" SortExpression="Vendor Name">
                        <HeaderStyle HorizontalAlign="Left"  Width="50%"  />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Currency" HeaderText="Currency" SortExpression="Currency">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Amount" HeaderText="Amount" SortExpression="Amount">
                        <HeaderStyle HorizontalAlign="Right" Width="12%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>
                    
                </Columns>
            </asp:DataGrid>
        </div>
        </div>
        <div id="divInDOSK" style="display:none;" runat="server">
        <div class="db_wrapper">
            <div class="db_tbl_hd" > 
                <asp:Label ID="InDO" runat="server"></asp:Label>
            </div>
            <asp:DataGrid ID="dtgInDOSK" runat="server" width="100%" OnSortCommand="SortCommandInDOSK_Click">
                <Columns>
                    <asp:TemplateColumn SortExpression="DOM_DO_NO" HeaderText="DO Number">
						<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink Runat="server" ID="lnkDONum"></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:BoundColumn DataField="DOM_DO_DATE" HeaderText="DO Date" SortExpression="DOM_DO_DATE">
                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="DOM_DO_Index" HeaderText="DOM_DO_Index" SortExpression="DOM_DO_Index" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="POM_PO_NO" HeaderText="PO Number" SortExpression="POM_PO_NO">
                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="POM_PO_DATE" HeaderText="PO Date" SortExpression="POM_PO_DATE">
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Left" />
                        <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="CM_COY_NAME" HeaderText="Vendor Name" SortExpression="CM_COY_NAME">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
        </div>
        </div>
        <div id="divOutstandingPOVend" style="display:none;" runat="server">
        <div class="db_wrapper">
            <div class="db_tbl_hd">
                 <asp:Label ID="PO" runat="server"></asp:Label>
            </div>
            <asp:DataGrid ID="dtgOutstandingPOVend" runat="server" OnSortCommand="SortCommandOutstandingPOVend_Click">
                <Columns>
                     <asp:TemplateColumn HeaderText="Status">
						<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:label Runat="server" ID="lblStatus"></asp:label>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:TemplateColumn SortExpression="PO Number" HeaderText="PO Number">
						<HeaderStyle HorizontalAlign="Left" Width="13%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink Runat="server" ID="lnkPONum"></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:BoundColumn DataField="POM_PO_Index" HeaderText="POM_PO_Index" SortExpression="POM_PO_Index" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="PO Date" HeaderText="PO Date" SortExpression="PO Date">
                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Due Date" HeaderText="Due Date" SortExpression="Due Date">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Buyer Company" HeaderText="Purchaser Company" SortExpression="Buyer Company">
                        <HeaderStyle HorizontalAlign="Left"  />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="POM_PO_INDEX" HeaderText="POM_PO_INDEX" SortExpression="POM_PO_INDEX" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="POM_B_COY_ID" HeaderText="POM_B_COY_ID" SortExpression="POM_B_COY_ID" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Ordered Qty" HeaderText="Ordered Qty" SortExpression="Ordered Qty">
                        <HeaderStyle HorizontalAlign="Right" Width="12%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Overdue Qty" HeaderText="Oustanding Qty" SortExpression="Overdue Qty">
                        <HeaderStyle HorizontalAlign="Right" Width="12%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
        </div>
        </div>
        <div id="divOverduePOVend" style="display:none;" runat="server">
        <div class="db_wrapper">
            <div class="db_tbl_hd">
               <asp:Label ID="OverduePO" runat="server"></asp:Label>
            </div>
            <asp:DataGrid ID="dtgOverduePOVend" runat="server" OnSortCommand="SortCommandOverduePOVend_Click">
                <Columns>
                    <asp:TemplateColumn SortExpression="POM_PO_No" HeaderText="PO Number">
						<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink Runat="server" ID="lnkPONum2"></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:BoundColumn DataField="POM_PO_Index" HeaderText="POM_PO_Index" SortExpression="POM_PO_Index"
                        Visible="False">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="PO Date" HeaderText="PO Date" SortExpression="PO Date">
                        <HeaderStyle HorizontalAlign="Left" Width="13%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Due Date" HeaderText="Due Date" SortExpression="Due Date">
                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Buyer Company" HeaderText="Purchaser Company" SortExpression="Buyer Company">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="POM_PO_INDEX" HeaderText="POM_PO_INDEX" SortExpression="POM_PO_INDEX" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="POM_B_COY_ID" HeaderText="POM_B_COY_ID" SortExpression="POM_B_COY_ID" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Ordered Qty" HeaderText="Ordered Qty" SortExpression="Ordered Qty">
                        <HeaderStyle HorizontalAlign="Right" Width="12%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Overdue Qty" HeaderText="Overdue Qty" SortExpression="Overdue Qty">
                        <HeaderStyle HorizontalAlign="Right" Width="12%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
        </div>
        </div>
        <div id="divOutstandingRFQVend" style="display:none;" runat="server">
        <div class="db_wrapper">
            <div class="db_tbl_hd">
                  <asp:Label ID="OutstdRFQVend" runat="server"></asp:Label>
                </div>
            <asp:DataGrid ID="dtgOutstandingRFQVend" runat="server" OnSortCommand="SortCommandOutstandingRFQVend_Click">
                <Columns>
                    <asp:TemplateColumn SortExpression="RFQ Number" HeaderText="RFQ Number">
						<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink Runat="server" ID="lnkRFQNum"></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:BoundColumn DataField="RM_RFQ_ID" HeaderText="RM_RFQ_ID" SortExpression="RM_RFQ_ID" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Creation Date" HeaderText="Creation Date" SortExpression="Creation Date">
                        <HeaderStyle HorizontalAlign="Left" Width="13%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Expiry Date" HeaderText="Expiry Date" SortExpression="Expiry Date">
                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                     <asp:BoundColumn DataField="RFQ Name" HeaderText="RFQ Description" SortExpression="RFQ Name">
                        <HeaderStyle HorizontalAlign="Left" Width="35%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                   <asp:BoundColumn DataField="Buyer Company" HeaderText="Purchaser Company" SortExpression="Buyer Company">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
        </div>
        </div>
        <div id="divOutstandingInvoiceVend" style="display:none;" runat="server">
        <div class="db_wrapper">
            <div class="db_tbl_hd">
                <asp:Label ID="OutstdInv" runat="server"></asp:Label>
        </div>
            <asp:DataGrid ID="dtgOutstandingInvoiceVend" runat="server" OnSortCommand="SortCommandOutstandingInvoiceVend_Click">
                <Columns>
                    <asp:TemplateColumn SortExpression="PO Number" HeaderText="PO Number">
						<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:HyperLink Runat="server" ID="lnkInv"></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
                    <asp:BoundColumn DataField="POM_PO_INDEX" HeaderText="POM_PO_INDEX" SortExpression="POM_PO_INDEX" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="DO Number" HeaderText="DO Number" SortExpression="DO Number">
                        <HeaderStyle HorizontalAlign="Left" Width="13%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                     <asp:BoundColumn DataField="GRN Number" HeaderText="GRN Number" SortExpression="GRN Number">
                        <HeaderStyle HorizontalAlign="Left" Width="13%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                   <asp:BoundColumn DataField="CM_COY_NAME" HeaderText="Purchaser Company" SortExpression="CM_COY_NAME">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="POM_CURRENCY_CODE" HeaderText="Currency" SortExpression="POM_CURRENCY_CODE">
                        <HeaderStyle HorizontalAlign="Left" Width="10%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Amount" HeaderText="Amount" SortExpression="Amount">
                        <HeaderStyle HorizontalAlign="Right" Width="15%" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>                    
                </Columns>
            </asp:DataGrid>
        </div>
        </div>
        <div id="divOutstandingGRNforQCVerify" style="display:none;" runat="server">
        <div class="db_wrapper">
            <div class="db_tbl_hd">
                <asp:Label ID="OutstdGRNQCVerify" runat="server"></asp:Label>
        </div>
            <asp:DataGrid ID="dtgOutstdGRNQCVerify" runat="server" OnSortCommand="SortCommandOutstandingGRNQCVerify_Click">
                <Columns>
				    <asp:TemplateColumn SortExpression="IV_GRN_NO" HeaderText="GRN Number">
					    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
					    <ItemStyle HorizontalAlign="Left"></ItemStyle>
					    <ItemTemplate>
						    <asp:HyperLink Runat="server" ID="lnkGRNNo"></asp:HyperLink>
					    </ItemTemplate>
				    </asp:TemplateColumn>
                   <asp:BoundColumn HeaderText="Vendor" DataField="CM_COY_NAME" SortExpression="CM_COY_NAME">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn HeaderText="PO Number" DataField="POM_PO_NO" SortExpression="POM_PO_NO">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn HeaderText="DO Number" DataField="DOM_DO_NO" SortExpression="DOM_DO_NO">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>                    
                    <asp:BoundColumn HeaderText="GRN Date" DataField="GM_CREATED_DATE" SortExpression="GM_CREATED_DATE">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn HeaderText="GRN Received Date" DataField="GM_DATE_RECEIVED" SortExpression="GM_DATE_RECEIVED">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
        </div>
        </div>
        
        <%--</div> --%>     
    </form>
</body>
</html>
