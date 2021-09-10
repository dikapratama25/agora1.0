<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PODetail.aspx.vb" Inherits="eProcure.PODetailVen" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PODetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script runat="server">
		    Dim dDispatcher As New AgoraLegacy.dispatcher
		    Dim sCollapseUp As String = dDispatcher.direct("Plugins/Images", "collapse_up.gif")
		    Dim sCollapseDown As String = dDispatcher.direct("Plugins/Images", "collapse_down.gif")
        </script>
        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>        
        
		<% Response.Write(Session("WheelScript"))%>
		
		<script language="javascript">
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

function PopWindow(myLoc)
{
	window.open(myLoc,"eProcure","width=900,height=600,location=no,toolbar=yes,menubar=no,scrollbars=yes,resizable=yes");
	return false;
}
//-->
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
        <%  Response.Write(Session("w_PODetail_tabs"))%>
			<TABLE class="alltable" id="Table10" cellSpacing="0" cellPadding="0" border="0">
             <tr>
					<TD class="linespacing1" colSpan="4"></TD>
			</TR>
			<TR>
				<TD >
					<asp:label id="lblAction" runat="server"  CssClass="lblInfo"
					    Text="Click the Accept PO button to accept the selected PO. To reject the selected PO,click the Reject PO button." Visible="False"
					></asp:label>

				</TD>
			</TR>
            <tr>
					<TD class="linespacing2" colSpan="4"></TD>
			</TR>
			</Table>
			<TABLE class="alltable" id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
				<TR>
					<TD class="tableheader" align="left" colSpan="5">&nbsp;<asp:label id="lblHeader" runat="server"></asp:label><asp:label id="lblHeader1" text="Purchase Order Header" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol" align="left" width="18%"><STRONG>&nbsp;PO 
							Number</STRONG> :</TD>
					<TD class="TableInput" vAlign="top" width="30%">&nbsp;<asp:label id="lblPoNo" runat="server"></asp:label></TD>
					<td class="tablecol" width="4%"></td>
					<TD class="tablecol" width="18%"><STRONG>&nbsp;Status</STRONG> 
						:</TD>
					<TD class="TableInput" vAlign="top" width="30%">&nbsp;<asp:label id="lblStatus" runat="server"></asp:label></TD>
				</TR>
						</TABLE>
				<DIV id="hiddiv" style="DISPLAY: none" runat="server">
						<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
							<TR>
								<TD class="tablecol" vAlign="top" width="18%" ><STRONG>&nbsp;CR Number :</STRONG></TD>
								<TD class="TableInput" colspan="4" vAlign="top">&nbsp;<asp:label id="lblCRNum" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
				</div>
			<TABLE class="alltable" id="Table13" cellSpacing="0" cellPadding="0" border="0" runat="server">
				<TR vAlign="top">
					<TD class="tablecol" width="18%"><STRONG>&nbsp;Order Date</STRONG> :</TD>
					<TD class="TableInput" vAlign="top" width="30%">&nbsp;<asp:label id="lblOrderDate" runat="server"></asp:label></TD>
					<td class="tablecol" width="4%"></td>
					<TD class="tablecol" vAlign="top" width="18%"><STRONG>&nbsp;Currency Code</STRONG> :</TD>
					<TD class="tableinput" vAlign="top" width="30%">&nbsp;<asp:label id="lblCurrCode" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol" vAlign="top"><STRONG>&nbsp;Vendor</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblVendor" runat="server"></asp:label></TD>
					<td class="tablecol"></td>
					<TD class="tablecol"><STRONG>&nbsp;Ship Via</STRONG>&nbsp;:</TD>
					<TD class="tableinput" vAlign="top">&nbsp;
						<asp:label id="lblShipVia" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol" vAlign="top"><STRONG>&nbsp;Vendor Address</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblVendorAddr" runat="server"></asp:label></TD>
					<td class="tablecol"></td>
					<TD class="tablecol" vAlign="top"><STRONG>&nbsp;Ship To</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblShipTo" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;Vendor Tel</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblVendorTel" runat="server"></asp:label></TD>
					<td class="tablecol"></td>
					<TD class="tablecol"><STRONG>&nbsp;Tel</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblTel" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD class="tablecol" vAlign="top"><STRONG>&nbsp;Vendor Fax</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblVendorFax" runat="server"></asp:label></TD>
					<td class="tablecol"></td>
					<TD class="tablecol" vAlign="top"><STRONG>&nbsp;Fax</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblFax" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;Vendor&nbsp;Email</STRONG> :</TD>
					<TD class="tableinput" style="WIDTH: 462px" vAlign="top" width="462">&nbsp;<asp:label id="lblVendorEmail" runat="server"></asp:label></TD>
					<td class="tablecol"></td>
					<TD class="tablecol"><STRONG>&nbsp;Contact</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblContact" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tableinput">&nbsp;</TD>
					<TD class="tableinput" style="WIDTH: 462px" vAlign="top" width="462">&nbsp;</TD>
					<td class="tablecol"></td>
					<TD class="tablecol"><STRONG>&nbsp;Email</STRONG> :</TD>
					<TD class="tableinput" vAlign="top" width="25%">&nbsp;<asp:label id="lblEmail" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol" style="HEIGHT: 23px"><STRONG>&nbsp;Payment Terms</STRONG> :</TD>
					<TD class="tableinput" vAlign="top" style="HEIGHT: 23px">&nbsp;<asp:label id="lblPaymentTerm" runat="server"></asp:label></TD>
					<td class="tablecol"></td>
					<TD class="tablecol" style="HEIGHT: 23px"><STRONG>&nbsp;Payment Method</STRONG> :</TD>
					<TD class="tableinput" vAlign="top" style="HEIGHT: 23px">&nbsp;<asp:label id="lblPaymentMethod" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;Shipment Terms</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblshipTerm" runat="server"></asp:label></TD>
					<td class="tablecol"></td>
					<TD class="tablecol"><STRONG>&nbsp;Shipment Mode</STRONG> :</TD>
					<TD class="tableinput" vAlign="top">&nbsp;<asp:label id="lblShipMethod" runat="server"></asp:label></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;File(s) attached</STRONG> :</TD>
					<TD class="tableinput" vAlign="top" colSpan="3">&nbsp;<asp:label id="lblFileAttac" runat="server"></asp:label></TD>
					<TD class="tableinput"></TD>
				</TR>
				<TR vAlign="top">
					<TD class="tablecol"><STRONG>&nbsp;Vendor's Remarks</STRONG> :</TD>
					<TD class="tableinput" colSpan="4">&nbsp;<asp:textbox id="txtRemark" Runat="server" CssClass="listtxtbox" Rows="3" MaxLength="1000" TextMode="MultiLine"
							Width="100%" Enabled="False"></asp:textbox></TD>
							</tr>
						<tr>	
					<TD colspan="5" >
						<div id="term_con" runat="server"><A id="link_term" href="#" runat="server">Click Here</A>
							to download the Term &amp; Conditions document</div>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4">&nbsp;&nbsp;
						<asp:validationsummary id="vldSumm" runat="server" CssClass="errormsg" DisplayMode="BulletList"></asp:validationsummary><INPUT id="hidSummary" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidSummary"
							runat="server"><INPUT id="hidControl" style="WIDTH: 32px; HEIGHT: 22px" type="hidden" size="1" name="hidControl"
							runat="server"></TD>
				</TR>
			</TABLE>
			<div style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide('POLine')">
					    <asp:label id="Label30" runat="server">Purchase Order Line Detail</asp:label>
                <asp:Image ID="Image1" runat="server" ImageUrl="#" /></div>
					    <div id="POLine" style="display:inline"  >
				        <table class="alltable" id="Table5" border="0" width="100%" cellSpacing="0"  >
				<TR>
					<TD><asp:datagrid id="dtg_POList" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_POList_Page"
							AutoGenerateColumns="False" CssClass="Grid">
							<HeaderStyle HorizontalAlign="Left" CssClass="GridHeader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn HeaderText="PO Line" SortExpression="POD_PO_Line">
									<HeaderStyle HorizontalAlign="right" Width="7%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_VENDOR_ITEM_CODE" SortExpression="POD_VENDOR_ITEM_CODE" HeaderText="Item Code">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_PRODUCT_DESC" SortExpression="POD_PRODUCT_DESC" HeaderText="Item Name">
									<HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_UOM" SortExpression="POD_UOM" HeaderText="UOM">
									<HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
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
								<asp:BoundColumn DataField="POD_ORDERED_QTY" SortExpression="POD_ORDERED_QTY" HeaderText="Ordered">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn SortExpression="POD_PR_LINE" HeaderText="Outstd">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_RECEIVED_QTY" SortExpression="POD_RECEIVED_QTY" HeaderText="Receive Qty">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_REJECTED_QTY" SortExpression="POD_REJECTED_QTY" HeaderText="Reject Qty">
									<HeaderStyle HorizontalAlign="right" Width="8%"></HeaderStyle>
									<ItemStyle HorizontalAlign="right"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="POD_REMARK" SortExpression="POD_REMARK" HeaderText="Remarks">
									<HeaderStyle HorizontalAlign="Left" Width="21%" CssClass="txtbox"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				            				        </table>
	                    </div>	

				<TABLE id="TABLE11" class="tableheader" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
				<TR id="hidApprFlow" style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide1('ApprFlow')">
						<TD vAlign="top" class="tableheader" >	
					    Approval Flow<asp:Image ID="Image2" runat="server" ImageUrl="#" /></td>
					    </tr>
				</table>
	      				<div id="ApprFlow" style="display:inline"  >
				        <table class="alltable" id="Table12" border="0" width="100%" cellSpacing="0" cellPadding="0" >
				<TR>
					<TD width="100%"><asp:datagrid id="dtg_apprflow" runat="server" CssClass="Grid" AutoGenerateColumns="False" Width="100%">
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
										<asp:DataList id=DataList1 runat="server" DataSource='<%# CreateFileLinks( DataBinder.Eval( Container.DataItem, "PRA_AO") , DataBinder.Eval( Container.DataItem, "PRA_A_AO") , DataBinder.Eval( Container.DataItem, "PRA_SEQ" ) ) %>' ShowFooter="False" Width="100%" BorderColor=#0000ff ShowHeader="False">
											<ItemTemplate>
												<%# DataBinder.Eval( Container.DataItem ,"Hyperlink") %>
											</ItemTemplate>
										</asp:DataList>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
</div>
              
				<TABLE id="TABLE8" class="tableheader" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
				<TR id="hidDO" style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide2('DOnGRN')">
								<TD vAlign="top" class="tableheader" >	DO And GRN Summary<asp:Image ID="Image3" runat="server" ImageUrl="#" /></TD>
							</TR>
				</TABLE>
				<div id="DOnGRN" style="display:inline"  >
				 <table class="alltable" id="Table4" border="0" width="100%" cellSpacing="0" cellPadding="0" >
				<TR>
					<TD><asp:datagrid id="dtg_doc" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_POList_Page"
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
						</asp:datagrid></TD>
				</TR>
				            				        </table>
	                    </div>			              
				<TABLE id="TABLE9" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
				<TR id="hidCR" style="width:100%; cursor:pointer;" class="tableheader" onclick="showHide3('CR')">
						<TD vAlign="top" class="tableheader" >	Cancellation Request Summary<asp:Image ID="Image4" runat="server" ImageUrl="#" /></td>
					    </tr></table>
					    
				<div id="CR" style="display:inline"  >
				        <table class="alltable" id="Table6" border="0" width="100%" cellSpacing="0" cellPadding="0" >
				<TR>
					<TD><asp:datagrid id="dtg_cr" runat="server" OnSortCommand="SortCommand_Click" OnPageIndexChanged="dtg_POList_Page"
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
						</asp:datagrid></TD>
				</TR>
			</TABLE>
</div>
				<table class="alltable" id="Table7" border="0" width="100%" cellSpacing="0" cellPadding="0" >
				<TR>
					<TD class="emptycol" colSpan="4">
						<TABLE class="alltable" id="Table2" cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD align="left"><asp:button id="cmd_accept" runat="server" CssClass="button" Visible="False" Text="Accept PO"
										Height="19px" width="75px"></asp:button>
									<asp:button id="cmd_reject" runat="server" CssClass="button" Visible="False" Text="Reject PO"
										Height="19px" width="75px"></asp:button>
									<INPUT class="button" id="cmd_preview" type="button" value="View PO" name="cmd_preview" style="WIDTH: 75px" runat="server">
									<asp:button id="cmd_ack" runat="server" CssClass="button" Visible="False" Text="Acknowledge"
										Height="19px" width="75px"></asp:button>
									<INPUT class="button" id="cmd_cr" style="VISIBILITY: hidden; WIDTH: 75px" type="button" value="View CR" 
										name="cmd_cr" runat="server"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="emptycol" colSpan="4"></TD>
				</TR>				
				<TR>
					<TD class="emptycol" ><A id="back" href="#" runat="server"><strong>&lt; Back</strong></A>
					<asp:Button ID="cmd_back" runat="server" Text="Close" cssclass="button" Visible="false" /></TD>
				</TR>
			</TABLE>

		</form>
	</body>
</HTML>
