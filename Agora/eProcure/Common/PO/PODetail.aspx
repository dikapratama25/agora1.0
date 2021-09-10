<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PODetail.aspx.vb" Inherits="eProcure.PODetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>PODetail</title>
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
        $('#cmd_accept').click(function() {      
        document.getElementById("cmd_accept").style.display= "none";
        document.getElementById("cmd_reject").style.display= "none";
        });
        
        $('#cmd_ack').click(function() {   
        document.getElementById("cmd_ack").style.display= "none";
        });
        });




function confirmReject()
{	
	result=resetSummary(1,0);	
	ans=confirm("Are you sure that you want to reject this PO ?");
	//alert(ans);
	//return false;	cmd_accept
	if (ans){
	    document.getElementById("cmd_reject").style.display= "none";	
	    document.getElementById("cmd_accept").style.display= "none";		
		return result;
		}
	else
	{
	    document.getElementById("cmd_reject").style.display= "";
	    document.getElementById("cmd_accept").style.display= "";
		return false;
		}
}

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
 
 		    function showHide1(lnkdesc)
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
		    function showHide2(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';
	                    document.getElementById("Image3").src = '<% Response.Write(sCollapseUp)%>';
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
	                    document.getElementById("Image3").src = '<% Response.Write(sCollapseDown)%>';
                    }
            }
		    function showHide3(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';
	                    document.getElementById("Image4").src = '<% Response.Write(sCollapseUp)%>';
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
	                    document.getElementById("Image4").src = '<% Response.Write(sCollapseDown)%>';
                    }
            }
            
            function showHide4(lnkdesc)
            {

                 if (document.getElementById(lnkdesc).style.display == 'none')
                    {
	                    document.getElementById(lnkdesc).style.display = '';
	                    document.getElementById("Image5").src = '<% Response.Write(sCollapseUp)%>';
                    } 
                 else 
                    {
	                    document.getElementById(lnkdesc).style.display = 'none';
	                    document.getElementById("Image5").src = '<% Response.Write(sCollapseDown)%>';
                    }
            }

function PopWindow(myLoc)
{
	window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
	return false;
}
//-->
		</script>
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_PODetail_tabs"))%>
			<table class="alltable" id="Table10" cellspacing="0" cellpadding="0" border="0">
             <tr>
					<td class="linespacing1" colspan="4"></td>
			</tr>
			<tr>
				<td >
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Click the Accept PO button to accept the selected PO. To reject the selected PO,click the Reject PO button." Visible="False"
					></asp:label>

				</td>
			</tr>
            <tr>
					<td class="linespacing2" colspan="4"></td>
			</tr>
			</table>
			<table class="alltable" id="Table1" cellspacing="0" cellpadding="0" border="0" runat="server">
				<tr>
					<td class="tableheader" align="left" colspan="5">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label><asp:label id="lblHeader1" text="Purchase Order Header" runat="server"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tablecol" align="left" width="18%"><strong>&nbsp;PO 
							Number</strong> :</td>
					<td class="Tableinput" valign="top" width="30%">&nbsp;<asp:label id="lblPoNo" runat="server"></asp:label></td>
					<td class="tablecol" width="4%"></td>
					<td class="tablecol" width="18%"><strong>&nbsp;Status</strong> 
						:</td>
					<td class="Tableinput" valign="top" width="30%">&nbsp;<asp:label id="lblStatus" runat="server"></asp:label></td>
				</tr>
				<tr id="trdiv" runat="server" style="DISPLAY: none">
					<td class="tablecol" valign="top" width="18%" ><strong>&nbsp;CR Number</strong> :</td>
					<td class="Tableinput" colspan="4" valign="top">&nbsp;<asp:label id="lblCRNum" runat="server"></asp:label></td>
				</tr>
				<tr id="trDivPRNo" runat="server" style="DISPLAY: none">
					<td class="tablecol" valign="top" width="18%" ><strong>&nbsp;PR Number</strong> :</td>
					<td class="Tableinput" colspan="4" valign="top">&nbsp;<asp:label id="lblPRNo" runat="server"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tablecol" width="18%" style="height: 19px"><strong>&nbsp;Order Date</strong> :</td>
					<td class="Tableinput" valign="top" width="30%" style="height: 19px">&nbsp;<asp:label id="lblOrderDate" runat="server"></asp:label></td>
					<td class="tablecol" width="4%" style="height: 19px"></td>
					<td class="tablecol" valign="top" width="18%" style="height: 19px"><strong>&nbsp;Currency Code</strong> :</td>
					<td class="tableinput" valign="top" width="30%" style="height: 19px">&nbsp;<asp:label id="lblCurrCode" runat="server"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tablecol" width="18%"><strong>&nbsp;PO Accepted Date</strong> :</td>
					<td class="Tableinput" valign="top" width="30%">&nbsp;<asp:label id="lblAcceptedDate" runat="server"></asp:label></td>
					<td class="tablecol" width="4%"></td>
					<td class="tablecol" width="18%"><strong>&nbsp;Delivery Term</strong>&nbsp;:</td>
					<td class="tableinput" width="30%">&nbsp;<asp:label id="lblDelTerm" runat="server"></asp:label></td>
				</tr>
				<tr valign="top" id="tr_SEH1" style="display:none" runat="server">
					<td class="tablecol" valign="top"><strong>&nbsp;Vendor Code</strong> :</td>
					<td class="tableinput" valign="top">&nbsp;<asp:label id="lblVendorCode" runat="server"></asp:label></td>
					<td class="tablecol"></td>
					<td class="tablecol"></td>
					<td class="tablecol" valign="top"></td>
				</tr>
				<tr valign="top">
					<td class="tablecol" valign="top"><strong>&nbsp;Vendor</strong> :</td>
					<td class="tableinput" valign="top">&nbsp;<asp:label id="lblVendor" runat="server"></asp:label></td>
					<td class="tablecol"></td>
					<td class="tablecol"><strong>&nbsp;Ship Via</strong>&nbsp;:</td>
					<td class="tableinput" valign="top">&nbsp;<asp:label id="lblShipVia" runat="server"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tablecol" valign="top"><strong>&nbsp;Vendor Address</strong> :</td>
					<td class="tableinput" valign="top">&nbsp;<asp:label id="lblVendorAddr" runat="server"></asp:label></td>
					<td class="tablecol"></td>
					<td class="tablecol" valign="top"><strong>&nbsp;Ship To</strong> :</td>
					<td class="tableinput" valign="top">&nbsp;<asp:label id="lblShipTo" runat="server"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tablecol" style="height: 19px"><strong>&nbsp;Vendor Tel</strong> :</td>
					<td class="tableinput" valign="top" style="height: 19px">&nbsp;<asp:label id="lblVendorTel" runat="server"></asp:label></td>
					<td class="tablecol" style="height: 19px"></td>
					<td class="tablecol" style="height: 19px"><strong>&nbsp;Tel</strong> :</td>
					<td class="tableinput" valign="top" style="height: 19px">&nbsp;<asp:label id="lblTel" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td class="tablecol" valign="top"><strong>&nbsp;Vendor Fax</strong> :</td>
					<td class="tableinput" valign="top">&nbsp;<asp:label id="lblVendorFax" runat="server"></asp:label></td>
					<td class="tablecol"></td>
					<td class="tablecol" valign="top"><strong>&nbsp;Fax</strong> :</td>
					<td class="tableinput" valign="top">&nbsp;<asp:label id="lblFax" runat="server"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tablecol"><strong>&nbsp;Vendor&nbsp;Email</strong> :</td>
					<td class="tableinput" style="WIDTH: 462px" valign="top" width="462">&nbsp;<asp:label id="lblVendorEmail" runat="server"></asp:label></td>
					<td class="tablecol"></td>
					<td class="tablecol"><strong>&nbsp;Contact</strong> :</td>
					<td class="tableinput" valign="top">&nbsp;<asp:label id="lblContact" runat="server"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tableinput">&nbsp;</td>
					<td class="tableinput" style="WIDTH: 462px" valign="top" width="462">&nbsp;</td>
					<td class="tablecol"></td>
					<td class="tablecol"><strong>&nbsp;Email</strong> :</td>
					<td class="tableinput" valign="top" width="25%">&nbsp;<asp:label id="lblEmail" runat="server"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tablecol" style="HEIGHT: 23px"><strong>&nbsp;Payment Terms</strong> :</td>
					<td class="tableinput" valign="top" style="HEIGHT: 23px">&nbsp;<asp:label id="lblPaymentTerm" runat="server"></asp:label></td>
					<td class="tablecol"></td>
					<td class="tablecol" style="HEIGHT: 23px"><strong>&nbsp;Payment Method</strong> :</td>
					<td class="tableinput" valign="top" style="HEIGHT: 23px">&nbsp;<asp:label id="lblPaymentMethod" runat="server"></asp:label></td>
				</tr>
				<tr valign="top">
					<td class="tablecol"><strong>&nbsp;Shipment Terms</strong> :</td>
					<td class="tableinput" valign="top">&nbsp;<asp:label id="lblshipTerm" runat="server"></asp:label></td>
					<td class="tablecol"></td>
					<td class="tablecol"><strong>&nbsp;Shipment Mode</strong> :</td>
					<td class="tableinput" valign="top">&nbsp;<asp:label id="lblShipMethod" runat="server"></asp:label></td>
				</tr>
				
				<tr valign="top">
					<td id="hidInt1" class="tablecol" valign="top" >&nbsp;<strong>Internal Remarks</strong>&nbsp;:</td>
					<td id="hidInt2" class="Tableinput" >&nbsp;<asp:label id="lblInternalRemark"  Runat="server">
					</asp:label></td>
					<td id="hidInt3" class="tablecol"></td>
					<td class="tablecol" valign="top" >&nbsp;<strong><asp:label id="lblExtRemark" text="External Remarks" runat="server"></asp:label></strong>&nbsp;:</td>
					<td class="Tableinput" colspan="6">&nbsp;<asp:label id="lblExternalRemark" Runat="server">
					</asp:label></td>

				</tr>
				
				<tr valign="top" id="hidIntAttach">
					<td class="tablecol"><strong>&nbsp;<asp:label id="lblAttachInt" text="Internal File(s) Attached" runat="server"></asp:label></strong> :</td>
					<td class="tableinput" valign="top" colspan="3">&nbsp;<asp:label id="lblFileAttacInt" runat="server"></asp:label></td>
					<td class="tableinput"></td>
				</tr>
				
				<tr valign="top">
					<td class="tablecol"><strong>&nbsp;<asp:label id="lblExtAttach" text="External File(s) Attached" runat="server"></asp:label></strong> :</td>
					<td class="tableinput" valign="top" colspan="3">&nbsp;<asp:label id="lblFileAttac" runat="server"></asp:label></td>
					<td class="tableinput"></td>
				</tr>
				<tr valign="top">
					<td class="tablecol"><strong>&nbsp;Vendor's Remarks</strong> :</td>
					<td class="tableinput" colspan="4">&nbsp;<asp:textbox id="txtRemark" Runat="server" CssClass="listtxtbox" Rows="3" MaxLength="1000" TextMode="MultiLine"
							Width="100%" Enabled="False"></asp:textbox></td>
							</tr>
						<tr>	
					<td colspan="5" >
						<div id="term_con" runat="server"><A id="link_term" href="#" runat="server">Click Here</A>
							to download the Term &amp; Conditions document</div>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4">&nbsp;&nbsp;
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><input id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"/><input id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"/></td>
				</tr>
			</table>
			<table id="TABLE11" class="tableheader" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
				<tr id="hidApprFlow" style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide1('ApprFlow')">
						<td valign="top" class="tableheader" >	
					    Approval Flow<asp:Image ID="Image2" runat="server" ImageUrl="#" /></td>
					    </tr>
				</table>
				        <div id="ApprFlow" style="display:inline"  >
				        <table class="alltable" id="Table12" border="0" width="100%" cellspacing="0" cellpadding="0" >
				<tr>
					<td width="100%"><asp:datagrid id="dtg_apprflow" runat="server" CssClass="Grid" AutoGenerateColumns="False" Width="100%">
							<Columns>
								<asp:BoundColumn DataField="PRA_SEQ" HeaderText="Level">
									<HeaderStyle Width="2%" CssClass="GridHeader"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AO_NAME" HeaderText="Approving Officer">
									<HeaderStyle Width="20%" CssClass="GridHeader"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AAO_NAME" HeaderText="A.Approving Officer">
									<HeaderStyle Width="20%" CssClass="GridHeader"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRA_APPROVAL_TYPE" HeaderText="Approval Type">
									<HeaderStyle Width="10%" CssClass="GridHeader"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRA_ACTION_DATE" HeaderText="Action Date">
									<HeaderStyle Width="10%" CssClass="GridHeader"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PRA_AO_REMARK" HeaderText="Remarks">
									<HeaderStyle Width="20%" CssClass="GridHeader"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Attachment">
									<HeaderStyle Width="20%" CssClass="GridHeader"></HeaderStyle>
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
			</table>
            </div>
            
			<div id="div1" runat="server" style="width:130%; cursor:pointer;" class="tableheader" onclick="showHide('POLine')">
			    <asp:label id="Label30" runat="server">Purchase Order Line Detail</asp:label>
                <asp:Image ID="Image1" runat="server" ImageUrl="#" />
            </div>
		    <div id="POLine" style="display:inline; width:130%;">
			    <table class="alltable" id="Table5" border="0" width="130%" cellspacing="0">
				<tr>
					<td><asp:datagrid id="dtg_POList" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_POList_Page"
							AutoGenerateColumns="False" CssClass="Grid">
							<HeaderStyle HorizontalAlign="Left" CssClass="GridHeader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn HeaderText="Line" SortExpression="POD_PO_Line">
									<HeaderStyle HorizontalAlign="right" Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>

                                <%--Jules 2018.05.07 - PAMB Scrum 2--%>
                                <asp:BoundColumn DataField="GIFT" SortExpression="GIFT" HeaderText="Gift">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="FUNDTYPE" SortExpression="FUNDTYPE" HeaderText="Fund Type (L1)">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="PERSONCODE" SortExpression="PERSONCODE" HeaderText="Person Code (L9)">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="PROJECTCODE" SortExpression="PROJECTCODE" HeaderText="Project / ACR (L8) Code">
									<HeaderStyle Width="5%"></HeaderStyle>
								</asp:BoundColumn>
                                <%--End modification--%>

								<asp:BoundColumn DataField="POD_VENDOR_ITEM_CODE" SortExpression="POD_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>                                
								<asp:BoundColumn DataField="GL_CODE" SortExpression="GL_CODE" HeaderText="GL Description (GL Code)">
									<HeaderStyle Width="12%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_B_CATEGORY_CODE" SortExpression="POD_B_CATEGORY_CODE" HeaderText="Category Code">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ASSET_CODE" SortExpression="ASSET_CODE" HeaderText="Asset Code">
									<HeaderStyle Width="10%"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PRODUCT_DESC" SortExpression="POD_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UNIT_COST" SortExpression="POD_UNIT_COST" HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GST_RATE" SortExpression="GST_RATE" HeaderText="SST Rate">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_TAX_VALUE" SortExpression="POD_TAX_VALUE" HeaderText="SST Amount">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_GST_INPUT_TAX_CODE" SortExpression="POD_GST_INPUT_TAX_CODE" HeaderText="SST Tax Code (Purchase) (L6)">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" SortExpression="POD_ETD" HeaderText="EDD" ></asp:BoundColumn>
								<asp:BoundColumn DataField="POD_MIN_PACK_QTY" SortExpression="POD_MIN_PACK_QTY" HeaderText="MPQ">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_WARRANTY_TERMS" SortExpression="POD_WARRANTY_TERMS" HeaderText="Warranty Terms">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" SortExpression="POD_ORDERED_QTY" HeaderText="PO Qty">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="POD_PR_LINE" HeaderText="Outstd Qty">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_RECEIVED_QTY" SortExpression="POD_RECEIVED_QTY" HeaderText="GRN Qty">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_REJECTED_QTY" SortExpression="POD_REJECTED_QTY" HeaderText="Rejected Qty">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Cost Centre Code (L7)">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Delivery Address">
									<HeaderStyle Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_REMARK" SortExpression="POD_REMARK" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="15%" CssClass="txtbox"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid>
						</td>
				</tr>
				</table>
	      </div> 
	      
	    <div id="div2" runat="server" style="width:150%; cursor:pointer;" class="tableheader" onclick="showHide4('POLineForStock')">
				<asp:label id="Label1" runat="server">Purchase Order Line Detail</asp:label>
                <asp:Image ID="Image5" runat="server" ImageUrl="#" />
        </div>
		<div id="POLineForStock" style="display:inline; width:150%;" runat="server">
			<table class="alltable" id="Table15" border="0" width="150%" cellspacing="0">
				<tr>
					<td><asp:datagrid id="dtg_POListStock" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_POListStock_Page"
							AutoGenerateColumns="False" CssClass="Grid">
							<HeaderStyle HorizontalAlign="Left" CssClass="GridHeader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn HeaderText="Line" SortExpression="POD_PO_Line">
									<HeaderStyle HorizontalAlign="right" Width="7%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_VENDOR_ITEM_CODE" SortExpression="POD_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								
								<asp:BoundColumn DataField="ASSET_CODE" SortExpression="ASSET_CODE" HeaderText="Asset Code">
									<HeaderStyle Width="15%"></HeaderStyle>
								</asp:BoundColumn>
								
								<asp:BoundColumn DataField="POD_PRODUCT_DESC" SortExpression="POD_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UNIT_COST" SortExpression="POD_UNIT_COST" HeaderText="Unit Price">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GST_RATE" SortExpression="GST_RATE" HeaderText="SST Rate">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_TAX_VALUE" SortExpression="POD_TAX_VALUE" HeaderText="SST Amount">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_GST_INPUT_TAX_CODE" SortExpression="POD_GST_INPUT_TAX_CODE" HeaderText="SST Tax Code (Purchase)">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" SortExpression="POD_ETD" HeaderText="EDD" ></asp:BoundColumn>
								<asp:BoundColumn DataField="POD_MIN_PACK_QTY" SortExpression="POD_MIN_PACK_QTY" HeaderText="MPQ">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_WARRANTY_TERMS" SortExpression="POD_WARRANTY_TERMS" HeaderText="Warranty Terms">
									<HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_ORDERED_QTY" SortExpression="POD_ORDERED_QTY" HeaderText="PO Qty">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="POD_PR_LINE" HeaderText="Outstd Qty">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_RECEIVED_QTY" SortExpression="POD_RECEIVED_QTY" HeaderText="GRN Qty">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_REJECTED_QTY" SortExpression="POD_REJECTED_QTY" HeaderText="Rejected Qty">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Delivery Address">
									<HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PREV1_QTY" SortExpression="POD_PREV1_QTY" HeaderText="Past1stMthQty">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PREV2_QTY" SortExpression="POD_PREV2_QTY" HeaderText="Past2ndMthQty">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PREV3_QTY" SortExpression="POD_PREV3_QTY" HeaderText="Past3rdMthQty">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PREV_AVG" SortExpression="POD_PREV_AVG" HeaderText="Past 3 Mth Ave">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_CURR_QTY" SortExpression="POD_CURR_QTY" HeaderText="Curr Mth Usage">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_NEXT1_QTY" SortExpression="POD_NEXT1_QTY" HeaderText="1 Mth Usage">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_NEXT2_QTY" SortExpression="POD_NEXT2_QTY" HeaderText="2 Mth Usage">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_NEXT3_QTY" SortExpression="POD_NEXT3_QTY" HeaderText="3 Mth Usage">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_STOCK_ON_HAND_QTY" SortExpression="POD_STOCK_ON_HAND_QTY" HeaderText="Stk On Hand">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PO_BALANCE_QTY" SortExpression="POD_PO_BALANCE_QTY" HeaderText="PO Balance">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PO_IN_PROGRESS_QTY" SortExpression="POD_PO_IN_PROGRESS_QTY" HeaderText="PO In Progress">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn HeaderText="Forecast">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PUR_SPEC_NO" SortExpression="POD_PUR_SPEC_NO" HeaderText="Pur Spec No">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_SPEC1" SortExpression="POD_SPEC1" HeaderText="Spec 1">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_SPEC2" SortExpression="POD_SPEC2" HeaderText="Spec 2">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_SPEC3" SortExpression="POD_SPEC3" HeaderText="Spec 3">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_LEAD_TIME" SortExpression="POD_LEAD_TIME" HeaderText="Lead Time (Days)">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_MANUFACTURER" SortExpression="POD_MANUFACTURER" HeaderText="Mfg Name">
									<HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_REMARK" SortExpression="POD_REMARK" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="21%" CssClass="txtbox"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				 </tr>
			</table>
	    </div> 				
              
				<table id="TABLE8" class="tableheader" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
				<tr id="hidDO" style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide2('DOnGRN')">
								<td valign="top" class="tableheader" >	DO And GRN Summary<asp:Image ID="Image3" runat="server" ImageUrl="#" /></td>
							</tr>
				</table>
				<div id="DOnGRN" style="display:inline"  >
				 <table class="alltable" id="Table4" border="0" width="100%" cellspacing="0" cellpadding="0" >
				<tr>
					<td><asp:datagrid id="dtg_doc" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_POList_Page"
							AutoGenerateColumns="False" CssClass="Grid"  AllowSorting="True">
							<HeaderStyle HorizontalAlign="Left" CssClass="GridHeader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn SortExpression="DOM_DO_NO" HeaderText="DO NO.">
									<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="CREATIONDATE" HeaderText="DO Creation Date">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="SUBMITIONDATE" HeaderText="DO Submission Date">
									<HeaderStyle HorizontalAlign="Left" Width="16%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="DO_CREATED_BY" SortExpression="DO_CREATED_BY" HeaderText="DO Created By">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="GM_GRN_NO" HeaderText="GRN No.">
									<HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="GM_CREATED_DATE" HeaderText="GRN Date">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="GM_DATE_RECEIVED" HeaderText="GRN Received Date">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="GM1_CREATED_BY" SortExpression="GM1_CREATED_BY" HeaderText="GRN Created by">
									<HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				            				        </table>
	                    </div>			              
				<table id="TABLE9" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server">
				<tr id="hidCR" style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide3('CR')">
						<td valign="top" class="tableheader" >	Cancellation Request Summary<asp:Image ID="Image4" runat="server" ImageUrl="#" /></td>
					    </tr></table>
					    
				<div id="CR" style="display:inline"  >
				        <table class="alltable" id="Table6" border="0" width="100%" cellspacing="0" cellpadding="0" >
				<tr>
					<td><asp:datagrid id="dtg_cr" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_POList_Page"
							AutoGenerateColumns="False" CssClass="Grid" AllowSorting="True">
								<HeaderStyle HorizontalAlign="Left" CssClass="GridHeader"></HeaderStyle>
						<Columns>
								<asp:BoundColumn SortExpression="PCM_CR_NO" HeaderText="CR NO.">
									<HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="PCM_REQ_DATE" HeaderText="CR Creation Date">
									<HeaderStyle HorizontalAlign="Left" Width="16%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="PCM_REQ_BY" SortExpression="PCM_REQ_BY" HeaderText="CR Create By">
									<HeaderStyle HorizontalAlign="Left" Width="54%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="PCM_CR_NO">
									<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
			</table>
</div>
				<table class="alltable" id="Table7" border="0" width="100%" cellspacing="0" cellpadding="0" >
				<tr>
					<td class="emptycol" colspan="4">
						<table class="alltable" id="Table2" cellspacing="0" cellpadding="0" border="0">
							<tr>
								<td align="left">
								    <asp:button id="cmd_dupPO" runat="server" CssClass="button" style="display: none;" Text="Duplicate PO"
										Height="19px" width="75px"></asp:button>
								    <asp:button id="cmd_accept" runat="server" CssClass="button" Visible="False" Text="Accept PO"
										Height="19px" width="75px"></asp:button>
									<asp:button id="cmd_reject" runat="server" CssClass="button" Visible="False" Text="Reject PO"
										Height="19px" width="75px"></asp:button>
									<input class="button" id="cmd_preview" type="button" value="View PO" name="cmd_preview" style="WIDTH: 75px" runat="server"/>
									<asp:button id="cmd_ack" runat="server" CssClass="button" Visible="False" Text="Acknowledge"
										Height="19px" width="75px"></asp:button>
									<input class="button" id="cmd_cr" style="VISIBILITY: hidden; WIDTH: 75px" type="button" value="View CR" 
										name="cmd_cr" runat="server"/>
									</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="emptycol" colspan="4"></td>
				</tr>				
				<tr>
					<td class="emptycol" ><a id="back" href="#" runat="server"><strong>&lt; Back</strong></a>
					<asp:Button ID="cmd_back" runat="server" Text="Close" cssclass="button" Visible="false" /></td>
				</tr>
			</table>

		</form>
	</body>
</html>
